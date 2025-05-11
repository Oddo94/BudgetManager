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

        //SQL statement for retrieving the current total balance of the saving account
        private String sqlStatementSavingAccountCurrentBalance = @"SELECT
	                                                                   abs.currentBalance
                                                                   FROM
	                                                                   account_balance_storage abs
                                                                   INNER JOIN accounts acc ON
	                                                                   abs.account_ID = acc.accountID
                                                                   INNER JOIN account_types at ON
	                                                                   acc.type_ID = at.typeID
                                                                   WHERE at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT' AND acc.user_ID = @paramID";

        //SQL statement for retrieving the data showing the yearly saving account balance evolution
        private String sqlStatementFullYearBalanceEvolution = @"SELECT * FROM
                (SELECT sab.year, sab.month, SUM(sab.value) OVER(PARTITION BY sab.user_ID ORDER BY sab.year, sab.month) AS 'Running total'
                  FROM saving_accounts_balance sab
                  INNER JOIN accounts acc on sab.account_ID = acc.accountID
                  INNER JOIN account_types at on acc.type_ID = at.typeID
                  WHERE sab.user_ID = @paramID
                  AND at.typeName = 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'
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
