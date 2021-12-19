using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    class DataProvider {
        //Enum for specifying the ComboBox type 
        public enum ComboBoxType {
            INCOME_TYPE_COMBOBOX,
            EXPENSE_TYPE_COMBOBOX,
            CREDITOR_COMBOBOX,
            DEBTOR_COMBOBOX
        }

        //SQL queries used for selecting the values that fill comboboxes
        private String sqlStatementSelectIncomeTypes = @"SELECT typeName FROM income_types";
        private String sqlStatementSelectExpenseTypes = @"SELECT categoryName FROM expense_types";
        private String sqlStatementSelectCreditors = @"SELECT creditorName
                FROM users INNER JOIN users_creditors ON users.userID = users_creditors.user_ID
                INNER JOIN creditors ON users_creditors.creditor_ID = creditors.creditorID
                WHERE users_creditors.user_ID = @paramUserID";
        private String sqlStatementSelectDebtors = @"SELECT debtorName FROM users_debtors
                INNER JOIN users ON users.userID = users_debtors.user_ID
                INNER JOIN debtors ON debtors.debtorID = users_debtors.debtor_ID
                WHERE users_debtors.user_ID = 3";

        public void fillComboBox(ComboBox targetComboBox, ComboBoxType comboBoxType, int userID) {
            Guard.notNull(targetComboBox, "ComboBox");

            DataTable retrievedData = new DataTable();
            switch (comboBoxType) {
                case ComboBoxType.CREDITOR_COMBOBOX:
                    retrievedData = retrieveData(sqlStatementSelectCreditors, userID);
                    Guard.notNull(retrievedData, "DataTable");

                    targetComboBox.DataSource = retrievedData;
                    targetComboBox.DisplayMember = "creditorName";
                    break;

                case ComboBoxType.DEBTOR_COMBOBOX:
                    retrievedData = retrieveData(sqlStatementSelectDebtors, userID);
                    Guard.notNull(retrievedData, "DataTable");

                    targetComboBox.DataSource = retrievedData;
                    targetComboBox.DisplayMember = "debtorName";
                    break;

                case ComboBoxType.EXPENSE_TYPE_COMBOBOX:
                    retrievedData = retrieveData(sqlStatementSelectExpenseTypes);
                    Guard.notNull(retrievedData, "DataTable");

                    targetComboBox.DataSource = retrievedData;
                    targetComboBox.DisplayMember = "categoryName";
                    break;

                case ComboBoxType.INCOME_TYPE_COMBOBOX:
                    retrievedData = retrieveData(sqlStatementSelectIncomeTypes);
                    Guard.notNull(retrievedData, "DataTable");

                    targetComboBox.DataSource = retrievedData;
                    targetComboBox.DisplayMember = "typeName";
                    break;

                default:
                    break;
            }
        }



        private DataTable retrieveData(String sqlStatement, int userID) {
            Guard.notNull(sqlStatement, "data retrieval sqlStatement");

            MySqlCommand retrieveDataCommand = new MySqlCommand(sqlStatement);
            retrieveDataCommand.Parameters.AddWithValue("@paramUserID", userID);

            DataTable retrievedData = DBConnectionManager.getData(retrieveDataCommand);

            return retrievedData;
        }

        private DataTable retrieveData(String sqlStatement) {
            Guard.notNull(sqlStatement, "data retrieval sqlStatement");

            MySqlCommand retrieveDataCommand = new MySqlCommand(sqlStatement);

            DataTable retrievedData = DBConnectionManager.getData(retrieveDataCommand);

            return retrievedData;
        }
    }
}
