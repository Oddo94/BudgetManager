using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class BudgetSummaryModel : IModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];
      
        //Selecteaza suma veniturilor, cheltuielilor, datoriilor si economiilor pt o singura luna
        private String sqlStatementSummarySingle = @"
                SELECT (SELECT SUM(incomes.value) FROM incomes WHERE incomes.user_ID = @paramID AND (MONTH(incomes.date) = @paramMonth AND YEAR(incomes.date) = @paramYear)) AS 'Income value', 
                (SELECT SUM(expenses.value) FROM expenses WHERE expenses.user_ID = @paramID AND (MONTH(expenses.date) = @paramMonth AND YEAR(expenses.date) = @paramYear)) AS 'Expenses value',
                (SELECT SUM(debts.value) FROM debts WHERE debts.user_ID = @paramID AND (MONTH(debts.date) = @paramMonth AND YEAR(debts.date) = @paramYear)) AS 'Debts value',
                (SELECT SUM(savings.value) FROM savings WHERE savings.user_ID = @paramID AND (MONTH(savings.date) = @paramMonth AND YEAR(savings.date)= @paramYear)) AS 'Savings value'";

        //Selecteaza suma veniturilor, cheltuielilor, datoriilor si economiilor pt mai multe luni
        private String sqlStatementSummaryMultiple = @"
                SELECT (SELECT SUM(incomes.value) FROM incomes WHERE incomes.user_ID = @paramID AND incomes.date BETWEEN @paramStartDate AND @paramEndDate) AS 'Income value', 
                (SELECT SUM(expenses.value) FROM expenses WHERE expenses.user_ID = @paramID AND expenses.date BETWEEN @paramStartDate AND @paramEndDate) AS 'Expenses value',
                (SELECT SUM(debts.value) FROM debts WHERE debts.user_ID = @paramID AND debts.date BETWEEN @paramStartDate AND @paramEndDate) AS 'Debts value',
                (SELECT SUM(savings.value) FROM savings WHERE savings.user_ID = @paramID AND savings.date BETWEEN @paramStartDate AND @paramEndDate) AS 'Savings value'";


        public DataTable[] DataSources {
            get {
                return this.dataSources;
            }

            set {
                this.dataSources = value;
                notifyObservers();
            }
        }

        public void addObserver(IView observer) {
            observerList.Add(observer);
        }

        //Metoda prin care modelul aduce date din DB
        public DataTable getNewData(QueryType option, QueryData paramContainer, SelectedDataSource dataSource) {
            MySqlCommand command = null;
            if (option == QueryType.SINGLE_MONTH) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSummarySingle, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        break;

                    default:
                        break;
                   }
                } else if (option == QueryType.MULTIPLE_MONTHS) {
                    switch (dataSource) {
                        case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                            command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementSummaryMultiple, paramContainer);
                            break;

                        case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                            break;

                        case SelectedDataSource.STATIC_DATASOURCE:
                            break;

                        default:
                            break;
                    }
                }
             //In cazul modelelor care nu utilizeaza toate cele trei data tables definite in interfata IModel comanda SQL ramane cu valoarea null si
             //astfel nu se va mai executa metoda de extragere a datelor din DB
            if (command == null) {
                return null;
            }

            return DBConnectionManager.getData(command);
        }

        public void notifyObservers() {
            foreach(IView currentObserver in observerList) {
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
