-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: empty_db
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.11-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `account_balance_storage`
--

DROP TABLE IF EXISTS `account_balance_storage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_balance_storage` (
  `storageID` int(20) NOT NULL AUTO_INCREMENT,
  `account_ID` int(20) NOT NULL,
  `currentBalance` double NOT NULL,
  `createdDate` timestamp NOT NULL DEFAULT current_timestamp(),
  `lastUpdatedDate` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`storageID`),
  UNIQUE KEY `unique_storage_record_for_account_id` (`account_ID`),
  CONSTRAINT `account_balance_storage_ibfk_1` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_balance_storage`
--

LOCK TABLES `account_balance_storage` WRITE;
/*!40000 ALTER TABLE `account_balance_storage` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_balance_storage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_balance_storage_history`
--

DROP TABLE IF EXISTS `account_balance_storage_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_balance_storage_history` (
  `histID` int(20) NOT NULL AUTO_INCREMENT,
  `storage_ID` int(20) NOT NULL,
  `actionSource` varchar(100) DEFAULT NULL,
  `actionDescription` varchar(200) DEFAULT NULL,
  `databaseComponentType` varchar(20) DEFAULT NULL,
  `databaseOperation` varchar(20) DEFAULT NULL,
  `valueTriggeringUpdate` double DEFAULT NULL,
  `oldBalanceValue` double DEFAULT NULL,
  `newBalanceValue` double DEFAULT NULL,
  `actionTimestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`histID`),
  KEY `storage_ID` (`storage_ID`),
  CONSTRAINT `account_balance_storage_history_ibfk_1` FOREIGN KEY (`storage_ID`) REFERENCES `account_balance_storage` (`storageID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_balance_storage_history`
--

LOCK TABLES `account_balance_storage_history` WRITE;
/*!40000 ALTER TABLE `account_balance_storage_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_balance_storage_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_types`
--

DROP TABLE IF EXISTS `account_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_types` (
  `typeID` int(10) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_types`
--

LOCK TABLES `account_types` WRITE;
/*!40000 ALTER TABLE `account_types` DISABLE KEYS */;
INSERT INTO `account_types` VALUES (1,'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'),(2,'USER_DEFINED-CUSTOM_SAVING_ACCOUNT'),(3,'USER_DEFINED-CUSTOM_INVESTMENT_ACCOUNT');
/*!40000 ALTER TABLE `account_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounts` (
  `accountID` int(10) NOT NULL AUTO_INCREMENT,
  `accountName` varchar(50) DEFAULT NULL,
  `accountNumber` varchar(34) DEFAULT NULL,
  `user_ID` int(10) NOT NULL,
  `type_ID` int(10) NOT NULL,
  `bank_ID` int(10) NOT NULL,
  `currency_ID` int(10) NOT NULL,
  `creationDate` date DEFAULT NULL,
  PRIMARY KEY (`accountID`),
  UNIQUE KEY `accountName` (`accountName`,`user_ID`),
  UNIQUE KEY `accountName_2` (`accountName`,`user_ID`,`type_ID`,`bank_ID`),
  KEY `user_ID` (`user_ID`),
  KEY `type_ID` (`type_ID`),
  KEY `bank_ID` (`bank_ID`),
  KEY `currency` (`currency_ID`),
  CONSTRAINT `accounts_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `accounts_ibfk_2` FOREIGN KEY (`type_ID`) REFERENCES `account_types` (`typeID`) ON UPDATE CASCADE,
  CONSTRAINT `accounts_ibfk_3` FOREIGN KEY (`bank_ID`) REFERENCES `banks` (`bankID`) ON UPDATE CASCADE,
  CONSTRAINT `accounts_ibfk_4` FOREIGN KEY (`currency_ID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `banks`
--

DROP TABLE IF EXISTS `banks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `banks` (
  `bankID` int(10) NOT NULL AUTO_INCREMENT,
  `bankName` varchar(50) NOT NULL,
  PRIMARY KEY (`bankID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `banks`
--

LOCK TABLES `banks` WRITE;
/*!40000 ALTER TABLE `banks` DISABLE KEYS */;
/*!40000 ALTER TABLE `banks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `broker_accounts`
--

DROP TABLE IF EXISTS `broker_accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `broker_accounts` (
  `account_ID` int(20) NOT NULL,
  `broker_ID` int(20) NOT NULL,
  UNIQUE KEY `account_ID` (`account_ID`),
  KEY `broker_ID` (`broker_ID`),
  CONSTRAINT `broker_accounts_ibfk_5` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE,
  CONSTRAINT `broker_accounts_ibfk_6` FOREIGN KEY (`broker_ID`) REFERENCES `brokers` (`brokerID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `broker_accounts`
--

LOCK TABLES `broker_accounts` WRITE;
/*!40000 ALTER TABLE `broker_accounts` DISABLE KEYS */;
/*!40000 ALTER TABLE `broker_accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `broker_portfolio_transfers`
--

DROP TABLE IF EXISTS `broker_portfolio_transfers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `broker_portfolio_transfers` (
  `transferID` int(20) NOT NULL AUTO_INCREMENT,
  `oldBroker` int(20) NOT NULL,
  `newBroker` int(20) NOT NULL,
  `transferName` varchar(50) DEFAULT NULL,
  `transferObservations` varchar(150) DEFAULT NULL,
  `transferDate` date DEFAULT NULL,
  PRIMARY KEY (`transferID`),
  KEY `oldBroker` (`oldBroker`),
  KEY `newBroker` (`newBroker`),
  CONSTRAINT `broker_portfolio_transfers_ibfk_1` FOREIGN KEY (`oldBroker`) REFERENCES `brokers` (`brokerID`) ON UPDATE CASCADE,
  CONSTRAINT `broker_portfolio_transfers_ibfk_2` FOREIGN KEY (`newBroker`) REFERENCES `brokers` (`brokerID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `broker_portfolio_transfers`
--

LOCK TABLES `broker_portfolio_transfers` WRITE;
/*!40000 ALTER TABLE `broker_portfolio_transfers` DISABLE KEYS */;
/*!40000 ALTER TABLE `broker_portfolio_transfers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `brokers`
--

DROP TABLE IF EXISTS `brokers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `brokers` (
  `brokerID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `country_ID` int(20) NOT NULL,
  PRIMARY KEY (`brokerID`),
  KEY `country_ID` (`country_ID`),
  CONSTRAINT `brokers_ibfk_1` FOREIGN KEY (`country_ID`) REFERENCES `countries` (`countryID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `brokers`
--

LOCK TABLES `brokers` WRITE;
/*!40000 ALTER TABLE `brokers` DISABLE KEYS */;
/*!40000 ALTER TABLE `brokers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `budget_plans`
--

DROP TABLE IF EXISTS `budget_plans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `budget_plans` (
  `planID` int(10) NOT NULL AUTO_INCREMENT,
  `user_ID` int(10) NOT NULL,
  `planName` varchar(50) NOT NULL,
  `expenseLimit` int(2) NOT NULL,
  `debtLimit` int(2) NOT NULL,
  `savingLimit` int(2) NOT NULL,
  `planType` int(10) NOT NULL,
  `hasAlarm` tinyint(1) NOT NULL,
  `thresholdPercentage` int(2) NOT NULL,
  `startDate` date NOT NULL,
  `endDate` date NOT NULL,
  PRIMARY KEY (`planID`),
  UNIQUE KEY `planName` (`planName`),
  KEY `user_ID` (`user_ID`),
  KEY `planType` (`planType`),
  CONSTRAINT `budget_plans_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  CONSTRAINT `budget_plans_ibfk_2` FOREIGN KEY (`planType`) REFERENCES `plan_types` (`typeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `budget_plans`
--

LOCK TABLES `budget_plans` WRITE;
/*!40000 ALTER TABLE `budget_plans` DISABLE KEYS */;
/*!40000 ALTER TABLE `budget_plans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `countries`
--

DROP TABLE IF EXISTS `countries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `countries` (
  `countryID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `countryCode` varchar(2) DEFAULT NULL,
  PRIMARY KEY (`countryID`)
) ENGINE=InnoDB AUTO_INCREMENT=246 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `countries`
--

LOCK TABLES `countries` WRITE;
/*!40000 ALTER TABLE `countries` DISABLE KEYS */;
INSERT INTO `countries` VALUES (1,'Andorra','AD'),(2,'United Arab Emirates','AE'),(3,'Afghanistan','AF'),(4,'Antigua and Barbuda','AG'),(5,'Anguilla','AI'),(6,'Albania','AL'),(7,'Armenia','AM'),(8,'Netherlands Antilles','AN'),(9,'Angola','AO'),(10,'Antarctica','AQ'),(11,'Argentina','AR'),(12,'American Samoa','AS'),(13,'Austria','AT'),(14,'Australia','AU'),(15,'Aruba','AW'),(16,'Azerbaijan','AZ'),(17,'Bosnia and Herzegovina','BA'),(18,'Barbados','BB'),(19,'Bangladesh','BD'),(20,'Belgium','BE'),(21,'Burkina Faso','BF'),(22,'Bulgaria','BG'),(23,'Bahrain','BH'),(24,'Burundi','BI'),(25,'Benin','BJ'),(26,'Bermuda','BM'),(27,'Brunei','BN'),(28,'Bolivia','BO'),(29,'Brazil','BR'),(30,'Bahamas','BS'),(31,'Bhutan','BT'),(32,'Bouvet Island','BV'),(33,'Botswana','BW'),(34,'Belarus','BY'),(35,'Belize','BZ'),(36,'Canada','CA'),(37,'Cocos [Keeling] Islands','CC'),(38,'Congo [DRC]','CD'),(39,'Central African Republic','CF'),(40,'Congo [Republic]','CG'),(41,'Switzerland','CH'),(42,'Côte d\'Ivoire','CI'),(43,'Cook Islands','CK'),(44,'Chile','CL'),(45,'Cameroon','CM'),(46,'China','CN'),(47,'Colombia','CO'),(48,'Costa Rica','CR'),(49,'Cuba','CU'),(50,'Cape Verde','CV'),(51,'Christmas Island','CX'),(52,'Cyprus','CY'),(53,'Czech Republic','CZ'),(54,'Germany','DE'),(55,'Djibouti','DJ'),(56,'Denmark','DK'),(57,'Dominica','DM'),(58,'Dominican Republic','DO'),(59,'Algeria','DZ'),(60,'Ecuador','EC'),(61,'Estonia','EE'),(62,'Egypt','EG'),(63,'Western Sahara','EH'),(64,'Eritrea','ER'),(65,'Spain','ES'),(66,'Ethiopia','ET'),(67,'Finland','FI'),(68,'Fiji','FJ'),(69,'Falkland Islands [Islas Malvinas]','FK'),(70,'Micronesia','FM'),(71,'Faroe Islands','FO'),(72,'France','FR'),(73,'Gabon','GA'),(74,'United Kingdom','GB'),(75,'Grenada','GD'),(76,'Georgia','GE'),(77,'French Guiana','GF'),(78,'Guernsey','GG'),(79,'Ghana','GH'),(80,'Gibraltar','GI'),(81,'Greenland','GL'),(82,'Gambia','GM'),(83,'Guinea','GN'),(84,'Guadeloupe','GP'),(85,'Equatorial Guinea','GQ'),(86,'Greece','GR'),(87,'South Georgia and the South Sandwich Islands','GS'),(88,'Guatemala','GT'),(89,'Guam','GU'),(90,'GuineaBissau','GW'),(91,'Guyana','GY'),(92,'Gaza Strip','GZ'),(93,'Hong Kong','HK'),(94,'Heard Island and McDonald Islands','HM'),(95,'Honduras','HN'),(96,'Croatia','HR'),(97,'Haiti','HT'),(98,'Hungary','HU'),(99,'Indonesia','ID'),(100,'Ireland','IE'),(101,'Israel','IL'),(102,'Isle of Man','IM'),(103,'India','IN'),(104,'British Indian Ocean Territory','IO'),(105,'Iraq','IQ'),(106,'Iran','IR'),(107,'Iceland','IS'),(108,'Italy','IT'),(109,'Jersey','JE'),(110,'Jamaica','JM'),(111,'Jordan','JO'),(112,'Japan','JP'),(113,'Kenya','KE'),(114,'Kyrgyzstan','KG'),(115,'Cambodia','KH'),(116,'Kiribati','KI'),(117,'Comoros','KM'),(118,'Saint Kitts and Nevis','KN'),(119,'North Korea','KP'),(120,'South Korea','KR'),(121,'Kuwait','KW'),(122,'Cayman Islands','KY'),(123,'Kazakhstan','KZ'),(124,'Laos','LA'),(125,'Lebanon','LB'),(126,'Saint Lucia','LC'),(127,'Liechtenstein','LI'),(128,'Sri Lanka','LK'),(129,'Liberia','LR'),(130,'Lesotho','LS'),(131,'Lithuania','LT'),(132,'Luxembourg','LU'),(133,'Latvia','LV'),(134,'Libya','LY'),(135,'Morocco','MA'),(136,'Monaco','MC'),(137,'Moldova','MD'),(138,'Montenegro','ME'),(139,'Madagascar','MG'),(140,'Marshall Islands','MH'),(141,'Macedonia [FYROM]','MK'),(142,'Mali','ML'),(143,'Myanmar [Burma]','MM'),(144,'Mongolia','MN'),(145,'Macau','MO'),(146,'Northern Mariana Islands','MP'),(147,'Martinique','MQ'),(148,'Mauritania','MR'),(149,'Montserrat','MS'),(150,'Malta','MT'),(151,'Mauritius','MU'),(152,'Maldives','MV'),(153,'Malawi','MW'),(154,'Mexico','MX'),(155,'Malaysia','MY'),(156,'Mozambique','MZ'),(157,'Namibia','NA'),(158,'New Caledonia','NC'),(159,'Niger','NE'),(160,'Norfolk Island','NF'),(161,'Nigeria','NG'),(162,'Nicaragua','NI'),(163,'Netherlands','NL'),(164,'Norway','NO'),(165,'Nepal','NP'),(166,'Nauru','NR'),(167,'Niue','NU'),(168,'New Zealand','NZ'),(169,'Oman','OM'),(170,'Panama','PA'),(171,'Peru','PE'),(172,'French Polynesia','PF'),(173,'Papua New Guinea','PG'),(174,'Philippines','PH'),(175,'Pakistan','PK'),(176,'Poland','PL'),(177,'Saint Pierre and Miquelon','PM'),(178,'Pitcairn Islands','PN'),(179,'Puerto Rico','PR'),(180,'Palestinian Territories','PS'),(181,'Portugal','PT'),(182,'Palau','PW'),(183,'Paraguay','PY'),(184,'Qatar','QA'),(185,'Réunion','RE'),(186,'Romania','RO'),(187,'Serbia','RS'),(188,'Russia','RU'),(189,'Rwanda','RW'),(190,'Saudi Arabia','SA'),(191,'Solomon Islands','SB'),(192,'Seychelles','SC'),(193,'Sudan','SD'),(194,'Sweden','SE'),(195,'Singapore','SG'),(196,'Saint Helena','SH'),(197,'Slovenia','SI'),(198,'Svalbard and Jan Mayen','SJ'),(199,'Slovakia','SK'),(200,'Sierra Leone','SL'),(201,'San Marino','SM'),(202,'Senegal','SN'),(203,'Somalia','SO'),(204,'Suriname','SR'),(205,'São Tomé and Príncipe','ST'),(206,'El Salvador','SV'),(207,'Syria','SY'),(208,'Swaziland','SZ'),(209,'Turks and Caicos Islands','TC'),(210,'Chad','TD'),(211,'French Southern Territories','TF'),(212,'Togo','TG'),(213,'Thailand','TH'),(214,'Tajikistan','TJ'),(215,'Tokelau','TK'),(216,'TimorLeste','TL'),(217,'Turkmenistan','TM'),(218,'Tunisia','TN'),(219,'Tonga','TO'),(220,'Turkey','TR'),(221,'Trinidad and Tobago','TT'),(222,'Tuvalu','TV'),(223,'Taiwan','TW'),(224,'Tanzania','TZ'),(225,'Ukraine','UA'),(226,'Uganda','UG'),(227,'U.S. Minor Outlying Islands','UM'),(228,'United States','US'),(229,'Uruguay','UY'),(230,'Uzbekistan','UZ'),(231,'Vatican City','VA'),(232,'Saint Vincent and the Grenadines','VC'),(233,'Venezuela','VE'),(234,'British Virgin Islands','VG'),(235,'U.S. Virgin Islands','VI'),(236,'Vietnam','VN'),(237,'Vanuatu','VU'),(238,'Wallis and Futuna','WF'),(239,'Samoa','WS'),(240,'Kosovo','XK'),(241,'Yemen','YE'),(242,'Mayotte','YT'),(243,'South Africa','ZA'),(244,'Zambia','ZM'),(245,'Zimbabwe','ZW');
/*!40000 ALTER TABLE `countries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditors`
--

DROP TABLE IF EXISTS `creditors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditors` (
  `creditorID` int(10) NOT NULL AUTO_INCREMENT,
  `creditorName` varchar(50) NOT NULL,
  PRIMARY KEY (`creditorID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditors`
--

LOCK TABLES `creditors` WRITE;
/*!40000 ALTER TABLE `creditors` DISABLE KEYS */;
/*!40000 ALTER TABLE `creditors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `currencies`
--

DROP TABLE IF EXISTS `currencies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `currencies` (
  `currencyID` int(10) NOT NULL AUTO_INCREMENT,
  `currencyName` varchar(3) NOT NULL,
  PRIMARY KEY (`currencyID`),
  UNIQUE KEY `currencyName` (`currencyName`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `currencies`
--

LOCK TABLES `currencies` WRITE;
/*!40000 ALTER TABLE `currencies` DISABLE KEYS */;
INSERT INTO `currencies` VALUES (5,'CHF'),(2,'EUR'),(4,'GBP'),(1,'RON'),(3,'USD');
/*!40000 ALTER TABLE `currencies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `currency_exchange_rates`
--

DROP TABLE IF EXISTS `currency_exchange_rates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `currency_exchange_rates` (
  `exchangeRateID` int(20) NOT NULL AUTO_INCREMENT,
  `sourceCurrencyID` int(20) NOT NULL,
  `targetCurrencyID` int(20) NOT NULL,
  `value` double DEFAULT NULL,
  `lastUpdatedDate` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`exchangeRateID`),
  UNIQUE KEY `sourceCurrencyID` (`sourceCurrencyID`,`targetCurrencyID`),
  KEY `targetCurrencyID` (`targetCurrencyID`),
  CONSTRAINT `currency_exchange_rates_ibfk_1` FOREIGN KEY (`sourceCurrencyID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE,
  CONSTRAINT `currency_exchange_rates_ibfk_2` FOREIGN KEY (`targetCurrencyID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `currency_exchange_rates`
--

LOCK TABLES `currency_exchange_rates` WRITE;
/*!40000 ALTER TABLE `currency_exchange_rates` DISABLE KEYS */;
/*!40000 ALTER TABLE `currency_exchange_rates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debtors`
--

DROP TABLE IF EXISTS `debtors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debtors` (
  `debtorID` int(10) NOT NULL AUTO_INCREMENT,
  `debtorName` varchar(30) NOT NULL,
  PRIMARY KEY (`debtorID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debtors`
--

LOCK TABLES `debtors` WRITE;
/*!40000 ALTER TABLE `debtors` DISABLE KEYS */;
/*!40000 ALTER TABLE `debtors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debts`
--

DROP TABLE IF EXISTS `debts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debts` (
  `debtID` int(10) NOT NULL AUTO_INCREMENT,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `creditor_ID` int(10) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`debtID`),
  KEY `user_ID` (`user_ID`),
  KEY `creditor_ID` (`creditor_ID`),
  CONSTRAINT `debts_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  CONSTRAINT `debts_ibfk_2` FOREIGN KEY (`creditor_ID`) REFERENCES `creditors` (`creditorID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debts`
--

LOCK TABLES `debts` WRITE;
/*!40000 ALTER TABLE `debts` DISABLE KEYS */;
/*!40000 ALTER TABLE `debts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dividends`
--

DROP TABLE IF EXISTS `dividends`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dividends` (
  `dividendID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `value` double DEFAULT NULL,
  `currency_ID` int(20) NOT NULL,
  `symbol_ID` int(20) NOT NULL,
  `portfolio_ID` int(20) NOT NULL,
  `createdDate` date DEFAULT NULL,
  PRIMARY KEY (`dividendID`),
  KEY `currency_ID` (`currency_ID`),
  KEY `symbol_ID` (`symbol_ID`),
  KEY `portfolio_ID` (`portfolio_ID`),
  CONSTRAINT `dividends_ibfk_1` FOREIGN KEY (`currency_ID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE,
  CONSTRAINT `dividends_ibfk_2` FOREIGN KEY (`symbol_ID`) REFERENCES `symbols` (`symbolID`) ON UPDATE CASCADE,
  CONSTRAINT `dividends_ibfk_3` FOREIGN KEY (`portfolio_ID`) REFERENCES `portfolios` (`portfolioID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dividends`
--

LOCK TABLES `dividends` WRITE;
/*!40000 ALTER TABLE `dividends` DISABLE KEYS */;
/*!40000 ALTER TABLE `dividends` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Account balance update on dividend insertion`AFTER INSERT ON dividends FOR EACH ROW
BEGIN
	
DECLARE v_records_number INT(10);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_record_name VARCHAR(40);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);


SET
v_record_name = CONCAT('balance_record_', DATE_FORMAT(NEW.createdDate, '%Y-%m-%d'));

SELECT
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_ID,
	v_investor_ID
FROM
	dividends dvd
INNER JOIN portfolios ps ON
	dvd.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts bac ON
	ps.broker_ID = bac.broker_ID
INNER JOIN accounts acc ON
	bac.account_ID = acc.accountID
WHERE
	dvd.dividendID = NEW.dividendID;

SELECT
	COUNT(*)
INTO
	v_records_number
FROM
	saving_accounts_balance
WHERE
	MONTH = MONTH(NEW.createdDate)
	AND YEAR = YEAR(NEW.createdDate)
	AND user_ID = v_investor_ID
	AND account_ID = v_investment_account_ID;

-- Monthly account balance update logic
IF v_records_number = 0 THEN
INSERT
	INTO
	saving_accounts_balance(
    user_ID,
	account_ID,
	recordName,
	value,
	MONTH,
	YEAR
)
VALUES (
v_investor_ID,
v_investment_account_ID,
v_record_name,
NEW.value,
MONTH(NEW.createdDate),
YEAR(NEW.createdDate)
);
ELSE
UPDATE
	saving_accounts_balance
SET
	value = value + NEW.value,
	recordName = v_record_name
WHERE
	MONTH = MONTH(NEW.createdDate)
	AND YEAR = YEAR(NEW.createdDate)
	AND user_ID = v_investor_ID
	AND account_ID = v_investment_account_ID;
END IF;

-- Accounts balance storage update logic
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on dividend insertion';

SET v_action_description = 'Dividend insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

SET v_value_triggering_update = NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Account balance update on dividend update` AFTER UPDATE ON dividends FOR EACH ROW
BEGIN

DECLARE v_record_amount_difference DOUBLE;

DECLARE v_new_record_name VARCHAR(30);

DECLARE v_investment_account_ID INT(10);

DECLARE v_investor_ID INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

SET v_record_amount_difference = NEW.value - OLD.value;

SET v_new_record_name = CONCAT('balance_record_', DATE_FORMAT(NEW.createdDate, '%Y-%m-%d'));
	
SELECT
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_ID,
	v_investor_ID
FROM
	dividends dvd
INNER JOIN portfolios ps ON
	dvd.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts bac ON
	ps.broker_ID = bac.broker_ID
INNER JOIN accounts acc ON
	bac.account_ID = acc.accountID
WHERE
	dvd.dividendID = NEW.dividendID;


-- Monthly account balance update logic
UPDATE
	saving_accounts_balance
SET
	recordName = v_new_record_name,
	value = value + v_record_amount_difference
WHERE
	MONTH = MONTH(NEW.createdDate)
	AND YEAR = YEAR(NEW.createdDate)
	AND user_ID = v_investor_ID
	AND account_ID = v_investment_account_ID;

-- Account storage balance update logic
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on dividend update';

SET v_action_description = 'Dividend update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

SET v_value_triggering_update = v_record_amount_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Account balance update on dividend deletion` BEFORE DELETE ON dividends FOR EACH ROW
BEGIN 

DECLARE v_new_record_name VARCHAR(30);

DECLARE v_investment_account_ID INT(10);

DECLARE v_investor_ID INT(20);

DECLARE v_subtracted_amount DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);


SET v_new_record_name = CONCAT('balance_record_', DATE_FORMAT(CURRENT_DATE,'%Y-%m-%d'));

SET v_subtracted_amount = OLD.value;


SELECT
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_ID,
	v_investor_ID
FROM
	dividends dvd
INNER JOIN portfolios ps ON
	dvd.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts bac ON
	ps.broker_ID = bac.broker_ID
INNER JOIN accounts acc ON
	bac.account_ID = acc.accountID
WHERE
	dvd.dividendID = OLD.dividendID;

-- Monthly account balance update logic
UPDATE
	saving_accounts_balance
SET
	recordName = v_new_record_name,
	value = value - v_subtracted_amount
WHERE
	MONTH = MONTH(OLD.createdDate)
	AND YEAR = YEAR(OLD.createdDate)
	AND user_ID = v_investor_ID
	AND account_ID = v_investment_account_ID;

-- Accounts balance storage update logic
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on dividend deletion';

SET v_action_description = 'Dividend deletion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

SET v_value_triggering_update = -OLD.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `expense_types`
--

DROP TABLE IF EXISTS `expense_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `expense_types` (
  `categoryID` int(10) NOT NULL AUTO_INCREMENT,
  `categoryName` varchar(30) NOT NULL,
  PRIMARY KEY (`categoryID`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `expense_types`
--

LOCK TABLES `expense_types` WRITE;
/*!40000 ALTER TABLE `expense_types` DISABLE KEYS */;
INSERT INTO `expense_types` VALUES (4,'Utilities'),(5,'Education'),(6,'Sport'),(7,'Food'),(8,'Transport'),(9,'Housing'),(10,'Household items'),(11,'Insurance'),(12,'Healthcare'),(13,'Gifts/donations'),(14,'Entertainment'),(15,'Clothing'),(16,'Personal care'),(17,'Others'),(18,'IT&C'),(19,'Footwear'),(20,'Hobbies'),(21,'Travelling'),(22,'Souvenirs'),(23,'Banking fees'),(24,'Restaurants/bars/cafes'),(25,'Entrance fees'),(26,'Driving lessons');
/*!40000 ALTER TABLE `expense_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `expenses`
--

DROP TABLE IF EXISTS `expenses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `expenses` (
  `expenseID` int(10) NOT NULL AUTO_INCREMENT,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`expenseID`),
  KEY `type` (`type`),
  KEY `user_ID` (`user_ID`),
  CONSTRAINT `expenses_ibfk_1` FOREIGN KEY (`type`) REFERENCES `expense_types` (`categoryID`),
  CONSTRAINT `expenses_ibfk_2` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `expenses`
--

LOCK TABLES `expenses` WRITE;
/*!40000 ALTER TABLE `expenses` DISABLE KEYS */;
/*!40000 ALTER TABLE `expenses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `external_accounts_balance`
--

DROP TABLE IF EXISTS `external_accounts_balance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `external_accounts_balance` (
  `recordID` int(20) NOT NULL AUTO_INCREMENT,
  `account_ID` int(20) NOT NULL,
  `recordName` varchar(50) DEFAULT NULL,
  `value` double DEFAULT NULL,
  `createdDate` date DEFAULT NULL,
  `lastUpdatedDate` date DEFAULT NULL,
  PRIMARY KEY (`recordID`),
  KEY `external_accounts_balance_ibfk_2` (`account_ID`),
  CONSTRAINT `external_accounts_balance_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `external_accounts_balance`
--

LOCK TABLES `external_accounts_balance` WRITE;
/*!40000 ALTER TABLE `external_accounts_balance` DISABLE KEYS */;
/*!40000 ALTER TABLE `external_accounts_balance` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `External balance record validation` BEFORE INSERT ON `external_accounts_balance` FOR EACH ROW BEGIN
DECLARE v_exists INT DEFAULT 0;

SELECT COUNT(*)
INTO v_exists
FROM external_accounts_balance
WHERE account_ID = NEW.account_ID
AND MONTH(createdDate) = MONTH(NEW.createdDate)
AND YEAR(createdDate) = YEAR(NEW.createdDate);

IF v_exists >= 1 THEN
SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = 'Cannot insert duplicate data! A record already exists for the month/year represented by the creationDate field value.';

END IF;
                                                                                      
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `external_accounts_banking_fees`
--

DROP TABLE IF EXISTS `external_accounts_banking_fees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `external_accounts_banking_fees` (
  `bankingFeeID` int(20) NOT NULL AUTO_INCREMENT,
  `account_ID` int(20) NOT NULL,
  `name` varchar(70) DEFAULT NULL,
  `value` double DEFAULT NULL,
  `description` varchar(150) DEFAULT NULL,
  `createdDate` date DEFAULT NULL,
  PRIMARY KEY (`bankingFeeID`),
  KEY `account_ID` (`account_ID`),
  CONSTRAINT `external_accounts_banking_fees_ibfk_1` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `external_accounts_banking_fees`
--

LOCK TABLES `external_accounts_banking_fees` WRITE;
/*!40000 ALTER TABLE `external_accounts_banking_fees` DISABLE KEYS */;
/*!40000 ALTER TABLE `external_accounts_banking_fees` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update/creation on banking fee insertion`
AFTER
INSERT
	ON
	external_accounts_banking_fees FOR EACH ROW
BEGIN

DECLARE v_has_balance_record TINYINT(1);

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_balance_record_name VARCHAR(50);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_record_creation_result TINYINT(1);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(NEW.createdDate);
SET v_balance_record_year = YEAR(NEW.createdDate);

/*Checks the balance record existence*/
CALL has_balance_record_for_selected_month(NEW.account_ID,
v_balance_record_month,
v_balance_record_year,
v_has_balance_record);

/*Sets the name of the new/updated balance record*/
SET
v_balance_record_name = CONCAT('balance_record_', NEW.createdDate);

IF (v_has_balance_record = 0) THEN
/*Balance record creation branch*/
/*The new balance record value will be set to the negative value of the banking fee since money is withdrawn from the account in this case*/
SET
v_new_balance_record_value = -NEW.value;

/*Creates a new balance record for the external account*/
CALL create_external_account_balance_record(NEW.account_ID,
v_balance_record_name,
v_new_balance_record_value,
NEW.createdDate,
NULL,
v_record_creation_result);

ELSE 
/*Balance record value update branch*/
/*Retrieves the value of the existing external account balance record for the month and year on which the banking fee is inserted*/
SELECT
	eab.value
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance eab
WHERE
	MONTH(eab.createdDate) = v_balance_record_month
AND YEAR(eab.createdDate) = v_balance_record_year
AND account_ID = NEW.account_ID;

/*Calculates the new balance record value (the banking fee value is subtracted from the current value of the balance record*/
SET
v_new_balance_record_value = v_current_balance_record_value - NEW.value;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = NEW.account_ID;
END IF;

/*Account balance storage implementation-START*/

SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = NEW.account_ID;

SET v_action_source = 'Balance record update/creation on banking fee insertion';

SET v_action_description = 'Banking fee insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

/*Since the banking fee insertion leads to a decrease of the current account balance the value triggering the update will be set to negative*/
SET v_value_triggering_update = -NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on banking fee update`
AFTER
UPDATE
	ON
	external_accounts_banking_fees FOR EACH ROW
BEGIN 
	
DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_difference DOUBLE;

DECLARE v_current_balance_record_month INT(20);

DECLARE v_current_balance_record_year INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Check is added to prevent the execution of trigger logic when the value of the record is unchanged*/
IF (NEW.value != OLD.value) THEN 
/*Retrieves the value of the balance record for the month/year on which the banking fee that is updated was inserted*/
SET v_current_balance_record_value = get_balance_record_value_with_double_precision(NEW.account_ID,
NEW.createdDate);

/*Retrieves the current balance of the account from the account balance storage system*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = NEW.account_ID;

/*Calculates the difference between the current value of the banking fee and the updated value*/
SET v_difference = OLD.value - NEW.value;

/*Calculates the new value of the balance record
 If v_difference variable value is negative the current balance will decrease otherwise it will increase by the corresponding amount*/
SET v_new_balance_record_value = v_current_balance_record_value + v_difference;

/*Calculates the new value of the balance storage record*/
SET v_new_storage_balance = v_current_storage_balance + v_difference;

/*Retrieves the month and year which will be used to determine the balance record that needs to be updated*/
SET v_current_balance_record_month = MONTH(NEW.createdDate);
SET v_current_balance_record_year = YEAR(NEW.createdDate);

UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_current_balance_record_month
AND YEAR(createdDate) = v_current_balance_record_year
AND account_ID = NEW.account_ID;

END IF;

/*Account balance storage implementation-START*/

SET v_action_source = 'Balance record update on banking fee update';

SET v_action_description = 'Banking fee update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

SET v_value_triggering_update = v_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on banking fee deletion`
AFTER DELETE
ON external_accounts_banking_fees FOR EACH ROW
BEGIN 
	
DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_balance_record_month INT(20);

DECLARE v_current_balance_record_year INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Retrieves the value of the balance record for the month/year on which the banking fee that is updated was inserted*/
SET v_current_balance_record_value = get_balance_record_value_with_double_precision(OLD.account_ID,
OLD.createdDate);

/*Calculates the new value of the balance record (the value of the deleted banking fee is added to the current balance because the corresponding value was returned to the account)*/
SET v_new_balance_record_value = v_current_balance_record_value + OLD.value;

/*Retrieves the month and year which will be used to determine the balance record that needs to be updated*/
SET v_current_balance_record_month = MONTH(OLD.createdDate);
SET v_current_balance_record_year = YEAR(OLD.createdDate);

UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_current_balance_record_month
AND YEAR(createdDate) = v_current_balance_record_year
AND account_ID = OLD.account_ID;

/*Account balance storage implementation-START*/

SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = OLD.account_ID;

SET v_action_source = 'Balance record update on banking fee deletion';

SET v_action_description = 'Banking fee delete';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

/*The value triggering the update is set to the value of the deleted banking fee because this is the amount that will be added to the current value of the balance storage record*/
SET v_value_triggering_update = OLD.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(OLD.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/


END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `free_stocks`
--

DROP TABLE IF EXISTS `free_stocks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `free_stocks` (
  `stockID` int(20) NOT NULL AUTO_INCREMENT,
  `symbol_ID` int(20) NOT NULL,
  `portfolio_ID` int(20) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `totalQty` int(20) DEFAULT NULL,
  `companyName` varchar(20) DEFAULT NULL,
  `description` varchar(150) DEFAULT NULL,
  `createdDate` date DEFAULT NULL,
  PRIMARY KEY (`stockID`),
  KEY `symbol_ID` (`symbol_ID`),
  KEY `portfolio_ID` (`portfolio_ID`),
  CONSTRAINT `free_stocks_ibfk_1` FOREIGN KEY (`symbol_ID`) REFERENCES `symbols` (`symbolID`) ON UPDATE CASCADE,
  CONSTRAINT `free_stocks_ibfk_2` FOREIGN KEY (`portfolio_ID`) REFERENCES `portfolios` (`portfolioID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `free_stocks`
--

LOCK TABLES `free_stocks` WRITE;
/*!40000 ALTER TABLE `free_stocks` DISABLE KEYS */;
/*!40000 ALTER TABLE `free_stocks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `income_types`
--

DROP TABLE IF EXISTS `income_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `income_types` (
  `typeID` int(10) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(20) NOT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `income_types`
--

LOCK TABLES `income_types` WRITE;
/*!40000 ALTER TABLE `income_types` DISABLE KEYS */;
INSERT INTO `income_types` VALUES (3,'Dividends'),(4,'Other'),(5,'Copyright'),(6,'Salary'),(7,'Rental income'),(8,'Meal tickets refund'),(9,'Goods sale');
/*!40000 ALTER TABLE `income_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incomes`
--

DROP TABLE IF EXISTS `incomes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incomes` (
  `incomeID` int(10) NOT NULL AUTO_INCREMENT,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `incomeType` int(10) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`incomeID`),
  KEY `user_ID` (`user_ID`),
  KEY `incomeType` (`incomeType`),
  CONSTRAINT `incomes_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  CONSTRAINT `incomes_ibfk_2` FOREIGN KEY (`incomeType`) REFERENCES `income_types` (`typeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incomes`
--

LOCK TABLES `incomes` WRITE;
/*!40000 ALTER TABLE `incomes` DISABLE KEYS */;
/*!40000 ALTER TABLE `incomes` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record creation on income insertion` AFTER
INSERT
	ON
	`incomes` FOR EACH ROW 
BEGIN
	
DECLARE v_has_balance_record TINYINT(1) DEFAULT 0;

DECLARE v_insertion_result TINYINT(1) DEFAULT 0;

DECLARE v_user_ID INT DEFAULT 0;

DECLARE v_saving_account_ID INT DEFAULT 0;

DECLARE v_balance_record_name VARCHAR(50) DEFAULT NULL;

DECLARE v_balance_record_month INT DEFAULT 0;

DECLARE v_balance_record_year INT DEFAULT 0;

SET
v_user_ID = NEW.user_ID;

SELECT
	accountID 
INTO
	v_saving_account_ID
FROM
	accounts acc
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	user_ID = v_user_ID
	AND at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

SET
v_balance_record_name = CONCAT('balance_record_', CURDATE());

SET
v_balance_record_month = MONTH(NEW.date);

SET
v_balance_record_year = YEAR(NEW.date);

CALL has_balance_record_for_selected_month(v_saving_account_ID,
v_balance_record_month,
v_balance_record_year,
@v_has_balance_record);

/*Very important!! This assigns the value of the variable @v_has_balance_record to the variable v_has_balance_record. 
 Without this the v_has_balance_record variable will retain its default value and each time a new income will be inserted the trigger will try to create a new balance record for
 the default saving account even if there's already an existing one*/
SET
v_has_balance_record = @v_has_balance_record;

IF v_has_balance_record = 0 THEN
CALL create_saving_account_balance_record(v_user_ID, v_saving_account_ID, v_balance_record_name, 0, v_balance_record_month, v_balance_record_year, @v_insertion_result);
END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `industries`
--

DROP TABLE IF EXISTS `industries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `industries` (
  `industryID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`industryID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `industries`
--

LOCK TABLES `industries` WRITE;
/*!40000 ALTER TABLE `industries` DISABLE KEYS */;
INSERT INTO `industries` VALUES (1,'Financial'),(2,'Energy'),(3,'Automotive'),(4,'Technology');
/*!40000 ALTER TABLE `industries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `interest_payment_type`
--

DROP TABLE IF EXISTS `interest_payment_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `interest_payment_type` (
  `typeID` int(20) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `interest_payment_type`
--

LOCK TABLES `interest_payment_type` WRITE;
/*!40000 ALTER TABLE `interest_payment_type` DISABLE KEYS */;
INSERT INTO `interest_payment_type` VALUES (1,'Monthly payment'),(2,'Yearly payment');
/*!40000 ALTER TABLE `interest_payment_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `interest_types`
--

DROP TABLE IF EXISTS `interest_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `interest_types` (
  `typeID` int(20) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `interest_types`
--

LOCK TABLES `interest_types` WRITE;
/*!40000 ALTER TABLE `interest_types` DISABLE KEYS */;
INSERT INTO `interest_types` VALUES (1,'Yearly interest'),(2,'Monthly interest');
/*!40000 ALTER TABLE `interest_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `investment_taxes`
--

DROP TABLE IF EXISTS `investment_taxes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `investment_taxes` (
  `taxID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `value` double DEFAULT NULL,
  `investor_ID` int(20) NOT NULL,
  `country_ID` int(20) NOT NULL,
  `fiscalYear` int(4) DEFAULT NULL,
  `paymentDate` date DEFAULT NULL,
  PRIMARY KEY (`taxID`),
  UNIQUE KEY `investor_ID` (`investor_ID`,`country_ID`,`fiscalYear`),
  KEY `country_ID` (`country_ID`),
  CONSTRAINT `investment_taxes_ibfk_1` FOREIGN KEY (`investor_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `investment_taxes_ibfk_2` FOREIGN KEY (`country_ID`) REFERENCES `countries` (`countryID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `investment_taxes`
--

LOCK TABLES `investment_taxes` WRITE;
/*!40000 ALTER TABLE `investment_taxes` DISABLE KEYS */;
/*!40000 ALTER TABLE `investment_taxes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `partial_payments`
--

DROP TABLE IF EXISTS `partial_payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `partial_payments` (
  `paymentID` int(20) NOT NULL AUTO_INCREMENT,
  `receivable_ID` int(20) NOT NULL,
  `paymentName` varchar(50) NOT NULL,
  `paymentValue` int(20) NOT NULL,
  `paymentDate` date NOT NULL,
  PRIMARY KEY (`paymentID`),
  KEY `partial_payments_ibfk_1` (`receivable_ID`),
  CONSTRAINT `partial_payments_ibfk_1` FOREIGN KEY (`receivable_ID`) REFERENCES `receivables` (`receivableID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partial_payments`
--

LOCK TABLES `partial_payments` WRITE;
/*!40000 ALTER TABLE `partial_payments` DISABLE KEYS */;
/*!40000 ALTER TABLE `partial_payments` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Total paid amount update on insert` AFTER
INSERT
	ON
	`partial_payments` FOR EACH ROW BEGIN

DECLARE v_receivable_value INT(20);

DECLARE v_total_paid_amount INT(20);

DECLARE v_new_receivable_status VARCHAR(50);

/*Retrieves the receivable value and the sum of partial payments for the current receivable ID*/
SELECT
	rc.value,
	SUM(pps.paymentValue)
INTO
	v_receivable_value,
	v_total_paid_amount
FROM
	partial_payments pps
INNER JOIN receivables rc ON
	pps.receivable_ID = rc.receivableID
WHERE
	pps.receivable_ID = NEW.receivable_ID;

/*Retrieves the new receivable status which will be used to update the receivable after the partial payment insertion*/
CALL get_new_receivable_status(NEW.receivable_ID, FALSE, NULL,
v_new_receivable_status);

UPDATE
	receivables
SET
	totalPaidAmount = v_total_paid_amount,
	status_ID = (
	SELECT
		statusID
	FROM
		receivable_status
	WHERE
		statusDescription = v_new_receivable_status)
WHERE
	receivableID = NEW.receivable_ID;

/*If after inserting the current partial payment the total paid amount becomes equal to the receivable value then
the payOffDate field will be set to the current date as the receivable is considered fully paid*/
IF v_total_paid_amount = v_receivable_value THEN
UPDATE
	receivables
SET
	payOffDate = CURDATE()
WHERE
	receivableID = NEW.receivable_ID;
END IF;

/*IMPORTANT NOTE!!
 The balance record update logic after the partial payment insertion is included in the trigger "Balance record update on receivable update" which is present in the receivables table
 */
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Total paid amount update on update` AFTER
UPDATE
	ON
	`partial_payments` FOR EACH ROW BEGIN
		
DECLARE v_default_account_ID INT(20);

/*Updates the total paid amount of the receivable*/
UPDATE
	receivables
SET
	totalPaidAmount = (
	SELECT
		SUM(paymentValue)
	FROM
		partial_payments
	WHERE
		receivable_ID = NEW.receivable_ID)
WHERE
	receivables.receivableID = NEW.receivable_ID;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Total paid amount update on delete` AFTER
DELETE
	ON
	`partial_payments` FOR EACH ROW BEGIN

DECLARE v_new_receivable_status VARCHAR(50);

DECLARE v_total_partial_payments INT(20);

DECLARE v_new_receivable_status_ID VARCHAR(50);

/*Retrieves the new status of the receivable in order to corectly update it in the receivables table*/
CALL get_new_receivable_status(OLD.receivable_ID, FALSE, NULL,
v_new_receivable_status);

/*Retrieves the total partial payments value for the receivable whose partial payment is deleted*/
SELECT
		SUM(paymentValue)
INTO
		v_total_partial_payments
FROM
		partial_payments
WHERE
		receivable_ID = OLD.receivable_ID;

/*Retrieves the ID of the new receivable status*/
SELECT
	statusID
INTO
	v_new_receivable_status_ID
FROM
	receivable_status
WHERE
	statusDescription = v_new_receivable_status;

/*Updates the total paid amount of the receivable*/
UPDATE
	receivables
SET
	totalPaidAmount = v_total_partial_payments,
	status_ID = v_new_receivable_status_ID
WHERE
	receivables.receivableID = OLD.receivable_ID;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `plan_types`
--

DROP TABLE IF EXISTS `plan_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plan_types` (
  `typeID` int(10) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(30) NOT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plan_types`
--

LOCK TABLES `plan_types` WRITE;
/*!40000 ALTER TABLE `plan_types` DISABLE KEYS */;
INSERT INTO `plan_types` VALUES (1,'One month'),(2,'Six months');
/*!40000 ALTER TABLE `plan_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `portfolios`
--

DROP TABLE IF EXISTS `portfolios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `portfolios` (
  `portfolioID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `investor_ID` int(20) NOT NULL,
  `broker_ID` int(20) NOT NULL,
  `currency_ID` int(20) NOT NULL,
  `createdDate` date DEFAULT NULL,
  PRIMARY KEY (`portfolioID`),
  KEY `user_ID` (`investor_ID`),
  KEY `broker_ID` (`broker_ID`),
  KEY `currency_ID` (`currency_ID`),
  CONSTRAINT `portfolios_ibfk_1` FOREIGN KEY (`investor_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `portfolios_ibfk_2` FOREIGN KEY (`broker_ID`) REFERENCES `brokers` (`brokerID`) ON UPDATE CASCADE,
  CONSTRAINT `portfolios_ibfk_3` FOREIGN KEY (`currency_ID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `portfolios`
--

LOCK TABLES `portfolios` WRITE;
/*!40000 ALTER TABLE `portfolios` DISABLE KEYS */;
/*!40000 ALTER TABLE `portfolios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `portfolios_symbols`
--

DROP TABLE IF EXISTS `portfolios_symbols`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `portfolios_symbols` (
  `portfolio_ID` int(20) NOT NULL,
  `symbol_ID` int(20) NOT NULL,
  KEY `portfolio_ID` (`portfolio_ID`),
  KEY `symbol_ID` (`symbol_ID`),
  CONSTRAINT `portfolios_symbols_ibfk_1` FOREIGN KEY (`portfolio_ID`) REFERENCES `portfolios` (`portfolioID`) ON UPDATE CASCADE,
  CONSTRAINT `portfolios_symbols_ibfk_2` FOREIGN KEY (`symbol_ID`) REFERENCES `symbols` (`symbolID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `portfolios_symbols`
--

LOCK TABLES `portfolios_symbols` WRITE;
/*!40000 ALTER TABLE `portfolios_symbols` DISABLE KEYS */;
/*!40000 ALTER TABLE `portfolios_symbols` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receivable_history`
--

DROP TABLE IF EXISTS `receivable_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receivable_history` (
  `histID` int(20) NOT NULL AUTO_INCREMENT,
  `receivable_ID` int(20) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) DEFAULT NULL,
  `totalPaidAmount` int(20) DEFAULT NULL,
  `status` varchar(50) DEFAULT NULL,
  `debtor_ID` int(20) NOT NULL,
  `account_ID` int(20) DEFAULT NULL,
  `createdDate` date NOT NULL,
  `dueDate` date NOT NULL,
  `performedAction` varchar(50) DEFAULT NULL,
  `histTimestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`histID`),
  KEY `receivable_history_ibfk_1` (`receivable_ID`),
  KEY `receivable_history_ibfk_2` (`account_ID`),
  KEY `debtor_ID` (`debtor_ID`),
  CONSTRAINT `receivable_history_ibfk_1` FOREIGN KEY (`receivable_ID`) REFERENCES `receivables` (`receivableID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `receivable_history_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `receivable_history_ibfk_3` FOREIGN KEY (`debtor_ID`) REFERENCES `debtors` (`debtorID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receivable_history`
--

LOCK TABLES `receivable_history` WRITE;
/*!40000 ALTER TABLE `receivable_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `receivable_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receivable_status`
--

DROP TABLE IF EXISTS `receivable_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receivable_status` (
  `statusID` int(20) NOT NULL AUTO_INCREMENT,
  `statusDescription` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`statusID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receivable_status`
--

LOCK TABLES `receivable_status` WRITE;
/*!40000 ALTER TABLE `receivable_status` DISABLE KEYS */;
INSERT INTO `receivable_status` VALUES (1,'New'),(2,'Partially paid'),(3,'Paid'),(4,'Overdue');
/*!40000 ALTER TABLE `receivable_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receivables`
--

DROP TABLE IF EXISTS `receivables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receivables` (
  `receivableID` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `debtor_ID` int(10) DEFAULT NULL,
  `account_ID` int(20) NOT NULL,
  `totalPaidAmount` int(20) NOT NULL,
  `status_ID` int(20) NOT NULL,
  `createdDate` date NOT NULL,
  `dueDate` date NOT NULL,
  `payOffDate` date DEFAULT NULL,
  PRIMARY KEY (`receivableID`),
  KEY `debtor_ID` (`debtor_ID`),
  KEY `account_ID` (`account_ID`),
  KEY `status_ID` (`status_ID`),
  CONSTRAINT `receivables_ibfk_1` FOREIGN KEY (`debtor_ID`) REFERENCES `debtors` (`debtorID`) ON UPDATE CASCADE,
  CONSTRAINT `receivables_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE,
  CONSTRAINT `receivables_ibfk_3` FOREIGN KEY (`status_ID`) REFERENCES `receivable_status` (`statusID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receivables`
--

LOCK TABLES `receivables` WRITE;
/*!40000 ALTER TABLE `receivables` DISABLE KEYS */;
/*!40000 ALTER TABLE `receivables` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update/creation on receivable insertion` AFTER
INSERT
	ON
	`receivables` FOR EACH ROW 
BEGIN
	
DECLARE v_user_ID INT(20);

DECLARE v_default_account_ID INT(20);

DECLARE v_balance_record_name VARCHAR(50);

DECLARE v_record_month INT(20);

DECLARE v_record_year INT(20);

DECLARE v_has_balance_record TINYINT(1);

DECLARE v_record_creation_result TINYINT(1);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Retrieves the user_ID*/
SELECT
	acc.user_ID
INTO
	v_user_ID
FROM
	receivables rcs
INNER JOIN accounts acc ON
	rcs.account_ID = acc.accountID
WHERE
	rcs.receivableID = NEW.receivableID;

/*Retrieves the default account ID for the user*/
SELECT
	acc.accountID
INTO
	v_default_account_ID
FROM
	accounts acc
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.user_ID = v_user_ID
	AND at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

SET
v_record_month = MONTH(NEW.createdDate);

SET
v_record_year = YEAR(NEW.createdDate);

/*Checks if a balance record exists for the current  month and year*/
CALL has_balance_record_for_selected_month(v_default_account_ID,
v_record_month,
v_record_year,
v_has_balance_record);

IF(v_has_balance_record = 0) THEN
SET
v_balance_record_name = CONCAT('balance_record_', NEW.createdDate);

/*When creating a new balance record value its value will be equal to the negative receivable value-money is withdrawn from the account*/
SET
v_new_balance_record_value = -NEW.value;

/*Creates a new balance record*/
CALL create_saving_account_balance_record(v_user_ID,
v_default_account_ID,
v_balance_record_name,
v_new_balance_record_value,
v_record_month,
v_record_year,
v_record_creation_result);


CALL log_receivable_history(NEW.receivableID,
@logOutput);

ELSE
/*Retrieves the value of the existing balance record*/
SELECT
	value
INTO
	v_current_balance_record_value
FROM
	saving_accounts_balance
WHERE
	account_ID = v_default_account_ID
	AND MONTH = v_record_month
	AND YEAR = v_record_year;

/*Sets the new balance record value(current value - new receivable value)-money is withdrawn from the account*/
SET
v_new_balance_record_value = v_current_balance_record_value - NEW.value;

/*Updates the balance record value*/
UPDATE
	saving_accounts_balance
SET
	value = v_new_balance_record_value
WHERE
	account_ID = v_default_account_ID
	AND MONTH = v_record_month
	AND YEAR = v_record_year;

/*Logs the receivable insertion*/
CALL log_receivable_history(NEW.receivableID,
@logOutput);

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_default_account_ID;

SET v_action_source = 'Balance record update/creation on receivable insertion';

SET v_action_description = 'Receivable insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

/*Since the receivable insertion leads to a decrease of the current account balance the value triggering the update will be set to negative*/
SET v_value_triggering_update = -NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_default_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Receivable record changes before update` BEFORE
UPDATE
	ON
	`receivables` FOR EACH ROW BEGIN 
	
DECLARE v_new_receivable_status VARCHAR(50);

DECLARE v_new_receivable_status_ID INT(20);

IF NEW.dueDate > OLD.dueDate THEN

CALL get_new_receivable_status(NEW.receivableID, TRUE, NEW.dueDate,
v_new_receivable_status);

SELECT
	statusID
INTO
	v_new_receivable_status_ID
FROM
	receivable_status
WHERE
	statusDescription = v_new_receivable_status;

SET NEW.status_ID = v_new_receivable_status_ID;

END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on receivable update` AFTER

UPDATE

	ON

	`receivables` FOR EACH ROW BEGIN

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_new_receivable_status INT(20);

DECLARE v_record_amount_difference INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Retrieves the value of the balance record for the month & year in which the updated receivable was created*/
SELECT
	value
INTO
	v_current_balance_record_value
FROM
	saving_accounts_balance
WHERE
	account_ID = NEW.account_ID
	AND MONTH = MONTH(NEW.createdDate)
	AND YEAR = YEAR(NEW.createdDate);

/*Retrieves the current balance of the account from the account balance storage system*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = NEW.account_ID;

/*Retrieves the new status of the receivable in order to perform further checks */
CALL get_new_receivable_status(NEW.receivableID,
FALSE,
NULL,
v_new_receivable_status);

/*Covers the situation when the receivable value is increased*/
IF NEW.value > OLD.value THEN

	/*Calculates the difference between the new receivable value and the old receivable value and subtracts it from the current balance record value (because more money was retrieved from the account hence the balance must decrease)*/ 
	SET
		v_record_amount_difference = NEW.value - OLD.value;
	SET
		v_new_balance_record_value = v_current_balance_record_value - v_record_amount_difference;
	SET 
		v_new_storage_balance = v_current_storage_balance - v_record_amount_difference;

/*Covers the situation when the receivable value is decreased*/
ELSEIF NEW.value < OLD.value THEN

	/*Calculates the difference between the old receivable value and the new receivable value and adds it to the current balance record value (because less money was retrieved from the account hence the balance must increase)*/ 
	SET
		v_record_amount_difference = OLD.value - NEW.value;
	SET
		v_new_balance_record_value = v_current_balance_record_value + v_record_amount_difference;
	SET 
		v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;

/*Covers the situation when the receivable is marked as paid*/
ELSEIF v_new_receivable_status = 'Paid'
AND NEW.payOffDate != NULL THEN
	/*When the old total paid amount and the old receivable value are equal it means that the receivable was fully paid in one go and that there were no partial payments
	 In this case the difference that must be added to the balance record value is equal to the value of the receivable itself*/
	SET
		v_record_amount_difference = NEW.value - OLD.totalPaidAmount;
	SET
		v_new_balance_record_value = v_current_balance_record_value + v_record_amount_difference;
	SET 
		v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;

/*Covers the situation when a new partial payment is inserted or an existing partial payment value is increased*/
ELSEIF NEW.totalPaidAmount > OLD.totalPaidAmount THEN
	SET
		v_record_amount_difference = NEW.totalPaidAmount - OLD.totalPaidAmount;
	SET
		v_new_balance_record_value = v_current_balance_record_value + v_record_amount_difference;
	SET 
		v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;

/*Covers the situation when a partial payment is deleted or an existing partial payment value is decreased*/
ELSEIF NEW.totalPaidAmount < OLD.totalPaidAmount THEN
	/*CHANGE TO CORRECTLY DISPLAY THE valueTriggeringUpdate VALUE IN THE account_balance_storage_history TABLE!!!*/
	SET 
		v_record_amount_difference = -(OLD.totalPaidAmount - NEW.totalPaidAmount);/*Although the difference might be positive the actual paid amount has decreased by the corresponding difference (which is in fact a negative value)*/
	SET 
		v_new_balance_record_value = v_current_balance_record_value + v_record_amount_difference;
	SET 
		v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;
ELSE 
	/*The receivable value was not changed so the new balance record value is equal to the current record value */
	SET
		v_new_balance_record_value = v_current_balance_record_value;
	SET 
		v_new_storage_balance = v_current_storage_balance;
END IF;

/*Updates the balance record/storage record value only if the new value is different from the current one(there's no point in updating it otherwise */
IF v_new_balance_record_value != v_current_balance_record_value  OR v_new_storage_balance != v_current_storage_balance THEN

UPDATE
	saving_accounts_balance
SET
	value = v_new_balance_record_value
WHERE
	account_ID = OLD.account_ID
	AND MONTH = MONTH(OLD.createdDate)
	AND YEAR = YEAR(OLD.createdDate);
END IF;

IF (OLD.name != NEW.name)
OR (OLD.value != NEW.value)
OR (OLD.debtor_ID != NEW.debtor_ID)
OR (OLD.totalPaidAmount != NEW.totalPaidAmount)
OR (OLD.status_ID != NEW.status_ID)
OR (OLD.createdDate != NEW.createdDate)
OR (OLD.dueDate != NEW.dueDate)

THEN 
/*Logs the receivable update*/
CALL log_receivable_history(NEW.receivableID,
@logOutput);

/*Account balance storage implementation-START*/

SET v_action_source = 'Balance record update on receivable update';

SET v_action_description = 'Receivable update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

SET v_value_triggering_update = v_record_amount_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on receivable deletion` AFTER
DELETE
	ON
	`receivables` FOR EACH ROW BEGIN
DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Retrieves the value of the balance record for the month & year in which the deleted receivable was created*/
SELECT
	value
INTO
	v_current_balance_record_value
FROM
	saving_accounts_balance
WHERE
	account_ID = OLD.account_ID
	AND MONTH = MONTH(OLD.createdDate)
	AND YEAR = YEAR(OLD.createdDate);

/*Calculates the new balance record value as follows:
1.Subtracts the total paid amount of the receivable from the current balance record value (cancels the partial payments)
2.Adds the value of the receivable to the current balance record value (cancels the receivable itself)
The two steps are needed in order to restore the state of the balance record value that existed before the receivable was inserted and
any partial payment was performed*/
SET
v_new_balance_record_value = (v_current_balance_record_value - OLD.totalPaidAmount) + OLD.value;

/*Updates the balance record with the newly calculated value, as explained above*/
UPDATE
	saving_accounts_balance
SET
	value = v_new_balance_record_value
WHERE
	account_ID = OLD.account_ID
	AND MONTH = MONTH(OLD.createdDate)
	AND YEAR = YEAR(OLD.createdDate);

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = OLD.account_ID;

SET v_action_source = 'Balance record update on receivable deletion';

SET v_action_description = 'Receivable delete';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

/*The difference between the old receivable value and the total paid amount is the actual value that triggers the update(in order to show the correct value if partial payments were inserted)*/
SET v_value_triggering_update = OLD.value - OLD.totalPaidAmount; -- to be checked if it calculates the correct value

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = (v_current_storage_balance - OLD.totalPaidAmount) + OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(OLD.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `saving_accounts_balance`
--

DROP TABLE IF EXISTS `saving_accounts_balance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saving_accounts_balance` (
  `recordID` int(10) NOT NULL AUTO_INCREMENT,
  `user_ID` int(10) NOT NULL,
  `account_ID` int(10) NOT NULL,
  `recordName` varchar(50) DEFAULT NULL,
  `value` double NOT NULL,
  `month` int(2) NOT NULL,
  `year` int(4) NOT NULL,
  PRIMARY KEY (`recordID`),
  UNIQUE KEY `account_ID` (`account_ID`,`month`,`year`),
  KEY `user_ID` (`user_ID`),
  CONSTRAINT `saving_accounts_balance_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_balance_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saving_accounts_balance`
--

LOCK TABLES `saving_accounts_balance` WRITE;
/*!40000 ALTER TABLE `saving_accounts_balance` DISABLE KEYS */;
/*!40000 ALTER TABLE `saving_accounts_balance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `saving_accounts_expenses`
--

DROP TABLE IF EXISTS `saving_accounts_expenses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saving_accounts_expenses` (
  `expenseID` int(20) NOT NULL AUTO_INCREMENT,
  `account_ID` int(20) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(20) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`expenseID`),
  KEY `type` (`type`),
  KEY `saving_accounts_expenses_ibfk_3` (`account_ID`),
  CONSTRAINT `saving_accounts_expenses_ibfk_1` FOREIGN KEY (`type`) REFERENCES `expense_types` (`categoryID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_expenses_ibfk_3` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saving_accounts_expenses`
--

LOCK TABLES `saving_accounts_expenses` WRITE;
/*!40000 ALTER TABLE `saving_accounts_expenses` DISABLE KEYS */;
/*!40000 ALTER TABLE `saving_accounts_expenses` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update/creation after SAE insertion` AFTER INSERT ON `saving_accounts_expenses` FOR EACH ROW 
BEGIN
	
DECLARE v_records_number INT(20);

DECLARE v_default_account_ID INT(20);

DECLARE v_record_name VARCHAR(40);

DECLARE v_user_ID INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

SET v_record_name = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

#Retrieves account ID and user ID
SELECT
	acc.accountID,
	usr.userID
INTO
	v_default_account_ID,
	v_user_ID
FROM
	accounts acc
INNER JOIN users usr ON
	acc.user_ID = usr.userID
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = NEW.account_ID
	AND at.typeName LIKE '%SYSTEM_DEFINED%';

#Checks if there is an existing balance record for the month in which the saving account expense is inserted
IF v_default_account_ID > 0 THEN
SELECT
	COUNT(*) 
INTO
	v_records_number
FROM
	saving_accounts_balance
WHERE
	MONTH = MONTH(NEW.date)
	AND YEAR = YEAR(NEW.date)
	AND user_ID = v_user_ID
	AND account_ID = v_default_account_ID;

IF v_records_number = 0 THEN
INSERT
	INTO
	saving_accounts_balance(
user_ID,
	account_ID,
	recordName,
	value,
	MONTH,
	YEAR
)
VALUES (
v_user_ID,
v_default_account_ID,
v_record_name,
-(NEW.value),
MONTH(NEW.date),
YEAR(NEW.date)
);
ELSE
UPDATE
	saving_accounts_balance
SET
	value = value - NEW.value,
	recordName = v_record_name
WHERE
	MONTH = MONTH(NEW.date)
	AND YEAR = YEAR(NEW.date)
	AND user_ID = v_user_ID
	AND account_ID = v_default_account_ID;

END IF;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_default_account_ID;

SET v_action_source = 'Balance record update/creation after SAE creation';

SET v_action_description = 'Saving account expense insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

/*Since the saving account expense insertion leads to a decrease of the current account balance the value triggering the update will be set to negative*/
SET v_value_triggering_update = -NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_default_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END IF;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update after SAE update` AFTER UPDATE ON `saving_accounts_expenses` FOR EACH ROW 
BEGIN

DECLARE v_record_amount_difference INT(20);

DECLARE v_new_record_name VARCHAR(40);

DECLARE v_default_account_ID INT(20);

DECLARE v_user_ID INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

SET v_record_amount_difference = NEW.value - OLD.value;

SET v_new_record_name = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));


#Retrieves account ID and user ID
SELECT
	acc.accountID,
	usr.userID
INTO
	v_default_account_ID,
	v_user_ID
FROM
	accounts acc
INNER JOIN users usr ON
	acc.user_ID = usr.userID
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = NEW.account_ID
	AND at.typeName LIKE '%SYSTEM_DEFINED%';


#Updates the balance record with the difference between the old record value and the new record value
IF v_default_account_ID > 0 THEN
UPDATE
	saving_accounts_balance
SET
	value = COALESCE(value, 0) - v_record_amount_difference,
	recordName = v_new_record_name
WHERE
	MONTH = MONTH(NEW.date)
	AND YEAR = YEAR(NEW.date)
	AND user_ID = v_user_ID
	AND account_ID = v_default_account_ID;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_default_account_ID;

SET v_action_source = 'Balance record update after SAE update';

SET v_action_description = 'Saving account expense update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

/*Sets negative sign to the v_record_amount_difference variable value so that when the saving account expense is updated to a lower value the value triggering update will be
considered positive (balance increase) and when the saving account expense is updated to a higher value the value triggering update will be
considered negative (balance decrease)*/
SET v_value_triggering_update = -v_record_amount_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - v_record_amount_difference;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_default_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update after SAE deletion` AFTER DELETE ON `saving_accounts_expenses` FOR EACH ROW 
BEGIN 
	
DECLARE v_new_record_name VARCHAR(30);

DECLARE v_default_account_ID INT(20);

DECLARE v_user_ID INT(10);

DECLARE v_added_amount INT(20);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

SET v_new_record_name = CONCAT('balance_record_', DATE_FORMAT(CURRENT_DATE,'%Y-%m-%d'));

SET v_added_amount = OLD.value;

#Retrieves account ID and user ID
SELECT
	acc.accountID,
	usr.userID
INTO
	v_default_account_ID,
	v_user_ID
FROM
	accounts acc
INNER JOIN users usr ON
	acc.user_ID = usr.userID
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = OLD.account_ID
	AND at.typeName LIKE '%SYSTEM_DEFINED%';

IF v_default_account_ID > 0 THEN 
UPDATE
	saving_accounts_balance
SET
	recordName = v_new_record_name,
	value = COALESCE(value, 0) + v_added_amount
WHERE
	MONTH = MONTH(OLD.DATE)
	AND YEAR = YEAR(OLD.DATE)
	AND user_ID = v_user_ID
	AND account_ID = v_default_account_ID;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = OLD.account_ID;

SET v_action_source = 'Balance record update after SAE deletion';

SET v_action_description = 'Saving account expense delete';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

/*The value of the deleted saving account expense is the value that triggers the update because it leads to an increase of the corresponding account balance*/
SET v_value_triggering_update = OLD.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(OLD.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `saving_accounts_interest`
--

DROP TABLE IF EXISTS `saving_accounts_interest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saving_accounts_interest` (
  `interestID` int(20) NOT NULL AUTO_INCREMENT,
  `account_ID` int(20) NOT NULL,
  `interestName` varchar(50) DEFAULT NULL,
  `interestType` int(20) NOT NULL,
  `paymentType` int(20) NOT NULL,
  `interestRate` double DEFAULT NULL,
  `value` double NOT NULL,
  `transactionID` varchar(50) DEFAULT NULL,
  `creationDate` date DEFAULT NULL,
  `updatedDate` date DEFAULT NULL,
  PRIMARY KEY (`interestID`),
  KEY `saving_accounts_interest_ibfk_1` (`account_ID`),
  KEY `saving_accounts_interest_ibfk_2` (`interestType`),
  KEY `paymentType` (`paymentType`),
  CONSTRAINT `saving_accounts_interest_ibfk_1` FOREIGN KEY (`account_ID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_interest_ibfk_2` FOREIGN KEY (`interestType`) REFERENCES `interest_types` (`typeID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_interest_ibfk_3` FOREIGN KEY (`paymentType`) REFERENCES `interest_payment_type` (`typeID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saving_accounts_interest`
--

LOCK TABLES `saving_accounts_interest` WRITE;
/*!40000 ALTER TABLE `saving_accounts_interest` DISABLE KEYS */;
/*!40000 ALTER TABLE `saving_accounts_interest` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `External account balance insert/update on interest insertion` AFTER INSERT ON `saving_accounts_interest` FOR EACH ROW BEGIN
DECLARE v_has_balance_record_for_account TINYINT(1);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE DEFAULT 0;

DECLARE v_has_created_balance_record_for_account TINYINT(1);

DECLARE v_has_updated_balance_record_for_account TINYINT(1);

DECLARE v_balance_record_name VARCHAR(50) DEFAULT NULL;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

# Checks if the account has a balance record for the month/year of the interest insertion
CALL has_balance_record_for_selected_month(NEW.account_ID, MONTH(NEW.creationDate), YEAR(NEW.creationDate), v_has_balance_record_for_account);

# Creates a new balance record if it doesn't exist
IF v_has_balance_record_for_account = 0 THEN
	SET v_balance_record_name = CONCAT('balance_record_', 	  CURDATE());
	CALL 							   	create_external_account_balance_record(NEW.account_ID, 	v_balance_record_name, NEW.value, NEW.creationDate, NULL, v_has_created_balance_record_for_account);
    
    IF v_has_created_balance_record_for_account = 0 THEN
    SIGNAL SQLSTATE '45000'
    SET MESSAGE_TEXT = 'Cannot create a new balance record on interest insertion! Check the data and, if necessary, please create the record manually.';
    
    END IF;

# Updates the existing balance record
ELSEIF v_has_balance_record_for_account = 1 THEN

# Retrieves the current balance record
SELECT get_balance_record_value_with_double_precision(NEW.account_ID, NEW.creationDate)
INTO v_current_balance_record_value
FROM DUAL;

# Calculates the new balance record value
SET v_new_balance_record_value = v_current_balance_record_value + NEW.value;

	UPDATE external_accounts_balance
    SET value = v_new_balance_record_value,
    lastUpdatedDate = CURDATE()
    WHERE account_ID = NEW.account_ID
    AND MONTH(createdDate) = MONTH(NEW.creationDate)
    AND YEAR(createdDate) = YEAR(NEW.creationDate);
    
    SET v_has_updated_balance_record_for_account = ROW_COUNT();
    
    IF v_has_updated_balance_record_for_account = 0 THEN
    SIGNAL SQLSTATE '45000'
    SET MESSAGE_TEXT = 'Cannot update the existing balance record on interest insertion! Check the data and, if necessary, please update the record manually.';
    
    END IF;
    
END IF;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = NEW.account_ID;

SET v_action_source = 'External account balance insert/update on interest insertion';

SET v_action_description = 'Interest insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

SET v_value_triggering_update = NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `External account balance update on interest update` AFTER UPDATE ON `saving_accounts_interest` FOR EACH ROW BEGIN
DECLARE v_interest_difference DOUBLE;

DECLARE v_current_record_balance_value DOUBLE;

DECLARE v_new_record_balance_value DOUBLE;

DECLARE v_update_result TINYINT(1);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

#Calculates the difference between the new and old value of the updated interest record
SET v_interest_difference = NEW.value - OLD.value;

# Updates the balance record only if the differnce between the old and the new value is greater or lower than 0
IF v_interest_difference != 0 THEN
# Retrieves the current balance record value
SELECT
	get_balance_record_value_with_double_precision(NEW.account_ID,
	NEW.creationDate)
INTO
	v_current_record_balance_value
FROM
	DUAL;

/*Retrieves the account balance storage record value*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = NEW.account_ID;

/*	IF v_interest_difference > 0 THEN
    SET v_new_record_balance_value = 						v_current_record_balance_value + 						v_interest_difference;
    
    ELSEIF v_interest_difference < 0 THEN
    SET v_new_record_balance_value = 						v_current_record_balance_value -						ABS(v_interest_difference);
    
    END IF;*/


SET v_new_record_balance_value = v_current_record_balance_value + v_interest_difference;

#Updates the balance record value for the respective account, for the month/year of the interest record
UPDATE external_accounts_balance
SET value = v_new_record_balance_value,
lastUpdatedDate = CURDATE()
WHERE account_ID = NEW.account_ID
AND MONTH(createdDate) = MONTH(NEW.creationDate)
AND YEAR(createdDate) = YEAR(NEW.creationDate);

# Retrieves the update result
SET v_update_result = ROW_COUNT();

# Checks the update result and throws and error if the operation failed
IF v_update_result = 0 THEN
SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = 'Unable to update the external account balance record on interest update! Please check the data and, if necessary, update the record manually.';

END IF;

/*Account balance storage implementation-START*/

SET v_action_source = 'External account balance update on interest update';

SET v_action_description = 'Interest update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

SET v_value_triggering_update = v_interest_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + v_interest_difference;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/
   
END IF;
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `External account balance update after interest deletion` AFTER DELETE ON `saving_accounts_interest` FOR EACH ROW BEGIN 
DECLARE v_current_record_balance_value DOUBLE;

DECLARE v_new_record_balance_value DOUBLE;

DECLARE v_update_result TINYINT(1);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

# Retrieves the current balance record value
SELECT get_balance_record_value_with_double_precision(OLD.account_ID,
OLD.creationDate)
INTO v_current_record_balance_value
FROM DUAL; 

# Calculates the new balance record value
SET v_new_record_balance_value = v_current_record_balance_value - OLD.value;

#Updates the balance record value for the respective account, for the month/year of the interest record
UPDATE external_accounts_balance
SET value = v_new_record_balance_value,
lastUpdatedDate = CURDATE()
WHERE account_ID = OLD.account_ID
AND MONTH(createdDate) = MONTH(OLD.creationDate)
AND YEAR(createdDate) = YEAR(OLD.creationDate);

# Retrieves the update result
SET v_update_result = ROW_COUNT();

# Checks the update result an throws and error if the operation failed
IF v_update_result = 0 THEN
SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = 'Unable to update the external account balance record on interest deletion! Please check the data and, if necessary, update the record manually.';

END IF;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = OLD.account_ID;

SET v_action_source = 'External account balance update after interest deletion';

SET v_action_description = 'Interest deletion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

SET v_value_triggering_update = -OLD.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(OLD.account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/


END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `saving_accounts_transfers`
--

DROP TABLE IF EXISTS `saving_accounts_transfers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saving_accounts_transfers` (
  `transferID` int(10) NOT NULL AUTO_INCREMENT,
  `senderAccountID` int(10) NOT NULL,
  `receivingAccountID` int(10) DEFAULT NULL,
  `transferName` varchar(50) DEFAULT NULL,
  `sentValue` double DEFAULT NULL,
  `receivedValue` double DEFAULT NULL,
  `exchangeRate` double DEFAULT NULL,
  `transactionID` varchar(50) DEFAULT NULL,
  `observations` varchar(150) DEFAULT NULL,
  `transferDate` date DEFAULT NULL,
  PRIMARY KEY (`transferID`),
  KEY `senderAccountID` (`senderAccountID`),
  KEY `receivingAccountID` (`receivingAccountID`),
  CONSTRAINT `saving_accounts_transfers_ibfk_1` FOREIGN KEY (`senderAccountID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_transfers_ibfk_2` FOREIGN KEY (`receivingAccountID`) REFERENCES `accounts` (`accountID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saving_accounts_transfers`
--

LOCK TABLES `saving_accounts_transfers` WRITE;
/*!40000 ALTER TABLE `saving_accounts_transfers` DISABLE KEYS */;
/*!40000 ALTER TABLE `saving_accounts_transfers` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Saving accounts balance update on insert` AFTER
INSERT
	ON
	`saving_accounts_transfers` FOR EACH ROW 
BEGIN

DECLARE v_has_balance_record_for_sender_account TINYINT(1);

DECLARE v_has_balance_record_for_receiving_account TINYINT(1);

DECLARE v_has_created_sender_account_balance_record TINYINT(1);

DECLARE v_has_created_receiving_account_balance_record TINYINT(1);

DECLARE v_has_updated_balance_record TINYINT(1);

DECLARE v_user_ID INT;

DECLARE v_current_storage_balance_sender_account DOUBLE;

DECLARE v_current_storage_balance_receiving_account DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update_sender_account DOUBLE;

DECLARE v_value_triggering_update_receiving_account DOUBLE;

DECLARE v_old_storage_balance_sender_account DOUBLE;

DECLARE v_new_storage_balance_sender_account DOUBLE;

DECLARE v_old_storage_balance_receiving_account DOUBLE;

DECLARE v_new_storage_balance_receiving_account DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result_sender_account TINYINT(1);

DECLARE v_account_balance_storage_update_result_receiving_account TINYINT(1);

/*Checks if the sender account has balance record for the current month/year of the transfer*/
CALL has_balance_record_for_selected_month(NEW.senderAccountID,
MONTH(NEW.transferDate),
YEAR(NEW.transferDate),
v_has_balance_record_for_sender_account);


/*Checks if the receiving account has balance record for the current month/year of the transfer*/
CALL has_balance_record_for_selected_month(NEW.receivingAccountID,
MONTH(NEW.transferDate),
YEAR(NEW.transferDate),
v_has_balance_record_for_receiving_account);


/*Selects the user ID based on the default saving account ID-for DEFAULT ACCOUNT only!!*/
SELECT
	user_ID 
INTO
	v_user_ID
FROM
	accounts
WHERE
	accountID = NEW.senderAccountID
LIMIT 1;

/*Creates balance record for sender account if it doesn't exist or updates the current one*/
IF v_has_balance_record_for_sender_account = 0 THEN
	CALL create_account_balance_record_on_transfer(v_user_ID,
NEW.senderAccountID,
-NEW.sentValue,
NEW.transferDate,
v_has_created_sender_account_balance_record);
ELSE 
	CALL update_account_balance_on_transfer_insert(NEW.senderAccountID,
NEW.sentValue,
NEW.transferDate,
'SENDER',
v_has_updated_balance_record);
END IF;

/*Creates balance record for receiving account if it doesn't exist or updates the current one*/
IF v_has_balance_record_for_receiving_account = 0 THEN
	CALL create_account_balance_record_on_transfer(v_user_ID,
NEW.receivingAccountID,
NEW.receivedValue,
NEW.transferDate,
v_has_created_receiving_account_balance_record);
ELSE
	CALL update_account_balance_on_transfer_insert(NEW.receivingAccountID,
NEW.receivedValue,
NEW.transferDate,
'RECEIVER',
v_has_updated_balance_record);
END IF;

/*Account balance storage implementation-START*/

/*Account balance retrieval for the sender account*/
SELECT
	currentBalance
INTO
	v_current_storage_balance_sender_account
FROM
	account_balance_storage
WHERE
	account_ID = NEW.senderAccountID;

/*Account balance retrieval for the receiving account*/
SELECT
	currentBalance
INTO
	v_current_storage_balance_receiving_account
FROM
	account_balance_storage
WHERE
	account_ID = NEW.receivingAccountID;

SET v_action_source = 'Saving accounts balance update on insert';

SET v_action_description = 'Transfer insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

/*The transfer insertion leads to a decrease of the sender account balance the value triggering the update will be negative*/
SET v_value_triggering_update_sender_account = -NEW.sentValue;

/*The transfer insertion leads to an increase of the receiving account balance so the value triggering the update will be positive*/
SET v_value_triggering_update_receiving_account = NEW.receivedValue;

/*Sets the old storage balance for the sender and receiving account (the data  will be inserted into the account_balance_storage_history table)*/
SET v_old_storage_balance_sender_account = v_current_storage_balance_sender_account;

SET v_old_storage_balance_receiving_account = v_current_storage_balance_receiving_account;

/*Sets the new storage balance for the sender and receiving account (the data will be inserted into the account_balance_storage_history table)*/
SET v_new_storage_balance_sender_account = v_current_storage_balance_sender_account - NEW.sentValue;

SET v_new_storage_balance_receiving_account = v_current_storage_balance_receiving_account + NEW.receivedValue;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the sender account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.senderAccountID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update_sender_account,
v_old_storage_balance_sender_account,
v_new_storage_balance_sender_account,
v_action_timestamp,
v_account_balance_storage_update_result_sender_account);

/*Updates the current balance of the receiving account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(NEW.receivingAccountID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update_receiving_account,
v_old_storage_balance_receiving_account,
v_new_storage_balance_receiving_account,
v_action_timestamp,
v_account_balance_storage_update_result_receiving_account);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Saving accounts balance update on update` AFTER
UPDATE
	ON
	`saving_accounts_transfers` FOR EACH ROW BEGIN
DECLARE v_sender_account_update_result TINYINT(1) DEFAULT 0;

DECLARE v_sender_storage_update_result TINYINT(1) DEFAULT 0;

DECLARE v_receiver_account_update_result TINYINT(1);

DECLARE v_receiver_storage_update_result TINYINT(1);

/*Updates the balance records only if the sentValue or receivedValue was changed*/
IF NEW.sentValue != OLD.sentValue
OR NEW.receivedValue != OLD.receivedValue THEN
	CALL update_account_balance_on_transfer_update(OLD.senderAccountID,
OLD.receivingAccountID,
NEW.sentValue,
OLD.sentValue,
NEW.receivedValue,
OLD.receivedValue,
OLD.transferDate,
v_sender_account_update_result,
v_receiver_account_update_result,
v_sender_storage_update_result,
v_receiver_storage_update_result);

/*Raises errors if any of the balance records could not be updated*/
IF v_sender_account_update_result = 0 THEN
SIGNAL SQLSTATE '45000'
SET
MESSAGE_TEXT = 'Unable to update the balance record for the sender account! Please check the data and, if necessary, update the record manually.';
END IF;

IF v_receiver_account_update_result = 0 THEN
SIGNAL SQLSTATE '45000'
SET
MESSAGE_TEXT = 'Unable to update the balance record for the receiving account! Please check the data and, if necessary, update the record manually.';
END IF;
END IF;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Saving accounts balance update on delete` AFTER
DELETE
	ON
	`saving_accounts_transfers` FOR EACH ROW BEGIN
DECLARE v_sender_account_update_result TINYINT(1);

DECLARE v_receiver_account_update_result TINYINT(1);

DECLARE v_sender_storage_update_result TINYINT(1);

DECLARE v_receiver_storage_update_result TINYINT(1);

/*Updates the sender and receiving account balance records after the transfer deletion*/
CALL update_account_balance_on_transfer_delete(OLD.senderAccountID,
OLD.receivingAccountID,
OLD.sentValue,
OLD.receivedValue,
OLD.transferDate,
v_sender_account_update_result,
v_sender_storage_update_result,
v_receiver_account_update_result,
v_receiver_storage_update_result);

/*Checks the update results for both accounts and triggers exceptions if any of them failed*/
IF v_sender_account_update_result = 0 THEN
SIGNAL SQLSTATE '45000'
SET
MESSAGE_TEXT = 'Unable to update the sender account balance record after transfer deletion! Please check the data and, if necessary, update the record manually.';
END IF;

IF v_receiver_account_update_result = 0 THEN
SIGNAL SQLSTATE '45000'
SET
MESSAGE_TEXT = 'Unable to update the receiver account balance record after transfer deletion! Please check the data and, if necessary, update the record manually.';
END IF;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `savings`
--

DROP TABLE IF EXISTS `savings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `savings` (
  `savingID` int(10) NOT NULL AUTO_INCREMENT,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`savingID`),
  UNIQUE KEY `name` (`name`),
  UNIQUE KEY `date` (`date`),
  KEY `user_ID` (`user_ID`),
  CONSTRAINT `savings_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `savings`
--

LOCK TABLES `savings` WRITE;
/*!40000 ALTER TABLE `savings` DISABLE KEYS */;
/*!40000 ALTER TABLE `savings` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update/creation on saving insertion` AFTER INSERT ON `savings` FOR EACH ROW 
BEGIN

DECLARE v_records_number INT(10);

DECLARE v_default_account_ID INT(20);

DECLARE v_record_name VARCHAR(40);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);


SET v_record_name = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

SELECT
	acc.accountID
INTO
	v_default_account_ID
FROM
	accounts acc
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.user_ID = NEW.user_ID
	AND at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

IF v_default_account_ID IS NOT NULL THEN
SELECT
	COUNT(*) 
INTO
	v_records_number
FROM
	saving_accounts_balance
WHERE
	MONTH = MONTH(NEW.date)
	AND YEAR = YEAR(NEW.date)
	AND user_ID = NEW.user_ID
	AND account_ID = v_default_account_ID;


IF v_records_number = 0 THEN

INSERT
	INTO
	saving_accounts_balance(
    user_ID,
	account_ID,
	recordName,
	value,
	MONTH,
	YEAR
)
VALUES (
NEW.user_ID,
v_default_account_ID,
v_record_name,
NEW.value,
MONTH(NEW.date),
YEAR(NEW.date)
);

ELSE

UPDATE
	saving_accounts_balance
SET
	value = value + NEW.value,
	recordName = v_record_name
WHERE
	MONTH = MONTH(NEW.date)
	AND YEAR = YEAR(NEW.date)
	AND user_ID = NEW.user_ID
	AND account_ID = v_default_account_ID;


/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_default_account_ID;

SET v_action_source = 'Balance record update/creation on saving insertion';

SET v_action_description = 'Saving insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

SET v_value_triggering_update = NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_default_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END IF;
END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on saving update` AFTER UPDATE ON `savings` FOR EACH ROW 
BEGIN

DECLARE v_record_amount_difference INT(10);

DECLARE v_new_record_name VARCHAR(30);

DECLARE v_default_account_ID INT(10);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);


SET v_record_amount_difference = NEW.value - OLD.value;

SET v_new_record_name = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

SELECT
	acc.accountID
INTO
	v_default_account_ID
FROM
	accounts acc
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.user_ID = NEW.user_ID
	AND at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

IF v_default_account_ID IS NOT NULL THEN

UPDATE
	saving_accounts_balance
SET
	recordName = v_new_record_name,
	value = value + v_record_amount_difference
WHERE
	MONTH = MONTH(NEW.date)
	AND YEAR = YEAR(NEW.date)
	AND user_ID = NEW.user_ID
	AND account_ID = v_default_account_ID;

END IF;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_default_account_ID;

SET v_action_source = 'Balance record update on saving update';

SET v_action_description = 'Saving update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

SET v_value_triggering_update = v_record_amount_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_default_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on saving deletion` AFTER DELETE ON `savings` FOR EACH ROW 
BEGIN

DECLARE v_new_record_name VARCHAR(30);

DECLARE v_default_account_ID INT(10);

DECLARE v_subtracted_amount INT(10);

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);


SET v_new_record_name = CONCAT('balance_record_', DATE_FORMAT(CURRENT_DATE,'%Y-%m-%d'));

SET v_subtracted_amount = OLD.value;

SELECT
	acc.accountID
INTO
	v_default_account_ID
FROM
	accounts acc
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.user_ID = OLD.user_ID
	AND at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

IF v_default_account_ID IS NOT NULL THEN

UPDATE
	saving_accounts_balance
SET
	recordName = v_new_record_name,
	value = value - v_subtracted_amount
WHERE
	MONTH = MONTH(OLD.DATE)
	AND YEAR = YEAR(OLD.DATE)
	AND user_ID = OLD.user_ID
	AND account_ID = v_default_account_ID;
END IF;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_default_account_ID;

SET v_action_source = 'Balance record update on saving deletion';

SET v_action_description = 'Saving deletion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

SET v_value_triggering_update = -OLD.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_default_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

/*Account balance storage implementation-END*/

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `stock_option_plan_items`
--

DROP TABLE IF EXISTS `stock_option_plan_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_option_plan_items` (
  `itemID` int(20) NOT NULL AUTO_INCREMENT,
  `plan_ID` int(20) NOT NULL,
  `symbol_ID` int(20) NOT NULL,
  `pricePerUnit` double DEFAULT NULL,
  `totalQty` int(20) DEFAULT NULL,
  PRIMARY KEY (`itemID`),
  KEY `plan_ID` (`plan_ID`),
  KEY `symbol_ID` (`symbol_ID`),
  CONSTRAINT `stock_option_plan_items_ibfk_1` FOREIGN KEY (`plan_ID`) REFERENCES `stock_option_plans` (`planID`) ON UPDATE CASCADE,
  CONSTRAINT `stock_option_plan_items_ibfk_2` FOREIGN KEY (`symbol_ID`) REFERENCES `symbols` (`symbolID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_option_plan_items`
--

LOCK TABLES `stock_option_plan_items` WRITE;
/*!40000 ALTER TABLE `stock_option_plan_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `stock_option_plan_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stock_option_plans`
--

DROP TABLE IF EXISTS `stock_option_plans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_option_plans` (
  `planID` int(20) NOT NULL AUTO_INCREMENT,
  `planName` varchar(50) DEFAULT NULL,
  `companyName` varchar(50) DEFAULT NULL,
  `investor_ID` int(20) NOT NULL,
  `portfolio_ID` int(20) NOT NULL,
  `createdDate` date DEFAULT NULL,
  PRIMARY KEY (`planID`),
  KEY `investorID` (`investor_ID`),
  KEY `portfolio_ID` (`portfolio_ID`),
  CONSTRAINT `stock_option_plans_ibfk_1` FOREIGN KEY (`investor_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `stock_option_plans_ibfk_2` FOREIGN KEY (`portfolio_ID`) REFERENCES `portfolios` (`portfolioID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_option_plans`
--

LOCK TABLES `stock_option_plans` WRITE;
/*!40000 ALTER TABLE `stock_option_plans` DISABLE KEYS */;
/*!40000 ALTER TABLE `stock_option_plans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stock_order_actions`
--

DROP TABLE IF EXISTS `stock_order_actions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_order_actions` (
  `actionID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`actionID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_order_actions`
--

LOCK TABLES `stock_order_actions` WRITE;
/*!40000 ALTER TABLE `stock_order_actions` DISABLE KEYS */;
INSERT INTO `stock_order_actions` VALUES (1,'BUY'),(2,'SELL');
/*!40000 ALTER TABLE `stock_order_actions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stock_order_items`
--

DROP TABLE IF EXISTS `stock_order_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_order_items` (
  `itemID` int(20) NOT NULL AUTO_INCREMENT,
  `order_ID` int(20) NOT NULL,
  `symbol_ID` int(20) NOT NULL,
  `pricePerUnit` double DEFAULT NULL,
  `totalQty` int(20) DEFAULT NULL,
  PRIMARY KEY (`itemID`),
  KEY `order_ID` (`order_ID`),
  KEY `symbol_ID` (`symbol_ID`),
  CONSTRAINT `stock_order_items_ibfk_1` FOREIGN KEY (`order_ID`) REFERENCES `stock_orders` (`orderID`) ON UPDATE CASCADE,
  CONSTRAINT `stock_order_items_ibfk_2` FOREIGN KEY (`symbol_ID`) REFERENCES `symbols` (`symbolID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_order_items`
--

LOCK TABLES `stock_order_items` WRITE;
/*!40000 ALTER TABLE `stock_order_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `stock_order_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stock_orders`
--

DROP TABLE IF EXISTS `stock_orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_orders` (
  `orderID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `totalValue` double DEFAULT NULL,
  `action_ID` int(20) NOT NULL,
  `portfolio_ID` int(20) NOT NULL,
  `currency_ID` int(20) NOT NULL,
  `createdDate` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`orderID`),
  KEY `broker_ID` (`action_ID`),
  KEY `currency_ID` (`currency_ID`),
  KEY `portfolio_ID` (`portfolio_ID`),
  CONSTRAINT `stock_orders_ibfk_2` FOREIGN KEY (`portfolio_ID`) REFERENCES `portfolios` (`portfolioID`) ON UPDATE CASCADE,
  CONSTRAINT `stock_orders_ibfk_3` FOREIGN KEY (`currency_ID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE,
  CONSTRAINT `stock_orders_ibfk_4` FOREIGN KEY (`action_ID`) REFERENCES `stock_order_actions` (`actionID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_orders`
--

LOCK TABLES `stock_orders` WRITE;
/*!40000 ALTER TABLE `stock_orders` DISABLE KEYS */;
/*!40000 ALTER TABLE `stock_orders` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Update investment account balance on stock order insert`
AFTER
INSERT
	ON
	stock_orders FOR EACH ROW
BEGIN
	
DECLARE v_has_investment_account INT(20);

DECLARE v_investment_account_current_balance INT(20);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_balance_record_name VARCHAR(40);

DECLARE v_error_message VARCHAR(200);

DECLARE v_stock_order_action VARCHAR(20);

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_has_balance_record TINYINT(1);

DECLARE v_record_creation_result TINYINT(1);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

-- Checks if the broker to which this order is associated has an investment account whose account balance can be updated 
SELECT
	COUNT(*)
INTO
	v_has_investment_account
FROM portfolios ps
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_types act ON
	acc.type_ID = act.typeID
WHERE
	ps.portfolioID = NEW.portfolio_ID
AND act.typeName = 'USER_DEFINED-CUSTOM_INVESTMENT_ACCOUNT';

IF v_has_investment_account = 0 THEN
SET v_error_message = 'No investment account was found for the specified broker! Please create an account before inserting orders.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

-- Retrieves the action type of the order (BUY/SELL) 
SELECT
	name
INTO
	v_stock_order_action
FROM
	stock_order_actions
WHERE
	actionID = NEW.action_ID;

-- Retrieves the current balance of the investment account, its account ID and the investor ID
SELECT
	abs.currentBalance,
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_current_balance,
	v_investment_account_ID,
	v_investor_ID	
FROM portfolios ps 
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	ps.portfolioID = NEW.portfolio_ID;

IF NEW.totalValue > v_investment_account_current_balance THEN
SET v_error_message = 'The order value cannot exceed the current available balance of the investment account! Please update the order value or increase the account balance.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

/*Sets the name of the new/updated balance record*/
SET v_balance_record_name = CONCAT('balance_record_', DATE(NEW.createdDate));

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(NEW.createdDate);
SET v_balance_record_year = YEAR(NEW.createdDate);

/*Checks if a balance record exists for the current  month and year*/
CALL has_balance_record_for_selected_month(v_investment_account_ID,
v_balance_record_month,
v_balance_record_year,
v_has_balance_record);

IF (v_has_balance_record = 0) THEN

-- For BUY orders the investment account is debited (amount withdrawn) while for the SELL orders the investment account is credited (amount deposited)
IF v_stock_order_action = 'BUY' THEN
	SET v_new_balance_record_value = -NEW.totalValue;
ELSEIF v_stock_order_action = 'SELL' THEN
	SET v_new_balance_record_value = NEW.totalValue;
END IF;

/*Creates a new balance record for the external account*/
CALL create_external_account_balance_record(v_investment_account_ID,
v_balance_record_name,
v_new_balance_record_value,
NEW.createdDate,
NULL,
v_record_creation_result);

ELSE 
/*Balance record value update branch*/
/*Retrieves the value of the existing external account balance record for the month and year on which the banking fee is inserted*/
SELECT
	eab.value
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance eab
WHERE
	MONTH(eab.createdDate) = v_balance_record_month
AND YEAR(eab.createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

IF v_stock_order_action = 'BUY' THEN
	SET v_new_balance_record_value = v_current_balance_record_value - NEW.totalValue;
ELSEIF v_stock_order_action = 'SELL' THEN
	SET v_new_balance_record_value = v_current_balance_record_value + NEW.totalValue;
END IF;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;
END IF;

-- Accounts balance storage update logic
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on stock order insertion';

SET v_action_description = 'Stock order insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

SET v_old_storage_balance = v_current_storage_balance;

-- Sets the new account balance and the value triggering the update based on the order's action type
IF v_stock_order_action = 'BUY' THEN
    SET v_value_triggering_update = -NEW.totalValue;
	SET v_new_storage_balance = v_current_storage_balance - NEW.totalValue;
ELSEIF v_stock_order_action = 'SELL' THEN
	SET v_value_triggering_update = NEW.totalValue;
	SET v_new_storage_balance = v_current_storage_balance + NEW.totalValue;
END IF;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Update investment account balance on stock order update`
AFTER UPDATE
	ON 
	stock_orders FOR EACH ROW
BEGIN

DECLARE v_has_investment_account INT(20);

DECLARE v_error_message VARCHAR(200);

DECLARE v_stock_order_action VARCHAR(20);

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_investment_account_current_balance INT(20);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_record_amount_difference DOUBLE;

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

-- The order type cannot be changed because this would lead to an overly complicated logic for calculating the account balance
IF NEW.action_ID != OLD.action_ID THEN
SET v_error_message = 'It is not allowed to change the order type! Please delete the incorrect order and re-insert it with the correct type.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

-- Checks if the broker to which this order is associated has an investment account whose account balance can be updated 
SELECT
	COUNT(*)
INTO
	v_has_investment_account
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
WHERE
	so.orderID = NEW.orderID;

IF v_has_investment_account = 0 THEN
SET v_error_message = 'No investment account was found for the specified broker! Please create an account before updating orders.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

-- Retrieves the action type of the order (BUY/SELL) 
SELECT
	name
INTO
	v_stock_order_action
FROM
	stock_order_actions
WHERE
	actionID = OLD.action_ID;

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(NEW.createdDate);
SET v_balance_record_year = YEAR(NEW.createdDate);

SELECT
	abs.currentBalance,
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_current_balance,
	v_investment_account_ID,
	v_investor_ID	
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	so.orderID = NEW.orderID;

SET v_record_amount_difference = NEW.totalValue - OLD.totalValue;

-- This check is performed only for the BUY orders because they withdraw money. 
-- For SELL orders, which deposit money, any change will just update the balance and no additional check will have to be performed.
IF (v_record_amount_difference > v_investment_account_current_balance AND v_stock_order_action = 'BUY') THEN
SET v_error_message = 'The difference between the new value and the old value cannot exceed the current available balance of the investment account! Please update the order value or increase the account balance.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

-- Monthly account balance update logic
/*Balance record value update branch*/
/*Retrieves the value of the existing external account balance record for the month and year on which the stock order is inserted*/
SELECT
	eab.value
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance eab
WHERE
	MONTH(eab.createdDate) = v_balance_record_month
AND YEAR(eab.createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

-- For BUY orders the investment account is debited (amount withdrawn) while for the SELL orders the investment account is credited (amount deposited)
IF v_stock_order_action = 'BUY' THEN
	SET v_new_balance_record_value = v_current_balance_record_value - v_record_amount_difference;
ELSEIF v_stock_order_action = 'SELL' THEN
	SET v_new_balance_record_value = v_current_balance_record_value + v_record_amount_difference;
END IF;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;


-- Accounts balance storage update logic
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on stock order update';

SET v_action_description = 'Stock order update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

SET v_old_storage_balance = v_current_storage_balance;

-- Sets the new account balance and the value triggering the update based on the order's action type
IF v_stock_order_action = 'BUY' THEN
	/*In case of BUY orders if the new value of the order is lower than the initial value it menas that less money was withdrawn from
    from the account hence the difference must be set as positive (the account balance increases)*/
	IF v_record_amount_difference < 0 THEN
    	SET v_value_triggering_update = ABS(v_record_amount_difference);
    ELSE
    	SET v_value_triggering_update = -v_record_amount_difference;
    END IF;
   
	SET v_new_storage_balance = v_current_storage_balance - v_record_amount_difference;

ELSEIF v_stock_order_action = 'SELL' THEN
    SET v_value_triggering_update = v_record_amount_difference;
   
	SET v_new_storage_balance = v_current_storage_balance + v_record_amount_difference;
END IF;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Update investment account balance on stock order deletion`
AFTER DELETE
	ON 
	stock_orders FOR EACH ROW	
BEGIN

DECLARE v_has_investment_account INT(20);

DECLARE v_stock_order_action VARCHAR(20);

DECLARE v_error_message VARCHAR(200);

DECLARE v_investment_account_current_balance INT(20);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

-- Checks if the broker to which this order is associated has an investment account whose account balance can be updated 
SELECT
	COUNT(*)
INTO
	v_has_investment_account
FROM portfolios ps
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_types act ON
	acc.type_ID = act.typeID
WHERE
	ps.portfolioID = OLD.portfolio_ID
AND act.typeName = 'USER_DEFINED-CUSTOM_INVESTMENT_ACCOUNT';

IF v_has_investment_account = 0 THEN
SET v_error_message = 'No investment account was found for the specified broker! Please create an account before updating orders.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

-- Retrieves the action type of the order (BUY/SELL) 
SELECT
	name
INTO
	v_stock_order_action
FROM
	stock_order_actions
WHERE
	actionID = OLD.action_ID;

-- Retrieves the current balance of the investment account, its account ID and the investor ID
SELECT
	abs.currentBalance,
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_current_balance,
	v_investment_account_ID,
	v_investor_ID	
FROM portfolios ps 
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	ps.portfolioID = OLD.portfolio_ID;

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(OLD.createdDate);
SET v_balance_record_year = YEAR(OLD.createdDate);

/*Retrieves the value of the balance record for the month & year in which the deleted stock order was created*/
SELECT
	COALESCE(value, 0)
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance
WHERE
	account_ID = v_investment_account_ID
	AND MONTH(createdDate) = v_balance_record_month
	AND YEAR(createdDate) = v_balance_record_year;

-- For BUY orders the investment account is debited (amount withdrawn) while for the SELL orders the investment account is credited (amount deposited)
IF v_stock_order_action = 'BUY' THEN
	SET v_new_balance_record_value = v_current_balance_record_value + OLD.totalValue;
ELSEIF v_stock_order_action = 'SELL' THEN
	SET v_new_balance_record_value = v_current_balance_record_value - OLD.totalValue;
END IF;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

-- Accounts balance storage update logic
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on stock order deletion';

SET v_action_description = 'Stock order deletion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

SET v_old_storage_balance = v_current_storage_balance;

-- Sets the new account balance and the value triggering the update based on the order's action type
IF v_stock_order_action = 'BUY' THEN
    SET v_value_triggering_update = OLD.totalValue;
	SET v_new_storage_balance = v_current_storage_balance + OLD.totalValue;
ELSEIF v_stock_order_action = 'SELL' THEN
	SET v_value_triggering_update = -OLD.totalValue;
	SET v_new_storage_balance = v_current_storage_balance - OLD.totalValue;
END IF;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `symbols`
--

DROP TABLE IF EXISTS `symbols`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `symbols` (
  `symbolID` int(20) NOT NULL AUTO_INCREMENT,
  `symbolName` varchar(4) DEFAULT NULL,
  `isin` varchar(20) DEFAULT NULL,
  `companyName` varchar(50) DEFAULT NULL,
  `country_ID` int(20) NOT NULL,
  `industry_ID` int(20) NOT NULL,
  PRIMARY KEY (`symbolID`),
  KEY `country_ID` (`country_ID`),
  KEY `industry_ID` (`industry_ID`),
  CONSTRAINT `symbols_ibfk_1` FOREIGN KEY (`country_ID`) REFERENCES `countries` (`countryID`) ON UPDATE CASCADE,
  CONSTRAINT `symbols_ibfk_2` FOREIGN KEY (`industry_ID`) REFERENCES `industries` (`industryID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `symbols`
--

LOCK TABLES `symbols` WRITE;
/*!40000 ALTER TABLE `symbols` DISABLE KEYS */;
/*!40000 ALTER TABLE `symbols` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tax_settings`
--

DROP TABLE IF EXISTS `tax_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tax_settings` (
  `settingID` int(20) NOT NULL AUTO_INCREMENT,
  `settingName` varchar(50) DEFAULT NULL,
  `settingDescription` varchar(100) DEFAULT NULL,
  `paramDescription1` varchar(100) DEFAULT NULL,
  `paramValue1` double DEFAULT NULL,
  `paramDescription2` varchar(100) DEFAULT NULL,
  `paramValue2` double DEFAULT NULL,
  `paramDescription3` varchar(100) DEFAULT NULL,
  `paramValue3` double DEFAULT NULL,
  `paramDescription4` varchar(100) DEFAULT NULL,
  `paramValue4` double DEFAULT NULL,
  PRIMARY KEY (`settingID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tax_settings`
--

LOCK TABLES `tax_settings` WRITE;
/*!40000 ALTER TABLE `tax_settings` DISABLE KEYS */;
INSERT INTO `tax_settings` VALUES (1,'MINIMUM_GROSS_SALARY','The value of the minimum gross salary guaranteed by the state','Gross salary value',3700,NULL,NULL,NULL,NULL,NULL,NULL),(2,'HEALTH_INSURANCE_TAX','The tax owed for supporting the national healthcare system','Tax percentage',10,NULL,NULL,NULL,NULL,NULL,NULL),(3,'TAX_THRESHOLD_1','The first health insurance tax threshold','Low limit for the first threshold expressed as number of minimum gross salaries',6,'High limit for the first threshold expressed as number of minimum gross salaries',12,NULL,NULL,NULL,NULL),(4,'TAX_THRESHOLD_2','The second health insurance tax threshold','Low limit for the second threshold expressed as number of minimum gross salaries',12,'High limit for the second threshold expressed as number of minimum gross salaries',24,NULL,NULL,NULL,NULL),(5,'TAX_THRESHOLD_3','The third health insurance tax threshold','Low limit for the third threshold expressed as number of minimum gross salaries',24,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `tax_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trading_fees`
--

DROP TABLE IF EXISTS `trading_fees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trading_fees` (
  `tradingFeeID` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `value` double DEFAULT NULL,
  `order_ID` int(20) NOT NULL,
  `createdDate` date DEFAULT NULL,
  PRIMARY KEY (`tradingFeeID`),
  KEY `order_ID` (`order_ID`),
  CONSTRAINT `trading_fees_ibfk_1` FOREIGN KEY (`order_ID`) REFERENCES `stock_orders` (`orderID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trading_fees`
--

LOCK TABLES `trading_fees` WRITE;
/*!40000 ALTER TABLE `trading_fees` DISABLE KEYS */;
/*!40000 ALTER TABLE `trading_fees` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Update investment account balance on trading fee insert`
AFTER INSERT
	ON 
	trading_fees FOR EACH ROW
BEGIN
	
DECLARE v_has_investment_account INT(20);

DECLARE v_error_message VARCHAR(200);

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_investment_account_current_balance INT(20);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_has_balance_record TINYINT(1);

DECLARE v_balance_record_name VARCHAR(50);

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_record_creation_result TINYINT(1);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

SELECT
	COUNT(*)
INTO
	v_has_investment_account
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
WHERE
	so.orderID = NEW.order_ID;

IF v_has_investment_account = 0 THEN
SET v_error_message = 'No investment account was found for the specified broker! Please create an account before inserting trading fees.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(NEW.createdDate);
SET v_balance_record_year = YEAR(NEW.createdDate);

SELECT
	abs.currentBalance,
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_current_balance,
	v_investment_account_ID,
	v_investor_ID	
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	so.orderID = NEW.order_ID;

/*Checks the balance record existence*/
CALL has_balance_record_for_selected_month(v_investment_account_ID,
v_balance_record_month,
v_balance_record_year,
v_has_balance_record);

/*Sets the name of the new/updated balance record*/
SET v_balance_record_name = CONCAT('balance_record_', NEW.createdDate);

IF (v_has_balance_record = 0) THEN
/*Balance record creation branch*/
/*The new balance record value will be set to the negative value of the trading fee since money is withdrawn from the account in this case*/
SET v_new_balance_record_value = -NEW.value;

/*Creates a new balance record for the external account*/
CALL create_external_account_balance_record(v_investment_account_ID,
v_balance_record_name,
v_new_balance_record_value,
NEW.createdDate,
NULL,
v_record_creation_result);

ELSE 
/*Balance record value update branch*/
/*Retrieves the value of the existing external account balance record for the month and year on which the trading fee is inserted*/
SELECT
	eab.value
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance eab
WHERE
	MONTH(eab.createdDate) = v_balance_record_month
AND YEAR(eab.createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

/*Calculates the new balance record value (the trading fee value is subtracted from the current value of the balance record*/
SET v_new_balance_record_value = v_current_balance_record_value - NEW.value;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;
END IF;

/*Account balance storage implementation-START*/

SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on trading fee insertion';

SET v_action_description = 'Trading fee insertion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'INSERT';

/*Since the trading fee insertion leads to a decrease of the current account balance the value triggering the update will be set to negative*/
SET v_value_triggering_update = -NEW.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - NEW.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);
		
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Update investment account balance on trading fee update`
AFTER UPDATE
	ON 
	trading_fees FOR EACH ROW
BEGIN
	
DECLARE v_has_investment_account INT(20);

DECLARE v_error_message VARCHAR(200);

DECLARE v_record_amount_difference DOUBLE;

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_investment_account_current_balance INT(20);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);

/*Check is added to prevent the execution of the trigger logic when the value of the record is unchanged*/
IF (NEW.value != OLD.value) THEN

SELECT
	COUNT(*)
INTO
	v_has_investment_account
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
WHERE
	so.orderID = NEW.order_ID;

IF v_has_investment_account = 0 THEN
SET v_error_message = 'No investment account was found for the specified broker! Please create an account before updating trading fees.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

SET v_record_amount_difference = NEW.value - OLD.value;

IF v_record_amount_difference > v_investment_account_current_balance THEN
SET v_error_message = 'The difference between the new value and the old value cannot exceed the current available balance of the investment account! Please update the order value or increase the account balance.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(NEW.createdDate);
SET v_balance_record_year = YEAR(NEW.createdDate);

SELECT
	abs.currentBalance,
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_current_balance,
	v_investment_account_ID,
	v_investor_ID	
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	so.orderID = NEW.order_ID;
 
/*Balance record value update branch*/
/*Retrieves the value of the existing external account balance record for the month and year on which the trading fee is inserted*/
SELECT
	eab.value
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance eab
WHERE
	MONTH(eab.createdDate) = v_balance_record_month
AND YEAR(eab.createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

/*Calculates the new balance record value (the difference between the new trading fee value and the old one is subtracted from the current value of the balance record*/
SET v_new_balance_record_value = v_current_balance_record_value - v_record_amount_difference;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on trading fee update';

SET v_action_description = 'Trading fee update';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'UPDATE';

/*Since the trading fee update leads to a difference between the new trading fee value and the old one the value triggering the update will be set to this difference*/
SET v_value_triggering_update = -v_record_amount_difference;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance - v_record_amount_difference;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);

END IF;
		
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Update investment account balance on trading fee deletion`
AFTER DELETE
	ON 
	trading_fees FOR EACH ROW
	
BEGIN
	
DECLARE v_has_investment_account INT(20);

DECLARE v_error_message VARCHAR(200);

DECLARE v_balance_record_month INT(20);

DECLARE v_balance_record_year INT(20);

DECLARE v_investment_account_current_balance INT(20);

DECLARE v_investment_account_ID INT(20);

DECLARE v_investor_ID INT(20);

DECLARE v_current_balance_record_value DOUBLE;

DECLARE v_new_balance_record_value DOUBLE;

DECLARE v_current_storage_balance DOUBLE;

DECLARE v_action_source VARCHAR(100);

DECLARE v_action_description VARCHAR(200);

DECLARE v_database_component_type VARCHAR(20);

DECLARE v_database_operation VARCHAR(20);

DECLARE v_value_triggering_update DOUBLE;

DECLARE v_old_storage_balance DOUBLE;

DECLARE v_new_storage_balance DOUBLE;

DECLARE v_action_timestamp TIMESTAMP;

DECLARE v_account_balance_storage_update_result TINYINT(1);


SELECT
	COUNT(*)
INTO
	v_has_investment_account
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
WHERE
	so.orderID = OLD.order_ID;

IF v_has_investment_account = 0 THEN
SET v_error_message = 'No investment account was found for the specified broker! Please create an account before deleting trading fees.';

SIGNAL SQLSTATE '45000'
SET MESSAGE_TEXT = v_error_message;
END IF;

/*Retrieves the month and year which will be used to determine if an external account balance record exists and if so, the actual record that needs to be updated*/
SET v_balance_record_month = MONTH(OLD.createdDate);
SET v_balance_record_year = YEAR(OLD.createdDate);

SELECT
	abs.currentBalance,
	acc.accountID,
	acc.user_ID
INTO
	v_investment_account_current_balance,
	v_investment_account_ID,
	v_investor_ID	
FROM
	stock_orders so
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
INNER JOIN broker_accounts ba ON
	ps.broker_ID = ba.broker_ID
INNER JOIN accounts acc ON
	ba.account_ID = acc.accountID
INNER JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	so.orderID = OLD.order_ID;
 
/*Retrieves the value of the existing external account balance record for the month and year on which the trading fee is inserted*/
SELECT
	eab.value
INTO
	v_current_balance_record_value
FROM
	external_accounts_balance eab
WHERE
	MONTH(eab.createdDate) = v_balance_record_month
AND YEAR(eab.createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

/*Calculates the new balance record value (the difference between the new trading fee value and the old one is subtracted from the current value of the balance record*/
SET v_new_balance_record_value = v_current_balance_record_value + OLD.value;

/*Updates the existing balance record value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_balance_record_value,
	lastUpdatedDate = CURDATE()
WHERE
	MONTH(createdDate) = v_balance_record_month
AND YEAR(createdDate) = v_balance_record_year
AND account_ID = v_investment_account_ID;

/*Account balance storage implementation-START*/
SELECT
	currentBalance
INTO
	v_current_storage_balance
FROM
	account_balance_storage
WHERE
	account_ID = v_investment_account_ID;

SET v_action_source = 'Account balance update on trading fee deletion';

SET v_action_description = 'Trading fee deletion';

SET v_database_component_type = 'TRIGGER';

SET v_database_operation = 'DELETE';

/*Since the trading fee is deleted the value triggering the update will be set to the trading fee value*/
SET v_value_triggering_update = OLD.value;

SET v_old_storage_balance = v_current_storage_balance;

SET v_new_storage_balance = v_current_storage_balance + OLD.value;

SET v_action_timestamp = CURRENT_TIMESTAMP();

/*Updates the current balance of the account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/
CALL update_account_balance_storage(v_investment_account_ID,
v_action_source,
v_action_description,
v_database_component_type,
v_database_operation,
v_value_triggering_update,
v_old_storage_balance,
v_new_storage_balance,
v_action_timestamp,
v_account_balance_storage_update_result);
		
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `userID` int(10) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `salt` binary(32) NOT NULL,
  `password` varchar(100) NOT NULL,
  `email` varchar(30) NOT NULL,
  PRIMARY KEY (`userID`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `username_2` (`username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users_creditors`
--

DROP TABLE IF EXISTS `users_creditors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users_creditors` (
  `user_ID` int(10) NOT NULL,
  `creditor_ID` int(10) NOT NULL,
  KEY `user_ID` (`user_ID`),
  KEY `creditor_ID` (`creditor_ID`),
  CONSTRAINT `users_creditors_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  CONSTRAINT `users_creditors_ibfk_2` FOREIGN KEY (`creditor_ID`) REFERENCES `creditors` (`creditorID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users_creditors`
--

LOCK TABLES `users_creditors` WRITE;
/*!40000 ALTER TABLE `users_creditors` DISABLE KEYS */;
/*!40000 ALTER TABLE `users_creditors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users_debtors`
--

DROP TABLE IF EXISTS `users_debtors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users_debtors` (
  `user_ID` int(10) NOT NULL,
  `debtor_ID` int(10) NOT NULL,
  KEY `userID` (`user_ID`),
  KEY `debtorID` (`debtor_ID`),
  CONSTRAINT `users_debtors_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `users_debtors_ibfk_2` FOREIGN KEY (`debtor_ID`) REFERENCES `debtors` (`debtorID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users_debtors`
--

LOCK TABLES `users_debtors` WRITE;
/*!40000 ALTER TABLE `users_debtors` DISABLE KEYS */;
/*!40000 ALTER TABLE `users_debtors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'empty_db'
--
/*!50003 DROP FUNCTION IF EXISTS `get_account_type` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `get_account_type`(`p_account_ID` INT) RETURNS varchar(20) CHARSET utf8mb4
    NO SQL
BEGIN

/* 
Description:	Function that retrieves the type of the specified saving account
 			 	
Input parameter:	p_account_ID -> the id of the account whose type needs to be retrieved				

Returns:	the type of the specified account(SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT/USER_DEFINED-CUSTOM_SAVING_ACCOUNT)
*/
	
DECLARE v_account_type VARCHAR(20) DEFAULT NULL;

SELECT
	typeName
INTO
	v_account_type
FROM
	accounts acc
INNER JOIN account_types at ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = p_account_ID;

RETURN v_account_type;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `get_action_performed_on_receivable` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `get_action_performed_on_receivable`(p_receivable_ID INT) RETURNS varchar(50) CHARSET utf8mb4
BEGIN
/* 
Description: Function that retrieves the type of action performed on a specified receivable
 			 	
Input parameter: p_receivable_ID -> the id of the receivable on which the action was performed			

Returns: a string that describes the action performed on the receivable	
*/

	

DECLARE v_action varchar(50);



DECLARE v_hist_records_count INT(20);



DECLARE v_total_partial_payments INT(20);



DECLARE v_current_receivable_name VARCHAR(50);



DECLARE v_current_receivable_value INT(20);



DECLARE v_current_total_paid_amount INT(20);



DECLARE v_current_status VARCHAR(50);



DECLARE v_current_debtor_ID INT(20);



DECLARE v_current_created_date DATE;



DECLARE v_current_due_date DATE;



DECLARE v_pay_off_date date;



DECLARE v_hist_receivable_name VARCHAR(50);



DECLARE v_hist_receivable_value INT(20);



DECLARE v_hist_total_paid_amount INT(20);



DECLARE v_hist_debtor_ID INT(20);



DECLARE v_hist_created_date DATE;



DECLARE v_hist_due_date DATE;

/*Sets the default action*/
SET v_action = 'Undefined action';



/*Checks if the receivable has history records*/

SELECT

	COUNT(*)

INTO

	v_hist_records_count

FROM

	receivable_history

WHERE

	receivable_ID = p_receivable_ID;



/*Current receivable data retrieval*/

SELECT

	rc.name,

	rc.value,

	rc.totalPaidAmount,

	rcs.statusDescription,

	rc.debtor_ID,

	rc.createdDate,

	rc.dueDate,

	rc.payOffDate

INTO

	v_current_receivable_name,

	v_current_receivable_value,

	v_current_total_paid_amount,

	v_current_status,

	v_current_debtor_ID,

	v_current_created_date,

	v_current_due_date,

	v_pay_off_date

FROM

	receivables rc

INNER JOIN receivable_status rcs ON

	rc.status_ID = rcs.statusID

WHERE

	rc.receivableID = p_receivable_ID;



/*History receivable data retrieval.It selects only the latest history record from the table*/

SELECT

	name,

	value,

	totalPaidAmount,

	debtor_ID,

	createdDate,

	dueDate

INTO

	v_hist_receivable_name,

	v_hist_receivable_value,

	v_hist_total_paid_amount,

	v_hist_debtor_ID,

	v_hist_created_date,

	v_hist_due_date

FROM

	receivable_history

WHERE

	receivable_ID = p_receivable_ID

	AND histID = (

	SELECT

		max(histID)

	FROM

		receivable_history

	WHERE

		receivable_ID = p_receivable_ID);



/*Retrieves the total value of partial payments*/

SELECT

	COALESCE(SUM(paymentValue), 0)

INTO

	v_total_partial_payments

FROM

	partial_payments

WHERE

	receivable_ID = p_receivable_ID;



/*Checks if the receivable is newly created*/

IF v_current_total_paid_amount = 0

	AND v_hist_records_count = 0 THEN 

SET

	v_action = 'Receivable creation';

END IF;


/*Checks if a new partial payment was inserted for the receivable*/

IF (v_total_partial_payments = v_current_total_paid_amount)

	AND (v_current_total_paid_amount != v_current_receivable_value)
	AND (v_current_total_paid_amount > v_hist_total_paid_amount) THEN

SET

	v_action = 'Partial payment insertion';

END IF;


/*Checks if the receivable was marked as paid */

IF (v_total_partial_payments < v_current_total_paid_amount)

	AND (v_current_total_paid_amount = v_current_receivable_value) THEN 

SET

	v_action = 'Marked as paid';

END IF;

/*Checks if the receivable was fully paid by inserting multiple partial payments(*/
IF (v_total_partial_payments = v_current_total_paid_amount)
	AND (v_current_total_paid_amount = v_current_receivable_value) THEN 
SET
	v_action = 'Fully paid by partial payments';
END IF;


/*Checks if the receivable status was automatically set to 'Overdue' by the event specifically created for this task*/

IF v_current_due_date < CURDATE()

	AND v_current_status = 'Overdue' THEN 

SET

	v_action = 'Automatic status update to overdue';

END IF;


/*Performs comparisons with the latest history record only if the receivable has such a record*/

IF v_hist_records_count > 0 THEN 

/*Checks if any of the following receivable fields were updated: name, value, debtor ID, created date, due date*/

	IF (v_current_receivable_value != v_hist_receivable_value)

		OR (v_current_debtor_ID != v_hist_debtor_ID)

		OR (v_current_receivable_name != v_hist_receivable_name)

		OR (v_current_created_date != v_hist_created_date)

		OR (v_current_due_date != v_hist_due_date) THEN 

			SET

				v_action = 'Receivable update';
	END IF;

END IF;




RETURN v_action;


END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `get_balance_record_value` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `get_balance_record_value`(`p_account_ID` INT(20),

`p_record_date` DATE) RETURNS int(11)
    NO SQL
BEGIN
/* 
Description:	Function that retrieves the balance record value of the specified account for the provided date
 			 	
Input parameter:	p_account_ID -> the id of the account whose balance record value needs to be retrieved
					p_record_date -> the date of the balance record whose value needs to be retrieved			

Returns:	an integer representing the value of the balance record
*/


DECLARE v_account_type VARCHAR(50) DEFAULT NULL;



DECLARE v_balance_record_value INT(20) DEFAULT 0;



# Retrieves the account type

SELECT

	get_account_type(p_account_ID)

INTO

	v_account_type

FROM

	DUAL;



/* Retrieves the curent balance record value for the account, based on its type */

IF v_account_type LIKE '%SYSTEM_DEFINED%' THEN

SELECT

	value

INTO

	v_balance_record_value

FROM

	saving_accounts_balance

WHERE

	account_ID = p_account_ID

	AND MONTH = MONTH(p_record_date)

	AND YEAR = YEAR(p_record_date);



ELSEIF v_account_type LIKE '%USER_DEFINED%' THEN

SELECT

	value

INTO

	v_balance_record_value

FROM

	external_accounts_balance

WHERE

	account_ID = p_account_ID

	AND MONTH(createdDate) = MONTH(p_record_date)

	AND YEAR(createdDate) = YEAR(p_record_date);

END IF;



RETURN v_balance_record_value;



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `get_balance_record_value_with_double_precision` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `get_balance_record_value_with_double_precision`(`p_account_ID` INT(20),

`p_record_date` DATE) RETURNS double
    NO SQL
BEGIN
/* 
Description:	Function that retrieves the balance record value of the specified account for the provided date with double precision
 			 	
Input parameter:	p_account_ID -> the id of the account whose balance record value needs to be retrieved
-- 					p_record_date -> the date of the balance record whose value needs to be retrieved			

Returns:	a double representing the value of the balance record
*/



DECLARE v_account_type VARCHAR(50) DEFAULT NULL;



DECLARE v_balance_record_value DOUBLE DEFAULT 0;



# Retrieves the account type

SELECT

	get_account_type(p_account_ID)

INTO

	v_account_type

FROM

	DUAL;



/* Retrieves the curent balance record value for the account, based on its type */

IF v_account_type LIKE '%SYSTEM_DEFINED%' THEN

SELECT

	value

INTO

	v_balance_record_value

FROM

	saving_accounts_balance

WHERE

	account_ID = p_account_ID

	AND MONTH = MONTH(p_record_date)

	AND YEAR = YEAR(p_record_date);



ELSEIF v_account_type LIKE '%USER_DEFINED%' THEN

SELECT

	value

INTO

	v_balance_record_value

FROM

	external_accounts_balance

WHERE

	account_ID = p_account_ID

	AND MONTH(createdDate) = MONTH(p_record_date)

	AND YEAR(createdDate) = YEAR(p_record_date);

END IF;



RETURN v_balance_record_value;



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `calculate_investment_tax_amount` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `calculate_investment_tax_amount`(IN p_investor_ID INT(20), IN p_fiscal_year INT(20), OUT p_total_interest_value DOUBLE, OUT p_total_investment_earnings DOUBLE, OUT p_total_dividend_value DOUBLE, OUT p_total_sold_shares_value DOUBLE, OUT p_applied_tax_threshold VARCHAR(40), OUT p_total_tax int(20), OUT p_response_message VARCHAR(100))
BEGIN

	
DECLARE v_gross_salary_value DOUBLE;

DECLARE v_first_tax_threshold_lower_limit DOUBLE;

DECLARE v_first_tax_threshold_upper_limit DOUBLE;

DECLARE v_second_tax_threshold_lower_limit DOUBLE;

DECLARE v_second_tax_threshold_upper_limit DOUBLE;

DECLARE v_third_tax_threshold_lower_limit DOUBLE;

DECLARE v_third_tax_threshold_upper_limit DOUBLE;

DECLARE v_paid_investment_tax_value DOUBLE;


-- Calculates the total interest value for the specified fiscal year
-- SELECT
-- 	COALESCE(SUM(value), 0)
-- INTO
-- 	p_total_interest_value
-- FROM
-- 	saving_accounts_interest sai
-- INNER JOIN saving_accounts sa ON
-- 	sai.account_ID = sa.accountID
-- WHERE
-- 	sa.user_ID = p_investor_ID
-- 	AND YEAR(sai.creationDate) = p_fiscal_year;

SELECT
	sum(totalInterestValue)
INTO
	p_total_interest_value
FROM
	(
	SELECT
		acc.accountName,
		CASE
			WHEN ccy2.currencyName != 'RON' THEN 
	round(COALESCE(sum(sai.value), 0) * (
		SELECT
			value
		FROM
			currency_exchange_rates cer
		INNER JOIN currencies ccy1 ON
			cer.sourceCurrencyID = ccy1.currencyID
		WHERE
			ccy1.currencyName = ccy2.currencyName), 2)
			ELSE
	round(sum(sai.value), 2)
		END AS totalInterestValue
	FROM
		accounts acc
	INNER JOIN saving_accounts_interest sai ON
		acc.accountID = sai.account_ID
	INNER JOIN currencies ccy2 ON
		acc.currency_ID = ccy2.currencyID
	WHERE
		acc.user_ID = p_investor_ID
		AND YEAR(sai.creationDate) = p_fiscal_year
	GROUP BY
		acc.accountID) accountInterests;

-- Calculates the total dividend value for the specified fiscal year*/
SELECT
	COALESCE(SUM(ds.value), 0)
INTO
	p_total_dividend_value
FROM
	broker_accounts ba
INNER JOIN accounts acc ON
	acc.accountID = ba.account_ID
INNER JOIN brokers bs ON
	ba.broker_ID = bs.brokerID
INNER JOIN portfolios ps ON
	bs.brokerID = ps.broker_ID
INNER JOIN dividends ds ON
	ps.portfolioID = ds.portfolio_ID
WHERE
	acc.user_ID = p_investor_ID
	AND YEAR(ds.createdDate) = p_fiscal_year;

-- Calculates the total value of sold shares for the specified fiscal year
SELECT
	COALESCE(SUM(soi.pricePerUnit * totalQty), 0)
INTO
	p_total_sold_shares_value
FROM
	broker_accounts ba
INNER JOIN accounts acc ON
	acc.accountID = ba.account_ID
INNER JOIN brokers bs ON
	ba.broker_ID = bs.brokerID
INNER JOIN portfolios ps ON
	bs.brokerID = ps.broker_ID
INNER JOIN stock_orders so ON
	ps.portfolioID = so.portfolio_ID
INNER JOIN stock_order_items soi ON
	so.orderID = soi.order_ID
INNER JOIN stock_order_actions soa ON
	so.action_ID = soa.actionID
WHERE
	acc.user_ID = p_investor_ID
	AND soa.name = 'SELL'
	AND YEAR(so.createdDate) = p_fiscal_year;

-- Retrieves the gross salary value
SELECT
	COALESCE(paramValue1, 0)
INTO
	v_gross_salary_value
FROM
	tax_settings
WHERE
	settingName IN ('MINIMUM_GROSS_SALARY');

-- Calculates the tax thresholds
SELECT
	paramValue1 * v_gross_salary_value,
	paramValue2 * v_gross_salary_value
INTO
	v_first_tax_threshold_lower_limit,
	v_first_tax_threshold_upper_limit
FROM
	tax_settings
WHERE
	settingName IN ('TAX_THRESHOLD_1');

SELECT
	paramValue1 * v_gross_salary_value,
	paramValue2 * v_gross_salary_value
INTO
	v_second_tax_threshold_lower_limit,
	v_second_tax_threshold_upper_limit
FROM
	tax_settings
WHERE
	settingName IN ('TAX_THRESHOLD_2');

SELECT
	paramValue1 * v_gross_salary_value,
	4294967295-- max integer value in MySQL MariaDB
INTO
	v_third_tax_threshold_lower_limit,
	v_third_tax_threshold_upper_limit
FROM
	tax_settings
WHERE
	settingName IN ('TAX_THRESHOLD_3');

-- SELECT
-- 	concat('TAX_THRESHOLD_1_LOW: ', v_first_tax_threshold_lower_limit);
-- 
-- SELECT
-- 	concat('TAX_THRESHOLD_1_HIGH: ', v_first_tax_threshold_upper_limit);
-- 
-- SELECT
-- 	concat('TAX_THRESHOLD_2_LOW: ', v_second_tax_threshold_lower_limit);
-- 
-- SELECT
-- 	concat('TAX_THRESHOLD_2_HIGH: ', v_second_tax_threshold_upper_limit);
-- 
-- SELECT
-- 	concat('TAX_THRESHOLD_3_LOW: ', v_third_tax_threshold_lower_limit);
-- 
-- SELECT
-- 	concat('TAX_THRESHOLD_3_HIGH: ', v_third_tax_threshold_upper_limit);

SET p_total_investment_earnings = p_total_interest_value + p_total_dividend_value + p_total_sold_shares_value;

-- Selects the appropriate tax threshold based on the earnings value
IF p_total_investment_earnings >= v_first_tax_threshold_lower_limit AND p_total_investment_earnings <= v_first_tax_threshold_upper_limit THEN
SET p_total_tax = (v_first_tax_threshold_lower_limit * 10) / 100;

SET p_applied_tax_threshold = CONCAT('TAX THRESHOLD 1- ', v_first_tax_threshold_lower_limit, ' -> ', v_first_tax_threshold_upper_limit);

SET p_response_message = CONCAT('The investment tax was calculated based on the first tax threshold');

ELSEIF p_total_investment_earnings >= v_second_tax_threshold_lower_limit AND p_total_investment_earnings <= v_second_tax_threshold_upper_limit THEN
SET p_total_tax = (v_second_tax_threshold_lower_limit * 10) / 100;

SET p_applied_tax_threshold = CONCAT('TAX THRESHOLD 2: ', v_second_tax_threshold_lower_limit, ' -> ', v_second_tax_threshold_upper_limit);

SET p_response_message = CONCAT('The investment tax was calculated based on the second tax threshold');

ELSEIF p_total_investment_earnings >= v_third_tax_threshold_lower_limit AND p_total_investment_earnings <= v_third_tax_threshold_upper_limit THEN
SET p_total_tax = (v_third_tax_threshold_lower_limit * 10) / 100;

SET p_applied_tax_threshold = CONCAT('TAX THRESHOLD 3: ','Greater than ', v_first_tax_threshold_lower_limit);

SET p_response_message = CONCAT('The investment tax was calculated based on the third tax threshold');

ELSE 
SET p_total_tax = 0;

SET p_applied_tax_threshold = 'N/A';

SET p_response_message = CONCAT('The investment earnings are lower than the minimum tax threshold. No tax will be applied.');


END IF;

-- Checks if the investor has already paid the investment tax for the specified year
SELECT
	COALESCE(value, 0)
INTO
	v_paid_investment_tax_value
FROM
	investment_taxes
WHERE
	investor_ID = p_investor_ID
	AND fiscalYear = p_fiscal_year
	AND country_ID = 186;

-- Sets the total tax to zero if the investment tax was already paid
IF p_total_tax != 0 AND v_paid_investment_tax_value = p_total_tax THEN 
SET p_total_tax = 0;

SET p_response_message = CONCAT('The investment tax for ', p_fiscal_year, ' was already paid');

END IF;

SELECT CONCAT('TOTAL INTEREST VALUE: ', p_total_interest_value);

SELECT CONCAT('TOTAL DIVIDEND VALUE: ', p_total_dividend_value);

SELECT CONCAT('TOTAL SOLD SHARES VALUE: ', p_total_sold_shares_value);

SELECT CONCAT('TOTAL INVESTMENT EARNINGS: ', p_total_investment_earnings);

SELECT CONCAT('APPLIED TAX THRESHOLD: ', p_applied_tax_threshold);

SELECT CONCAT('TOTAL TAX: ', p_total_tax);

SELECT CONCAT('DETAILS: ', p_response_message);

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `calculate_total_stock_profit_for_fiscal_year` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `calculate_total_stock_profit_for_fiscal_year`(IN p_investor_ID INT(20), IN p_fiscal_year INT(20), OUT p_total_profit_for_fiscal_year DOUBLE)
BEGIN

/* 
Description:	Procedure that calculates the total yearly profit resulted from selling stocks
 			 	
Input parameters:	p_investor_ID -> the ID of investor who sold the stocks (user_ID from the users table)
					p_fiscal_year -> the year for which the total profit is calculated
						
Output parameters:	p_total_profit_for_fiscal_year -> the total calculated profit for the specified year
*/
	
DECLARE v_current_symbol VARCHAR(50);

DECLARE v_current_average_price_for_symbol DOUBLE;

DECLARE v_expected_income_for_symbol DOUBLE;

DECLARE v_actual_income_for_symbol DOUBLE;

DECLARE v_total_profit_for_symbol DOUBLE;

DECLARE v_total_profit_for_year DOUBLE;

DECLARE v_total_dividends DOUBLE;

DECLARE v_total_health_contribution_tax DOUBLE;

DECLARE v_total_trading_fees DOUBLE;

-- This loop iterates over all the symbols traded during the specified year, calculating the profit for each of them and adding it to the total profit
tradedSymbolsLoop:FOR currentSymbol IN (
SELECT
	DISTINCT(sym.symbolName) AS symbolName
FROM
	stock_orders so
INNER JOIN stock_order_items soi ON
	so.orderID = soi.order_ID
INNER JOIN stock_order_actions soa ON
	so.action_ID = soa.actionID
INNER JOIN symbols sym ON
	soi.symbol_ID = sym.symbolID
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
WHERE
	ps.investor_ID = p_investor_ID
	AND soa.name = 'SELL'
	AND YEAR(so.createdDate) = p_fiscal_year
ORDER BY
	sym.symbolName)

DO
SET v_current_symbol = currentSymbol.symbolName;

-- Retrieves the average price for the current symbol
CALL get_average_price_for_symbol(p_investor_ID,
v_current_symbol,
p_fiscal_year,
v_current_average_price_for_symbol);

-- Calculates the expected income (based on the average price), actual income (based on the selling price) and the profit for the current symbol
SELECT
	symbolFinancialInfo.expectedIncome,
	symbolFinancialInfo.actualIncome,
	symbolFinancialInfo.actualIncome - symbolFinancialInfo.expectedIncome
INTO
	v_expected_income_for_symbol,
	v_actual_income_for_symbol,
	v_total_profit_for_symbol
FROM
	(
	SELECT
		sym.symbolName,
		SUM(soi.totalQty) * v_current_average_price_for_symbol AS expectedIncome,
		SUM(soi.pricePerUnit * soi.totalQty) AS actualIncome
	FROM
		stock_orders so
	INNER JOIN stock_order_items soi ON
		so.orderID = soi.order_ID
	INNER JOIN stock_order_actions soa ON
		so.action_ID = soa.actionID
	INNER JOIN symbols sym ON
		soi.symbol_ID = sym.symbolID
	INNER JOIN portfolios ps ON
		so.portfolio_ID = ps.portfolioID
	WHERE
		ps.investor_ID = p_investor_ID
		AND soa.name = 'SELL'
		AND sym.symbolName = v_current_symbol
		AND YEAR(so.createdDate) = p_fiscal_year
	ORDER BY
		sym.symbolName) symbolFinancialInfo;

/* Adds the current profit/loss to the total profit value
Prevents the result from being set to NULL if the symbol is NULL and the total profit cannot be calculated */
SET p_total_profit_for_fiscal_year = COALESCE(p_total_profit_for_fiscal_year, 0) + COALESCE(v_total_profit_for_symbol, 0); 

SELECT CONCAT('SYMBOL NAME: ', v_current_symbol, '; ', 'EXPECTED INCOME: ', v_expected_income_for_symbol, '; ', 'ACTUAL INCOME: ', v_actual_income_for_symbol, '; ', 'TOTAL PROFIT FOR SYMBOL: ', v_total_profit_for_symbol, ';');

END FOR tradedSymbolsLoop;

-- Retrieves the total dividends for the fiscal year
SELECT
	COALESCE(SUM(ds.value), 0)
INTO
	v_total_dividends
FROM
	dividends ds
INNER JOIN portfolios ps ON
	ds.portfolio_ID = ps.portfolioID
WHERE
	ps.investor_ID = p_investor_ID
	AND YEAR(ds.createdDate) = p_fiscal_year;

SELECT CONCAT('TOTAL DIVIDENDS: ', v_total_dividends);

-- Retrieves the total health contribution tax for the fiscal year
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_health_contribution_tax
FROM
	investment_taxes
WHERE
	investor_ID = p_investor_ID
	AND fiscalYear = p_fiscal_year;

SELECT CONCAT('TOTAL HEALTH CONTRIBUTION TAX: ', v_total_health_contribution_tax);
	
-- Retrieves the total trading fees for the fiscal year
SELECT
	COALESCE(SUM(tf.value), 0)
INTO
	v_total_trading_fees
FROM
	trading_fees tf
INNER JOIN stock_orders so ON
	tf.order_ID = so.orderID
INNER JOIN portfolios ps ON
	so.portfolio_ID = ps.portfolioID
WHERE
	ps.investor_ID = p_investor_ID
	AND YEAR(so.createdDate) = p_fiscal_year;

SELECT CONCAT('TOTAL TRADING FEES: ', v_total_trading_fees);

-- Adds the dividends for the current fiscal year to the total yearly profit and subtracts the total taxes (health contribution + trading fees) 
SET p_total_profit_for_fiscal_year = (p_total_profit_for_fiscal_year + v_total_dividends) - (v_total_health_contribution_tax + v_total_trading_fees);

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `can_delete_selected_income` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `can_delete_selected_income`(IN `p_deleted_income_value` INT(20), IN `p_user_id` INT(20), IN `p_start_date` DATE, IN `p_end_date` DATE, OUT `p_result` TINYINT(1))
    NO SQL
BEGIN
/* 
Description:	Procedure that calculates whether the selected income can be deleted or not. 
 			 	It is considered that an income can be deleted when the remaining income/s value is greater than or equal to the value of the income to be deleted 
   
Input parameters:	p_deleted_income_value -> the value of the deleted income
 					p_user_id -> the id of the user for which the income was inserted
 					p_start_date -> the start date of the interval for which the income was inserted
 					p_end_date -> the end date of the interval for which the income was inserted	

Output parameter:	p_result -> a boolean value indicating if the selected income can be deleted or not		  
*/



DECLARE v_total_incomes INT DEFAULT 0;



DECLARE v_remaining_incomes INT DEFAULT 0;



DECLARE v_total_expenses INT DEFAULT 0;



DECLARE v_total_debts INT DEFAULT 0;



DECLARE v_total_savings INT DEFAULT 0;


/*Calculates the total incomes from the specified time period */

SET

	v_total_incomes = (

SELECT

	COALESCE(SUM(value), 0)

FROM

	incomes

WHERE

	user_ID = p_user_id

	AND date BETWEEN p_start_date AND p_end_date);



SELECT

	(CONCAT('Total incomes: ', v_total_incomes));



/*Calculates the total expenses, debts and savings from the specified time period */

SET

	v_total_expenses = (

SELECT

	COALESCE(SUM(value), 0)

FROM

	expenses

WHERE

	user_ID = p_user_id

	AND date BETWEEN p_start_date AND p_end_date);



SELECT

	(CONCAT('Total expenses: ', v_total_expenses));



SET

	v_total_debts = (

SELECT

	COALESCE(SUM(value), 0)

FROM

	debts

WHERE

	user_ID = p_user_id

	AND date BETWEEN p_start_date AND p_end_date);



SELECT

	(CONCAT('Total debts: ', v_total_debts));



SET

	v_total_savings = (

SELECT

	COALESCE(SUM(value), 0)

FROM

	savings

WHERE

	user_ID = p_user_id

	AND date BETWEEN p_start_date AND p_end_date);



SELECT

	(CONCAT('Total savings: ', v_total_savings));



SET

v_remaining_incomes = v_total_incomes - (v_total_expenses + v_total_debts + v_total_savings);



SELECT

	(CONCAT('Remaining incomes: ', v_remaining_incomes));



/* Checks to see if the deleted income value is lower than the remaining amount or equal to it. Otherwise there would be items(expenses, debts, savings) that would not be covered by the available incomes */

IF v_remaining_incomes > 0 THEN

	IF v_remaining_incomes >= p_deleted_income_value THEN

    	SET

			p_result = 1;

ELSE

    	SET

			p_result = 0;

END IF;

ELSE



/*Covers the case when the deleted income value is 0 */

IF v_remaining_incomes = 0

AND p_deleted_income_value = 0 THEN

    	SET

			p_result = 1;

ELSE 

		SET

			p_result = 0;

END IF;

END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `can_perform_requested_transfer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `can_perform_requested_transfer`(IN `p_account_ID` INT(20), IN `p_transfer_value` DOUBLE, OUT `p_result` TINYINT(1),

OUT `p_account_balance` DOUBLE)
    NO SQL
BEGIN

/* 

Description:	Procedure that checks whether the requested transfer can be performed or not. 

 			 	It is considered that the transfer can be performed if the source account(the one from which the money is taken) balance is greater than or equal to the transfer amount.

Input parameters:	p_account_ID -> the id of the source account

 					p_transfer_value -> the transfer value

Output parameter:	p_result -> a boolean value indicating if the requested transfer can be performed

					p_account_balance -> an integer value representing the current balance of the source account  

*/

DECLARE v_account_type VARCHAR(50) DEFAULT NULL;

DECLARE v_user_ID INT DEFAULT 0;

DECLARE v_total_credit DOUBLE DEFAULT 0;

DECLARE v_total_debit DOUBLE DEFAULT 0;

DECLARE v_total_transfers_in DOUBLE DEFAULT 0;

DECLARE v_total_transfers_out DOUBLE DEFAULT 0;

DECLARE v_total_interest_amount DOUBLE DEFAULT 0;

DECLARE v_total_savings INT DEFAULT 0;

DECLARE v_total_saving_account_expenses INT DEFAULT 0;

DECLARE v_account_balance DOUBLE DEFAULT 0;

SELECT
	at.typeName,
	acc.user_ID
INTO
	v_account_type,
	v_user_ID
FROM
	accounts acc
INNER JOIN account_types AT ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = p_account_ID;


SELECT
	(CONCAT('ACCOUNT TYPE: ', v_account_type));

SELECT
	(CONCAT('USER ID: ', v_user_ID));


/*Total IN transfers*/
SELECT
	COALESCE(SUM(receivedValue), 0)
INTO
	v_total_transfers_in
FROM
	saving_accounts_transfers
WHERE
	receivingAccountID = p_account_ID;

SELECT
	(CONCAT('TOTAL IN TRANSFERS: ', v_total_transfers_in));


/*Total OUT transfers*/
SELECT
	COALESCE(SUM(sentValue), 0)
INTO
	v_total_transfers_out
FROM
	saving_accounts_transfers
WHERE
	senderAccountID = p_account_ID;


SELECT
	(CONCAT('TOTAL OUT TRANSFERS: ', v_total_transfers_out));


/*Total interest amount*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_interest_amount
FROM
	saving_accounts_interest
WHERE
	account_ID = p_account_ID;


SELECT
	(CONCAT('TOTAL INTEREST AMOUNT: ', v_total_interest_amount));


/*Total saving_account expenses*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_saving_account_expenses
FROM
	saving_accounts_expenses
WHERE
	account_ID = p_account_ID;

SELECT
	(CONCAT('TOTAL SAVING ACCOUNT EXPENSES: ', v_total_saving_account_expenses));


/*Balance calculation is different based on the account type.That is because the default saving accounts balance is increased by the user savings while for the user defined accounts they are not taken into calculation. Apart from that the logic is similar.*/
IF v_account_type = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT' THEN

/*Total savings*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_savings
FROM
	savings
WHERE
	user_ID = v_user_ID;


SELECT
	(CONCAT('TOTAL SAVINGS: ', v_total_savings));

SET
v_total_credit = v_total_transfers_in + v_total_interest_amount + v_total_savings;

SET
v_total_debit = v_total_transfers_out + v_total_saving_account_expenses;

SET
v_account_balance = v_total_credit - v_total_debit;


SELECT
	(CONCAT('ACCOUNT BALANCE: ', v_account_balance));

ELSEIF v_account_type = 'USER_DEFINED-CUSTOM_SAVING_ACCOUNT' THEN

SET
v_total_credit = v_total_transfers_in + v_total_interest_amount;

SET
v_total_debit = v_total_transfers_out + v_total_saving_account_expenses;

SET
v_account_balance = v_total_credit - v_total_debit;

SELECT
	(CONCAT('ACCOUNT BALANCE: ', v_account_balance));
END IF;

IF v_account_balance >= p_transfer_value THEN

SET
p_result = 1;

SET
p_account_balance = v_account_balance;
ELSE

SET
p_result = 0;

SET
p_account_balance = v_account_balance;
END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `can_update_element_to specified_value` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `can_update_element_to specified_value`(IN `p_updated_item_type` VARCHAR(20), IN `p_new_value` INT(20), IN `p_user_id` INT(20), IN `p_start_date` DATE, IN `p_end_date` DATE, OUT `p_result` TINYINT(1))
    NO SQL
BEGIN
/* 
Description:	Procedure that checks whether the specified budget element can be updated to the requested value or not. 
 			 	It is considered that the budget element can be updated if the difference between the new value and the old value if equal to or lower than the remaining incomes for the specified time interval
   
Input parameters:	p_updated_item_type -> the type of the budget item which is about to be updated
 					p_new_value -> the new value of the budget item
 					p_user_id -> the id of the user for which the budget item was inserted
 					p_start_date -> the start date of the interval for which the budget item was inserted
 					p_end_date -> the end date of the interval for which the budget item was inserted

Output parameters:	p_result -> a boolean value indicating if the budget item can be updated to the specified value
*/



DECLARE v_old_value INT DEFAULT 0;



DECLARE v_total_incomes INT DEFAULT 0;



DECLARE v_total_expenses INT DEFAULT 0;



DECLARE v_total_debts INT DEFAULT 0;



DECLARE v_total_savings INT DEFAULT 0;



DECLARE v_difference INT DEFAULT 0;



DECLARE v_remaining_amount INT DEFAULT 0;



/*Selects the old total value of the specified item(before the update)*/

IF p_updated_item_type = 'Expense' THEN

SET

	v_old_value = (

SELECT

	COALESCE(SUM(VALUE), 0)

FROM

	expenses

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date);



ELSEIF p_updated_item_type = 'Debt' THEN

SET

	v_old_value = (

SELECT

	COALESCE(SUM(VALUE), 0)

FROM

	debts

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date);



ELSEIF p_updated_item_type = 'Saving' THEN

SET

	v_old_value = (

SELECT

	COALESCE(SUM(VALUE), 0)

FROM

	savings

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date);

ELSE

SET

	v_old_value = 0;

END IF;



SELECT

	CONCAT('Item type:', p_updated_item_type);



SELECT

	CONCAT('Item total value:', v_old_value);



/*Retrieves the total incomes for the specified time interval*/

SELECT

	COALESCE(SUM(VALUE), 0)

INTO

	v_total_incomes

FROM

	incomes

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date;



SELECT

	CONCAT('Total incomes:', v_total_incomes);



/*Retrieves the total expenses for the specified time interval*/

SELECT

	COALESCE(SUM(VALUE), 0) 

INTO

	v_total_expenses

FROM

	expenses

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date;



SELECT

	CONCAT('Total expenses:', v_total_expenses);



/*Retrieves the total debts for the specified time interval*/

SELECT

	COALESCE(SUM(VALUE), 0) 

INTO

	v_total_debts

FROM

	debts

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date;



SELECT

	CONCAT('Total debts:', v_total_debts);



/*Retrieves the total savings for the specified time interval*/

SELECT

	COALESCE(SUM(VALUE), 0) 

INTO

	v_total_savings

FROM

	savings

WHERE

	user_ID = p_user_id

	AND DATE BETWEEN p_start_date AND p_end_date;



SELECT

	CONCAT('Total savings:', v_total_savings);



/*Calculates the remaining amount*/

SET

v_remaining_amount = v_total_incomes - (v_total_expenses + v_total_debts + v_total_savings);



SELECT

	CONCAT('Remaining amount:', v_remaining_amount);



/*Calculates the difference between the new value and the old value of the budget item*/

SET

v_difference = p_new_value - v_old_value;



SELECT

	CONCAT('Difference:', v_difference);



/*Checks if the budget item can be updated to the specified value*/

IF v_difference > 0 THEN

	IF v_remaining_amount >= v_difference THEN

    	SET

			p_result = 1;

ELSE

    	SET

			p_result = 0;

END IF;

ELSE

	SET

		p_result = 1;

END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `create_account_balance_record_on_transfer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `create_account_balance_record_on_transfer`(IN `p_user_ID` INT(20), IN `p_account_ID` INT(20), IN `p_record_value` DOUBLE, IN `p_record_date` DATE, OUT `p_execution_result` TINYINT(1))
    NO SQL
BEGIN
/* 
Description:	Procedure that creates a new balance record for the specified account when a transfer is performed
 			 	
Input parameters:	p_user_id -> the id of the user which performs the transfer
					p_account_ID -> the id of the account for which the balance record needs to be created
 					p_record_value -> the value of the new balance record
 					p_record_date -> the date of the new balance record

Output parameters:	p_execution_result -> a boolean value indicating if the balance record was successfully created or not
*/

DECLARE v_account_type VARCHAR(50) DEFAULT NULL;

DECLARE v_has_balance_record_specified_month TINYINT(1) DEFAULT 0;

DECLARE v_record_name VARCHAR(50) DEFAULT NULL;

SELECT
	get_account_type(p_account_ID)
INTO
	v_account_type
FROM
	DUAL;


SET
v_record_name = CONCAT('balance_record_', p_record_date);

/*Creates balance record based on the account type(SYSTEM_DEFINED- inserts into saving_accounts_balance table/USER_DEFINED-inserts into external_accounts_balance table)*/
IF v_account_type LIKE '%SYSTEM_DEFINED%' THEN
		CALL create_saving_account_balance_record(p_user_ID,
p_account_ID,
v_record_name,
p_record_value,
MONTH(p_record_date),
YEAR(p_record_date),
p_execution_result);

ELSEIF v_account_type LIKE '%USER_DEFINED%' THEN
       CALL create_external_account_balance_record(p_account_ID,
v_record_name,
p_record_value,
p_record_date,
NULL,
p_execution_result);
END IF;


END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `create_external_account_balance_record` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `create_external_account_balance_record`(IN `p_account_ID` INT, IN `p_balance_record_name` VARCHAR(50), IN `p_balance_record_value` DOUBLE, IN `p_created_date` DATE, IN `p_last_updated_date` DATE, OUT `p_execution_result` INT)
    NO SQL
BEGIN



DECLARE v_inserted_rows INT DEFAULT 0;







INSERT INTO external_accounts_balance(



account_ID,



recordName, 



value,



createdDate,



lastUpdatedDate) 



VALUES(



p_account_ID,



p_balance_record_name,



p_balance_record_value,



p_created_date,



p_last_updated_date



);  







SET v_inserted_rows = ROW_COUNT();







IF v_inserted_rows = 1 THEN



	SET p_execution_result = 1;



ELSE



	SET p_execution_result = 0;



END IF;







END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `create_saving_account_balance_record` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `create_saving_account_balance_record`(IN `p_user_ID` INT(10), IN `p_saving_account_ID` INT(10), IN `p_balance_record_name` VARCHAR(50), IN `p_balance_record_value` DOUBLE, IN `p_month` INT(2), IN `p_year` INT(4), OUT `p_execution_result` TINYINT(1))
BEGIN
/* 
Description:	Procedure that creates a new balance record for a default saving account(the one which is automatically created by the system)
 			 	
Input parameters:	p_user_ID -> the id of the user which performs the transfer
					p_saving_account_ID -> the id of the account for which the balance record needs to be created
					p_balance_record_name -> the name of the new balance record
 					p_balance_record_value -> the value of the new balance record
 					p_month -> the month for which new balance record is created
 					p_year -> the year for which new balance record is created

Output parameters:	p_execution_result -> a boolean value indicating if the balance record was successfully created or not
*/

DECLARE v_inserted_rows INT DEFAULT 0;

INSERT
	INTO
	saving_accounts_balance(user_ID,
	account_ID,
	recordName,
	value,
	MONTH,
	YEAR)
VALUES(p_user_ID,
p_saving_account_ID,
p_balance_record_name,
p_balance_record_value,
p_month,
p_year);

SET
v_inserted_rows = ROW_COUNT();

IF v_inserted_rows = 1 THEN
SET
	p_execution_result = 1;

ELSE
SET
	p_execution_result = 0;

END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_account_statistics` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_account_statistics`(
IN p_account_ID INT(20), 
OUT p_account_type VARCHAR(50),
OUT p_account_name VARCHAR(50),
OUT p_bank_name VARCHAR(50),
OUT p_account_ccy VARCHAR(50),
OUT p_account_creation_date VARCHAR(10),
OUT p_account_balance DOUBLE,
OUT p_total_in_transfers DOUBLE,
OUT p_total_out_transfers DOUBLE,
OUT p_total_interest_amount DOUBLE,
OUT p_total_saving_account_expenses DOUBLE,
OUT p_total_savings DOUBLE,
OUT p_total_unpaid_receivable_amount DOUBLE)
BEGIN

/* 

Description:	Procedure that calculates the balance of the specified account

 			 	

Input parameters:	p_account_ID -> the id of the account whose balance needs to be calculated			



Output parameters:	p_account_balance -> an integer value representing the current balance of the specified account

*/
-- DECLARE v_account_type VARCHAR(50) DEFAULT NULL;


DECLARE v_user_ID INT DEFAULT 0;

DECLARE v_total_credit DOUBLE DEFAULT 0;

DECLARE v_total_debit DOUBLE DEFAULT 0;

DECLARE v_total_transfers_in INT DEFAULT 0;

DECLARE v_total_transfers_out INT DEFAULT 0;

DECLARE v_total_interest_amount DOUBLE DEFAULT 0;

DECLARE v_total_saving_account_expenses INT DEFAULT 0;

DECLARE v_total_banking_fees DOUBLE DEFAULT 0;

DECLARE v_account_balance DOUBLE DEFAULT 0;


/*Retrieves the following data:

 -account name

 -bank name at which the account was created or the default value if no such information is present(e.g: for default saving accounts)

 -account currency

 -account creation date

 */

SELECT
	acc.accountName ,
	COALESCE(bnk.bankName, 'N/A'),
	crs.currencyName ,
	acc.creationDate
INTO
	p_account_name,
	p_bank_name,
	p_account_ccy,
	p_account_creation_date
FROM
	accounts acc
INNER JOIN banks bnk ON
	acc.bank_ID = bnk.bankID
INNER JOIN currencies crs ON
	acc.currency_ID = crs.currencyID
WHERE
	acc.accountID = p_account_ID;


/*Retrieves the account type and the ID of the user that owns the specified account*/
SELECT
at.typeName,
	acc.user_ID
INTO
	p_account_type,
	v_user_ID
FROM
	accounts acc
INNER JOIN account_types AT ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = p_account_ID;
-- SELECT

-- 	(CONCAT('ACCOUNT TYPE: ', v_account_type));

-- 

-- SELECT

-- 	(CONCAT('USER ID: ', v_user_ID));


/*Total IN transfers*/
SELECT
	COALESCE(SUM(receivedValue), 0)
INTO
	p_total_in_transfers
FROM
	saving_accounts_transfers
WHERE
	receivingAccountID = p_account_ID;

-- SELECT

-- 	(CONCAT('TOTAL IN TRANSFERS: ', v_total_transfers_in));


/*Total OUT transfers*/
SELECT
	COALESCE(SUM(sentValue), 0)
INTO
	p_total_out_transfers
FROM
	saving_accounts_transfers
WHERE
	senderAccountID = p_account_ID;
-- SELECT

-- 	(CONCAT('TOTAL OUT TRANSFERS: ', v_total_transfers_out));


/*Total interest amount*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	p_total_interest_amount
FROM
	saving_accounts_interest
WHERE
	account_ID = p_account_ID;
-- SELECT

-- 

-- 	(CONCAT('TOTAL INTEREST AMOUNT: ', v_total_interest_amount));


/*Total saving_account expenses*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	p_total_saving_account_expenses
FROM
	saving_accounts_expenses
WHERE
	account_ID = p_account_ID;

-- SELECT

-- 	(CONCAT('TOTAL SAVING ACCOUNT EXPENSES: ', v_total_saving_account_expenses));


/*Balance calculation is different based on the account type.That is because the default saving accounts balance is increased by the user savings while for the user defined accounts they are not taken into calculation. Apart from that the logic is similar.*/
IF p_account_type = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT' THEN

/*Total savings*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	p_total_savings
FROM
	savings
WHERE
	user_ID = v_user_ID;
-- SELECT (CONCAT('USER_ID: ', v_user_ID));

-- SELECT

-- 	(CONCAT('TOTAL SAVINGS: ', v_total_savings));


/*Total amount left to be paid for the existing receivables(receivable value - total paid amount)*/
SELECT
	COALESCE(SUM(value - totalPaidAmount), 0)
INTO
	p_total_unpaid_receivable_amount
FROM
	receivables
WHERE
	account_ID = p_account_ID;

-- SELECT

-- 	(CONCAT('TOTAL UNPAID RECEIVABLE AMOUNT: ', v_total_unpaid_receivable_amount));



/*Calculates the total credit amount of the account*/

/*SET

v_total_credit = p_total_in_transfers + p_total_savings;*/



/*Calculates the total debit amount of the account*/

/*SET

v_total_debit = p_total_out_transfers + p_total_saving_account_expenses + p_total_unpaid_receivable_amount;*/



/*Calculates the current balance of the account*/

/*SET

v_account_balance = v_total_credit - v_total_debit;*/


/*Account balance retrieval from the balance storage system*/
SELECT
	ROUND(currentBalance, 2)
INTO
	v_account_balance
FROM
	account_balance_storage
WHERE
	account_ID = p_account_ID;


-- SELECT

-- 	(CONCAT('ACCOUNT BALANCE FOR DEFAULT SAVING ACCOUNT: ', v_account_balance));



ELSEIF p_account_type = 'USER_DEFINED-CUSTOM_SAVING_ACCOUNT' THEN

SET
p_total_savings = 0;

SET
p_total_unpaid_receivable_amount = 0;



/*Calculates the total banking fees for the account*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_banking_fees
FROM
	external_accounts_banking_fees
WHERE
	account_ID = p_account_ID;


/*SET

	v_total_credit = p_total_in_transfers + p_total_interest_amount;*/



/*SET

	v_total_debit = p_total_out_transfers + p_total_saving_account_expenses + v_total_banking_fees;*/



/*SET

	v_account_balance = ROUND(v_total_credit - v_total_debit, 2);*/

-- SELECT

-- 	(CONCAT('ACCOUNT BALANCE FOR CUSTOM SAVING ACCOUNT: ', v_account_balance));



/*Account balance retrieval from the balance storage system*/
SELECT
	ROUND(currentBalance, 2)
INTO
	v_account_balance
FROM
	account_balance_storage
WHERE
	account_ID = p_account_ID;


END IF;


SET
p_account_balance = v_account_balance;

-- SELECT

-- 	(CONCAT('OUTPUT VARIABLE VALUE: ', p_account_balance));

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_average_price_for_symbol` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_average_price_for_symbol`(IN p_investor_ID INT(20), IN p_symbol_name VARCHAR(50), IN p_fiscal_year INT(20), OUT p_average_price DOUBLE)
BEGIN

/* 
Description:	Procedure that calculates the average price for a specified symbol
 			 	
Input parameters:	p_investor_ID -> the id of the investor who owns the stock portfolio
					p_symbol_name -> the symbol name
					p_fiscal_year -> the year for which the calculation is performed
			
Output parameters:	p_average_price -> the calculated average price of the symbol
*/
	
DECLARE p_items_to_ignore_result VARCHAR(50);

CALL get_items_to_ignore_for_stock_price_calculation(p_investor_ID,
p_symbol_name,
p_fiscal_year);

WITH items_for_average_price_calculation AS (
SELECT
	'BUY ORDER' AS itemType,
	sym.symbolName,
	soi.pricePerUnit,
	soi.totalQty,
	so.createdDate
FROM
	stock_orders so
INNER JOIN stock_order_items soi ON
		so.orderID = soi.order_ID
INNER JOIN portfolios ps ON
		so.portfolio_ID = ps.portfolioID
INNER JOIN stock_order_actions soa ON
		so.action_ID = soa.actionID
INNER JOIN symbols sym ON
		soi.symbol_ID = sym.symbolID
WHERE
		ps.investor_ID = p_investor_ID
	AND soa.name = 'BUY'
	AND sym.symbolName = p_symbol_name
	AND YEAR(so.createdDate) < p_fiscal_year
	AND so.orderID NOT IN (
	SELECT
		order_ID
	FROM
		orders_to_ignore_for_stock_price_calculation
	WHERE
		investor_ID = p_investor_ID
		AND symbolName = p_symbol_name
		)
UNION
SELECT
	'STOCK_OPTION_PLAN' AS itemType,
	sym.symbolName,
	sopi.pricePerUnit,
	sopi.totalQty,
	sop.createdDate
FROM
	stock_option_plans sop
INNER JOIN stock_option_plan_items sopi ON
	sop.planID = sopi.plan_ID
INNER JOIN symbols sym ON
	sopi.symbol_ID = sym.symbolID
WHERE
	sop.investor_ID = p_investor_ID
	AND sym.symbolName = p_symbol_name
	AND YEAR(sop.createdDate) < p_fiscal_year
	AND sop.planID NOT IN (
	SELECT
		plan_ID
	FROM
		stock_option_plans_to_ignore_for_stock_price_calculation
	WHERE
		investor_ID = p_investor_ID
		AND symbolName = p_symbol_name)
UNION
SELECT 
	'FREE STOCKS' AS itemType,
	sym.symbolName,
	0 AS pricePerUnit,
	fs.totalQty,
	fs.createdDate
FROM
	free_stocks fs
INNER JOIN symbols sym ON
	fs.symbol_ID = sym.symbolID
INNER JOIN portfolios ps ON
	fs.portfolio_ID = ps.portfolioID 
WHERE
	ps.investor_ID = p_investor_ID
AND sym.symbolName = p_symbol_name
	AND YEAR(fs.createdDate) <= p_fiscal_year)
SELECT
	ROUND(SUM(pricePerUnit * totalQty) / SUM(totalQty), 2)
INTO
	p_average_price
FROM
	items_for_average_price_calculation;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_items_to_ignore_for_stock_price_calculation` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_items_to_ignore_for_stock_price_calculation`(IN p_investor_ID INT(20), IN p_symbol_name VARCHAR(3), IN p_fiscal_year INT(20))
BEGIN
	
/* 
Description:	Procedure that retrieves all the items(BUY ORDER/STOCK OPTION PLAN/FREE STOCK) that need to be ignored when performing the average price calculation for a specified symbol
				This procedure populates the following temporay tables which will store data about the items that need to be ignored:
					1.orders_to_ignore_for_stock_price_calculation -> for the ignored BUY ORDERS
					2.stock_option_plans_to_ignore_for_stock_price_calculation -> for the ignored STOCK OPTION PLANS
					3.free_stocks_to_ignore_for_stock_price_calculation -> for the ignored FREE STOCKS
 			 	
Input parameters:	p_investor_ID -> the id of the investor who owns the stock portfolio
					p_symbol_name -> the symbol name
					p_fiscal_year -> the year for which the calculation is performed
							
Output parameters:	N/A
*/
	
DECLARE v_total_stock_option_plans INT(20);

DECLARE v_total_buy_orders INT(20);

DECLARE v_item_type VARCHAR(20);

DECLARE v_item_ID INT(20);

DECLARE v_symbol_name VARCHAR(3);

DECLARE v_total_qty INT(20);

DECLARE v_created_date DATE;

DECLARE v_stock_option_plans_to_ignore_count INT(20);

DECLARE v_remaining_shares INT(20);

DECLARE v_finished TINYINT(1);

-- Retrieves all the items(BUY ORDER/STOCK OPTION PLAN/FREE STOCK) that added stocks to the portfolio of the specified investor
DECLARE items_to_process_cursor CURSOR
FOR
SELECT itemsToProcess.itemType, 
	itemsToProcess.itemID, 
	itemsToProcess.symbolName,
	itemsToProcess.totalQty,
	itemsToProcess.createdDate
	FROM (
SELECT
		'BUY ORDER' as itemType,
        so.orderID AS itemID,
        so.name AS itemName,
		sym.symbolName AS symbolName,
		soi.pricePerUnit AS pricePerUnit,
		soi.totalQty AS totalQty,
		so.createdDate AS createdDate
	FROM
		stock_orders so
	INNER JOIN stock_order_items soi ON
		so.orderID = soi.order_ID
	INNER JOIN portfolios ps ON
		so.portfolio_ID = ps.portfolioID
	INNER JOIN stock_order_actions soa ON
		so.action_ID = soa.actionID
	INNER JOIN symbols sym ON
		soi.symbol_ID = sym.symbolID
	WHERE
		ps.investor_ID = p_investor_ID
		AND soa.name = 'BUY'
		AND sym.symbolName = p_symbol_name
	AND YEAR(so.createdDate) < p_fiscal_year
UNION 
SELECT
 'STOCK OPTION PLAN' AS itemType,
 sop.planID AS itemID,
 sop.planName AS itemName,
 sym.symbolName AS symbolName,
 sopi.pricePerUnit AS pricePerUnit,
 sopi.totalQty AS totalQty,
 sop.createdDate AS createdDate
FROM
	stock_option_plans sop
INNER JOIN stock_option_plan_items sopi ON
	sop.planID = sopi.plan_ID
INNER JOIN symbols sym ON
	sopi.symbol_ID = sym.symbolID
WHERE
	investor_ID = p_investor_ID
	AND sym.symbolName = p_symbol_name
	AND YEAR(createdDate) < p_fiscal_year
UNION
SELECT 
	'FREE STOCK' AS itemType,
	fs.stockID AS itemID,
	fs.name AS itemName,
	sym.symbolName AS symbolName,
	0 AS pricePerUnit,
	fs.totalQty AS totalQty,
	fs.createdDate AS createdDate
FROM
	free_stocks fs
INNER JOIN symbols sym ON
	fs.symbol_ID = sym.symbolID
INNER JOIN portfolios ps ON 
fs.portfolio_ID = ps.portfolioID
WHERE
	ps.investor_ID = p_investor_ID
	AND sym.symbolName = p_symbol_name
	AND YEAR(fs.createdDate) < p_fiscal_year)
itemsToProcess ORDER BY itemsToProcess.createdDate;

DECLARE CONTINUE HANDLER FOR NOT FOUND
SET v_finished = 1;

-- Checks the total number of existing stock option plans
SELECT
	COUNT(*)
INTO
	v_total_stock_option_plans
FROM
	stock_option_plans sop
INNER JOIN stock_option_plan_items sopi ON
	sop.planID = sopi.plan_ID
INNER JOIN symbols sym ON
	sopi.symbol_ID = sym.symbolID
WHERE
	investor_ID = p_investor_ID
	AND sym.symbolName = p_symbol_name
	AND YEAR(createdDate) < p_fiscal_year;

-- Checks the total number of existing 'BUY' orders
SELECT
	COUNT(*)
INTO
	v_total_buy_orders
FROM
		stock_orders so
INNER JOIN stock_order_items soi ON
		so.orderID = soi.order_ID
INNER JOIN portfolios ps ON
		so.portfolio_ID = ps.portfolioID
INNER JOIN stock_order_actions soa ON
		so.action_ID = soa.actionID
INNER JOIN symbols sym ON
		soi.symbol_ID = sym.symbolID
WHERE
	ps.investor_ID = p_investor_ID
	AND soa.name = 'BUY'
	AND sym.symbolName = p_symbol_name
	AND YEAR(so.createdDate) < p_fiscal_year;

-- Creates the table that will store the buy orders that need to be ignored during the stock price calculation
CREATE TEMPORARY TABLE IF NOT EXISTS orders_to_ignore_for_stock_price_calculation (
investor_ID INT(20) NOT NULL,
symbolName VARCHAR(3) NOT NULL,
order_ID INT(20) NOT NULL
);

-- Creates the table that will store the stock option plans that need to be ignored during the stock price calculation
CREATE TEMPORARY TABLE IF NOT EXISTS stock_option_plans_to_ignore_for_stock_price_calculation (
investor_ID INT(20) NOT NULL,
symbolName VARCHAR(3) NOT NULL,
plan_ID INT(20) NOT NULL
);

-- Creates the table that will store the free stocks that need to be ignored during the stock price calculation
CREATE TEMPORARY TABLE IF NOT EXISTS free_stocks_to_ignore_for_stock_price_calculation (
investor_ID INT(20) NOT NULL,
symbolName VARCHAR(3) NOT NULL,
stock_ID INT(20) NOT NULL
);

-- Creates the table that will store the sell orders that were created up until the specified fiscal year
CREATE TEMPORARY TABLE IF NOT EXISTS sell_orders (
investor_ID INT(20) NOT NULL,
order_ID INT(20) NOT NULL,
orderName VARCHAR(50),
totalQty INT(20),
createdDate TIMESTAMP
);

-- Populates the SELL_ORDERS temporary table
INSERT INTO sell_orders(investor_ID, order_ID, orderName, totalQty, createdDate) SELECT
		ps.investor_ID AS investor_ID,
        so.orderID AS order_ID,
		so.name AS orderName,
		soi.totalQty AS totalQty,
		so.createdDate AS createdDate
	FROM
		stock_orders so
	INNER JOIN stock_order_items soi ON
		so.orderID = soi.order_ID
	INNER JOIN portfolios ps ON
		so.portfolio_ID = ps.portfolioID
	INNER JOIN stock_order_actions soa ON
		so.action_ID = soa.actionID
	INNER JOIN symbols sym ON
		soi.symbol_ID = sym.symbolID
	WHERE
		ps.investor_ID = p_investor_ID
		AND soa.name = 'SELL'
		AND sym.symbolName = p_symbol_name
	AND YEAR(so.createdDate) < p_fiscal_year
	ORDER BY createdDate;

OPEN items_to_process_cursor;

processItems:LOOP
	FETCH items_to_process_cursor
	INTO v_item_type, v_item_ID, v_symbol_name, v_total_qty, v_created_date;

IF v_finished = 1 THEN 
LEAVE processItems;
END IF;

SET v_remaining_shares = v_total_qty;
	
	-- Iterates over each SELL order and subtracts its total value from the current item (BUY ORDER/STOCK OPTION PLAN/FREE STOCK) value
sellOrderLoop:FOR sellOrder IN (
SELECT
	order_ID,
	orderName,
	totalQty,
	createdDate
FROM
	sell_orders
WHERE
	investor_ID = p_investor_ID
ORDER BY
	createdDate)
DO
SET v_remaining_shares = v_remaining_shares - sellOrder.totalQty;
	
/*When the remaining value is 0 it means that the SELL order must be removed (will not be taken into consideration during the next iterations) 
and the current item (BUY ORDER/STOCK OPTION PLAN/FREE STOCK) must be ignored (inserted into the corresponding table)*/
IF v_remaining_shares = 0 THEN
DELETE
FROM
	sell_orders
WHERE
	order_ID = sellOrder.order_ID;

-- Calls the procedure that manages the insertion of items that need to be ignored into the correct temporary tables
CALL manage_items_to_ignore(v_item_type, p_investor_ID, v_symbol_name, v_item_ID);

-- SELECT concat('Inside 0 branch. Remaining shares: ', v_remaining_shares) FROM DUAL;

LEAVE sellOrderLoop;

/*When the remaining value is less than 0 it means that the SELL order total quantity was greater than the current item's total quantity so the difference must be subtracted
 from the current SELL order value (hence the SELL order is updated)
 As a result, the SELL order will be taken into account for the next iterations*/
ELSEIF v_remaining_shares < 0 THEN
UPDATE
	sell_orders
SET
	totalQty = totalQty - ABS(v_remaining_shares)
WHERE
	order_ID = sellOrder.order_ID;

-- Calls the procedure that manages the insertion of items that need to be ignored into the correct temporary tables
CALL manage_items_to_ignore(v_item_type, p_investor_ID, v_symbol_name, v_item_ID);

-- SELECT concat('Inside < 0 branch. Remaining shares: ', v_remaining_shares) FROM DUAL;

LEAVE sellOrderLoop;

/*When the remaining value is greater than zero it means that the item's total quantity was greater than 
the SELL order total quantity so the current SELL order must be removed (it will not be taken into consideration during the next iterations)*/
ELSE
DELETE
FROM
	sell_orders
WHERE
	order_ID = sellOrder.order_ID;
END IF;

-- SELECT concat(sellOrder.orderID, '-', sellOrder.orderName) FROM DUAL;

-- SELECT concat('Inside > 0 branch. Remaining shares: ', v_remaining_shares) FROM DUAL;

END FOR sellOrderLoop;

END LOOP processItems;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_new_receivable_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_new_receivable_status`(IN `p_receivable_ID` INT(20),  IN `p_has_extended_due_date` BOOLEAN, IN `p_new_due_date` DATE, OUT `p_new_receivable_status` VARCHAR(50))
    NO SQL
proc_label:BEGIN

/* 

Description:	Procedure that sets the correct status of the specified receivable

 			 	

Input parameter:	p_receivable_ID -> the id of the receivable whose status needs to be updated



Output parameter:	p_new_receivable_status -> a string describing the new receivable status

*/



DECLARE v_receivable_value INT(20);



DECLARE v_partial_payments_value INT(20);



DECLARE v_amount_left INT(20);



DECLARE v_receivable_due_date DATE;



DECLARE v_new_status VARCHAR(50);



DECLARE v_old_status VARCHAR(50);



/*Retrieves the due date of the receivable*/

SELECT

	dueDate

INTO

	v_receivable_due_date

FROM

	receivables

WHERE

	receivableID = p_receivable_ID;

IF p_has_extended_due_date = TRUE AND p_new_due_date >= v_receivable_due_date THEN
	SET
		v_receivable_due_date = p_new_due_date;
END IF;



/*Sets the status to overdue if the receivable is expired and exits the procedure.The 'Overdue' status has the highest priority so no other checks are performed.*/

IF(v_receivable_due_date < CURDATE()) THEN

	SET

p_new_receivable_status = 'Overdue';



LEAVE proc_label;

END IF;



/*Retrieves the receivable value, the value of its partial payments (if they exist, otherwise 0 will be set using the COALESCE function) and the current receivable status*/

SELECT

	rcs.value,

	COALESCE(SUM(pps.paymentValue), 0),

	rs.statusDescription 

INTO

	v_receivable_value,

	v_partial_payments_value,

	v_old_status

FROM

	receivables rcs

INNER JOIN partial_payments pps ON

	rcs.receivableID = pps.receivable_ID

INNER JOIN receivable_status rs ON

	rcs.status_ID = rs.statusID

WHERE

	rcs.receivableID = p_receivable_ID;



SET

v_amount_left = v_receivable_value - v_partial_payments_value;



IF(v_amount_left = v_receivable_value) THEN

	/*No partial payments were performed hence the status will be set to 'New'*/

    SET

		p_new_receivable_status = 'New';



ELSEIF(v_amount_left > 0

	AND v_amount_left < v_receivable_value) THEN

	/*Partial payments were performed hence the status will be set accordingly*/

    SET

		p_new_receivable_status = 'Partially paid';



ELSEIF(v_amount_left = 0) THEN

	/*The amount left is 0 which means that the receivable was fully paid hence the status will be set to 'Paid'*/

    SET

		p_new_receivable_status = 'Paid';

END IF;



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_saving_account_balance` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_saving_account_balance`(IN `p_account_ID` INT(20), OUT `p_account_balance` DOUBLE)
    NO SQL
BEGIN

/* 
Description:	Procedure that calculates the balance of the specified account
 			 	
Input parameters:	p_account_ID -> the id of the account whose balance needs to be calculated			

Output parameters:	p_account_balance -> an integer value representing the current balance of the specified account
*/

DECLARE v_account_type VARCHAR(50) DEFAULT NULL;

DECLARE v_user_ID INT DEFAULT 0;

DECLARE v_total_credit DOUBLE DEFAULT 0;

DECLARE v_total_debit DOUBLE DEFAULT 0;

DECLARE v_total_transfers_in INT DEFAULT 0;

DECLARE v_total_transfers_out INT DEFAULT 0;

DECLARE v_total_interest_amount DOUBLE DEFAULT 0;

DECLARE v_total_savings INT DEFAULT 0;

DECLARE v_total_saving_account_expenses INT DEFAULT 0;

DECLARE v_account_balance DOUBLE DEFAULT 0;

DECLARE v_total_unpaid_receivable_amount INT(20) DEFAULT 0;

DECLARE v_total_banking_fees DOUBLE DEFAULT 0;


/*Retrieves the account type and the ID of the user that owns the specified account*/
SELECT
	at.typeName,
	acc.user_ID
INTO
	v_account_type,
	v_user_ID
FROM
	accounts acc
INNER JOIN account_types AT ON
	acc.type_ID = at.typeID
WHERE
	acc.accountID = p_account_ID;

SELECT
	(CONCAT('ACCOUNT TYPE: ', v_account_type)) AS 'ACCOUNT TYPE';

SELECT
	(CONCAT('USER ID: ', v_user_ID)) AS 'USER ID';


/*Total IN transfers*/
SELECT
	COALESCE(SUM(receivedValue), 0)
INTO
	v_total_transfers_in
FROM
	saving_accounts_transfers
WHERE
	receivingAccountID = p_account_ID;

SELECT
	(CONCAT('TOTAL IN TRANSFERS: ', v_total_transfers_in)) AS 'TOTAL IN TRANSFERS';


/*Total OUT transfers*/
SELECT
	COALESCE(SUM(sentValue), 0)
INTO
	v_total_transfers_out
FROM
	saving_accounts_transfers
WHERE
	senderAccountID = p_account_ID;

SELECT
	(CONCAT('TOTAL OUT TRANSFERS: ', v_total_transfers_out)) AS 'TOTAL OUT TRANSFERS';


/*Total interest amount*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_interest_amount
FROM
	saving_accounts_interest
WHERE
	account_ID = p_account_ID;

SELECT
	(CONCAT('TOTAL INTEREST AMOUNT: ', v_total_interest_amount)) AS 'TOTAL INTEREST AMOUNT';

/*Total saving_account expenses*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_saving_account_expenses
FROM
	saving_accounts_expenses
WHERE
	account_ID = p_account_ID;

SELECT
	(CONCAT('TOTAL SAVING ACCOUNT EXPENSES: ', v_total_saving_account_expenses)) AS 'TOTAL SAVING ACCOUNT EXPENSES';

/*Balance calculation is different based on the account type.That is because the default saving accounts balance is increased by the user savings while for the user defined accounts they are not taken into calculation. Apart from that the logic is similar.*/
IF v_account_type = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT' THEN

/*Total savings*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_savings
FROM
	savings
WHERE
	user_ID = v_user_ID;

SELECT
	(CONCAT('TOTAL SAVINGS: ', v_total_savings)) AS 'TOTAL SAVINGS';

/*Total amount left to be paid for the existing receivables(receivable value - total paid amount)*/
SELECT
	COALESCE(SUM(value - totalPaidAmount), 0)
INTO
	v_total_unpaid_receivable_amount
FROM
	receivables
WHERE
	account_ID = p_account_id;

SELECT
	(CONCAT('TOTAL UNPAID RECEIVABLE AMOUNT: ', v_total_unpaid_receivable_amount)) AS 'TOTAL UNPAID RECEIVABLE AMOUNT';


/*Calculates the total credit amount of the account*/
SET
v_total_credit = v_total_transfers_in + v_total_savings;

/*Calculates the total debit amount of the account*/
SET
v_total_debit = v_total_transfers_out + v_total_saving_account_expenses + v_total_unpaid_receivable_amount;

/*Calculates the current balance of the account*/
SET
v_account_balance = v_total_credit - v_total_debit;

SELECT
	(CONCAT('ACCOUNT BALANCE FOR DEFAULT SAVING ACCOUNT: ', v_account_balance)) AS 'ACCOUNT BALANCE FOR DEFAULT SAVING ACCOUNT';


ELSEIF v_account_type = 'USER_DEFINED-CUSTOM_SAVING_ACCOUNT' THEN
/*Total banking fees*/
SELECT
	COALESCE(SUM(value), 0)
INTO
	v_total_banking_fees
FROM
	external_accounts_banking_fees
WHERE
	account_ID = p_account_ID;

SELECT
	CONCAT('TOTAL BANKING_FEES: ' || v_total_banking_fees) AS 'TOTAL BANKING FEES';

SET
v_total_credit = v_total_transfers_in + v_total_interest_amount;

SET
v_total_debit = v_total_transfers_out + v_total_saving_account_expenses + v_total_banking_fees;

SET
v_account_balance = v_total_credit - v_total_debit;

SELECT
	(CONCAT('ACCOUNT BALANCE FOR CUSTOM SAVING ACCOUNT: ', v_account_balance)) AS 'ACCOUNT BALANCE FOR CUSTOM SAVING ACCOUNT';
END IF;

SET
p_account_balance = v_account_balance;

SELECT
	(CONCAT('OUTPUT VARIABLE VALUE: ', p_account_balance)) AS 'OUTPUT VARIABLE VALUE';

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_stock_portfolio_content` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_stock_portfolio_content`(IN p_portfolio_ID INT(20))
BEGIN

/* 
Description:	Procedure that displays the content of the specified stock portfolio
 			 	
Input parameters:	p_portfolio_ID -> the id of the portfolio whose contents need to be displayed			

Output parameters:	N/A
*/

/* Aggregates all the item types that make up the portfolio content(STOCK ORDERS, STOCK OPTION PLANS, FREE STOCKS)
and displays only the ones with a value greater than 0 */
WITH stocks_statistics AS (
SELECT
	'STOCK ORDER' AS itemType,
	CASE
		soa.name
		WHEN 'BUY' THEN 'IN'
		WHEN 'SELL' THEN 'OUT'
	END AS transactionType,
		sym.symbolName,
		soi.totalQty AS totalQty
FROM
		stock_orders so
INNER JOIN stock_order_items soi ON
		so.orderID = soi.order_ID
INNER JOIN symbols sym ON
		soi.symbol_ID = sym.symbolID
INNER JOIN stock_order_actions soa ON
		so.action_ID = soa.actionID
INNER JOIN portfolios ps ON
		so.portfolio_ID = ps.portfolioID
WHERE
		ps.portfolioID = p_portfolio_ID
	AND YEAR(so.createdDate) <= YEAR(CURDATE())
GROUP BY
		sym.symbolName,
		transactionType
UNION
SELECT
		'STOCK OPTION PLAN' AS itemType,
		'IN' AS transactionType,
		sym.symbolName,
		sopi.totalQty
FROM
	stock_option_plans sop
INNER JOIN stock_option_plan_items sopi ON
		sop.planID = sopi.plan_ID
INNER JOIN symbols sym ON
		sopi.symbol_ID = sym.symbolID
INNER JOIN portfolios ps ON
		sop.portfolio_ID = ps.portfolioID
WHERE
		ps.portfolioID = p_portfolio_ID
	AND YEAR(sop.createdDate) <= YEAR(CURDATE())
UNION
SELECT 
	'FREE STOCKS' AS itemType,
	'IN' AS transactionType,
	sym.symbolName,
	fs.totalQty
FROM
	free_stocks fs
INNER JOIN symbols sym ON
	fs.symbol_ID = sym.symbolID
WHERE
	fs.portfolio_ID = p_portfolio_ID
	AND YEAR(fs.createdDate) <= YEAR(CURDATE())
),
aggregated_statistics AS (
SELECT
	stocks_statistics.symbolName,
	stocks_statistics.transactionType,
	SUM(stocks_statistics.totalQty) AS totalQty
FROM
	stocks_statistics
GROUP BY
	stocks_statistics.symbolName,
	stocks_statistics.transactionType
ORDER BY
	stocks_statistics.transactionType),
	
final_portfolio_statistics AS (
SELECT
	aggregated_statistics.symbolName,
	SUM(CASE WHEN(aggregated_statistics.transactionType = 'IN') THEN aggregated_statistics.totalQty ELSE 0 END) AS totalAcquired,
	SUM(CASE WHEN(aggregated_statistics.transactionType = 'OUT') THEN aggregated_statistics.totalQty ELSE 0 END) AS totalSold,
	(SUM(CASE WHEN(aggregated_statistics.transactionType = 'IN') THEN aggregated_statistics.totalQty ELSE 0 END) - 
	SUM(CASE WHEN(aggregated_statistics.transactionType = 'OUT') THEN aggregated_statistics.totalQty ELSE 0 END)) AS remainingStocks
FROM
	aggregated_statistics
GROUP BY
	aggregated_statistics.symbolName)
SELECT * FROM final_portfolio_statistics WHERE remainingStocks > 0;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `has_balance_record_for_selected_month` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `has_balance_record_for_selected_month`(IN `p_account_ID` INT, IN `p_balance_record_month` INT, IN `p_balance_record_year` INT, OUT `p_exists` TINYINT)
BEGIN 

/* 
Description:	Procedure that checks if a saving account has a balance record for the specified month/year
 			 	
Input parameters:	p_account_ID -> the id of the account whose balance record needs to be checked
					p_balance_record_month -> the month for which to check the balance record existence
					p_balance_record_year -> the year for which to check the balance record existence			

Output parameter:	p_exists -> a boolean value indicating whether the balance record exists or not
*/
	

DECLARE v_count INT DEFAULT 0;



DECLARE v_account_type VARCHAR(50) DEFAULT NULL;

# Retrieves the account type(SYSTEM_DEFINED/USER_DEFINED) so that it can determine the correct table for inserting the default balance record(saving_accounts_balance/external_accounts_balance)

SELECT

	get_account_type(p_account_id) 

INTO

	v_account_type

FROM

	DUAL;

# SELECT(CONCAT('ACCOUNT TYPE: ', v_account_type)); 



IF v_account_type LIKE '%SYSTEM_DEFINED%' THEN

	SELECT

	COUNT(*) 

	INTO

	v_count

FROM

	saving_accounts_balance

WHERE

	account_ID = p_account_ID

	AND MONTH = p_balance_record_month

	AND YEAR = p_balance_record_year;



ELSEIF v_account_type LIKE '%USER_DEFINED%' THEN

	SELECT

	COUNT(*) 

	INTO

	v_count

FROM

	external_accounts_balance

WHERE

	account_ID = p_account_ID

	AND MONTH(createdDate) = p_balance_record_month

	AND YEAR(createdDate)= p_balance_record_year;

ELSE

# Sets the default count variable value if the account type does not match the two types

	SET

		v_count = 0;

END IF;



IF v_count > 0 THEN 

	SET

		p_exists = 1;

ELSE 

	SET

		p_exists = 0;

END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `log_account_balance_storage_history` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `log_account_balance_storage_history`(IN p_account_ID INT(20), IN p_action_source VARCHAR(100), IN p_action_description VARCHAR(200), IN p_database_component_type VARCHAR(20), IN p_database_operation VARCHAR(20), IN p_value_triggering_update DOUBLE, IN p_old_balance_value DOUBLE, IN p_action_timestamp TIMESTAMP)
BEGIN

/*

Description:	Procedure that creates a new history record in the account_balance_storage_history table based on the provided data



Input parameters:	p_account_ID -> the id of the account whose balance storage record needs to be updated

					p_action_source -> the name of the component (trigger, procedure, etc) which initates the balance storage record update

					p_action_description -> the description of the action which leads to the balance storage record update

					p_database_component_type -> the type of the database component (trigger, procedure, etc) from which the update action is initiated

					p_database_operation -> the DML operation that was performed on the data (INSERT, UPDATE, DELETE)

					p_value_triggering_update -> the amount which needs to be reflected into the current balance of the account (it can be positive/negative)

					p_old_balance_value -> the current balance value before the storage record update

					p_action_timestamp -> the timestamp of the update operation that was performed on the balance storage record



Output parameter: none 

*/

	

DECLARE v_storage_ID INT(20);



DECLARE v_new_balance_value DOUBLE;





SELECT

	storageID,

	currentBalance

INTO

	v_storage_ID,

	v_new_balance_value

FROM

	account_balance_storage

WHERE

	account_ID = p_account_ID;





INSERT

	INTO

	account_balance_storage_history(

	storage_ID,

	actionSource,

	actionDescription,

	databaseComponentType,

	databaseOperation,

	valueTriggeringUpdate,

	oldBalanceValue,

	newBalanceValue,

	actionTimestamp

)

VALUES (

	v_storage_ID,

	p_action_source,

	p_action_description,

	p_database_component_type,

	p_database_operation,

	p_value_triggering_update,

	p_old_balance_value,

	v_new_balance_value,

	p_action_timestamp

);



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `log_receivable_history` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `log_receivable_history`(IN p_receivable_ID INT(20), OUT p_execution_result TINYINT(1))
BEGIN
/* 
Description: Procedure that logs the changes performed on a receivable during its lifetime
 			 	
Input parameter: p_receivable_ID -> the id of the receivable for which the log will be inserted into the history table		

Output parameter: p_execution_result -> an boolean value(0 or 1) which shows if the logging process was successfull or not
*/

DECLARE v_receivable_ID INT(20);



DECLARE v_receivable_name VARCHAR(50);



DECLARE v_receivable_value INT(20);



DECLARE v_total_paid_amount INT(20);



DECLARE v_status VARCHAR(50);



DECLARE v_debtor_ID INT(20);



DECLARE v_account_ID INT(20);



DECLARE v_created_date DATE;



DECLARE v_due_date DATE;



DECLARE v_performed_action VARCHAR(50);



/*Retrieves the receivable data that will be inserted into the history table*/

SELECT

	rc.receivableID,

	rc.name,

	rc.value,

	rc.totalPaidAmount,

	rcs.statusDescription,

	rc.debtor_ID,

	rc.account_ID,

	rc.createdDate,

	rc.dueDate

	INTO

	v_receivable_ID,

	v_receivable_name,

	v_receivable_value,

	v_total_paid_amount,

	v_status,

	v_debtor_ID,

	v_account_ID,

	v_created_date,

	v_due_date

FROM

	receivables rc

INNER JOIN receivable_status rcs ON

	rc.status_ID = rcs.statusID

WHERE

	rc.receivableID = p_receivable_ID;



/*Retrives the action performed on the receivable*/

SELECT

	get_action_performed_on_receivable(p_receivable_ID)

INTO

	v_performed_action;



/*Performs the actual data insertion into the history table*/

INSERT

	INTO

	receivable_history(
	receivable_ID,

	name,

	value,

	totalPaidAmount,

	status,

	debtor_ID,

	account_ID,

	createdDate,

	dueDate,

	performedAction,
	histTimestamp)

VALUES(v_receivable_ID,

v_receivable_name,

v_receivable_value,

v_total_paid_amount,

v_status,

v_debtor_ID,

v_account_ID,

v_created_date,

v_due_date,

v_performed_action,

CURRENT_TIMESTAMP());



/*Checks if the logging process was successfull or not by looking at the number of inserted rows*/

IF ROW_COUNT() > 0 THEN
SET

	p_execution_result = 1;

ELSE
SET

	p_execution_result = 0;

END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `manage_items_to_ignore` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `manage_items_to_ignore`(IN p_item_type VARCHAR(20), IN p_investor_ID INT(20), IN p_symbol_name VARCHAR(3), IN p_item_ID INT(20))
BEGIN
	
/* 
Description:	Procedure that manages the insertion of items(BUY ORDER/STOCK OPTION PLAN/FREE STOCK) that need to be ignored into the correct temporary table
		
 			 	
Input parameters:	p_item_type -> the type of item that needs to be inserted (BUY ORDER/STOCK OPTION PLAN/FREE STOCK)
					p_symbol_name -> the symbol name
					p_item_ID -> the ID of the item
							
Output parameters:	N/A
*/	
	
DECLARE v_error_message VARCHAR(150);

IF p_item_type = 'BUY ORDER' THEN
	INSERT INTO orders_to_ignore_for_stock_price_calculation(investor_ID, symbolName, order_ID) VALUES(p_investor_ID, p_symbol_name, p_item_ID);
ELSEIF p_item_type = 'STOCK OPTION PLAN' THEN
	INSERT INTO stock_option_plans_to_ignore_for_stock_price_calculation(investor_ID, symbolName, plan_ID) VALUES(p_investor_ID, p_symbol_name, p_item_ID); 
ELSEIF p_item_type = 'FREE STOCK' THEN
	INSERT INTO free_stocks_to_ignore_for_stock_price_calculation(investor_ID, symbolName, stock_ID) VALUES(p_investor_ID, p_symbol_name, p_item_ID); 
ELSE
	SET v_error_message = CONCAT('Invalid item type \'', p_item_type, '\'. Expected one of the following: \'BUY ORDER\', \'STOCK OPTION PLAN\', \'FREE STOCK\'');
	
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = v_error_message;
END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `mark_overdue_receivables` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `mark_overdue_receivables`()
    NO SQL
BEGIN
/* 
Description:	Procedure that marks the receivables whose dues date if before the current date as 'Overdue'
   
Input parameters: none

Output parameter: none  
*/



DECLARE v_receivable_id INT(20);



DECLARE v_finished TINYINT(1);



/*Declares a variable of type cursor which holds all the id's of the receivables which are overdue*/

DECLARE receivable_cursor 



	CURSOR FOR 



SELECT

	receivableID

FROM

	receivables

WHERE

	dueDate < CURDATE()

ORDER BY

	dueDate;



/*Declares a flag which is set to true in case there are no receivables meeting the previously mentioned criterion.
  This is then used to prevent the execution of update statements in such cases.*/

DECLARE CONTINUE HANDLER FOR NOT FOUND SET

v_finished = 1;



OPEN receivable_cursor;



setOverdueReceivable:LOOP



	FETCH receivable_cursor

INTO

	v_receivable_id;



IF v_finished = 1 THEN



    	LEAVE setOverdueReceivable;

END IF;


/*Updates only the receivables for which the due date is before the current date and whose current status is not 'Paid'.
That's because we need to avoid marking a receivable as 'Overdue' once it has correctly been marked as 'Paid'*/

UPDATE

	receivables rc

INNER JOIN receivable_status rcs ON

	rc.status_ID = rcs.statusID

SET

		rc.status_ID = (

	SELECT

		statusID

	FROM

		receivable_status

	WHERE

		statusDescription = 'Overdue')

WHERE

	rc.receivableID = v_receivable_id

	AND rcs.statusDescription != 'Paid';


END LOOP setOverdueReceivable;



CLOSE receivable_cursor;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `populate_account_balance_storage_with_existing_accounts` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `populate_account_balance_storage_with_existing_accounts`(OUT p_total_inserted_accounts INT(20))
BEGIN

DECLARE v_current_account_ID INT(20);

DECLARE v_current_account_name VARCHAR(50);

DECLARE v_current_account_balance DOUBLE;

DECLARE v_total_inserted_accounts INT(20) DEFAULT 0;

DECLARE v_finished TINYINT(1);

DECLARE account_cursor CURSOR FOR
SELECT
	acc.accountID,
	acc.accountName
FROM
	accounts acc
LEFT JOIN account_balance_storage abs ON
	acc.accountID = abs.account_ID
WHERE
	abs.account_ID IS NULL;

DECLARE CONTINUE HANDLER FOR NOT FOUND SET
v_finished = 1;

OPEN account_cursor;

insertAccounts:LOOP



	FETCH account_cursor
INTO
	v_current_account_ID,
	v_current_account_name;

IF v_finished = 1 THEN

	LEAVE insertAccounts;
END IF;

CALL get_saving_account_balance(v_current_account_ID,
v_current_account_balance);

IF v_current_account_balance >= 0 THEN
INSERT
	INTO
	account_balance_storage(account_ID,
	currentBalance,
	createdDate,
	lastUpdatedDate)
VALUES(v_current_account_ID,
v_current_account_balance,
CURRENT_TIMESTAMP(),
NULL);

SET
v_total_inserted_accounts = v_total_inserted_accounts + 1;
ELSE
SELECT
	CONCAT('Cannot insert the account named \'', v_current_account_name, ' \'', ' because its current balance is negative. The actual balance value is ', v_current_account_balance) AS 'ERROR MESSAGE';
END IF;
END LOOP insertAccounts;

SET
p_total_inserted_accounts = v_total_inserted_accounts;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `set_receivable_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `set_receivable_status`(IN `p_receivable_ID` INT(20))
    NO SQL
proc_label:BEGIN

/* 
Description:	Procedure that sets the correct status of the specified receivable
 			 	
Input parameter:	p_receivable_ID -> the id of the receivable whose status needs to be updated

Output parameter:	none
*/





DECLARE v_receivable_value INT(20);



DECLARE v_partial_payments_value INT(20);



DECLARE v_amount_left INT(20);



DECLARE v_receivable_due_date DATE;



DECLARE v_new_status VARCHAR(50);



DECLARE v_old_status VARCHAR(50);



/*Retrieves the due date of the receivable*/

SELECT

	dueDate

INTO

	v_receivable_due_date

FROM

	receivables

WHERE

	receivableID = p_receivable_ID;



/*Sets the status to overdue if the receivable is expired and exits the procedure.The 'Overdue' status has the highest priority so no other checks are performed.*/

IF(v_receivable_due_date < CURDATE()) THEN

	UPDATE

	receivables

SET

	status_ID = (

	SELECT

		statusID

	FROM

		receivable_status

	WHERE

		statusDescription = 'Overdue')

WHERE

	receivableID = p_receivable_ID;

-- SELECT

-- 	'RECEIVABLE STATUS SET TO OVERDUE' AS 'Performed action';



LEAVE proc_label;

END IF;



/*Retrieves the receivable value, the value of its partial payments (if they exist, otherwise 0 will be set using the COALESCE function) and the current receivable status*/

SELECT

	rcs.value,

	COALESCE(SUM(pps.paymentValue), 0),

	rs.statusDescription 

INTO

	v_receivable_value,

	v_partial_payments_value,

	v_old_status

FROM

	receivables rcs

INNER JOIN partial_payments pps ON

	rcs.receivableID = pps.receivable_ID

INNER JOIN receivable_status rs ON

	rcs.status_ID = rs.statusID

WHERE

	rcs.receivableID = p_receivable_ID;



SET

v_amount_left = v_receivable_value - v_partial_payments_value;



IF(v_amount_left = v_receivable_value) THEN

	/*No partial payments were performed hence the status will be set to 'New'*/

    SET

		v_new_status = 'New';



ELSEIF(v_amount_left > 0

	AND v_amount_left < v_receivable_value) THEN

	/*Partial payments were performed hence the status will be set accordingly*/

    SET

		v_new_status = 'Partially paid';



ELSEIF(v_amount_left = 0) THEN

	/*The amount left is 0 which means that the receivable was fully paid hence the status will be set to 'Paid'*/

    SET

		v_new_status = 'Paid';

END IF;



/*Updates the receivable status only if the new status is different from the old one*/

IF(v_old_status != v_new_status) THEN

UPDATE

	receivables

SET

	status_ID = (

	SELECT

		statusID

	FROM

		receivable_status

	WHERE

		statusDescription = v_new_status)

WHERE

	receivableID = p_receivable_ID;



SELECT

	CONCAT('STATUS UPDATE WAS PERFORMED', ' (OLD STATUS: ', v_old_status, '; NEW STATUS: ', v_new_status, ')') AS 'Performed action';

ELSE 



SELECT

	CONCAT('NO STATUS UPDATE IS NECESSARY!', ' (OLD STATUS: ', v_old_status, '; NEW STATUS: ', v_new_status, ')') AS 'Performed action';

END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `test_proc` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `test_proc`(IN p_input varchar(50))
BEGIN

	

	

SELECT 	* FROM incomes WHERE user_ID = 3;

	

	

	

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `truncate_tables` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `truncate_tables`()
BEGIN

DECLARE v_current_truncate_stmt TEXT;

DECLARE v_total_tables_found VARCHAR(50);

DECLARE v_finished TINYINT(1);

DECLARE truncate_tbl_command_cursor

CURSOR FOR
SELECT
	CONCAT('TRUNCATE TABLE ', table_name)
FROM
	information_schema.tables
WHERE
	table_schema = 'empty_db'
	AND table_name IN ('account_balance_storage', 'account_balance_storage_history', 'accounts', 'banks', 'broker_accounts', 'broker_portfolio_transfers', 'brokers',  'budget_plans', 'creditors', 'currency_exchange_rates', 'debtors', 'debts', 'dividends', 'expenses', 
	                   'external_accounts_balance', 'external_accounts_banking_fees', 'free_stocks', 'incomes', 'investment_taxes', 'partial_payments', 'portfolios', 'portfolios_symbols', 'receivable_history', 'receivables',  'saving_accounts_balance', 'saving_accounts_expenses', 
	                   'saving_accounts_interest', 'saving_accounts_transfers', 'savings', 'stock_option_plan_items', 'stock_option_plans', 'stock_order_items', 'stock_orders', 'symbols', 'trading_fees', 'users', 'users_creditors', 'users_debtors');

DECLARE CONTINUE HANDLER FOR NOT FOUND 

SET
v_finished = 1;

OPEN truncate_tbl_command_cursor;

SET
FOREIGN_KEY_CHECKS = 0;

SELECT
	FOUND_ROWS()
INTO
	v_total_tables_found;

SELECT
	CONCAT('FOUND ' || v_total_tables_found || ' TO TRUNCATE');

performTruncateTable:LOOP

	

FETCH truncate_tbl_command_cursor
INTO
	v_current_truncate_stmt;

IF v_finished = 1 THEN

LEAVE performTruncateTable;
END IF;

SET
@execution_variable = v_current_truncate_stmt;

PREPARE current_statement
FROM
@execution_variable;

EXECUTE current_statement;

SELECT
	CONCAT('CURRENT TRUNCATE TABLE COMMAND: ' || v_current_truncate_stmt);
END LOOP performTruncateTable;

CLOSE truncate_tbl_command_cursor;

SET
FOREIGN_KEY_CHECKS = 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_account_balance_on_transfer_delete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_account_balance_on_transfer_delete`(IN `p_sender_account_ID` INT(20), IN `p_receiving_account_ID` INT(20), IN `p_sent_value` DOUBLE, IN `p_received_value` DOUBLE, IN `p_transfer_date` DATE, OUT `p_sender_update_result` TINYINT(1), OUT `p_sender_storage_update_result` TINYINT(1),

OUT `p_receiver_update_result` TINYINT(1), OUT `p_receiver_storage_update_result` TINYINT(1))
    NO SQL
BEGIN

/* 

Description:	Procedure that updates the balance of the provided saving account when a transfer is deleted

 			 	

Input parameters:	p_sender_account_ID -> the id of the account from which the money was withdrawn

					p_receiving_account_ID -> the id of the account to which the money was transferred

					p_sent_value -> the amount sent by the sender account

					p_received_value -> the amount received by the receiving account

					p_transfer_date -> the date of the transfer

					p_account_role -> value indicating the transaction role of the account whose id is provided (sender- the account from which the money is withdrawn; receiver- the account to which the money is transferred)		



Output parameters:	p_sender_update_result -> a boolean value indicating whether the sender account balance record was successfully updated or not

					p_sender_storage_update_result -> a boolean value indicating whether the sender account balance storage record was successfully updated or not

					p_receiver_update_result -> a boolean value indicating whether the receiver account balance record was successfully updated or not

				    p_receiver_storage_update_result -> a boolean value indicating whether the receiver account balance storage record was successfully updated or not				

*/



DECLARE v_sender_account_type VARCHAR(20);



DECLARE v_receiving_account_type VARCHAR(20);



DECLARE v_sender_account_current_record_value DOUBLE;



DECLARE v_receiving_account_current_record_value DOUBLE;



DECLARE v_sender_account_new_record_value DOUBLE;



DECLARE v_receiving_account_new_record_value DOUBLE;



DECLARE v_saving_accounts_balance_record_name VARCHAR(50);



DECLARE v_current_storage_balance_sender_account DOUBLE;



DECLARE v_current_storage_balance_receiving_account DOUBLE;



DECLARE v_action_source VARCHAR(100);



DECLARE v_action_description VARCHAR(200);



DECLARE v_database_component_type VARCHAR(20);



DECLARE v_database_operation VARCHAR(20);



DECLARE v_value_triggering_update_sender_account DOUBLE;



DECLARE v_value_triggering_update_receiving_account DOUBLE;



DECLARE v_old_storage_balance_sender_account DOUBLE;



DECLARE v_new_storage_balance_sender_account DOUBLE;



DECLARE v_old_storage_balance_receiving_account DOUBLE;



DECLARE v_new_storage_balance_receiving_account DOUBLE;



DECLARE v_action_timestamp TIMESTAMP;



DECLARE v_account_balance_storage_update_result_sender_account TINYINT(1);



DECLARE v_account_balance_storage_update_result_receiving_account TINYINT(1);



/*Retrieves the account type for the sender account*/

SELECT

	get_account_type(p_sender_account_ID)

INTO

	v_sender_account_type

FROM

	DUAL;



/*Retrieves the account type for the receiving account*/

SELECT

	get_account_type(p_receiving_account_ID)

INTO

	v_receiving_account_type

FROM

	DUAL;



/*Retrieves the current balance record value for the month/year of the transfer for both sender and receiving accounts*/

SELECT

	get_balance_record_value_with_double_precision(p_sender_account_ID,

	p_transfer_date)

INTO

	v_sender_account_current_record_value

FROM

	DUAL;



SELECT

	get_balance_record_value_with_double_precision(p_receiving_account_ID,

	p_transfer_date)

INTO

	v_receiving_account_current_record_value

FROM

	DUAL;



/*Calculates the new value for the sender account balance record

On transfer delete the balance of the sender account will increase while the balance of the receiving account will decrease*/

SET

v_sender_account_new_record_value = v_sender_account_current_record_value + p_sent_value;



/*Calculates the new value for the receiving account balance record*/

SET

v_receiving_account_new_record_value = v_receiving_account_current_record_value - p_received_value;



/*Sets the name which will be used to update the saving accounts balance table record*/

SET

v_saving_accounts_balance_record_name = CONCAT('balance_record_', p_transfer_date);



/*Updates the corresponding balance record value for the sender account based on its type*/

IF v_sender_account_type LIKE '%SYSTEM_DEFINED%' THEN

UPDATE

	saving_accounts_balance

SET

	value = v_sender_account_new_record_value,

	recordName = v_saving_accounts_balance_record_name

WHERE

	account_ID = p_sender_account_ID

	AND MONTH = MONTH(p_transfer_date)

	AND YEAR = YEAR(p_transfer_date);



/*Retrieves the sender account balance record update result*/

SET

p_sender_update_result = ROW_COUNT();



ELSEIF v_sender_account_type LIKE '%USER_DEFINED%' THEN

UPDATE

	external_accounts_balance

SET

	value = v_sender_account_new_record_value,

	lastUpdatedDate = CURDATE()

WHERE

	account_ID = p_sender_account_ID

	AND MONTH(createdDate) = MONTH(p_transfer_date)

	AND YEAR(createdDate) = YEAR(p_transfer_date);



SET

p_sender_update_result = ROW_COUNT();

END IF;





IF v_receiving_account_type LIKE '%SYSTEM_DEFINED%' THEN

UPDATE

	saving_accounts_balance

SET

	value = v_receiving_account_new_record_value,

	recordName = v_saving_accounts_balance_record_name

WHERE

	account_ID = p_receiving_account_ID

	AND MONTH = MONTH(p_transfer_date)

	AND YEAR = YEAR(p_transfer_date);



/*Retrieves the receiving account balance record update result*/

SET

p_receiver_update_result = ROW_COUNT();



ELSEIF v_receiving_account_type LIKE '%USER_DEFINED%' THEN

UPDATE

	external_accounts_balance

SET

	value = v_receiving_account_new_record_value,

	lastUpdatedDate = CURDATE()

WHERE

	account_ID = p_receiving_account_ID

	AND MONTH(createdDate) = MONTH(p_transfer_date)

	AND YEAR(createdDate) = YEAR(p_transfer_date);



SET

p_receiver_update_result = ROW_COUNT();

END IF;



/*Account balance storage implementation-START*/



/*Account balance retrieval for the sender account*/

SELECT

	currentBalance

INTO

	v_current_storage_balance_sender_account

FROM

	account_balance_storage

WHERE

	account_ID = p_sender_account_ID;



/*Account balance retrieval for the receiving account*/

SELECT

	currentBalance

INTO

	v_current_storage_balance_receiving_account

FROM

	account_balance_storage

WHERE

	account_ID = p_receiving_account_ID;



SET v_action_source = 'update_account_balance_on_transfer_delete';



SET v_action_description = 'Transfer deletion';



SET v_database_component_type = 'PROCEDURE';



SET v_database_operation = 'DELETE';



/*The transfer deletion leads to an increase of the sender account balance so the value triggering the update will be positive*/

SET v_value_triggering_update_sender_account = p_sent_value;



/*The transfer deletion leads to an decrease of the receiving account balance so the value triggering the update will be negative*/

SET v_value_triggering_update_receiving_account = -p_received_value;



/*Sets the old storage balance for the sender and receiving account (the data  will be inserted into the account_balance_storage_history table)*/

SET v_old_storage_balance_sender_account = v_current_storage_balance_sender_account;



SET v_old_storage_balance_receiving_account = v_current_storage_balance_receiving_account;



/*Sets the new storage balance for the sender and receiving account (the data will be inserted into the account_balance_storage_history table)*/

SET v_new_storage_balance_sender_account = v_current_storage_balance_sender_account + p_sent_value;



SET v_new_storage_balance_receiving_account = v_current_storage_balance_receiving_account - p_received_value;



SET v_action_timestamp = CURRENT_TIMESTAMP();



/*Updates the current balance of the sender account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/

CALL update_account_balance_storage(p_sender_account_ID,

v_action_source,

v_action_description,

v_database_component_type,

v_database_operation,

v_value_triggering_update_sender_account,

v_old_storage_balance_sender_account,

v_new_storage_balance_sender_account,

v_action_timestamp,

v_account_balance_storage_update_result_sender_account);



/*Updates the current balance of the receiving account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/

CALL update_account_balance_storage(p_receiving_account_ID,

v_action_source,

v_action_description,

v_database_component_type,

v_database_operation,

v_value_triggering_update_receiving_account,

v_old_storage_balance_receiving_account,

v_new_storage_balance_receiving_account,

v_action_timestamp,

v_account_balance_storage_update_result_receiving_account);



SET p_sender_storage_update_result = v_account_balance_storage_update_result_sender_account;

SET p_receiver_storage_update_result = v_account_balance_storage_update_result_receiving_account;



/*Account balance storage implementation-END*/



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_account_balance_on_transfer_insert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_account_balance_on_transfer_insert`(IN `p_account_ID` INT(20), IN `p_transfer_value` DOUBLE, IN `p_transfer_date` DATE, IN `p_account_role` VARCHAR(50), OUT `p_execution_result` TINYINT(1))
    NO SQL
BEGIN
/* 
Description:	Procedure that updates the balance of the provided saving account when a transfer is performed
 			 	
Input parameters:	p_account_ID -> the id of the account whose balance record needs to be updated
					p_transfer_value -> the amount transferred
					p_transfer_date -> the date of the transfer
					p_account_role -> value indicating the transaction role of the account whose id is provided (sender- the account from which the money is withdrawn; receiver- the account to which the money is transferred)		

Output parameter:	p_execution_result -> a boolean value indicating whether the balance record update was successfull or not
*/

DECLARE v_account_type VARCHAR(50) DEFAULT NULL;

DECLARE v_record_name VARCHAR(50) DEFAULT NULL;

DECLARE v_current_value DOUBLE;

DECLARE v_new_value DOUBLE;

DECLARE v_inserted_rows INT DEFAULT 0;

/*Checks the account type*/
SELECT
	get_account_type(p_account_ID)
INTO
	v_account_type
FROM
	DUAL;

/*Updates DEFAULT saving accounts*/
IF v_account_type LIKE '%SYSTEM_DEFINED%' THEN

/*Retrieves the current balance record value*/
SELECT
	get_balance_record_value_with_double_precision(p_account_ID,
	p_transfer_date)
INTO
	v_current_value
FROM
	DUAL;

/*Sets a new name for the record, which reflects the date at which it was updated*/
SET
v_record_name = CONCAT('balance_record_', CURDATE());

/*Calculates the new record value based on the transaction role of the specified account*/
IF p_account_role = 'SENDER' THEN
SET
v_new_value = v_current_value - p_transfer_value;

ELSEIF p_account_role = 'RECEIVER' THEN
SET
v_new_value = v_current_value + p_transfer_value;

END IF;

/*Sets the corresponding balance record to the correct value*/
UPDATE
	saving_accounts_balance
SET
	value = v_new_value,
	recordName = v_record_name
WHERE
	account_ID = p_account_ID
	AND MONTH = MONTH(p_transfer_date)
	AND YEAR = YEAR(p_transfer_date);

/*Retrieves the number of rows affected by the UPDATE operation*/
SET
v_inserted_rows = ROW_COUNT();

END IF;

/*Updates CUSTOM saving accounts*/
IF v_account_type LIKE '%USER_DEFINED%' THEN

/*Retrieves the current balance record value*/
SELECT
	get_balance_record_value_with_double_precision(p_account_ID,
	p_transfer_date)
INTO
	v_current_value
FROM
	DUAL;


IF p_account_role = 'SENDER' THEN
SET
v_new_value = v_current_value - p_transfer_value;

ELSEIF p_account_role = 'RECEIVER' THEN
SET
v_new_value = v_current_value + p_transfer_value;

END IF;

/*Sets the corresponding balance record to the correct value*/
UPDATE
	external_accounts_balance
SET
	value = v_new_value,
	lastUpdatedDate = CURDATE()
WHERE
	account_ID = p_account_ID
	AND MONTH(createdDate) = MONTH(p_transfer_date)
	AND YEAR(createdDate) = YEAR(p_transfer_date);

/*Retrieves the number of rows affected by the UPDATE operation*/
SET
v_inserted_rows = ROW_COUNT();
END IF;

/*Checks if the update operation was successfull or not*/
IF v_inserted_rows = 1 THEN
SET
	p_execution_result = 1;
ELSE
SET
	p_execution_result = 0;
END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_account_balance_on_transfer_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_account_balance_on_transfer_update`(IN `p_sender_account_ID` INT(20), IN `p_receiving_account_ID` INT(20), IN `p_new_sent_value` DOUBLE, IN `p_old_sent_value` DOUBLE, IN `p_new_received_value` DOUBLE, IN `p_old_received_value` DOUBLE, IN `p_transfer_date` DATE, OUT `p_sender_update_result` TINYINT(1), OUT `p_sender_storage_update_result` TINYINT(1),

OUT `p_receiver_update_result` TINYINT(1), OUT `p_receiver_storage_update_result` TINYINT(1))
    NO SQL
BEGIN

/* 

Description:	Procedure that updates the balance of the provided saving account when a transfer is updated

 			 	

Input parameters:	p_sender_account_ID -> the id of the account from which the money was withdrawn

					p_receiving_account_ID -> the id of the account to which the money was transferred

					p_new_sent_value -> the updated value of the sent amount

					p_old_sent_value -> the old value of the sent amount				

					p_new_received_value -> the updated value of the received amount

					p_old_received_value -> the old value of the received amount

					p_transfer_date -> the date of the transfer



Output parameters:	p_sender_update_result -> a boolean value indicating whether the sender account balance record was successfully updated or not

				    p_sender_storage_update_result -> a boolean value indicating whether the sender account balance storage record was successfully updated or not

					p_receiver_update_result -> a boolean value indicating whether the receiver account balance record was successfully updated or not

				    p_receiver_storage_update_result -> a boolean value indicating whether the receiver account balance storage record was successfully updated or not				

*/



DECLARE v_sender_account_type VARCHAR(20);



DECLARE v_receiving_account_type VARCHAR(20);



DECLARE v_sent_value_diff DOUBLE;



DECLARE v_received_value_diff DOUBLE;



DECLARE v_sender_account_current_record_value DOUBLE;



DECLARE v_receiving_account_current_record_value DOUBLE;



DECLARE v_sender_account_new_record_value DOUBLE;



DECLARE v_receiving_account_new_record_value DOUBLE;



DECLARE v_saving_accounts_balance_record_name VARCHAR(50);



DECLARE v_current_storage_balance_sender_account DOUBLE;



DECLARE v_current_storage_balance_receiving_account DOUBLE;



DECLARE v_action_source VARCHAR(100);



DECLARE v_action_description VARCHAR(200);



DECLARE v_database_component_type VARCHAR(20);



DECLARE v_database_operation VARCHAR(20);



DECLARE v_value_triggering_update_sender_account DOUBLE;



DECLARE v_value_triggering_update_receiving_account DOUBLE;



DECLARE v_old_storage_balance_sender_account DOUBLE;



DECLARE v_new_storage_balance_sender_account DOUBLE;



DECLARE v_old_storage_balance_receiving_account DOUBLE;



DECLARE v_new_storage_balance_receiving_account DOUBLE;



DECLARE v_action_timestamp TIMESTAMP;



DECLARE v_account_balance_storage_update_result_sender_account TINYINT(1);



DECLARE v_account_balance_storage_update_result_receiving_account TINYINT(1);



/*Get the account type for the sender account*/

SELECT

	get_account_type(p_sender_account_ID)

INTO

	v_sender_account_type

FROM

	DUAL;



/*Get the account type for the receiving account*/

SELECT

	get_account_type(p_receiving_account_ID)

INTO

	v_receiving_account_type

FROM

	DUAL;



/*Retrieves the current balance record value for the month/year of the transfer for both sender and receiving accounts*/

SELECT

	get_balance_record_value_with_double_precision(p_sender_account_ID,

	p_transfer_date)

INTO

	v_sender_account_current_record_value

FROM

	DUAL;



SELECT

	get_balance_record_value_with_double_precision(p_receiving_account_ID,

	p_transfer_date)

INTO

	v_receiving_account_current_record_value

FROM

	DUAL;



/*Account balance storage retrieval for the sender account*/

SELECT

	currentBalance

INTO

	v_current_storage_balance_sender_account

FROM

	account_balance_storage

WHERE

	account_ID = p_sender_account_ID;



/*Account balance storage retrieval for the receiving account*/

SELECT

	currentBalance

INTO

	v_current_storage_balance_receiving_account

FROM

	account_balance_storage

WHERE

	account_ID = p_receiving_account_ID;



/*Calculates the difference between the new value and old value for the sent and received amounts*/

SET

v_sent_value_diff = p_new_sent_value - p_old_sent_value;



SET

v_received_value_diff = p_new_received_value - p_old_received_value;



/*Calculates the new value for the sender account balance record and for the storage record*/

IF v_sent_value_diff >= 0 THEN

SET

	v_sender_account_new_record_value = v_sender_account_current_record_value - v_sent_value_diff;

SET	

	v_new_storage_balance_sender_account = v_current_storage_balance_sender_account - v_sent_value_diff;



ELSE

SET

	v_sender_account_new_record_value = v_sender_account_current_record_value + ABS(v_sent_value_diff);

SET

	v_new_storage_balance_sender_account = v_current_storage_balance_sender_account + ABS(v_sent_value_diff);



END IF;





/*Calculates the new value for the receiving account balance record and for the storage record*/

IF v_received_value_diff >= 0 THEN

SET

	v_receiving_account_new_record_value = v_receiving_account_current_record_value + v_received_value_diff;

SET

	v_new_storage_balance_receiving_account = v_current_storage_balance_receiving_account + v_received_value_diff;



ELSE 

SET

	v_receiving_account_new_record_value = v_receiving_account_current_record_value - ABS(v_received_value_diff);

SET

	v_new_storage_balance_receiving_account = v_current_storage_balance_receiving_account - ABS(v_received_value_diff);



END IF;



/*Sets the name which will be used to update saving accounts balance table record*/

SET

v_saving_accounts_balance_record_name = CONCAT('balance_record_', p_transfer_date);



/*Updates the corresponding balance record value for the sender account based on its type*/

IF v_sender_account_type LIKE '%SYSTEM_DEFINED%' THEN

UPDATE

	saving_accounts_balance

SET

	value = v_sender_account_new_record_value,

	recordName = v_saving_accounts_balance_record_name

WHERE

	account_ID = p_sender_account_ID

	AND MONTH = MONTH(p_transfer_date)

	AND YEAR = YEAR(p_transfer_date);



/*Retrieves the sender account balance record update result*/

SET

	p_sender_update_result = ROW_COUNT();



ELSEIF v_sender_account_type LIKE '%USER_DEFINED%' THEN

UPDATE

	external_accounts_balance

SET

	value = v_sender_account_new_record_value,

	lastUpdatedDate = CURDATE()

WHERE

	account_ID = p_sender_account_ID

	AND MONTH(createdDate) = MONTH(p_transfer_date)

	AND YEAR(createdDate) = YEAR(p_transfer_date);



SET

	p_sender_update_result = ROW_COUNT();



END IF;



/*Updates the corresponding balance record value for the receiving account based on its type*/

IF v_receiving_account_type LIKE '%SYSTEM_DEFINED%' THEN

UPDATE

	saving_accounts_balance

SET

	value = v_receiving_account_new_record_value,

	recordName = v_saving_accounts_balance_record_name

WHERE

	account_ID = p_receiving_account_ID

	AND MONTH = MONTH(p_transfer_date)

	AND YEAR = YEAR(p_transfer_date);



/*Retrieves the receiving account balance record update result*/

SET

	p_receiver_update_result = ROW_COUNT();



ELSEIF v_receiving_account_type LIKE '%USER_DEFINED%' THEN

UPDATE

	external_accounts_balance

SET

	value = v_receiving_account_new_record_value,

	lastUpdatedDate = CURDATE()

WHERE

	account_ID = p_receiving_account_ID

	AND MONTH(createdDate) = MONTH(p_transfer_date)

	AND YEAR(createdDate) = YEAR(p_transfer_date);



SET

	p_receiver_update_result = ROW_COUNT();



END IF;



/*Account balance storage implementation-START*/



SET v_action_source = 'update_balance_record_on_transfer_update';



SET v_action_description = 'Transfer update';



SET v_database_component_type = 'PROCEDURE';



SET v_database_operation = 'UPDATE';



/*The account balance storage record for the sender account is modified as a result of the sent value update*/

SET v_value_triggering_update_sender_account = -v_sent_value_diff;



/*The account balance storage record for the receiving account is modified as a result of the received value update*/

SET v_value_triggering_update_receiving_account = v_received_value_diff;



/*Sets the old storage balance for the sender and receiving account (the data  will be inserted into the account_balance_storage_history table)*/

SET v_old_storage_balance_sender_account = v_current_storage_balance_sender_account;



SET v_old_storage_balance_receiving_account = v_current_storage_balance_receiving_account;



SET v_action_timestamp = CURRENT_TIMESTAMP();



/*Updates the current balance of the sender account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/

CALL update_account_balance_storage(p_sender_account_ID,

v_action_source,

v_action_description,

v_database_component_type,

v_database_operation,

v_value_triggering_update_sender_account,

v_old_storage_balance_sender_account,

v_new_storage_balance_sender_account,

v_action_timestamp,

v_account_balance_storage_update_result_sender_account);



/*Updates the current balance of the receiving account from the account balance storage table and inserts a new record into the account_balance_storage_history table*/

CALL update_account_balance_storage(p_receiving_account_ID,

v_action_source,

v_action_description,

v_database_component_type,

v_database_operation,

v_value_triggering_update_receiving_account,

v_old_storage_balance_receiving_account,

v_new_storage_balance_receiving_account,

v_action_timestamp,

v_account_balance_storage_update_result_receiving_account);



SET p_sender_storage_update_result = v_account_balance_storage_update_result_sender_account;

SET p_receiver_storage_update_result = v_account_balance_storage_update_result_receiving_account;



/*Account balance storage implementation-END*/



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_account_balance_storage` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_account_balance_storage`(IN p_account_ID INT(20), IN p_action_source VARCHAR(100),

IN p_action_description VARCHAR(200), IN p_database_component_type VARCHAR(20), IN p_database_operation VARCHAR(20), IN p_value_triggering_update DOUBLE,

IN p_old_balance_value DOUBLE, IN p_new_balance_value DOUBLE, IN p_action_timestamp TIMESTAMP, OUT p_update_result TINYINT(1))
BEGIN

/*

Description:	Procedure that updates the account balance storage record and also creates a history record which saves the change that has been performed	

 

Input parameters:	p_account_ID -> the id of the account whose balance storage record needs to be updated

					p_action_source -> the name of the component (trigger, procedure, etc) which initates the balance storage record update

					p_action_description -> the description of the action which leads to the balance storage record update

					p_database_component_type -> the type of the database component (trigger, procedure, etc) from which the update action is initiated

					p_database_operation -> the DML operation that was performed on the data (INSERT, UPDATE, DELETE)

					p_value_triggering_update -> the amount which needs to be reflected into the current balance of the account (it can be positive/negative)

					p_old_balance_value -> the current balance value before the storage record update

					p_new_balance_value -> the current balance value after the storage record update

					p_action_timestamp -> the timestamp of the update operation that was performed on the balance storage record

					

Output parameters:	p_update_result -> a boolean value indicating whether the balance storage record update was successful or not

 */

	

UPDATE

	account_balance_storage

SET

	currentBalance = p_new_balance_value,

	lastUpdatedDate = p_action_timestamp

WHERE

	account_ID = p_account_ID;



SET p_update_result = ROW_COUNT();



/*A new record is inserted into the history table only if the account balance storage update was successful*/ 

IF p_update_result = 1 THEN 



CALL log_account_balance_storage_history(

p_account_ID,

p_action_source,

p_action_description, 

p_database_component_type, 

p_database_operation,

p_value_triggering_update,

p_old_balance_value,

p_action_timestamp

);



END IF;



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-12-30 11:17:38
