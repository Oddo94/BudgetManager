using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    class UpdateUserDataModel : IUpdaterModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];

        //SQL statements for selecting single month data
        String sqlStatementSelectSingleMonthIncomes = @"SELECT incomeID, name, incomeType, value, date FROM incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthGeneralExpenses = @"SELECT expenseID, name, type, value, date FROM expenses WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthSavingAccountExpenses = @"SELECT expenseID AS 'ID', name AS 'Name', (SELECT typeName FROM income_types WHERE typeID = type) AS 'Expense type', value AS 'Value', date AS 'Date' FROM `saving_account_expenses` WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthDebts = @"SELECT debtID, name, value, creditor_ID, date FROM debts WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthSavings = @"SELECT savingID, name, value, date FROM savings WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";

        //SQL statements for selecting full year data
        String sqlStatementSelectFullYearIncomes = @"SELECT incomeID, name, incomeType, value, date FROM incomes WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearGeneralExpenses = @"SELECT expenseID, name, type, value, date FROM expenses WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearSavingAccountExpenses = @"SELECT expenseID AS 'ID', name AS 'Name', (SELECT typeName FROM income_types WHERE typeID = type) AS 'Expense type', value AS 'Value', date AS 'Date' FROM `saving_account_expenses` WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearDebts = @"SELECT debtID, name, value, creditor_ID, date FROM debts WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearSavings = @"SELECT savingID, name, value, date FROM savings WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";


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
                        break;
                    //Pie chart
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        break;
                    //Column chart
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    default:
                        break;

                }

            } else if (option == QueryType.FULL_YEAR) {
                switch (dataSource) {
                    //Grid view
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:                       
                        break;
                    //Pie chart
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:                       
                        break;
                    //Column chart
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    default:
                        break;

                }
            } 

            return DBConnectionManager.getData(command);
        }

        public int updateData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            int executionResult = 0;
           
            //Recreating the command used for displaying the data in the table
            MySqlCommand updateTableCommand = getCorrectSqlCommandForDataDisplay(option, paramContainer);
            
            //Calling the method which updates the data
            executionResult = DBConnectionManager.updateData(updateTableCommand, sourceDataTable);

            if (executionResult > 0) {
                return executionResult;
            }

            return -1;

        }

        //CHANGE!!!!!
        public int deleteData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            MySqlCommand updateTableCommand = getCorrectSqlCommandForDataDisplay(option, paramContainer);
            int executionResult = DBConnectionManager.deleteData(updateTableCommand, sourceDataTable);

            return executionResult;
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
            observerList.Remove(this);
        }

        private MySqlCommand getCorrectSqlCommandForDataDisplay(QueryType option, QueryData paramContainer) {          
            //Retrieving data from the Querydata object(container object)
            int userID = paramContainer.UserID;
            String tableName = paramContainer.TableName;
            int selectedMonth = 0;
        
            //If single month data is requested then the value of the month from the QueryData object will be retrieved
            if (option == QueryType.SINGLE_MONTH) {
                selectedMonth = paramContainer.Month;
            }

            int selectedYear = paramContainer.Year;

            switch (tableName) {              
                //Creates the correct SQL command based on the dateTimePicker selection(single month command/full year command)
                case "Incomes":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthIncomes, new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).build());
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearIncomes, new QueryData.Builder(userID).addYear(selectedYear).build()); //CHANGE
                    } else {
                        return null;
                    }               

                case "General expenses":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthGeneralExpenses, new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).build());
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearGeneralExpenses, new QueryData.Builder(userID).addYear(selectedYear).build());
                    } else {
                        return null;
                    }            

                case "Saving account expenses":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthSavingAccountExpenses, new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).build());
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearSavingAccountExpenses, new QueryData.Builder(userID).addYear(selectedYear).build());
                    } else {
                        return null;
                    }

                case "Debts":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthDebts, new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).build());
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearDebts, new QueryData.Builder(userID).addYear(selectedYear).build());
                    } else {
                        return null;
                    }             

                case "Savings":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthSavings, new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).build());
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearSavings, new QueryData.Builder(userID).addYear(selectedYear).build());
                    } else {
                        return null;
                    }              

                default:
                    return null;
            }
        }
    }
 }
 

