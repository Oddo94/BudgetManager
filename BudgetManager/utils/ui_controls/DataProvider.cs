using BudgetManager.utils.enums;
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
            DEBTOR_COMBOBOX,
            SAVING_ACCOUNT_COMBOBOX,
            INTEREST_TYPE_COMBOBOX,
            PAYMENT_TYPE_COMBOBOX
        }

        //SQL queries used for selecting the values that fill comboboxes
        private String sqlStatementSelectIncomeTypes = @"SELECT typeName
                                                         FROM income_types
                                                         ORDER BY
	                                                        CASE
		                                                        WHEN typeName = 'Salary' THEN 1
		                                                        WHEN typeName = 'Meal tickets refund' THEN 2
		                                                        WHEN typeName = 'Goods sale' THEN 3
		                                                        WHEN typeName = 'Other' THEN 4
		                                                        ELSE 5
	                                                        END;";

        private String sqlStatementSelectExpenseTypes = @"SELECT categoryName FROM expense_types ORDER BY categoryName";

        private String sqlStatementSelectCreditors = @"SELECT crs.creditorName
                FROM users usr
                INNER JOIN users_creditors uc ON usr.userID = uc.user_ID
                INNER JOIN creditors crs ON uc.creditor_ID = crs.creditorID
                WHERE uc.user_ID = @paramUserID
                ORDER BY crs.creditorName;";

        private String sqlStatementSelectDebtors = @"SELECT dbs.debtorName 
                FROM users_debtors ud
                INNER JOIN users usr ON usr.userID = ud.user_ID
                INNER JOIN debtors dbs ON dbs.debtorID = ud.debtor_ID
                WHERE ud.user_ID = @paramUserID
                ORDER BY dbs.debtorName;";

        private String sqlStatementSelectSavingAccounts = @"SELECT sa.accountName 
                FROM saving_accounts sa 
                INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID 
                WHERE sa.user_ID = @paramUserID
                ORDER BY sa.accountName";

        private String sqlStatementSelectInterestTypes = @"SELECT typeName FROM interest_types ORDER BY typeName";

        private String sqlStatementSelectPaymentTypes = @"SELECT typeName FROM interest_payment_type ORDER BY typeName";

        //Default method for filling comboboxes with data
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

                case ComboBoxType.INTEREST_TYPE_COMBOBOX:
                    retrievedData = retrieveData(sqlStatementSelectInterestTypes);
                    Guard.notNull(retrievedData, "DataTable");

                    targetComboBox.DataSource = retrievedData;
                    targetComboBox.DisplayMember = "typeName";
                    break;

                case ComboBoxType.PAYMENT_TYPE_COMBOBOX:
                    retrievedData = retrieveData(sqlStatementSelectPaymentTypes);
                    Guard.notNull(retrievedData, "DataTable");

                    targetComboBox.DataSource = retrievedData;
                    targetComboBox.DisplayMember = "typeName";
                    break;

                default:
                    break;
            }
        }

        //Special method for filling the saving account combobox (needs to be filled with accounts of different type based on the necessity, hence the additional logic)
        public void fillSavingAccountsComboBox(ComboBox targetComboBox, AccountType savingAccountType, int userID) {
            Guard.notNull(targetComboBox, "ComboBox");

            DataTable retrievedData = retrieveData(sqlStatementSelectSavingAccounts, userID);
            Guard.notNull(retrievedData, "DataTable");

            targetComboBox.DisplayMember = "accountName";
            if (savingAccountType == AccountType.DEFAULT_ACCOUNT || savingAccountType == AccountType.CUSTOM_ACCOUNT) {
                //Filters the account list based on the serach criterion(default/custom saving accounts only)
                DataTable filteredAccounts = filterAccountList(savingAccountType, retrievedData);
                targetComboBox.DataSource = filteredAccounts;
            } else {
                //Inserts the entire account list regardless of the account types contained
                targetComboBox.DataSource = retrievedData;
            }

        }

        //Methods that filters the saving account list based on the account type
        public DataTable filterAccountList(AccountType searchedType, DataTable accountDataSource) {
            String accountName = null;
            //Retrieves the original number of records so that the loop will be executed by the correct number of times (otherwise only some accounts will be removed)
            int originalNumberOfRecords = accountDataSource.Rows.Count;

            if (searchedType == AccountType.DEFAULT_ACCOUNT) {
                for (int i = originalNumberOfRecords - 1; i >= 0; i--) {
                    accountName = accountDataSource.Rows[i].ItemArray[0].ToString();

                    //Deletes all custom saving accounts from the list of retrieved accounts
                    if (!"SYSTEM_DEFINED_SAVING_ACCOUNT".Equals(accountName)) {
                        accountDataSource.Rows.RemoveAt(i);
                    }
                }
            } else if (searchedType == AccountType.CUSTOM_ACCOUNT) {
                for (int i = originalNumberOfRecords - 1; i >= 0; i--) {
                    accountName = accountDataSource.Rows[i].ItemArray[0].ToString();

                    //Deletes all default saving accounts from the list of retrieved accounts
                    if ("SYSTEM_DEFINED_SAVING_ACCOUNT".Equals(accountName)) {
                        accountDataSource.Rows.RemoveAt(i);
                    }
                }
            }

            return accountDataSource;
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
