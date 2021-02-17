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
 
        //Fraze SQL pt tabel
        //Selecteaza lista veniturilor pe o singura luna
        private String sqlStatementSingleMonthIncomes = @"SELECT name AS 'Income name', (SELECT typeName FROM income_types WHERE income_types.typeID = incomes.incomeType) AS 'Income type', value AS 'Value', date AS 'Date' 
                FROM incomes 
                WHERE user_ID = @paramID AND(MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)
                ORDER BY date ASC";
        //Selecteaza lista veniturilor pe mai multe luni
        private String sqlStatementMultipleMonthsIncomes = @"SELECT name AS 'Income name',(SELECT typeName FROM income_types WHERE income_types.typeID = incomes.incomeType) AS 'Income type' , value AS 'Value', date AS 'Date'
                FROM incomes
                WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate
                ORDER BY date ASC";
        //Fraze SQL pt pie chart
        //Selecteaza suma veniturilor pe o luna dupa tipul de venit selectat(Active/Passive)      
        private String sqlStatementIncomeTypeSumSingle = @"SELECT SUM(CASE WHEN user_ID = @paramID 
                AND incomeType = (SELECT typeID FROM income_types WHERE typeName = 'Active income') 
                AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear) THEN value ELSE 0 END) AS 'Active income',
                SUM(CASE WHEN user_ID = @paramID 
                AND incomeType = (SELECT typeID FROM income_types WHERE typeName = 'Passive income') 
                AND(MONTH(date) = @paramMonth  AND YEAR(date) = @paramYear) THEN value ELSE 0 END) AS 'Passive income'
                FROM incomes";
        //Selecteaza suma veniturilor pe mai multe luni dupa tipul de venit selectat(Active/Passive)      
        private String sqlStatementIncomeTypeSumMultiple = @"SELECT SUM(CASE WHEN user_ID = @paramID
                AND incomeType = (SELECT typeID FROM income_types WHERE typeName = 'Active income')
                AND date BETWEEN @paramStartDate AND  @paramEndDate
                THEN value ELSE 0 END) AS 'Active income',
                SUM(CASE WHEN user_ID = @paramID
                AND incomeType = (SELECT typeID FROM income_types WHERE typeName = 'Passive income')
                AND date BETWEEN @paramStartDate AND @paramEndDate THEN value ELSE 0 END) AS 'Passive income'
                FROM incomes";
        //Fraze SQL pt column chart
        //Selecteaza valoarea totala a venitului pt fiecare luna a anului specificat, pentru utilizatorul curent
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

        //Metoda returneaza un obiect de tip DataTable utilizand parametri furnizati pt a decide fraza SQL efectiva ce va fi executata(se tine cont de tipul de optiune(una sau mai multe luni) respectiv de sursa de date care se doreste a fi populata)
        public DataTable getNewData(QueryType option, QueryData paramContainer,SelectedDataSource dataSource) {
            //Se creaza o referită de tipul MySqlCommand careia ii va fi atribuita obiectul creat in urma executiei blocului if/else si a blocului switch
            MySqlCommand command = null;
            //Interogare date pe o luna
            //Pentru fiecare sursă de date se selectează fraza SQL specifica pe baza careia se creaza comanda, in functie de tipul de date ce se doresc a fi afisate
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
              //Interogare date pe mai multe luni                
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
              //Interogare date pt situatia lunara dintr-un an 
            } else if (option == QueryType.MONTHLY_TOTALS) {
                command = SQLCommandBuilder.getMonthlyTotalsCommand(sqlStatementMonthlyTotalIncomes, paramContainer);

            }

            //In final se apeleaza metoda getData() a clasei DBConnection Manager utilizand comanda creata
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
