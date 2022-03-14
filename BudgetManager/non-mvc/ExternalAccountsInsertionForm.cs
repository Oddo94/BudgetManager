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

        private int userID;    

        private String sqlStatementRetrieveAccountTypes = @"SELECT typeName FROM saving_account_types WHERE typeName != 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'";
        private String sqlStatementRetrieveAvailableBanks = @"SELECT bankName FROM banks";
        private String sqlStatementRetrieveAvailableCurrencies = @"SELECT currencyName FROM currencies";
        private String sqlStatementInsertExternalAccount = @"INSERT INTO saving_accounts(accountName, user_ID, type_ID, bank_ID, currency_ID, creationDate) VALUES(@paramAccountName, @paramUserId, @paramAccountTypeId, @paramBankId, @paramCurrencyId, @paramCreationDate)";

        private String sqlStatementGetAccountTypeId = @"SELECT typeID FROM saving_account_types WHERE typeName = @paramTypeName";
        private String sqlStatementGetCurrencyId = @"SELECT currencyID  FROM currencies WHERE currencyName = @paramTypeName";
        private String sqlStatementGetBankId = @"SELECT bankID FROM banks where bankName = @paramTypeName";

        public ExternalAccountsInsertionForm(int userID) {
            InitializeComponent();
            fillComboBoxesWithData();
            this.userID = userID;          
        }

        private void createAccountButton_Click(object sender, EventArgs e) {
            QueryData paramContainer = getDataForInsertion();

            MySqlCommand externalAccountInsertionCommand = SQLCommandBuilder.getExternalAccountInsertionCommand(sqlStatementInsertExternalAccount, paramContainer);

            int executionResult = DBConnectionManager.insertData(externalAccountInsertionCommand);

            if (executionResult > 0) {
                String successMessage = String.Format("The account named '{0}' was successfully created!", paramContainer.ItemName);
                MessageBox.Show(successMessage, "External account creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                String errorMessage = String.Format("The account named '{0}' could not be created!", paramContainer.ItemName);
                MessageBox.Show(errorMessage, "External account creation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void fillComboBoxesWithData() {
            DataTable accountTypeComboBoxData = retrieveComboBoxData(sqlStatementRetrieveAccountTypes);
            DataTable accountCurrencyComboBoxData = retrieveComboBoxData(sqlStatementRetrieveAvailableCurrencies);
            DataTable accountBankComboBoxData = retrieveComboBoxData(sqlStatementRetrieveAvailableBanks);

            fillComboBox(accountTypeComboBox, accountTypeComboBoxData, "typeName");
            fillComboBox(accountCurrencyComboBox, accountCurrencyComboBoxData, "currencyName");
            fillComboBox(accountBankComboBox, accountBankComboBoxData, "bankName");

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


        private QueryData getDataForInsertion() {
            String accountName = externalAccountNameTextField.Text;
            String accountType = accountTypeComboBox.Text;
            String accountCurrency = accountCurrencyComboBox.Text;
            String accountBank = accountBankComboBox.Text;
            String accountCreationDate = accountCreationDateTimePicker.Value.ToString("yyyy-MM-dd");

            Dictionary<String, int> itemIdDictionary = getElementIds(accountType, accountCurrency, accountBank);
            int accountTypeId = itemIdDictionary["accountType"];
            int accountCurrencyId = itemIdDictionary["accountCurrency"];
            int accountBankId = itemIdDictionary["accountBank"];

            QueryData paramContainer = new QueryData.Builder(userID)
                .addItemName(accountName)
                .addItemTypeID(accountTypeId)
                .addCurrencyID(accountCurrencyId)
                .addBankID(accountBankId)
                .addItemCreationDate(accountCreationDate)
                .build();

            return paramContainer;

        }

        private Dictionary<String, int> getElementIds(String accountType, String accountCurrency, String accountBank) {
            Guard.notNull(accountType, "account type");
            Guard.notNull(accountCurrency, "account currency");
            Guard.notNull(accountBank, "account bank");

            MySqlCommand accountTypeIdRetrievalCommand = SQLCommandBuilder.getTypeIDForItemCommand(sqlStatementGetAccountTypeId, accountType);
            MySqlCommand accountCurrencyIdRetrievalCommand = SQLCommandBuilder.getTypeIDForItemCommand(sqlStatementGetCurrencyId, accountCurrency);
            MySqlCommand accountBankIdRetrievalCommand = SQLCommandBuilder.getTypeIDForItemCommand(sqlStatementGetBankId, accountBank);


            DataTable typeIdDataTable = DBConnectionManager.getData(accountTypeIdRetrievalCommand);
            DataTable currencyIdDataTable = DBConnectionManager.getData(accountCurrencyIdRetrievalCommand);
            DataTable bankIdDataTable = DBConnectionManager.getData(accountBankIdRetrievalCommand);

            int accountTypeId = Convert.ToInt32(typeIdDataTable.Rows[0].ItemArray[0]);
            int accountCurrencyId = Convert.ToInt32(currencyIdDataTable.Rows[0].ItemArray[0]);
            int accountBankId = Convert.ToInt32(bankIdDataTable.Rows[0].ItemArray[0]);

            Dictionary<String, int> itemIdDictionary = new Dictionary<String, int>();
            itemIdDictionary.Add("accountType", accountTypeId);
            itemIdDictionary.Add("accountCurrency", accountCurrencyId);
            itemIdDictionary.Add("accountBank", accountBankId);

            return itemIdDictionary;
        }

        private void resetFieldsButton_Click(object sender, EventArgs e) {
            List<Control> activeControls = new List<Control>() { externalAccountNameTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox, accountCreationDateTimePicker};

            UserControlsManager.clearActiveControls(activeControls);
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Visible = false;
            this.Dispose(true);
        }

        private void externalAccountNameTextField_TextChanged(object sender, EventArgs e) {
            List<Control> activeControls = new List<Control>() { externalAccountNameTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox};
            UserControlsManager.setButtonState(createAccountButton, activeControls);
        }

        private void accountTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            List<Control> activeControls = new List<Control>() { externalAccountNameTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox };
            UserControlsManager.setButtonState(createAccountButton, activeControls);
        }

        private void accountCurrencyComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            List<Control> activeControls = new List<Control>() { externalAccountNameTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox };
            UserControlsManager.setButtonState(createAccountButton, activeControls);
        }

        private void accountBankComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            List<Control> activeControls = new List<Control>() { externalAccountNameTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox };
            UserControlsManager.setButtonState(createAccountButton, activeControls);
        }

        
    }
}
