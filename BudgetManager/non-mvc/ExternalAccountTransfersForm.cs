﻿using BudgetManager.mvc.models;
using BudgetManager.utils;
using BudgetManager.utils.enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    public partial class ExternalAccountTransfersForm : Form {

        private int userID;

        //Data retrieval SQL statements
        private String sqlStatementGetUserAccounts = @"SELECT sa.accountName, ccy.currencyName FROM saving_accounts sa                                                        
                                                         INNER JOIN currencies ccy ON sa.currency_ID = ccy.currencyID 
                                                         WHERE sa.user_ID = @paramID";
        private String sqlStatementGetAccountID = @"SELECT accountID FROM saving_accounts WHERE user_ID = @paramID AND accountName = @paramRecordName";
        private string sqlStatementInsertTransfer = @"INSERT INTO saving_accounts_transfers(senderAccountID, receivingAccountID, transferName, sentValue, receivedValue, exchangeRate, transactionID, observations, transferDate) 
                                                    VALUES(@paramSenderAccountId, @paramReceivingAccountId, @paramTransferName, @paramSentValue, @paramReceivedValue, @paramExchangeRate, @paramTransactionID, @paramObservations, @paramTransferDate)";

        //Command was added at class level so that it can be reused by other methods (it is initialized once the comboboxes are populated)     
        private MySqlCommand userAccountsDataRetrievalCommand;

        private List<Control> activeControls;
        //Map containing key-value pairs of account names and their corresponding currencies
        private Dictionary<String, String> accountCurrencyMap;
        private ErrorProvider transferNameErrorProvider;
        private ErrorProvider sourceAccountErrorProvider;
        private ErrorProvider destinationAccountErrorProvider;
        private ErrorProvider transferValueErrorProvider;
        private ErrorProvider exchangeRateValueErrorProvider;

        public ExternalAccountTransfersForm(int userID) {
            InitializeComponent();
            activeControls = new List<Control>() { transferNameTextBox, sourceAccountComboBox, destinationAccountComboBox, amountTransferredTextBox, exchangeRateTextBox, transferDateTimePicker, transactionIDTextBox, transferObservationsRichTextBox };
            this.userID = userID;

            sourceAccountErrorProvider = new ErrorProvider();
            sourceAccountErrorProvider.SetIconAlignment(sourceAccountComboBox, ErrorIconAlignment.MiddleRight);

            destinationAccountErrorProvider = new ErrorProvider();
            destinationAccountErrorProvider.SetIconAlignment(destinationAccountComboBox, ErrorIconAlignment.MiddleRight);

            transferValueErrorProvider = new ErrorProvider();
            transferValueErrorProvider.SetIconAlignment(amountTransferredTextBox, ErrorIconAlignment.MiddleRight);

            exchangeRateValueErrorProvider = new ErrorProvider();
            exchangeRateValueErrorProvider.SetIconAlignment(exchangeRateTextBox, ErrorIconAlignment.MiddleRight);

            transferNameErrorProvider = new ErrorProvider();
            transferNameErrorProvider.SetIconAlignment(transferNameTextBox, ErrorIconAlignment.MiddleRight);

            populateControls(userID);
            populateDataMaps();
            setDefaultIndexForComboBoxes();
        }


        private void transferNameTextBox_Validated(object sender, EventArgs e) {
            String transferName = transferNameTextBox.Text;
            int maxLength = 50;

            if ("".Equals(transferNameTextBox.Text)) {
                transferNameErrorProvider.SetError(transferNameTextBox, "Please provide a name for your transfer!");
            } else if (transferName.Length > maxLength) {
                transferNameErrorProvider.SetError(transferNameTextBox, "The transfer name length cannot exceed 50 characters!");
            } else {
                transferNameErrorProvider.SetError(transferNameTextBox, String.Empty);
            }
        }

        //DATA VALIDATION LOGIC
        private void sourceAccountComboBox_Validated(object sender, EventArgs e) {
            int selectedIndex = sourceAccountComboBox.SelectedIndex;
            String sourceAccountName = sourceAccountComboBox.Text;
            String destinationAccountName = destinationAccountComboBox.Text;

            if (selectedIndex == -1) {
                sourceAccountErrorProvider.SetError(sourceAccountComboBox, "Please select the source account for your transfer!");
            } else if (sourceAccountName.Equals(destinationAccountName)) {
                sourceAccountErrorProvider.SetError(sourceAccountComboBox, "Cannot perform transfers between the same account! The source and destination accounts must be different.");
            } else {
                sourceAccountErrorProvider.SetError(sourceAccountComboBox, String.Empty);
            }
        }

        private void destinationAccountComboBox_Validated(object sender, EventArgs e) {
            int selectedIndex = destinationAccountComboBox.SelectedIndex;
            String sourceAccountName = sourceAccountComboBox.Text;
            String destinationAccountName = destinationAccountComboBox.Text;

            if (selectedIndex == -1) {
                destinationAccountErrorProvider.SetError(destinationAccountComboBox, "Please select the destination account for your transfer!");
            } else if (sourceAccountName.Equals(destinationAccountName)) {
                destinationAccountErrorProvider.SetError(destinationAccountComboBox, "Cannot perform transfers between the same account! The source and destination accounts must be different.");
            } else {
                destinationAccountErrorProvider.SetError(destinationAccountComboBox, String.Empty);
            }
        }

        private void amountTransferredTextBox_Validated(object sender, EventArgs e) {
            String transferredAmount = amountTransferredTextBox.Text;

            if (!isValidDecimal(transferredAmount) || (isValidDecimal(transferredAmount) && Convert.ToDouble(transferredAmount) <= 0)) {
                transferValueErrorProvider.SetError(amountTransferredTextBox, "The transfer value must be a positive integer/decimal value!");
            } else {
                transferValueErrorProvider.SetError(amountTransferredTextBox, String.Empty);
            }
        }

        private void exchangeRateTextBox_Validated(object sender, EventArgs e) {
            String exchangeRateAmount = exchangeRateTextBox.Text;

            if (!isValidDecimal(exchangeRateAmount) || (isValidDecimal(exchangeRateAmount) && Convert.ToDouble(exchangeRateAmount) <= 0)) {
                exchangeRateValueErrorProvider.SetError(exchangeRateTextBox, "The exchange rate must be a positive integer/decimal value!");
            } else {
                exchangeRateValueErrorProvider.SetError(exchangeRateTextBox, String.Empty);
            }
        }

        private bool isValidInputAmount(String inputValue, Regex regex) {
            Guard.notNull(inputValue, "The input value provided for validation cannot be null", "amount to validate");
            Guard.notNull(regex, "The regex object provided for input amount validation cannot be null", "regex");

            if (regex.IsMatch(inputValue)) {
                return true;
            }

            return false;
        }

        private void transferObservationsRichTextBox_TextChanged(object sender, EventArgs e) {
            int characterCount = transferObservationsRichTextBox.Text.Length;
            int charactersLeft = transferObservationsRichTextBox.MaxLength - characterCount;

            charactersLeftLabel.Text = String.Format("You have {0} characters left", charactersLeft);


        }

        private void transferObservationsRichTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            int characterCount = transferObservationsRichTextBox.Text.Length;

            if (characterCount >= transferObservationsRichTextBox.MaxLength) {
                e.Handled = true;
                return;
            }

        }

        //TRANSFER EXECUTION LOGIC
        private void transferButton_Click(object sender, EventArgs e) {
            DialogResult userOptionConfirmTransfer = MessageBox.Show("Are you sure that you want to perform the requested transfer?", "External account transfers", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOptionConfirmTransfer == DialogResult.No) {
                return;
            }

            //General input check
            int userInputCheckResult = performInputChecks();

            //User input data check
            if (userInputCheckResult == -1) {
                return;
            }

            QueryData paramContainer = retrieveUserInputData();
            //Transfer amount check(checks if it's less than the available balance of the saving account)
            DataCheckResponse transferAmountCheckResult = performTransferValueCheck(paramContainer.SentValue, paramContainer.SourceAccountID);

            //Source account balance check
            if (transferAmountCheckResult.ExecutionResult == -1) {
                MessageBox.Show(transferAmountCheckResult.ErrorMessage, "Transfer check", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }

            //QueryData paramContainer = retrieveUserInputData();
            MySqlCommand transferInsertionCommand = SQLCommandBuilder.getTransferInsertionCommand(sqlStatementInsertTransfer, paramContainer);

            int executionResult = DBConnectionManager.insertData(transferInsertionCommand);

            if (executionResult > 0) {
                //Additional details for transfers and info message type
                MessageBox.Show("The transfer was successfully performed!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                String transferInfo = getTransferSummary(paramContainer);
                MessageBox.Show(transferInfo, "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserControlsManager.clearActiveControls(activeControls);

            } else {
                //Error message type
                MessageBox.Show("Unable to perform the requested transfer!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Shows a preview of the transfer that is about to be performed
        private void previewTransferButton_Click(object sender, EventArgs e) {
            int checkResult = performInputChecks();

            if (checkResult == -1) {
                return;
            }

            QueryData paramContainer = retrieveUserInputData();

            String transferPreviewData = getTransferSummary(paramContainer);

            MessageBox.Show(transferPreviewData, "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void resetButton_Click(object sender, EventArgs e) {
            UserControlsManager.clearActiveControls(activeControls);
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Visible = false;
            this.Dispose();
        }

        private void sourceAccountComboBox_MouseHover(object sender, EventArgs e) {
            displayCurrencyInformation(sourceAccountComboBox, AccountType.SOURCE_ACCOUNT);
        }

        private void destinationAccountComboBox_MouseHover(object sender, EventArgs e) {
            displayCurrencyInformation(destinationAccountComboBox, AccountType.DESTINATION_ACCOUNT);
        }


        //DATA CHECKS METHODS
        //Method for performing general input checks (checks if any error message was triggered for the form fields)
        private int performInputChecks() {
            int transferNameMaxLength = 50;
            String sourceAccountName = sourceAccountComboBox.Text;
            String destinationAccountName = destinationAccountComboBox.Text;

            //Triggers the validation event for each control that has this specific logic implemented
            focusControls(activeControls);

            String transferNameValidationErrorMessage = transferNameErrorProvider.GetError(transferNameTextBox);
            String sourceAccountValidationErrorMessage = sourceAccountErrorProvider.GetError(sourceAccountComboBox);
            String destinationAccountValidationErrorMessage = destinationAccountErrorProvider.GetError(destinationAccountComboBox);
            String transferAmountValidationErrorMessage = transferValueErrorProvider.GetError(amountTransferredTextBox);
            String exchangeRateValidationErrorMessage = exchangeRateValueErrorProvider.GetError(exchangeRateTextBox);

            List<String> validationErrorList = new List<String> { transferNameValidationErrorMessage, sourceAccountValidationErrorMessage, destinationAccountValidationErrorMessage, transferAmountValidationErrorMessage, exchangeRateValidationErrorMessage };

            //Checks if the validation of any form fields has failed
            foreach (String errorMessage in validationErrorList) {
                if (!errorMessage.Equals(String.Empty)) {
                    MessageBox.Show("Please correct all the invalid form data before performing the transfer!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }

            return 0;
        }

        //Method for checking if the amount to be transferred is greater than the available balance of the saving account
        private DataCheckResponse performTransferValueCheck(double transferValue, int sourceAccountID) {
            //Improve the check method (performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert)) to accept all the parameters being sent as attributes of the QueryData object
            String itemName = "account transfer";

            QueryData paramContainer = new QueryData.Builder(userID).addSourceAccountID(sourceAccountID).build();

            DataInsertionCheckStrategy transferCheckStrategy = new TransferCheckStrategy();
            DataInsertionCheckerContext dataInsertionCheckerContext = new DataInsertionCheckerContext();
            dataInsertionCheckerContext.setStrategy(transferCheckStrategy);

            //int transferValueCheckResult = dataInsertionCheckerContext.invoke(paramContainer, itemName, transferValue);
            DataCheckResponse transferValueCheckResponse = dataInsertionCheckerContext.invoke(paramContainer, itemName, transferValue);


            return transferValueCheckResponse;

        }

        //Methods for populating comboboxes with data
        private void populateControls(int userID) {
            QueryData paramContainer = new QueryData.Builder(userID).build();
            //CHANGE-DISPLAY ALL USER ACCOUNTS
            userAccountsDataRetrievalCommand = SQLCommandBuilder.getTypeNameForItemCommand(sqlStatementGetUserAccounts, paramContainer);

            int sourceAccountPopulationResult = UserControlsManager.fillComboBoxWithData(sourceAccountComboBox, userAccountsDataRetrievalCommand, "accountName");
            int destinationAccountPopulationResult = UserControlsManager.fillComboBoxWithData(destinationAccountComboBox, userAccountsDataRetrievalCommand, "accountName");

            //Checks are performed to see if source/destination accounts could be retrieved
            if (sourceAccountPopulationResult == -1) {
                MessageBox.Show("Unable to retrieve the source account/s for the current user! Please consider creating an account before proceeding.", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Disables the transfer button to avoid performing useless checks in this situation
                transferButton.Enabled = false;
            }

            if (sourceAccountPopulationResult == -1) {
                MessageBox.Show("Unable to retrieve the destination account/s for the current user! Please consider creating an account before proceeding.", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                transferButton.Enabled = false;
            }
        }

        private void setDefaultIndexForComboBoxes() {
            sourceAccountComboBox.SelectedIndex = -1;
            destinationAccountComboBox.SelectedIndex = -1;
        }

        //Method used for populating the source/destination account maps with data which will later be used for dislaying the currency for each account involved in the transfer
        private void populateDataMaps() {
            accountCurrencyMap = UserControlsManager.getMapFromDataTable(userAccountsDataRetrievalCommand);
        }

        //UTILITY METHODS

        //Method used for retrieving the data provided by the user and creating a QueryData object based on it
        private QueryData retrieveUserInputData() {
            String sourceAccountName = sourceAccountComboBox.Text;
            String destinationAccountName = destinationAccountComboBox.Text;

            QueryData paramContainerSourceAccount = new QueryData.Builder(userID).addItemName(sourceAccountName).build();
            QueryData paramContainerDestinationAccount = new QueryData.Builder(userID).addItemName(destinationAccountName).build();


            MySqlCommand sourceAccountIdRetrievalCommand = SQLCommandBuilder.getRecordIDCommand(sqlStatementGetAccountID, paramContainerSourceAccount);
            MySqlCommand destinationAccountIdRetrievalCommand = SQLCommandBuilder.getRecordIDCommand(sqlStatementGetAccountID, paramContainerDestinationAccount);

            String transferName = transferNameTextBox.Text;
            int sourceAccountId = getAccountId(sourceAccountIdRetrievalCommand);
            int destinationAccountId = getAccountId(destinationAccountIdRetrievalCommand);
            double exchangeRate = Convert.ToDouble(exchangeRateTextBox.Text);//How much one unit of the sent currency represents compared to one unit of the received currency(e.g-GBP-EUR-1.17 => 1 GBP is equal to 1.17 EUR)
            //int sentValue = Convert.ToInt32(amountTransferredTextBox.Text);
            double sentValue = Convert.ToDouble(amountTransferredTextBox.Text);
            //int receivedValue = (int)(sentValue / exchangeRate);
            double receivedValue = sentValue / exchangeRate;
            String transferDate = transferDateTimePicker.Value.ToString("yyyy-MM-dd");
            String transferObservations = transferObservationsRichTextBox.Text;
            String transactionID = !transactionIDTextBox.Text.Equals("") ? transactionIDTextBox.Text : null;

            QueryData paramContainer = new QueryData.Builder(userID)
                .addSourceAccountID(sourceAccountId)
                .addDestinationAccountID(destinationAccountId)
                .addItemName(transferName)
                .addSentValue(sentValue)
                .addReceivedValue(receivedValue)
                .addExchangeRate(exchangeRate)
                .addAdditionalData(transferObservations)
                .addItemCreationDate(transferDate)
                .addGenericID(transactionID)
                .build();

            return paramContainer;

        }

        private int getAccountId(MySqlCommand accountIdRetrievalCommand) {
            Guard.notNull(accountIdRetrievalCommand, "SQL ID retrieval command");

            DataTable accountIdDataTable = DBConnectionManager.getData(accountIdRetrievalCommand);

            if (accountIdDataTable.Rows.Count == 0) {
                return -1;
            }

            int accountID = Convert.ToInt32(accountIdDataTable.Rows[0].ItemArray[0]);

            return accountID;
        }

        //Method used for creating and formatting the string containing the transfer details
        private String getTransferSummary(QueryData paramContainer) {
            Guard.notNull(paramContainer, "transfer summary details");
            String sourceAccountCurrency = getAccountCurrency(AccountType.SOURCE_ACCOUNT, sourceAccountComboBox.Text);
            String destinationAccountCurrency = getAccountCurrency(AccountType.DESTINATION_ACCOUNT, destinationAccountComboBox.Text);

            //Creates each line of data separately in order to allow the correct text alignment(to the left)
            //Newline characters are added to insert a blank line after each data (in addition to the newline character added when appending the data to the StringBuilder object)
            String titleData = String.Format("{0, -10}", "TRANSFER DETAILS\n");
            String transferNameData = String.Format("{0, -10}: {1, -10}\n", "Transfer name", paramContainer.ItemName);
            String sourceAccountData = String.Format("{0, -10}: {1, -10}\n", "Source account", sourceAccountComboBox.Text);
            String destinationAccountData = String.Format("{0, -10}: {1, -10}\n", "Destination account", destinationAccountComboBox.Text);
            String amountTransferredData = String.Format("{0, -10}: {1} {2}\n", "Amount transferred", paramContainer.SentValue, sourceAccountCurrency);
            String amountReceivedData = String.Format("{0, -10}: {1} {2}\n", "Amount received", paramContainer.ReceivedValue, destinationAccountCurrency);
            String exchangeRateData = String.Format("{0, -10}: {1, -10}\n", "Exchange rate", paramContainer.ExchangeRate);
            String transactionIDData = String.Format("{0, -10}: {1, 10}\n", "Transfer ID", paramContainer.GenericID);
            String transferDateData = String.Format("{0, -10}: {1, -10}\n", "Transfer date", paramContainer.ItemCreationDate);
            String transferObservationsData = String.Format("{0, -10}: {1, 10}\n\n", "Transfer observations", paramContainer.AdditionalData);
            String hintData = String.Format("{0, -10}", "Press Ctrl + C to copy the transfer details information.");

            List<String> dataList = new List<String>();

            dataList.Add(titleData);
            dataList.Add(transferNameData);
            dataList.Add(sourceAccountData);
            dataList.Add(destinationAccountData);
            dataList.Add(amountTransferredData);
            dataList.Add(amountReceivedData);
            dataList.Add(exchangeRateData);
            dataList.Add(transactionIDData);
            dataList.Add(transferDateData);
            dataList.Add(transferObservationsData);
            dataList.Add(hintData);


            StringBuilder sb = new StringBuilder();
            foreach (String data in dataList) {
                sb.Append(data + "\n");
            }

            return sb.ToString();

        }


        //Method used for displaying the currency for each selected account inside a tool tip which is displayed on mouse hover over the combobox
        private void displayCurrencyInformation(ComboBox targetComboBox, AccountType accountType) {
            int selectedIndex = targetComboBox.SelectedIndex;
            String selectedAccountName = targetComboBox.Text;

            ToolTip toolTip = new ToolTip();
            String toolTipMessage;

            if (selectedIndex <= -1) {
                toolTipMessage = "No account selected";
                toolTip.SetToolTip(targetComboBox, toolTipMessage);
            } else {
                String selectedAccountCurrency = getAccountCurrency(accountType, selectedAccountName);

                toolTipMessage = String.Format("Account currency: {0}", selectedAccountCurrency);

                toolTip.SetToolTip(targetComboBox, toolTipMessage);
            }
        }

        //Method used for retrieving the selected account currency from the maps(sourceAccountMap, destinationAccountMap)
        private String getAccountCurrency(AccountType accountType, String accountName) {
            String retrievedCurrency = "N/A";

            accountCurrencyMap.TryGetValue(accountName, out retrievedCurrency);

            return retrievedCurrency;
        }

        private bool isValidDecimal(String inputValue) {
            double result;

            return Double.TryParse(inputValue, out result);
        }

        //Method that calls the focus method on each control from the list so that the validation of that control is triggered
        private void focusControls(List<Control> controlList) {
            if (controlList == null) {
                return;
            }

            foreach (Control control in controlList) {
                control.Focus();
            }
        }
    }
}


