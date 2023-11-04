using BudgetManager.mvc.models;
using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils.data_insertion {
    internal class GeneralAccountBalanceCheckStrategy : DataInsertionCheckStrategy {
        
        public DataCheckResponse performCheck(QueryData inputData, string selectedItemName, double valueToInsert) {
            String accountName = inputData.ItemName;
            int userID = inputData.UserID;

            DataCheckResponse dataCheckResponse = new DataCheckResponse();
            AccountUtils accountUtils = new AccountUtils();
            double currentAccountBalance = accountUtils.getSavingAccountCurrentBalance(accountName, userID);

            if (currentAccountBalance >= valueToInsert) {
                //return 0;
                dataCheckResponse.ExecutionResult = 0;
                dataCheckResponse.SuccessMessage = String.Format("The account balance is higher than the amount of the inserted {0}", selectedItemName);
            } else {
                dataCheckResponse.ExecutionResult = -1;
                dataCheckResponse.ErrorMessage = String.Format("The {0} value is greater than the available balance. Please check the account balance and/or the inserted value and try again.", selectedItemName);
            }

            //String errorMessage = String.Format("The {0} value is greater than the available balance. Please check the account balance and/or the inserted value and try again.", selectedItemName);
            //MessageBox.Show(errorMessage, "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //return -1;

            return dataCheckResponse;
        }

        public DataCheckResponse performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert) {
            throw new NotImplementedException();
        }

        public DataCheckResponse performCheck() {
            throw new NotImplementedException();
        }

       
    }
}
