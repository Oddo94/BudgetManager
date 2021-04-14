using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    //Clasa utilitara prin care se realizeaza conexiunea la baza de date si se obtin datele din aceasta sub forma de DataTable
    class DBConnectionManager {
        
        public static readonly String BUDGET_MANAGER_CONN_STRING = Properties.Settings.Default.BUDGET_MANAGER_CONN_STRING;

        //Creates a new connection
        public static MySqlConnection getConnection(String connString) {

            return new MySqlConnection(connString);
        }

        //Creates a new data adapter
        public static MySqlDataAdapter getDataAdapter(MySqlCommand command) {

            return new MySqlDataAdapter(command);

        }

        public static DataTable getData(MySqlCommand command) {
            //Se creaza conexiunea
            MySqlConnection conn = DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);
            //Se impune proprietății obiectului command conexiunea creata
            command.Connection = conn;
            //Se creaza un adaptor pe baza obiectului command
            MySqlDataAdapter adp = DBConnectionManager.getDataAdapter(command);
            //Se initializeaza un obiect de tip DataTable gol
            DataTable dataTable = new DataTable();

            try {
                //Se deschide conexiunea si se umple cu date obiectul DataTable
                conn.Open();
                adp.Fill(dataTable);

            } catch (MySqlException ex) {
                //Daca se ridica vreo exceptie se afiseaza o fereastra de dialog care contine mesajul acesteia             
                MessageBox.Show(ex.Message, "DBConnectionManager");
                
            } finally {
                //Indiferent de rezultat se inchide conexiune creata
                conn.Close();
            }

            //Se returneaza obiectul DataTable
            return dataTable;
        }

        //Metoda de inserare a datelor in baza de date
        public static int insertData(MySqlCommand command) {
            int executionResult = 0;
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
            command.Connection = conn;
            conn.Open();
            //Creare tranzactie
            MySqlTransaction tx = conn.BeginTransaction();
            command.Transaction = tx;

            try {
                //Numarul de randuri afectat de executia comezii SQL este stocat in variabila(daca este mai mare decat 0 inseamna ca executia a avut loc cu succes altfel inseamna ca a esuat)
                executionResult = command.ExecuteNonQuery();
                //Confirmarea modificarii in baza de date
                tx.Commit();
            } catch (MySqlException ex) {
                int errorCode = ex.Number;
                string message = "";

                switch (errorCode) {
                    case 1062:
                        message = "The entry already exists in the database!";
                        break;

                    default:
                        message = ex.Message;
                        break;
                }
                MessageBox.Show(message, "DBConnectionManager");
                tx.Rollback();//Daca apare o exceptie se readuce baza de date la starea initiala
            } finally {
                conn.Close();
            }

            //Daca executia comenzii a avut loc cu succes se returneaza numarul de randuri afectate din tabel iar in caz contrar se returneaza -1 ceea ce indica esuarea operatiunii
            if (executionResult != 0 ) {           
                return executionResult;
            }

            return -1;
        }

        public static int updateData(MySqlCommand command, DataTable sourceTable) {
            int executionResult = 0;
            //Creare conexiune si atribuirea acesteia comenzii
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
            command.Connection = conn;

            //Deschidere conexiune
            conn.Open();

            //Creare tranzactie
            MySqlTransaction tx = conn.BeginTransaction();
            command.Transaction = tx;

            //Creare DataAdapter pt actualizarea datelor din baza de date cu modificarile efecutate de utilizator in interfata
            MySqlDataAdapter dataAdapter = getDataAdapter(command);
            //Creare obiect CommandBuilder pentru generarea automata a comenzilor INSERT, UPDATE, DELETE care sa reflecte modificarile din tabelul sursa in baza de date
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);
            
            try {
                //Numarul de randuri afectat de executia comezii SQL este stocat in variabila(daca este mai mare decat 0 inseamna ca executia a avut loc cu succes altfel inseamna ca a esuat)
                executionResult = dataAdapter.Update(sourceTable);
                sourceTable.AcceptChanges();
                //Confirmarea modificarii in baza de date
                tx.Commit();

            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message, "DBConnectionManager");
                tx.Rollback();//Daca se genereaza o execptie baza de date este readusa la starea initiala
            } finally {
                conn.Close();
            }

            //Daca executia comenzii a avut loc cu succes se returneaza numarul de randuri afectate din tabel iar in caz contrar se returneaza -1 ceea ce indica esuarea operatiunii
            if (executionResult != 0) {
                return executionResult;
            }

            return -1;

        }


        //public static int deleteData(MySqlCommand command) {
        //    //Creare conexiune și impunerea acesteia comenzii primite ca argument
        //    MySqlConnection conn = getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);
        //    command.Connection = conn;
        //    conn.Open();
        //    //Creare tranzactie si impunerea acesteia comenzii
        //    MySqlTransaction tx = conn.BeginTransaction();
        //    command.Transaction = tx;

        //    int executionResult = 0;
        //    try {
        //        //Obtinere rezultat executie și confirmarea modificării in baza de date
        //        executionResult = command.ExecuteNonQuery();
        //        //Confirmarea modificarii in baza de date
        //        tx.Commit();

        //    } catch (MySqlException ex) {
        //        MessageBox.Show(ex.Message, "DBConnectionManager");
        //        tx.Rollback();//Se readuce baza de date la starea initiala daca s-a generat o exceptie
        //    } finally {
        //        conn.Close();// Inchidere conexiune
        //    }

        //    //Daca executia comenzii a avut loc cu succes se returneaza numarul de randuri afectate din tabel iar in caz contrar se returneaza -1 ceea ce indica esuarea operatiunii
        //    if (executionResult != 0) {                
        //        return executionResult;
        //    }

        //    return -1;
        //}

        //CHANGE!!!!
        public static int deleteData(MySqlCommand command, DataTable sourceDataTable) {
            MySqlConnection conn = getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);

            command.Connection = conn;
            conn.Open();

            MySqlTransaction tx = conn.BeginTransaction();
            command.Transaction = tx;

            MySqlDataAdapter dataAdapter = getDataAdapter(command);
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

            int executionResult = -1;
            try {

                executionResult = dataAdapter.Update(sourceDataTable);
                sourceDataTable.AcceptChanges();
                tx.Commit();

            }catch(MySqlException ex) {
                MessageBox.Show(ex.Message);
                tx.Rollback();

            } catch(DBConcurrencyException ex) {
                MessageBox.Show(ex.Message);
                tx.Rollback();

            } finally {
               conn.Close();
            }

            return executionResult;

        }

        public static bool hasConnection() {
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);

            try {
                conn.Open();
                return true;

            } catch(MySqlException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
