
using BudgetManager;
using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;

namespace BudgetManagerTests {
    [TestClass]
    public class AccountBalanceTests {
        [TestMethod]
        public void testCurrentAccountBalance() {
            double expectedBalance = 24724;

            /*BudgetManager.utils.enums.AccountType accountType = BudgetManager.utils.enums.AccountType.SOURCE_ACCOUNT;*/

            List<MySqlParameter> inputParams = new List<MySqlParameter>();
            MySqlParameter accountId = new MySqlParameter("p_account_ID", MySqlDbType.Int32, 8);
            inputParams.Add(accountId);

            List<MySqlParameter> outputParams = new List<MySqlParameter>();
            MySqlParameter accountBalance = new MySqlParameter("p_account_balance", MySqlDbType.Double);
            outputParams.Add(accountBalance);

            List<MySqlParameter> procedureExecutionResults = DBConnectionManager.callDatabaseStoredProcedure("get_saving_account_balance", inputParams, outputParams);
            double actualBalance;

             if (procedureExecutionResults != null && procedureExecutionResults.Count > 0) {
                MySqlParameter accountBalanceParam = procedureExecutionResults.ElementAt(0);

                if (accountBalanceParam == null) {
                    throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
                }

                actualBalance = Convert.ToDouble(accountBalanceParam.Value);

            } else {
                throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
            }

            Assert.AreEqual(expectedBalance, actualBalance);
        }
    }
}