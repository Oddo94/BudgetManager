using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    class ExpenseInsertionStrategy : DataInsertionStrategy {

        //SQL statements for inserting the two different types of expenses(general incomes expenses(having the salary as income source) and saving account expenses(having the saving account as income source))
        private String sqlStatementInsertGeneralIncomesExpense = @"INSERT INTO expenses(user_ID, name, type, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";
        private String sqlStatementInsertSavingAccountExpense = @"INSERT INTO saving_account_expenses(user_ID, name, type, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";

        //SQL statement for retrieving the type id of the expense based on its type name
        private String sqlStatementSelectExpenseTypeID = @"SELECT categoryID FROM expense_types WHERE categoryName = @paramTypeName";


        public int execute(QueryData paramContainer) {
            //Data retrieval from the container object
            IncomeSource incomeSource = paramContainer.IncomeSource;
            int userID = paramContainer.UserID;
            String expenseName = paramContainer.ItemName;            
            int expenseTypeID = DataInsertionUtils.getID(sqlStatementSelectExpenseTypeID, paramContainer.TypeName);
            int expenseValue = paramContainer.ItemValue;
            String expenseDate = paramContainer.ItemCreationDate;

            int executionResult = -1;
            MySqlCommand expenseInsertionCommand = null;
            if (incomeSource == IncomeSource.GENERAL_INCOMES) {
                expenseInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementInsertGeneralIncomesExpense, userID, expenseName, expenseTypeID, expenseValue, expenseDate);
                executionResult = DBConnectionManager.insertData(expenseInsertionCommand); ;//Uses the SQL statement that inserts the expense in the expenses table of the DB
            } else if (incomeSource == IncomeSource.SAVING_ACCOUNT) {
                expenseInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementInsertSavingAccountExpense, userID, expenseName, expenseTypeID, expenseValue, expenseDate);
                executionResult = DBConnectionManager.insertData(expenseInsertionCommand);//Uses SQL statement that inserts the expense in the saving_account_expenses table of the DB
            }


            return executionResult;
           
        }
    }
}
