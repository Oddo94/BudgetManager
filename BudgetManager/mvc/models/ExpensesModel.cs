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
        
        //Fraze SQL pt tabel
        //Selecteaza lista cheltuielilor efectuate pe o singura luna
        private String sqlStatementSingleMonthExpenses = @"SELECT expenses.name AS 'Expense name', (SELECT categoryName FROM expense_types WHERE expense_types.categoryID = expenses.type) AS 'Expense type', expenses.value AS 'Expense value', expenses.date AS 'Date'
                FROM expenses INNER JOIN users ON expenses.user_ID = users.userID
                WHERE users.userID = @paramID AND (MONTH(expenses.date) = @paramMonth  AND YEAR(expenses.date) = @paramYear) ORDER BY date ASC";
        //Selecteaza lista cheltuielilor efectuate pe mai multe luni
        private String sqlStatementMultipleMonthsExpenses = @"SELECT expenses.name AS 'Expense name',(SELECT categoryName FROM expense_types WHERE expense_types.categoryID = expenses.type) AS 'Expense type',  expenses.value AS 'Expense value', expenses.date AS 'Date'
                FROM expenses INNER JOIN users ON expenses.user_ID = users.userID
                WHERE users.userID = @paramID AND expenses.date BETWEEN @paramStartDate AND @paramEndDate ORDER BY date ASC";

        //Fraze SQL pt pie chart   
        //Selecteaza valoarea totala a cheltuielilor pt luna selectata pt fiecare tip de cheltuiala(fixa, periodica, variabila) pt utilizatorul curent
        //private String sqlStatementExpenseTypeSumSingle = @"SELECT SUM(CASE WHEN user_ID = @paramID 
        //        AND type = (SELECT categoryID FROM expense_types WHERE categoryName = 'Fixed expense')
        //        AND MONTH(date) = @paramMonth
        //        AND YEAR(date) = @paramYear THEN value ELSE 0 END) AS 'Fixed expenses',
        //        SUM(CASE WHEN user_ID = @paramID 
        //        AND type = (SELECT categoryID FROM expense_types WHERE categoryName = 'Periodic expense')
        //        AND MONTH(date) = @paramMonth
        //        AND YEAR(date) = @paramYear THEN value ELSE 0 END) AS 'Periodic expenses',
        //        SUM(CASE WHEN user_ID = @paramID 
        //        AND type = (SELECT categoryID FROM expense_types WHERE categoryName = 'Variable expense')
        //        AND MONTH(date) = @paramMonth
        //        AND YEAR(date) = @paramYear THEN value ELSE 0 END) AS 'Variable expenses'
        //        FROM expenses";

        private String sqlStatementExpenseTypeSumSingle = @"SELECT et.categoryName AS 'Expense type', 
                SUM(ex.value) AS 'Total value', 
                FORMAT(((SUM(ex.value) * 100) / (SELECT SUM(value) FROM expenses WHERE MONTH(date) = @paramMonth and YEAR(date) = @paramYear AND user_ID = @paramID)), 2) AS 'Percentage from total expenses' 
                FROM expenses ex
                INNER JOIN expense_types et ON ex.type = et.categoryID
                WHERE MONTH(ex.date) = @paramMonth 
                    AND YEAR(ex.date) = @paramYear 
                    AND ex.user_ID = @paramID
                GROUP BY et.categoryName";
        ////Selecteaza valoarea totala a cheltuielilor pe mai multe luni pt fiecare tip de cheltuiala(fixa, periodica, variabila) pt utilizatorul curent
        //private String sqlStatementExpenseTypeSumMultiple = @"SELECT SUM(CASE WHEN user_ID = @paramID 
        //        AND type = (SELECT categoryID FROM expense_types WHERE categoryName = 'Fixed expense')
        //        AND date BETWEEN @paramStartDate AND @paramEndDate THEN value ELSE 0 END) AS 'Fixed expenses',
        //        SUM(CASE WHEN user_ID = @paramID 
        //        AND type = (SELECT categoryID FROM expense_types WHERE categoryName = 'Periodic expense')
        //        AND date BETWEEN @paramStartDate AND @paramEndDate THEN value ELSE 0 END) AS 'Periodic expenses',
        //        SUM(CASE WHEN user_ID = @paramID 
        //        AND type = (SELECT categoryID FROM expense_types WHERE categoryName = 'Variable expense')
        //        AND date BETWEEN @paramStartDate AND @paramEndDate THEN value ELSE 0 END) AS 'Variable expenses'
        //        FROM expenses";

        private String sqlStatementExpenseTypeSumMultiple = @"SELECT et.categoryName AS 'Expense type', 
                SUM(ex.value) AS 'Total value', 
                FORMAT(((SUM(ex.value) * 100) / (SELECT SUM(value) FROM expenses WHERE date BETWEEN @paramStartDate AND @paramEndDate AND user_ID = @paramID)), 2) AS 'Percentage from total expenses' 
                FROM expenses ex
                INNER JOIN expense_types et ON ex.type = et.categoryID
                WHERE ex.date BETWEEN @paramStartDate AND @paramEndDate 
                    AND ex.user_ID = @paramID
                GROUP BY et.categoryName;";

        //Fraze SQL pt column chart
        //Selecteaza valoarea totala a cheltuielilor pt fiecare luna a anului specificat,pentru utilizatorul curent
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
