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

namespace BudgetManager {

    public enum BudgetItemType {
        INCOME,
        EXPENSE,
        DEBT,
        SAVING,
        UNDEFINED
    }

    public enum IncomeSource {
        GENERAL_INCOMES,
        SAVING_ACCOUNT,
        UNDEFINED
    }

    public partial class InsertDataForm : Form {

        private int userID;
        private Control[] inputFields;
        private bool hasClearedFields = false;

        //SQL queries used for selecting the values that fill comboboxes
        private String sqlStatementSelectIncomeTypes = @"SELECT typeName FROM income_types";
        private String sqlStatementSelectExpenseTypes = @"SELECT categoryName FROM expense_types";
        private String sqlStatementSelectCreditors = @"SELECT creditorName
                FROM users INNER JOIN users_creditors ON users.userID = users_creditors.user_ID
                INNER JOIN creditors ON users_creditors.creditor_ID = creditors.creditorID
                WHERE users_creditors.user_ID = @paramUserID";
        private String sqlStatementCheckCreditorExistence = @"SELECT creditorName FROM creditors WHERE creditorName = @paramCreditorName";

        //SQL queries for selecting the ID's used in the INSERT commands
        private String sqlStatementSelectIncomeTypeID = @"SELECT typeID FROM income_types WHERE typeName = @paramTypeName";
        private String sqlStatementSelectExpenseTypeID = @"SELECT categoryID FROM expense_types WHERE categoryName = @paramTypeName";
        private String sqlStatementSelectCreditorID = @"SELECT creditorID FROM creditors WHERE creditorName = @paramTypeName";

        //SQL queries for checking the existence of a creditor in the current user creditor list
        private String sqlStatementCheckCreditorExistenceInUserList = @"SELECT user_ID, creditor_ID FROM users_creditors WHERE user_ID = @paramUserID AND creditor_ID = @paramCreditorID";

        //SQL queries for inserting incomes, expenses, debts and savings
        private String sqlStatementInsertIncome = @"INSERT INTO incomes(user_ID, name, incomeType, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";
        private String sqlStatementInsertGeneralIncomesExpense = @"INSERT INTO expenses(user_ID, name, type, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";
        private String sqlStatementInsertSavingAccountExpense = @"INSERT INTO saving_account_expenses(user_ID, name, type, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";
        private String sqlStatementInsertDebt = @"INSERT INTO debts(user_ID, name, value, creditor_ID, date) VALUES(@paramID, @paramDebtName, @paramDebtValue, @paramCreditorID, @paramDebtDate)";
        private String sqlStatementInsertSaving = @"INSERT INTO savings(user_ID, name, value, date) VALUES(@paramID, @paramSavingName, @paramSavingValue, @paramSavingDate)";
        private String sqlStatementInsertCreditor = @"INSERT INTO creditors(creditorName) VALUES(@paramCreditorName)";
        private String sqlStatementInsertCreditorID = @"INSERT INTO users_creditors(user_ID, creditor_ID) VALUES(@paramUserID, @paramCreditorID)";

        //SQL queries for getting the total value of budget elements for a month in order to allow further checks
        private String sqlStatementSingleMonthIncomes = @"SELECT SUM(value) from incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthExpenses = @"SELECT SUM(value) from expenses WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthDebts = @"SELECT SUM(value) from debts WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthSavings = @"SELECT SUM(value) from savings WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";

        //SQL query to get the saving account current balance value in order to allow further checks when the user selects the saving account as the income source for the inserted expense
        private String sqlStatementGetSavingAccountBalance = @"SELECT SUM(value) FROM saving_account_balance WHERE user_ID = @paramID AND year <= @paramYear";

        public InsertDataForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            inputFields = new Control[] { nameTextBox, valueTextBox, incomeTypeComboBox, expenseTypeComboBox, creditorNameComboBox };

            //Filling incomeTypeComboBox with income types
            MySqlCommand fillIncomeTypesCommand = new MySqlCommand(sqlStatementSelectIncomeTypes);
            DataTable dTableIncomeTypes = DBConnectionManager.getData(fillIncomeTypesCommand);

            fillComboBox(incomeTypeComboBox, dTableIncomeTypes);

