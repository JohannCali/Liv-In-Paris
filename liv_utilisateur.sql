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
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `utilisateur` (
  `Id_Utilisateur` varchar(50) NOT NULL,
  `Nom` varchar(50) DEFAULT NULL,
  `Prenom` varchar(50) DEFAULT NULL,
  `Adresse` varchar(255) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Mot_de_Passe` varchar(255) DEFAULT NULL,
  `Numero_de_Telephone` varchar(20) DEFAULT NULL,
  `Station` varchar(255) DEFAULT NULL,
  `Code_Postal` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Id_Utilisateur`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utilisateur`
--

LOCK TABLES `utilisateur` WRITE;
/*!40000 ALTER TABLE `utilisateur` DISABLE KEYS */;
INSERT INTO `utilisateur` VALUES ('1','Dupont','Pierre','123 Rue de Paris','dupont.pierre@mail.com','Motdepasse1','0601010101','Argentine','75001'),('10','Durand','Julien','987 Rue du Père Lachaise','durand.julien@mail.com','Motdepasse10','0610101010','Père Lachaise','75020'),('2','Martin','Marie','456 Avenue des Champs','martin.marie@mail.com','Motdepasse2','0602020202','Daumesnil','75011'),('3','Bernard','Paul','789 Boulevard Saint-Germain','bernard.paul@mail.com','Motdepasse3','0603030303','Invalides','75007'),('4','Lemoine','Sophie','321 Rue de la République','lemoine.sophie@mail.com','Motdepasse4','0604040404','Nation','75012'),('5','Lemoine','Alexandre','654 Rue du Faubourg','lemoine.alexandre@mail.com','Motdepasse5','0605050505','Gare de Lyon','75012'),('6','Moreau','Claire','987 Place de l’Opéra','moreau.claire@mail.com','Motdepasse6','0606060606','Opéra','75009'),('7','Petit','Jacques','123 Rue de la Bastille','petit.jacques@mail.com','Motdepasse7','0607070707','Bastille','75011'),('8','Fournier','Julien','321 Rue de Châtelet','fournier.julien@mail.com','Motdepasse8','0608080808','Châtelet','75001'),('9','Leclerc','Amandine','654 Rue de Belleville','leclerc.amandine@mail.com','Motdepasse9','0609090909','Belleville','75019');
/*!40000 ALTER TABLE `utilisateur` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-09 17:18:39
