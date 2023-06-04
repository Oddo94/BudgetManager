using BudgetManager.mvp.models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.repositories {
    internal class ExternalAccountStatisticsRepository : IExternalAccountStatisticsRepository {
        private String sqlStatementGetUserAccounts = "SELECT accountName FROM saving_accounts WHERE user_ID = @paramID ORDER BY accountName";
        private String sqlStatementGetAccountId = "SELECT accountID FROM saving_accounts WHERE user_ID = @paramID AND accountName = @paramAccountName";

        public DataTable getUserAccounts(int userId) {
            QueryData paramContainer = new QueryData.Builder(userId).build();
            MySqlCommand getUserAccountsCommand = SQLCommandBuilder.getSpecificUserRecordsCommand(sqlStatementGetUserAccounts, paramContainer);

            DataTable userAccountsDT = DBConnectionManager.getData(getUserAccountsCommand);

            return userAccountsDT;
        }

        public ExternalAccountDetailsModel getAccountDetails(String selectedAccountName, int userId) {
            String procedureName = "get_account_statistics";
            MySqlParameter accountId = new MySqlParameter("p_account_ID", MySqlDbType.Int32, 20);
            MySqlParameter accountType = new MySqlParameter("p_account_type", MySqlDbType.VarChar, 50);
            MySqlParameter accountName = new MySqlParameter("p_account_name", MySqlDbType.VarChar, 50);
            MySqlParameter bankName = new MySqlParameter("p_bank_name", MySqlDbType.VarChar, 50);
            MySqlParameter accountCreationDate = new MySqlParameter("p_account_creation_date", MySqlDbType.VarChar, 10);
            MySqlParameter accountBalance = new MySqlParameter("p_account_balance", MySqlDbType.Double);
            MySqlParameter totalInTransfers = new MySqlParameter("p_total_in_transfers", MySqlDbType.Double);
            MySqlParameter totalOutTransfers = new MySqlParameter("p_total_out_transfers", MySqlDbType.Double);
            MySqlParameter totalInterestAmount = new MySqlParameter("p_total_interest_amount", MySqlDbType.Double);
            MySqlParameter totalSavingAccountExpenses = new MySqlParameter("p_total_saving_account_expenses", MySqlDbType.Double);
            MySqlParameter totalSavings = new MySqlParameter("p_total_savings", MySqlDbType.Double);
            MySqlParameter totalUnpaidReceivableAmount = new MySqlParameter("p_total_unpaid_receivable_amount", MySqlDbType.Double);

            byte decimalPrecision = 10;
            byte decimalScale = 2;

            int retrievedAccountId = getAccountId(selectedAccountName, userId);
            accountId.Value = retrievedAccountId;

            setPrecisionAndScaleForDecimalParams(decimalPrecision, decimalScale, accountBalance, totalInTransfers, totalOutTransfers, totalInterestAmount, totalSavingAccountExpenses, totalSavings, totalUnpaidReceivableAmount);


            List<MySqlParameter> inputParameters = new List<MySqlParameter>() { accountId };
            List<MySqlParameter> outputParameters = new List<MySqlParameter>() { accountType, accountName, bankName, accountCreationDate, accountBalance, totalInTransfers, totalOutTransfers, totalInterestAmount, totalSavingAccountExpenses, totalSavings, totalUnpaidReceivableAmount };


            List<MySqlParameter> populatedOutputParameters = DBConnectionManager.callDatabaseStoredProcedure(procedureName, inputParameters, outputParameters);
            ExternalAccountDetailsModel externalAccountDetails = new ExternalAccountDetailsModel();

            foreach (MySqlParameter currentParam in populatedOutputParameters) {
                switch (currentParam.ParameterName) {
                    case "p_account_name":
                        externalAccountDetails.AccountName = currentParam.Value.ToString();
                        break;

                    case "p_bank_name":
                        externalAccountDetails.BankName = currentParam.Value.ToString();
                        break;

                    case "p_account_creation_date":
                        externalAccountDetails.CreationDate = currentParam.Value.ToString();
                        break;

                    case "p_account_balance":
                        externalAccountDetails.AccountBalance = Convert.ToDouble(currentParam.Value.ToString());
                        break;

                    case "p_total_in_transfers":
                        externalAccountDetails.TotalInTransfers = Convert.ToDouble(currentParam.Value.ToString());
                        break;

                    case "p_total_out_transfers":
                        externalAccountDetails.TotalOutTransfers = Convert.ToDouble(currentParam.Value.ToString());
                        break;

                    case "p_total_interest_amount":
                        externalAccountDetails.TotalInterestAmount = Convert.ToDouble(currentParam.Value.ToString());
                        break;

                    default:
                        break;
                }
            }

            return externalAccountDetails;
        }

        public ExternalAccountDetailsModel getAccountDetailsById(int accountId) {
            throw new NotImplementedException();
        }

        //UTILITY METHODS
        private int getAccountId(String accountName, int userId) {
            MySqlCommand accountIdRetrievalCommand = new MySqlCommand(sqlStatementGetAccountId);
            accountIdRetrievalCommand.Parameters.AddWithValue("@paramID", userId);
            accountIdRetrievalCommand.Parameters.AddWithValue("@paramAccountName", accountName);

            DataTable accountIdDT = DBConnectionManager.getData(accountIdRetrievalCommand);

            int accountId = -1;
            if (accountIdDT.Rows.Count > 0) {
                bool canParse = Int32.TryParse(accountIdDT.Rows[0].ItemArray[0].ToString(), out accountId);
            }

            return accountId;

        }

        private void setPrecisionAndScaleForDecimalParams(byte precision, byte scale, params MySqlParameter[] decimalParamList) {
            foreach (MySqlParameter currentParam in decimalParamList) {
                currentParam.Precision = precision;
                currentParam.Scale = scale;
            }
        }
    }
}
