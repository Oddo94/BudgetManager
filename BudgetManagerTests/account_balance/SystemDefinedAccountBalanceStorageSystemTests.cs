using BudgetManager;
using BudgetManager.utils.enums;
using BudgetManager.utils.exceptions;
using BudgetManagerTests.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManagerTests.account_balance {
    /*This class contains all the tests that are designed to check the corectness of the account balance records for system defined accounts using the new balance storage system implemented in the database*/
    [TestClass]
    public class SystemDefinedAccountBalanceStorageSystemTests {
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
        private static int newLowerReceivableValue;
        private static int newHigherReceivableValue;
        private static String debtorName;
        private static String sourceAccountName;
        private static int totalPaidAmount;
        private static ReceivableStatus receivableStatus;
        private static String createdDate;
        private static String dueDate;

        //Test transfer variables
        private static int sourceAccountId;
        private static int destinationAccountId;
        private static String transferName;
        private static double sentValue;
        private static double newLowerSentValue;
        private static double newHigherSentValue;
        private static double receivedValue;
        private static double newLowerReceivedValue;
        private static double newHigherReceivedValue;
        private static double exchangeRate;
        private static String transactionId;
        private static String transferObservations;
        private static String transferDate;

        private string sqlStatementGetSavingAccountCurrentBalance = @"SELECT currentBalance FROM account_balance_storage WHERE account_ID = @paramId";


        //Declaring the TestContext variable like this will ensure that it will be updated after each test so that the current test name can be retrieved correctly
        public TestContext TestContext { get; set; }
        private static TestSavingUtils testSavingUtils;
        private static TestReceivableUtils testReceivableUtils;
        private static TestTransferUtils testTransferUtils;

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
            newLowerReceivableValue = Convert.ToInt32(testContext.Properties["newLowerReceivableValue"].ToString());
            newHigherReceivableValue = Convert.ToInt32(testContext.Properties["newHigherReceivableValue"].ToString());
            debtorName = testContext.Properties["debtorName"].ToString();
            sourceAccountName = testContext.Properties["sourceAccountName"].ToString();
            totalPaidAmount = Convert.ToInt32(testContext.Properties["totalPaidAmount"].ToString());
            receivableStatus = ReceivableStatusExtension.getReceivableStatusByDescription(testContext.Properties["receivableStatus"].ToString());
            createdDate = testContext.Properties["createdDate"].ToString();
            dueDate = testContext.Properties["dueDate"].ToString();

            sourceAccountId = Convert.ToInt32(testContext.Properties["sourceAccountId"].ToString());
            destinationAccountId = Convert.ToInt32(testContext.Properties["destinationAccountId"].ToString());
            transferName = testContext.Properties["transferName"].ToString();
            sentValue = Convert.ToDouble(testContext.Properties["sentValue"].ToString());
            newLowerSentValue = Convert.ToDouble(testContext.Properties["newLowerSentValue"].ToString());
            newHigherSentValue = Convert.ToDouble(testContext.Properties["newHigherSentValue"].ToString());
            receivedValue = Convert.ToDouble(testContext.Properties["receivedValue"].ToString());
            newLowerReceivedValue = Convert.ToDouble(testContext.Properties["newLowerReceivedValue"].ToString());
            newHigherReceivedValue = Convert.ToDouble(testContext.Properties["newHigherReceivedValue"].ToString());
            exchangeRate = Convert.ToDouble(testContext.Properties["exchangeRate"].ToString());
            transactionId = testContext.Properties["transactionId"].ToString();
            transferObservations = testContext.Properties["transferObservations"].ToString();
            transferDate = testContext.Properties["transferDate"].ToString();

            testSavingUtils = new TestSavingUtils(savingName, savingValue, savingDate);
            testReceivableUtils = new TestReceivableUtils(receivableName, receivableValue, totalPaidAmount, debtorName, sourceAccountName, receivableStatus, createdDate, dueDate, userId);
            testTransferUtils = new TestTransferUtils(sourceAccountId, destinationAccountId, transferName, sentValue, receivedValue, exchangeRate, transactionId, transferObservations, transferDate, userId);
        }

        [TestMethod]
        public void testBalanceAfterSavingInsertion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            double expectedBalance = initialBalance + savingValue;
            Console.WriteLine("EXPECTED BALANCE: " + expectedBalance);

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);

            if (insertExecutionResult == -1) {
                Assert.Fail("Unable to insert the test saving");
                return;
            }

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("ACTUAL BALANCE AFTER SAVING INSERTION: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingSavingToLowerValue() {
            double intialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE: " + intialBalance);

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = testSavingUtils.updateTestSavingFromDb(savingName, newLowerSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to update the test saving {0}", savingName));
            }

            double actualBalanceAfterUpdate = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER UPDATE: " + actualBalanceAfterUpdate);

            int amountDifference = savingValue - newLowerSavingValue;
            Console.WriteLine("AMOUNT DIFFERENCE: " + amountDifference);

            double expectedBalanceAfterUpdate = currentBalanceAfterInsert - amountDifference;
            Console.WriteLine("EXPECTED BALANCE AFTER UPDATE: " + expectedBalanceAfterUpdate);

            Assert.AreEqual(expectedBalanceAfterUpdate, actualBalanceAfterUpdate);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingSavingToHigherValue() {
            double intialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE: " + intialBalance);

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = testSavingUtils.updateTestSavingFromDb(savingName, newHigherSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to update the test saving {0}", savingName));
            }

            double actualBalanceAfterUpdate = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER UPDATE: " + actualBalanceAfterUpdate);

            int amountDifference = newHigherSavingValue - savingValue;
            Console.WriteLine("AMOUNT DIFFERENCE: " + amountDifference);

            double expectedBalanceAfterUpdate = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine("EXPECTED BALANCE AFTER UPDATE: " + expectedBalanceAfterUpdate);

            Assert.AreEqual(expectedBalanceAfterUpdate, actualBalanceAfterUpdate);
        }

        [TestMethod]
        public void testBalanceAfterSavingDeletion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int deleteExecutionResult = testSavingUtils.deleteTestSavingFromDb(savingName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(string.Format("Unable to delete saving {0} from the database", savingName));
            }

            double expectedBalance = initialBalance;
            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER DELETE: " + actualBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterReceivableInsertion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE: " + initialBalance);

            int insertExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable {0} into the database", receivableName));
            }

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("ACTUAL BALANCE AFTER INSERT: " + actualBalance);

            double expectedBalance = initialBalance - receivableValue;
            Console.WriteLine("EXPECTED BALANCE AFTER INSERT: " + expectedBalance);

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingReceivableToLowerValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable {0}", receivableName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = testReceivableUtils.updateTestReceivableFromDb(receivableName, newLowerReceivableValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test receivable {0}", receivableName));
            }

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER UPDATE: {0}", actualBalance));

            int amountDifference = receivableValue - newLowerReceivableValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER UPDATE: {0}", expectedBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingReceivableToHigherValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable {0}", receivableName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int updateExecutionResult = testReceivableUtils.updateTestReceivableFromDb(receivableName, newHigherReceivableValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test receivable {0}", receivableName));
            }

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER UPDATE: {0}", actualBalance));

            int amountDifference = receivableValue - newHigherReceivableValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER UPDATE: {0}", expectedBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterReceivableDeletion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable {0} into the database", receivableName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER INSERT: " + currentBalanceAfterInsert);

            int deleteExecutionResult = testReceivableUtils.deleteTestReceivableFromDb(receivableName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to delete the test receivable {0} from the database", receivableName));
            }

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER DELETE: " + actualBalance);

            double expectedBalance = initialBalance;

            Assert.AreEqual(expectedBalance, actualBalance);
        }


        [TestCleanup]
        public void performTestCleanup() {
            //Retrieves the current test name which will be used to decide the correct cleanup method to be executed
            String testName = TestContext.TestName;

            if (testName.Contains("Saving")) {
                removeTestSavingFromDb();
            } else if (testName.Contains("Receivable")) {
                removeTestReceivableFromDb();
            } else if (testName.Contains("Transfer")) {
                //removeTestTransferFromDb();
            }
        }

        public void removeTestSavingFromDb() {
            Console.WriteLine("\n======RemoveInsertedSavingFromDb======");
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);


            int executionResult = testSavingUtils.deleteTestSavingFromDb(savingName);

            if (executionResult == -1) {
                Console.WriteLine(string.Format("Unable to delete the test saving {0}", savingName));
            }

            double finalBalance = getAccountBalanceFromSelect(accountId); ;
            Console.WriteLine("FINAL BALANCE AFTER DELETION: " + finalBalance);
        }

        public void removeTestReceivableFromDb() {
            Console.WriteLine("\n======RemoveTestReceivableFromDb======");
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("INITIAL BALANCE BEFORE DELETION: " + initialBalance);

            int executionResult = testReceivableUtils.deleteTestReceivableFromDb(receivableName);

            if (executionResult == -1) {
                Console.WriteLine(string.Format("Unable to delete the test receivable {0}", receivableName));
            }

            double finalBalance = getAccountBalanceFromSelect(accountId); ;
            Console.WriteLine("FINAL BALANCE AFTER DELETION: " + finalBalance);

        }

        private double getAccountBalanceFromSelect(int accountId) {
            MySqlCommand sqlCommandGetAccountBalance = new MySqlCommand(sqlStatementGetSavingAccountCurrentBalance);
            sqlCommandGetAccountBalance.Parameters.AddWithValue("@paramId", accountId);

            DataTable resultDataTable = DBConnectionManager.getData(sqlCommandGetAccountBalance);

            double accountBalance = -1;
            bool parseResult;
            if (resultDataTable != null && resultDataTable.Rows.Count > 0) {
                parseResult = Double.TryParse(resultDataTable.Rows[0].ItemArray[0].ToString(), out accountBalance);

            } else {
                throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
            }

            return Math.Round(accountBalance, 2);
        }

    }
}
