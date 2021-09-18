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

        //Method for retrieving the ID of budget item whch has multiple types(incomes, expenses, etc)
        public static int getID(String sqlStatement, String typeName) {
            Guard.notNull(sqlStatement, "SQL statement");

            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", typeName);

            DataTable typeIDTable = DBConnectionManager.getData(getTypeIDCommand);

            if (typeIDTable != null && typeIDTable.Rows.Count == 1) {
                int typeID = Convert.ToInt32(typeIDTable.Rows[0].ItemArray[0]);
                return typeID;
            }

            return -1;
        }
    }
}
