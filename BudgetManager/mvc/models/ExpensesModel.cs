using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class ExpensesModel : IModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];

        //SQL queries for populating the table
        //Selects the list of expenses for a single month
        private String sqlStatementSingleMonthExpenses = @"SELECT expenses.name AS 'Expense name', (SELECT categoryName FROM expense_types WHERE expense_types.categoryID = expenses.type) AS 'Expense type', expenses.value AS 'Expense value', expenses.date AS 'Date'
                FROM expenses INNER JOIN users ON expenses.user_ID = users.userID
                WHERE users.userID = @paramID AND (MONTH(expenses.date) = @paramMonth  AND YEAR(expenses.date) = @paramYear) ORDER BY date ASC";

        //Selects the list of incomes for multiple months
        private String sqlStatementMultipleMonthsExpenses = @"SELECT expenses.name AS 'Expense name',(SELECT categoryName FROM expense_types WHERE expense_types.categoryID = expenses.type) AS 'Expense type',  expenses.value AS 'Expense value', expenses.date AS 'Date'
                FROM expenses INNER JOIN users ON expenses.user_ID = users.userID
                WHERE users.userID = @paramID AND expenses.date BETWEEN @paramStartDate AND @paramEndDate ORDER BY date ASC";

        //SQL queries for populating the pie chart
        //Selects the type name and percentage for each type of expense from the total expenses for a single month
        private String sqlStatementExpenseTypeSumSingle = @"SELECT et.categoryName AS 'Expense type', 
                SUM(ex.value) AS 'Total value', 
                FORMAT(((SUM(ex.value) * 100) / (SELECT SUM(value) FROM expenses WHERE MONTH(date) = @paramMonth and YEAR(date) = @paramYear AND user_ID = @paramID)), 2) AS 'Percentage from total expenses' 
                FROM expenses ex
                INNER JOIN expense_types et ON ex.type = et.categoryID
                WHERE MONTH(ex.date) = @paramMonth 
                    AND YEAR(ex.date) = @paramYear 
                    AND ex.user_ID = @paramID
                GROUP BY et.categoryName";

        //Selects the type name and percentage for each type of expense from the total expenses for multiple months
        private String sqlStatementExpenseTypeSumMultiple = @"SELECT et.categoryName AS 'Expense type', 
                SUM(ex.value) AS 'Total value', 
                FORMAT(((SUM(ex.value) * 100) / (SELECT SUM(value) FROM expenses WHERE date BETWEEN @paramStartDate AND @paramEndDate AND user_ID = @paramID)), 2) AS 'Percentage from total expenses' 
                FROM expenses ex
                INNER JOIN expense_types et ON ex.type = et.categoryID
                WHERE ex.date BETWEEN @paramStartDate AND @paramEndDate 
                    AND ex.user_ID = @paramID
                GROUP BY et.categoryName;";

        //SQL queries for populating the column chart
        //Selects the total expenses for each month of the specified year, for the current user
        private String sqlStatementMonthlyTotalExpenses = @"SELECT MONTH(date), SUM(value)
                FROM expenses
                WHERE user_ID = @paramID AND YEAR(date) = @paramYear
                GROUP BY YEAR(date), MONTH(date)";


        public DataTable[] DataSources {
            get {
                return this.dataSources;
            }

            set {
                this.dataSources = value;
                notifyObservers();
            }
        }

        public DataTable getNewData(QueryType option, QueryData paramContainer, SelectedDataSource dataSource) {
            MySqlCommand command = null;
            if (option == QueryType.SINGLE_MONTH) {
                switch (dataSource) {
                    //Grid view
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthExpenses, paramContainer);
                        break;
                    //Pie chart
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementExpenseTypeSumSingle, paramContainer);
                        break;
                    //Column chart
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalExpenses, paramContainer);
                        break;

                    default:
                        break;

                }

            } else if (option == QueryType.MULTIPLE_MONTHS) {
                switch (dataSource) {
                    //Grid view
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsExpenses, paramContainer);
                        break;
                    //Pie chart
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementExpenseTypeSumMultiple, paramContainer);
                        break;
                    //Column chart
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalExpenses, paramContainer);
                        break;

                    default:
                        break;

                }
            } else if (option == QueryType.MONTHLY_TOTALS) {
                command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalExpenses, paramContainer);

            }

            return DBConnectionManager.getData(command);
        }

        public void addObserver(IView observer) {
            observerList.Add(observer);
        }
   
        public void notifyObservers() {
            foreach (IView currentObserver in observerList) {
                currentObserver.updateView(this);
            }
        }

        public void removeObserver(IView observer) {
            observerList.Remove(observer);
        }

        public bool hasDBConnection() {
            return DBConnectionManager.hasConnection();
        }
    }
}
