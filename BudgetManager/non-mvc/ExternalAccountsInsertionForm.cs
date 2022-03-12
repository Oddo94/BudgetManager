using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BudgetManager.utils;


namespace BudgetManager.non_mvc {
    public partial class ExternalAccountsInsertionForm : Form {

        private String sqlStatementRetrieveAccountTypes = @"SELECT typeName FROM saving_account_types WHERE typeName != 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'";
        private String sqlStatementRetrieveAvailableBanks = @"SELECT bankName FROM banks";
        private String sqlStatementRetrieveAvailableCurrencies = @"SELECT currencyName FROM currencies";

        public ExternalAccountsInsertionForm() {          
            InitializeComponent();
            fillComboBoxesWithData();
        }

       
        private void fillComboBoxesWithData() {
            DataTable accountTypeComboBoxData = retrieveComboBoxData(sqlStatementRetrieveAccountTypes);          
            DataTable accountCurrencyComboBoxData = retrieveComboBoxData(sqlStatementRetrieveAvailableCurrencies);
            DataTable accountBankComboBoxData = retrieveComboBoxData(sqlStatementRetrieveAvailableBanks);

            fillComboBox(accountTypeComboBox, accountTypeComboBoxData, "typeName");
            fillComboBox(accountCurrencyComboBox, accountCurrencyComboBoxData, "currencyName");
            fillComboBox(accountBankComboBox, accountBankComboBoxData,"bankName");

            accountTypeComboBox.SelectedIndex = -1;
            accountCurrencyComboBox.SelectedIndex = -1;
            accountBankComboBox.SelectedIndex = -1;
        }




        private void fillComboBox(ComboBox targetComboBox, DataTable sourceDataTable, String displayMember) {
            Guard.notNull(targetComboBox, "comboBox");
            Guard.notNull(sourceDataTable, "DataTable combobox source");
            Guard.notNull(displayMember, "column name");

            targetComboBox.DataSource = sourceDataTable;
            targetComboBox.DisplayMember = displayMember;

        }

        private DataTable retrieveComboBoxData(String sqlStatement) {
            Guard.notNull(sqlStatement, "SQL statement");

            MySqlCommand comboBoxDataRetrievalCommand = new MySqlCommand(sqlStatement);
            DataTable comboBoxDataTable = DBConnectionManager.getData(comboBoxDataRetrievalCommand);

            return comboBoxDataTable;
        }
    }
}
