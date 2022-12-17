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
    //Utility class for managing the application connection to the database
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
            //Creates a new connection
            MySqlConnection conn = DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);

            //Assigning the connection to the command object
            command.Connection = conn;
           
            //Creating a DataAdapter based on the command object
            MySqlDataAdapter adp = DBConnectionManager.getDataAdapter(command);
           
            //Creating an empty DataTable
            DataTable dataTable = new DataTable();

            try {
                //Se deschide conexiunea si se umple cu date obiectul DataTable
                //Opening the connection and populating the DataTable object with data
                conn.Open();
                adp.Fill(dataTable);

            } catch (MySqlException ex) {               
                //If an exception is thrown then a MessageBox containing the execption or the custom message (for error code 1042) is displayed
                int errorCode = ex.Number;
                String message;
                if (errorCode == 1042) {
                     message = "Unable to connect to the database! Please check the connection and try again.";
                } else {
                     message = ex.Message;
                }          
                MessageBox.Show(message, "DBConnectionManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            } finally {
                //Closing the connection irrespective of the command execution result
                conn.Close();
            }
           
            return dataTable;
        }


        public static int insertData(MySqlCommand command) {
            int executionResult = 0;
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);
            command.Connection = conn;

            //Creating a new transaction (it is created here to allow it to be referneced in the catch block      
            MySqlTransaction tx = null;

            try {
                conn.Open();

                //The transaction is started uusing the previously created Connection object and it is assigned to the command
                tx = conn.BeginTransaction();
                command.Transaction = tx;

                //The number of affected rows after the execution of the SQL statement command is stored in this variable(if the number is greater than 0 is means that the command was executed successfully otherwise it means that it has failed)
                executionResult = command.ExecuteNonQuery();
                //The changes are submitted to the DB
                tx.Commit();
            } catch (MySqlException ex) {
                //Retrieving the error code
                int errorCode = ex.Number;
                //The message is composed based on the error code returned (in order to improve the error understanding for the end user)
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

        public static List<MySqlParameter> callDatabaseStoredProcedure(String procedureName, List<MySqlParameter> inputParameters, List<MySqlParameter> outputParameters) {
            if (procedureName == null) {
                throw new ArgumentNullException(procedureName, "The name of the stored procedure cannot be null!");
            }

            if (inputParameters == null) {
                throw new ArgumentNullException("input parameter list", "The input parameter list cannot be null!");
            }

            if (outputParameters == null) {
                throw new ArgumentNullException("output parameter list", "The output parameter list cannot be null!");
            }

            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);

            try {
                MySqlCommand procedureCallingCommand = new MySqlCommand(procedureName, conn);
                procedureCallingCommand.CommandType = System.Data.CommandType.StoredProcedure;

                //Adds the input parameters to the command object
                foreach (MySqlParameter param in inputParameters) {
                    param.Direction = System.Data.ParameterDirection.Input;
                    procedureCallingCommand.Parameters.Add(param);
                }

                //Sets the type for each output parameter and adds it to the command object
                foreach (MySqlParameter param in outputParameters) {
                    param.Direction = System.Data.ParameterDirection.Output;
                    procedureCallingCommand.Parameters.Add(param);
                }

                conn.Open();

                procedureCallingCommand.ExecuteNonQuery();

            } catch (MySqlException ex) {
                throw;
            } finally {
                conn.Close();
            }

            return outputParameters;

        }

        //Method for checking that the connection with the DB is up and running
        public static bool hasConnection() {
            //A new connection is created 
            MySqlConnection conn = getConnection(BUDGET_MANAGER_CONN_STRING);

            //If it can be opened then it means that everything is OK, otherwise and exception will be thrown and the false value will be returned
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
