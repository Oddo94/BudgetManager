using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class SavingsModel : IModel {
        
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];

        //Fraze SQL pt tabel
        //Selecteaza economiile de pe o singura luna
        private String sqlStatementSingleMonthSavings = @"SELECT name AS 'Saving name', value AS 'Value', date AS 'Date' 
                FROM savings 
                WHERE user_ID = @paramID AND(MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)
                ORDER BY date ASC";
        //Selecteaza economiile de pe mai multe luni
        private String sqlStatementMultipleMonthsSavings = @"SELECT name AS 'Saving name', value AS 'Value', date AS 'Date'
                FROM savings
                WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate
                ORDER BY date ASC";

        //Fraze SQL pt pie chart
        //Selecteaza valoarea totala a economiilor de pe o singura luna
        private String sqlStatementSavingsValueSumSingle = @"SELECT (SELECT SUM(savings.value) FROM savings WHERE savings.user_ID = @paramID AND (MONTH(savings.date) = @paramMonth AND YEAR(savings.date) = @paramYear)) AS 'Total savings',
                (SELECT SUM(incomes.value) FROM incomes WHERE incomes.user_ID = @paramID AND (MONTH(incomes.date) = @paramMonth AND YEAR(incomes.date) = @paramYear)) AS 'Total incomes'";
        //Selecteaza valoarea totala a economiilor de pe mai multe luni
        private String sqlStatementSavingsValueSumMultiple = @"SELECT (SELECT SUM(savings.value) FROM savings WHERE savings.user_ID = @paramID AND savings.date BETWEEN @paramStartDate  AND @paramEndDate) AS 'Total savings',
                (SELECT SUM(incomes.value) FROM incomes WHERE incomes.user_ID = @paramID AND incomes.date BETWEEN @paramStartDate AND @paramEndDate) AS 'Total incomes'";

        //Fraze SQL pt column chart
        //Selecteaza suma economiilor pt fiecare luna anului specificat
        private String sqlStatementMonthlyTotalSavings = @"SELECT MONTH(date), SUM(value)
                FROM savings
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
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthSavings, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSavingsValueSumSingle, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalSavings, paramContainer);
                        break;

                    default:
                        break;

                }

            } else if (option == QueryType.MULTIPLE_MONTHS) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsSavings, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementSavingsValueSumMultiple, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalSavings, paramContainer);
                        break;

                    default:
                        break;

                }
                //Obtinere comanda pentru extragere date pentru fiecare luna a unui an
            } else if (option == QueryType.MONTHLY_TOTALS) {
                command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalSavings, paramContainer);

            }

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
