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
    }
}
