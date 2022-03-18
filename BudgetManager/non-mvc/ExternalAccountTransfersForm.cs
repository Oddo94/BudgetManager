using BudgetManager.utils;
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

        public ExternalAccountTransfersForm(int userID) {
            InitializeComponent();
            this.userID = userID;
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
            Regex exchangeRateRegex = new Regex("^\\d+\\.\\d+$");

            if (!isValidInputAmount(exchangeRateValue, exchangeRateRegex)) {               
                exchangeRateTextBox.BackColor = Color.Red;
            } else {
                exchangeRateTextBox.BackColor = Color.White;
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
    }
}
