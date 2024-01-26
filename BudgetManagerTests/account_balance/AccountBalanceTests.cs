
using BudgetManager;
using BudgetManager.utils.enums;
using BudgetManager.utils.exceptions;
using BudgetManagerTests.utils;
using MySql.Data.MySqlClient;
using System.Data;

namespace BudgetManagerTests.account_balance {
    [TestClass]
    public class AccountBalanceTests {
        //Test saving variables
        private static int accountId;
        private static int userId;
        private static String savingName;
        private static int savingValue;
        private static int newLowerSavingValue;
        private static int newHigherSavingValue;
        private static String savingDate;

        //Test receivable variables
        private static String receivableName;
        private static int receivableValue;
        private static String debtorName;
        private static String sourceAccountName;
        private static int totalPaidAmount;
        private static ReceivableStatus receivableStatus;
        private static String createdDate;
        private static String dueDate;

        private string sqlStatementSavingAccountCurrentBalance = @"SELECT SUM(value) FROM
                (SELECT sab.value, sab.account_ID, sat.typeID, sab.month, sab.year FROM saving_accounts_balance sab
                  INNER JOIN saving_accounts sa on sab.account_ID = sa.accountID
                  INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                  WHERE sab.user_ID = @paramID
                  AND sat.typeID = 1
                  AND year <= year(CURDATE())) AS subquery
                WHERE (subquery.month <= MONTH(CURDATE()) AND subquery.year <= YEAR(CURDATE())) OR (subquery.month > MONTH(CURDATE()) AND subquery.year < YEAR(CURDATE()))";

        //Declaring the TestContext variable like this will ensure that it will be updated after each test so that the current test name can be retrieved correctly
        public TestContext TestContext { get; set; }
        private static TestSavingUtils testSavingUtils;
        private static TestReceivableUtils testReceivableUtils;

        [ClassInitialize]
        public static void setupTestData(TestContext testContext) {
            //The 'testContext' object used here is only for initializing the class attributes related to the test data
            //In order to have access to the updated object (which changes after each test execution) use the 'TestContext' object defined above as a property

            accountId = Convert.ToInt32(testContext.Properties["accountId"].ToString());
            userId = Convert.ToInt32(testContext.Properties["userId"].ToString());
            savingName = testContext.Properties["savingName"].ToString();
            savingValue = Convert.ToInt32(testContext.Properties["savingValue"].ToString());
            newLowerSavingValue = Convert.ToInt32(testContext.Properties["newLowerSavingValue"].ToString());
            newHigherSavingValue = Convert.ToInt32(testContext.Properties["newHigherSavingValue"].ToString());
            savingDate = testContext.Properties["savingDate"].ToString();

            receivableName = testContext.Properties["receivableName"].ToString();
            receivableValue = Convert.ToInt32(testContext.Properties["receivableValue"].ToString());
            debtorName = testContext.Properties["debtorName"].ToString();
            sourceAccountName = testContext.Properties["sourceAccountName"].ToString();
            totalPaidAmount = Convert.ToInt32(testContext.Properties["totalPaidAmount"].ToString());
            receivableStatus = ReceivableStatusExtension.getReceivableStatusByDescription(testContext.Properties["receivableStatus"].ToString());
            createdDate = testContext.Properties["createdDate"].ToString();
            dueDate = testContext.Properties["dueDate"].ToString();

            testSavingUtils = new TestSavingUtils(savingName, savingValue, savingDate);
            testReceivableUtils = new TestReceivableUtils(receivableName, receivableValue, totalPaidAmount, debtorName, sourceAccountName, receivableStatus, createdDate, dueDate, userId);
        }

        [TestMethod]
        public void testBalanceAfterSavingInsertion2() {
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            int expectedBalance = initialBalance + savingValue;
            Console.WriteLine("EXPECTED BALANCE: " + expectedBalance);

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);

            if (insertExecutionResult == -1) {
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

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            int currentBalanceAfterInsert = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = testSavingUtils.updateTestSavingFromDb(savingName, newLowerSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to update the test saving {0}", savingName));
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

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            int currentBalanceAfterInsert = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = testSavingUtils.updateTestSavingFromDb(savingName, newHigherSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to update the test saving {0}", savingName));
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

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            int currentBalanceAfterInsert = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int deleteExecutionResult = testSavingUtils.deleteTestSavingFromDb(savingName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to delete saving {0} from the database", savingName));
            }

            int expectedBalance = initialBalance;
            int actualBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("CURRENT BALANCE AFTER DELETE: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterReceivableInsertion() {
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            int insertExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if(insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable {0} into the database", receivableName));
            }

            int actualBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("ACTUAL BALANCE AFTER INSERT: " + actualBalance);

            int expectedBalance = initialBalance - receivableValue;
            Console.WriteLine("EXPECTED BALANCE AFTER INSERT: " + expectedBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestCleanup]
        public void performTestCleanup() {
            //Retrieves the current test name which will be used to decide the correct cleanup method to be executed
            String testName = TestContext.TestName;
            Console.WriteLine("CURRENT TEST NAME: " + testName);

            if(testName.Contains("Saving")) {
                removeTestSavingFromDb();
            } else if (testName.Contains("Receivable")) {
                removeTestReceivableFromDb();
            }
        }

        public void removeTestSavingFromDb() {
            Console.WriteLine("\n======RemoveInsertedSavingFromDb======");
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);


            int executionResult = testSavingUtils.deleteTestSavingFromDb(savingName);

            if (executionResult == -1) {
                Console.WriteLine(string.Format("Unable to delete the test saving {0}", savingName));
            }

            int finalBalance = getAccountBalanceFromSelect(userId); ;
            Console.WriteLine("FINAL BALANCE AFTER DELETION: " + finalBalance);
        }

        public void removeTestReceivableFromDb() {
            Console.WriteLine("\n======RemoveTestReceivableFromDb======");
            int initialBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);

            int executionResult = testReceivableUtils.deleteTestReceivableFromDb();

            if (executionResult == -1) {
                Console.WriteLine(string.Format("Unable to delete the test receivable {0}", receivableName));
            }

            int finalBalance = getAccountBalanceFromSelect(userId); ;
            Console.WriteLine("FINAL BALANCE AFTER DELETION: " + finalBalance);

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
                parseResult = int.TryParse(resultDataTable.Rows[0].ItemArray[0].ToString(), out accountBalance);

            } else {
                throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
            }

            return accountBalance;
        }
    }
}