using BudgetManager;
using BudgetManager.non_mvc;
using BudgetManager.utils.enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManagerTests.utils {
    internal class TestSavingAccountExpenseUtils {
        private int userId;
        private String savingAccountExpenseName;
        private String savingAccountExpenseTypeName;
        private int savingAccountExpenseValue;
        private String savingAccountExpenseCreationDate;

        private String sqlStatementUpdateTestSavingAccountExpense = "UPDATE saving_accounts_expenses SET value = @paramValue WHERE name = @paramName";
        private String sqlStatementDeleteTestSavingAccountExpense = "DELETE FROM saving_accounts_expenses WHERE name = @paramName";


        public TestSavingAccountExpenseUtils(int userId, string savingAccountExpenseName, string savingAccountExpenseTypeName, int savingAccountExpenseValue, string savingAccountExpenseCreationDate) {
            this.userId = userId;
            this.savingAccountExpenseName = savingAccountExpenseName;
            this.savingAccountExpenseTypeName = savingAccountExpenseTypeName;
            this.savingAccountExpenseValue = savingAccountExpenseValue;
            this.savingAccountExpenseCreationDate = savingAccountExpenseCreationDate;
        }

        public int insertTestSavingAccountExpenseIntoDb() {
            QueryData paramContainer = new QueryData.Builder(userId)              
                .addItemName(savingAccountExpenseName)
                .addTypeName(savingAccountExpenseTypeName)
                .addItemValue(savingAccountExpenseValue)
                .addItemCreationDate(savingAccountExpenseCreationDate)
                .addIncomeSource(IncomeSource.SAVING_ACCOUNT)
                .build();

            DataInsertionStrategy expenseInsertionStrategy = new ExpenseInsertionStrategy();
            DataInsertionContext expenseInsertionContext = new DataInsertionContext();
            expenseInsertionContext.setStrategy(expenseInsertionStrategy);

            int executionResult = expenseInsertionContext.invoke(paramContainer);

            return executionResult;
        }

        public int updateTestSavingAccountExpenseFromDb(String savingAccountExpenseName, int newSavingAccountExpenseValue) {
            MySqlCommand sqlCommandUpdateSavingAccountExpense = new MySqlCommand(sqlStatementUpdateTestSavingAccountExpense);
            sqlCommandUpdateSavingAccountExpense.Parameters.AddWithValue("@paramName", savingAccountExpenseName);
            sqlCommandUpdateSavingAccountExpense.Parameters.AddWithValue("@paramValue", newSavingAccountExpenseValue);

            int executionResult = DBConnectionManager.updateData(sqlCommandUpdateSavingAccountExpense);

            return executionResult;
        }

        public int deleteTestSavingAccountExpenseFromDb(String savingAccountExpenseName) {
            MySqlCommand sqlCommandDeleteTestSavingAccountExpense = new MySqlCommand(sqlStatementDeleteTestSavingAccountExpense);
            sqlCommandDeleteTestSavingAccountExpense.Parameters.AddWithValue("@paramName", savingAccountExpenseName);

            int executionResult = DBConnectionManager.deleteData(sqlCommandDeleteTestSavingAccountExpense);

            return executionResult;
        }

    }
}
