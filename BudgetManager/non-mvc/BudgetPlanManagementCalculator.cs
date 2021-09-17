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
    public partial class BudgetPlanManagementCalculator : Form {
        private TextBox[] textBoxesToReset;
        private TextBox[] textBoxesToCheck;
        private bool hasClearedFields;

        public BudgetPlanManagementCalculator() {
            InitializeComponent();
            //Creates an array of text boxes for further use by the reset button and hasDataOnActiveFields() method
            textBoxesToReset = new TextBox[] { inputValueOrPercentageTextBox, totalIncomesTextBox, resultTextBox};
            //Creates an array of text boxes for further use by the hasDataOnActiveFields() method
            textBoxesToCheck = new TextBox[] { inputValueOrPercentageTextBox, totalIncomesTextBox};
            //Sets the calculation mode combobox item to the first item present in the list
            calculationModeComboBox.SelectedIndex = 0;
            //Sets the flag that keeps track of the field clearing actions to false
            hasClearedFields = false;
        }

        private void inputValueOrPercentageTextBox_TextChanged(object sender, EventArgs e) {
            checkTextBox(inputValueOrPercentageTextBox);
            int selectedIndex = calculationModeComboBox.SelectedIndex;

            //Checks if the input fields contain data and if so enables the "Calculate" button
            if (hasDataOnActiveFields(textBoxesToCheck) && selectedIndex > 0) {
                calculateButton.Enabled = true;
            } else {
                calculateButton.Enabled = false;
            }   
        }

        private void totalIncomesTextBox_TextChanged(object sender, EventArgs e) {
            checkTextBox(totalIncomesTextBox);
            int selectedIndex = calculationModeComboBox.SelectedIndex;

            if (hasDataOnActiveFields(textBoxesToCheck) && selectedIndex > 0) {
                calculateButton.Enabled = true;
            } else {
                calculateButton.Enabled = false;
            }
        }

        private void calculationModeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            //Clears the fields when each time a new calculation mode is selected
            resetFields(textBoxesToReset);
            int selectedIndex = calculationModeComboBox.SelectedIndex;

            
            if (selectedIndex == 0) {
                calculateButton.Enabled = false;

              //Input percentage to value option selected
            } else if (selectedIndex == 1) {
                //Sets the text value of the label near the textbox used to input the percentage or value
                inputPercentageOrValueLabel.Text = "Input percentage";
                //Sets the result label text
                resultLabel.Text = "Result value";
                //Shows the percentage value label on the result field
                percentSymbolLabel1.Visible = true;
                percentSymbolLabel2.Visible = false;

              //Input value to percentage option selected
            } else if (selectedIndex == 2) {
                inputPercentageOrValueLabel.Text = "Input value";
                resultLabel.Text = "Result percentage";
                percentSymbolLabel1.Visible = false;
                percentSymbolLabel2.Visible = true;
            }


        }

        private void resetButton_Click(object sender, EventArgs e) {
            //Clears the text boxes present in the array
            //foreach (TextBox textBox in textBoxes) {
            //    textBox.Text = "";
            //}
            resetFields(textBoxesToReset);

            calculationModeComboBox.SelectedIndex = 0;
            //Sets the flag that indicates the field reset
            hasClearedFields = true;
        }

        private void calculateButton_Click(object sender, EventArgs e) {
            BudgetPlanChecker planChecker = new BudgetPlanChecker();

            if (calculationModeComboBox.SelectedIndex == 1) {
                int inputPercentage = Convert.ToInt32(inputValueOrPercentageTextBox.Text);

                //Checks if the percentage input value exceds 100%(only if the "Input percentage to value" mode is selected)
                if (inputPercentage > 100) {
                    MessageBox.Show("The input percentage cannot exceed 100%! Please change the value and try again.", "Plan management calculator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int totalIncomes = Convert.ToInt32(totalIncomesTextBox.Text);              
              
                int result = planChecker.calculateValueFromPercentage(totalIncomes, inputPercentage);

                resultTextBox.Text = Convert.ToString(result);

            } else if (calculationModeComboBox.SelectedIndex == 2) {
                int inputValue = Convert.ToInt32(inputValueOrPercentageTextBox.Text);
                int totalIncomes = Convert.ToInt32(totalIncomesTextBox.Text);
                
                int result = planChecker.calculateCurrentItemPercentageValue(inputValue, totalIncomes);

                resultTextBox.Text = Convert.ToString(result);

            }

        }

        private void checkTextBox(TextBox textBox) {
            if (textBox == null) {
                return;
            }
            //Regex to check if the text box contains only digits
            Regex inputDataChecker = new Regex("\\b\\d+\\b");

            //If the input contains any other characters apart from digits then the textbox content will be cleared automatically
            if (!inputDataChecker.IsMatch(textBox.Text)) {
                textBox.Text = "";
            }
        }


        private bool hasDataOnActiveFields(TextBox[] fields) {
            if (hasClearedFields) {
                hasClearedFields = false;
                return false;
            }

            foreach (TextBox field in fields) {
                if (field.Enabled == true && "".Equals(field.Text)) {
                    return false;
                }
            }

            return true;
        }
        

        private void resetFields(params TextBox[] textBoxes) {

            foreach(TextBox textBox in textBoxes) {
                textBox.Text = "";
            } 
        }
    }
}
