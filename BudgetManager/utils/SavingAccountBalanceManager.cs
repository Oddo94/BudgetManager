using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    class SavingAccountBalanceManager {
        private int userID;
        private int balanceRecordMonth;
        private int balanceRecordYear;
        private int value;

        //SQL statement for checking balance record existence
        private String sqlStatementCheckRecordExistence = @"SELECT * FROM saving_account_balance WHERE user_ID = @paramID AND month = @paramMonth AND year = @paramYear";

        //SQL statements for inserting data into the saving_account_balance table
        private String sqlStatementInsertBalanceRecord = @"INSERT INTO saving_account_balance(user_ID, value, month, year) VALUES(@paramID, @paramValue, @paramMonth, @paramYear)";

        public SavingAccountBalanceManager(int userID, int balanceRecordMonth, int balanceRecordYear, int value) {
            this.userID = userID;
            this.balanceRecordMonth = balanceRecordMonth;
            this.balanceRecordYear = balanceRecordYear;
            this.value = value;
        }


        private void createBalanceRecord() {
            QueryData paramContainer = new QueryData.Builder(userID).addItemValue(value).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();

            MySqlCommand createBalanceRecordCommand = SQLCommandBuilder.getBalanceRecordInsertionCommand(sqlStatementInsertBalanceRecord, paramContainer);

            int executionResult = DBConnectionManager.insertData(createBalanceRecordCommand);

            if (executionResult == -1) {
                MessageBox.Show("The saving account balance could not be created!", "Insert data form", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

        }




        //Checks if there is any existing record for the specified month and year for the current user
        private bool hasBalanceRecord() {
            if(balanceRecordMonth <= 0 || balanceRecordMonth > 12) {
                return false;
            }

            if(balanceRecordYear <= 0) {
                return false;
            }

            QueryData paramContainer = new QueryData.Builder(userID).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();
            MySqlCommand recordExistenceCheckCommand = SQLCommandBuilder.getSingleMonthCommand(sqlStatementCheckRecordExistence, paramContainer);

            DataTable resultDataTable = DBConnectionManager.getData(recordExistenceCheckCommand);

            if(resultDataTable != null && resultDataTable.Rows.Count == 1) {
                return true;
            }

            return false;
        }



    }
}
