using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    public partial class BudgetPlanCreator : Form {
        private CheckBox[] checkBoxes;
        private ComboBox[] comboBoxes;
        private int userID;
        private bool hasResetLimits;

        //SQL statements for checking budget plan existence for the same time interval
        private String sqlStatementCheckBudgetPlanExistence = @"SELECT planName, startDate, endDate FROM budget_plans WHERE user_ID = @paramID AND @paramStartDate BETWEEN startDate AND endDate";

        public BudgetPlanCreator(int userID) {
            InitializeComponent();
            checkBoxes = new CheckBox[] { oneMonthCheckBox, sixMonthsCheckBox };
           
            this.userID = userID;
            hasResetLimits = false;

            startMonthNumericUpDown.Minimum = DateTime.Now.Month;

            //fillCombobox(startMonthNumericUpDown, 1, 13);
            //fillCombobox(expensesLimitComboBox, 0, 101);
            //fillCombobox(debtsLimitComboBox, 0, 101);
            //fillCombobox(savingsLimitComboBox, 0, 101);
        }

        private void fillCombobox(ComboBox comboBox, int start, int end) {
            comboBox.Items.AddRange(Enumerable.Range(start, end).Select(i => (object)i).ToArray());
        }

        private void planNameTextBox_TextChanged(object sender, EventArgs e) {
            Regex wordCheck = new Regex("\\w");

            if (!wordCheck.IsMatch(planNameTextBox.Text)) {
                planNameTextBox.Text = "";
            }

            decideButtonState();
        }

        private void oneMonthCheckBox_CheckedChanged(object sender, EventArgs e) {
            sixMonthsCheckBox.Checked = false;
            if (oneMonthCheckBox.Checked == true) {
                disableCheckBox(sixMonthsCheckBox);
            } else {
                enableCheckBox(sixMonthsCheckBox);
            }

            decideButtonState();
        }

        private void sixMonthsCheckBox_CheckedChanged(object sender, EventArgs e) {
            oneMonthCheckBox.Checked = false;
            if (sixMonthsCheckBox.Checked == true) {
                disableCheckBox(oneMonthCheckBox);
            } else {
                enableCheckBox(oneMonthCheckBox);
            }

            decideButtonState();
        }

        private void disableCheckBox(CheckBox checkBox) {
            checkBox.Enabled = false;
        }

        private void enableCheckBox(CheckBox checkBox) {
            checkBox.Enabled = true;
        }

        private bool hasRequiredData() {
            bool hasRequiredData = true;

            if (oneMonthCheckBox.Checked  == false && sixMonthsCheckBox.Checked == false) {
                hasRequiredData = false;
            }

            if ("".Equals(planNameTextBox.Text)) {
                hasRequiredData = false;
            }


            return hasRequiredData;
        }

        private void createPlanButton_Click(object sender, EventArgs e) {
            //Checks if the total value of the set percentages is equal to 100
            int totalPercentageValue = Convert.ToInt32(expensesNumericUpDown.Value) + Convert.ToInt32(debtsNumericUpDown.Value) + Convert.ToInt32(savingsNumericUpDown.Value);
            if (totalPercentageValue < 100) {
                MessageBox.Show("The total value of the set percentages must be equal to 100! Please fill in the remaining value by assigning it to one or more elements.", "Budget plan creator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }

            if (!hasMonthsLeftForCurrentSelection(startMonthNumericUpDown, oneMonthCheckBox, sixMonthsCheckBox)) {
                MessageBox.Show("The selected budget plan cannot be created if the number of remaining months is lower than the selected plan's length!", "Budget plan creator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }

            int selectedMonth = Convert.ToInt32(startMonthNumericUpDown.Value);
            if (hasPlanForCurrenMonthSelection(userID, selectedMonth)) {
                MessageBox.Show("A budget plan already exists for the selected interval! Please select another interval or modify/delete the existing plan", "Budget plan creator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }


        }

    
        private void expensesNumericUpDown_ValueChanged(object sender, EventArgs e) {
            //int total = Convert.ToInt32(expensesNumericUpDown.Value) + Convert.ToInt32(debtsNumericUpDown.Value)+ Convert.ToInt32(savingsNumericUpDown.Value);
            //if (total > 100) {
            //    int difference = 100 - total;
            //    expensesNumericUpDown.Value = expensesNumericUpDown.Value + difference;

            //}   
            setCorrectValue(expensesNumericUpDown);            
        }

        private void debtsNumericUpDown_ValueChanged(object sender, EventArgs e) {
            //int total = Convert.ToInt32(expensesNumericUpDown.Value) + Convert.ToInt32(debtsNumericUpDown.Value) + Convert.ToInt32(savingsNumericUpDown.Value);
            //if (total > 100) {
            //    int difference = 100 - total;
            //    debtsNumericUpDown.Value = debtsNumericUpDown.Value + difference;              
            //} 
            setCorrectValue(debtsNumericUpDown);         
        }

        private void savingsNumericUpDown_ValueChanged(object sender, EventArgs e) {
            //int total = Convert.ToInt32(expensesNumericUpDown.Value) + Convert.ToInt32(debtsNumericUpDown.Value) + Convert.ToInt32(savingsNumericUpDown.Value);
            //if (total > 100) {
            //    int difference = 100 - total;
            //    savingsNumericUpDown.Value = savingsNumericUpDown.Value + difference;               
            //}
            setCorrectValue(savingsNumericUpDown);
        }

        private void startingMonthComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            decideButtonState();
        }


        private void decideButtonState() {
            if (hasRequiredData()) {
                setButtonState(createPlanButton, true);
            } else {
                setButtonState(createPlanButton, false);
            }
        }

        private void setButtonState(Button button, bool enabled) {
            button.Enabled = enabled;
        }

        private int calculateLimit(int setValue, int totalValue,int remainingControls) {
            if (remainingControls == 0 || totalValue < setValue) {
                return -1;
            }

            int availableAmount = totalValue - setValue;
            int limit = 0;

            if (availableAmount == 0) {
                return limit;
            }

            limit = availableAmount / remainingControls;

            return limit;

        } 

        private void setCorrectValue(NumericUpDown targetUpDown) {
            //Calculates the total value of the percentages present in the upDown controls
            int total = Convert.ToInt32(expensesNumericUpDown.Value) + Convert.ToInt32(debtsNumericUpDown.Value) + Convert.ToInt32(savingsNumericUpDown.Value);
            if (total > 100) {
                //Calculates the difference(it will always be negative if total is greater than 100)
                int difference = 100 - total;
                //Subtracts the difference from the value of the last modified upDown control(so that the total sum will always less than/equal to 100)
                targetUpDown.Value = targetUpDown.Value + difference;
            }
        }

        private bool hasMonthsLeftForCurrentSelection(NumericUpDown upDownControl, CheckBox oneMonthCheckBox, CheckBox sixMonthsCheckBox) {
            if (oneMonthCheckBox.Checked == true) {
                return true;
            } else if (sixMonthsCheckBox.Checked == true) {
                int currentSelectedMonth = Convert.ToInt32(upDownControl.Value);
                int planEndingMonth = currentSelectedMonth + 6;

                if (planEndingMonth <= 12) {
                    return true;
                }
            }

            return false;
        }


        private bool hasPlanForCurrenMonthSelection(int userID, int month) {            
            int year = DateTime.Now.Year;
            int day = 1;
            DateTime selectedValueDate = new DateTime(year, month, day);
            //DateTime sqlFormatDate = DateTime.ParseExact(selectedValueDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            QueryData paramContainer = new QueryData(userID, selectedValueDate.ToString("yyyy-MM-dd"));
            MySqlCommand budgetPlanCheckCommand = SQLCommandBuilder.getBudgetPlanCheckCommand(sqlStatementCheckBudgetPlanExistence, paramContainer);

            DataTable budgetPlanDataTable = DBConnectionManager.getData(budgetPlanCheckCommand);

            if (budgetPlanDataTable != null && budgetPlanDataTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }
      
    }
}
