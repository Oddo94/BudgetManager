using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    //Modelul concret pt tabul Incomes
    class IncomesModel : IModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];
 
        //SQL queries for populating the table
        //Selects the list of incomes for a single month
        private String sqlStatementSingleMonthIncomes = @"SELECT name AS 'Income name', (SELECT typeName FROM income_types WHERE income_types.typeID = incomes.incomeType) AS 'Income type', value AS 'Value', date AS 'Date' 
                FROM incomes 
                WHERE user_ID = @paramID AND(MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)
                ORDER BY date ASC";

        //Selects the list of incomes for multiple months
        private String sqlStatementMultipleMonthsIncomes = @"SELECT name AS 'Income name',(SELECT typeName FROM income_types WHERE income_types.typeID = incomes.incomeType) AS 'Income type' , value AS 'Value', date AS 'Date'
                FROM incomes
                WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate
                ORDER BY date ASC";

        //SQL queries for populating the pie chart
        //Selects the type name and percentage for each type of income from the total incomes for a single month
        private String sqlStatementIncomeTypeSumSingle = @"SELECT it.typeName AS 'Income type', 
                SUM(inc.value) AS 'Total value', 
                FORMAT(((SUM(inc.value) * 100) / (SELECT SUM(value) FROM incomes WHERE MONTH(date) = @paramMonth and YEAR(date) = @paramYear AND user_ID = @paramID)), 2) AS 'Percentage from total incomes' 
                FROM incomes inc
                INNER JOIN income_types it ON inc.incomeType = it.typeID
                WHERE MONTH(inc.date) = @paramMonth
                    AND YEAR(inc.date) = @paramYear
                    AND inc.user_ID = @paramID
                GROUP BY it.typeName";

        //Selects the type name and percentage for each type of income from the total incomes for multiple months
        private String sqlStatementIncomeTypeSumMultiple = @"SELECT it.typeName AS 'Income type', 
                SUM(inc.value) AS 'Total value', 
                FORMAT(((SUM(inc.value) * 100) / (SELECT SUM(value) FROM incomes WHERE date BETWEEN @paramStartDate AND @paramEndDate AND user_ID = @paramID)), 2) AS 'Percentage from total incomes' 
                FROM incomes inc
                INNER JOIN income_types it ON inc.incomeType = it.typeID
                WHERE date BETWEEN @paramStartDate AND @paramEndDate
                    AND inc.user_ID = @paramID
                GROUP BY it.typeName;";

    
        //SQL queries for populating the column chart
        //Selects the total incomes for each month of the specified year, for the current user
        private String sqlStatementMonthlyTotalIncomes = @"SELECT MONTH(date), SUM(value)
                FROM incomes
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

        //The method returns a DataTable object using the provided arguments to decide the actual SQl query that will be executed(the query type(single/multiple months) and the data source that will be populated with data are taken into account for this decision)
        public DataTable getNewData(QueryType option, QueryData paramContainer,SelectedDataSource dataSource) {
            //Creates a MySqlCommand object tht will be populated with the actual command object selected after analyzing the previously mentioned arguments inside the if/else and switch block
            MySqlCommand command = null;
            //Single month query
            //The specific SQL query from which the command will be created  is selected based on the data source and the type of data that needs to be displayed
            if (option == QueryType.SINGLE_MONTH) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthIncomes, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementIncomeTypeSumSingle, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalIncomes, paramContainer);
                        break;

                    default:
                        break;

             }           
              //Multiple months query               
            } else if (option == QueryType.MULTIPLE_MONTHS) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsIncomes, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementIncomeTypeSumMultiple, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalIncomes, paramContainer);
                        break;

                    default:
                        break;

                }           
              //Monthly totals query
            } else if (option == QueryType.MONTHLY_TOTALS) {
                command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalIncomes, paramContainer);

            }

            //The data is retrieved by passing the previously obtained command to the DBConnectionManger's getData() method
            return DBConnectionManager.getData(command);
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

        public bool hasDBConnection() {
            return DBConnectionManager.hasConnection();
        }
    }
}
