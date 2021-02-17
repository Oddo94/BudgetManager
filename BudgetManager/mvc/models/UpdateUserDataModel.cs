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

        //Selectare date pe o luna
        String sqlStatementSelectSingleMonthIncomes = @"SELECT incomeID, name, incomeType, value, date FROM incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthExpenses = @"SELECT expenseID, name, type, value, date FROM expenses WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthDebts = @"SELECT debtID, name, value, creditor_ID, date FROM debts WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";
        String sqlStatementSelectSingleMonthSavings = @"SELECT savingID, name, value, date FROM savings WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) ORDER BY date ASC";

        //Selectare date pe un an intreg
        String sqlStatementSelectFullYearIncomes = @"SELECT incomeID, name, incomeType, value, date FROM incomes WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearExpenses = @"SELECT expenseID, name, type, value, date FROM expenses WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearDebts = @"SELECT debtID, name, value, creditor_ID, date FROM debts WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";
        String sqlStatementSelectFullYearSavings = @"SELECT savingID, name, value, date FROM savings WHERE user_ID = @paramID AND YEAR(date) = @paramYear ORDER BY date ASC";

        //Comenzi generale de stergere a datelor
        String sqlStatementDeleteIncome = @"DELETE FROM incomes WHERE incomeID = @paramItemID";
        String sqlStatementDeleteExpense = @"DELETE FROM expenses WHERE expenseID = @paramItemID";
        String sqlStatementDeleteDebt = @"DELETE FROM debts WHERE debtID = @paramItemID";
        String sqlStatementDeleteSaving = @"DELETE FROM savings WHERE savingID = @paramItemID";

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
                        //command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthExpenses, paramContainer);
                        break;
                    //Pie chart
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        //command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementExpenseTypeSumSingle, paramContainer);
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
                        //command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsExpenses, paramContainer);
                        break;
                    //Pie chart
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        //command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementExpenseTypeSumMultiple, paramContainer);
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
            //MySqlConnection conn = DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);
            //try {
                
            //    MySqlCommand updateTableCommand = getCorrectSqlCommandForDataDisplay(option, paramContainer);
            //    updateTableCommand.Connection = conn;
            //    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(updateTableCommand);
            //    MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

            //    DataTable currentTable = sourceDataTable;

            //    executionResult = dataAdapter.Update(currentTable);
            //    currentTable.AcceptChanges();
          

            //} catch (MySqlException ex) {
            //    MessageBox.Show(ex.Message, "Data update form");
            //}

            //if (executionResult > 0) {
            //    return executionResult;
            //}
            //Refacere comanda utilizata la afisarea datelor din tabel
            MySqlCommand updateTableCommand = getCorrectSqlCommandForDataDisplay(option, paramContainer);
            //Apelare metoda de actualizare efectiva a datelor
            executionResult = DBConnectionManager.updateData(updateTableCommand, sourceDataTable);

            if (executionResult > 0) {
                return executionResult;
            }

            return -1;

        }

        public int deleteData(String tableName, int itemID) {
            int executionResult = 0;
            //Creare comandă de ștergere
            MySqlCommand deleteCommand = getCorrectSqlCommandForDeletion(tableName, itemID);
            //executionResult = DBConnectionManager.deleteData(deleteCommand);//MODIFICARE DENUMIRE COMANDA
            //Obtinere rezultat executie
            executionResult = DBConnectionManager.deleteData(deleteCommand);

            if (executionResult > 0) {
                return executionResult;
            }

            return -1;

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
            //Obtinere date din obiectul de tip QueryData
            int userID = paramContainer.UserID;
            String tableName = paramContainer.TableName;
            int selectedMonth = 0;

            //Daca se cer datele pt o luna se va lua valoarea lunii din obiectul de tip QueryData
            if (option == QueryType.SINGLE_MONTH) {
                selectedMonth = paramContainer.Month;
            }

            int selectedYear = paramContainer.Year;

            switch (tableName) {
                //Creaza comanda SQL adecvata in functie de selectia din dateTimePicker(comanda pt o luna/un an intreg)
                case "Incomes":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthIncomes, new QueryData(userID, selectedMonth, selectedYear));
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearIncomes, new QueryData(userID, selectedYear));
                    } else {
                        return null;
                    }
                //break;

                case "Expenses":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthExpenses, new QueryData(userID, selectedMonth, selectedYear));
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearExpenses, new QueryData(userID, selectedYear));
                    } else {
                        return null;
                    }
                //break;

                case "Debts":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthDebts, new QueryData(userID, selectedMonth, selectedYear));
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearDebts, new QueryData(userID, selectedYear));
                    } else {
                        return null;
                    }
                //break;

                case "Savings":
                    if (option == QueryType.SINGLE_MONTH) {
                        return SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectSingleMonthSavings, new QueryData(userID, selectedMonth, selectedYear));
                    } else if (option == QueryType.FULL_YEAR) {
                        return SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectFullYearSavings, new QueryData(userID, selectedYear));
                    } else {
                        return null;
                    }
                //break;

                default:
                    return null;

            }
        }

              private MySqlCommand getCorrectSqlCommandForDeletion(String tableName, int itemID) {
            switch (tableName) {
                //Creaza comanda sql adecvata in functie de selectia din dateTimePicker(comanda pt o luna/un an intreg)
                case "Incomes":
                    MySqlCommand deleteIncomeCommand = new MySqlCommand(sqlStatementDeleteIncome);
                    deleteIncomeCommand.Parameters.AddWithValue("@paramItemID", itemID);

                    return deleteIncomeCommand;

                case "Expenses":
                    MySqlCommand deleteExpenseCommand = new MySqlCommand(sqlStatementDeleteExpense);
                    deleteExpenseCommand.Parameters.AddWithValue("@paramItemID", itemID);

                    return deleteExpenseCommand;

                case "Debts":
                    MySqlCommand deleteDebtCommand = new MySqlCommand(sqlStatementDeleteDebt);
                    deleteDebtCommand.Parameters.AddWithValue("@paramItemID", itemID);

                    return deleteDebtCommand;

                case "Savings":
                    MySqlCommand deleteSavingCommand = new MySqlCommand(sqlStatementDeleteSaving);
                    deleteSavingCommand.Parameters.AddWithValue("@paramItemID", itemID);

                    return deleteSavingCommand;


                default:
                    return null;

            }
        }
    }

    }
 

