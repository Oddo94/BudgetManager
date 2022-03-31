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
    public partial class ExternalAccountTransfersForm : Form {

        private int userID;

        private String sqlStatementGetSourceAccounts = @"SELECT sa.accountName FROM saving_accounts sa
                                                        INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                                                        WHERE sa.user_ID = @paramID AND sat.typeName like '%SYSTEM_DEFINED%'";
        private String sqlStatementGetDestinationAccounts = @"SELECT sa.accountName FROM saving_accounts sa
                                                        INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                                                        WHERE sa.user_ID = @paramID AND sat.typeName like '%USER_DEFINED%'";

        private List<Control> activeControls;

        public ExternalAccountTransfersForm(int userID) {
            InitializeComponent();
            activeControls = new List<Control>() { sourceAccountComboBox, destinationAccountComboBox, amountTransferredTextBox, exchangeRateTextBox, transferDateTimePicker, transferObservationsRichTextBox};
            this.userID = userID;

            populateControls(userID);
            setDefaultIndexForComboBoxes();
            

        }

        private void amountTransferredTextBox_TextChanged(object sender, EventArgs e) {
            String transferredAmount = amountTransferredTextBox.Text;
            Regex transferredAmountRegex = new Regex("^\\d+$");

            if (!isValidInputAmount(transferredAmount, transferredAmountRegex)) {
                amountTransferredTextBox.Text = "";
            }

            //if("".Equals(transferredAmount)) {
            //    transferButton.Enabled = false;
            //}

        }

        private void exchangeRateTextBox_TextChanged(object sender, EventArgs e) {
            String exchangeRateValue = exchangeRateTextBox.Text;
            Regex exchangeRateRegexNonZeroValue = new Regex("[^0+]");
            Regex exchangeRateRegexGeneralFormat = new Regex("^\\d+(?(?=\\.{1})\\.\\d+|\\b)$");

            if (!isValidInputAmount(exchangeRateValue, exchangeRateRegexNonZeroValue) || !isValidInputAmount(exchangeRateValue, exchangeRateRegexGeneralFormat)) {
                //exchangeRateTextBox.Text = "";
                invalidExchangeRateFormatLabel.Text = "Invalid exchange rate value.It must be a positive integer/double value";
            } else {
                invalidExchangeRateFormatLabel.Text = "";
            }

            //if("".Equals(exchangeRateValue)) {
            //    transferButton.Enabled = false;
            //}
        }

        private int performInputChecks() {
            if(sourceAccountComboBox.SelectedIndex == -1) {
                MessageBox.Show("Please select the source account for your transfer!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if (destinationAccountComboBox.SelectedIndex == -1) {
                MessageBox.Show("Please select the destination account for your transfer!", "External account transfers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if("".Equals(amountTransferredTextBox.Text)) {
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

        private void exchangeRateTextBox_Leave(object sender, EventArgs e) {
            //String exchangeRateValue = exchangeRateTextBox.Text;
            //Regex exchangeRateRegexNonZeroValue = new Regex("[^0+]");
            //Regex exchangeRateRegexGeneralFormat = new Regex("^\\d+(?(?=\\.{1})\\.\\d+|\\b)$");

            //if (!isValidInputAmount(exchangeRateValue, exchangeRateRegexNonZeroValue) || !isValidInputAmount(exchangeRateValue, exchangeRateRegexGeneralFormat)) {
            //    exchangeRateTextBox.Text = "";
            //    invalidExchangeRateFormatLabel.Text = "Invalid exchange rate value.It must be a positive integer/double value";
            //} else {
            //    invalidExchangeRateFormatLabel.Text = "";
            //}

            //UserControlsManager.setButtonState(transferButton, activeControls);

        }

        private void transferButton_Click(object sender, EventArgs e) {
            int userInputCheckResult = performInputChecks();

            if(userInputCheckResult == -1) {
                return;
            }

            //Improve the check method (performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert)) to accept all the parameters being sent as attributes of the QueryData object
            String itemName = "account transfer";
            int transferValue = Convert.ToInt32(amountTransferredTextBox.Text);

            QueryData paramContainer = new QueryData.Builder(userID).addIncomeSource(IncomeSource.SAVING_ACCOUNT).build();



            GeneralInsertionCheckStrategy generalInsertionCheckStrategy = new GeneralInsertionCheckStrategy();
            DataInsertionCheckerContext dataInsertionCheckerContext = new DataInsertionCheckerContext();
            dataInsertionCheckerContext.setStrategy(generalInsertionCheckStrategy);

            int dataInsertionCheckResult = dataInsertionCheckerContext.invoke(paramContainer, itemName, transferValue);

            //FOR TESTING PURPOSES ONLY
            if (dataInsertionCheckResult == -1) {
                MessageBox.Show("General check failed.Returning from the method...");
                return;
            }
        }

        private void resetButton_Click(object sender, EventArgs e) {
            UserControlsManager.clearActiveControls(activeControls);
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Visible = false;
            this.Dispose();
        }


        //Methods for populating comboboxes with data
        private void populateControls(int userID) {
            QueryData paramContainer = new QueryData.Builder(userID).build();
            MySqlCommand sourceAccountsDataRetrievalCommand = SQLCommandBuilder.getTypeNameForItemCommand(sqlStatementGetSourceAccounts, paramContainer);
            MySqlCommand destinationAccountsDataRetrievalCommand = SQLCommandBuilder.getTypeNameForItemCommand(sqlStatementGetDestinationAccounts, paramContainer);


            UserControlsManager.fillComboBoxWithData(sourceAccountComboBox, sourceAccountsDataRetrievalCommand, "accountName");
            UserControlsManager.fillComboBoxWithData(destinationAccountComboBox, destinationAccountsDataRetrievalCommand, "accountName");
        }

        private void setDefaultIndexForComboBoxes() {
            sourceAccountComboBox.SelectedIndex = -1;
            destinationAccountComboBox.SelectedIndex = -1;
        }

        private void sourceAccountComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            //UserControlsManager.setButtonState(transferButton, activeControls);
        }

        private void destinationAccountComboBox_SelectedIndexChanged(object sender, EventArgs e) {         
            //UserControlsManager.setButtonState(transferButton, activeControls);
        }

       
    }
}
