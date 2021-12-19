using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils {
    //Utility class that holds the utility metods needed for data insertion
    class DataInsertionUtils {

        //Method for retrieving the ID of an item (it works for elements having multiple types such as expenses, incomes as well as other elements such as creditors)
        public static int getID(String sqlStatement, String itemName) {
            Guard.notNull(sqlStatement, "SQL statement");

            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", itemName);

            DataTable typeIDTable = DBConnectionManager.getData(getTypeIDCommand);

            if (typeIDTable != null && typeIDTable.Rows.Count == 1) {
                int typeID = Convert.ToInt32(typeIDTable.Rows[0].ItemArray[0]);
                return typeID;
            }

            return -1;
        }

        //Method for checking if the specififed creditor/debtor is present in the database
        public static bool entryIsPresent(MySqlCommand command, String entryName) {
            //Executes the data retrieval command using the name of the specified creditor/debtor        
            DataTable entryDataTable = DBConnectionManager.getData(command);

            if (entryDataTable != null) {
                if (entryDataTable.Rows.Count > 0) {
                    for (int i = 0; i < entryDataTable.Rows.Count; i++) {
                        //Checks if the name of the creditor/debtor that was obtained after the execution of the command is the same as the one that the users tries to insert(case insensitive string comparison)
                        if (entryName.Equals(entryDataTable.Rows[i].ItemArray[0].ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        //Method for checking if the specified creditor/debtor is present in the user's creditor/debtor list(users_creditors or user_debtors table of the database)
        public static bool isAssignedToCurrentUser(MySqlCommand command) {
            DataTable assignmentListTable = DBConnectionManager.getData(command);

            if (assignmentListTable != null && assignmentListTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }
    }
}
