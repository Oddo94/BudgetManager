
using BudgetManager;
using BudgetManager.non_mvc;
using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;

namespace BudgetManagerTests {
    [TestClass]
    public class AccountBalanceTests {
        private static readonly int accountId = 8;
        private static readonly int userId = 3;
        private static readonly String savingName = "AUTOMATED_TEST_SAVING-3";
        private static readonly int savingValue = 200;
        private static readonly String savingDate = "2024-01-14";


        //[TestMethod]
        public void testCurrentAccountBalance() {
            double expectedBalance = 24724;
            int accountIdValue = 8;

            /*BudgetManager.utils.enums.AccountType accountType = BudgetManager.utils.enums.AccountType.SOURCE_ACCOUNT;*/

            MySqlParameter accountId = new MySqlParameter("p_account_ID", accountIdValue);
            MySqlParameter accountBalance = new MySqlParameter("p_account_balance", MySqlDbType.Double);

            List<MySqlParameter> inputParams = new List<MySqlParameter>() { accountId };
            List<MySqlParameter> outputParams = new List<MySqlParameter>() { accountBalance };


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

        [TestMethod]
        public void testBalanceAfterSavingInsertion() {
            double initialBalance = getAccountBalance(accountId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            double expectedBalance = initialBalance + savingValue;
            Console.WriteLine("EXPECTED BALANCE: " + expectedBalance);

            QueryData paramContainer = new QueryData.Builder(userId)
                        .addItemName(savingName)
                        .addItemValue(savingValue)
                        .addItemCreationDate(savingDate)
                        .build();

            DataInsertionContext datainsertionContext = new DataInsertionContext();
            SavingInsertionStrategy savingInsertionStrategy = new SavingInsertionStrategy();
            datainsertionContext.setStrategy(savingInsertionStrategy);

            int executionResult = datainsertionContext.invoke(paramContainer);

            if (executionResult == -1) {
                Assert.Fail("Unable to insert the test saving");
                return;
            }

            double actualBalance = getAccountBalance(accountId);
            Console.WriteLine("ACTUAL BALANCE AFTER SAVING INSERTION: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);

        }

        private double getAccountBalance(int testAccountId) {
            MySqlParameter accountId = new MySqlParameter("p_account_ID", testAccountId);
            MySqlParameter accountBalance = new MySqlParameter("p_account_balance", MySqlDbType.Double);

            List<MySqlParameter> inputParams = new List<MySqlParameter>() { accountId };
            List<MySqlParameter> outputParams = new List<MySqlParameter>() { accountBalance };


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

            return actualBalance;
        }
    }
}