using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.mvc.models.dto;
using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    class TransferCheckStrategy : DataInsertionCheckStrategy {
        private String accountBalanceCheckProcedure = "can_perform_requested_transfer";
    
        public int performCheck(QueryData inputData, string selectedItemName, int valueToInsert) {
            int balanceCheckResult = checkAvailableBalance(inputData, valueToInsert);

            return balanceCheckResult;

        }

        //Method that checks if the transferred amount is lower/equal to the balance of the specified account, using a database stored procedure
        private int checkAvailableBalance(QueryData inputData, int transferValue) {
            MySqlParameter checkResultOutput = null;
            MySqlParameter accountBalanceOutput = null;

            try {
                //Creates database connection
                MySqlConnection conn = DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);

                //Creates command used for calling the stored procedure
                MySqlCommand balanceCheckCommand = new MySqlCommand(accountBalanceCheckProcedure, conn);
                balanceCheckCommand.CommandType = System.Data.CommandType.StoredProcedure;

                //Defines the input parameters
                balanceCheckCommand.Parameters.Add(new MySqlParameter("p_account_ID", inputData.SourceAccountID));
                balanceCheckCommand.Parameters.Add(new MySqlParameter("p_transfer_value", transferValue));

                //Defines the output parameters
                checkResultOutput = new MySqlParameter("p_result", MySqlDbType.Int32);
                checkResultOutput.Direction = System.Data.ParameterDirection.Output;

                accountBalanceOutput = new MySqlParameter("p_account_balance", MySqlDbType.Int32);
                accountBalanceOutput.Direction = System.Data.ParameterDirection.Output;

                //Adds parameters to the command
                balanceCheckCommand.Parameters.Add(checkResultOutput);
                balanceCheckCommand.Parameters.Add(accountBalanceOutput);

                conn.Open();

                balanceCheckCommand.ExecuteNonQuery();

                conn.Close();


            } catch (MySqlException ex) {
                String errorMessage = String.Format("Cannot perform the saving account balance check due to the following exception:\n{0}", ex.Message);
                Console.Error.WriteLine(errorMessage);
            }

            int checkResult = Convert.ToInt32(checkResultOutput.Value.ToString());

            //If the procedure returns the value 1 it means that the transfer can be performed otherwise the operation is not possible
            if (checkResult == 1) {
                return 0;
            } else {
                return -1;
            }

        }

        public int performCheck(QueryData paramContainer, String selectedItemName, double valueToInsert) {
            throw new NotImplementedException();
        }

        public int performCheck() {
            throw new NotImplementedException();
        }
    }
}
