using BudgetManager.utils.exceptions;
using BudgetManager.utils.enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BudgetManager.utils.data_insertion {
    //Class that provides utility methods for retrieving data about the user accounts
    internal class AccountUtils {
        //SQL query used for retrieving the account ID for which the balance check is performed
        private String sqlStatementGetAccountID = @"SELECT accountID FROM saving_accounts WHERE accountName LIKE CONCAT('%', @paramRecordName,'%') AND user_ID = @paramID";

        private String sqlStatementGetAccountIdForStorageRecordCreation = @"SELECT sa.accountID
                                                                            FROM saving_accounts sa
                                                                            INNER JOIN users usr ON sa.user_ID = usr.userID
                                                                            INNER JOIN saving_account_types sat ON sa.type_ID = sat.typeID
                                                                            WHERE (usr.username = @paramUsername OR usr.userID = @paramID) AND sat.typeName = @paramTypeName AND sa.accountName = @paramAccountName";
        private String sqlStatementInsertAccountBalanceStorageRecord = @"INSERT INTO account_balance_storage(account_ID, currentBalance, createdDate) VALUES(@paramAccountId, 0, CURRENT_TIMESTAMP())";

        public double getSavingAccountCurrentBalance(String accountName, int userID) {
            double currentBalance = 0;
            int accountID = -1;

            try {
                accountID = getAccountID(accountName, userID);
            } catch (NoDataFoundException ex) {
                Console.WriteLine(ex.Message);
                return -1;
            }

            MySqlParameter accountIDParam = new MySqlParameter("p_account_ID", accountID);
            MySqlParameter accountBalanceParam = new MySqlParameter("p_account_balance", MySqlDbType.Double);

            List<MySqlParameter> inputParameters = new List<MySqlParameter>() { accountIDParam };
            List<MySqlParameter> outputParameters = new List<MySqlParameter>() { accountBalanceParam };

            List<MySqlParameter> procedureExecutionResults = DBConnectionManager.callDatabaseStoredProcedure("get_saving_account_balance", inputParameters, outputParameters);

            if (procedureExecutionResults != null && procedureExecutionResults.Count > 0) {
                accountBalanceParam = procedureExecutionResults.ElementAt(0);

                if (accountBalanceParam == null) {
                    throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
                }

                currentBalance = Convert.ToDouble(accountBalanceParam.Value);

            } else {
                throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
            }

            return currentBalance;
        }


        public int getAccountID(String accountName, int userID) {
            //Setting the default value for current balance.If data cannot be retrieved for any reason then 0 will be returned since it is not be allowed for the saving account to have negative balance
            double currentBalance = 0;
            int accountID = -1;
            DataTable accountIDDataTable = null;

            QueryData paramContainerAccountRetrieval = new QueryData.Builder(userID).addItemName(accountName).build();

            //Retrieves the account ID
            MySqlCommand accountIDRetrievalCommand = SQLCommandBuilder.getRecordIDCommand(sqlStatementGetAccountID, paramContainerAccountRetrieval);
            accountIDDataTable = DBConnectionManager.getData(accountIDRetrievalCommand);

            if (accountIDDataTable != null && accountIDDataTable.Rows.Count > 0) {
                Object result = accountIDDataTable.Rows[0].ItemArray[0];

                if (result == DBNull.Value) {
                    throw new NoDataFoundException("Unable to retrieve the ID of the saving account whose balance needs to be checked!");
                }

                accountID = Convert.ToInt32(result);

            } else {
                throw new NoDataFoundException("Unable to retrieve the ID of the saving account whose balance needs to be checked!");
            }

            return accountID;
        }

        /*Method used to create the account balance storage record for a specified account. 
          It uses the username/user ID for retrieving the account ID of that respective account*/
        public int createAccountBalanceStorageRecordForAccount(String userName, int? userId,  AccountType accountType, String accountName) {
            String accountTypeName = EnumExtensions.getEnumDescription(accountType);

            MySqlCommand getAccountIdForStorageRecordCreationCommand = new MySqlCommand(sqlStatementGetAccountIdForStorageRecordCreation);
            getAccountIdForStorageRecordCreationCommand.Parameters.AddWithValue("@paramUsername", userName);
            getAccountIdForStorageRecordCreationCommand.Parameters.AddWithValue("@paramID", userId);
            getAccountIdForStorageRecordCreationCommand.Parameters.AddWithValue("@paramTypeName", accountTypeName);
            getAccountIdForStorageRecordCreationCommand.Parameters.AddWithValue("@paramAccountName", accountName);

            DataTable accountIdDataTable = DBConnectionManager.getData(getAccountIdForStorageRecordCreationCommand);

            int accountID = -1;
            if (accountIdDataTable != null && accountIdDataTable.Rows.Count > 0) {
                Object result = accountIdDataTable.Rows[0].ItemArray[0];

                if (result == DBNull.Value) {
                    return -1;
                }

                accountID = Convert.ToInt32(result);

            } else {
                return -1;
            }


            MySqlCommand insertAccountBalanceStorageRecordCommand = new MySqlCommand(sqlStatementInsertAccountBalanceStorageRecord);
            insertAccountBalanceStorageRecordCommand.Parameters.AddWithValue("@paramAccountId", accountID);

            int executionResult = DBConnectionManager.insertData(insertAccountBalanceStorageRecordCommand); 

            if(executionResult > 0) {
                return executionResult;
            }

            return -1;
        }
    }
}
