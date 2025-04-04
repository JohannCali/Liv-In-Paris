-- Création de la base de données si elle n'existe pas
CREATE DATABASE IF NOT EXISTS LIV;
USE LIV;

-- Création de la table Utilisateur si elle n'existe pas
CREATE TABLE IF NOT EXISTS Utilisateur (
   Id_Utilisateur VARCHAR(50) PRIMARY KEY,
   Nom VARCHAR(50),
   Prenom VARCHAR(50),
   Adresse VARCHAR(255),
   Email VARCHAR(50),
   Mot_de_Passe VARCHAR(50),
   Numero_de_Telephone VARCHAR(20)
);

-- Création de la table Plats si elle n'existe pas
CREATE TABLE IF NOT EXISTS Plats (
   Id_Plat VARCHAR(50) PRIMARY KEY,
   Nom VARCHAR(50),
   Type_Plat ENUM('Entree', 'Plat Principal', 'Dessert'),
   Pour_combien_de_personnes INT,
   Nationalite VARCHAR(50),
   Regime_Alimentaire VARCHAR(50),
   Ingrédients TEXT,
   Image LONGBLOB,
   Date_de_fabrication DATETIME,
   Date_de_Peremption DATETIME,
   Prix_par_Portion DECIMAL(15, 2)
);

-- Création de la table Cuisinier si elle n'existe pas
CREATE TABLE IF NOT EXISTS Cuisinier (
   Id_Utilisateur VARCHAR(50) PRIMARY KEY,
   Specialite Varchar(50),
   FOREIGN KEY (Id_Utilisateur) REFERENCES Utilisateur(Id_Utilisateur)
);

-- Création de la table Client si elle n'existe pas
CREATE TABLE IF NOT EXISTS Client (
   Id_Utilisateur VARCHAR(50) PRIMARY KEY,
   Particulier BOOLEAN,
   FOREIGN KEY (Id_Utilisateur) REFERENCES Utilisateur(Id_Utilisateur)
);

-- Création de la table Commandes si elle n'existe pas
CREATE TABLE IF NOT EXISTS Commandes (
   Id_Commande INT PRIMARY KEY AUTO_INCREMENT,
   Quantite INT,
   Date_heure DATETIME,
   Id_Utilisateur VARCHAR(50) NOT NULL,
   Id_Plat VARCHAR(50) NOT NULL,
   FOREIGN KEY (Id_Utilisateur) REFERENCES Client(Id_Utilisateur),
   FOREIGN KEY (Id_Plat) REFERENCES Plats(Id_Plat)
);

-- Création de l'utilisateur si il n'existe pas
CREATE USER IF NOT EXISTS 'nouvel_utilisateur'@'localhost' IDENTIFIED BY 'mot_de_passe_secure';

-- Attribution des privilèges si l'utilisateur n'a pas les privilèges nécessaires
GRANT ALL PRIVILEGES ON *.* TO 'nouvel_utilisateur'@'localhost' WITH GRANT OPTION;

-- Affichage des données des tables
SELECT * FROM Utilisateur;

SELECT * FROM Plats;

SELECT * FROM Cuisinier;

-- SELECT * FROM Client;

-- SELECT * FROM Commandes;

-- Commande pour décrire si les types des attributs de cuisinier
DESCRIBE Cuisinier;
-- commande pour forcer à modifier la spécialité en varchar 
ALTER TABLE Cuisinier MODIFY COLUMN Specialite VARCHAR(50);