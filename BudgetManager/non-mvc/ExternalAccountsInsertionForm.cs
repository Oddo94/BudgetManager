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
using BudgetManager.utils.data_insertion;


namespace BudgetManager.non_mvc {
    public partial class ExternalAccountsInsertionForm : Form {

        private int userID;
        private List<Control> activeControls;
        private List<Control> allFormControls;
        private AccountUtils accountUtils;
        //Boolean flag used to determine if the controls was changes as a result of user action or code execution(e.g: when creating and populating controls with data)
        private bool isEditedByUser = false;

        private String sqlStatementRetrieveAccountTypes = @"SELECT typeName FROM saving_account_types WHERE typeName != 'SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT'";
        private String sqlStatementRetrieveAvailableBanks = @"SELECT bankName FROM banks ORDER BY bankName";
        private String sqlStatementRetrieveAvailableCurrencies = @"SELECT currencyName FROM currencies ORDER BY currencyName";
        private String sqlStatementInsertExternalAccount = @"INSERT INTO saving_accounts(accountName, accountNumber, user_ID, type_ID, bank_ID, currency_ID, creationDate) VALUES(@paramAccountName, @paramAccountNumber, @paramUserId, @paramAccountTypeId, @paramBankId, @paramCurrencyId, @paramCreationDate)";

        private String sqlStatementGetAccountTypeId = @"SELECT typeID FROM saving_account_types WHERE typeName = @paramTypeName";
        private String sqlStatementGetCurrencyId = @"SELECT currencyID FROM currencies WHERE currencyName = @paramTypeName";
        private String sqlStatementGetBankId = @"SELECT bankID FROM banks where bankName = @paramTypeName";

        public ExternalAccountsInsertionForm(int userID) {
            InitializeComponent();
            fillComboBoxesWithData();
            this.userID = userID;
            this.isEditedByUser = true;//After the controls are created and populated with data the flag is set to true as from that point on any change can be performed only by the user(not the code)
            this.activeControls = new List<Control>() { externalAccountNameTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox };
            this.allFormControls = new List<Control>() { externalAccountNameTextField, externalAccountNumberTextField, accountTypeComboBox, accountCurrencyComboBox, accountBankComboBox, accountCreationDateTimePicker };
            this.accountUtils = new AccountUtils();
        }

        private void createAccountButton_Click(object sender, EventArgs e) {
            DialogResult userOptionConfirmCreation = MessageBox.Show("Are you sure that you want the create a new account?", "External account creation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOptionConfirmCreation == DialogResult.No) {
                return;
            }

            QueryData paramContainer = getDataForInsertion();

            MySqlCommand externalAccountInsertionCommand = SQLCommandBuilder.getExternalAccountInsertionCommand(sqlStatementInsertExternalAccount, paramContainer);

            int externalAccountCreationResult = DBConnectionManager.insertData(externalAccountInsertionCommand);

            int accountBalanceStorageRecordCreationResult = accountUtils.createAccountBalanceStorageRecordForAccount(null, userID, utils.enums.AccountType.CUSTOM_ACCOUNT, paramContainer.ItemName);

            String successMessage;
            String errorMessage;
            if (externalAccountCreationResult > 0 && accountBalanceStorageRecordCreationResult > 0) {
                successMessage = String.Format("The account named '{0}' was successfully created!", paramContainer.ItemName);
                MessageBox.Show(successMessage, "External account creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Resets the form fields
                UserControlsManager.clearActiveControls(allFormControls);
            } else if (externalAccountCreationResult > 0 && accountBalanceStorageRecordCreationResult == -1) {
                errorMessage = String.Format("Unable to create the balance storage record for the account named '{0}'! Please contact the application administrator for fixing this issue", paramContainer.ItemName);
                MessageBox.Show(errorMessage, "External account creation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                errorMessage = String.Format("The account named '{0}' could not be created!", paramContainer.ItemName);
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
            String accountNumber = externalAccountNumberTextField.Text;
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
                .addItemIdentificationNumber(accountNumber)
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
            UserControlsManager.clearActiveControls(allFormControls);
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Visible = false;
            this.Dispose(true);
        }

        private void externalAccountNameTextField_TextChanged(object sender, EventArgs e) {
            //Checks if the user modified the control or not
            if (isEditedByUser) {
                UserControlsManager.setButtonState(createAccountButton, activeControls);
            }
        }

        private void accountTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (isEditedByUser) {
                UserControlsManager.setButtonState(createAccountButton, activeControls);
            }
        }

        private void accountCurrencyComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (isEditedByUser) {
                UserControlsManager.setButtonState(createAccountButton, activeControls);
            }
        }

        private void accountBankComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (isEditedByUser) {
                UserControlsManager.setButtonState(createAccountButton, activeControls);
            }
        }
    }
}
