﻿using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {   
    //Utilty class used for creating SQL commands(single/multiple months)
    class SQLCommandBuilder {
       
        //General purpose method for creating single month SQL commands
        public static MySqlCommand getSingleMonthCommand(String sqlStatement, QueryData paramContainer) {
            //Argument null checks
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");
  
            MySqlCommand singleMonthCommand = new MySqlCommand(sqlStatement, DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING));//Nu e neaparata adaugarea conexiunii la crearea comenzii intrucat metoda getData a clasei DBConnectionmanager face deja asta in mod implicit
            singleMonthCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            singleMonthCommand.Parameters.AddWithValue("@paramMonth", paramContainer.Month);
            singleMonthCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            return singleMonthCommand;
        }

        //Method for selecting all the year records for a given user
        public static MySqlCommand getFullYearRecordsCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand fullYearRecordsCommand = new MySqlCommand(sqlStatement);
            fullYearRecordsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            fullYearRecordsCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            return fullYearRecordsCommand;

        }

        //General purpose method for creating multiple months SQL command
        public static MySqlCommand getMultipleMonthsCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand multipleMonthsCommand = new MySqlCommand(sqlStatement, DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING));//Nu e neaparata adaugarea conexiunii la crearea comenzii intrucat metoda getData a clasei DBConnectionmanager face deja asta in mod implicit
            multipleMonthsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            multipleMonthsCommand.Parameters.AddWithValue("@paramStartDate", paramContainer.StartDate);
            multipleMonthsCommand.Parameters.AddWithValue("@paramEndDate", paramContainer.EndDate);           

            return multipleMonthsCommand;
        }
     
        //Method for creating the SQL statement that retrieves the data displayed by the yearly column chart
        public static MySqlCommand getMonthlyTotalsCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand monthlyTotalsCommand = new MySqlCommand(sqlStatement, DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING));//Nu e neaparata adaugarea conexiunii la crearea comenzii intrucat metoda getData a clasei DBConnectionmanager face deja asta in mod implicit
            monthlyTotalsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            monthlyTotalsCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            return monthlyTotalsCommand;
        }
      
        //Method for creating the SQL command for inserting a new user in the DB
        public static MySqlCommand getNewUserCreationCommand(String sqlStatement, String userName, byte[] salt, String hashCode, String emailAddress) {
            //Null checks for all the provided object arguments 
            performNullChecksForMultipleArguments("Object", sqlStatement, userName, hashCode, emailAddress);

            MySqlCommand userCreationCommand = new MySqlCommand(sqlStatement);
            userCreationCommand.Parameters.AddWithValue("@paramUserName", userName);
            userCreationCommand.Parameters.AddWithValue("@paramSalt", salt);
            userCreationCommand.Parameters.AddWithValue("@paramHashCode", hashCode);
            userCreationCommand.Parameters.AddWithValue("@paramEmailAddress", emailAddress);

            return userCreationCommand;
        }

        //Method for creating the SQL command used to update the user password
        public static MySqlCommand getUpdatePasswordCommand(String sqlStatement, byte[] salt, String hashCode, int userID) {
            performNullChecksForMultipleArguments("Object", sqlStatement, salt, hashCode);

            MySqlCommand updatePasswordCommand = new MySqlCommand(sqlStatement);
            updatePasswordCommand.Parameters.AddWithValue("@paramSalt", salt);
            updatePasswordCommand.Parameters.AddWithValue("@paramHashCode", hashCode);
            updatePasswordCommand.Parameters.AddWithValue("@paramID", userID);

            return updatePasswordCommand;
        }
       
        //Method for creating the SQL statement for inserting elements that have different sub-types(incomes, expenses, etc)
        public static MySqlCommand getInsertCommandForMultipleTypeItem(String sqlStatement, int userID, String itemName, int typeID, int itemValue, String itemDate) {
            performNullChecksForMultipleArguments("Object", sqlStatement, itemName, itemDate);

            MySqlCommand insertMultipleTypeItemCommand = new MySqlCommand(sqlStatement);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramID", userID );
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramItemName", itemName);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramTypeID", typeID);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramItemValue", itemValue);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramItemDate", itemDate);

            return insertMultipleTypeItemCommand;

        }
        
        //Method for creating the SQL command used to insert data into the debts table of the DB
        public static MySqlCommand getDebtInsertionCommand(String sqlStatement, int userID, String debtName, int debtValue, int creditorID, String debtDate) {
            performNullChecksForMultipleArguments("Object", sqlStatement, debtName, debtDate);

            MySqlCommand debtInsertionCommand = new MySqlCommand(sqlStatement);
            debtInsertionCommand.Parameters.AddWithValue("@paramID", userID);
            debtInsertionCommand.Parameters.AddWithValue("@paramDebtName", debtName);
            debtInsertionCommand.Parameters.AddWithValue("@paramDebtValue", debtValue);
            debtInsertionCommand.Parameters.AddWithValue("@paramCreditorID", creditorID);
            debtInsertionCommand.Parameters.AddWithValue("@paramDebtDate", debtDate);

            return debtInsertionCommand;
        }

        //Method for getting the command necessary for inserting savings in the DB table
        public static MySqlCommand getSavingInsertionCommand(String sqlStatement, int userID, String savingName, int savingValue, String savingDate) {
            performNullChecksForMultipleArguments("Object", sqlStatement, savingName, savingDate);

            MySqlCommand savingInsertionCommand = new MySqlCommand(sqlStatement);
            savingInsertionCommand.Parameters.AddWithValue("@paramID", userID);
            savingInsertionCommand.Parameters.AddWithValue("@paramSavingName", savingName);
            savingInsertionCommand.Parameters.AddWithValue("@paramSavingValue", savingValue);
            savingInsertionCommand.Parameters.AddWithValue("@paramSavingDate", savingDate);

            return savingInsertionCommand;
        }

        //Method for creating a command that will insert or update a record in the balance record table
        public static MySqlCommand getBalanceRecordInsertUpdateCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand balanceRecordInsertionCommand = new MySqlCommand(sqlStatement);
            balanceRecordInsertionCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            balanceRecordInsertionCommand.Parameters.AddWithValue("@paramRecordName",paramContainer.ItemName);
            balanceRecordInsertionCommand.Parameters.AddWithValue("@paramValue", paramContainer.ItemValue);
            balanceRecordInsertionCommand.Parameters.AddWithValue("paramMonth", paramContainer.Month);
            balanceRecordInsertionCommand.Parameters.AddWithValue("paramYear", paramContainer.Year);

            return balanceRecordInsertionCommand;
        }

        //Method for creating a command that can be used to check the sum of a table values for a user(can be used to check current balance or other limits on item insertion into the DB)
        public static MySqlCommand getRecordSumValueCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand recordSumValueCommand = new MySqlCommand(sqlStatement);
            recordSumValueCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);

            return recordSumValueCommand;
        }

        //Method for getting the command that helps check if there is an existing budget plan for the specified time interval
        public static MySqlCommand getBudgetPlanCheckCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand budgetPlanCheckCommand = new MySqlCommand(sqlStatement);
            budgetPlanCheckCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            budgetPlanCheckCommand.Parameters.AddWithValue("@paramDate", paramContainer.StartDate);

            return budgetPlanCheckCommand;
        }

        //Method for retrieving the ID of a specified element type from the DB (e.g. budget plan)
        public static MySqlCommand getTypeIDForItemCommand(String sqlStatement, String typeName) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(typeName, "Type name");

            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", typeName);

            return getTypeIDCommand;
        }

        //SHOULD replace the previous ID retrieval method and be more generic
        //Method for retrieving the ID of a specified element type from the DB (e.g. budget plan)
        public static MySqlCommand getRecordIDCommand(String sqlStatement, String recordName) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(recordName, "record name");

            MySqlCommand getRecordIDCommand = new MySqlCommand(sqlStatement);
            getRecordIDCommand.Parameters.AddWithValue("@paramRecordName", recordName);

            return getRecordIDCommand;
        }

        //Method for getting the command used to insert a new budget plan into the DB
        public static MySqlCommand getBudgetPlanCreationCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand budgetPlanCreationCommand = new MySqlCommand(sqlStatement);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramPlanName", paramContainer.BudgetPlanName);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramExpenseLimit", paramContainer.ExpenseLimit);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramDebtLimit", paramContainer.DebtLimit);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramSavingLimit", paramContainer.SavingLimit);            
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramPlanTypeID", paramContainer.PlanTypeID);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramAlarmExistence", paramContainer.AlarmExistenceValue);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramThresholdPercentage", paramContainer.ThresholdPercentage);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramStartDate", paramContainer.StartDate);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramEndDate", paramContainer.EndDate);

            return budgetPlanCreationCommand;
        }

        //Method for creating commands with a single parameter representing the user ID
        public static MySqlCommand getSpecificUserRecordsCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(sqlStatement, "SQL statement");
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand getSpecificUserRecordsCommand = new MySqlCommand(sqlStatement);
            getSpecificUserRecordsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);

            return getSpecificUserRecordsCommand;
        }

        //Method for checking multiple arguments for null values(uses varargs to provide more flexibility regarding the number of passed objects to be processed)
        private static void performNullChecksForMultipleArguments(String argumentName, params Object[] arguments) {                

            foreach (Object currentArgument in arguments ) {
                
                Guard.notNull(currentArgument, argumentName);
            }
        }

        public static MySqlCommand getReceivableInsertionCommand(String sqlStatement, QueryData paramContainer) {
            Guard.notNull(paramContainer, "parameter container");

            MySqlCommand insertReceivableCommand = new MySqlCommand(sqlStatement);
            insertReceivableCommand.Parameters.AddWithValue("@paramItemName", paramContainer.ItemName);
            insertReceivableCommand.Parameters.AddWithValue("@paramItemValue", paramContainer.ItemValue);
            insertReceivableCommand.Parameters.AddWithValue("@paramDebtorID", paramContainer.DebtorID);
            insertReceivableCommand.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
            insertReceivableCommand.Parameters.AddWithValue("@paramPaidAmount", paramContainer.PaidAmount);
            insertReceivableCommand.Parameters.AddWithValue("@paramStartDate", paramContainer.StartDate);
            insertReceivableCommand.Parameters.AddWithValue("@paramEndDate", paramContainer.EndDate);

            return insertReceivableCommand;
        }
    }
}
