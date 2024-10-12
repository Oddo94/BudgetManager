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
  CONSTRAINT `account_balance_storage_ibfk_1` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE
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
-- Table structure for table `banks`
--

DROP TABLE IF EXISTS `banks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `banks` (
  `bankID` int(10) NOT NULL AUTO_INCREMENT,
  `bankName` varchar(50) NOT NULL,
  PRIMARY KEY (`bankID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `banks`
--

LOCK TABLES `banks` WRITE;
/*!40000 ALTER TABLE `banks` DISABLE KEYS */;
INSERT INTO `banks` VALUES (1,'Banca Transilvania'),(2,'Banca Comerciala Romana'),(3,'BRD Group Societe Generale'),(4,'ING'),(5,'CEC Bank'),(6,'NO_BANK');
/*!40000 ALTER TABLE `banks` ENABLE KEYS */;
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
-- Table structure for table `expense_types`
--

DROP TABLE IF EXISTS `expense_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `expense_types` (
  `categoryID` int(10) NOT NULL AUTO_INCREMENT,
  `categoryName` varchar(30) NOT NULL,
  PRIMARY KEY (`categoryID`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `expense_types`
--

LOCK TABLES `expense_types` WRITE;
/*!40000 ALTER TABLE `expense_types` DISABLE KEYS */;
INSERT INTO `expense_types` VALUES (4,'Utilities'),(5,'Education'),(6,'Sport'),(7,'Food'),(8,'Transport'),(9,'Housing'),(10,'Household items'),(11,'Insurance'),(12,'Healthcare'),(13,'Gifts/donations'),(14,'Entertainment'),(15,'Clothing'),(16,'Personal care'),(17,'Others'),(18,'IT&C'),(19,'Footwear'),(20,'Hobbies'),(21,'Travelling'),(22,'Souvenirs'),(23,'Banking fees'),(24,'Restaurants/bars/cafes'),(25,'Entrance fees');
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
  CONSTRAINT `external_accounts_balance_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE
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
  CONSTRAINT `external_accounts_banking_fees_ibfk_1` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE
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
	`incomes` FOR EACH ROW BEGIN
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
	saving_accounts
WHERE
	user_ID = v_user_ID
	AND type_ID = 1;

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
  CONSTRAINT `receivable_history_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON DELETE CASCADE ON UPDATE CASCADE,
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
  CONSTRAINT `receivables_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE,
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
	`receivables` FOR EACH ROW BEGIN
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
	sa.user_ID
INTO
	v_user_ID
FROM
	receivables rcs
INNER JOIN saving_accounts sa ON
	rcs.account_ID = sa.accountID
WHERE
	rcs.receivableID = NEW.receivableID;

/*Retrieves the default account ID for the user*/
SELECT
	sa.accountID
INTO
	v_default_account_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.user_ID = v_user_ID
	AND sat.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

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
-- Table structure for table `saving_account_types`
--

