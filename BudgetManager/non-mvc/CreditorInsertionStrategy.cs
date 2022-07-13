using BudgetManager.mvc.models.dto;
using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    class CreditorInsertionStrategy : DataInsertionStrategy {
        //SQL query for checking if the entered creditor name exists in the database
        private String sqlStatementCheckCreditorExistence = @"SELECT creditorName FROM creditors WHERE creditorName = @paramCreditorName";

        //SQL query for checking the existence of a creditor in the current user creditor list
        private String sqlStatementCheckCreditorExistenceInUserList = @"SELECT user_ID, creditor_ID FROM users_creditors WHERE user_ID = @paramUserID AND creditor_ID = @paramCreditorID";

        //SQL query for selecting the ID that will be used in the creditor insertion command
        private String sqlStatementSelectCreditorID = @"SELECT creditorID FROM creditors WHERE creditorName = @paramTypeName";

        //SQL query for inserting a new creditor into the database
        private String sqlStatementInsertCreditor = @"INSERT INTO creditors(creditorName) VALUES(@paramCreditorName)";

        //SQL query for inserting assiging a creditor to a user in the users_creditors link table of the database
        private String sqlStatementInsertCreditorID = @"INSERT INTO users_creditors(user_ID, creditor_ID) VALUES(@paramUserID, @paramCreditorID)";


        public int execute(QueryData paramContainer) {
            int executionResult = -1;
            //Checks if the entered creditor name exists in the database
            MySqlCommand creditorSelectionCommand = new MySqlCommand(sqlStatementCheckCreditorExistence);
            creditorSelectionCommand.Parameters.AddWithValue("@paramCreditorName", paramContainer.CreditorName);
            if (entryIsPresent(creditorSelectionCommand, paramContainer.CreditorName)) {
                DialogResult userChoice = MessageBox.Show("The provided creditor name already exists. Do you want to add it to your creditors list?", "Data insertion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (userChoice == DialogResult.Yes) {
                    //Checks if the creditor is already present in the current user creditors' list
                    MySqlCommand creditorPresenceInListCommand = new MySqlCommand(sqlStatementCheckCreditorExistenceInUserList);
                    creditorPresenceInListCommand.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
                    creditorPresenceInListCommand.Parameters.AddWithValue("@paramCreditorID", DataInsertionUtils.getID(sqlStatementSelectCreditorID, paramContainer.CreditorName));//Looks for the id of the creditor whose name was inserted
                    if (isPresentInUserCreditorList(creditorPresenceInListCommand)) {
                        MessageBox.Show("The provided creditor is already present in your creditor list and cannot be assigned again! Please enter a different creditor", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return executionResult;
                    } else {
                        //If the creditor aleady exists but is assigned to the current user a new entry will be created in the users_creditors table of the database
                        MySqlCommand creditorIDInsertCommandForExistingEntry = new MySqlCommand(sqlStatementInsertCreditorID);
                        creditorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
                        creditorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramCreditorID", DataInsertionUtils.getID(sqlStatementSelectCreditorID, paramContainer.CreditorName));
                        executionResult = DBConnectionManager.insertData(creditorIDInsertCommandForExistingEntry);

                    }

                } else {
                    //If the user option is 'No' we return from the method
                    return executionResult;
                }
            } else {
                //Inserting a new creditor in the creditors table of the database
                MySqlCommand creditorInsertCommand = new MySqlCommand(sqlStatementInsertCreditor);
                creditorInsertCommand.Parameters.AddWithValue("@paramCreditorName", paramContainer.CreditorName);
                executionResult = DBConnectionManager.insertData(creditorInsertCommand);

                //Checks if the insertion in the creditor table of the database was successfull and if not returns the value of the executionResult(which will be -1) so that the user will know that something went wrong during the process
                if (executionResult == -1) {
                    return executionResult;
                }

                //Inserting the ID of the newly created creditor in the users_creditors table of the database
                MySqlCommand creditorIDInsertCommand = new MySqlCommand(sqlStatementInsertCreditorID);
                creditorIDInsertCommand.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
                creditorIDInsertCommand.Parameters.AddWithValue("@paramCreditorID", DataInsertionUtils.getID(sqlStatementSelectCreditorID, paramContainer.CreditorName));
                executionResult = DBConnectionManager.insertData(creditorIDInsertCommand);
            }

            return executionResult;
        }

        //Method for checking if the specififed creditor is present in the database
        private bool entryIsPresent(MySqlCommand command, String entryName) {
            //Executes the data retrieval command using the name of the specified creditor        
            DataTable entryDataTable = DBConnectionManager.getData(command);

            if (entryDataTable != null) {
                if (entryDataTable.Rows.Count > 0) {
                    for (int i = 0; i < entryDataTable.Rows.Count; i++) {
                        //Checks if the name of the creditor that was obtained after the execution of the command is the same as the one that the users tries to insert(case insensitive string comparison)
                        if (entryName.Equals(entryDataTable.Rows[i].ItemArray[0].ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        //Method for checking if the specified creditor is present in the user's creditor list
        private bool isPresentInUserCreditorList(MySqlCommand command) {
            DataTable creditorListPresenceTable = DBConnectionManager.getData(command);

            if (creditorListPresenceTable != null && creditorListPresenceTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }

        public int execute(IDataInsertionDTO dataInsertionDTO) {
            throw new NotImplementedException();
        }
    }
}
