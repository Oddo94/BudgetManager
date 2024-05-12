using BudgetManager;
using BudgetManager.utils.enums;
using BudgetManager.utils.exceptions;
using BudgetManagerTests.utils;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace BudgetManagerTests.account_balance {
    /*This class contains all the tests that are designed to check the corectness of the account balance records for system defined accounts using the new balance storage system implemented in the database*/
    [TestClass]
    public class SystemDefinedAccountBalanceStorageSystemTests {
        //Test accounts variables       
        //private static int userDefinedAccountId;

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

        //Test partial payment variables
        private static String partialPaymentName;
        private static int partialPaymentValue;
        private static int newLowerPartialPaymentValue;
        private static int newHigherPartialPaymentValue;

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
        private static TestPartialPaymentUtils testPartialPaymentUtils;

        [ClassInitialize]
        public static void setupTestData(TestContext testContext) {
            //The 'testContext' object used here is only for initializing the class attributes related to the test data
            //In order to have access to the updated object (which changes after each test execution) use the 'TestContext' object defined above as a property

            //userDefinedAccountId = Convert.ToInt32(testContext.Properties["userDefinedAccountId"].ToString());

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

            partialPaymentName = testContext.Properties["partialPaymentName"].ToString();
            partialPaymentValue = Convert.ToInt32(testContext.Properties["partialPaymentValue"].ToString());
            newLowerPartialPaymentValue = Convert.ToInt32(testContext.Properties["newLowerPartialPaymentValue"].ToString());
            newHigherPartialPaymentValue = Convert.ToInt32(testContext.Properties["newHigherPartialPaymentValue"].ToString());

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
            testPartialPaymentUtils = new TestPartialPaymentUtils(partialPaymentName, receivableName, partialPaymentValue);
        }

        [TestMethod]
        public void testBalanceAfterSavingInsertion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);

            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));                
            }

            double expectedBalance = initialBalance + savingValue;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER SAVING INSERTION: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER SAVING INSERTION: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingSavingToLowerValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER SAVING INSERTION: {0}", currentBalanceAfterInsert));

            int updateExecutionResult = testSavingUtils.updateTestSavingFromDb(savingName, newLowerSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test saving {0}", savingName));
            }
     
            int amountDifference = savingValue - newLowerSavingValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));
        
            double expectedBalance = currentBalanceAfterInsert - amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER SAVING UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER SAVING UPDATE: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingSavingToHigherValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER SAVING INSERTION: " + currentBalanceAfterInsert);

            int updateExecutionResult = testSavingUtils.updateTestSavingFromDb(savingName, newHigherSavingValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test saving {0}", savingName));
            }
    
            int amountDifference = newHigherSavingValue - savingValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER SAVING UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER SAVING UPDATE: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterSavingDeletion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testSavingUtils.insertTestSavingIntoDb(userId);
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test saving {0} into the database", savingName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER SAVING INSERTION: " + currentBalanceAfterInsert);

            int deleteExecutionResult = testSavingUtils.deleteTestSavingFromDb(savingName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to delete saving {0} from the database", savingName));
            }

            double expectedBalance = initialBalance;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER SAVING DELETION: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER SAVING DELETION: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterReceivableInsertion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable {0} into the database", receivableName));
            }
         
            double expectedBalance = initialBalance - receivableValue;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER RECEIVABLE INSERTION: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER RECEIVABLE INSERTION: {0}", actualBalance));

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
            Console.WriteLine("CURRENT BALANCE AFTER RECEIVABLE INSERTION: " + currentBalanceAfterInsert);

            int updateExecutionResult = testReceivableUtils.updateTestReceivableFromDb(receivableName, newLowerReceivableValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test receivable {0}", receivableName));
            }
    
            int amountDifference = receivableValue - newLowerReceivableValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER RECEIVABLE UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER RECEIVABLE UPDATE: {0}", actualBalance));

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
            Console.WriteLine("CURRENT BALANCE AFTER RECEIVABLE INSERTION: " + currentBalanceAfterInsert);

            int updateExecutionResult = testReceivableUtils.updateTestReceivableFromDb(receivableName, newHigherReceivableValue);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test receivable {0}", receivableName));
            }
         
            int amountDifference = receivableValue - newHigherReceivableValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER RECEIVABLE UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER RECEIVABLE UPDATE: {0}", actualBalance));

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
            Console.WriteLine("CURRENT BALANCE AFTER RECEIVABLE INSERTION: " + currentBalanceAfterInsert);

            int deleteExecutionResult = testReceivableUtils.deleteTestReceivableFromDb(receivableName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to delete the test receivable {0} from the database", receivableName));
            }

            double expectedBalance = initialBalance;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER RECEIVABLE DELETION: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER RECEIVABLE DELETION: {0}", actualBalance));          

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterPartialPaymentInsertion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int receivableInsertionExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (receivableInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable '{0}' into the database", receivableName));
            }

            double currentBalanceAfterReceivableInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER RECEIVABLE INSERTION: {0}", currentBalanceAfterReceivableInsertion));

            int partialPaymentInsertionExecutionResult = testPartialPaymentUtils.insertTestPartialPaymentIntoDb();
            if(partialPaymentInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test partial payment '{0}' into the database"));
            }

            double expectedBalance = currentBalanceAfterReceivableInsertion + partialPaymentValue;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER PARTIAL PAYMENT INSERTION: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER PARTIAL PAYMENT INSERTION: {0}", actualBalance));
    
            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingPartialPaymentToLowerValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int receivableInsertionExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (receivableInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable '{0}' into the database", receivableName));
            }

            double currentBalanceAfterReceivableInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER RECEIVABLE INSERTION: " + currentBalanceAfterReceivableInsertion);

            int partialPaymentInsertionExecutionResult = testPartialPaymentUtils.insertTestPartialPaymentIntoDb();
            if (partialPaymentInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test partial payment '{0}' into the database"));
            }
        
            double currentBalanceAfterPartialPaymentInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER PARTIAL PAYMENT INSERTION: {0}", currentBalanceAfterPartialPaymentInsertion));

            int partialPaymentUpdateExecutionResult = testPartialPaymentUtils.updateTestPartialPaymentFromDb(newLowerPartialPaymentValue, partialPaymentName);
            if(partialPaymentUpdateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test partial payment '{0}'", partialPaymentName));
            }

            int amountDifference = newLowerPartialPaymentValue - partialPaymentValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));
        
            double expectedBalance = currentBalanceAfterPartialPaymentInsertion + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER PARTIAL PAYMENT UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER PARTIAL PAYMENT UPDATE: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingPartialPaymentToHigherValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int receivableInsertionExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (receivableInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable '{0}' into the database", receivableName));
            }

            double currentBalanceAfterReceivableInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER RECEIVABLE INSERTION: {0}", currentBalanceAfterReceivableInsertion));

            int partialPaymentInsertionExecutionResult = testPartialPaymentUtils.insertTestPartialPaymentIntoDb();
            if (partialPaymentInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test partial payment '{0}' into the database"));
            }

            double currentBalanceAfterPartialPaymentInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER PARTIAL PAYMENT INSERTION: {0}", currentBalanceAfterPartialPaymentInsertion));

            int partialPaymentUpdateExecutionResult = testPartialPaymentUtils.updateTestPartialPaymentFromDb(newHigherPartialPaymentValue, partialPaymentName);
            if (partialPaymentUpdateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test partial payment '{0}'", partialPaymentName));
            }

            int amountDifference = newHigherPartialPaymentValue - partialPaymentValue;
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterPartialPaymentInsertion + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER PARTIAL PAYMENT UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER PARTIAL PAYMENT UPDATE: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterPartialPaymentDeletion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int receivableInsertionExecutionResult = testReceivableUtils.insertTestReceivableIntoDb();
            if (receivableInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test receivable '{0}' into the database", receivableName));
            }

            double currentBalanceAfterReceivableInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER RECEIVABLE INSERTION: {0}", currentBalanceAfterReceivableInsertion));

            int partialPaymentInsertionExecutionResult = testPartialPaymentUtils.insertTestPartialPaymentIntoDb();
            if (partialPaymentInsertionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test partial payment '{0}' into the database"));
            }

            double currentBalanceAfterPartialPaymentInsertion = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER PARTIAL PAYMENT INSERTION: " + currentBalanceAfterPartialPaymentInsertion);

            int partialPaymentDeletionExecutionResult = testPartialPaymentUtils.deleteTestPartialPaymentFromDb(partialPaymentName);
            if (partialPaymentDeletionExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to delete the test partial payment '{0}'", partialPaymentName));
            }

            double expectedBalance = currentBalanceAfterReceivableInsertion;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER PARTIAL PAYMENT DELETION: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER PARTIAL PAYMENT DELETION: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterTransferInsertion() {
            double initialBalanceSenderAccount = getAccountBalanceFromSelect(sourceAccountId);
            Console.WriteLine(String.Format("INITIAL BALANCE FOR SENDING ACCOUNT: {0}", initialBalanceSenderAccount));

            double initialBalanceReceivingAccount = getAccountBalanceFromSelect(destinationAccountId);
            Console.WriteLine(String.Format("INITIAL BALANCE FOR RECEIVING ACCOUNT: {0}", initialBalanceReceivingAccount));

            int insertExecutionResult = testTransferUtils.insertTestTransferIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test transfer {0} into the database", transferName));
            }
          
            //Sender account balance check
            double expectedBalanceSenderAccount = initialBalanceSenderAccount - sentValue;
            Console.WriteLine(String.Format("EXPECTED BALANCE FOR SENDER ACCOUNT AFTER TRANSFER INSERTION: {0}", expectedBalanceSenderAccount));

            double actualBalanceSenderAccount = getAccountBalanceFromSelect(sourceAccountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE FOR SENDER ACCOUNT AFTER TRANSFER INSERTION: {0}", actualBalanceSenderAccount));

            //Receiving account balance check
            double expectedBalanceReceivingAccount = initialBalanceReceivingAccount + sentValue;
            Console.WriteLine(String.Format("EXPECTED BALANCE FOR RECEIVING ACCOUNT AFTER TRANSFER INSERTION: {0}", expectedBalanceReceivingAccount));

            double actualBalanceReceivingAccount = getAccountBalanceFromSelect(destinationAccountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE FOR SENDER ACCOUNT AFTER TRANSFER INSERTION: {0}", actualBalanceReceivingAccount));

            Assert.AreEqual(expectedBalanceSenderAccount, actualBalanceSenderAccount);

            Assert.AreEqual(expectedBalanceReceivingAccount, actualBalanceReceivingAccount);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingTransferToLowerValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testTransferUtils.insertTestTransferIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test transfer {0}", transferName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine("CURRENT BALANCE AFTER TRANSFER INSERTION: " + currentBalanceAfterInsert);

            int updateExecutionResult = testTransferUtils.updateTestTransferFromDb(transferName, newLowerSentValue, newLowerReceivedValue, exchangeRate);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test transfer {0}", transferName));
            }

            double amountDifference = Math.Round(sentValue - newLowerSentValue, 2);
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER TRANSFER UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER TRANSFER UPDATE: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterUpdatingTransferToHigherValue() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testTransferUtils.insertTestTransferIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test transfer {0}", transferName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER TRANSFER INSERTION: {0}" + currentBalanceAfterInsert));

            int updateExecutionResult = testTransferUtils.updateTestTransferFromDb(transferName, newHigherSentValue, newHigherReceivedValue, exchangeRate);
            if (updateExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to update the test transfer {0}", transferName));
            }

            double amountDifference = Math.Round(sentValue - newHigherSentValue, 2);
            Console.WriteLine(String.Format("AMOUNT DIFFERENCE: {0}", amountDifference));

            double expectedBalance = currentBalanceAfterInsert + amountDifference;
            Console.WriteLine(String.Format("EXPECTED BALANCE AFTER TRANSFER UPDATE: {0}", expectedBalance));

            double actualBalance = getAccountBalanceFromSelect(userId);
            Console.WriteLine(String.Format("ACTUAL BALANCE AFTER TRANSFER UPDATE: {0}", actualBalance));

            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void testBalanceAfterTransferDeletion() {
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE: {0}", initialBalance));

            int insertExecutionResult = testTransferUtils.insertTestTransferIntoDb();
            if (insertExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to insert the test transfer {0} into the database", transferName));
            }

            double currentBalanceAfterInsert = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER TRANSFER INSERTION: {0}", currentBalanceAfterInsert));

            int deleteExecutionResult = testTransferUtils.deleteTestTransferFromDb(transferName);
            if (deleteExecutionResult == -1) {
                Assert.Fail(String.Format("Unable to delete the test transfer {0} from the database", transferName));
            }

            double actualBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("CURRENT BALANCE AFTER TRANSFER DELETION: {0}", actualBalance));

            double expectedBalance = initialBalance;

            Assert.AreEqual(expectedBalance, actualBalance);
        }


        [TestCleanup]
        public void performTestCleanup() {
            //Retrieves the current test name which will be used to decide the correct cleanup method to be executed
            String testName = TestContext.TestName;

            if (testName.Contains("Saving")) {
                removeTestSavingFromDb();
            } else if (testName.Contains("Receivable") || testName.Contains("PartialPayment")) {
                removeTestReceivableFromDb();
            } else if (testName.Contains("Transfer")) {
                removeTestTransferFromDb();
            }
        }

        public void removeTestSavingFromDb() {
            Console.WriteLine("\n======RemoveInsertedSavingFromDb======");
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE BEFORE DELETION: {0}", initialBalance));


            int executionResult = testSavingUtils.deleteTestSavingFromDb(savingName);

            if (executionResult == -1) {
                Console.WriteLine(String.Format("Unable to delete the test saving {0}", savingName));
            }

            double finalBalance = getAccountBalanceFromSelect(accountId); ;
            Console.WriteLine(String.Format("FINAL BALANCE AFTER DELETION: {0}", finalBalance));
        }

        public void removeTestReceivableFromDb() {
            Console.WriteLine("\n======RemoveTestReceivableFromDb======");
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE BEFORE DELETION: {0}", initialBalance));

            int executionResult = testReceivableUtils.deleteTestReceivableFromDb(receivableName);

            if (executionResult == -1) {
                Console.WriteLine(String.Format("Unable to delete the test receivable {0}", receivableName));
            }

            double finalBalance = getAccountBalanceFromSelect(accountId); ;
            Console.WriteLine(String.Format("FINAL BALANCE AFTER DELETION: {0}", finalBalance));

        }

        public void removeTestTransferFromDb() {
            Console.WriteLine("\n======RemoveTestTransferFromDb======");
            double initialBalance = getAccountBalanceFromSelect(accountId);
            Console.WriteLine(String.Format("INITIAL BALANCE BEFORE DELETION: {0}", initialBalance));

            int executionResult = testTransferUtils.deleteTestTransferFromDb(transferName);

            if (executionResult == -1) {
                Console.WriteLine(String.Format("Unable to delete the test transfer {0}", transferName));
            }

            double finalBalance = getAccountBalanceFromSelect(accountId); ;
            Console.WriteLine(String.Format("FINAL BALANCE AFTER DELETION: {0}", finalBalance));
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
