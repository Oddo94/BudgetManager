using BudgetManager.non_mvc;
using BudgetManager;
using MySql.Data.MySqlClient;

namespace BudgetManagerTests.utils {
    internal class TestSavingUtils {
        private String savingName;
        private int savingValue;
        private String savingDate;


        private String sqlStatementDeleteInsertedSaving = "DELETE FROM savings WHERE name = @paramName";
        private String sqlStatementUpdateInsertedSaving = "UPDATE savings SET value = @paramValue WHERE name = @paramName";

        public TestSavingUtils(string savingName, int savingValue, string savingDate) {
            this.savingName = savingName;
            this.savingValue = savingValue;
            this.savingDate = savingDate;
        }

        public int insertTestSavingIntoDb(int userId) {
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

        public int updateTestSavingFromDb(string savingName, int newSavingValue) {
            MySqlCommand sqlCommandUpdateSaving = new MySqlCommand(sqlStatementUpdateInsertedSaving);
            sqlCommandUpdateSaving.Parameters.AddWithValue("@paramValue", newSavingValue);
            sqlCommandUpdateSaving.Parameters.AddWithValue("@paramName", savingName);

            int executionResult = DBConnectionManager.updateData(sqlCommandUpdateSaving);

            return executionResult;
        }

        public int deleteTestSavingFromDb(string savingName) {
            MySqlCommand sqlCommandDeleteSaving = new MySqlCommand(sqlStatementDeleteInsertedSaving);
            sqlCommandDeleteSaving.Parameters.AddWithValue("@paramName", savingName);

            int executionResult = DBConnectionManager.deleteData(sqlCommandDeleteSaving);

            return executionResult;
        }
    }
}
