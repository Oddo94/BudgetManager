using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    public enum AccountType {
        SOURCE_ACCOUNT,
        DESTINATION_ACCOUNT
    }

    public partial class ExternalAccountTransfersForm : Form {

        private int userID;

        //Data retrieval SQL statements
        //private String sqlStatementGetSourceAccounts = @"SELECT sa.accountName, ccy.currencyName FROM saving_accounts sa
        //                                                 INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
        //                                                 INNER JOIN currencies ccy ON sa.currency_ID = ccy.currencyID 
        //                                                 WHERE sa.user_ID = @paramID AND sat.typeName like '%SYSTEM_DEFINED%'";
        private String sqlStatementGetUserAccounts = @"SELECT sa.accountName, ccy.currencyName FROM saving_accounts sa                                                        
                                                         INNER JOIN currencies ccy ON sa.currency_ID = ccy.currencyID 
                                                         WHERE sa.user_ID = @paramID";
        //private String sqlStatementGetDestinationAccounts = @"SELECT sa.accountName, ccy.currencyName FROM saving_accounts sa
        //                                                INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
        //                                                INNER JOIN currencies ccy ON sa.currency_ID = ccy.currencyID 
        //                                                WHERE sa.user_ID = @paramID AND sat.typeName like '%USER_DEFINED%';";
        private String sqlStatementGetAccountID = @"SELECT accountID FROM saving_accounts WHERE user_ID = @paramID AND accountName = @paramRecordName";
        private string sqlStatementInsertTransfer = @"INSERT INTO saving_accounts_transfers(senderAccountID, receivingAccountID, transferName, sentValue, receivedValue, exchangeRate, observations, transferDate) 
                                                    VALUES(@paramSenderAccountId, @paramReceivingAccountId, @paramTransferName, @paramSentValue, @paramReceivedValue, @paramExchangeRate, @paramObservations, @paramTransferDate)";

        //Commands were added at class level so that they can be reused by other methods (they are initialized once the comboboxes are populated)
        //private MySqlCommand sourceAccountsDataRetrievalCommand;
        //private MySqlCommand destinationAccountsDataRetrievalCommand;
        private MySqlCommand userAccountsDataRetrievalCommand;
 
        private List<Control> activeControls;
        //Maps containing key-value pairs of account names and their corresponding currencies
        //private Dictionary<String, String> sourceAccountMap;
        //private Dictionary<String, String> destinationAccountMap;
        private Dictionary<String, String> accountCurrencyMap;

        public ExternalAccountTransfersForm(int userID) {
            InitializeComponent();
            activeControls = new List<Control>() { transferNameTextBox, sourceAccountComboBox, destinationAccountComboBox, amountTransferredTextBox, exchangeRateTextBox, transferDateTimePicker, transferObservationsRichTextBox };
            this.userID = userID;

            populateControls(userID);
            populateDataMaps();
            setDefaultIndexForComboBoxes();

        }

        private void amountTransferredTextBox_TextChanged(object sender, EventArgs e) {
            String transferredAmount = amountTransferredTextBox.Text;
            Regex transferredAmountRegex = new Regex("^\\d+$");

            if (!isValidInputAmount(transferredAmount, transferredAmountRegex)) {
                amountTransferredTextBox.Text = "";
            }
        }

        private void exchangeRateTextBox_TextChanged(object sender, EventArgs e) {
            String exchangeRateValue = exchangeRateTextBox.Text;
            Regex exchangeRateRegexNonZeroValue = new Regex("[^0+]");
            Regex exchangeRateRegexGeneralFormat = new Regex("^\\d+(?(?=\\.{1})\\.\\d+|\\b)$");

            if (!isValidInputAmount(exchangeRateValue, exchangeRateRegexNonZeroValue) || !isValidInputAmount(exchangeRateValue, exchangeRateRegexGeneralFormat)) {
                invalidExchangeRateFormatLabel.Text = "Invalid exchange rate value.It must be a positive integer/double value";
            } else {
                invalidExchangeRateFormatLabel.Text = "";
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

        private void transferButton_Click(object sender, EventArgs e) {
            DialogResult userOptionConfirmTransfer = MessageBox.Show("Are you sure that you want to perform the requested transfer?", "External account transfers", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOptionConfirmTransfer == DialogResult.No) {
                return;
            }

            int transferValue = Convert.ToInt32(amountTransferredTextBox.Text);

            //General input check
            int userInputCheckResult = performInputChecks();
            //Transfer amount check(checks if it's less than the available balance of the saving account)
            //EXTEND METHOD TO CHECK THE AVAILABLE BALANCE FOR ALL THE DISPLAYED ACCOUNTS!!!
            int transferAmountCheckResult = performTransferValueCheck(transferValue);

            if (userInputCheckResult == -1 || transferAmountCheckResult == -1) {
                return;
            }

            QueryData paramContainer = retrieveUserInputData();
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

            if(checkResult == -1) {
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

        //Method for performing general input checks
        private int performInputChecks() {
            int transferNameMaxLength = 50;
            String sourceAccountName = sourceAccountComboBox.Text;
            String destinationAccountName = destinationAccountComboBox.Text;


            if ("".Equals(transferNameTextBox.Text)) {
                MessageBox.Show("Please provide a name for your transfer!", "External account transfers.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if (transferNameTextBox.Text.Length > transferNameMaxLength) {
                MessageBox.Show("The transfer name length cannot exceed 50 characters.", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if (sourceAccountComboBox.SelectedIndex == -1) {
                MessageBox.Show("Please select the source account for your transfer!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if (sourceAccountName.Equals(destinationAccountName)) {
                MessageBox.Show("Cannot perform transfers between the same account! The source and destination accounts must be different.", "External accounts transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;                
            }
         
            if (destinationAccountComboBox.SelectedIndex == -1) {
                MessageBox.Show("Please select the destination account for your transfer!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if ("".Equals(amountTransferredTextBox.Text)) {
                MessageBox.Show("Please specify the transferred amount!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if ("".Equals(exchangeRateTextBox.Text)) {
                MessageBox.Show("Please specify the exchange rate value!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            } else if (!"".Equals(invalidExchangeRateFormatLabel.Text)) {
                MessageBox.Show("Invalid exchange rate value!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            return 0;
        }

        //Method for checking if the amount to be transferred is greater than the available balance f the saving account
        private int performTransferValueCheck(int transferValue) {
            //Improve the check method (performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert)) to accept all the parameters being sent as attributes of the QueryData object
            String itemName = "account transfer";

            QueryData paramContainer = new QueryData.Builder(userID).addIncomeSource(IncomeSource.SAVING_ACCOUNT).build();

            GeneralInsertionCheckStrategy generalInsertionCheckStrategy = new GeneralInsertionCheckStrategy();
            DataInsertionCheckerContext dataInsertionCheckerContext = new DataInsertionCheckerContext();
            dataInsertionCheckerContext.setStrategy(generalInsertionCheckStrategy);

            int transferValueCheckResult = dataInsertionCheckerContext.invoke(paramContainer, itemName, transferValue);

            return transferValueCheckResult;
        }




        //Methods for populating comboboxes with data
        private void populateControls(int userID) {
            QueryData paramContainer = new QueryData.Builder(userID).build();
            //sourceAccountsDataRetrievalCommand = SQLCommandBuilder.getTypeNameForItemCommand(sqlStatementGetSourceAccounts, paramContainer);
            //destinationAccountsDataRetrievalCommand = SQLCommandBuilder.getTypeNameForItemCommand(sqlStatementGetDestinationAccounts, paramContainer);

            //CHANGE-DISPLAY ALL USER ACCOUNTS
            userAccountsDataRetrievalCommand = SQLCommandBuilder.getTypeNameForItemCommand(sqlStatementGetUserAccounts, paramContainer);


            //int sourceAccountPopulationResult = UserControlsManager.fillComboBoxWithData(sourceAccountComboBox, sourceAccountsDataRetrievalCommand, "accountName");
            //int destinationAccountPopulationResult = UserControlsManager.fillComboBoxWithData(destinationAccountComboBox, destinationAccountsDataRetrievalCommand, "accountName");


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

        //Method used for populating the source/destination account maps with data whichc will later be used for dislaying the currency for each account involved in the transfer
        private void populateDataMaps() {
            //CHANGE-DISPLAY ALL ACCOUNTS
            //sourceAccountMap = UserControlsManager.getMapFromDataTable(sourceAccountsDataRetrievalCommand);
            //destinationAccountMap = UserControlsManager.getMapFromDataTable(destinationAccountsDataRetrievalCommand);

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
            double exchangeRate = Convert.ToDouble(exchangeRateTextBox.Text);
            int sentValue = Convert.ToInt32(amountTransferredTextBox.Text);
            int receivedValue = (int)(sentValue * exchangeRate);
            String transferDate = transferDateTimePicker.Value.ToString("yyyy-MM-dd");
            String transferObservations = transferObservationsRichTextBox.Text;

            QueryData paramContainer = new QueryData.Builder(userID)
                .addSourceAccountID(sourceAccountId)
                .addDestinationAccountID(destinationAccountId)
                .addItemName(transferName)
                .addSentValue(sentValue)
                .addReceivedValue(receivedValue)
                .addExchangeRate(exchangeRate)
                .addAdditionalData(transferObservations)
                .addItemCreationDate(transferDate)
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

            //switch (accountType) {
            //    case AccountType.SOURCE_ACCOUNT:
            //        sourceAccountMap.TryGetValue(accountName, out retrievedCurrency);
            //        break;

            //    case AccountType.DESTINATION_ACCOUNT:
            //        destinationAccountMap.TryGetValue(accountName, out retrievedCurrency);
            //        break;

            //    default:
            //        break;
            //}

            accountCurrencyMap.TryGetValue(accountName, out retrievedCurrency);

            return retrievedCurrency;
        }

    }
}
    

