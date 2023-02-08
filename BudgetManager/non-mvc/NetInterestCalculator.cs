using BudgetManager.utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    public partial class NetInterestCalculator : Form {
        private List<Control> activeControls;
        private ErrorProvider grossInterestErrorProvider;
        private TextBox fieldToPopulate;

        public NetInterestCalculator(TextBox fieldToPopulate) {
            InitializeComponent();
            //Initializes the field that will be populated with the net interest result value
            this.fieldToPopulate = fieldToPopulate;

            //Initializes the list of active controls which will be used to reset them
            activeControls = new List<Control>() { grossInterestTextBox, taxPercentageNumericUpDown, netInterestTextBox };

            grossInterestErrorProvider = new ErrorProvider();
            grossInterestErrorProvider.SetIconAlignment(grossInterestTextBox, ErrorIconAlignment.MiddleRight);
          

        }

        private bool isValidInputAmount(String inputValue, Regex regex) {
            Guard.notNull(inputValue, "The input value provided for validation cannot be null", "amount to validate");
            Guard.notNull(regex, "The regex object provided for input amount validation cannot be null", "regex");

            if (regex.IsMatch(inputValue)) {
                return true;
            }

            return false;
        }

        private void grossInterestTextBox_Validated(object sender, EventArgs e) {
            String grossInterestInput = grossInterestTextBox.Text;
            double parseResult;
            bool canParseGrossInterestInput = Double.TryParse(grossInterestInput, out parseResult);

            if (!canParseGrossInterestInput) {
                grossInterestErrorProvider.SetError(grossInterestTextBox, "The gross interest amount must be a positive decimal value!");
                calculateInterestButton.Enabled = false;
            } else {
                grossInterestErrorProvider.SetError(grossInterestTextBox, String.Empty);
                calculateInterestButton.Enabled = true;

            }
        }

        private void netInterestTextBox_TextChanged(object sender, EventArgs e) {
            String netInterestResult = netInterestTextBox.Text;

            if(!"".Equals(netInterestResult)) {
                //Enables the 'Copy to field' and 'Copy to clipboard' buttons
                copyToFieldButton.Enabled = true;
                copyToClipboardButton.Enabled = true;
            } else {
                //Disables the 'Copy to field' and 'Copy to clipboard' buttons
                copyToFieldButton.Enabled = false;
                copyToClipboardButton.Enabled = false;
            }
         
        }

        private void resetFieldsButton_Click(object sender, EventArgs e) {
            //Resets all the controls present in the list
            UserControlsManager.clearActiveControls(activeControls);
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void copyToFieldButton_Click(object sender, EventArgs e) {
            String netInterestAmount = netInterestTextBox.Text;
            //populates the specified field from the data insertion form with the value resulted from the net interest calculation
            fieldToPopulate.Text = netInterestAmount;
        }

        private void calculateInterestButton_Click(object sender, EventArgs e) {
            double grossInterestAmount = Convert.ToDouble(grossInterestTextBox.Text);
            double taxPercentage = (double) taxPercentageNumericUpDown.Value;

            double taxAmount = (grossInterestAmount * taxPercentage) / 100;
            double netInterestAmount = Math.Round(grossInterestAmount - taxAmount, 2);
            

            netInterestTextBox.Text = Convert.ToString(netInterestAmount);
        }

        private void copyToClipboardButton_Click(object sender, EventArgs e) {
            String netInterestAmount = netInterestTextBox.Text;
            Clipboard.SetText(netInterestAmount);
        }
    }
}