DROP TABLE IF EXISTS `saving_account_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saving_account_types` (
  `typeID` int(10) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saving_account_types`
--

LOCK TABLES `saving_account_types` WRITE;
/*!40000 ALTER TABLE `saving_account_types` DISABLE KEYS */;
INSERT INTO `saving_account_types` VALUES (1,'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'),(2,'USER_DEFINED-CUSTOM_SAVING_ACCOUNT');
/*!40000 ALTER TABLE `saving_account_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `saving_accounts`
--

DROP TABLE IF EXISTS `saving_accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saving_accounts` (
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
  CONSTRAINT `saving_accounts_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_ibfk_2` FOREIGN KEY (`type_ID`) REFERENCES `saving_account_types` (`typeID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_ibfk_3` FOREIGN KEY (`bank_ID`) REFERENCES `banks` (`bankID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_ibfk_4` FOREIGN KEY (`currency_ID`) REFERENCES `currencies` (`currencyID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saving_accounts`
--

LOCK TABLES `saving_accounts` WRITE;
/*!40000 ALTER TABLE `saving_accounts` DISABLE KEYS */;
/*!40000 ALTER TABLE `saving_accounts` ENABLE KEYS */;
UNLOCK TABLES;

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
  CONSTRAINT `saving_accounts_balance_ibfk_2` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE
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
  CONSTRAINT `saving_accounts_expenses_ibfk_3` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE
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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update/creation after SAE insertion` AFTER INSERT ON `saving_accounts_expenses` FOR EACH ROW BEGIN
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
	sa.accountID,
	usr.userID
INTO
	v_default_account_ID,
	v_user_ID
FROM
	saving_accounts sa
INNER JOIN users usr ON
	sa.user_ID = usr.userID
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = NEW.account_ID
	AND sat.typeName LIKE '%SYSTEM_DEFINED%';

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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update after SAE update` AFTER UPDATE ON `saving_accounts_expenses` FOR EACH ROW BEGIN

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
	sa.accountID,
	usr.userID
INTO
	v_default_account_ID,
	v_user_ID
FROM
	saving_accounts sa
INNER JOIN users usr ON
	sa.user_ID = usr.userID
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = NEW.account_ID
	AND sat.typeName LIKE '%SYSTEM_DEFINED%';


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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update after SAE deletion` AFTER DELETE ON `saving_accounts_expenses` FOR EACH ROW BEGIN 
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
	sa.accountID,
	usr.userID
INTO
	v_default_account_ID,
	v_user_ID
FROM
	saving_accounts sa
INNER JOIN users usr ON
	sa.user_ID = usr.userID
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = OLD.account_ID
	AND sat.typeName LIKE '%SYSTEM_DEFINED%';

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
  CONSTRAINT `saving_accounts_interest_ibfk_1` FOREIGN KEY (`account_ID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE,
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
  CONSTRAINT `saving_accounts_transfers_ibfk_1` FOREIGN KEY (`senderAccountID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE,
  CONSTRAINT `saving_accounts_transfers_ibfk_2` FOREIGN KEY (`receivingAccountID`) REFERENCES `saving_accounts` (`accountID`) ON UPDATE CASCADE
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
	`saving_accounts_transfers` FOR EACH ROW BEGIN

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
	saving_accounts
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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update/creation on saving insertion` AFTER INSERT ON `savings` FOR EACH ROW BEGIN

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
	sa.accountID
INTO
	v_default_account_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.user_ID = NEW.user_ID
	AND sat.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on saving update` AFTER UPDATE ON `savings` FOR EACH ROW BEGIN

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
	sa.accountID
INTO
	v_default_account_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.user_ID = NEW.user_ID
	AND sat.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Balance record update on saving deletion` AFTER DELETE ON `savings` FOR EACH ROW BEGIN

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
	sa.accountID
INTO
	v_default_account_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.user_ID = OLD.user_ID
	AND sat.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT';

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
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `userID` int(10) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `salt` binary(16) NOT NULL,
  `password` varchar(50) NOT NULL,
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
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = p_account_ID;

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
	sat.typeName,
	sa.user_ID
INTO
	v_account_type,
	v_user_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = p_account_ID;

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
	sa.accountName ,
	COALESCE(bnk.bankName, 'N/A'),
	crs.currencyName ,
	sa.creationDate
INTO
	p_account_name,
	p_bank_name,
	p_account_ccy,
	p_account_creation_date
FROM
	saving_accounts sa
INNER JOIN banks bnk ON
	sa.bank_ID = bnk.bankID
INNER JOIN currencies crs ON
	sa.currency_ID = crs.currencyID 
WHERE
	sa.accountID = p_account_ID;

/*Retrieves the account type and the ID of the user that owns the specified account*/
SELECT
	sat.typeName,

	sa.user_ID
INTO
	p_account_type,

	v_user_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = p_account_ID;
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
	currentBalance
INTO
	v_account_balance
FROM
	account_balance_storage
WHERE
	account_ID = p_account_ID;

-- SELECT
-- 	(CONCAT('ACCOUNT BALANCE FOR DEFAULT SAVING ACCOUNT: ', v_account_balance));

ELSEIF p_account_type = 'USER_DEFINED-CUSTOM_SAVING_ACCOUNT' THEN
SET p_total_savings = 0;

SET p_total_unpaid_receivable_amount = 0;

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
	currentBalance
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
	sat.typeName,

	sa.user_ID
INTO
	v_account_type,

	v_user_ID
FROM
	saving_accounts sa
INNER JOIN saving_account_types sat ON
	sa.type_ID = sat.typeID
WHERE
	sa.accountID = p_account_ID;

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
	sa.accountID, sa.accountName
FROM
	saving_accounts sa
LEFT JOIN account_balance_storage abs ON
	sa.accountID = abs.account_ID
WHERE
	abs.account_ID IS NULL;

DECLARE CONTINUE HANDLER FOR NOT FOUND SET v_finished = 1;

OPEN account_cursor;

insertAccounts:LOOP

	FETCH account_cursor
INTO 
	v_current_account_ID, v_current_account_name;

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
	CONCAT('Cannot insert the account named \'', v_current_account_name, '\'', ' because its current balance is negative. The actual balance value is ', v_current_account_balance) AS 'ERROR MESSAGE';
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
	AND table_name IN ('account_balance_storage', 'account_balance_storage_history', 'budget_plans', 'creditors', 'debtors', 'debts', 'expenses', 'external_accounts_balance', 'external_accounts_banking_fees', 'incomes', 'partial_payments', 'receivable_history', 'receivables', 'saving_accounts', 'saving_accounts_balance', 'saving_accounts_expenses', 'saving_accounts_interest', 'saving_accounts_transfers', 'savings', 'users', 'users_creditors', 'users_debtors');

DECLARE CONTINUE HANDLER FOR NOT FOUND 
SET
v_finished = 1;

OPEN truncate_tbl_command_cursor;
SET FOREIGN_KEY_CHECKS=0;

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

SET @execution_variable = v_current_truncate_stmt;
PREPARE current_statement FROM @execution_variable;
EXECUTE current_statement;

SELECT
	CONCAT('CURRENT TRUNCATE TABLE COMMAND: ' || v_current_truncate_stmt);

END LOOP performTruncateTable;

CLOSE truncate_tbl_command_cursor;

SET FOREIGN_KEY_CHECKS=1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_account_balance_on_transfer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_account_balance_on_transfer`(IN `p_account_ID` INT(20), IN `p_transfer_value` INT(20), IN `p_transfer_date` DATE, IN `p_account_role` VARCHAR(50), OUT `p_execution_result` TINYINT(1))
    NO SQL
BEGIN


DECLARE v_account_type VARCHAR(50) DEFAULT NULL;

DECLARE v_record_name VARCHAR(50) DEFAULT NULL;

DECLARE v_current_value INT DEFAULT 0;

DECLARE v_new_value INT DEFAULT 0;

DECLARE v_inserted_rows INT DEFAULT 0;


SELECT
	get_account_type(p_account_ID)
INTO
	v_account_type
FROM
	DUAL;



IF v_account_type LIKE '%SYSTEM_DEFINED%' THEN




SELECT
	get_balance_record_value(p_account_ID,
	p_transfer_date)
INTO
	v_current_value
FROM
	DUAL;


SET
v_record_name = CONCAT('balance_record_', CURDATE());


IF p_account_role = 'SENDER' THEN
SET
v_new_value = v_current_value - p_transfer_value;

ELSEIF p_account_role = 'RECEIVER' THEN
SET
v_new_value = v_current_value + p_transfer_value;
END IF;


UPDATE
	saving_accounts_balance
SET
	value = v_new_value,
	recordName = v_record_name
WHERE
	account_ID = p_account_ID
	AND MONTH = MONTH(p_transfer_date)
	AND YEAR = YEAR(p_transfer_date);


SET
v_inserted_rows = ROW_COUNT();

END IF;


IF v_account_type LIKE '%USER_DEFINED%' THEN





SELECT
	get_balance_record_value(p_account_ID,
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


UPDATE
	external_accounts_balance
SET
	value = v_new_value,
	lastUpdatedDate = CURDATE()
WHERE
	account_ID = p_account_ID
	AND MONTH(createdDate) = MONTH(p_transfer_date)
	AND YEAR(createdDate) = YEAR(p_transfer_date);


SET
v_inserted_rows = ROW_COUNT();
END IF;


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
/*!50003 DROP PROCEDURE IF EXISTS `update_balance_record_on_transfer_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_balance_record_on_transfer_update`(IN `p_sender_account_ID` INT(20), IN `p_receiving_account_ID` INT(20), IN `p_new_sent_value` INT(20), IN `p_old_sent_value` INT(20), IN `p_new_received_value` INT(20), IN `p_old_received_value` INT(20), IN `p_transfer_date` DATE, OUT `p_sender_update_result` TINYINT(1),
OUT `p_receiver_update_result` TINYINT(1))
    NO SQL
BEGIN


DECLARE v_sender_account_type VARCHAR(20) DEFAULT NULL;

DECLARE v_receiving_account_type VARCHAR(20) DEFAULT NULL;

DECLARE v_sent_value_diff INT DEFAULT 0;

DECLARE v_received_value_diff INT DEFAULT 0;

DECLARE v_sender_account_current_record_value INT DEFAULT 0;

DECLARE v_receiving_account_current_record_value INT DEFAULT 0;

DECLARE v_sender_account_new_record_value INT DEFAULT 0;

DECLARE v_receiving_account_new_record_value INT DEFAULT 0;

DECLARE v_saving_accounts_balance_record_name VARCHAR(50) DEFAULT NULL;


SELECT
	get_account_type(p_sender_account_ID)
INTO
	v_sender_account_type
FROM
	DUAL;


SELECT
	get_account_type(p_receiving_account_ID)
INTO
	v_receiving_account_type
FROM
	DUAL;





SELECT
	get_balance_record_value(p_sender_account_ID,
	p_transfer_date)
INTO
	v_sender_account_current_record_value
FROM
	DUAL;

SELECT
	get_balance_record_value(p_receiving_account_ID,
	p_transfer_date)
INTO
	v_receiving_account_current_record_value
FROM
	DUAL;


SET
v_sent_value_diff = p_new_sent_value - p_old_sent_value;

SET
v_received_value_diff = p_new_received_value - p_old_received_value;


IF v_sent_value_diff >= 0 THEN
SET
	v_sender_account_new_record_value = v_sender_account_current_record_value - v_sent_value_diff;

ELSE
SET
	v_sender_account_new_record_value = v_sender_account_current_record_value + ABS(v_sent_value_diff);

END IF;




IF v_received_value_diff >= 0 THEN
SET
	v_receiving_account_new_record_value = v_receiving_account_current_record_value + v_received_value_diff;

ELSE 
SET
	v_receiving_account_new_record_value = v_receiving_account_current_record_value - ABS(v_received_value_diff);

END IF;



SET
v_saving_accounts_balance_record_name = CONCAT('balance_record_', p_transfer_date);


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

-- Dump completed on 2024-10-12 11:22:23
