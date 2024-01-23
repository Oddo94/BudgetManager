
using BudgetManager;
using BudgetManager.non_mvc;
using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace BudgetManagerTests {
    [TestClass]
    public class AccountBalanceTests {

        public static TestContext testContextWork;

        private static int accountId;
        private static int userId;
        private static String savingName;
        private static int savingValue;
        private static int newLowerSavingValue;
        private static int newHigherSavingValue;
        private static String savingDate;

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


        [ClassInitialize]
        public static void setupTestData(TestContext testContext) {
            testContextWork = testContext;

            accountId = Convert.ToInt32(testContextWork.Properties["accountId"].ToString());
            userId = Convert.ToInt32(testContextWork.Properties["userId"].ToString());
            savingName = testContextWork.Properties["savingName"].ToString();
            savingValue = Convert.ToInt32(testContextWork.Properties["savingValue"].ToString());
            newLowerSavingValue = Convert.ToInt32(testContextWork.Properties["newLowerSavingValue"].ToString());
            newHigherSavingValue = Convert.ToInt32(testContextWork.Properties["newHigherSavingValue"].ToString());
            savingDate = testContextWork.Properties["savingDate"].ToString();
        }

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
            if (insertExecutionResult == -1) {
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

        [TestMethod]
        public void testBalanceAfterUpdatingSavingToHigherValue() {
            Console.WriteLine("======TestBalanceAfterUpdatingSavingToHigherValue======");

            int intialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE: " + intialBalance);

            int insertExecutionResult = insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            int currentBalanceAfterInsert = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = updateTestSavingFromDb(savingName, newHigherSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test saving {0}", savingName));
            }

            int actualBalanceAfterUpdate = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER UPDATE: " + actualBalanceAfterUpdate);

            int amountDifference = newHigherSavingValue - savingValue;
            Console.WriteLine("AMOUNT DIFFERENCE: " + amountDifference);

            int expectedBalanceAfterUpdate = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine("EXPECTED BALANCE AFTER UPDATE: " + expectedBalanceAfterUpdate);

            Assert.AreEqual(expectedBalanceAfterUpdate, actualBalanceAfterUpdate);
        }

        [TestMethod]
        public void testBalanceAfterSavingDeletion() {
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            int insertExecutionResult = insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            int currentBalanceAfterInsert = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int deleteExecutionResult = deleteTestSavingFromDb(savingName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to delete saving {0} from the database", savingName));
            }

            int expectedBalance = initialBalance;
            int actualBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER DELETE: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }


        [TestCleanup]
        public void removeInsertedSavingFromDb() {
            Console.WriteLine("\n======RemoveInsertedSavingFromDb======");
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);


            int executionResult = deleteTestSavingFromDb(savingName);

            if (executionResult == -1) {
                Console.WriteLine(String.Format("Unable to delete the test saving {0}", savingName));
            }

            int finalBalance = getAccountBalanceFromSelect(userId); ;
            Console.WriteLine("FINAL BALANCE AFTER DELETION: " + finalBalance);
        }

        //[TestCleanup]
        //public void removeInsertedSavingFromDb() {
        //    Console.WriteLine("======RemoveInsertedSavingFromDb======");
        //    int initialBalance = getAccountBalanceFromSelect(userId);
        //    Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);

        //    MySqlCommand sqlCommandDeleteSaving = new MySqlCommand(sqlStatementDeleteInsertedSaving);
        //    sqlCommandDeleteSaving.Parameters.AddWithValue("@paramName", savingName);

        //    int executionResult = DBConnectionManager.deleteData(sqlCommandDeleteSaving);

        //    if (executionResult == -1) {
        //        Console.WriteLine(String.Format("Unable to delete the test saving {0}", savingName));
        //    }

        //    int finalBalance = initialBalance - savingValue;
        //    Console.WriteLine("FINAL BALANCE AFTER DELETION: " + finalBalance);
        //}


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

        private int deleteTestSavingFromDb(String savingName) {
            MySqlCommand sqlCommandDeleteSaving = new MySqlCommand(sqlStatementDeleteInsertedSaving);
            sqlCommandDeleteSaving.Parameters.AddWithValue("@paramName", savingName);

            int executionResult = DBConnectionManager.deleteData(sqlCommandDeleteSaving);

            return executionResult;
        }
    }
}