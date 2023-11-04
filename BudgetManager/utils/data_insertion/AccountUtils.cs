using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils.data_insertion {
    //Class that provides utility methods for retrieving data about the user accounts
    internal class AccountUtils {
        //SQL query used for retrieving the account ID for which the balance check is performed
        private String sqlStatementGetAccountID = @"SELECT accountID FROM saving_accounts WHERE accountName LIKE CONCAT('%', @paramRecordName,'%') AND user_ID = @paramID";

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
    }
}