            //Filling expenseTypeComboBox with expense types
            MySqlCommand fillExpenseTypesCommand = new MySqlCommand(sqlStatementSelectExpenseTypes);
            DataTable dTableExpenseTypes = DBConnectionManager.getData(fillExpenseTypesCommand);

            fillComboBox(expenseTypeComboBox, dTableExpenseTypes);

            fillCreditorsComboBox();

            //Deactivating data input fields
            toggleInputFormFieldsState(inputFields, false);
        }


        //CONTROLS METHODS      
        //When selecting a new option all the data input fields are cleared and then only the necessary fields for entering the selected element are activated       
        private void budgetItemComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedIndex = budgetItemComboBox.SelectedIndex;

            switch (selectedIndex) {
                //Incomes
                case 0:
                    clearFields(inputFields);//Clearing the previously filled input fields
                    toggleInputFormFieldsState(inputFields, true);//Activating all the fields                    
                    expenseTypeComboBox.Enabled = false;//Deactivating the fields that are not necessary for the insertion of the currently selected element
                    generalIncomesRadioButton.Enabled = false;
                    savingAccountRadioButton.Enabled = false;
                    creditorNameComboBox.Enabled = false;
                    break;

                //Expenses
                case 1:
                    clearFields(inputFields);
                    toggleInputFormFieldsState(inputFields, true);
                    incomeTypeComboBox.Enabled = false;
                    generalIncomesRadioButton.Enabled = true;
                    savingAccountRadioButton.Enabled = true;
                    creditorNameComboBox.Enabled = false;
                    break;

                //Debts
                case 2:
                    clearFields(inputFields);
                    fillCreditorsComboBox();
                    toggleInputFormFieldsState(inputFields, true);
                    incomeTypeComboBox.Enabled = false;
                    generalIncomesRadioButton.Enabled = false;
                    savingAccountRadioButton.Enabled = false;
                    expenseTypeComboBox.Enabled = false;
                    break;

                //Savings
                case 3:
                    clearFields(inputFields);
                    toggleInputFormFieldsState(inputFields, true);
                    incomeTypeComboBox.Enabled = false;
                    expenseTypeComboBox.Enabled = false;
                    generalIncomesRadioButton.Enabled = false;
                    savingAccountRadioButton.Enabled = false;
                    creditorNameComboBox.Enabled = false;
                    break;

                //Creditors
                case 4:
                    clearFields(inputFields);
                    toggleInputFormFieldsState(inputFields, true);
                    valueTextBox.Enabled = false;
                    incomeTypeComboBox.Enabled = false;
                    expenseTypeComboBox.Enabled = false;
                    generalIncomesRadioButton.Enabled = false;
                    savingAccountRadioButton.Enabled = false;
                    creditorNameComboBox.Enabled = false;
                    break;
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e) {
            //Regex for matching digits
            Regex numberRegex = new Regex("\\b[\\d]+\\b", RegexOptions.Compiled);

            //If the input contains any other characters apart from digits then the textbox content will be cleared automatically
            if (!numberRegex.IsMatch(valueTextBox.Text)) {
                valueTextBox.Text = "";
            }
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }


