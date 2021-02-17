using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class DebtsModel : IModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];
   
        //Fraze SQL pt tabel
        //Selecteaza lista datoriilor utilizatorului curent pentru luna si anul specificate
        private String sqlStatementSingleMonthDebts = @"SELECT debts.name AS 'Debt name', debts.value AS 'Value',creditors.creditorName AS 'Creditor name', debts.date AS 'Date' 
                FROM debts
                INNER JOIN creditors on debts.creditor_ID = creditors.creditorID
                WHERE user_ID = @paramID 
                AND (MONTH(debts.date) = @paramMonth AND YEAR(debts.date) = @paramYear)
                ORDER BY date ASC";
        //Selecteaza lista datoriilor utilizatorului curent pentru intervalul specificat
        private String sqlStatementMultipleMonthsDebts = @"SELECT debts.name AS 'Debt name', debts.value AS 'Value',creditors.creditorName AS 'Creditor name', debts.date AS 'Date' 
                FROM debts
                INNER JOIN creditors on debts.creditor_ID = creditors.creditorID
                WHERE user_ID = @paramID 
                AND debts.date BETWEEN @paramStartDate AND  @paramEndDate
                ORDER BY date ASC";

        //Fraze SQL pt pie chart
        //Selecteaza suma datoriilor pentru fiecare creditor al utilizatorului curent pentru luna si anul specificate
        private String sqlStatementDebtValueSumForCreditorSingle = @"SELECT creditors.creditorName, SUM(debts.value) 
                FROM debts
                INNER JOIN creditors ON debts.creditor_ID = creditors.creditorID
                WHERE debts.user_ID = @paramID 
                AND (MONTH(debts.date) = @paramMonth AND YEAR(debts.date) = @paramYear)
                GROUP BY creditors.creditorID";
        //Selecteaza suma datoriilor pentru fiecare creditor al utilizatorului curent pentru intervalul specificat
        private String sqlStatementDebtValueSumForCreditorMultiple = @"SELECT creditors.creditorName, SUM(debts.value) 
                FROM debts 
                INNER JOIN creditors ON debts.creditor_ID = creditors.creditorID
                WHERE debts.user_ID = @paramID 
                AND debts.date BETWEEN @paramStartDate AND @paramEndDate
                GROUP BY creditors.creditorID";

        //Fraze SQL pt column chart
        //Selecteaza valoarea totala a datoriilor pt fiecare luna a anului specificat, pentru utilizatorul curent
        private String sqlStatementMonthlyTotalDebts = @"SELECT MONTH(date), SUM(value)
                FROM debts
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
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSingleMonthDebts, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getSingleMonthCommand(sqlStatementDebtValueSumForCreditorSingle, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalDebts, paramContainer);
                        break;

                    default:
                        break;

                }

            } else if (option == QueryType.MULTIPLE_MONTHS) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementMultipleMonthsDebts, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementDebtValueSumForCreditorMultiple, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalDebts, paramContainer);
                        break;

                    default:
                        break;

                }
            } else if (option == QueryType.MONTHLY_TOTALS) {
                command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalDebts, paramContainer);

            }

            return DBConnectionManager.getData(command);
        }
       
        public void notifyObservers() {
            foreach(IView currentObserver in observerList) {
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
