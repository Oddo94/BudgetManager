
using BudgetManager;
using BudgetManager.non_mvc;
using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace BudgetManagerTests {
    [TestClass]
    public class AccountBalanceTests {
        private static readonly int accountId = 8;
        private static readonly int userId = 3;
        private static readonly String savingName = "AUTOMATED_TEST_SAVING-1";
        private static readonly int savingValue = 200;
        private static readonly int newLowerSavingValue = 100;
        private static readonly int newHigherSavingValue = 300;
        private static readonly String savingDate = "2024-01-18";

        private String sqlStatementSavingAccountCurrentBalance = @"SELECT SUM(value) FROM
                (SELECT sab.value, sab.account_ID, sat.typeID, sab.month, sab.year FROM saving_accounts_balance sab
                  INNER JOIN saving_accounts sa on sab.account_ID = sa.accountID
                  INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                  WHERE sab.user_ID = @paramID
                  AND sat.typeID = 1
                  AND year <= year(CURDATE())) AS subquery
                WHERE (subquery.month <= MONTH(CURDATE()) AND subquery.year <= YEAR(CURDATE())) OR (subquery.month > MONTH(CURDATE()) AND subquery.year < YEAR(CURDATE()))";

        private String sqlStatementDeleteInsertedSaving = "DELETE FROM savings WHERE name = @paramName";
        private String sqlStatementUpdateInsertedSaving = "UPDATE savings SET value = @paramValue WHERE name = @paramName";

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

        //[TestMethod]
        public void testBalanceAfterSavingInsertion1() {
            double initialBalance = getAccountBalanceFromProcedure(accountId);
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

            double actualBalance = getAccountBalanceFromProcedure(accountId);
            Console.WriteLine("ACTUAL BALANCE AFTER SAVING INSERTION: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);

        }

        [TestMethod]
        public void testBalanceAfterSavingInsertion2() {
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            int expectedBalance = initialBalance + savingValue;
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

            int actualBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("ACTUAL BALANCE AFTER SAVING INSERTION: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingSavingToLowerValue() {
            Console.WriteLine("======TestBalanceAfterUpdatingSavingToLowerValue======");

            int intialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE: " + intialBalance);
            
            int insertExecutionResult = insertTestSavingIntoDb(userId);
            if(insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            int currentBalanceAfterInsert = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = updateTestSavingFromDb(savingName, newLowerSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test saving {0}", savingName));
            }

            int actualBalanceAfterUpdate = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER UPDATE: " + actualBalanceAfterUpdate);

            int amountDifference = savingValue - newLowerSavingValue;
            Console.WriteLine("AMOUNT DIFFERENCE: " + amountDifference);

            int expectedBalanceAfterUpdate = currentBalanceAfterInsert - amountDifference;
            Console.WriteLine("EXPECTED BALANCE AFTER UPDATE: " + expectedBalanceAfterUpdate);

            Assert.AreEqual(expectedBalanceAfterUpdate, actualBalanceAfterUpdate);
        }

        [TestCleanup]
        public void removeInsertedSavingFromDb() {
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);

            int expectedBalance = initialBalance - savingValue;
            Console.WriteLine("EXPECTED BALANCE AFTER DELETION: " + expectedBalance);

            MySqlCommand sqlCommandDeleteSaving = new MySqlCommand(sqlStatementDeleteInsertedSaving);
            sqlCommandDeleteSaving.Parameters.AddWithValue("@paramName", savingName);

            int executionResult = DBConnectionManager.deleteData(sqlCommandDeleteSaving);

            if (executionResult == -1) {
                Console.WriteLine(String.Format("Unable to delete the test saving {0}", savingName));
            }
        }

        private double getAccountBalanceFromProcedure(int testAccountId) {
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

        private int getAccountBalanceFromSelect(int userId) {
            MySqlCommand sqlCommandGetAccountBalance = new MySqlCommand(sqlStatementSavingAccountCurrentBalance);
            sqlCommandGetAccountBalance.Parameters.AddWithValue("@paramID", userId);

            DataTable resultDataTable = DBConnectionManager.getData(sqlCommandGetAccountBalance);

            int accountBalance = -1;
            bool parseResult;
            if (resultDataTable != null && resultDataTable.Rows.Count > 0) {
               parseResult = Int32.TryParse(resultDataTable.Rows[0].ItemArray[0].ToString(), out accountBalance);

            } else {
                throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
            }

            return accountBalance;
        }

        private int insertTestSavingIntoDb(int userId) {
            QueryData paramContainer = new QueryData.Builder(userId)
                       .addItemName(savingName)
                       .addItemValue(savingValue)
                       .addItemCreationDate(savingDate)
                       .build();

            DataInsertionContext datainsertionContext = new DataInsertionContext();
            SavingInsertionStrategy savingInsertionStrategy = new SavingInsertionStrategy();
            datainsertionContext.setStrategy(savingInsertionStrategy);

            int executionResult = datainsertionContext.invoke(paramContainer);

            return executionResult;
        }

        private int updateTestSavingFromDb(String savingName, int newSavingValue) {
            MySqlCommand sqlCommandUpdateSaving = new MySqlCommand(sqlStatementUpdateInsertedSaving);
            sqlCommandUpdateSaving.Parameters.AddWithValue("@paramValue", newSavingValue);
            sqlCommandUpdateSaving.Parameters.AddWithValue("@paramName", savingName);

            int executionResult = DBConnectionManager.updateData(sqlCommandUpdateSaving);

            return executionResult;
        }
    }
}