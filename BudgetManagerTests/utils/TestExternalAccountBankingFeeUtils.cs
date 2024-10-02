using BudgetManager;
using BudgetManager.mvc.models.dto;
using BudgetManager.non_mvc;
using BudgetManager.utils.data_insertion;
using MySql.Data.MySqlClient;



namespace BudgetManagerTests.utils {
    internal class TestExternalAccountBankingFeeUtils {
        private String bankingFeeName;
        private  String bankingFeeAccountName;
        private double bankingFeeValue;
        private String bankingFeeDescription;
        private String bankingFeeCreationDate;
        private int userId;

        private String sqlStatementUpdateTestExternalAccountBankingFee = "UPDATE external_accounts_banking_fees SET value = @paramValue WHERE name = @paramName";
        private String sqlStatementDeleteTestExternalAccountBankingFee = "DELETE FROM external_accounts_banking_fees WHERE name = @paramName";

        public TestExternalAccountBankingFeeUtils(String bankingFeeName, String bankingFeeAccountName, double bankingFeeValue, String bankingFeeDescription, String bankingFeeCreationDate, int userId) {
            this.bankingFeeName = bankingFeeName;
            this.bankingFeeAccountName = bankingFeeAccountName;
            this.bankingFeeValue = bankingFeeValue;
            this.bankingFeeDescription = bankingFeeDescription;
            this.bankingFeeCreationDate = bankingFeeCreationDate;
            this.userId = userId;
        }

        public int insertTestExternalAccountBankingFeeIntoDb() {
            BankingFeeDTO bankingFeeDTO = new BankingFeeDTO();
            bankingFeeDTO.AccountName = bankingFeeAccountName;
            bankingFeeDTO.Name = bankingFeeName;
            bankingFeeDTO.Value = bankingFeeValue;
            bankingFeeDTO.Description = bankingFeeDescription;
            bankingFeeDTO.CreatedDate = bankingFeeCreationDate;
            bankingFeeDTO.UserID = userId;

            DataInsertionStrategy bankingFeeInsertionStrategy = new ExternalAccountBankingFeeInsertionStrategy();
            DataInsertionContext dataInsertionContext = new DataInsertionContext();
            dataInsertionContext.setStrategy(bankingFeeInsertionStrategy);

            int executionResult = dataInsertionContext.invoke(bankingFeeDTO);

            return executionResult;
        }

        public int updateTestExternalAccountBankingFeeFromDb(String bankingFeeName, double newBankingFeeValue) {
            MySqlCommand updateTestExternalAccountBankingFee = new MySqlCommand(sqlStatementUpdateTestExternalAccountBankingFee);
            updateTestExternalAccountBankingFee.Parameters.AddWithValue("@paramName", bankingFeeName);
            updateTestExternalAccountBankingFee.Parameters.AddWithValue("@paramValue", newBankingFeeValue);

            int executionResult = DBConnectionManager.updateData(updateTestExternalAccountBankingFee);

            return executionResult;
        }

        public int deleteTestExternalAccountBankingFeeFromDb(String bankingFeeName) {
            MySqlCommand deleteTestExternalAccountBankingFee = new MySqlCommand(sqlStatementDeleteTestExternalAccountBankingFee);
            deleteTestExternalAccountBankingFee.Parameters.AddWithValue("@paramName", bankingFeeName);

            int executionResult = DBConnectionManager.deleteData(deleteTestExternalAccountBankingFee);

            return executionResult;
        }
    }
}
