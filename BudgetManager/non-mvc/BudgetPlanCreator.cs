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

    public enum BudgetPlanType {
        ONE_MONTH,
        SIX_MONTHS
    }

    public partial class BudgetPlanCreator : Form {
        private CheckBox[] checkBoxes;        
        private int userID;       

        //SQL statement for checking budget plan existence for the same time interval
        private String sqlStatementCheckBudgetPlanExistence = @"SELECT planName, startDate, endDate FROM budget_plans WHERE user_ID = @paramID AND @paramDate BETWEEN startDate AND endDate";
        //SQL statement for inserting a new budget plan into the DB
        private String sqlStatementInsertNewPlanData = @"INSERT INTO budget_plans(user_ID, planName, expenseLimit, debtLimit, savingLimit, planType, hasAlarm, thresholdPercentage, startDate, endDate) VALUES(@paramID, @paramPlanName, @paramExpenseLimit, @paramDebtLimit, @paramSavingLimit, @paramPlanTypeID, @paramAlarmExistence, @paramThresholdPercentage, @paramStartDate, @paramEndDate)";
        //SQL statement for getting the ID for the selected plan type(in order to fill in the data for the previous INSERT statement)
        private String sqlStatementGetBudgetPlanTypeID = @"SELECT typeID FROM plan_types WHERE typeName = @paramTypeName";


        public BudgetPlanCreator(int userID) {
            InitializeComponent();
            checkBoxes = new CheckBox[] { oneMonthCheckBox, sixMonthsCheckBox };
           
            this.userID = userID;           

            //Sets the month selection control to the current month(budget plans cannot be created starting from the past)
            startMonthNumericUpDown.Minimum = DateTime.Now.Month;
           
        }

        private void fillCombobox(ComboBox comboBox, int start, int end) {
            comboBox.Items.AddRange(Enumerable.Range(start, end).Select(i => (object)i).ToArray());
        }

        private void planNameTextBox_TextChanged(object sender, EventArgs e) {
            //Regex for checking word characters
            Regex wordCheck = new Regex("\\w");

            //If no word character is found the textbox is automatically cleared
            if (!wordCheck.IsMatch(planNameTextBox.Text)) {
                planNameTextBox.Text = "";
            }

            decideButtonState();
        }

        private void oneMonthCheckBox_CheckedChanged(object sender, EventArgs e) {
            //Checking the checkbox for one of the plan types automatically unchecks and deactivates the other one
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

        //Method for checking if the required data for plan creation was inserted by the user(checkboxes and budget plan name textbox are the only ones verified because the limit setting controls already have a default value)
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

            //Checks if the plan that the user wants to create has a length smaller or equal to the number of months left from the current year
            if (!hasMonthsLeftForCurrentSelection(startMonthNumericUpDown, oneMonthCheckBox, sixMonthsCheckBox)) {
                MessageBox.Show("The selected budget plan cannot be created if the number of remaining months is lower than the selected plan's length!", "Budget plan creator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }

            //Checks if a budget plan already exists for the selected period
            int selectedMonth = Convert.ToInt32(startMonthNumericUpDown.Value);
            if (hasPlanForCurrenMonthSelection(userID, selectedMonth)) {
                MessageBox.Show("A budget plan already exists for the selected interval! Please select another interval or modify/delete the existing plan", "Budget plan creator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }

            //Asks the user to confirm his intention of creating a new budget plan
            DialogResult createPlanOption = MessageBox.Show("Are you sure that you want to create a new budget plan using the provided data?", "Budget plan creator", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (createPlanOption == DialogResult.No) {
                return;
            }
           
            //Asks the uer if he wants to use the default value of the budget item limit percentage
            NumericUpDown[] budgetItemLimitControls = new NumericUpDown[] { expensesNumericUpDown, debtsNumericUpDown, savingsNumericUpDown };
            DialogResult useDefaultValueOption = MessageBox.Show("One or more controls used for setting the budget item limit percentage is/are set to the default value. By using this value you will limit your possibility of entering records for that item. Are you sure that you want to continue?", "Insert data form", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(useDefaultValueOption == DialogResult.No) {
                return;
            }

            QueryData paramContainer = getDataForBudgetPlanCreation(userID);
            MySqlCommand budgetPlanCreationCommand = SQLCommandBuilder.getBudgetPlanCreationCommand(sqlStatementInsertNewPlanData, paramContainer);

            int executionResult = DBConnectionManager.insertData(budgetPlanCreationCommand);

            if (executionResult != -1) {
                MessageBox.Show("Your new budget plan was successfully created!", "Budget plan creator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("Unable to create the new budget plan! Please try again", "Budget plan creator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }          
        }

    
        private void expensesNumericUpDown_ValueChanged(object sender, EventArgs e) {
          
            setCorrectValue(expensesNumericUpDown);            
        }

        private void debtsNumericUpDown_ValueChanged(object sender, EventArgs e) {
         
            setCorrectValue(debtsNumericUpDown);         
        }

        private void savingsNumericUpDown_ValueChanged(object sender, EventArgs e) {
      
            setCorrectValue(savingsNumericUpDown);
        }

        private void startingMonthComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            decideButtonState();
        }

        private void alarmCheckBox_CheckedChanged(object sender, EventArgs e) {
            //The alarm threshold field is activated only when the alarm checkbox is checked
            if (alarmCheckBox.Checked == true) {
                thresholdNumericUpDown.Enabled = true;

            } else {
                thresholdNumericUpDown.Enabled = false;
            }
        }

        //Method for activating/deactivating the "Create plan" button according to the existence of the required data for budget plan creation
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

        //Method for setting the correct precentage value so that the total sum will never exceeed 100%
        private void setCorrectValue(NumericUpDown targetUpDown) {
            //Calculates the total value of the percentages present in the upDown controls
            int total = Convert.ToInt32(expensesNumericUpDown.Value) + Convert.ToInt32(debtsNumericUpDown.Value) + Convert.ToInt32(savingsNumericUpDown.Value);
            if (total > 100) {
                //Calculates the difference(it will always be negative if total is greater than 100)
                int difference = 100 - total;
                //Subtracts the difference from the value of the last modified upDown control(so that the total sum will always be less than/equal to 100)
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
            //Creates a DateTime object representing the first day of the selected month from the current year
            DateTime startDate = new DateTime(year, month, day);
           

            if (oneMonthCheckBox.Checked == true) {
                //Creates a data container which will be passed to the MySqlCommand builder method(the date is transformed into a string having the format required by the MySql database)               
                QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate.ToString("yyyy-MM-dd")).build(); //CHANGE

                MySqlCommand budgetPlanStartDateCheckCommand = SQLCommandBuilder.getBudgetPlanCheckCommand(sqlStatementCheckBudgetPlanExistence, paramContainer);

                //Executes a MySqlCommand for checking the existence of a budget plan for the selected interval
                DataTable budgetPlanDataTable = DBConnectionManager.getData(budgetPlanStartDateCheckCommand);

                //The DataTable object is checked to see if it contains any results
                if (budgetPlanDataTable != null && budgetPlanDataTable.Rows.Count > 0) {
                    return true;
                }

            } else if (sixMonthsCheckBox.Checked == true) {
                //Gets the last month of the interval               
                int endMonth = month + 6; 
                //Gets the last day of the month               
                int lastDayOfEndMonth = DateTime.DaysInMonth(year, endMonth);
                DateTime endDate = new DateTime(year, endMonth, lastDayOfEndMonth);

                //SQL commands are created for the start month and end month of the interval               
                QueryData paramContainerStartDate = new QueryData.Builder(userID).addStartDate(startDate.ToString("yyyy-MM-dd")).build(); //CHANGE
                MySqlCommand budgetPlanStartDateCheckCommand = SQLCommandBuilder.getBudgetPlanCheckCommand(sqlStatementCheckBudgetPlanExistence, paramContainerStartDate);
               
                QueryData paramContainerEndDate = new QueryData.Builder(userID).addEndDate(endDate.ToString("yyyy-MM-dd")).build();//CHANGE
                MySqlCommand budgetPlanEndDateCheckCommand = SQLCommandBuilder.getBudgetPlanCheckCommand(sqlStatementCheckBudgetPlanExistence, paramContainerEndDate);

                //The commands are executed against the DB
                DataTable budgetPlanDataTableStart = DBConnectionManager.getData(budgetPlanStartDateCheckCommand);
                DataTable budgetPlanDataTableEnd = DBConnectionManager.getData(budgetPlanEndDateCheckCommand);

                //If the start month or the end month is present in the interval specified by an existing budget plan then it means that the two plans overlap and the new plan will not be created.
                if (budgetPlanDataTableStart != null && budgetPlanDataTableStart.Rows.Count > 0 || budgetPlanDataTableEnd != null && budgetPlanDataTableEnd.Rows.Count > 0) {                    
                        return true;                                    
                }
            }

            return false;
        }

        //Method for gathering the required data for budget plan creation(it returns an object of type QueryData which contains the data)
        private QueryData getDataForBudgetPlanCreation(int userID) { 
            //The budget plan type name as it is defined in the plan_types table of the database   
            String budgetPlanTypeName = oneMonthCheckBox.Checked == true ? "One month" : "Six months";
            //The actual name of the budget plan as it was specified by the user
            String budgetPlanName = planNameTextBox.Text;
            int expenseLimit = Convert.ToInt32(expensesNumericUpDown.Value);
            int debtLimit = Convert.ToInt32(debtsNumericUpDown.Value);
            int savingLimit = Convert.ToInt32(savingsNumericUpDown.Value);          
            //The ID for the selected plan type 
            int planTypeID = getBudgetTypeID(budgetPlanTypeName);
            //Indicates if the alarm is activated(0-false; 1-true)
            int alarmSelectionValue = getAlarmSelectionValue();
            //The limit at which the budget alarm will be triggered
            int thresholdPercentage = alarmCheckBox.Checked == true ? Convert.ToInt32(thresholdNumericUpDown.Value) : 0;
            
            String startDate = getDate(getPlanType(), DateType.START_DATE);
            String endDate = getDate(getPlanType(), DateType.END_DATE);
           
            QueryData paramContainer = new QueryData.Builder(userID)
                .addBudgetPlanName(budgetPlanName)
                .addExpenseLimit(expenseLimit)
                .addDebtLimit(debtLimit)
                .addSavingLimit(savingLimit)              
                .addPlanTypeID(planTypeID)
                .addAlarmExistenceValue(alarmSelectionValue)
                .addThresholdPercentage(thresholdPercentage)                
                .addStartDate(startDate)
                .addEndDate(endDate)
                .build(); //CHANGE


            return paramContainer;
        }

        //Method for retrieving the ID for the selected budget plan type
        private int getBudgetTypeID(String budgetPlanTypeName) {          
            MySqlCommand getTypeIDCommand = SQLCommandBuilder.getTypeIDForItemCommand(sqlStatementGetBudgetPlanTypeID, budgetPlanTypeName); //CHANGE
            
            DataTable typeIDDataTable = DBConnectionManager.getData(getTypeIDCommand);

            if (typeIDDataTable != null && typeIDDataTable.Rows.Count == 1) {
                //Null check for the retrieved data(if the element at index 0 of the ItemArray is null then the typeID variable is assigned -1 value)
                int typeID = typeIDDataTable.Rows[0].ItemArray[0] != DBNull.Value ? Convert.ToInt32(typeIDDataTable.Rows[0].ItemArray[0]) : -1;

                return typeID;
            }

            return -1;

        }

        private int getAlarmSelectionValue() {
            int alarmSelectionValue = 0;

            //The hasAlarm column in the budget_plans table is set to boolean(or actually tiny int) so the sero value represents false and non-zero values represent true(here 1 is used as a general accepted value for true)
            if (alarmCheckBox.Checked == true) {
                alarmSelectionValue = 1;
            }

            return alarmSelectionValue;
        }

        //Method for getting the correct date for the budget plan according to user plan type selection and date type (start/end)
        private String getDate(BudgetPlanType planType, DateType dateType) {
            String resultDate = "";
            //One month plans
            if (planType == BudgetPlanType.ONE_MONTH) {
                //The start date is created by taking the first day of the month, the month selected by user and the current year
                if (dateType == DateType.START_DATE) {
                    int day = 1;
                    int month = Convert.ToInt32(startMonthNumericUpDown.Value);
                    int year = DateTime.Now.Year;

                    //DateTime object is created using the month selected by the user
                    DateTime startDate = new DateTime(year, month, day);

                    //The DateTime object is converted to its String representation having the format required by the MySQL database
                    resultDate = startDate.ToString("yyyy-MM-dd");

                   //The end date is created by taking the last day of the month, the month selected by user and the current year
                } else if (dateType == DateType.END_DATE) {                 
                    int month = Convert.ToInt32(startMonthNumericUpDown.Value);
                    int year = DateTime.Now.Year;
                    int day = DateTime.DaysInMonth(year, month);

                    DateTime startDate = new DateTime(year, month, day);

                    resultDate = startDate.ToString("yyyy-MM-dd");
                }
                //Six months plans
            } else if (planType == BudgetPlanType.SIX_MONTHS) {
                //The start date is created by taking the first day of the month, the month selected by user and the current year
                if (dateType == DateType.START_DATE) {
                    int day = 1;
                    int month = Convert.ToInt32(startMonthNumericUpDown.Value);
                    int year = DateTime.Now.Year;

                    DateTime startDate = new DateTime(year, month, day);

                    resultDate = startDate.ToString("yyyy-MM-dd");

                  //The end date is created by taking the last day of the month, the sixth month after the one selected by user and the current year
                } else if (dateType == DateType.END_DATE) {
                    int month = Convert.ToInt32(startMonthNumericUpDown.Value) + 6;
                    int year = DateTime.Now.Year;
                    int day = DateTime.DaysInMonth(year, month);

                    DateTime startDate = new DateTime(year, month, day);

                    resultDate = startDate.ToString("yyyy-MM-dd");
                }
            }

            return resultDate;
        }

        //Method for getting the correct BudgetPlanType enum value according to the user checkbox selection
        private BudgetPlanType getPlanType() {           

            if (oneMonthCheckBox.Checked == true) {
                return BudgetPlanType.ONE_MONTH;
            } else {
                return BudgetPlanType.SIX_MONTHS;
            }
        }

        //Method for checking if any of the controls used for setting budget item percentage limits are set to default value(which is 1)
        private bool hasDefaultLimitForItem(NumericUpDown[] budgetItemLimitControls) {
            foreach (NumericUpDown limitControl in budgetItemLimitControls) {
                if (limitControl.Value == limitControl.Minimum) {
                    return true;
                }
            }

            return false;
        }
    }
}
