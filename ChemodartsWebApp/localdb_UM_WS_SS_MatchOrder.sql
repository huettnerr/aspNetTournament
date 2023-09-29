-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: localhost    Database: localdb
-- ------------------------------------------------------
-- Server version	8.0.34

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
-- Table structure for table `groups`
--

DROP TABLE IF EXISTS `groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groups` (
  `groupId` int NOT NULL AUTO_INCREMENT,
  `roundId` int NOT NULL,
  `name` text NOT NULL,
  `hasRematch` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`groupId`),
  KEY `roundId` (`roundId`),
  CONSTRAINT `g_rId` FOREIGN KEY (`roundId`) REFERENCES `rounds` (`roundId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=206 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES (55,6,'Sternburg',0),(56,6,'Coschützer',0),(57,6,'Tyskie',0),(58,6,'Pilsner Urquell',0),(59,6,'Oettinger Pils',0),(60,6,'Staropramen',0),(61,6,'Ur-Krostitzer',0),(62,6,'Feldschlösschen',0),(70,7,'Stufe der besten 16',0),(71,7,'Stufe der besten 8',0),(72,7,'Stufe der besten 4',0),(73,7,'Stufe der besten 2',0),(74,8,'Alice Cooper',0),(75,8,'Black Sabbath',0),(76,8,'Cinderella',0),(77,8,'Dokken',0),(78,8,'Europe',0),(79,8,'Foreigner',0),(80,8,'Iron Maiden',0),(81,8,'Judas Priest',0),(105,11,'Stufe der besten 16',0),(106,11,'Viertelfinale',0),(107,11,'Halbfinale',0),(108,11,'Finale',0),(117,14,'Row Zero',0),(118,14,'Backstage',0),(119,14,'Chemoklo',0),(120,14,'Dance',0),(121,14,'Einlass',0),(122,14,'Freisitz',0),(123,14,'Garderobe',0),(124,14,'Tresen',0),(133,15,'Stufe der besten 16',0),(134,15,'Viertelfinale',0),(135,15,'Halbfinale',0),(136,15,'Finale',0),(137,16,'A',0),(138,16,'B',0),(139,16,'C',0),(140,16,'D',0),(141,16,'E',0),(142,16,'F',0),(143,16,'G',0),(144,16,'H',0),(154,17,'Viertelfinale',0),(155,17,'Halbfinale',0),(156,17,'Finale',0),(179,19,'Viertelfinale',0),(180,19,'Halbfinale',0),(181,19,'Finale',0),(201,18,'a',0),(202,18,'b',0),(203,18,'c',1),(204,18,'d',1);
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map_round_seed_player`
--

DROP TABLE IF EXISTS `map_round_seed_player`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `map_round_seed_player` (
  `tpMapId` int NOT NULL AUTO_INCREMENT,
  `roundId` int NOT NULL,
  `seedId` int NOT NULL,
  `playerId` int DEFAULT NULL,
  `playerFixed` tinyint(1) NOT NULL DEFAULT '0',
  `playerCheckedIn` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`tpMapId`),
  KEY `playerId` (`playerId`),
  KEY `seedId` (`seedId`),
  KEY `playerId_2` (`playerId`),
  KEY `map_tp_rId_idx1` (`roundId`),
  CONSTRAINT `map_rsp_pId` FOREIGN KEY (`playerId`) REFERENCES `players` (`playerId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `map_rsp_rId` FOREIGN KEY (`roundId`) REFERENCES `rounds` (`roundId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `map_rsp_sId` FOREIGN KEY (`seedId`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=309 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_round_seed_player`
--

LOCK TABLES `map_round_seed_player` WRITE;
/*!40000 ALTER TABLE `map_round_seed_player` DISABLE KEYS */;
INSERT INTO `map_round_seed_player` VALUES (69,6,188,24,0,0),(70,6,189,37,0,0),(71,6,190,36,0,0),(72,6,191,14,0,0),(73,6,192,26,0,0),(74,6,193,11,0,0),(75,6,194,38,0,0),(76,6,195,41,0,0),(77,6,196,17,0,0),(78,6,197,43,0,0),(79,6,198,42,0,0),(80,6,199,44,0,0),(81,6,200,22,0,0),(82,6,201,45,0,0),(83,6,202,15,0,0),(84,6,203,16,0,0),(85,6,204,27,0,0),(86,6,205,8,0,1),(87,6,206,7,0,0),(88,6,207,10,0,0),(89,6,208,23,0,0),(90,6,209,20,0,0),(91,6,210,31,0,0),(92,6,211,29,0,0),(93,6,212,25,0,0),(94,6,213,39,0,0),(95,6,214,18,0,0),(96,6,215,13,0,0),(97,6,216,21,0,0),(98,6,217,33,0,0),(99,6,218,28,0,0),(100,6,219,40,0,0),(101,8,294,33,0,0),(102,8,295,31,0,0),(103,8,296,7,0,0),(104,8,297,11,0,0),(105,8,298,51,0,0),(106,8,299,42,0,0),(107,8,300,44,0,0),(108,8,301,37,0,0),(109,8,302,53,0,0),(110,8,303,27,0,0),(111,8,304,49,0,0),(112,8,305,14,0,0),(113,8,306,8,0,0),(114,8,307,20,0,0),(115,8,308,52,0,0),(116,8,309,47,0,0),(117,8,310,41,0,0),(118,8,311,48,0,0),(119,8,312,24,0,0),(121,8,314,10,0,0),(122,8,315,26,0,0),(123,8,316,13,0,0),(125,8,318,9,0,0),(126,8,319,29,0,0),(127,8,320,23,0,0),(129,8,322,50,0,0),(130,8,323,22,0,0),(131,8,324,46,0,0),(140,14,564,11,0,0),(141,14,565,41,0,0),(142,14,566,60,0,0),(143,14,567,59,0,0),(144,14,568,57,0,0),(145,14,569,47,0,0),(146,14,570,42,0,0),(147,14,571,27,0,0),(148,14,572,61,0,0),(149,14,573,58,0,0),(150,14,574,24,0,0),(151,14,575,63,0,0),(152,14,576,7,0,0),(153,14,577,10,0,0),(154,14,578,54,0,0),(156,14,580,50,0,0),(157,14,581,44,0,0),(158,14,582,26,0,0),(160,14,584,39,0,0),(161,14,585,56,0,0),(162,14,586,8,0,0),(164,14,588,62,0,0),(165,14,589,22,0,0),(166,14,590,9,0,0),(168,14,592,37,0,0),(169,14,593,55,0,0),(170,14,594,33,0,0),(171,16,686,48,0,0),(172,16,687,21,0,0),(173,16,688,64,0,0),(174,16,689,25,0,0),(175,16,690,14,0,0),(176,16,691,13,0,0),(177,16,692,27,0,0),(178,16,693,65,0,0),(179,16,694,7,0,0),(180,16,695,26,0,0),(181,16,696,17,0,0),(182,16,697,34,0,0),(183,16,698,50,0,0),(184,16,699,43,0,0),(185,16,700,8,0,0),(187,16,702,66,0,0),(188,16,703,67,0,0),(189,16,704,24,0,0),(190,16,705,68,0,0),(191,16,706,33,0,0),(192,16,707,69,0,0),(193,16,708,16,0,0),(194,16,709,70,0,0),(195,16,710,71,0,0),(196,16,711,23,0,0),(197,16,712,20,0,0),(198,16,713,9,0,0),(290,18,906,NULL,0,0),(291,18,907,NULL,0,0),(292,18,908,NULL,0,0),(293,18,909,NULL,0,0),(294,18,910,NULL,0,0),(295,18,911,NULL,0,0),(296,18,912,NULL,0,0),(297,18,913,NULL,0,0),(298,18,914,NULL,0,0),(299,18,915,NULL,0,0),(300,18,916,NULL,0,0),(301,18,917,NULL,0,0),(302,18,918,NULL,0,0),(303,18,919,NULL,0,0);
/*!40000 ALTER TABLE `map_round_seed_player` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map_round_venue`
--

DROP TABLE IF EXISTS `map_round_venue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `map_round_venue` (
  `rvMapId` int NOT NULL AUTO_INCREMENT,
  `roundId` int NOT NULL,
  `venueId` int NOT NULL,
  PRIMARY KEY (`rvMapId`),
  KEY `roundId` (`roundId`),
  KEY `venueId` (`venueId`),
  CONSTRAINT `map_rv_rId` FOREIGN KEY (`roundId`) REFERENCES `rounds` (`roundId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `map_rv_vId` FOREIGN KEY (`venueId`) REFERENCES `venues` (`venueId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_round_venue`
--

LOCK TABLES `map_round_venue` WRITE;
/*!40000 ALTER TABLE `map_round_venue` DISABLE KEYS */;
INSERT INTO `map_round_venue` VALUES (1,6,5),(2,6,6),(3,6,7),(4,6,8),(5,6,9),(8,7,5),(9,7,6),(10,7,7),(11,7,8),(12,7,9),(14,8,5),(15,8,6),(16,8,7),(17,8,8),(18,8,9),(19,11,5),(20,11,6),(21,11,7),(22,11,8),(23,11,9),(24,14,5),(25,14,6),(26,14,7),(27,14,8),(28,14,9),(29,15,5),(30,15,6),(31,15,7),(32,15,8);
/*!40000 ALTER TABLE `map_round_venue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `matches`
--

DROP TABLE IF EXISTS `matches`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `matches` (
  `matchId` int NOT NULL AUTO_INCREMENT,
  `matchOrderValue` int DEFAULT NULL,
  `matchStage` int DEFAULT NULL,
  `groupId` int NOT NULL,
  `seed1Id` int NOT NULL,
  `seed2Id` int NOT NULL,
  `winnerSeedId` int DEFAULT NULL,
  `status` enum('Created','Active','Finished','Aborted') NOT NULL DEFAULT 'Created',
  `venueId` int DEFAULT NULL,
  `time_started` datetime DEFAULT NULL,
  `time_finished` datetime DEFAULT NULL,
  PRIMARY KEY (`matchId`),
  UNIQUE KEY `matchId_2` (`matchId`),
  KEY `matchId` (`matchId`),
  KEY `player1Id` (`seed1Id`),
  KEY `player2Id` (`seed2Id`),
  KEY `groupId` (`groupId`),
  KEY `venueId` (`venueId`),
  KEY `m_wsId_idx` (`winnerSeedId`),
  CONSTRAINT `m_gId` FOREIGN KEY (`groupId`) REFERENCES `groups` (`groupId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `m_s1Id` FOREIGN KEY (`seed1Id`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `m_s2Id` FOREIGN KEY (`seed2Id`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `m_vId` FOREIGN KEY (`venueId`) REFERENCES `venues` (`venueId`) ON UPDATE CASCADE,
  CONSTRAINT `m_wsId` FOREIGN KEY (`winnerSeedId`) REFERENCES `seeds` (`seedId`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1020 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `matches`
--

LOCK TABLES `matches` WRITE;
/*!40000 ALTER TABLE `matches` DISABLE KEYS */;
INSERT INTO `matches` VALUES (112,1,NULL,55,188,189,189,'Finished',NULL,NULL,NULL),(113,1,NULL,55,190,191,191,'Finished',NULL,NULL,NULL),(114,3,NULL,55,188,190,190,'Finished',NULL,NULL,NULL),(115,3,NULL,55,189,191,189,'Finished',NULL,NULL,NULL),(116,5,NULL,55,188,191,191,'Finished',NULL,NULL,NULL),(117,5,NULL,55,189,190,189,'Finished',NULL,NULL,NULL),(118,1,NULL,56,192,193,192,'Finished',NULL,NULL,NULL),(119,1,NULL,56,194,195,195,'Finished',NULL,NULL,NULL),(120,3,NULL,56,192,194,194,'Finished',NULL,NULL,NULL),(121,3,NULL,56,193,195,195,'Finished',NULL,NULL,NULL),(122,5,NULL,56,192,195,192,'Finished',NULL,NULL,NULL),(123,5,NULL,56,193,194,194,'Finished',NULL,NULL,NULL),(124,1,NULL,57,196,197,197,'Finished',NULL,NULL,NULL),(125,1,NULL,57,198,199,199,'Finished',NULL,NULL,NULL),(126,3,NULL,57,196,198,198,'Finished',NULL,NULL,NULL),(127,3,NULL,57,197,199,199,'Finished',NULL,NULL,NULL),(128,5,NULL,57,196,199,199,'Finished',NULL,NULL,NULL),(129,5,NULL,57,197,198,197,'Finished',NULL,NULL,NULL),(130,1,NULL,58,200,201,201,'Finished',NULL,NULL,NULL),(131,1,NULL,58,202,203,202,'Finished',NULL,NULL,NULL),(132,3,NULL,58,200,202,202,'Finished',NULL,NULL,NULL),(133,3,NULL,58,201,203,203,'Finished',NULL,NULL,NULL),(134,5,NULL,58,200,203,203,'Finished',NULL,NULL,NULL),(135,5,NULL,58,201,202,202,'Finished',NULL,NULL,NULL),(136,1,NULL,59,204,205,205,'Finished',NULL,NULL,NULL),(137,1,NULL,59,206,207,207,'Finished',NULL,NULL,NULL),(138,3,NULL,59,204,206,204,'Finished',NULL,NULL,NULL),(139,3,NULL,59,205,207,205,'Finished',NULL,NULL,NULL),(140,5,NULL,59,204,207,204,'Finished',NULL,NULL,NULL),(141,5,NULL,59,205,206,206,'Finished',NULL,NULL,NULL),(142,1,NULL,60,208,209,208,'Finished',NULL,NULL,NULL),(143,1,NULL,60,210,211,210,'Finished',NULL,NULL,NULL),(144,3,NULL,60,208,210,210,'Finished',NULL,NULL,NULL),(145,3,NULL,60,209,211,211,'Finished',NULL,NULL,NULL),(146,5,NULL,60,208,211,208,'Finished',NULL,NULL,NULL),(147,5,NULL,60,209,210,210,'Finished',NULL,NULL,NULL),(148,1,NULL,61,212,213,213,'Finished',NULL,NULL,NULL),(149,1,NULL,61,214,215,215,'Finished',NULL,NULL,NULL),(150,3,NULL,61,212,214,214,'Finished',NULL,NULL,NULL),(151,3,NULL,61,213,215,215,'Finished',NULL,NULL,NULL),(152,5,NULL,61,212,215,215,'Finished',NULL,NULL,NULL),(153,5,NULL,61,213,214,213,'Finished',NULL,NULL,NULL),(154,1,NULL,62,216,217,216,'Finished',NULL,NULL,NULL),(155,1,NULL,62,218,219,218,'Finished',NULL,NULL,NULL),(156,3,NULL,62,216,218,218,'Finished',NULL,NULL,NULL),(157,3,NULL,62,217,219,217,'Finished',NULL,NULL,NULL),(158,5,NULL,62,216,219,216,'Finished',NULL,NULL,NULL),(159,5,NULL,62,217,218,218,'Finished',NULL,NULL,NULL),(167,0,NULL,70,189,216,189,'Finished',NULL,NULL,NULL),(168,1,NULL,70,195,213,195,'Finished',NULL,NULL,NULL),(169,2,NULL,70,199,208,199,'Finished',NULL,NULL,NULL),(170,3,NULL,70,202,205,202,'Finished',NULL,NULL,NULL),(171,4,NULL,70,204,203,204,'Finished',NULL,NULL,NULL),(172,5,NULL,70,210,197,210,'Finished',NULL,NULL,NULL),(173,6,NULL,70,215,194,215,'Finished',NULL,NULL,NULL),(174,7,NULL,70,218,191,218,'Finished',NULL,NULL,NULL),(175,0,NULL,71,189,195,195,'Finished',NULL,NULL,NULL),(176,1,NULL,71,199,202,199,'Finished',NULL,NULL,NULL),(177,2,NULL,71,204,210,210,'Finished',NULL,NULL,NULL),(178,3,NULL,71,215,218,215,'Finished',NULL,NULL,NULL),(179,0,NULL,72,195,199,195,'Finished',NULL,NULL,NULL),(180,1,NULL,72,210,215,210,'Finished',NULL,NULL,NULL),(181,0,NULL,73,195,210,195,'Finished',NULL,NULL,NULL),(366,1,NULL,74,294,295,295,'Finished',NULL,NULL,NULL),(367,1,NULL,74,296,297,296,'Finished',NULL,NULL,NULL),(368,3,NULL,74,294,296,296,'Finished',NULL,NULL,NULL),(369,3,NULL,74,295,297,295,'Finished',NULL,NULL,NULL),(370,5,NULL,74,294,297,297,'Finished',NULL,NULL,NULL),(371,5,NULL,74,295,296,296,'Finished',NULL,NULL,NULL),(372,1,NULL,75,298,299,298,'Finished',NULL,NULL,NULL),(373,1,NULL,75,300,301,301,'Finished',NULL,NULL,NULL),(374,3,NULL,75,298,300,298,'Finished',NULL,NULL,NULL),(375,3,NULL,75,299,301,301,'Finished',NULL,NULL,NULL),(376,5,NULL,75,298,301,301,'Finished',NULL,NULL,NULL),(377,5,NULL,75,299,300,300,'Finished',NULL,NULL,NULL),(378,1,NULL,76,302,303,303,'Finished',NULL,NULL,NULL),(379,1,NULL,76,304,305,304,'Finished',NULL,NULL,NULL),(380,3,NULL,76,302,304,302,'Finished',NULL,NULL,NULL),(381,3,NULL,76,303,305,305,'Finished',NULL,NULL,NULL),(382,5,NULL,76,302,305,305,'Finished',NULL,NULL,NULL),(383,5,NULL,76,303,304,304,'Finished',NULL,NULL,NULL),(384,1,NULL,77,306,307,307,'Finished',NULL,NULL,NULL),(385,1,NULL,77,308,309,309,'Finished',NULL,NULL,NULL),(386,3,NULL,77,306,308,306,'Finished',NULL,NULL,NULL),(387,3,NULL,77,307,309,309,'Finished',NULL,NULL,NULL),(388,5,NULL,77,306,309,309,'Finished',NULL,NULL,NULL),(389,5,NULL,77,307,308,307,'Finished',NULL,NULL,NULL),(390,0,NULL,78,310,311,310,'Finished',NULL,NULL,NULL),(391,1,NULL,78,311,312,312,'Finished',NULL,NULL,NULL),(392,2,NULL,78,310,312,310,'Finished',NULL,NULL,NULL),(393,3,NULL,78,312,311,312,'Finished',NULL,NULL,NULL),(394,4,NULL,78,312,310,310,'Finished',NULL,NULL,NULL),(395,5,NULL,78,311,310,310,'Finished',NULL,NULL,NULL),(396,0,NULL,79,314,315,314,'Finished',NULL,NULL,NULL),(397,1,NULL,79,315,316,315,'Finished',NULL,NULL,NULL),(398,2,NULL,79,314,316,314,'Finished',NULL,NULL,NULL),(399,3,NULL,79,316,315,316,'Finished',NULL,NULL,NULL),(400,4,NULL,79,316,314,314,'Finished',NULL,NULL,NULL),(401,5,NULL,79,315,314,315,'Finished',NULL,NULL,NULL),(402,0,NULL,80,318,319,319,'Finished',NULL,NULL,NULL),(403,1,NULL,80,319,320,320,'Finished',NULL,NULL,NULL),(404,2,NULL,80,318,320,320,'Finished',NULL,NULL,NULL),(405,3,NULL,80,320,319,320,'Finished',NULL,NULL,NULL),(406,4,NULL,80,320,318,320,'Finished',NULL,NULL,NULL),(407,5,NULL,80,319,318,319,'Finished',NULL,NULL,NULL),(408,0,NULL,81,322,323,322,'Finished',NULL,NULL,NULL),(409,1,NULL,81,323,324,324,'Finished',NULL,NULL,NULL),(410,2,NULL,81,322,324,322,'Finished',NULL,NULL,NULL),(411,3,NULL,81,324,323,324,'Finished',NULL,NULL,NULL),(412,4,NULL,81,324,322,322,'Finished',NULL,NULL,NULL),(413,5,NULL,81,323,322,322,'Finished',NULL,NULL,NULL),(414,0,NULL,105,296,324,296,'Finished',NULL,NULL,NULL),(415,1,NULL,105,301,319,301,'Finished',NULL,NULL,NULL),(416,2,NULL,105,305,315,305,'Finished',NULL,NULL,NULL),(417,3,NULL,105,309,312,309,'Finished',NULL,NULL,NULL),(418,4,NULL,105,310,307,310,'Finished',NULL,NULL,NULL),(419,5,NULL,105,314,304,314,'Finished',NULL,NULL,NULL),(420,6,NULL,105,320,298,320,'Finished',NULL,NULL,NULL),(421,7,NULL,105,322,295,322,'Finished',NULL,NULL,NULL),(422,0,NULL,106,296,301,301,'Finished',NULL,NULL,NULL),(423,1,NULL,106,305,309,309,'Finished',NULL,NULL,NULL),(424,2,NULL,106,310,314,310,'Finished',NULL,NULL,NULL),(425,3,NULL,106,320,322,322,'Finished',NULL,NULL,NULL),(426,0,NULL,107,301,309,309,'Finished',NULL,NULL,NULL),(427,1,NULL,107,310,322,310,'Finished',NULL,NULL,NULL),(428,0,NULL,108,309,310,309,'Finished',NULL,NULL,NULL),(573,1,NULL,117,564,565,565,'Finished',NULL,NULL,NULL),(574,1,NULL,117,566,567,567,'Finished',NULL,NULL,NULL),(575,3,NULL,117,564,566,566,'Finished',NULL,NULL,NULL),(576,3,NULL,117,565,567,565,'Finished',NULL,NULL,NULL),(577,5,NULL,117,564,567,564,'Finished',NULL,NULL,NULL),(578,5,NULL,117,565,566,565,'Finished',NULL,NULL,NULL),(579,1,NULL,118,568,569,568,'Finished',NULL,NULL,NULL),(580,1,NULL,118,570,571,570,'Finished',NULL,NULL,NULL),(581,3,NULL,118,568,570,568,'Finished',NULL,NULL,NULL),(582,3,NULL,118,569,571,569,'Finished',NULL,NULL,NULL),(583,5,NULL,118,568,571,568,'Finished',NULL,NULL,NULL),(584,5,NULL,118,569,570,569,'Finished',NULL,NULL,NULL),(585,1,NULL,119,572,573,573,'Finished',NULL,NULL,NULL),(586,1,NULL,119,574,575,575,'Finished',NULL,NULL,NULL),(587,3,NULL,119,572,574,574,'Finished',NULL,NULL,NULL),(588,3,NULL,119,573,575,575,'Finished',NULL,NULL,NULL),(589,5,NULL,119,572,575,575,'Finished',NULL,NULL,NULL),(590,5,NULL,119,573,574,573,'Finished',NULL,NULL,NULL),(591,0,NULL,120,576,577,576,'Finished',NULL,NULL,NULL),(592,1,NULL,120,577,578,578,'Finished',NULL,NULL,NULL),(593,2,NULL,120,576,578,578,'Finished',NULL,NULL,NULL),(594,3,NULL,120,578,577,578,'Finished',NULL,NULL,NULL),(595,4,NULL,120,578,576,578,'Finished',NULL,NULL,NULL),(596,5,NULL,120,577,576,576,'Finished',NULL,NULL,NULL),(597,0,NULL,121,580,581,580,'Finished',NULL,NULL,NULL),(598,1,NULL,121,581,582,581,'Finished',NULL,NULL,NULL),(599,2,NULL,121,580,582,580,'Finished',NULL,NULL,NULL),(600,3,NULL,121,582,581,581,'Finished',NULL,NULL,NULL),(601,4,NULL,121,582,580,582,'Finished',NULL,NULL,NULL),(602,5,NULL,121,581,580,580,'Finished',NULL,NULL,NULL),(603,0,NULL,122,584,585,585,'Finished',NULL,NULL,NULL),(604,1,NULL,122,585,586,585,'Finished',NULL,NULL,NULL),(605,2,NULL,122,584,586,584,'Finished',NULL,NULL,NULL),(606,3,NULL,122,586,585,586,'Finished',NULL,NULL,NULL),(607,4,NULL,122,586,584,584,'Finished',NULL,NULL,NULL),(608,5,NULL,122,585,584,584,'Finished',NULL,NULL,NULL),(609,0,NULL,123,588,589,588,'Finished',NULL,NULL,NULL),(610,1,NULL,123,589,590,589,'Finished',NULL,NULL,NULL),(611,2,NULL,123,588,590,588,'Finished',NULL,NULL,NULL),(612,3,NULL,123,590,589,590,'Finished',NULL,NULL,NULL),(613,4,NULL,123,590,588,588,'Finished',NULL,NULL,NULL),(614,5,NULL,123,589,588,588,'Finished',NULL,NULL,NULL),(615,0,NULL,124,592,593,592,'Finished',NULL,NULL,NULL),(616,1,NULL,124,593,594,593,'Finished',NULL,NULL,NULL),(617,2,NULL,124,592,594,592,'Finished',NULL,NULL,NULL),(618,3,NULL,124,594,593,593,'Finished',NULL,NULL,NULL),(619,4,NULL,124,594,592,592,'Finished',NULL,NULL,NULL),(620,5,NULL,124,593,592,593,'Finished',NULL,NULL,NULL),(621,0,NULL,133,565,593,565,'Finished',NULL,NULL,NULL),(622,1,NULL,133,568,589,568,'Finished',NULL,NULL,NULL),(623,2,NULL,133,575,585,575,'Finished',NULL,NULL,NULL),(624,3,NULL,133,578,581,581,'Finished',NULL,NULL,NULL),(625,4,NULL,133,580,576,576,'Finished',NULL,NULL,NULL),(626,5,NULL,133,584,573,573,'Finished',NULL,NULL,NULL),(627,6,NULL,133,588,569,569,'Finished',NULL,NULL,NULL),(628,7,NULL,133,592,567,592,'Finished',NULL,NULL,NULL),(629,0,NULL,134,565,568,565,'Finished',NULL,NULL,NULL),(630,1,NULL,134,575,581,575,'Finished',NULL,NULL,NULL),(631,2,NULL,134,576,573,573,'Finished',NULL,NULL,NULL),(632,3,NULL,134,569,592,592,'Finished',NULL,NULL,NULL),(633,0,NULL,135,565,575,575,'Finished',NULL,NULL,NULL),(634,1,NULL,135,573,592,592,'Finished',NULL,NULL,NULL),(635,0,NULL,136,575,592,592,'Finished',NULL,NULL,NULL),(717,1,NULL,137,686,687,687,'Finished',NULL,NULL,NULL),(718,1,NULL,137,688,689,688,'Finished',NULL,NULL,NULL),(719,3,NULL,137,686,688,688,'Finished',NULL,NULL,NULL),(720,3,NULL,137,687,689,687,'Finished',NULL,NULL,NULL),(721,5,NULL,137,686,689,686,'Finished',NULL,NULL,NULL),(722,5,NULL,137,687,688,688,'Finished',NULL,NULL,NULL),(723,1,NULL,138,690,691,690,'Finished',NULL,NULL,NULL),(724,1,NULL,138,692,693,692,'Finished',NULL,NULL,NULL),(725,3,NULL,138,690,692,690,'Finished',NULL,NULL,NULL),(726,3,NULL,138,691,693,691,'Finished',NULL,NULL,NULL),(727,5,NULL,138,690,693,690,'Finished',NULL,NULL,NULL),(728,5,NULL,138,691,692,691,'Finished',NULL,NULL,NULL),(729,1,NULL,139,694,695,694,'Finished',NULL,NULL,NULL),(730,1,NULL,139,696,697,696,'Finished',NULL,NULL,NULL),(731,3,NULL,139,694,696,694,'Finished',NULL,NULL,NULL),(732,3,NULL,139,695,697,695,'Finished',NULL,NULL,NULL),(733,5,NULL,139,694,697,694,'Finished',NULL,NULL,NULL),(734,5,NULL,139,695,696,695,'Finished',NULL,NULL,NULL),(735,0,NULL,140,698,699,699,'Finished',NULL,NULL,NULL),(736,1,NULL,140,699,700,699,'Finished',NULL,NULL,NULL),(737,2,NULL,140,698,700,698,'Finished',NULL,NULL,NULL),(738,3,NULL,140,700,699,699,'Finished',NULL,NULL,NULL),(739,4,NULL,140,700,698,698,'Finished',NULL,NULL,NULL),(740,5,NULL,140,699,698,698,'Finished',NULL,NULL,NULL),(741,0,NULL,141,702,703,702,'Finished',NULL,NULL,NULL),(742,1,NULL,141,703,704,703,'Finished',NULL,NULL,NULL),(743,2,NULL,141,702,704,702,'Finished',NULL,NULL,NULL),(744,3,NULL,141,704,703,703,'Finished',NULL,NULL,NULL),(745,4,NULL,141,704,702,702,'Finished',NULL,NULL,NULL),(746,5,NULL,141,703,702,702,'Finished',NULL,NULL,NULL),(747,0,NULL,142,705,706,705,'Finished',NULL,NULL,NULL),(748,1,NULL,142,706,707,706,'Finished',NULL,NULL,NULL),(749,2,NULL,142,705,707,705,'Finished',NULL,NULL,NULL),(750,3,NULL,142,707,706,706,'Finished',NULL,NULL,NULL),(751,4,NULL,142,707,705,705,'Finished',NULL,NULL,NULL),(752,5,NULL,142,706,705,705,'Finished',NULL,NULL,NULL),(753,0,NULL,143,708,709,708,'Finished',NULL,NULL,NULL),(754,1,NULL,143,709,710,709,'Finished',NULL,NULL,NULL),(755,2,NULL,143,708,710,708,'Finished',NULL,NULL,NULL),(756,3,NULL,143,710,709,710,'Finished',NULL,NULL,NULL),(757,4,NULL,143,710,708,708,'Finished',NULL,NULL,NULL),(758,5,NULL,143,709,708,708,'Finished',NULL,NULL,NULL),(759,0,NULL,144,711,712,711,'Finished',NULL,NULL,NULL),(760,1,NULL,144,712,713,712,'Finished',NULL,NULL,NULL),(761,2,NULL,144,711,713,711,'Finished',NULL,NULL,NULL),(762,3,NULL,144,713,712,712,'Finished',NULL,NULL,NULL),(763,4,NULL,144,713,711,711,'Finished',NULL,NULL,NULL),(764,5,NULL,144,712,711,712,'Finished',NULL,NULL,NULL),(765,0,NULL,154,688,711,711,'Finished',NULL,NULL,NULL),(766,1,NULL,154,694,705,705,'Finished',NULL,NULL,NULL),(767,2,NULL,154,690,708,690,'Finished',NULL,NULL,NULL),(768,3,NULL,154,698,702,702,'Finished',NULL,NULL,NULL),(769,0,NULL,155,711,705,705,'Finished',NULL,NULL,NULL),(770,1,NULL,155,690,702,702,'Finished',NULL,NULL,NULL),(771,0,NULL,156,705,702,702,'Finished',NULL,NULL,NULL),(836,0,NULL,179,832,833,NULL,'Created',NULL,NULL,NULL),(837,1,NULL,179,834,835,NULL,'Created',NULL,NULL,NULL),(838,2,NULL,179,836,837,NULL,'Created',NULL,NULL,NULL),(839,3,NULL,179,838,839,NULL,'Created',NULL,NULL,NULL),(840,0,NULL,180,840,841,NULL,'Created',NULL,NULL,NULL),(841,1,NULL,180,842,843,NULL,'Created',NULL,NULL,NULL),(842,0,NULL,181,844,845,NULL,'Created',NULL,NULL,NULL),(986,5,1,201,906,909,NULL,'Created',NULL,NULL,NULL),(987,6,1,201,907,908,NULL,'Created',NULL,NULL,NULL),(988,12,2,201,906,908,NULL,'Created',NULL,NULL,NULL),(989,13,2,201,909,907,NULL,'Created',NULL,NULL,NULL),(990,19,3,201,906,907,NULL,'Created',NULL,NULL,NULL),(991,20,3,201,908,909,NULL,'Created',NULL,NULL,NULL),(992,7,1,202,910,913,NULL,'Created',NULL,NULL,NULL),(993,8,1,202,911,912,NULL,'Created',NULL,NULL,NULL),(994,14,2,202,910,912,NULL,'Created',NULL,NULL,NULL),(995,15,2,202,913,911,NULL,'Created',NULL,NULL,NULL),(996,21,3,202,910,911,NULL,'Created',NULL,NULL,NULL),(997,22,3,202,912,913,NULL,'Created',NULL,NULL,NULL),(998,1,1,203,915,916,NULL,'Created',NULL,NULL,NULL),(999,3,2,203,914,916,NULL,'Created',NULL,NULL,NULL),(1000,9,3,203,914,915,NULL,'Created',NULL,NULL,NULL),(1001,11,4,203,916,915,NULL,'Created',NULL,NULL,NULL),(1002,17,5,203,916,914,NULL,'Created',NULL,NULL,NULL),(1003,23,6,203,915,914,NULL,'Created',NULL,NULL,NULL),(1004,2,1,204,918,919,NULL,'Created',NULL,NULL,NULL),(1005,4,2,204,917,919,NULL,'Created',NULL,NULL,NULL),(1006,10,3,204,917,918,NULL,'Created',NULL,NULL,NULL),(1007,16,4,204,919,918,NULL,'Created',NULL,NULL,NULL),(1008,18,5,204,919,917,NULL,'Created',NULL,NULL,NULL),(1009,24,6,204,918,917,NULL,'Created',NULL,NULL,NULL);
/*!40000 ALTER TABLE `matches` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `players`
--

DROP TABLE IF EXISTS `players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `players` (
  `playerId` int NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  `dartname` text,
  `contactData` text,
  `interpret` text,
  `song` text,
  PRIMARY KEY (`playerId`),
  UNIQUE KEY `PlayerId` (`playerId`)
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `players`
--

LOCK TABLES `players` WRITE;
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` VALUES (7,'Ronny Rotator','Turning Tables',NULL,'Snake','Day of Solution'),(8,'Ralle','Regenbogenfo',NULL,'Scooter','How much is the fish?'),(9,'Steve\'o','Score Or Die',NULL,NULL,NULL),(10,'Sebastian','Basstart',NULL,'Driller Killer','Skaneland'),(11,'Mike','Der Leopard',NULL,'Survivor','Eye of the Tiger'),(12,'Marie','MC Hammerschmidt',NULL,'MC Hammer','Can\'t Touch This'),(13,'Glenn','Der fliegende Holländer',NULL,'Wolfgang Petri','Der Himmel brennt'),(14,'Pascal','Ralle Export',NULL,'Scooter','I like it loud'),(15,'Tom-Louis','Hawkcore',NULL,'O*****','Stunde des Siegers'),(16,'Niebe','Der Unendliche',NULL,'Klaus Doldinger','Flug auf dem Glücksdrachen'),(17,'Henning','Snooper',NULL,'Iggy Pop','The Passenger'),(18,'Sascha','Ranger',NULL,'Gigi D\'agostini','l\'amour tojours'),(19,'Tim','The Four-Eye',NULL,'Biggi','Juicy'),(20,'Chrischan','Toddler',NULL,'Fancy','Slice me nice'),(21,'David','The Caller',NULL,'The Pharside','Passing me by'),(22,'Fred','Eisentod',NULL,'Die Kassierer','Blumenkohl am Pillermann'),(23,'David','Coach-Atze 69',NULL,'Mrk Foggo\'s Skasters','Car on a train'),(24,'Markus','Zitterhand',NULL,NULL,NULL),(25,'Stefan','Vögeldi',NULL,'Joe Satriani','Shockwave Supernova'),(26,'Moritz','Krümelmännchen',NULL,'Pixies','Rock Music'),(27,'Felix','Sherrif',NULL,'The good, the Bad and the Ugly (Movie)','The Theme Song'),(28,'Basti','HansKlausFranz',NULL,'Wham!','Last Christmas'),(29,'Eric','Roter Samu',NULL,'Wolfgang Petri','Weiß der Geier'),(30,'Dennybube',NULL,NULL,NULL,NULL),(31,'Rico','Ziese',NULL,'Asphalt Indianer','Zug'),(32,'Alexander H.',NULL,NULL,NULL,NULL),(33,'Mirko','Mirconan der Barbar',NULL,'Isolierband','Keine Gnade'),(34,'Ralle','Nonkonform',NULL,NULL,NULL),(35,'Franz Z.',NULL,NULL,NULL,NULL),(36,'Fritz','Fritto',NULL,'Canned Heat','Going up to the Country'),(37,'Maximilian ','The Fall Down Boy',NULL,'Heaven Shall Burn','Black Tears'),(38,'Thomas','Captain Europe',NULL,NULL,'Bibi und Tina (Madrid Remix)'),(39,'Thomas','Total Eclipse of the Dart',NULL,'Horse Giirl','My white little Pony'),(40,'Robby','Bronko',NULL,'Eminem','Slim Shady'),(41,'Leopold','Piri Piri',NULL,'Kontra K','Erfolg ist kein Glück'),(42,'Martin','Erlero',NULL,'Backstreet Boys','Everybody'),(43,'Philipp','The Bullett',NULL,'1. FC Nürnberg','Die Legende lebt'),(44,'Moritz','Wine Mania',NULL,'Sepultura','Bloody Roots'),(45,'Vadim','Strahlemann',NULL,'\"Serienintro\"','Sailermoon'),(46,'Alex Heinze','Ronny McHammergeil',NULL,'Elsterglanz','Kaputtschaahn'),(47,'Andreas Gregor','The Tower',NULL,'Megaton Sword','Cowards Remain'),(48,'Manu','The Flying Koggsman',NULL,'Bon Jovi','Raise your Hands'),(49,'Chris','M3tl3r',NULL,'Dicks on Fire','Superbad Motherfucker'),(50,'Marcel Davideit','The Lightning',NULL,'MOP','Ante Up'),(51,'Jakob Piribauer','Siracha',NULL,NULL,NULL),(52,'Toni','Fit Tony',NULL,'Broilers','Küss meinen Ring'),(53,'Pete','Grind Peter',NULL,'Municipal Waste','Born to Party'),(54,'Max','Der Verkehrte',NULL,'Thomas & Friends','Thomas Theme'),(55,'Robert','The Omen',NULL,'Magic Affair','Omen III'),(56,'Michel','Sippi',NULL,'Mehnersmoss','Bir'),(57,'Kotze','Dartagnion',NULL,'Oxo86','Manchmal'),(58,'Martin','Martin',NULL,'Martin','Martin'),(59,'Christoph','Klaus',NULL,NULL,NULL),(60,'Robin','Fritz Berger',NULL,'Motorhead','Sympathy'),(61,'Raiko','Dagobert Dart',NULL,'Angerfist','Hoax'),(62,'Lukas','Pink Flamingo',NULL,'Angerfist','Bodybag'),(63,'Simon','Ronny\'s größter Fan',NULL,'Jpetersen','Ho Now'),(64,'Manuel Sieber',NULL,NULL,NULL,NULL),(65,'Chrischi',NULL,NULL,NULL,NULL),(66,'Mike Kautschat','Magic Mike',NULL,NULL,NULL),(67,'Wendelin Rabe',NULL,NULL,NULL,NULL),(68,'Rico Lubertsch','The Shadow',NULL,NULL,NULL),(69,'Adrian',NULL,NULL,NULL,NULL),(70,'Marius Baumgürtel',NULL,NULL,NULL,NULL),(71,'Milbe',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `players` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rounds`
--

DROP TABLE IF EXISTS `rounds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rounds` (
  `roundId` int NOT NULL AUTO_INCREMENT,
  `tournamentId` int NOT NULL,
  `name` text NOT NULL,
  `modus` enum('RoundRobin','SingleKo','DoubleKo','') NOT NULL,
  `scoring` enum('SetsOnly','LegsOnly','SetsAndLegs') NOT NULL,
  `isStarted` tinyint(1) NOT NULL DEFAULT '0',
  `isFinished` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`roundId`),
  KEY `r_tId` (`tournamentId`),
  CONSTRAINT `r_tId` FOREIGN KEY (`tournamentId`) REFERENCES `tournaments` (`tournamentId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rounds`
--

LOCK TABLES `rounds` WRITE;
/*!40000 ALTER TABLE `rounds` DISABLE KEYS */;
INSERT INTO `rounds` VALUES (6,3,'Vorgeplänkel','RoundRobin','LegsOnly',1,1),(7,3,'On Stage!','SingleKo','LegsOnly',1,1),(8,4,'Vorbands','RoundRobin','LegsOnly',1,1),(11,4,'On Stage!','SingleKo','LegsOnly',1,1),(14,5,'Verortung','RoundRobin','LegsOnly',1,1),(15,5,'On Stage!','SingleKo','LegsOnly',1,1),(16,2,'Vorrunde','RoundRobin','LegsOnly',1,1),(17,2,'On Stage!','SingleKo','LegsOnly',1,1),(18,6,'Group','RoundRobin','LegsOnly',1,1),(19,6,'aaaw','SingleKo','LegsOnly',1,1);
/*!40000 ALTER TABLE `rounds` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scores`
--

DROP TABLE IF EXISTS `scores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scores` (
  `scoreId` int NOT NULL AUTO_INCREMENT,
  `matchId` int NOT NULL,
  `p1Sets` int NOT NULL,
  `p2Sets` int NOT NULL,
  `p1Legs` int NOT NULL,
  `p2Legs` int NOT NULL,
  PRIMARY KEY (`scoreId`),
  UNIQUE KEY `matchId_2` (`matchId`),
  KEY `matchId` (`matchId`),
  CONSTRAINT `s_mId` FOREIGN KEY (`matchId`) REFERENCES `matches` (`matchId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=325 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scores`
--

LOCK TABLES `scores` WRITE;
/*!40000 ALTER TABLE `scores` DISABLE KEYS */;
INSERT INTO `scores` VALUES (4,112,0,0,0,2),(5,113,0,0,0,2),(6,118,0,0,2,1),(7,119,0,0,1,2),(8,124,0,0,1,2),(9,125,0,0,0,2),(10,130,0,0,0,2),(11,131,0,0,2,0),(12,136,0,0,1,2),(13,137,0,0,1,2),(14,142,0,0,2,0),(15,143,0,0,2,0),(16,148,0,0,0,2),(17,149,0,0,0,2),(18,154,0,0,2,0),(19,155,0,0,2,0),(20,114,0,0,1,2),(21,115,0,0,2,0),(22,120,0,0,1,2),(23,121,0,0,0,2),(24,126,0,0,0,2),(25,127,0,0,0,2),(26,132,0,0,0,2),(27,133,0,0,1,2),(28,138,0,0,2,0),(29,139,0,0,2,0),(30,144,0,0,1,2),(31,145,0,0,1,2),(32,150,0,0,0,2),(33,151,0,0,0,2),(34,156,0,0,0,2),(35,157,0,0,2,1),(36,116,0,0,1,2),(37,117,0,0,2,0),(38,122,0,0,2,1),(39,123,0,0,0,2),(40,128,0,0,0,2),(41,129,0,0,2,0),(42,134,0,0,0,2),(43,135,0,0,1,2),(44,140,0,0,2,1),(45,141,0,0,0,2),(46,146,0,0,2,1),(47,147,0,0,0,2),(48,152,0,0,0,2),(49,153,0,0,2,0),(50,158,0,0,2,0),(51,159,0,0,0,2),(52,167,0,0,2,0),(53,168,0,0,2,0),(54,169,0,0,2,1),(55,170,0,0,2,1),(56,171,0,0,2,1),(57,172,0,0,2,0),(58,173,0,0,2,0),(59,174,0,0,2,1),(60,175,0,0,2,3),(61,176,0,0,3,0),(62,177,0,0,0,3),(63,178,0,0,3,2),(64,179,0,0,3,0),(65,180,0,0,3,2),(66,181,0,0,4,1),(72,390,0,0,2,1),(73,396,0,0,2,0),(74,402,0,0,1,2),(75,408,0,0,2,0),(76,366,0,0,1,2),(77,367,0,0,2,0),(78,372,0,0,2,0),(79,373,0,0,1,2),(80,378,0,0,0,2),(81,379,0,0,2,0),(82,384,0,0,0,2),(83,385,0,0,1,2),(84,391,0,0,1,2),(85,397,0,0,2,1),(86,403,0,0,1,2),(87,409,0,0,0,2),(89,398,0,0,2,0),(90,404,0,0,0,2),(91,392,0,0,2,0),(92,368,0,0,1,2),(93,369,0,0,2,0),(94,410,0,0,2,0),(95,374,0,0,2,0),(96,375,0,0,0,2),(97,380,0,0,2,0),(98,381,0,0,0,2),(99,386,0,0,2,0),(100,387,0,0,1,2),(101,393,0,0,2,0),(102,399,0,0,2,0),(103,405,0,0,2,1),(104,411,0,0,2,0),(105,370,0,0,1,2),(106,394,0,0,0,2),(107,400,0,0,1,2),(108,406,0,0,2,0),(109,371,0,0,1,2),(110,376,0,0,0,2),(111,413,0,0,0,2),(112,377,0,0,1,2),(113,382,0,0,0,2),(114,383,0,0,1,2),(115,388,0,0,0,2),(116,389,0,0,2,0),(117,395,0,0,1,2),(118,401,0,0,2,1),(119,407,0,0,2,1),(120,412,0,0,0,2),(121,414,0,0,3,0),(122,415,0,0,3,0),(123,416,0,0,3,1),(124,417,0,0,3,1),(125,418,0,0,3,0),(126,419,0,0,3,0),(127,420,0,0,3,1),(128,421,0,0,3,0),(129,422,0,0,1,3),(130,423,0,0,0,3),(131,424,0,0,3,0),(132,425,0,0,2,3),(133,426,0,0,1,4),(134,427,0,0,4,1),(135,428,0,0,5,3),(141,573,0,0,0,2),(142,574,0,0,0,2),(143,579,0,0,2,0),(144,580,0,0,2,1),(145,585,0,0,0,2),(146,597,0,0,2,1),(147,591,0,0,2,1),(148,603,0,0,1,2),(149,609,0,0,2,0),(150,615,0,0,2,1),(151,586,0,0,1,2),(152,592,0,0,1,2),(153,598,0,0,2,1),(157,616,0,0,2,0),(158,593,0,0,0,2),(159,610,0,0,2,0),(160,604,0,0,2,1),(161,599,0,0,2,1),(162,575,0,0,0,2),(163,617,0,0,2,0),(164,611,0,0,2,0),(165,576,0,0,2,0),(166,605,0,0,2,0),(167,581,0,0,2,1),(168,582,0,0,2,0),(169,587,0,0,0,2),(170,588,0,0,1,2),(171,594,0,0,2,0),(172,600,0,0,1,2),(173,606,0,0,2,0),(174,612,0,0,2,1),(175,618,0,0,0,2),(176,595,0,0,2,0),(177,584,0,0,2,1),(178,601,0,0,2,1),(179,577,0,0,2,1),(180,607,0,0,0,2),(181,578,0,0,2,1),(182,619,0,0,0,2),(183,583,0,0,2,0),(184,589,0,0,0,2),(185,613,0,0,0,2),(186,590,0,0,2,0),(187,596,0,0,0,2),(188,602,0,0,1,2),(189,608,0,0,0,2),(190,614,0,0,0,2),(191,620,0,0,2,1),(192,621,0,0,3,0),(193,622,0,0,3,0),(194,623,0,0,3,0),(195,624,0,0,1,3),(196,626,0,0,0,3),(197,625,0,0,0,3),(198,627,0,0,1,3),(199,628,0,0,3,0),(200,629,0,0,3,2),(201,630,0,0,3,0),(202,631,0,0,1,3),(203,632,0,0,1,3),(204,633,0,0,2,4),(205,634,0,0,1,4),(206,635,0,0,1,5),(263,765,0,0,1,3),(264,766,0,0,1,3),(265,767,0,0,3,1),(266,768,0,0,2,3),(267,769,0,0,1,3),(268,770,0,0,2,3),(269,771,0,0,3,4),(270,717,0,0,0,2),(271,718,0,0,2,0),(272,719,0,0,0,2),(273,720,0,0,2,1),(274,721,0,0,2,0),(275,722,0,0,0,2),(276,723,0,0,2,0),(277,724,0,0,2,1),(278,725,0,0,2,0),(279,726,0,0,2,0),(280,727,0,0,2,0),(281,728,0,0,2,1),(282,729,0,0,2,0),(283,730,0,0,2,1),(284,731,0,0,2,0),(285,732,0,0,2,0),(286,733,0,0,2,0),(287,734,0,0,2,1),(288,735,0,0,1,2),(289,736,0,0,2,0),(290,737,0,0,2,0),(291,738,0,0,0,2),(292,739,0,0,1,2),(293,740,0,0,0,2),(294,741,0,0,2,0),(295,742,0,0,2,0),(296,743,0,0,2,0),(297,744,0,0,1,2),(298,745,0,0,1,2),(299,746,0,0,0,2),(300,747,0,0,2,0),(301,748,0,0,2,0),(302,749,0,0,2,0),(303,750,0,0,0,2),(304,751,0,0,0,2),(305,752,0,0,0,2),(306,753,0,0,2,1),(307,754,0,0,2,0),(308,755,0,0,2,0),(309,756,0,0,2,1),(310,757,0,0,0,1),(311,758,0,0,1,2),(312,759,0,0,2,0),(313,761,0,0,2,0),(314,760,0,0,2,0),(315,762,0,0,1,2),(316,763,0,0,0,2),(317,764,0,0,2,1);
/*!40000 ALTER TABLE `scores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `seed_statistics`
--

DROP TABLE IF EXISTS `seed_statistics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seed_statistics` (
  `seedStatisticsId` int NOT NULL AUTO_INCREMENT,
  `seedId` int NOT NULL,
  `matches` int DEFAULT NULL,
  `matchesWon` int DEFAULT NULL,
  `matchesLost` int DEFAULT NULL,
  `matchesTied` int DEFAULT NULL,
  `setsWon` int DEFAULT NULL,
  `setsLost` int DEFAULT NULL,
  `legsWon` int DEFAULT NULL,
  `legsLost` int DEFAULT NULL,
  PRIMARY KEY (`seedStatisticsId`),
  UNIQUE KEY `seedStatisticsId_UNIQUE` (`seedStatisticsId`),
  KEY `ss_sId_idx` (`seedId`),
  CONSTRAINT `ss_sId` FOREIGN KEY (`seedId`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=318 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `seed_statistics`
--

LOCK TABLES `seed_statistics` WRITE;
/*!40000 ALTER TABLE `seed_statistics` DISABLE KEYS */;
INSERT INTO `seed_statistics` VALUES (1,687,3,2,1,0,0,0,4,3),(2,686,3,1,2,0,0,0,2,4),(3,688,3,3,0,0,0,0,6,0),(4,689,3,0,3,0,0,0,1,6),(8,189,3,3,0,0,0,0,6,0),(9,188,3,0,3,0,0,0,2,6),(10,190,3,1,2,0,0,0,2,5),(11,191,3,2,1,0,0,0,4,3),(12,192,3,2,1,0,0,0,5,4),(13,193,3,0,3,0,0,0,1,6),(14,194,3,2,1,0,0,0,5,3),(15,195,3,2,1,0,0,0,5,3),(16,197,3,2,1,0,0,0,4,3),(17,196,3,0,3,0,0,0,1,6),(18,198,3,1,2,0,0,0,2,4),(19,199,3,3,0,0,0,0,6,0),(20,201,3,1,2,0,0,0,4,4),(21,200,3,0,3,0,0,0,0,6),(22,202,3,3,0,0,0,0,6,1),(23,203,3,2,1,0,0,0,4,3),(24,205,3,2,1,0,0,0,4,3),(25,204,3,2,1,0,0,0,5,3),(26,206,3,1,2,0,0,0,3,4),(27,207,3,1,2,0,0,0,3,5),(28,208,3,2,1,0,0,0,5,3),(29,209,3,0,3,0,0,0,1,6),(30,210,3,3,0,0,0,0,6,1),(31,211,3,1,2,0,0,0,3,5),(32,213,3,2,1,0,0,0,4,2),(33,212,3,0,3,0,0,0,0,6),(34,214,3,1,2,0,0,0,2,4),(35,215,3,3,0,0,0,0,6,0),(36,216,3,2,1,0,0,0,4,2),(37,217,3,1,2,0,0,0,2,5),(38,218,3,3,0,0,0,0,6,0),(39,219,3,0,3,0,0,0,1,6),(40,264,0,0,0,0,0,0,0,0),(41,265,0,0,0,0,0,0,0,0),(42,266,0,0,0,0,0,0,0,0),(43,267,0,0,0,0,0,0,0,0),(44,268,0,0,0,0,0,0,0,0),(45,269,0,0,0,0,0,0,0,0),(46,270,0,0,0,0,0,0,0,0),(47,271,0,0,0,0,0,0,0,0),(48,272,0,0,0,0,0,0,0,0),(49,273,0,0,0,0,0,0,0,0),(50,274,0,0,0,0,0,0,0,0),(51,275,0,0,0,0,0,0,0,0),(52,276,0,0,0,0,0,0,0,0),(53,277,0,0,0,0,0,0,0,0),(54,278,0,0,0,0,0,0,0,0),(55,279,0,0,0,0,0,0,0,0),(56,280,0,0,0,0,0,0,0,0),(57,281,0,0,0,0,0,0,0,0),(58,282,0,0,0,0,0,0,0,0),(59,283,0,0,0,0,0,0,0,0),(60,284,0,0,0,0,0,0,0,0),(61,285,0,0,0,0,0,0,0,0),(62,286,0,0,0,0,0,0,0,0),(63,287,0,0,0,0,0,0,0,0),(64,288,0,0,0,0,0,0,0,0),(65,289,0,0,0,0,0,0,0,0),(66,290,0,0,0,0,0,0,0,0),(67,291,0,0,0,0,0,0,0,0),(68,292,0,0,0,0,0,0,0,0),(69,293,0,0,0,0,0,0,0,0),(70,295,3,2,1,0,0,0,5,3),(71,294,3,0,3,0,0,0,3,6),(72,296,3,3,0,0,0,0,6,2),(73,297,3,1,2,0,0,0,2,5),(74,298,3,2,1,0,0,0,4,2),(75,299,3,0,3,0,0,0,1,6),(76,300,3,1,2,0,0,0,3,5),(77,301,3,3,0,0,0,0,6,1),(78,303,3,1,2,0,0,0,3,4),(79,302,3,1,2,0,0,0,2,4),(80,304,3,2,1,0,0,0,4,3),(81,305,3,2,1,0,0,0,4,2),(82,307,3,2,1,0,0,0,5,2),(83,306,3,1,2,0,0,0,2,4),(84,308,3,0,3,0,0,0,1,6),(85,309,3,3,0,0,0,0,6,2),(86,310,4,4,0,0,0,0,8,2),(87,311,4,0,4,0,0,0,3,8),(88,312,4,2,2,0,0,0,4,5),(89,314,4,3,1,0,0,0,7,3),(90,315,4,2,2,0,0,0,4,6),(91,316,4,1,3,0,0,0,4,6),(92,319,4,2,2,0,0,0,6,6),(93,318,4,0,4,0,0,0,2,8),(94,320,4,4,0,0,0,0,8,2),(95,322,4,4,0,0,0,0,8,0),(96,323,4,0,4,0,0,0,0,8),(97,324,4,2,2,0,0,0,4,4),(98,490,0,0,0,0,0,0,0,0),(99,491,0,0,0,0,0,0,0,0),(100,492,0,0,0,0,0,0,0,0),(101,493,0,0,0,0,0,0,0,0),(102,494,0,0,0,0,0,0,0,0),(103,495,0,0,0,0,0,0,0,0),(104,496,0,0,0,0,0,0,0,0),(105,497,0,0,0,0,0,0,0,0),(106,498,0,0,0,0,0,0,0,0),(107,499,0,0,0,0,0,0,0,0),(108,500,0,0,0,0,0,0,0,0),(109,501,0,0,0,0,0,0,0,0),(110,502,0,0,0,0,0,0,0,0),(111,503,0,0,0,0,0,0,0,0),(112,504,0,0,0,0,0,0,0,0),(113,505,0,0,0,0,0,0,0,0),(114,506,0,0,0,0,0,0,0,0),(115,507,0,0,0,0,0,0,0,0),(116,508,0,0,0,0,0,0,0,0),(117,509,0,0,0,0,0,0,0,0),(118,510,0,0,0,0,0,0,0,0),(119,511,0,0,0,0,0,0,0,0),(120,512,0,0,0,0,0,0,0,0),(121,513,0,0,0,0,0,0,0,0),(122,514,0,0,0,0,0,0,0,0),(123,515,0,0,0,0,0,0,0,0),(124,516,0,0,0,0,0,0,0,0),(125,517,0,0,0,0,0,0,0,0),(126,518,0,0,0,0,0,0,0,0),(127,519,0,0,0,0,0,0,0,0),(128,565,3,3,0,0,0,0,6,1),(129,564,3,1,2,0,0,0,2,5),(130,566,3,1,2,0,0,0,3,4),(131,567,3,1,2,0,0,0,3,4),(132,568,3,3,0,0,0,0,6,1),(133,569,3,2,1,0,0,0,4,3),(134,570,3,1,2,0,0,0,4,5),(135,571,3,0,3,0,0,0,1,6),(136,573,3,2,1,0,0,0,5,2),(137,572,3,0,3,0,0,0,0,6),(138,574,3,1,2,0,0,0,3,4),(139,575,3,3,0,0,0,0,6,2),(140,576,4,2,2,0,0,0,4,5),(141,577,4,0,4,0,0,0,2,8),(142,578,4,4,0,0,0,0,8,1),(143,580,4,3,1,0,0,0,7,5),(144,581,4,2,2,0,0,0,6,6),(145,582,4,1,3,0,0,0,5,7),(146,585,4,2,2,0,0,0,4,6),(147,584,4,3,1,0,0,0,7,2),(148,586,4,1,3,0,0,0,3,6),(149,588,4,4,0,0,0,0,8,0),(150,589,4,1,3,0,0,0,3,6),(151,590,4,1,3,0,0,0,2,7),(152,592,4,3,1,0,0,0,7,3),(153,593,4,3,1,0,0,0,7,3),(154,594,4,0,4,0,0,0,0,8),(155,656,0,0,0,0,0,0,0,0),(156,657,0,0,0,0,0,0,0,0),(157,658,0,0,0,0,0,0,0,0),(158,659,0,0,0,0,0,0,0,0),(159,660,0,0,0,0,0,0,0,0),(160,661,0,0,0,0,0,0,0,0),(161,662,0,0,0,0,0,0,0,0),(162,663,0,0,0,0,0,0,0,0),(163,664,0,0,0,0,0,0,0,0),(164,665,0,0,0,0,0,0,0,0),(165,666,0,0,0,0,0,0,0,0),(166,667,0,0,0,0,0,0,0,0),(167,668,0,0,0,0,0,0,0,0),(168,669,0,0,0,0,0,0,0,0),(169,670,0,0,0,0,0,0,0,0),(170,671,0,0,0,0,0,0,0,0),(171,672,0,0,0,0,0,0,0,0),(172,673,0,0,0,0,0,0,0,0),(173,674,0,0,0,0,0,0,0,0),(174,675,0,0,0,0,0,0,0,0),(175,676,0,0,0,0,0,0,0,0),(176,677,0,0,0,0,0,0,0,0),(177,678,0,0,0,0,0,0,0,0),(178,679,0,0,0,0,0,0,0,0),(179,680,0,0,0,0,0,0,0,0),(180,681,0,0,0,0,0,0,0,0),(181,682,0,0,0,0,0,0,0,0),(182,683,0,0,0,0,0,0,0,0),(183,684,0,0,0,0,0,0,0,0),(184,685,0,0,0,0,0,0,0,0),(185,690,3,3,0,0,0,0,6,0),(186,691,3,2,1,0,0,0,4,3),(187,692,3,1,2,0,0,0,3,5),(188,693,3,0,3,0,0,0,1,6),(189,694,3,3,0,0,0,0,6,0),(190,695,3,2,1,0,0,0,4,3),(191,696,3,1,2,0,0,0,3,5),(192,697,3,0,3,0,0,0,1,6),(193,699,4,3,1,0,0,0,6,3),(194,698,4,3,1,0,0,0,7,3),(195,700,4,0,4,0,0,0,1,8),(196,702,4,4,0,0,0,0,8,1),(197,703,4,2,2,0,0,0,4,5),(198,704,4,0,4,0,0,0,2,8),(199,705,4,4,0,0,0,0,8,0),(200,706,4,2,2,0,0,0,4,4),(201,707,4,0,4,0,0,0,0,8),(202,708,4,4,0,0,0,0,7,2),(203,709,4,1,3,0,0,0,5,6),(204,710,4,1,3,0,0,0,2,6),(205,711,4,3,1,0,0,0,7,2),(206,712,4,3,1,0,0,0,6,4),(207,713,4,0,4,0,0,0,1,8),(208,756,0,0,0,0,0,0,0,0),(209,757,0,0,0,0,0,0,0,0),(210,758,0,0,0,0,0,0,0,0),(211,759,0,0,0,0,0,0,0,0),(212,760,0,0,0,0,0,0,0,0),(213,761,0,0,0,0,0,0,0,0),(214,762,0,0,0,0,0,0,0,0),(215,763,0,0,0,0,0,0,0,0),(216,764,0,0,0,0,0,0,0,0),(217,765,0,0,0,0,0,0,0,0),(218,766,0,0,0,0,0,0,0,0),(219,767,0,0,0,0,0,0,0,0),(220,768,0,0,0,0,0,0,0,0),(221,769,0,0,0,0,0,0,0,0),(299,906,0,0,0,0,0,0,0,0),(300,907,0,0,0,0,0,0,0,0),(301,908,0,0,0,0,0,0,0,0),(302,909,0,0,0,0,0,0,0,0),(303,910,0,0,0,0,0,0,0,0),(304,911,0,0,0,0,0,0,0,0),(305,912,0,0,0,0,0,0,0,0),(306,913,0,0,0,0,0,0,0,0),(307,914,0,0,0,0,0,0,0,0),(308,915,0,0,0,0,0,0,0,0),(309,916,0,0,0,0,0,0,0,0),(310,917,0,0,0,0,0,0,0,0),(311,918,0,0,0,0,0,0,0,0),(312,919,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `seed_statistics` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `seeds`
--

DROP TABLE IF EXISTS `seeds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seeds` (
  `seedId` int NOT NULL AUTO_INCREMENT,
  `groupId` int NOT NULL,
  `seedNr` int NOT NULL,
  `seedRank` int NOT NULL DEFAULT '0',
  `seedName` text,
  `ancestorMatchId` int DEFAULT NULL,
  `seedscol` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`seedId`),
  UNIQUE KEY `seedId_UNIQUE` (`seedId`),
  KEY `groupId` (`groupId`),
  KEY `ancesterMatchId` (`ancestorMatchId`),
  CONSTRAINT `s_amId` FOREIGN KEY (`ancestorMatchId`) REFERENCES `matches` (`matchId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `s_gId` FOREIGN KEY (`groupId`) REFERENCES `groups` (`groupId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=925 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `seeds`
--

LOCK TABLES `seeds` WRITE;
/*!40000 ALTER TABLE `seeds` DISABLE KEYS */;
INSERT INTO `seeds` VALUES (188,55,1,4,'Seed #1',NULL,NULL),(189,55,2,1,'The Fall Down Boy',NULL,NULL),(190,55,3,3,'Seed #3',NULL,NULL),(191,55,4,2,'Ralle Export',NULL,NULL),(192,56,5,3,'Seed #5',NULL,NULL),(193,56,6,4,'Seed #6',NULL,NULL),(194,56,7,2,'Captain Europe',NULL,NULL),(195,56,8,1,'Piri Piri',NULL,NULL),(196,57,9,4,'Seed #9',NULL,NULL),(197,57,10,2,'The Bullett',NULL,NULL),(198,57,11,3,'Seed #11',NULL,NULL),(199,57,12,1,'Wine Mania',NULL,NULL),(200,58,13,4,'Seed #13',NULL,NULL),(201,58,14,3,'Seed #14',NULL,NULL),(202,58,15,1,'Hawkcore',NULL,NULL),(203,58,16,2,'Der Unendliche',NULL,NULL),(204,59,17,1,'Sherrif',NULL,NULL),(205,59,18,2,'Regenbogenfo',NULL,NULL),(206,59,19,3,'Seed #19',NULL,NULL),(207,59,20,4,'Seed #20',NULL,NULL),(208,60,21,2,'Coach-Atze 69',NULL,NULL),(209,60,22,4,'Seed #22',NULL,NULL),(210,60,23,1,'Ziese',NULL,NULL),(211,60,24,3,'Seed #24',NULL,NULL),(212,61,25,4,'Seed #25',NULL,NULL),(213,61,26,2,'Total Eclipse of the Dart',NULL,NULL),(214,61,27,3,'Seed #27',NULL,NULL),(215,61,28,1,'The Smoking Rüdiger',NULL,NULL),(216,62,29,2,'The Caller',NULL,NULL),(217,62,30,3,'Seed #30',NULL,NULL),(218,62,31,1,'HansKlausFranz',NULL,NULL),(219,62,32,4,'Seed #32',NULL,NULL),(264,70,0,1,'Please Run Script',NULL,NULL),(265,70,1,1,'Please Run Script',NULL,NULL),(266,70,2,1,'Please Run Script',NULL,NULL),(267,70,3,1,'Please Run Script',NULL,NULL),(268,70,4,1,'Please Run Script',NULL,NULL),(269,70,5,1,'Please Run Script',NULL,NULL),(270,70,6,1,'Please Run Script',NULL,NULL),(271,70,7,1,'Please Run Script',NULL,NULL),(272,70,8,1,'Please Run Script',NULL,NULL),(273,70,9,1,'Please Run Script',NULL,NULL),(274,70,10,1,'Please Run Script',NULL,NULL),(275,70,11,1,'Please Run Script',NULL,NULL),(276,70,12,1,'Please Run Script',NULL,NULL),(277,70,13,1,'Please Run Script',NULL,NULL),(278,70,14,1,'Please Run Script',NULL,NULL),(279,70,15,1,'Please Run Script',NULL,NULL),(280,71,0,1,'The Fall Down Boy | The Caller',NULL,NULL),(281,71,1,1,'Piri Piri | Total Eclipse of the Dart',NULL,NULL),(282,71,2,1,'Wine Mania | Coach-Atze 69',NULL,NULL),(283,71,3,1,'Hawkcore | Regenbogenfo',NULL,NULL),(284,71,4,1,'Sherrif | Der Unendliche',NULL,NULL),(285,71,5,1,'Ziese | The Bullett',NULL,NULL),(286,71,6,1,'The Smoking Rüdiger | Captain Europe',NULL,NULL),(287,71,7,1,'HansKlausFranz | Ralle Export',NULL,NULL),(288,72,0,1,'The Fall Down Boy | Piri Piri',175,NULL),(289,72,0,1,'Wine Mania | Hawkcore',176,NULL),(290,72,0,1,'Sherrif | Ziese',177,NULL),(291,72,0,1,'The Smoking Rüdiger | HansKlausFranz',178,NULL),(292,73,0,1,'Piri Piri | Wine Mania',179,NULL),(293,73,0,1,'Ziese | The Smoking Rüdiger',180,NULL),(294,74,1,4,'Seed #1',NULL,NULL),(295,74,2,2,'Ziese',NULL,NULL),(296,74,3,1,'Turning Tables',NULL,NULL),(297,74,4,3,'Seed #4',NULL,NULL),(298,75,5,2,'Siracha',NULL,NULL),(299,75,6,4,'Seed #6',NULL,NULL),(300,75,7,3,'Seed #7',NULL,NULL),(301,75,8,1,'The Fall Down Boy',NULL,NULL),(302,76,9,4,'Seed #9',NULL,NULL),(303,76,10,3,'Seed #10',NULL,NULL),(304,76,11,2,'M3tl3r',NULL,NULL),(305,76,12,1,'Ralle Export',NULL,NULL),(306,77,13,3,'Seed #13',NULL,NULL),(307,77,14,2,'Toddler',NULL,NULL),(308,77,15,4,'Seed #15',NULL,NULL),(309,77,16,1,'The Tower',NULL,NULL),(310,78,17,1,'Piri Piri',NULL,NULL),(311,78,18,3,'Seed #18',NULL,NULL),(312,78,19,2,'Zitterhand',NULL,NULL),(314,79,20,1,'Basstart',NULL,NULL),(315,79,21,2,'Krümelmännchen',NULL,NULL),(316,79,22,3,'Seed #22',NULL,NULL),(318,80,23,3,'Seed #23',NULL,NULL),(319,80,24,2,'Roter Samu',NULL,NULL),(320,80,25,1,'Coach-Atze 69',NULL,NULL),(322,81,26,1,'The Lightning',NULL,NULL),(323,81,27,3,'Seed #27',NULL,NULL),(324,81,28,2,'Ronny McHammergeil',NULL,NULL),(490,105,0,1,'Please Run Script',NULL,NULL),(491,105,1,1,'Please Run Script',NULL,NULL),(492,105,2,1,'Please Run Script',NULL,NULL),(493,105,3,1,'Please Run Script',NULL,NULL),(494,105,4,1,'Please Run Script',NULL,NULL),(495,105,5,1,'Please Run Script',NULL,NULL),(496,105,6,1,'Please Run Script',NULL,NULL),(497,105,7,1,'Please Run Script',NULL,NULL),(498,105,8,1,'Please Run Script',NULL,NULL),(499,105,9,1,'Please Run Script',NULL,NULL),(500,105,10,1,'Please Run Script',NULL,NULL),(501,105,11,1,'Please Run Script',NULL,NULL),(502,105,12,1,'Please Run Script',NULL,NULL),(503,105,13,1,'Please Run Script',NULL,NULL),(504,105,14,1,'Please Run Script',NULL,NULL),(505,105,15,1,'Please Run Script',NULL,NULL),(506,106,0,1,'Turning Tables | Ronny McHammergeil',NULL,NULL),(507,106,1,1,'The Fall Down Boy | Roter Samu',NULL,NULL),(508,106,2,1,'Ralle Export | Krümelmännchen',NULL,NULL),(509,106,3,1,'The Tower | Zitterhand',NULL,NULL),(510,106,4,1,'Piri Piri | Toddler',NULL,NULL),(511,106,5,1,'Basstart | M3tl3r',NULL,NULL),(512,106,6,1,'Coach-Atze 69 | Siracha',NULL,NULL),(513,106,7,1,'The Lightning | Ziese',NULL,NULL),(514,107,0,1,'Turning Tables | The Fall Down Boy',422,NULL),(515,107,0,1,'Ralle Export | The Tower',423,NULL),(516,107,0,1,'Piri Piri | Basstart',424,NULL),(517,107,0,1,'Coach-Atze 69 | The Lightning',425,NULL),(518,108,0,1,'The Fall Down Boy | The Tower',426,NULL),(519,108,0,1,'Piri Piri | The Lightning',427,NULL),(564,117,1,4,'Seed #1',NULL,NULL),(565,117,2,1,'Piri Piri',NULL,NULL),(566,117,3,3,'Seed #3',NULL,NULL),(567,117,4,2,'Klaus',NULL,NULL),(568,118,5,1,'Dartagnion',NULL,NULL),(569,118,6,2,'The Tower',NULL,NULL),(570,118,7,3,'Seed #7',NULL,NULL),(571,118,8,4,'Seed #8',NULL,NULL),(572,119,9,4,'Seed #9',NULL,NULL),(573,119,10,2,'Martin',NULL,NULL),(574,119,11,3,'Seed #11',NULL,NULL),(575,119,12,1,'Ronny\'s größter Fan',NULL,NULL),(576,120,13,2,'Turning Tables',NULL,NULL),(577,120,14,3,'Seed #14',NULL,NULL),(578,120,15,1,'Der Verkehrte',NULL,NULL),(580,121,17,1,'The Lightning',NULL,NULL),(581,121,18,2,'Wine Mania',NULL,NULL),(582,121,19,3,'Seed #19',NULL,NULL),(584,122,21,1,'Total Eclipse of the Dart',NULL,NULL),(585,122,22,2,'Sippi',NULL,NULL),(586,122,23,3,'Seed #23',NULL,NULL),(588,123,25,1,'Pink Flamingo',NULL,NULL),(589,123,26,2,'Eisentod',NULL,NULL),(590,123,27,3,'Seed #27',NULL,NULL),(592,124,29,1,'The Fall Down Boy',NULL,NULL),(593,124,30,1,'The Omen',NULL,NULL),(594,124,31,3,'Seed #31',NULL,NULL),(656,133,0,1,'Please Run Script',NULL,NULL),(657,133,1,1,'Please Run Script',NULL,NULL),(658,133,2,1,'Please Run Script',NULL,NULL),(659,133,3,1,'Please Run Script',NULL,NULL),(660,133,4,1,'Please Run Script',NULL,NULL),(661,133,5,1,'Please Run Script',NULL,NULL),(662,133,6,1,'Please Run Script',NULL,NULL),(663,133,7,1,'Please Run Script',NULL,NULL),(664,133,8,1,'Please Run Script',NULL,NULL),(665,133,9,1,'Please Run Script',NULL,NULL),(666,133,10,1,'Please Run Script',NULL,NULL),(667,133,11,1,'Please Run Script',NULL,NULL),(668,133,12,1,'Please Run Script',NULL,NULL),(669,133,13,1,'Please Run Script',NULL,NULL),(670,133,14,1,'Please Run Script',NULL,NULL),(671,133,15,1,'Please Run Script',NULL,NULL),(672,134,0,1,'Piri Piri | The Omen',NULL,NULL),(673,134,1,1,'Dartagnion | Eisentod',NULL,NULL),(674,134,2,1,'Ronny\'s größter Fan | Sippi',NULL,NULL),(675,134,3,1,'Der Verkehrte | Wine Mania',NULL,NULL),(676,134,4,1,'The Lightning | Turning Tables',NULL,NULL),(677,134,5,1,'Total Eclipse of the Dart | Martin',NULL,NULL),(678,134,6,1,'Pink Flamingo | The Tower',NULL,NULL),(679,134,7,1,'The Fall Down Boy | Klaus',NULL,NULL),(680,135,0,1,'Piri Piri | Dartagnion',629,NULL),(681,135,0,1,'Ronny\'s größter Fan | Wine Mania',630,NULL),(682,135,0,1,'Turning Tables | Martin',631,NULL),(683,135,0,1,'The Tower | The Fall Down Boy',632,NULL),(684,136,0,1,'Piri Piri | Ronny\'s größter Fan',633,NULL),(685,136,0,1,'Martin | The Fall Down Boy',634,NULL),(686,137,1,3,'The Flying Koggsman',NULL,NULL),(687,137,2,2,'Seed #2',NULL,NULL),(688,137,3,1,'Seed #3',NULL,NULL),(689,137,4,4,'Seed #4',NULL,NULL),(690,138,5,1,'Ralle Export',NULL,NULL),(691,138,6,2,'Seed #6',NULL,NULL),(692,138,7,3,'Seed #7',NULL,NULL),(693,138,8,4,'Seed #8',NULL,NULL),(694,139,9,1,'Turning Tables',NULL,NULL),(695,139,10,2,'Seed #10',NULL,NULL),(696,139,11,3,'Seed #11',NULL,NULL),(697,139,12,4,'Seed #12',NULL,NULL),(698,140,13,1,'The Lightning',NULL,NULL),(699,140,14,2,'Seed #14',NULL,NULL),(700,140,15,3,'Seed #15',NULL,NULL),(702,141,16,1,'Magic Mike',NULL,NULL),(703,141,17,2,'Seed #17',NULL,NULL),(704,141,18,3,'Seed #18',NULL,NULL),(705,142,19,1,'The Shadow',NULL,NULL),(706,142,20,2,'Seed #20',NULL,NULL),(707,142,21,3,'Seed #21',NULL,NULL),(708,143,22,1,'Der Unendliche',NULL,NULL),(709,143,23,2,'Seed #23',NULL,NULL),(710,143,24,3,'Seed #24',NULL,NULL),(711,144,25,1,'Coach-Atze 69',NULL,NULL),(712,144,26,2,'Seed #26',NULL,NULL),(713,144,27,3,'Seed #27',NULL,NULL),(756,154,0,1,'Please Run Script',NULL,NULL),(757,154,1,1,'Please Run Script',NULL,NULL),(758,154,2,1,'Please Run Script',NULL,NULL),(759,154,3,1,'Please Run Script',NULL,NULL),(760,154,4,1,'Please Run Script',NULL,NULL),(761,154,5,1,'Please Run Script',NULL,NULL),(762,154,6,1,'Please Run Script',NULL,NULL),(763,154,7,1,'Please Run Script',NULL,NULL),(764,155,0,1,'Please Run Script',NULL,NULL),(765,155,1,1,'Please Run Script',NULL,NULL),(766,155,2,1,'Please Run Script',NULL,NULL),(767,155,3,1,'Please Run Script',NULL,NULL),(768,156,0,1,'Coach-Atze 69 | The Shadow',769,NULL),(769,156,0,1,'Ralle Export | Magic Mike',770,NULL),(832,179,0,0,'Please Run Script',NULL,NULL),(833,179,1,0,'Please Run Script',NULL,NULL),(834,179,2,0,'Please Run Script',NULL,NULL),(835,179,3,0,'Please Run Script',NULL,NULL),(836,179,4,0,'Please Run Script',NULL,NULL),(837,179,5,0,'Please Run Script',NULL,NULL),(838,179,6,0,'Please Run Script',NULL,NULL),(839,179,7,0,'Please Run Script',NULL,NULL),(840,180,0,0,'Please Run Script',NULL,NULL),(841,180,1,0,'Please Run Script',NULL,NULL),(842,180,2,0,'Please Run Script',NULL,NULL),(843,180,3,0,'Please Run Script',NULL,NULL),(844,181,0,0,'Please Run Script',840,NULL),(845,181,0,0,'Please Run Script',841,NULL),(906,201,1,0,'Seed #1',NULL,NULL),(907,201,2,0,'Seed #2',NULL,NULL),(908,201,3,0,'Seed #3',NULL,NULL),(909,201,4,0,'Seed #4',NULL,NULL),(910,202,5,0,'Seed #5',NULL,NULL),(911,202,6,0,'Seed #6',NULL,NULL),(912,202,7,0,'Seed #7',NULL,NULL),(913,202,8,0,'Seed #8',NULL,NULL),(914,203,9,0,'Seed #9',NULL,NULL),(915,203,10,0,'Seed #10',NULL,NULL),(916,203,11,0,'Seed #11',NULL,NULL),(917,204,12,0,'Seed #12',NULL,NULL),(918,204,13,0,'Seed #13',NULL,NULL),(919,204,14,0,'Seed #14',NULL,NULL);
/*!40000 ALTER TABLE `seeds` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tournaments`
--

DROP TABLE IF EXISTS `tournaments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tournaments` (
  `tournamentId` int NOT NULL AUTO_INCREMENT,
  `name` text,
  `starttime` datetime DEFAULT NULL,
  PRIMARY KEY (`tournamentId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tournaments`
--

LOCK TABLES `tournaments` WRITE;
/*!40000 ALTER TABLE `tournaments` DISABLE KEYS */;
INSERT INTO `tournaments` VALUES (2,'Chemodarts Vol. 1',NULL),(3,'Chemodarts Vol. 2','2022-11-27 14:00:00'),(4,'Chemodarts Vol. 3','2023-04-02 12:00:00'),(5,'Chemodarts Vol. 4','2023-09-17 12:00:00'),(6,'Boardbau Gutz','2023-09-22 20:21:00');
/*!40000 ALTER TABLE `tournaments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `venues`
--

DROP TABLE IF EXISTS `venues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `venues` (
  `venueId` int NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  PRIMARY KEY (`venueId`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `venues`
--

LOCK TABLES `venues` WRITE;
/*!40000 ALTER TABLE `venues` DISABLE KEYS */;
INSERT INTO `venues` VALUES (5,'Board 1'),(6,'Board 2'),(7,'Board 3'),(8,'Board 4'),(9,'Board 5');
/*!40000 ALTER TABLE `venues` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-09-29 14:02:45
