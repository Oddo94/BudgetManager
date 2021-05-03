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
        private DateTime date;//Used for creating the record name when a new balance record is inserted or set the saving account expense date when a new record is inserted

        //SQL query for checking balance record existence
        private String sqlStatementCheckRecordExistence = @"SELECT * FROM saving_account_balance WHERE user_ID = @paramID AND month = @paramMonth AND year = @paramYear";
   
        //SQL query for inserting data
        private String sqlStatementCreateBalanceRecord = @"INSERT INTO saving_account_balance(user_ID, recordName, value, month, year) VALUES(@paramID, @paramRecordName, @paramValue, @paramMonth, @paramYear)";
        private String sqlStatementCreateSavingAccountExpense = @"INSERT INTO saving_account_expenses(user_ID, name, type, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";

        //SQL query for updating data contained in the saving_account_balance table
        private String sqlStatementUpdateBalanceRecord = @"UPDATE saving_account_balance SET recordName = @paramRecordName, value = @paramValue WHERE user_ID = @paramID AND (month = @paramMonth AND year = @paramYear)";


        public SavingAccountBalanceManager(int userID, int balanceRecordMonth, int balanceRecordYear, DateTime date) {
            this.userID = userID;
            this.balanceRecordMonth = balanceRecordMonth;
            this.balanceRecordYear = balanceRecordYear;         
            this.date = date;
        }

        //Method for inserting a new record in the saving account balance record table
        public int createBalanceRecord(int recordValue) {
            String recordName = createRecordName(date);
            QueryData paramContainer = new QueryData.Builder(userID).addItemName(recordName).addItemValue(recordValue).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();

            MySqlCommand createBalanceRecordCommand = SQLCommandBuilder.getBalanceRecordInsertUpdateCommand(sqlStatementCreateBalanceRecord, paramContainer);

            int executionResult = DBConnectionManager.insertData(createBalanceRecordCommand);

            return executionResult;

        }

        //Method for inserting a new record in the saving account expenses table
        public int createSavingAccountExpenseRecord(String recordName, int recordValue, int typeID) {
            //Creates the SQL command for inserting a new record into the saving account expenses table
            MySqlCommand createSavingAccountExpenseCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementCreateSavingAccountExpense, userID, recordName, typeID, recordValue, date.ToString("yyyy-MM-dd"));

            int executionResult = DBConnectionManager.insertData(createSavingAccountExpenseCommand);

            return executionResult;
        }

        //Method for updating a record in the saving account balance table
        public int updateBalanceRecord(int recordValue) {
            String recordName = createRecordName(date);

            QueryData paramContainer = new QueryData.Builder(userID).addItemName(recordName).addItemValue(recordValue).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();

            MySqlCommand createBalanceRecordUpdateCommand = SQLCommandBuilder.getBalanceRecordInsertUpdateCommand(sqlStatementUpdateBalanceRecord, paramContainer);

            int executionResult = DBConnectionManager.updateData(createBalanceRecordUpdateCommand);

            return executionResult;
        }

        //Method for retrieving the record value from the saving account balance table
        public int getRecordValue() {
            int recordValue = -1;

            QueryData paramContainer = new QueryData.Builder(userID).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();
            MySqlCommand recordRetrievalCommand = SQLCommandBuilder.getSingleMonthCommand(sqlStatementCheckRecordExistence, paramContainer);

            DataTable resultDataTable = DBConnectionManager.getData(recordRetrievalCommand);

            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                Object valueObject = resultDataTable.Rows[0].ItemArray[3];

                recordValue = valueObject != DBNull.Value ? Convert.ToInt32(valueObject) : -1;
            }

            return recordValue;
        }

        //Method for creating the record name
        private String createRecordName(DateTime updateDate) {
            if (updateDate == null) {
                return null;
            }

            //Creates the string representation of the provided DateTime object
            String recordDate = updateDate.ToString("yyyy-MM-dd");
            //Sets the value of the fixed size component of the record name       
            String fixedNameComponent = "balance_record_";

            String finalRecordName = fixedNameComponent + recordDate;

            return finalRecordName;
        }


        //Checks if there is any existing record for the specified month and year for the current user
        public bool hasBalanceRecord() {
            if (balanceRecordMonth <= 0 || balanceRecordMonth > 12) {
                return false;
            }

            if (balanceRecordYear <= 0) {
                return false;
            }

            QueryData paramContainer = new QueryData.Builder(userID).addMonth(balanceRecordMonth).addYear(balanceRecordYear).build();
            MySqlCommand recordExistenceCheckCommand = SQLCommandBuilder.getSingleMonthCommand(sqlStatementCheckRecordExistence, paramContainer);

            DataTable resultDataTable = DBConnectionManager.getData(recordExistenceCheckCommand);

            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                return true;
            }

            return false;
        }
    }
}
