
# Gestion des Codes-Barres - Application WPF

Bienvenue dans le projet **Gestion des Codes-Barres** ! Cette application WPF (Windows Presentation Foundation) permet de gérer efficacement des codes-barres et leurs descriptions, tout en offrant des fonctionnalités avancées telles que la génération de PDF, l'importation de fichiers CSV et la gestion de logos personnalisés. Idéale pour les professionnels et les entreprises qui ont besoin de manipuler et documenter des codes-barres de manière fluide et pratique.

## Fonctionnalités

- **Gestion des Codes-Barres** : Ajoutez, modifiez et réorganisez facilement vos codes-barres et leurs descriptions.
- **Importation CSV** : Importez un fichier CSV pour charger des codes-barres en masse. Le fichier doit contenir des colonnes pour le code-barres et sa description.
- **Génération de PDF** : Créez des fichiers PDF à partir de la liste de codes-barres avec des options pour ajouter des logos personnalisés (Logo Client, Logo Société).
- **Réorganisation des Codes** : Utilisez les boutons Haut et Bas pour ajuster l'ordre des codes-barres dans la liste.

## Technologies Utilisées

- **C#** avec **WPF** pour une interface utilisateur moderne et réactive.
- **iTextSharp** pour la génération de fichiers PDF.
- **ZXing.Net** pour la génération de codes-barres.
- **CsvHelper** pour l'importation de fichiers CSV.

## Installation

1. **Cloner le projet** :
   Clonez ce projet dans votre environnement local avec Git.

   ```bash
   git clone https://github.com/rodrigueantunes/gestion-codes-barres.git
   ```

2. **Ouvrir dans Visual Studio** :
   Ouvrez le projet dans Visual Studio pour commencer à l'utiliser ou le personnaliser.

3. **Installer les dépendances** :
   Assurez-vous d'avoir les dépendances suivantes installées via NuGet :
   - iTextSharp pour la génération de PDF.
   - CsvHelper pour l'importation des fichiers CSV.
   - ZXing.Net pour la gestion des codes-barres.

4. **Exécuter le projet** :
   Exécutez le projet avec F5 ou en sélectionnant Démarrer dans Visual Studio.

## Utilisation

![Gestion code barre](https://github.com/user-attachments/assets/6f18ab22-7777-4d8a-ac66-e4b7c0809a89)

### Ajouter un Code

1. Entrez un code-barres et sa description dans les champs respectifs.
2. Cliquez sur **Ajouter** pour ajouter le code à la liste.

### Importer un fichier CSV

1. Cliquez sur **Importer CSV** et sélectionnez un fichier CSV valide.
2. Le fichier CSV doit être structuré avec des colonnes pour le code-barres et la description.

### Générer un PDF

1. Cliquez sur **Générer PDF** pour créer un fichier PDF contenant tous les codes-barres et descriptions de la liste.
2. Vous avez la possibilité d'ajouter des logos personnalisés via les boutons **Logo Client** et **Logo Société**.

### Déplacer des Codes

1. Sélectionnez un code dans la liste et utilisez les boutons **Haut** et **Bas** pour réorganiser les codes-barres.

## Licence

Ce projet est sous Licence MIT. Vous pouvez l'utiliser, le modifier et le distribuer librement.
