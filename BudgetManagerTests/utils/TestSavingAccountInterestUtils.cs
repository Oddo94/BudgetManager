using BudgetManager;
using MySql.Data.MySqlClient;

namespace BudgetManagerTests.utils {
    internal class TestSavingAccountInterestUtils {
        private int interestAccountId;
        private String interestName;
        private int interestType;
        private int interestPaymentType;
        private double interestRate;
        private double interestValue;
        private String interestTransactionId;
        private String interestCreationDate;

        public TestSavingAccountInterestUtils(int interestAccountId, String interestName, int interestType, int interestPaymentType, double interestRate, double interestValue, String interestTransactionId, String interestCreationDate) {
            this.interestAccountId = interestAccountId;
            this.interestName = interestName;
            this.interestType = interestType;
            this.interestPaymentType = interestPaymentType;
            this.interestRate = interestRate;
            this.interestValue = interestValue;
            this.interestTransactionId = interestTransactionId;
            this.interestCreationDate = interestCreationDate;
        }

        private String sqlStatementInsertTestSavingAccountInterest = @"INSERT INTO saving_accounts_interest(account_ID, interestName, interestType, paymentType, interestRate, value, transactionID, creationDate) 
                                                                       VALUES (@paramAccountId, @paramInterestName, @paramInterestType, @paramInterestPaymentType, @paramInterestRate, @paramInterestValue, @paramTransactionID, @paramCreationDate)";
        private String sqlStatementUpdateTestSavingAccountInterest = @"UPDATE saving_accounts_interest SET value = @paramInterestValue WHERE interestName = @paramInterestName";
        private String sqlStatementDeleteTestSavingAccountInterest = @"DELETE FROM saving_accounts_interest WHERE interestName = @paramInterestName";


        public int insertTestSavingAccountInterestIntoDb() {
            MySqlCommand savingAccountInterestInsertionCommand = new MySqlCommand(sqlStatementInsertTestSavingAccountInterest);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramAccountId", interestAccountId);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramInterestName", interestName);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramInterestType", interestType);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramInterestPaymentType", interestPaymentType);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramInterestRate", interestRate);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramInterestValue", interestValue);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramTransactionID", interestTransactionId);
            savingAccountInterestInsertionCommand.Parameters.AddWithValue("@paramCreationDate", interestCreationDate);

            int executionResult = DBConnectionManager.insertData(savingAccountInterestInsertionCommand);

            return executionResult;
        }

        public int updateTestSavingAccountInterestFromDb(String interestName, double newInterestValue) {
            MySqlCommand savingAccountInterestUpdateCommand = new MySqlCommand(sqlStatementUpdateTestSavingAccountInterest);
            savingAccountInterestUpdateCommand.Parameters.AddWithValue("@paramInterestValue", newInterestValue);
            savingAccountInterestUpdateCommand.Parameters.AddWithValue("@paramInterestName", interestName);

            int executionResult = DBConnectionManager.updateData(savingAccountInterestUpdateCommand);

            return executionResult;
        }

        public int deleteSavingAccountInterestFromDb(String interestName) {
            MySqlCommand savingAccountInterestDeleteCommand = new MySqlCommand(sqlStatementDeleteTestSavingAccountInterest);
            savingAccountInterestDeleteCommand.Parameters.AddWithValue("@paramInterestName", interestName);

            int executionResult = DBConnectionManager.deleteData(savingAccountInterestDeleteCommand);

            return executionResult;
        }
    }
}
