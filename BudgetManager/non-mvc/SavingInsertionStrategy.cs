using BudgetManager.mvc.models.dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    class SavingInsertionStrategy : DataInsertionStrategy {

        //SQL query for inserting a new saving into the database
        private String sqlStatementInsertSaving = @"INSERT INTO savings(user_ID, name, value, date) VALUES(@paramID, @paramSavingName, @paramSavingValue, @paramSavingDate)";

        public int execute(QueryData paramContainer) {
            //Getting the necessary data
            int userID = paramContainer.UserID;
            String savingName = paramContainer.ItemName;
            int savingValue = paramContainer.ItemValue;
            String savingDate = paramContainer.ItemCreationDate;

            //Creating command for saving insertion
            MySqlCommand savingInsertionCommand = SQLCommandBuilder.getSavingInsertionCommand(sqlStatementInsertSaving, userID, savingName, savingValue, savingDate);
            int executionResult = DBConnectionManager.insertData(savingInsertionCommand);

            return executionResult;
        }

        public int execute(IDataInsertionDTO dataInsertionDTO) {
            throw new NotImplementedException();
        }
    }
}
