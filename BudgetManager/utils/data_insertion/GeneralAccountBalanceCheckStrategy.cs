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
        
        public int performCheck(QueryData inputData, string selectedItemName, double valueToInsert) {
            String accountName = inputData.ItemName;
            int userID = inputData.UserID;

            AccountUtils accountUtils = new AccountUtils();
            double currentAccountBalance = accountUtils.getSavingAccountCurrentBalance(accountName, userID);

            if (currentAccountBalance >= valueToInsert) {
                return 0;
            }

            String errorMessage = String.Format("The {0} value is greater than the available balance. Please check the account balance and/or the inserted value and try again.", selectedItemName);
            MessageBox.Show(errorMessage, "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            return -1;
        }

        public int performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert) {
            throw new NotImplementedException();
        }

        public int performCheck() {
            throw new NotImplementedException();
        }

       
    }
}
