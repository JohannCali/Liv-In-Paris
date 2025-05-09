-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: liv
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `plats`
--

DROP TABLE IF EXISTS `plats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plats` (
  `Id_Plat` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(50) DEFAULT NULL,
  `Type_Plat` enum('Entree','Plat Principal','Dessert') DEFAULT NULL,
  `Pour_combien_de_personnes` int DEFAULT NULL,
  `Specialite` varchar(50) DEFAULT NULL,
  `Regime_Alimentaire` varchar(50) DEFAULT NULL,
  `Ingrédients` text,
  `Date_de_fabrication` datetime DEFAULT NULL,
  `Date_de_Peremption` datetime DEFAULT NULL,
  `Prix_par_Portion` decimal(15,2) DEFAULT NULL,
  `ID_Cuisinier` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id_Plat`),
  KEY `fk_id_cuisinier_plats` (`ID_Cuisinier`),
  CONSTRAINT `fk_id_cuisinier_plats` FOREIGN KEY (`ID_Cuisinier`) REFERENCES `utilisateur` (`Id_Utilisateur`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plats`
--

LOCK TABLES `plats` WRITE;
/*!40000 ALTER TABLE `plats` DISABLE KEYS */;
INSERT INTO `plats` VALUES (1,'Croque-Monsieur','Entree',1,'Française','Classique','Pain de mie, Jambon, Fromage','2023-02-01 00:00:00','2023-03-01 00:00:00',5.50,'8'),(2,'Quiche Lorraine','Plat Principal',2,'Française','Classique','Pâte brisée, Lardons, Crème fraîche, Oeufs, Fromage râpé','2023-02-10 00:00:00','2023-04-10 00:00:00',10.00,'8'),(3,'Salade César','Plat Principal',1,'Italienne','Végétarien','Laitue, Poulet, Croutons, Parmesan, Sauce César','2023-02-12 00:00:00','2023-03-12 00:00:00',12.50,'9'),(4,'Soupe de légumes','Entree',2,'Française','Végétarien','Carottes, Poireaux, Pommes de terre, Bouillon de légumes','2023-02-15 00:00:00','2023-03-15 00:00:00',4.00,'9'),(5,'Poulet rôti','Plat Principal',4,'Française','Classique','Poulet, Herbes de Provence, Beurre, Ail','2023-02-20 00:00:00','2023-04-20 00:00:00',15.00,'10'),(6,'Tarte aux pommes','Dessert',2,'Française','Classique','Pâte brisée, Pommes, Sucre, Beurre','2023-02-22 00:00:00','2023-03-22 00:00:00',6.00,'10');
/*!40000 ALTER TABLE `plats` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-09 17:18:40
