-- phpMyAdmin SQL Dump
-- version 5.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 30, 2021 at 10:59 AM
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
(1, 'Fixed expense'),
(2, 'Periodic expense'),
(3, 'Variable expense');

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
-- Table structure for table `savings`
--

CREATE TABLE `savings` (
  `savingID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `saving_account_balance`
--

CREATE TABLE `saving_account_balance` (
  `recordID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `recordName` varchar(50) NOT NULL,
  `value` int(10) NOT NULL,
  `month` int(2) NOT NULL,
  `year` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `saving_account_expenses`
--

CREATE TABLE `saving_account_expenses` (
  `expenseID` int(10) NOT NULL,
  `user_ID` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `value` int(20) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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

--
-- Indexes for dumped tables
--

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
-- Indexes for table `plan_types`
--
ALTER TABLE `plan_types`
  ADD PRIMARY KEY (`typeID`);

--
-- Indexes for table `savings`
--
ALTER TABLE `savings`
  ADD PRIMARY KEY (`savingID`),
  ADD UNIQUE KEY `name` (`name`),
  ADD UNIQUE KEY `date` (`date`),
  ADD KEY `user_ID` (`user_ID`);

--
-- Indexes for table `saving_account_balance`
--
ALTER TABLE `saving_account_balance`
  ADD PRIMARY KEY (`recordID`),
  ADD KEY `user_ID` (`user_ID`);

--
-- Indexes for table `saving_account_expenses`
--
ALTER TABLE `saving_account_expenses`
  ADD PRIMARY KEY (`expenseID`),
  ADD KEY `type` (`type`),
  ADD KEY `user_ID` (`user_ID`);

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
-- AUTO_INCREMENT for dumped tables
--

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
  MODIFY `categoryID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
-- AUTO_INCREMENT for table `plan_types`
--
ALTER TABLE `plan_types`
  MODIFY `typeID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `savings`
--
ALTER TABLE `savings`
  MODIFY `savingID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `saving_account_balance`
--
ALTER TABLE `saving_account_balance`
  MODIFY `recordID` int(10) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `saving_account_expenses`
--
ALTER TABLE `saving_account_expenses`
  MODIFY `expenseID` int(10) NOT NULL AUTO_INCREMENT;

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
