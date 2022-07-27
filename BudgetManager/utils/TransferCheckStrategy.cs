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

            if(balanceCheckResult == -1) {
                MessageBox.Show(String.Format("The specified transfer value is higher than the currently available account balance! Please specify a value lower or equal to the account balance and try again.", selectedItemName.ToLower()), "Transfer check", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }

            return balanceCheckResult;

        }

        private int checkAvailableBalance(QueryData inputData, int transferValue) {
            MySqlParameter checkResultOutput = null;
            MySqlParameter accountBalanceOutput = null;

            try {
                MySqlConnection conn = DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING);

                MySqlCommand balanceCheckCommand = new MySqlCommand(accountBalanceCheckProcedure, conn);
                balanceCheckCommand.CommandType = System.Data.CommandType.StoredProcedure;

                balanceCheckCommand.Parameters.Add(new MySqlParameter("p_account_ID", inputData.SourceAccountID));
                balanceCheckCommand.Parameters.Add(new MySqlParameter("p_transfer_value", transferValue));

                checkResultOutput = new MySqlParameter("p_result", MySqlDbType.Int32);
                checkResultOutput.Direction = System.Data.ParameterDirection.Output;

                accountBalanceOutput = new MySqlParameter("p_account_balance", MySqlDbType.Int32);
                accountBalanceOutput.Direction = System.Data.ParameterDirection.Output;

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
    }
}
