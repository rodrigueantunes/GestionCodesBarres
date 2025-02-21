using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using ZXing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using PdfDocument = iTextSharp.text.Document;
using PdfImage = iTextSharp.text.Image;

namespace GestionCodesBarres
{
    public partial class MainWindow : Window
    {
        private List<CodeDescription> codesList = new List<CodeDescription>();

        public MainWindow()
        {
            InitializeComponent();
            CodesListBox.ItemsSource = codesList;
        }

        public class CodeDescription
        {
            public string Code { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(description))
            {
                codesList.Add(new CodeDescription { Code = code, Description = description });
                CodesListBox.Items.Refresh(); // Met à jour l'affichage
                CodeTextBox.Clear();
                DescriptionTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Fichiers CSV (*.csv)|*.csv",
                Title = "Sélectionner un fichier CSV"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        var importedCodes = csv.GetRecords<CodeDescription>().ToList();
                        codesList.AddRange(importedCodes);
                        CodesListBox.Items.Refresh(); // Met à jour l'affichage
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'importation : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GeneratePdfButton_Click(object sender, RoutedEventArgs e)
        {
            if (codesList.Count == 0)
            {
                MessageBox.Show("Aucun code à générer dans le PDF.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Fichiers PDF (*.pdf)|*.pdf",
                Title = "Enregistrer le fichier PDF"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string outputPdfPath = saveFileDialog.FileName;
                GeneratePdfWithDatamatrix(codesList, outputPdfPath);
                MessageBox.Show("PDF généré avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GeneratePdfWithDatamatrix(List<CodeDescription> codes, string outputPdfPath)
        {
            try
            {
                PdfDocument doc = new PdfDocument(PageSize.A4);
                PdfWriter.GetInstance(doc, new FileStream(outputPdfPath, FileMode.Create));
                doc.Open();

                // Ajouter les logos (mettre les chemins corrects)
                string logoPathLeft = "chemin_vers_logo_gauche.png";
                string logoPathRight = "chemin_vers_logo_droit.png";

                if (File.Exists(logoPathLeft) && File.Exists(logoPathRight))
                {
                    PdfImage logoLeft = PdfImage.GetInstance(File.ReadAllBytes(logoPathLeft));
                    PdfImage logoRight = PdfImage.GetInstance(File.ReadAllBytes(logoPathRight));

                    logoLeft.ScaleToFit(100f, 50f);
                    logoRight.ScaleToFit(100f, 50f);

                    logoLeft.SetAbsolutePosition(50f, doc.PageSize.Height - 80f);
                    logoRight.SetAbsolutePosition(doc.PageSize.Width - 150f, doc.PageSize.Height - 80f);

                    doc.Add(logoLeft);
                    doc.Add(logoRight);
                }

                // Table des codes-barres
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 3f, 2f });

                foreach (var item in codes)
                {
                    table.AddCell(item.Code);
                    table.AddCell(item.Description);

                    var barcodeWriter = new BarcodeWriterPixelData
                    {
                        Format = BarcodeFormat.DATA_MATRIX
                    };

                    var pixelData = barcodeWriter.Write(item.Code);
                    using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppArgb))
                    {
                        var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                            System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                        try
                        {
                            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }

                        using (MemoryStream stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Png);
                            PdfImage datamatrix = PdfImage.GetInstance(stream.ToArray());
                            datamatrix.ScaleToFit(50f, 50f);
                            table.AddCell(datamatrix);
                        }




                    doc.Add(table);
                        doc.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la génération du PDF : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
