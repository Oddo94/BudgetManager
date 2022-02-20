-- phpMyAdmin SQL Dump
-- version 5.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 20, 2022 at 08:47 AM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `empty_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `banks`
--

CREATE TABLE `banks` (
  `bankID` int(10) NOT NULL,
  `bankName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `budget_plans`
--

CREATE TABLE `budget_plans` (
  `planID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `planName` varchar(30) NOT NULL,
  `expenseLimit` int(2) NOT NULL,
  `debtLimit` int(2) NOT NULL,
  `savingLimit` int(2) NOT NULL,
  `planType` int(10) NOT NULL,
  `thresholdPercentage` int(2) NOT NULL,
  `hasAlarm` tinyint(1) NOT NULL,
  `startDate` date NOT NULL,
  `endDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `creditors`
--

CREATE TABLE `creditors` (
  `creditorID` int(10) NOT NULL,
  `creditorName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `currencies`
--

CREATE TABLE `currencies` (
  `currencyID` int(10) NOT NULL,
  `currencyName` varchar(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `debtors`
--

CREATE TABLE `debtors` (
  `debtorID` int(10) NOT NULL,
  `debtorName` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `debts`
--

CREATE TABLE `debts` (
  `debtID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `creditor_ID` int(10) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `expenses`
--

CREATE TABLE `expenses` (
  `expenseID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `expense_types`
--

CREATE TABLE `expense_types` (
  `categoryID` int(10) NOT NULL,
  `categoryName` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `expense_types`
--

INSERT INTO `expense_types` (`categoryID`, `categoryName`) VALUES
(4, 'Food'),
(5, 'Transport'),
(6, 'Housing'),
(7, 'Household items'),
(8, 'Insurance'),
(9, 'Healthcare'),
(10, 'Utilities'),
(11, 'Gifts/donations'),
(12, 'Entertainment'),
(13, 'Clothing'),
(14, 'Education'),
(15, 'Sport'),
(16, 'Personal care'),
(17, 'Others');

-- --------------------------------------------------------

--
-- Table structure for table `incomes`
--

CREATE TABLE `incomes` (
  `incomeID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `incomeType` int(10) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `income_types`
--

CREATE TABLE `income_types` (
  `typeID` int(10) NOT NULL,
  `typeName` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `income_types`
--

INSERT INTO `income_types` (`typeID`, `typeName`) VALUES
(1, 'Active income'),
(2, 'Passive income');

-- --------------------------------------------------------

--
-- Table structure for table `partial_payments`
--

CREATE TABLE `partial_payments` (
  `paymentID` int(10) NOT NULL,
  `receivable_ID` int(10) NOT NULL,
  `paymentName` varchar(50) NOT NULL,
  `paymentValue` int(20) NOT NULL,
  `paymentDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Triggers `partial_payments`
--
DELIMITER $$
CREATE TRIGGER `Total paid amount update on delete` AFTER DELETE ON `partial_payments` FOR EACH ROW UPDATE receivables SET totalPaidAmount = (SELECT SUM(paymentValue) FROM partial_payments WHERE receivable_ID = OLD.receivable_ID) WHERE receivables.receivableID = OLD.receivable_ID
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `Total paid amount update on insert` AFTER INSERT ON `partial_payments` FOR EACH ROW UPDATE receivables SET totalPaidAmount = (SELECT SUM(paymentValue) FROM partial_payments WHERE receivable_ID = NEW.receivable_ID) WHERE receivables.receivableID = NEW.receivable_ID
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `Total paid amount update on update` AFTER UPDATE ON `partial_payments` FOR EACH ROW UPDATE receivables SET totalPaidAmount = (SELECT SUM(paymentValue) FROM partial_payments WHERE receivable_ID = NEW.receivable_ID) WHERE receivables.receivableID = NEW.receivable_ID
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `plan_types`
--

CREATE TABLE `plan_types` (
  `typeID` int(10) NOT NULL,
  `typeName` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `plan_types`
--

INSERT INTO `plan_types` (`typeID`, `typeName`) VALUES
(1, 'One month'),
(2, 'Six months');

-- --------------------------------------------------------

--
-- Table structure for table `receivables`
--

CREATE TABLE `receivables` (
  `receivableID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `debtor_ID` int(10) DEFAULT NULL,
  `user_ID` int(10) DEFAULT NULL,
  `totalPaidAmount` int(20) NOT NULL,
  `dateCreated` date NOT NULL,
  `dateDue` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `savings`
--

CREATE TABLE `savings` (
  `savingID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Triggers `savings`
--
DELIMITER $$
CREATE TRIGGER `Balance record creation` AFTER INSERT ON `savings` FOR EACH ROW BEGIN
DECLARE recordsNumber INTEGER(10);
DECLARE defaultAccountID INTEGER(10);
DECLARE recordName VARCHAR(40);

SET recordName = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

SELECT accountID
INTO defaultAccountID
FROM saving_accounts
WHERE user_ID = NEW.user_ID
AND type_ID = 1;

IF defaultAccountID > 0 THEN
SELECT COUNT(*) 
INTO recordsNumber
FROM saving_accounts_balance
WHERE month = MONTH(NEW.date)
AND year = YEAR(NEW.date)
AND user_ID = NEW.user_ID
AND account_ID = defaultAccountID;


IF recordsNumber = 0 THEN
INSERT INTO saving_accounts_balance(
user_ID,
account_ID,
recordName,
value,
month,
year
) VALUES (
NEW.user_ID,
defaultAccountID,
recordName,
NEW.value,
MONTH(NEW.date),
YEAR(NEW.date)
);
ELSE
UPDATE saving_accounts_balance
SET value = value + NEW.value,
recordName = recordName
WHERE month = MONTH(NEW.date)
AND year = YEAR(NEW.date)
AND user_ID = NEW.user_ID
AND account_ID = defaultAccountID;

END IF;
END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `Balance record update` AFTER UPDATE ON `savings` FOR EACH ROW BEGIN

DECLARE recordAmountDifference INT(10);
DECLARE newRecordName VARCHAR(30);
DECLARE defaultAccountID INT(10);

SET recordAmountDifference = NEW.value - OLD.value;

SET newRecordName = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

SET defaultAccountID = (SELECT accountID
FROM saving_accounts
WHERE user_ID = NEW.user_ID
AND type_ID = 1); 

IF defaultAccountID > 0 THEN
UPDATE saving_accounts_balance SET 
recordName = newRecordName,
value = value + recordAmountDifference
WHERE month = MONTH(NEW.date)
AND year = YEAR(NEW.date)
AND user_ID = NEW.user_ID
AND account_ID = defaultAccountID;

END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `Balance record update after deleted saving` AFTER DELETE ON `savings` FOR EACH ROW BEGIN

DECLARE newRecordName VARCHAR(30);
DECLARE defaultAccountID INT(10);
DECLARE subtractedAmount INT(10);

SET newRecordName = CONCAT('balance_record_', DATE_FORMAT(CURRENT_DATE,'%Y-%m-%d'));

SET defaultAccountID = (SELECT accountID
FROM saving_accounts
WHERE user_ID = OLD.user_ID
AND type_ID = 1);

SET subtractedAmount = OLD.value;

IF defaultAccountID > 0 THEN
UPDATE saving_accounts_balance SET 
recordName = newRecordName,
value = value - subtractedAmount
WHERE month = MONTH(OLD.DATE)
AND year = YEAR(OLD.DATE)
AND user_ID = OLD.user_ID
AND account_ID = defaultAccountID;
END IF;

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `saving_accounts`
--

CREATE TABLE `saving_accounts` (
  `accountID` int(10) NOT NULL,
  `accountName` varchar(50) DEFAULT NULL,
  `user_ID` int(10) NOT NULL,
  `type_ID` int(10) NOT NULL,
  `bank_ID` int(10) NOT NULL,
  `currency_ID` int(10) NOT NULL,
  `currentBalance` int(20) DEFAULT NULL,
  `creationDate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `saving_accounts_balance`
--

CREATE TABLE `saving_accounts_balance` (
  `recordID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `account_ID` int(10) DEFAULT NULL,
  `recordName` varchar(50) NOT NULL,
  `value` int(10) NOT NULL,
  `month` int(2) NOT NULL,
  `year` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `saving_accounts_expenses`
--

CREATE TABLE `saving_accounts_expenses` (
  `expenseID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Triggers `saving_accounts_expenses`
--
DELIMITER $$
CREATE TRIGGER `Balance record update after SAE deletion` AFTER DELETE ON `saving_accounts_expenses` FOR EACH ROW BEGIN 
DECLARE newRecordName VARCHAR(30);
DECLARE defaultAccountID INT(10);
DECLARE addedAmount INT(10);

SET newRecordName = CONCAT('balance_record_', DATE_FORMAT(CURRENT_DATE,'%Y-%m-%d'));

SET defaultAccountID = (SELECT accountID
FROM saving_accounts
WHERE user_ID = OLD.user_ID
AND type_ID = 1);

SET addedAmount = OLD.value;

IF defaultAccountID > 0 THEN 
UPDATE saving_accounts_balance SET 
recordName = newRecordName,
value = value + addedAmount
WHERE month = MONTH(OLD.DATE)
AND year = YEAR(OLD.DATE)
AND user_ID = OLD.user_ID
AND account_ID = defaultAccountID;

END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `Balance record update after SAE update` AFTER UPDATE ON `saving_accounts_expenses` FOR EACH ROW BEGIN
DECLARE recordAmountDifference INT(10);
DECLARE newRecordName VARCHAR(40);
DECLARE defaultAccountID INTEGER(10);

SET recordAmountDifference = OLD.value - NEW.value;

SET newRecordName = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

SET defaultAccountID = (SELECT accountID
FROM saving_accounts
WHERE user_ID = NEW.user_ID
AND type_ID = 1);

IF defaultAccountID > 0 THEN
UPDATE saving_accounts_balance
SET value = value + recordAmountDifference,
recordName = newRecordName
WHERE month = MONTH(NEW.date)
AND year = YEAR(NEW.date)
AND user_ID = NEW.user_ID
AND account_ID = defaultAccountID;

END IF;

END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `Balance record update/creation after SAE creation` AFTER INSERT ON `saving_accounts_expenses` FOR EACH ROW BEGIN
DECLARE recordsNumber INTEGER(10);
DECLARE defaultAccountID INTEGER(10);
DECLARE recordName VARCHAR(40);

SET recordName = CONCAT('balance_record_', DATE_FORMAT(NEW.date, '%Y-%m-%d'));

SELECT accountID
INTO defaultAccountID
FROM saving_accounts
WHERE user_ID = NEW.user_ID
AND type_ID = 1;

IF defaultAccountID > 0 THEN
SELECT COUNT(*) 
INTO recordsNumber
FROM saving_accounts_balance
WHERE month = MONTH(NEW.date)
AND year = YEAR(NEW.date)
AND user_ID = NEW.user_ID
AND account_ID = defaultAccountID;

IF recordsNumber = 0 THEN
INSERT INTO saving_accounts_balance(
user_ID,
account_ID,
recordName,
value,
month,
year
) VALUES (
NEW.user_ID,
defaultAccountID,
recordName,
-(NEW.value),
MONTH(NEW.date),
YEAR(NEW.date)
);
ELSE
UPDATE saving_accounts_balance
SET value = value - NEW.value,
recordName = recordName
WHERE month = MONTH(NEW.date)
AND year = YEAR(NEW.date)
AND user_ID = NEW.user_ID
AND account_ID = defaultAccountID;

END IF;
END IF;

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `saving_accounts_transfers`
--

CREATE TABLE `saving_accounts_transfers` (
  `transferID` int(10) NOT NULL,
  `senderAccountID` int(10) NOT NULL,
  `receivingAccountID` int(10) DEFAULT NULL,
  `transferName` varchar(50) DEFAULT NULL,
  `sentValue` int(20) DEFAULT NULL,
  `receivedValue` int(20) DEFAULT NULL,
  `conversionRate` double DEFAULT NULL,
  `observations` varchar(100) DEFAULT NULL,
  `transferDate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `saving_account_types`
--

CREATE TABLE `saving_account_types` (
  `typeID` int(10) NOT NULL,
  `typeName` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `saving_account_types`
--

INSERT INTO `saving_account_types` (`typeID`, `typeName`) VALUES
(1, 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'),
(2, 'USER_DEFINED-CUSTOM_SAVING_ACCOUNT');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `userID` int(10) NOT NULL,
  `username` varchar(20) NOT NULL,
  `salt` binary(16) NOT NULL,
  `password` varchar(50) NOT NULL,
  `email` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `users_creditors`
--

CREATE TABLE `users_creditors` (
  `user_ID` int(10) NOT NULL,
  `creditor_ID` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `users_debtors`
--

CREATE TABLE `users_debtors` (
  `user_ID` int(10) NOT NULL,
  `debtor_ID` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `banks`
--
ALTER TABLE `banks`
  ADD PRIMARY KEY (`bankID`);

--
-- Indexes for table `budget_plans`
--
ALTER TABLE `budget_plans`
  ADD PRIMARY KEY (`planID`),
  ADD KEY `user_ID` (`user_ID`),
  ADD KEY `planType` (`planType`);

--
-- Indexes for table `creditors`
--
ALTER TABLE `creditors`
  ADD PRIMARY KEY (`creditorID`);

--
-- Indexes for table `currencies`
--
ALTER TABLE `currencies`
  ADD PRIMARY KEY (`currencyID`),
  ADD UNIQUE KEY `currencyName` (`currencyName`);

--
-- Indexes for table `debtors`
--
ALTER TABLE `debtors`
  ADD PRIMARY KEY (`debtorID`);

--
-- Indexes for table `debts`
--
ALTER TABLE `debts`
  ADD PRIMARY KEY (`debtID`),
  ADD KEY `user_ID` (`user_ID`),
  ADD KEY `creditor_ID` (`creditor_ID`);

--
-- Indexes for table `expenses`
--
ALTER TABLE `expenses`
  ADD PRIMARY KEY (`expenseID`),
  ADD KEY `type` (`type`),
  ADD KEY `user_ID` (`user_ID`);

--
-- Indexes for table `expense_types`
--
ALTER TABLE `expense_types`
  ADD PRIMARY KEY (`categoryID`);

--
-- Indexes for table `incomes`
--
ALTER TABLE `incomes`
  ADD PRIMARY KEY (`incomeID`),
  ADD KEY `user_ID` (`user_ID`),
  ADD KEY `incomeType` (`incomeType`);

--
-- Indexes for table `income_types`
--
ALTER TABLE `income_types`
  ADD PRIMARY KEY (`typeID`);

--
-- Indexes for table `partial_payments`
--
ALTER TABLE `partial_payments`
  ADD PRIMARY KEY (`paymentID`);

--
-- Indexes for table `plan_types`
--
ALTER TABLE `plan_types`
  ADD PRIMARY KEY (`typeID`);

--
-- Indexes for table `receivables`
--
ALTER TABLE `receivables`
  ADD PRIMARY KEY (`receivableID`),
  ADD KEY `debtor_ID` (`debtor_ID`);

--
-- Indexes for table `savings`
--
ALTER TABLE `savings`
  ADD PRIMARY KEY (`savingID`),
  ADD UNIQUE KEY `name` (`name`),
  ADD UNIQUE KEY `date` (`date`),
  ADD KEY `user_ID` (`user_ID`);

--
-- Indexes for table `saving_accounts`
--
ALTER TABLE `saving_accounts`
  ADD PRIMARY KEY (`accountID`),
  ADD UNIQUE KEY `accountName` (`accountName`,`user_ID`),
  ADD KEY `user_ID` (`user_ID`),
  ADD KEY `type_ID` (`type_ID`),
  ADD KEY `bank_ID` (`bank_ID`),
  ADD KEY `currency` (`currency_ID`);

--
-- Indexes for table `saving_accounts_balance`
--
ALTER TABLE `saving_accounts_balance`
  ADD PRIMARY KEY (`recordID`),
  ADD KEY `user_ID` (`user_ID`),
  ADD KEY `account_ID` (`account_ID`);

--
-- Indexes for table `saving_accounts_expenses`
--
ALTER TABLE `saving_accounts_expenses`
  ADD PRIMARY KEY (`expenseID`),
  ADD KEY `type` (`type`),
  ADD KEY `user_ID` (`user_ID`);

--
-- Indexes for table `saving_accounts_transfers`
--
ALTER TABLE `saving_accounts_transfers`
  ADD PRIMARY KEY (`transferID`);

--
-- Indexes for table `saving_account_types`
--
ALTER TABLE `saving_account_types`
  ADD PRIMARY KEY (`typeID`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`userID`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `username_2` (`username`);

--
-- Indexes for table `users_creditors`
--
ALTER TABLE `users_creditors`
  ADD KEY `user_ID` (`user_ID`),
  ADD KEY `creditor_ID` (`creditor_ID`);

--
-- Indexes for table `users_debtors`
--
ALTER TABLE `users_debtors`
  ADD KEY `userID` (`user_ID`),
  ADD KEY `debtorID` (`debtor_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `banks`
--
ALTER TABLE `banks`
  MODIFY `bankID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `budget_plans`
--
ALTER TABLE `budget_plans`
  MODIFY `planID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `creditors`
--
ALTER TABLE `creditors`
  MODIFY `creditorID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `currencies`
--
ALTER TABLE `currencies`
  MODIFY `currencyID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `debtors`
--
ALTER TABLE `debtors`
  MODIFY `debtorID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `debts`
--
ALTER TABLE `debts`
  MODIFY `debtID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `expenses`
--
ALTER TABLE `expenses`
  MODIFY `expenseID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `expense_types`
--
ALTER TABLE `expense_types`
  MODIFY `categoryID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT for table `incomes`
--
ALTER TABLE `incomes`
  MODIFY `incomeID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `income_types`
--
ALTER TABLE `income_types`
  MODIFY `typeID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `partial_payments`
--
ALTER TABLE `partial_payments`
  MODIFY `paymentID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `plan_types`
--
ALTER TABLE `plan_types`
  MODIFY `typeID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `receivables`
--
ALTER TABLE `receivables`
  MODIFY `receivableID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `savings`
--
ALTER TABLE `savings`
  MODIFY `savingID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `saving_accounts`
--
ALTER TABLE `saving_accounts`
  MODIFY `accountID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `saving_accounts_balance`
--
ALTER TABLE `saving_accounts_balance`
  MODIFY `recordID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=57;

--
-- AUTO_INCREMENT for table `saving_accounts_expenses`
--
ALTER TABLE `saving_accounts_expenses`
  MODIFY `expenseID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=45;

--
-- AUTO_INCREMENT for table `saving_accounts_transfers`
--
ALTER TABLE `saving_accounts_transfers`
  MODIFY `transferID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `saving_account_types`
--
ALTER TABLE `saving_account_types`
  MODIFY `typeID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `userID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `budget_plans`
--
ALTER TABLE `budget_plans`
  ADD CONSTRAINT `budget_plans_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  ADD CONSTRAINT `budget_plans_ibfk_2` FOREIGN KEY (`planType`) REFERENCES `plan_types` (`typeID`);

--
-- Constraints for table `debts`
--
ALTER TABLE `debts`
  ADD CONSTRAINT `debts_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  ADD CONSTRAINT `debts_ibfk_2` FOREIGN KEY (`creditor_ID`) REFERENCES `creditors` (`creditorID`);

--
-- Constraints for table `expenses`
--
ALTER TABLE `expenses`
  ADD CONSTRAINT `expenses_ibfk_1` FOREIGN KEY (`type`) REFERENCES `expense_types` (`categoryID`),
  ADD CONSTRAINT `expenses_ibfk_2` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`);

--
-- Constraints for table `incomes`
--
ALTER TABLE `incomes`
  ADD CONSTRAINT `incomes_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  ADD CONSTRAINT `incomes_ibfk_2` FOREIGN KEY (`incomeType`) REFERENCES `income_types` (`typeID`);

--
-- Constraints for table `receivables`
--
ALTER TABLE `receivables`
  ADD CONSTRAINT `receivables_ibfk_1` FOREIGN KEY (`debtor_ID`) REFERENCES `debtors` (`debtorID`) ON UPDATE CASCADE;

--
-- Constraints for table `savings`
--
ALTER TABLE `savings`
  ADD CONSTRAINT `savings_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`);

--
-- Constraints for table `users_creditors`
--
ALTER TABLE `users_creditors`
  ADD CONSTRAINT `users_creditors_ibfk_1` FOREIGN KEY (`user_ID`) REFERENCES `users` (`userID`),
  ADD CONSTRAINT `users_creditors_ibfk_2` FOREIGN KEY (`creditor_ID`) REFERENCES `creditors` (`creditorID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
