using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    class DebtorInsertionStrategy : DataInsertionStrategy {
        //SQL query for checking if the entered debtor name exists in the database
        private String sqlStatementCheckDebtorExistence = "SELECT debtorName FROM debtors WHERE debtorName = @paramDebtorName";

        //SQL query for checking the existence of a debtor in the current user creditor list
        private String sqlStatementCheckDebtorExistenceInUserList = @"SELECT user_ID, debtor_ID FROM users_debtors WHERE user_ID = @paramUserID AND debtor_ID = @paramDebtorID";

        //SQL query for selecting the ID that will be used in the debtor insertion command
        private String sqlStatementSelectDebtorID = @"SELECT debtorID FROM debtors WHERE debtorName = @paramTypeName";

        //SQL query for inserting a new debtor into the database
        private String sqlStatementInsertDebtor = @"INSERT INTO debtors(debtorName) VALUES(@paramDebtorName)";

        //SQL query for assigning a debtor to a user in the users_debtors link table of the database
        private String sqlStatementInsertDebtorID = @"INSERT INTO users_debtors(user_ID, debtor_ID) VALUES(@paramUserID, @paramDebtorID)";


        public int execute(QueryData paramContainer) {
            int executionResult = -1;
            //Checks if the entered debtor name exists in the database
            MySqlCommand debtorSelectionCommand = new MySqlCommand(sqlStatementCheckDebtorExistence);
            debtorSelectionCommand.Parameters.AddWithValue("@paramDebtorName", paramContainer.DebtorName);
            if (DataInsertionUtils.entryIsPresent(debtorSelectionCommand, paramContainer.DebtorName)) {
                DialogResult userChoice = MessageBox.Show("The provided debtor name already exists. Do you want to add it to your debtors list?", "Data insertion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (userChoice == DialogResult.Yes) {
                    //Checks if the debtor is already present in the current user debtor's list
                    MySqlCommand debtorPresenceInListCommand = new MySqlCommand(sqlStatementCheckDebtorExistenceInUserList);
                    debtorPresenceInListCommand.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
                    debtorPresenceInListCommand.Parameters.AddWithValue("@paramDebtorID", DataInsertionUtils.getID(sqlStatementSelectDebtorID, paramContainer.DebtorName));//Looks for the id of the debtor whose name was inserted
                    if (DataInsertionUtils.isAssignedToCurrentUser(debtorPresenceInListCommand)) {
                        MessageBox.Show("The provided debtor is already present in your debtor list and cannot be assigned again! Please enter a different debtor", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return executionResult;
                    } else {
                        //If the debtor already exists but it's not assigned to the current user a new entry will be created in the users_debtors table of the database
                        MySqlCommand debtorIDInsertCommandForExistingEntry = new MySqlCommand(sqlStatementInsertDebtorID);
                        debtorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
                        debtorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramDebtorID", DataInsertionUtils.getID(sqlStatementSelectDebtorID, paramContainer.DebtorName));
                        executionResult = DBConnectionManager.insertData(debtorIDInsertCommandForExistingEntry);

                    }

                } else {
                    //If the user option is 'No' we return from the method
                    return executionResult;
                }
            } else {
                //Inserting a new debtor in the debtors table of the database
                MySqlCommand debtorInsertCommand = new MySqlCommand(sqlStatementInsertDebtor);
                debtorInsertCommand.Parameters.AddWithValue("@paramDebtorName", paramContainer.DebtorName);
                executionResult = DBConnectionManager.insertData(debtorInsertCommand);

                //Checks if the insertion in the debtortable of the database was successfull and if not returns the value of the executionResult(which will be -1) so that the user will know that something went wrong during the process
                if (executionResult == -1) {
                    return executionResult;
                }

                //Inserting the ID of the newly created creditor in the users_creditors table of the database
                MySqlCommand debtorIDInsertCommand = new MySqlCommand(sqlStatementInsertDebtorID);
                debtorIDInsertCommand.Parameters.AddWithValue("@paramUserID", paramContainer.UserID);
                debtorIDInsertCommand.Parameters.AddWithValue("@paramDebtorID", DataInsertionUtils.getID(sqlStatementSelectDebtorID, paramContainer.DebtorName));
                executionResult = DBConnectionManager.insertData(debtorIDInsertCommand);
            }

            return executionResult;
        }
    }
}
