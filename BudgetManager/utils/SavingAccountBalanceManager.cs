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
        private DateTime date;        

        //SQL statement for checking balance record existence
        private String sqlStatementCheckRecordExistence = @"SELECT * FROM saving_account_balance WHERE user_ID = @paramID AND month = @paramMonth AND year = @paramYear";

        //SQL statements for inserting data into the saving_account_balance table
        private String sqlStatementInsertBalanceRecord = @"INSERT INTO saving_account_balance(user_ID, recordName, value, month, year) VALUES(@paramID, @paramRecordName, @paramValue, @paramMonth, @paramYear)";

        //SQL statements for updating data contained in the saving_account_balance table
        private String sqlStatementUpdateBalanceRecord = @"UPDATE saving_account_balance SET recordName = @paramRecordName, value = @paramValue WHERE userID = @paramID AND (month = @paramMonth AND year = @paramYear)";


        public SavingAccountBalanceManager(int userID, int balanceRecordMonth, int balanceRecordYear, int value, DateTime date) {
            this.userID = userID;
            this.balanceRecordMonth = balanceRecordMonth;
            this.balanceRecordYear = balanceRecordYear;
            this.value = value;
            this.date = date;           
        }


        public int createBalanceRecord() {
            String recordName = createRecordName(date);
            QueryData paramContainer = new QueryData.Builder(userID).addItemName(recordName).addItemValue(value).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();

            MySqlCommand createBalanceRecordCommand = SQLCommandBuilder.getBalanceRecordInsertUpdateCommand(sqlStatementInsertBalanceRecord, paramContainer);

            int executionResult = DBConnectionManager.insertData(createBalanceRecordCommand);

            //if (executionResult == -1) {
            //    MessageBox.Show("The saving account balance could not be created!", "Insert data form", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            //}

            return executionResult;

        }

        public int updateBalanceRecord() {
            String recordName = createRecordName(date);
            QueryData paramContainer = new QueryData.Builder(userID).addItemName(recordName).addItemValue(value).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();

            MySqlCommand createBalanceRecordUpdateCommand = SQLCommandBuilder.getBalanceRecordInsertUpdateCommand(sqlStatementUpdateBalanceRecord, paramContainer);

            int executionResult = DBConnectionManager.updateData(createBalanceRecordUpdateCommand);

            return executionResult;
        }

        //Method for creating the record name
        private String createRecordName(DateTime updateDate) {
            if (updateDate == null) {
                return null;
            }

            //Creates the string representtatio of the provided DateTime object
            String recordDate = updateDate.ToString("yyyy-MM-dd");
            //Sets the value of the fixed size component of the record name       
            String fixedNameComponent = "balance_record_";

            String finalRecordName = fixedNameComponent + recordDate;

            return finalRecordName;
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
