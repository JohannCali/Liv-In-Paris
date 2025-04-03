# Liv'in Paris - Partage de Repas entre Voisins

## Présentation du Projet

Liv'in Paris est une application innovante permettant le partage de repas entre voisins à Paris intramuros. Que vous soyez cuisinier ou client, l'application vous permet de vous inscrire, de partager des plats faits maison et de commander des repas préparés par des voisins. L'objectif est de faciliter les interactions culinaires locales tout en optimisant les trajets de livraison grâce à des algorithmes de graphes.

## Fonctionnalités Principales

### 1. Inscription et Gestion des Utilisateurs
- **Inscription** : Les utilisateurs (cuisiniers ou clients) s'inscrivent en fournissant leurs informations personnelles.
- **Identifiants Uniques** : Chaque utilisateur reçoit un identifiant unique et un mot de passe.
- **Rôles Multiples** : Un utilisateur peut être à la fois cuisinier et client.

### 2. Gestion des Plats
- **Publication de Plats** : Les cuisiniers publient des plats avec des détails tels que le type de plat (entrée, plat principal, dessert), nombre de personnes, date de fabrication, prix, nationalité de la cuisine, régime alimentaire, ingrédients principaux, et une photo.
- **Livraison** : Les cuisiniers assurent la livraison des plats et peuvent combiner plusieurs livraisons en fonction des zones.

### 3. Commandes et Transactions
- **Commandes** : Les clients peuvent passer des commandes pour des plats disponibles.
- **Transactions** : Les transactions financières sont réalisées avant la première livraison.
- **Historique** : L'application conserve l'historique des transactions pour identifier les meilleurs cuisiniers et clients.

### 4. Optimisation des Trajets
- **Calcul du Plus Court Chemin** : Utilisation d'algorithmes de graphes (Dijkstra, Bellman-Ford, Floyd-Warshall) pour calculer le plus court chemin entre les adresses du cuisinier et du client via les lignes de métro.
- **Visualisation** : Outils de visualisation pour afficher les trajets optimisés.

### 5. Statistiques et Analyses
- **Statistiques** : Affichage de diverses statistiques telles que le nombre de livraisons par cuisinier, les commandes par période, la moyenne des prix des commandes, etc.
- **Coloration de Graphe** : Utilisation de l'algorithme de Welsh-Powell pour la coloration de graphe et l'analyse des relations entre clients et cuisiniers.

## Étapes du Projet

### Étape 1 : Algorithmique et Graphes
- **Création de Classes** : Classes Nœud, Lien, et Graphe pour traiter les données sous forme de graphe.
- **Parcours de Graphe** : Implémentation des algorithmes de parcours en largeur et en profondeur.
- **Propriétés du Graphe** : Analyse des propriétés du graphe (connexité, cycles, etc.).

### Étape 2 : Généralisation et Intégration
- **Classes Génériques** : Modification des classes pour les rendre génériques.
- **Plan du Métro de Paris** : Application des modifications au plan du métro de Paris.
- **Algorithmes de Chemin le Plus Court** : Implémentation des algorithmes de Dijkstra, Bellman-Ford, et Floyd-Warshall.

### Étape 3 : Gestion de Couverture de Graphe
- **Coloration de Graphe** : Application de la coloration de graphe pour analyser les relations entre clients et cuisiniers.
- **Exportation de Données** : Exportation des données en formats JSON et XML.

## Technologies Utilisées

- **Langage** : C#
- **Base de Données** : SQL

## Créateurs

Ce projet a été développé par Johann Cali, Jules Brugère et Solal Chatelein.

---

