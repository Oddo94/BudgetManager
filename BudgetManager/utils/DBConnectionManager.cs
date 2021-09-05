﻿using MySql.Data.MySqlClient;
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
                int errorCode = ex.Number;
                String message;
                if (errorCode == 1042) {
                     message = "Unable to connect to the database! Please check the connection and try again.";
                } else {
                     message = ex.Message;
                }          
                MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            } finally {
                //Indiferent de rezultat se inchide conexiune creata
                conn.Close();
            }

            //Se returneaza obiectul DataTable
            return dataTable;
        }

        //Metoda de inserare a datelor in baza de date
        //public static int insertData(MySqlCommand command) {
        //    int executionResult = 0;
        //    MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
        //    command.Connection = conn;
        //    conn.Open();
        //    //Creare tranzactie
        //    MySqlTransaction tx = conn.BeginTransaction();
        //    command.Transaction = tx;

        //    try {
        //        //Numarul de randuri afectat de executia comezii SQL este stocat in variabila(daca este mai mare decat 0 inseamna ca executia a avut loc cu succes altfel inseamna ca a esuat)
        //        executionResult = command.ExecuteNonQuery();
        //        //Confirmarea modificarii in baza de date
        //        tx.Commit();
        //    } catch (MySqlException ex) {
        //        int errorCode = ex.Number;
        //        string message = "";

        //        switch (errorCode) {
        //            case 1062:
        //                message = "The entry already exists in the database!";
        //                break;

        //            default:
        //                message = ex.Message;
        //                break;
        //        }
        //        MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        tx.Rollback();//Daca apare o exceptie se readuce baza de date la starea initiala
        //    } finally {
        //        conn.Close();
        //    }

        //    //Daca executia comenzii a avut loc cu succes se returneaza numarul de randuri afectate din tabel iar in caz contrar se returneaza -1 ceea ce indica esuarea operatiunii
        //    if (executionResult != 0 ) {           
        //        return executionResult;
        //    }

        //    return -1;
        //}

        public static int insertData(MySqlCommand command) {
            int executionResult = 0;
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
            command.Connection = conn;

            //Creare tranzactie
            //MySqlTransaction tx = conn.BeginTransaction();
            //command.Transaction = tx;
            MySqlTransaction tx = null;

            try {
                conn.Open();

                tx = conn.BeginTransaction();
                command.Transaction = tx;

                //Numarul de randuri afectat de executia comezii SQL este stocat in variabila(daca este mai mare decat 0 inseamna ca executia a avut loc cu succes altfel inseamna ca a esuat)
                executionResult = command.ExecuteNonQuery();
                //Confirmarea modificarii in baza de date
                tx.Commit();
            } catch (MySqlException ex) {
                int errorCode = ex.Number;
                string message = "";

                switch (errorCode) {               
                    case 1042:
                        message = "Unable to connect to the database! Please check the connection and try again.";
                        break;

                    case 1062:
                        message = "The entry already exists in the database!";
                        break;

                    default:
                        message = ex.Message;
                        break;
                }
                MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Null check for transaction in case the connection cannot be established
                if (tx != null) {
                    tx.Rollback();//Daca apare o exceptie se readuce baza de date la starea initiala
                }              
            } finally {
                conn.Close();
            }

            //Daca executia comenzii a avut loc cu succes se returneaza numarul de randuri afectate din tabel iar in caz contrar se returneaza -1 ceea ce indica esuarea operatiunii
            if (executionResult != 0) {
                return executionResult;
            }

            return -1;
        }

        //public static int updateData(MySqlCommand command, DataTable sourceTable) {
        //    int executionResult = 0;
        //    //Creare conexiune si atribuirea acesteia comenzii
        //    MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
        //    command.Connection = conn;

        //    //Deschidere conexiune
        //    conn.Open();

        //    //Creare tranzactie
        //    MySqlTransaction tx = conn.BeginTransaction();
        //    command.Transaction = tx;

        //    //Creare DataAdapter pt actualizarea datelor din baza de date cu modificarile efecutate de utilizator in interfata
        //    MySqlDataAdapter dataAdapter = getDataAdapter(command);
        //    //Creare obiect CommandBuilder pentru generarea automata a comenzilor INSERT, UPDATE, DELETE care sa reflecte modificarile din tabelul sursa in baza de date
        //    MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

        //    try {
        //        //Numarul de randuri afectat de executia comezii SQL este stocat in variabila(daca este mai mare decat 0 inseamna ca executia a avut loc cu succes altfel inseamna ca a esuat)
        //        executionResult = dataAdapter.Update(sourceTable);
        //        sourceTable.AcceptChanges();
        //        //Confirmarea modificarii in baza de date
        //        tx.Commit();

        //    } catch (MySqlException ex) {
        //        MessageBox.Show(ex.Message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        tx.Rollback();//Daca se genereaza o execptie baza de date este readusa la starea initiala
        //    } finally {
        //        conn.Close();
        //    }

        //    //Daca executia comenzii a avut loc cu succes se returneaza numarul de randuri afectate din tabel iar in caz contrar se returneaza -1 ceea ce indica esuarea operatiunii
        //    if (executionResult != 0) {
        //        return executionResult;
        //    }

        //    return -1;

        //}

        public static int updateData(MySqlCommand command, DataTable sourceTable) {
            int executionResult = 0;
           
            //Creating the connection object and assigning it to the command object
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
            command.Connection = conn;
                
            //Creating the transaction(it is created here and initialised to null to allow its use in the catch block)
            MySqlTransaction tx = null;
                           
            //Creating the DataAdapter object for updating the DB with the changes performed by the user in the GUI
            MySqlDataAdapter dataAdapter = getDataAdapter(command);
            
            //Creating the CommandBuilder object for the automatic creation of the INSERT, UPDATE, DELETE commands which will reflect the changes from the source DataTable into the DB 
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

            try {
                conn.Open();

                tx = conn.BeginTransaction();
                command.Transaction = tx;
               
                //The number of affected rows after the execution of the SQL statement command is stored in this variable(if the number is greater than 0 is means that the command was executed successfully otherwise it means that it has failed)
                executionResult = dataAdapter.Update(sourceTable);
                sourceTable.AcceptChanges();
                //The changes are submitted to the DB
                tx.Commit();

            } catch (MySqlException ex) {
                //Retrieving the error code
                int errorCode = ex.Number;
                //The message is composed based on the error code returned (in order to improve the error understanding for the end user)
                String message;
                if (errorCode == 1042) {
                    message = "Unable to connect to the database! Please check the connection and try again.";
                } else {
                    message = ex.Message;
                }

                MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Null check for the transaction object to avoid NPE when there's no DB connectionn(in that case the transaction remains null since the conn.Object() statement throws an exception and the rest of the code is not executed anymore)
                if (tx != null) {
                    tx.Rollback();//Reverting the DB to its original state
                }
                
            } finally {
                conn.Close();
            }
            
            //If the execution was successfull the number of affected rows is returned, otherwise the method returns -1 which means that the update operation failed
            if (executionResult != 0) {
                return executionResult;
            }

            return -1;

        }

        public static int updateData(MySqlCommand command) {
            int executionResult = -1;//The default value for the execution result

            //Creates connection and assigns it to the provided command
            MySqlConnection conn = getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);
            command.Connection = conn;

            try {
                //Opens the connection
                conn.Open();
                //Executes the command
                executionResult = command.ExecuteNonQuery();


            } catch (MySqlException ex) {
                //Retrieving the error code
                int errorCode = ex.Number;
                //The message is composed based on the error code returned (in order to improve the error understanding for the end user)
                String message;
                if (errorCode == 1042) {
                    message = "Unable to connect to the database! Please check the connection and try again.";
                } else {
                    message = ex.Message;
                }

                MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);

            } finally {
                //Closes the connection irrespective of the execution result
                conn.Close();
            }

            return executionResult;
        }


        //CHANGE!!!!
        //public static int deleteData(MySqlCommand command, DataTable sourceDataTable) {
        //    MySqlConnection conn = getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);

        //    command.Connection = conn;
        //    conn.Open();

        //    MySqlTransaction tx = conn.BeginTransaction();
        //    command.Transaction = tx;

        //    MySqlDataAdapter dataAdapter = getDataAdapter(command);
        //    MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

        //    int executionResult = -1;
        //    try {

        //        executionResult = dataAdapter.Update(sourceDataTable);
        //        sourceDataTable.AcceptChanges();
        //        tx.Commit();

        //    }catch(MySqlException ex) {
        //        MessageBox.Show(ex.Message,"DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        tx.Rollback();

        //    } catch(DBConcurrencyException ex) {
        //        MessageBox.Show(ex.Message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        tx.Rollback();

        //    } finally {
        //       conn.Close();
        //    }

        //    return executionResult;

        //}

        public static int deleteData(MySqlCommand command, DataTable sourceDataTable) {
            MySqlConnection conn = getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);

            command.Connection = conn;

            MySqlTransaction tx = null;
      
            MySqlDataAdapter dataAdapter = getDataAdapter(command);
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

            int executionResult = -1;
            try {
                conn.Open();

                tx = conn.BeginTransaction();
                command.Transaction = tx;

                executionResult = dataAdapter.Update(sourceDataTable);
                sourceDataTable.AcceptChanges();
                tx.Commit();

            } catch (MySqlException ex) {
                //Retrieving the error code
                int errorCode = ex.Number;
                //The message is composed based on the error code returned (in order to improve the error understanding for the end user)
                String message;
                if (errorCode == 1042) {
                    message = "Unable to connect to the database! Please check the connection and try again.";
                } else {
                    message = ex.Message;
                }
                
                MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (tx != null) {
                    tx.Rollback();
                }
                
            } catch (DBConcurrencyException ex) {
                MessageBox.Show(ex.Message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
