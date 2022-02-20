using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.models {
    class SavingAccountModel : IModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];

        //SQL statements for retrieving single/multiple months savings data
        private String sqlStatementSingleMonthSavings = @"SELECT savingID AS 'ID', name AS 'Saving name', value AS 'Value', date AS 'Date' FROM savings WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        private String sqlStatementMultipleMonthsSavings = @"SELECT savingID AS 'ID', name AS 'Saving name', value AS 'Value', date AS 'Date'
                FROM savings
                WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate
                ORDER BY date ASC";

        //SQL statements for retrieving single month/multiple months saving account expenses data
        private String sqlStatementSingleMonthSavingAccountExpenses = @"SELECT expenseID AS 'ID', name AS 'Name', (SELECT categoryName FROM expense_types WHERE categoryID = type) AS 'Expense type', value AS 'Value', date AS 'Date' FROM saving_accounts_expenses WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        private String sqlStatementMultipleMonthsSavingAccountExpenses = @"SELECT expenseID AS 'ID', name AS 'Name', (SELECT categoryName FROM expense_types WHERE categoryID = type) AS 'Expense type', value AS 'Value', date AS 'Date' FROM saving_accounts_expenses WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate ORDER BY date ASC";

        //SQL statement for calculating the current total balance of the saving account(up to the current month of the current year)
        private String sqlStatementSavingAccountCurrentBalance = @"SELECT SUM(value) FROM
                (SELECT sab.value, sab.account_ID, sat.typeID, sab.month, sab.year FROM saving_accounts_balance sab
                  INNER JOIN saving_accounts sa on sab.account_ID = sa.accountID
                  INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                  WHERE sab.user_ID = @paramID
                  AND sat.typeID = 1
                  AND year <= year(CURDATE())) AS subquery
                WHERE (subquery.month <= MONTH(CURDATE()) AND subquery.year <= YEAR(CURDATE())) OR (subquery.month > MONTH(CURDATE()) AND subquery.year < YEAR(CURDATE()))";

        //SQL statement for retrieving the data showing the yearly saving account balance evolution
        private String sqlStatementFullYearBalanceEvolution = @"SELECT * FROM
                (SELECT sab.year, sab.month, SUM(sab.value) OVER(PARTITION BY sab.user_ID ORDER BY sab.year, sab.month) AS 'Running total'
                  FROM saving_accounts_balance sab
                  INNER JOIN saving_accounts sa on sab.account_ID = sa.accountID
                  INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                  WHERE sab.user_ID = @paramID
                  AND sat.typeID = 1
               ) AS subquery
                WHERE year = @paramYear";


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
                    //Grid view data source
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = getCorrectCommandForDataDisplay(option, paramContainer);
                        break;
                    //Column chart data source
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = getCorrectCommandForDataDisplay(option, paramContainer);
                        break;
                    //Current balance field data source
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getSpecificUserRecordsCommand(sqlStatementSavingAccountCurrentBalance, paramContainer);
                        break;
                }
            } else if (option == QueryType.MULTIPLE_MONTHS) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = getCorrectCommandForDataDisplay(option, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = getCorrectCommandForDataDisplay(option, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getSpecificUserRecordsCommand(sqlStatementSavingAccountCurrentBalance, paramContainer);
                        break;
                }
            } else if (option == QueryType.MONTHLY_TOTALS) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        //command = getCorrectCommandForDataDisplay(option, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        //The getCorrectCommandFordataDisplay() method is not used in this case since displaying the monthly balance evolution for the selected year does not require to specify a table from which data will be extracted
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementFullYearBalanceEvolution, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getSpecificUserRecordsCommand(sqlStatementSavingAccountCurrentBalance, paramContainer);
                        break;
                        //command = SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementFullYearBalanceEvolution, paramContainer);
                }
            } else if (option == QueryType.TOTAL_VALUE) {
                command = SQLCommandBuilder.getSpecificUserRecordsCommand(sqlStatementSavingAccountCurrentBalance, paramContainer); ;
            }

            return DBConnectionManager.getData(command);
        }

        private MySqlCommand getCorrectCommandForDataDisplay(QueryType option, QueryData paramContainer) {
            //Retrieving data from the Querydata object(container object)
            int userID = paramContainer.UserID;
            String tableName = paramContainer.TableName;
            int selectedMonth = 0;

            //If single month data is requested then the value of the month from the QueryData object will be retrieved
            if (option == QueryType.SINGLE_MONTH) {
                selectedMonth = paramContainer.Month;
            }

            int selectedYear = paramContainer.Year;

            switch(tableName) {
                //Creates the correct SQL command based on the dateTimePicker selection(single month command/multiple months command)
                case "Savings":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthSavings, paramContainer);
                    } else if (option == QueryType.MULTIPLE_MONTHS) {
                        return SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsSavings, paramContainer);
                    } else {
                        return null;
                    }

                case "Saving accounts expenses":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthSavingAccountExpenses, paramContainer);                   
                    } else if (option == QueryType.MULTIPLE_MONTHS) {
                        return SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsSavingAccountExpenses, paramContainer);
                    } else {
                        return null;
                    }

                default:
                    return null;
            }

        }

        public bool hasDBConnection() {
            return DBConnectionManager.hasConnection();
        }

        public void notifyObservers() {
            foreach (IView currentObserver in observerList) {
                currentObserver.updateView(this);
            }
                
        }

        public void addObserver(IView observer) {
            observerList.Add(observer);
        }

        public void removeObserver(IView observer) {
            observerList.Remove(observer);
        }
    }
}