        private void incomeTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void expenseTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void creditorNameComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void addEntryButton_Click(object sender, EventArgs e) {
            int executionResult = -1;
            //The index of the currently selected element
            int selectedItemIndex = budgetItemComboBox.SelectedIndex;

            DialogResult userOptionConfirmInsertion = MessageBox.Show("Are you sure that you want to insert the provided data?", "Data insertion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOptionConfirmInsertion == DialogResult.No) {
                return;
            }


            String selectedItem = budgetItemComboBox.Text;
            //If the user wants to insert a creditor or an income then the available amount check is no longer performed
            if (!"Creditor".Equals(selectedItem, StringComparison.InvariantCultureIgnoreCase) && !"Income".Equals(selectedItem, StringComparison.InvariantCultureIgnoreCase)) {
                //Checks if the user has enough money left to insert the selected item value
                int insertedValue = Convert.ToInt32(valueTextBox.Text);
                int selectedMonth = newEntryDateTimePicker.Value.Month;
                int selectedYear = newEntryDateTimePicker.Value.Year;
                //QueryData paramContainer = new QueryData(userID, selectedMonth, selectedYear);
                QueryData paramContainer = new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).build(); //CHANGE

                /****SAVING ACCOUNT SOURCE****/
                if (getIncomeSource() == IncomeSource.SAVING_ACCOUNT) {
                    if (!hasEnoughMoney(IncomeSource.SAVING_ACCOUNT, insertedValue, paramContainer)) {
                        MessageBox.Show("The inserted value is higher than the money left in the saving account! You cannot exceed the currently available balance of the saving account.", "Data insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        return;
                    }

                    executionResult = insertSelectedItem(selectedItemIndex);

                } else if (getIncomeSource() == IncomeSource.GENERAL_INCOMES) {
                    /****GENERAL INCOMES SOURCE****/
                    //GENERAL CHECK(item value(expense, debt, saving) > available amount)
                    //Checks if the inserted item value is greater than the amount of money left 
                    if (!hasEnoughMoney(IncomeSource.GENERAL_INCOMES, insertedValue, paramContainer)) {
                        MessageBox.Show(String.Format("The inserted value for the current {0} is higher than the money left! You cannot exceed the maximum incomes for the current month.", selectedItem.ToLower()), "Data insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //BUDGET PLAN CHECKS
                    String entryDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");
                    int entryValue = Convert.ToInt32(valueTextBox.Text);

                    BudgetPlanChecker planChecker = new BudgetPlanChecker(userID, entryDate);

                    //Checks if a budget plan exists for the month selected when inserting the item
                    if (planChecker.hasBudgetPlanForSelectedMonth()) {
                        //Gets the plan data for the currently applicable budget plan
                        DataTable budgetPlanDataTable = planChecker.getBudgetPlanData();
                        //Extracts the start date and end date of the budget plan into a String array
                        String[] budgetPlanBoundaries = planChecker.getBudgetPlanBoundaries(budgetPlanDataTable);

                        //Checks if the array containing the budget plan start and end date contains data
                        if (budgetPlanBoundaries != null) {
                            //Calculates the total incomes for the selected time interval
                            int totalIncomes = planChecker.getTotalIncomes(budgetPlanBoundaries[0], budgetPlanBoundaries[1]);
                            //Extracts the percentage limit that was set in the budget plan for the currently selected item
                            int percentageLimitForItem = planChecker.getPercentageLimitForItem(getSelectedType(budgetItemComboBox));
                            //Calculates the actual limit value for the currently selected item based on the previously extracted percentage
                            int limitValueForSelectedItem = planChecker.calculateMaxLimitValue(totalIncomes, percentageLimitForItem);

                            //Checks if an alarm was set for the current budget plan
                            if (planChecker.hasBudgetPlanAlarm(budgetPlanDataTable)) {
                                //Extracts the threshold percentage set in the budget plan for the triggering of the alarm
                                int thresholdPercentage = planChecker.getThresholdPercentage(budgetPlanDataTable);
                                //Calculates the sum of the existing database records for the currently selected item(expense, debt, saving) in the specified time interval
                                int currentItemTotalValue = planChecker.getTotalValueForSelectedItem(getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1]);
                                //Calculates the actual threshold value at which the alarm will be triggered
                                int thresholdValue = planChecker.calculateValueFromPercentage(limitValueForSelectedItem, thresholdPercentage);

                                //Calculates the value which will result after adding the current user input value for the selected item to the sum of the existing database records
                                int futureItemTotalValue = currentItemTotalValue + entryValue;

                                //Checks if the previously calculated value is between the threshold value and the max limit for the selected item(as calculated based on the percentage set in the budget plan)
                                if (planChecker.isBetweenThresholdAndMaxLimit(futureItemTotalValue, thresholdValue, limitValueForSelectedItem)) {
                                    //Calculates the percentage of the futureItemTotalValue
                                    int currentItemPercentageValue = planChecker.calculateCurrentItemPercentageValue(futureItemTotalValue, limitValueForSelectedItem);
                                    //Calculates the difference between the previous percentage value and the threshold percentage(in order to show the percentage by which the threshold is exceeded) 
                                    int percentageDifference = currentItemPercentageValue - thresholdPercentage;
                                    DialogResult userOptionExceedThreshold = MessageBox.Show(String.Format("By inserting the current {0} you will exceed the alarm threshold by {1}%. Are you sure that you want to continue?", selectedItem.ToLower(), percentageDifference), "Insert data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                    if (userOptionExceedThreshold == DialogResult.No) {
                                        return;
                                    } else {
                                        //If the user confirms that he agrees to exceed the threshold the value is inserted in the DB
                                        executionResult = insertSelectedItem(selectedItemIndex);
                                    }

                                } else {
                                    //If the futureItemTotalValue is above the limit set in the budget plan a warning message is shown and no value is inserted
                                    if (planChecker.exceedsItemLimitValue(entryValue, limitValueForSelectedItem, getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1])) {
                                        MessageBox.Show(String.Format("Cannot insert the provided {0} since it would exceed the {1}% limit imposed by the currently applicable budget plan! Please revise the plan or insert a lower value.", selectedItem.ToLower(), percentageLimitForItem), "Insert data form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                        //If the futureItemTotalValue is not between threshold limit and limit value and it doesn't exceed the limit value for the selected item it means that it can be inserted in the DB
                                    } else {
                                        executionResult = insertSelectedItem(selectedItemIndex);
                                    }
                                }
                            } else {
                                //If the plan doesn't contain an alarm a check is made to see if the future total value for the item (user input value + sum of existing database records for the selected item) is greater than the max limit for the selected item(as calculated based on the percentage set in the budget plan)
                                if (planChecker.exceedsItemLimitValue(entryValue, limitValueForSelectedItem, getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1])) {
                                    MessageBox.Show(String.Format("Cannot insert the provided {0} since it would exceed the {1}% limit imposed by the currently applicable budget plan! Please revise the plan or insert a lower value.", selectedItem.ToLower(), percentageLimitForItem), "Insert data form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                    //If the value is less than the limit then it is inserted in the DB
                                } else {
                                    executionResult = insertSelectedItem(selectedItemIndex);
                                }
                            }
                            //If the String array containing the start and end dates for the budget plan is null then no record is inserted in the database since no check can be performed to see if the limits imposed through it are respected
                        } else {
                            MessageBox.Show("Unable to retrieve the start and end dates of the current budget plan! Please revise the plan before trying to insert new data.", "Insert data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //If there is no budget plan for the selected month/the existing budget plan doesn't have proper start/end dates it means that no additional checks are made and the value can be inserted(the general check is already passed at this point)
                    } else {
                        executionResult = insertSelectedItem(selectedItemIndex);
                    }
                }
             //If the user wants to insert a creditor or an income in the database no additional checks are made and the value is inserted               
            } else {
                executionResult = insertSelectedItem(selectedItemIndex);
            }



            //Checks the execution result resturned by the insertion method(positive value mean success while -1 means the failure of the operation)
            if (executionResult != -1) {
                MessageBox.Show("Data inserted successfully!", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("Unable to insert the input data! Please try again.", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }  

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Visible = false;
        }

        private void resetButton_Click(object sender, EventArgs e) {
            clearFields(inputFields);
        }


        //UTIL METHODS
        private void fillBudgetItemElements(ComboBox cBox, String[] inputElements) {

            foreach (String element in inputElements) {
                cBox.Items.Add(element);
            }
        }

        private void setComponentVisibility(Control[] components, Boolean isVisible) {
            foreach (Control currentComponent in components) {
                currentComponent.Visible = isVisible;
            }

        }

        private void toggleInputFormFieldsState(Control[] fields, bool enabled) {
            foreach (Control field in fields) {
                field.Enabled = enabled;
            }

        }

        private void clearFields(Control[] fields) {
            hasClearedFields = true;
            foreach (Control field in fields) {
                if (field.GetType() == typeof(ComboBox)) {
                    ((ComboBox)field).SelectedIndex = -1;
                }

                field.Text = "";

            }
        }

        private bool hasDataOnActiveFields(Control[] fields) {
            if (hasClearedFields) {
                hasClearedFields = false;
                return false;
            }

            foreach (Control field in fields) {
                if (field.Enabled == true && "".Equals(field.Text)) {
                    return false;
                }
            }

            return true;
        }

        private void fillComboBox(ComboBox comboBox, DataTable dataTable) {

            for (int i = 0; i < dataTable.Rows.Count; i++) {
                String currentElement = (String)dataTable.Rows[i].ItemArray[0];
                comboBox.Items.Add(currentElement);
            }

        }

        private void fillCreditorsComboBox() {
            MySqlCommand fillCreditorsCommand = new MySqlCommand(sqlStatementSelectCreditors);
            fillCreditorsCommand.Parameters.AddWithValue("@paramUserID", userID);

            DataTable dTableCreditors = DBConnectionManager.getData(fillCreditorsCommand);

            creditorNameComboBox.DataSource = dTableCreditors;
            creditorNameComboBox.DisplayMember = "creditorName";
        }

        private int getID(String sqlStatement, String typeName) {
            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", typeName);

            DataTable typeIDTable = DBConnectionManager.getData(getTypeIDCommand);

            if (typeIDTable != null && typeIDTable.Rows.Count == 1) {
                int typeID = Convert.ToInt32(typeIDTable.Rows[0].ItemArray[0]);
                return typeID;
            }


            return -1;
        }

        private int insertSelectedItem(int selectedItemIndex) {
            int executionResult = -1;
            switch (selectedItemIndex) {
                //Income insertion
                case 0:              
                    executionResult = insertIncome();
                    break;

                //Expense insertion
                //CHANGE TO ALLOW THE CORRECT SELECTION OF EXPENSE INSERTION STATEMENT
                case 1:
                    if(getIncomeSource() == IncomeSource.GENERAL_INCOMES) {
                        executionResult = insertExpense(sqlStatementInsertGeneralIncomesExpense);//Uses the SQL statement that inserts the expense in the expenses table of the DB
                    } else if (getIncomeSource() == IncomeSource.SAVING_ACCOUNT) {
                        executionResult = insertExpense(sqlStatementInsertSavingAccountExpense);//Uses SQL statement that inserts the expense in the saving_account_expenses table of the DB
                    }                                             
                    break;

                //Debt insertion
                case 2:                   
                    executionResult = insertDebt();
                    break;

                //Saving insertion
                case 3:                 
                    executionResult = insertSaving();                
                    break;

                //New creditor insertion
                case 4:
                    executionResult = insertCreditor();
                    break;

                default:                
                    break;
            }

            return executionResult;
        }

        private int insertIncome() {
            //Getting the necessary data
            String incomeName = nameTextBox.Text;
            int incomeTypeID = getID(sqlStatementSelectIncomeTypeID, incomeTypeComboBox.Text);//Ia ca argumente fraza SQL si denumirea tipului de venit selectat
            int incomeValue = Convert.ToInt32(valueTextBox.Text);
            String incomeDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd"); //Obtinere data sub forma de String

            //Creating command for income insertion
            MySqlCommand incomeInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementInsertIncome, userID, incomeName, incomeTypeID, incomeValue, incomeDate);
            //Getting the execution command result
            int executionResult = DBConnectionManager.insertData(incomeInsertionCommand);

            return executionResult;
        }


        private int insertExpense(String sqlStatement) {
            //Getting the necessary data
            String expenseName = nameTextBox.Text;
            int expenseTypeID = getID(sqlStatementSelectExpenseTypeID, expenseTypeComboBox.Text);
            int expenseValue = Convert.ToInt32(valueTextBox.Text);
            String expenseDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");//Getting date as String

            //Creating command for expense insertion
            MySqlCommand expenseInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatement, userID, expenseName, expenseTypeID, expenseValue, expenseDate);
            //Getting the execution command result
            int executionResult = DBConnectionManager.insertData(expenseInsertionCommand);

            return executionResult;
        }

        private int insertDebt() {
            //Getting the necessary data
            String debtName = nameTextBox.Text;
            int debtValue = Convert.ToInt32(valueTextBox.Text);
            int creditorID = getID(sqlStatementSelectCreditorID, creditorNameComboBox.Text);
            String debtDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");

            //Creating command for debt insertion
            MySqlCommand debtInsertionCommand = SQLCommandBuilder.getDebtInsertionCommand(sqlStatementInsertDebt, userID, debtName, debtValue, creditorID, debtDate);
            int executionResult = DBConnectionManager.insertData(debtInsertionCommand);

            return executionResult;
        }

        private int insertSaving() {
            //Getting the necessary data
            String savingName = nameTextBox.Text;
            int savingValue = Convert.ToInt32(valueTextBox.Text);
            String savingDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");

            //Creating command for saving insertion
            MySqlCommand savingInsertionCommand = SQLCommandBuilder.getSavingInsertionCommand(sqlStatementInsertSaving, userID, savingName, savingValue, savingDate);
            int executionResult = DBConnectionManager.insertData(savingInsertionCommand);

            return executionResult;
        }

        private int insertCreditor() {
            int executionResult = -1;
            //Checks if the entered creditor name exists in the database
            MySqlCommand creditorSelectionCommand = new MySqlCommand(sqlStatementCheckCreditorExistence);
            creditorSelectionCommand.Parameters.AddWithValue("@paramCreditorName", nameTextBox.Text);
            if (entryIsPresent(creditorSelectionCommand, nameTextBox.Text)) {
                DialogResult userChoice = MessageBox.Show("The provided creditor name already exists. Do you want to add it to your creditors list?", "Data insertion", MessageBoxButtons.YesNoCancel);
                if (userChoice == DialogResult.Yes) {
                    //Checks if the creditor is already present in the current user creditors' list
                    MySqlCommand creditorPresenceInListCommand = new MySqlCommand(sqlStatementCheckCreditorExistenceInUserList);
                    creditorPresenceInListCommand.Parameters.AddWithValue("@paramUserID", userID);
                    creditorPresenceInListCommand.Parameters.AddWithValue("@paramCreditorID", getID(sqlStatementSelectCreditorID, nameTextBox.Text));//Looks for the id of the creditor whose name was inserted
                    if (isPresentInUserCreditorList(creditorPresenceInListCommand)) {
                        MessageBox.Show("The provided creditor is already present in your creditor list and cannot be assigned again! Please enter a different creditor", "Data insertion");
                        return -1;
                    } else {
                        //If the creditor aleady exists but is assigned to the current user a new entry will be created in the users_creditors table of the database
                        MySqlCommand creditorIDInsertCommandForExistingEntry = new MySqlCommand(sqlStatementInsertCreditorID);
                        creditorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramUserID", userID);
                        creditorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramCreditorID", getID(sqlStatementSelectCreditorID, nameTextBox.Text));
                        executionResult = DBConnectionManager.insertData(creditorIDInsertCommandForExistingEntry);

                    }

                } else {
                    //If the user option is 'No' we return from the method
                    return -1;
                }
            } else {
                //Inserting a new creditor in the creditors table of the database
                MySqlCommand creditorInsertCommand = new MySqlCommand(sqlStatementInsertCreditor);
                creditorInsertCommand.Parameters.AddWithValue("@paramCreditorName", nameTextBox.Text);
                executionResult = DBConnectionManager.insertData(creditorInsertCommand);

                //Inserting the ID of the newly created creditor in he users_creditors table of the database
                MySqlCommand creditorIDInsertCommand = new MySqlCommand(sqlStatementInsertCreditorID);
                creditorIDInsertCommand.Parameters.AddWithValue("@paramUserID", userID);
                creditorIDInsertCommand.Parameters.AddWithValue("@paramCreditorID", getID(sqlStatementSelectCreditorID, nameTextBox.Text));
                executionResult = DBConnectionManager.insertData(creditorIDInsertCommand);
            }

            return executionResult;
        }



        private bool entryIsPresent(MySqlCommand command, String entryName) {
            //Executes the data retrieval command using the name of the specified creditor        
            DataTable entryDataTable = DBConnectionManager.getData(command);

            if (entryDataTable != null) {
                if (entryDataTable.Rows.Count > 0) {
                    for (int i = 0; i < entryDataTable.Rows.Count; i++) {
                        //Checks if the name of the creditor that was obtained after the execution of the command is the same as the one that the users tries to insert(case insensitive string comparison)
                        if (entryName.Equals(entryDataTable.Rows[i].ItemArray[0].ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        private bool isPresentInUserCreditorList(MySqlCommand command) {
            DataTable creditorListPresenceTable = DBConnectionManager.getData(command);

            if (creditorListPresenceTable != null && creditorListPresenceTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }

        private bool hasEnoughMoney(IncomeSource incomeSource, int valueToInsert, QueryData paramContainer) {
            if (incomeSource == IncomeSource.GENERAL_INCOMES) {
                //Getting the total value for each budget element        
                int totalIncomes = getTotalValueForSelectedElement(BudgetItemType.INCOME, sqlStatementSingleMonthIncomes, paramContainer);
                int totalExpenses = getTotalValueForSelectedElement(BudgetItemType.EXPENSE, sqlStatementSingleMonthExpenses, paramContainer);
                int totalDebts = getTotalValueForSelectedElement(BudgetItemType.DEBT, sqlStatementSingleMonthDebts, paramContainer);
                int totalSavings = getTotalValueForSelectedElement(BudgetItemType.SAVING, sqlStatementSingleMonthSavings, paramContainer);

                //Calculating the amount left to spend
                int amountLeft = getAvailableAmount(totalIncomes, totalExpenses, totalDebts, totalSavings);

                if (valueToInsert <= amountLeft) {
                    return true;
                }

            } else if (incomeSource == IncomeSource.SAVING_ACCOUNT) {
                //Getting the current balance of the saving acount
                int currentBalance = getSavingAccountCurrentBalance(sqlStatementGetSavingAccountBalance, paramContainer);
           
                if (valueToInsert <= currentBalance) {
                    return true;
                }
            }

            return false;
        }

        //Method for calculating the amount left to spend
        private int getAvailableAmount(int totalIncomes, int totalExpenses, int totalDebts, int totalSavings) {

            return totalIncomes - (totalExpenses + totalDebts + totalSavings);

        }

        //Method that gets the total value of the selected element for the specified month
        private int getTotalValueForSelectedElement(BudgetItemType itemType, String sqlStatement, QueryData paramContainer) {
            int totalValue = 0;

            //Getting the correct SQL comand for the selected element
            MySqlCommand command = getCommand(itemType, sqlStatement, paramContainer);

            if (command == null) {
                return -1;
            }

            //Getting the data based on the previously created command
            DataTable resultDataTable = DBConnectionManager.getData(command);

            //Checking if the DataTable contains data and if so converting the value to int
            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                Object result = resultDataTable.Rows[0].ItemArray[0];
                totalValue = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                return totalValue;
            }

            return -1;

        }

        //Method for retrieving the total saving amount
        private int getSavingAccountCurrentBalance(String sqlStatement, QueryData paramContainer) {
            int currentBalance = 0;

            MySqlCommand getCurrentBalanceCommand = new MySqlCommand(sqlStatement);
            getCurrentBalanceCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            getCurrentBalanceCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            DataTable resultDataTable = DBConnectionManager.getData(getCurrentBalanceCommand);

            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                Object result = resultDataTable.Rows[0].ItemArray[0];
                currentBalance = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                return currentBalance;
            }

            return -1;
        }

        //Method that returns the correct SQL command according to the type of selected item 
        private MySqlCommand getCommand(BudgetItemType itemType, String sqlStatement, QueryData paramContainer) {
            switch (itemType) {
                case BudgetItemType.INCOME:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.EXPENSE:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.DEBT:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.SAVING:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                default:
                    return null;
            }
        }


        private BudgetItemType getSelectedType(ComboBox comboBox) {
            int selectedIndex = comboBox.SelectedIndex;

            switch (selectedIndex) {
                case 1:
                    return BudgetItemType.EXPENSE;

                case 2:
                    return BudgetItemType.DEBT;

                case 3:
                    return BudgetItemType.SAVING;

                default:
                    return BudgetItemType.UNDEFINED;
            }
        }

        private IncomeSource getIncomeSource() {
            //Setting the default value for the income source
            IncomeSource incomeSource = IncomeSource.UNDEFINED;
            
            if (generalIncomesRadioButton.Checked == true) {
                incomeSource = IncomeSource.GENERAL_INCOMES;//income source representing the active/passive incomes
            } else if (savingAccountRadioButton.Checked == true) {
                incomeSource = IncomeSource.SAVING_ACCOUNT;//income source representing the savings that the user currently owns
            }

            return incomeSource;
        }

        private void InsertDataForm_Load(object sender, EventArgs e) {

        }
    }
 }

