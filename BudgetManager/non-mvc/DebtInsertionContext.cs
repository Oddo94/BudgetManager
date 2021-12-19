using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    class DebtInsertionStrategy : DataInsertionStrategy {

        //SQL query for inserting a new debt into the DB
        private String sqlStatementInsertDebt = @"INSERT INTO debts(user_ID, name, value, creditor_ID, date) VALUES(@paramID, @paramDebtName, @paramDebtValue, @paramCreditorID, @paramDebtDate)";

        //SQL query for retrieving the id of the selected creditor
        private String sqlStatementSelectCreditorID = @"SELECT creditorID FROM creditors WHERE creditorName = @paramTypeName";

        public int execute(QueryData paramContainer) {

            //Getting the necessary data
            int userID = paramContainer.UserID;
            String debtName = paramContainer.ItemName;
            int debtValue = paramContainer.ItemValue;
            int creditorID = DataInsertionUtils.getID(sqlStatementSelectCreditorID, paramContainer.CreditorName);
            String debtDate = paramContainer.ItemCreationDate;

            //Creating command for debt insertion
            MySqlCommand debtInsertionCommand = SQLCommandBuilder.getDebtInsertionCommand(sqlStatementInsertDebt, userID, debtName, debtValue, creditorID, debtDate);
            int executionResult = DBConnectionManager.insertData(debtInsertionCommand);

            return executionResult;
        }
    }
}
