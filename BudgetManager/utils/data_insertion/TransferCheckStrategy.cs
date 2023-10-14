using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.mvc.models.dto;
using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using BudgetManager.mvc.models;

namespace BudgetManager.non_mvc {
    class TransferCheckStrategy : DataInsertionCheckStrategy {
        private String accountBalanceCheckProcedure = "can_perform_requested_transfer";
    
        public DataCheckResponse performCheck(QueryData inputData, string selectedItemName, int valueToInsert) {
            DataCheckResponse dataCheckResponse = new DataCheckResponse();

            if (checkAvailableBalance(inputData, valueToInsert) == 0) {
                dataCheckResponse.ExecutionResult = 0;
                dataCheckResponse.SuccessMessage = "The transfer can be performed.";

            } else {
                dataCheckResponse.ExecutionResult = -1;
                dataCheckResponse.ErrorMessage = "The specified transfer value is higher than the currently available account balance! Please specify a value lower or equal to the account balance and try again.";
            }

            return dataCheckResponse;

        }

        //Method that checks if the transferred amount is lower/equal to the balance of the specified account, using a database stored procedure
        //The stored procedure returns 1 (true) if the transfer can be performed and 0 (false) otherwise
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
            //Transforms the procedure output(1-true/0-false to 0-true/-1-false which is the return value convention inside the app) 
            if (checkResult == 1) {
                return 0;
            } else {
                return -1;
            }

        }

        public DataCheckResponse performCheck(QueryData paramContainer, String selectedItemName, double valueToInsert) {
            throw new NotImplementedException();
        }

        public DataCheckResponse performCheck() {
            throw new NotImplementedException();
        }
    }
}
