using BudgetManager.mvp.models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.repositories
{
    internal class ExternalAccountStatisticsRepository : IExternalAccountStatisticsRepository {
        private String sqlStatementGetUserAccounts = "SELECT accountName FROM saving_accounts WHERE user_ID = @paramID ORDER BY accountName";

        public DataTable getUserAccounts(int userId) {
            QueryData paramContainer = new QueryData.Builder(userId).build();
            MySqlCommand getUserAccountsCommand = SQLCommandBuilder.getSpecificUserRecordsCommand(sqlStatementGetUserAccounts, paramContainer);

            DataTable userAccountsDT = DBConnectionManager.getData(getUserAccountsCommand);

            return userAccountsDT;
        }
        public ExternalAccountDetailsModel getAccountDetails(string accountName, int userId) {
            throw new NotImplementedException();
        }

        public ExternalAccountDetailsModel getAccountDetailsById(int accountId) {
            throw new NotImplementedException();
        }
    }
}
