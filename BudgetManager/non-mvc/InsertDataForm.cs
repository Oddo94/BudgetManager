﻿using BudgetManager.mvc.models.dto;
using BudgetManager.utils;
using BudgetManager.utils.data_insertion;
using BudgetManager.utils.enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BudgetManager.utils.DataProvider;

namespace BudgetManager.non_mvc {
    public partial class InsertDataForm : Form {
        //Current user ID
        int userID;

        //General controls
        private TextBox itemNameTextBox;
        private TextBox itemValueTextBox;
        private DateTimePicker datePicker;
        private Label itemNameLabel;
        private Label itemValueLabel;
        private Label itemDatePickerLabel;


        //Incomes     
        private ComboBox incomeTypeComboBox;
        private Label incomeTypeLabel;
        private Label incomeSourceLabel;
        private RadioButton generalIncomesRadioButton;
        private RadioButton savingAccountRadioButton;

        //Expenses
        private ComboBox expenseTypeComboBox;
        private Label expenseTypeLabel;

        private FlowLayoutPanel container;

        //Debts
        Label creditorNameLabel;
        ComboBox creditorNameComboBox;

        //Receivables
        private DateTimePicker receivableDueDatePicker;
        private ComboBox debtorNameComboBox;
        private Label receivableCreationDateLabel;
        private Label receivableDueDateLabel;
        private Label debtorSelectionLabel;

        //Saving account interest
        private ComboBox savingAccountComboBox;
        private ComboBox interestTypeComboBox;
        private ComboBox paymentTypeComboBox;
        private TextBox interestRateTextBox;
        //private TextBox interestValueTextBox;
        private TextBox transactionIDTextBox;
        private Label savingAccountLabel;
        private Label interestTypeLabel;
        private Label paymentTypeLabel;
        private Label interestRateLabel;
        private Label transactionIDLabel;

        //Other variables
        private ArrayList activeControls;

        public InsertDataForm(int userID) {
            InitializeComponent();
            //The userID must be assigned the correct value BEFORE creating the components and populating the comboboxes otherwise they will be empty
            this.userID = userID;

            createLabels();
            createTextBoxes();
            createComboBoxes();
            createRadioButtons();
            createDatePickers();
            createContainer();

            groupBox1.Controls.Add(container);

            itemTypeSelectionComboBox.SelectedIndex = 0;
            //incomeTypeComboBox.SelectedIndex = -1;
                 
        }

        private void InsertDataForm2_Load(object sender, EventArgs e) {
            itemNameTextBox.TextChanged += new EventHandler(itemNameTextBox_TextChanged);
            itemValueTextBox.TextChanged += new EventHandler(itemValueTextBox_TextChanged);
            incomeTypeComboBox.SelectedIndexChanged += new EventHandler(incomeTypeComboBox_SelectedIndexChanged);
            expenseTypeComboBox.SelectedIndexChanged += new EventHandler(expenseTypeComboBox_SelectedIndexChanged);
            creditorNameComboBox.SelectedIndexChanged += new EventHandler(creditorNameComboBox_IndexChanged);
            debtorNameComboBox.SelectedIndexChanged += new EventHandler(debtorNameComboBox_IndexChanged);
            //receivableDueDatePicker.ValueChanged += new EventHandler(receivableDueDatePicker_ValueChanged);
            savingAccountComboBox.SelectedIndexChanged += new EventHandler(savingAccountComboBox_SelectedIndexChanged);
            interestTypeComboBox.SelectedIndexChanged += new EventHandler(interestTypeComboBox_SelectedIndexChanged);
            paymentTypeComboBox.SelectedIndexChanged += new EventHandler(paymentTypeComboBox_SelectedIndexChanged);
            interestRateTextBox.TextChanged += new EventHandler(interestRateTextBox_TextChanged);       
        }

        private void itemTypeSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedIndex = itemTypeSelectionComboBox.SelectedIndex;



            switch (selectedIndex) {
                //Incomes insertion layout
                case 0:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    List<Control> controlsListIncomes = new List<Control>() { incomeTypeLabel, incomeTypeComboBox};
                    addControlsToContainer(container, controlsListIncomes);
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    incomeTypeComboBox.SelectedIndex = -1;
                    break;

                //Expenses insertion layout
                case 1:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    List<Control> controlsListExpenses = new List<Control>() { expenseTypeLabel, expenseTypeComboBox, incomeSourceLabel, generalIncomesRadioButton, savingAccountRadioButton };
                    addControlsToContainer(container, controlsListExpenses);
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    expenseTypeComboBox.SelectedIndex = -1;
                    break;

                //Debt insertion layout
                case 2:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    List<Control> controlsListDebts = new List<Control>() { creditorNameLabel, creditorNameComboBox };
                    addControlsToContainer(container, controlsListDebts);
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    creditorNameComboBox.SelectedIndex = -1;
                    break;

                //Receivables insertion layout
                case 3:
                    container.Controls.Clear();
                    List<Control> controlsListReceivables = new List<Control>() { receivableCreationDateLabel, datePicker, receivableDueDateLabel, receivableDueDatePicker, itemNameLabel,
                    itemNameTextBox, itemValueLabel, itemValueTextBox, debtorSelectionLabel, debtorNameComboBox, incomeSourceLabel, generalIncomesRadioButton, savingAccountRadioButton };
                    addControlsToContainer(container, controlsListReceivables);
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    debtorNameComboBox.SelectedIndex = -1;
                    break;

                //Savings insertion layout
                case 4:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    break;

                //Creditor and debtor insertion layout(the layout is identical hence the intentional fall-through)
                case 5:
                case 6:
                    container.Controls.Clear();
                    List<Control> controlsListCreditorDebtor = new List<Control>() { itemNameLabel, itemNameTextBox };
                    addControlsToContainer(container, controlsListCreditorDebtor);
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    break;

                //Saving account interest insertion layout
                case 7:
                    container.Controls.Clear();
                    List<Control> controlsListSavingAccountInterest = new List<Control> { itemDatePickerLabel, datePicker, itemNameLabel, itemNameTextBox, savingAccountLabel, savingAccountComboBox, interestTypeLabel, interestTypeComboBox,
                        paymentTypeLabel, paymentTypeComboBox, interestRateLabel, interestRateTextBox, itemValueLabel, itemValueTextBox, transactionIDLabel, transactionIDTextBox};
                    addControlsToContainer(container, controlsListSavingAccountInterest);
                    populateActiveControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    break;

                default:
                    break;
            }
        }

        private void itemValueTextBox_TextChanged(object sender, EventArgs e) {
            String selectedItemName = itemTypeSelectionComboBox.Text;
            String specialItem = "Saving account interest";

            String inputValue = itemValueTextBox.Text;

            //Special check to verify if the saving account interest value can be parsed as a double(to allow for a greater precision when calculating the account balance)
            //The values of the other items will still be treated as integers
            if (specialItem.Equals(selectedItemName)) {               
                double result;
                bool isValid = Double.TryParse(inputValue, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out result);

                if (!isValid) {
                    itemValueTextBox.Clear();
                }
            } else {
                //Regex numberRegex = new Regex("\\b[0-9]+\\b", RegexOptions.Compiled);
                //Regex specialCharacterRegex = new Regex("[^\\w\\d\\s]", RegexOptions.Compiled);

                //String value = itemValueTextBox.Text;
                //if (!numberRegex.IsMatch(value) || specialCharacterRegex.IsMatch(value)) {
                //    //itemValueTextBox.Text = "";
                //    itemValueTextBox.Clear();
                //}

                //Regex that matches any non-digit character
                Regex forbiddenCharacters = new Regex("[^0-9]+", RegexOptions.Compiled);
            
                //If any such character is found the textbox will be cleared as the value field can contain only numbers
                if(forbiddenCharacters.IsMatch(inputValue)) {
                    itemValueTextBox.Clear();
                }
            }

            setAddEntryButtonState(activeControls);

        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void incomeTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (incomeTypeComboBox.SelectedIndex != -1) {
                setAddEntryButtonState(activeControls);
            }
        }

        private void expenseTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void creditorNameComboBox_IndexChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void debtorNameComboBox_IndexChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void receivableDueDatePicker_ValueChanged(object sender, EventArgs e) {
            DateTime startDate = datePicker.Value;
            DateTime endDate = receivableDueDatePicker.Value;

            //CHECK TO SEE IF THE BEHAVIOR IS CORRECT
            if (!isChronological(startDate, endDate)) {
                //MessageBox.Show("The receivable creation date must be before the due date!");
                addEntryButton.Enabled = false;
            } else {
                addEntryButton.Enabled = true;
            }

        }

        private void savingAccountComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void interestTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void paymentTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            setAddEntryButtonState(activeControls);
        }

        private void interestRateTextBox_TextChanged(object sender, EventArgs e) {
            String inputValue = interestRateTextBox.Text;
            double result;
            bool isValid = Double.TryParse(inputValue, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out result);

            if(!isValid) {
                interestRateTextBox.Clear();
            }

            setAddEntryButtonState(activeControls);       
        }

        private void addEntryButton_Click(object sender, EventArgs e) {
            int allChecksExecutionResult = -1;           
            int dataInsertionExecutionResult = -1;
            int selectedIndex = itemTypeSelectionComboBox.SelectedIndex;

            DialogResult userOptionConfirmInsertion = MessageBox.Show("Are you sure that you want to insert the provided data?", "Data insertion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOptionConfirmInsertion == DialogResult.No) {
                return;
            }
        
            String selectedItemName = itemTypeSelectionComboBox.Text;
            String specialItemName = "Saving account interest";

            //There is no need to perform checks when inserting a saving account interest item
            if (!specialItemName.Equals(selectedItemName)) {
                allChecksExecutionResult = performDataChecks();

                //Checks the execution result returned by the insertion method(positive value means success while -1 means the failure of the operation)
                if (allChecksExecutionResult != -1) {
                    dataInsertionExecutionResult = insertSelectedItem(selectedIndex);
                }
            } else {
                dataInsertionExecutionResult = insertSelectedItem(selectedIndex);//The saving account interest can be inserted directly without performing a precheck
            }   

            //Checks the execution result returned by the insertion method(positive value means success while -1 means the failure of the operation)
            if (dataInsertionExecutionResult != -1) {
                MessageBox.Show("Data inserted successfully!", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Clears the active controls if the data insertion is successful
                clearActiveControls(activeControls);
            } else {
                MessageBox.Show("Unable to insert the input data! Please try again.", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }


        private void resetButton_Click(object sender, EventArgs e) {
            //incomeTypeComboBox.SelectedIndex = -1;
            clearActiveControls(activeControls);
        }

        private void createTextBoxes() {
            itemNameTextBox = new TextBox();
            itemNameTextBox.Width = 200;
            itemNameTextBox.Margin = new Padding(0, 0, 0, 0);

            itemValueTextBox = new TextBox();
            itemValueTextBox.Width = 200;
            itemValueTextBox.Margin = new Padding(0, 0, 0, 0);

            interestRateTextBox = new TextBox();
            interestRateTextBox.Width = 200;
            interestRateTextBox.Margin = new Padding(0, 0, 0, 0);

            //Input field for the transaction ID.It is currently used only for inserting the saving aaccount interest but it can be used in the future for other items that require the insertion of this data
            transactionIDTextBox = new TextBox();
            transactionIDTextBox.Width = 200;
            transactionIDTextBox.Margin = new Padding(0, 0, 0, 0);
            transactionIDTextBox.MaxLength = 50;//sets the maxiumum number of characters that can be introduced, in order to match the database field constraints
        }

        private void createComboBoxes() {
            DataProvider dataProvider = new DataProvider();
            incomeTypeComboBox = new ComboBox();           
            dataProvider.fillComboBox(incomeTypeComboBox, ComboBoxType.INCOME_TYPE_COMBOBOX, userID);
            incomeTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            incomeTypeComboBox.Margin = new Padding(0, 0, 0, 0);       


            expenseTypeComboBox = new ComboBox();
            dataProvider.fillComboBox(expenseTypeComboBox, ComboBoxType.EXPENSE_TYPE_COMBOBOX, userID);
            expenseTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            expenseTypeComboBox.Margin = new Padding(0, 0, 0, 0);

            creditorNameComboBox = new ComboBox();
            dataProvider.fillComboBox(creditorNameComboBox, ComboBoxType.CREDITOR_COMBOBOX, userID);
            creditorNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            creditorNameComboBox.Margin = new Padding(0, 0, 0, 0);

            debtorNameComboBox = new ComboBox();
            dataProvider.fillComboBox(debtorNameComboBox, ComboBoxType.DEBTOR_COMBOBOX, userID);
            debtorNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            debtorNameComboBox.Margin = new Padding(0, 0, 0, 0);

            savingAccountComboBox = new ComboBox();
            dataProvider.fillComboBox(savingAccountComboBox, ComboBoxType.SAVING_ACCOUNT_COMBOBOX, userID);
            savingAccountComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            savingAccountComboBox.Margin = new Padding(0, 0, 0, 0);

            interestTypeComboBox = new ComboBox();
            dataProvider.fillComboBox(interestTypeComboBox, ComboBoxType.INTEREST_TYPE_COMBOBOX, userID);
            interestTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            interestTypeComboBox.Margin = new Padding(0, 0, 0, 0);
            
            paymentTypeComboBox = new ComboBox();
            dataProvider.fillComboBox(paymentTypeComboBox, ComboBoxType.PAYMENT_TYPE_COMBOBOX, userID);
            paymentTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            paymentTypeComboBox.Margin = new Padding(0, 0, 0, 0);

        }


        private void clearComboBoxes() {
            incomeTypeComboBox.SelectedIndex = -1;
            expenseTypeComboBox.SelectedIndex = -1;
        }

        private void createLabels() {
            itemDatePickerLabel = new Label();
            itemDatePickerLabel.Text = "Date";
           
            itemNameLabel = new Label();
            itemNameLabel.Text = "Name";
            itemNameLabel.Margin = new Padding(0, 10, 0, 0);

            itemValueLabel = new Label();
            itemValueLabel.Text = "Value";
            itemValueLabel.Margin = new Padding(0, 10, 0, 0);       

            incomeTypeLabel = new Label();
            incomeTypeLabel.Text = "Income type";
            incomeTypeLabel.Margin = new Padding(0, 10, 0, 0);
            
            incomeSourceLabel = new Label();
            incomeSourceLabel.Text = "Income source";
            incomeSourceLabel.Margin = new Padding(0, 10, 0, 0);

            expenseTypeLabel = new Label();
            expenseTypeLabel.Text = "Expense type";
            expenseTypeLabel.Margin = new Padding(0, 10, 0, 0);

            creditorNameLabel = new Label();
            creditorNameLabel.Text = "Creditor name";
            creditorNameLabel.Margin = new Padding(0, 10, 0, 0);

            receivableCreationDateLabel = new Label();
            receivableCreationDateLabel.Text = "Creation date";
            receivableCreationDateLabel.Margin = new Padding(0, 10, 0, 0);

            receivableDueDateLabel = new Label();
            receivableDueDateLabel.Text = "Due date";
            receivableDueDateLabel.Margin = new Padding(0, 10, 0, 0);

            debtorSelectionLabel = new Label();
            debtorSelectionLabel.Text = "Select debtor";
            debtorSelectionLabel.Margin = new Padding(0, 10, 0, 0);
            
            savingAccountLabel = new Label();
            savingAccountLabel.Text = "Saving account";
            savingAccountLabel.Margin = new Padding(0, 10, 0, 0);

            interestTypeLabel = new Label();
            interestTypeLabel.Text = "Interest type";
            interestTypeLabel.Margin = new Padding(0, 10, 0, 0);

            paymentTypeLabel = new Label();
            paymentTypeLabel.Text = "Payment type";
            paymentTypeLabel.Margin = new Padding(0, 10, 0, 0);

            interestRateLabel = new Label();
            interestRateLabel.Text = "Interest rate";
            interestRateLabel.Margin = new Padding(0, 10, 0, 0);

            transactionIDLabel = new Label();
            transactionIDLabel.Text = "Transaction ID";
            transactionIDLabel.Margin = new Padding(0, 10, 0, 0);

        }

        private void createRadioButtons() {
            generalIncomesRadioButton = new RadioButton();
            generalIncomesRadioButton.Text = "General incomes";
            generalIncomesRadioButton.Name = "radioButtonGeneralIncomes";
            generalIncomesRadioButton.Checked = true;

            savingAccountRadioButton = new RadioButton();
            savingAccountRadioButton.Text = "Saving account";
            savingAccountRadioButton.Name = "radioButtonSavingAccount";
        }

        private void createDatePickers() {
            datePicker = new DateTimePicker();
            datePicker.Margin = new Padding(0, 0, 0, -0);

            receivableDueDatePicker = new DateTimePicker();
            receivableDueDatePicker.Margin = new Padding(0, 0, 0, 0);
        }

        private void createContainer() {
            container = new FlowLayoutPanel();
            container.FlowDirection = FlowDirection.TopDown;
            container.Margin = new Padding(20, 20, 20, 20);
            container.Dock = DockStyle.Fill;

        }

        private void addGeneralPurposeControls() {           
            container.Controls.Add(itemDatePickerLabel);
            container.Controls.Add(datePicker);
            container.Controls.Add(itemNameLabel);
            container.Controls.Add(itemNameTextBox);
            container.Controls.Add(itemValueLabel);
            container.Controls.Add(itemValueTextBox);
        }

        private void addControlsToContainer(Panel targetContainer, List<Control> controlsList) {
            Guard.notNull(targetContainer, "Controls container");
            Guard.notNull(controlsList, "Controls list");

            if (!controlsList.Any()) {
                return;
            }
           
            foreach (Control currentControl in controlsList) {
                targetContainer.Controls.Add(currentControl);
            }
        }


        private void populateActiveControlsList(ComboBox comboBox) {
            Guard.notNull(comboBox, "item selection combo box");

            int selectedIndex = comboBox.SelectedIndex;

            switch (selectedIndex) {
                //Selected item -> incomes
                case 0:
                    activeControls = new ArrayList() { new InsertionFormField(datePicker, true), new InsertionFormField(itemNameTextBox, true), new InsertionFormField(itemValueTextBox, true), new InsertionFormField(incomeTypeComboBox, true)};
                    break;

                //Selected item -> expenses   
                case 1:
                    activeControls = new ArrayList() { new InsertionFormField(datePicker, true), new InsertionFormField(itemNameTextBox, true), new InsertionFormField(itemValueTextBox, true), new InsertionFormField(expenseTypeComboBox,true),
                        new InsertionFormField(generalIncomesRadioButton, true), new InsertionFormField(savingAccountRadioButton, true)};
                    break;

                //Selected item -> debts
                case 2:
                    activeControls = new ArrayList() { new InsertionFormField(datePicker, true), new InsertionFormField(itemNameTextBox, true), new InsertionFormField(itemValueTextBox, true), new InsertionFormField(creditorNameComboBox, true)};
                    break;

                //Selected item -> receivables
                case 3:
                    activeControls = new ArrayList() { new InsertionFormField(datePicker,true), new InsertionFormField(receivableDueDatePicker, true), new InsertionFormField(itemNameTextBox, true), new InsertionFormField(itemValueTextBox, true),
                        new InsertionFormField(debtorNameComboBox, true), new InsertionFormField(generalIncomesRadioButton, true), new InsertionFormField(savingAccountRadioButton, true)};
                    break;

                //Selected item -> savings
                case 4:
                    activeControls = new ArrayList() { new InsertionFormField(datePicker, true), new InsertionFormField(itemNameTextBox, true), new InsertionFormField(itemValueTextBox, true)};
                    break;

                //Selected item -> creditors or debtors(intentional fall through since the layout is identical)
                case 5:
                case 6:
                    activeControls = new ArrayList() { new InsertionFormField(itemNameTextBox, true)};
                    break;

                //Selected item-> saving account interest
                case 7:
                    activeControls = new ArrayList() { new InsertionFormField(datePicker, true), new InsertionFormField(itemNameTextBox, true), new InsertionFormField(savingAccountComboBox, true), new InsertionFormField(interestTypeComboBox, true),
                        new InsertionFormField(paymentTypeComboBox, true), new InsertionFormField(interestRateTextBox, true), new InsertionFormField(itemValueTextBox, true), new InsertionFormField(transactionIDLabel, true), new InsertionFormField(transactionIDTextBox, false) };
                    break;              

                default:
                    return;

            }
        }

        private bool hasDataOnActiveFields(ArrayList activeControls) {

            //foreach (Control control in activeControls) {
            //    if ("".Equals(control.Text)) {
            //        return false;
            //    }
            //}

            //return true;
            //Checks if the current control contains value and if it is required (in this case it will return false, meaning that the respective required field is not populated)
            foreach (InsertionFormField currentItem in activeControls) {
                if ("".Equals(currentItem.FormField.Text) && currentItem.IsRequired) {
                    return false;
                }
            }

            return true;
        }

        private void clearActiveControls(ArrayList activeControls) {
            Guard.notNull(activeControls, "The active controls list cannot be null");

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (InsertionFormField currentItem in activeControls) {
                Control control = currentItem.FormField;//gets the control object from the InsertionFormField object
                if (control is TextBox) {
                    ((TextBox)control).Text = "";
                } else if (control is ComboBox) {
                    //Setting SelectedIndex to -1 when any item other than the first one is selected does not work properly
                    ((ComboBox)control).SelectedIndex = -1;
                    ((ComboBox)control).SelectedIndex = -1;
                } else if (control is DateTimePicker) {
                    ((DateTimePicker)control).Value = DateTime.Now;
                } else if (control is RadioButton) {
                    //Sets the "General incomes" radio button as the default selection
                    RadioButton radioButton = (RadioButton)control;
                    if ("radioButtonGeneralIncomes".Equals(radioButton.Name)) {
                        radioButton.Checked = true;
                    }
                }
            }
        }

        //Checks if the start date is before the end date (for receivables only!)
        private bool isChronological(DateTime startDate, DateTime endDate) {

            return startDate <= endDate;
        }

        private void setAddEntryButtonState(ArrayList activeControls) {
            Guard.notNull(activeControls, "Active controls list cannot be null");

            if (hasDataOnActiveFields(activeControls)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }


        //Method for retrieving the user selected income source
        private IncomeSource getIncomeSource() {
            //Setting the default value for the income source
            IncomeSource incomeSource = IncomeSource.UNDEFINED;

            if (generalIncomesRadioButton.Checked == true) {
                incomeSource = IncomeSource.GENERAL_INCOMES;//Income source representing the active/passive incomes
            } else if (savingAccountRadioButton.Checked == true) {
                incomeSource = IncomeSource.SAVING_ACCOUNT;//Income source representing the savings that the user currently owns
            }

            return incomeSource;
        }

        private int checkReceivableDates() {
            int checkResult = -1;
            DateTime startDate = datePicker.Value;
            DateTime endDate = receivableDueDatePicker.Value;
            if (!isChronological(startDate, endDate)) {
                MessageBox.Show("The creation date and due date of the receivable must be in chronological order or at least equal!", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Resets the values of the date time pickers for the receivable item if the user tries to insert incorrect values for creation date/due date
                datePicker.Value = DateTime.Now;
                receivableDueDatePicker.Value = DateTime.Now;                
            } else {
                checkResult = 0;
            }

            return checkResult;
        }

        //Method for retrieving the user selected budget item type 
        private BudgetItemType getSelectedType(ComboBox comboBox) {
            int selectedIndex = comboBox.SelectedIndex;

            switch (selectedIndex) {
                case 1:
                    //return BudgetItemType.EXPENSE;
                    return BudgetItemType.GENERAL_EXPENSE;//CHANGE(FROM EXPENSE TO GENERAL_EXPENSE)

                case 2:
                    return BudgetItemType.DEBT;

                case 3:
                    return BudgetItemType.SAVING;

                default:
                    return BudgetItemType.UNDEFINED;
            }
        }

        private int insertSelectedItem(int selectedItemIndex) {
            int executionResult = -1;
            QueryData paramContainer = null;
            IDataInsertionDTO dataInsertionDTO = null;

            DataInsertionContext dataInsertionContext = new DataInsertionContext();
            switch (selectedItemIndex) {
                //Income insertion
                case 0:
                    //Creates the object that contains the necessary values for the execution of the SQL query
                    paramContainer = configureParamContainer(BudgetItemType.INCOME);
                    //Null check for this object
                    Guard.notNull(paramContainer, "income parameter container");

                    //Creates the actual data insertion strategy
                    DataInsertionStrategy incomeInsertionStrategy = new IncomeInsertionStrategy();
                    //Sets the strategy of the data insertion context to the previously created strategy
                    dataInsertionContext.setStrategy(incomeInsertionStrategy);

                    //Executes the strategy by calling the invoke() method of the context and passing it the paramContainer object
                    executionResult = dataInsertionContext.invoke(paramContainer);                                 
                    break;

                //Expense insertion
                //CHANGE TO ALLOW THE CORRECT SELECTION OF EXPENSE INSERTION STATEMENT
                case 1:
                    //GENERAL_EXPENSE type is used since there is an intentional fall through the cases inside the configuration method so that both types are treated identically(they need the same data)
                    paramContainer = configureParamContainer(BudgetItemType.GENERAL_EXPENSE);
                    Guard.notNull(paramContainer, "expense parameter container");

                    DataInsertionStrategy expenseInsertionStrategy = new ExpenseInsertionStrategy();
                    dataInsertionContext.setStrategy(expenseInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(paramContainer);              
                    break;

                //Debt insertion
                case 2:
                    paramContainer = configureParamContainer(BudgetItemType.DEBT);
                    Guard.notNull(paramContainer, "debt parameter container");

                    DataInsertionStrategy debtInsertionStrategy = new DebtInsertionStrategy();
                    dataInsertionContext.setStrategy(debtInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(paramContainer);               
                    break;

                //Receivable insertion
                case 3:
                    paramContainer = configureParamContainer(BudgetItemType.RECEIVABLE);
                    Guard.notNull(paramContainer, "receivable parameter container");

                    ReceivableInsertionStrategy receivableInsertionStrategy = new ReceivableInsertionStrategy();
                    dataInsertionContext.setStrategy(receivableInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(paramContainer);
                    break;

                //Saving insertion
                case 4:
                    paramContainer = configureParamContainer(BudgetItemType.SAVING);
                    Guard.notNull(paramContainer, "saving parameter container");

                    SavingInsertionStrategy savingInsertionStrategy = new SavingInsertionStrategy();
                    dataInsertionContext.setStrategy(savingInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(paramContainer);
                    break;

                //New creditor insertion
                case 5:
                    paramContainer = configureParamContainer(BudgetItemType.CREDITOR);
                    Guard.notNull(paramContainer, "creditor parameter container");

                    DataInsertionStrategy creditorInsertionStrategy = new CreditorInsertionStrategy();
                    dataInsertionContext.setStrategy(creditorInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(paramContainer);                
                    break;

                //New debtor insertion
                case 6:
                    paramContainer = configureParamContainer(BudgetItemType.DEBTOR);
                    Guard.notNull(paramContainer, "debtor parameter container");

                    DataInsertionStrategy debtorInsertionStrategy = new DebtorInsertionStrategy();
                    dataInsertionContext.setStrategy(debtorInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(paramContainer);
                    break;

                //Saving account interest insertion
                case 7:
                    dataInsertionDTO = configureDataInsertionDTO(BudgetItemType.SAVING_ACCOUNT_INTEREST);
                    Guard.notNull(dataInsertionDTO, "saving account interest DTO");

                    DataInsertionStrategy accountInterestInsertionStrategy = new AccountInterestInsertionStrategy();
                    dataInsertionContext.setStrategy(accountInterestInsertionStrategy);

                    executionResult = dataInsertionContext.invoke(dataInsertionDTO);                   
                    break;
                

                default:
                    break;
            }

            return executionResult;
        }

        //Method for configuring the param container object for the insertion of different budget items
        private QueryData configureParamContainer(BudgetItemType selectedItemType) {
            QueryData paramContainer = null;

            switch (selectedItemType) {
                //Income insertion object configuration
                case BudgetItemType.INCOME:
                    String incomeName = itemNameTextBox.Text;
                    String incomeTypeName = incomeTypeComboBox.Text;
                    int incomeValue = Convert.ToInt32(itemValueTextBox.Text);
                    String incomeDate = datePicker.Value.ToString("yyyy-MM-dd");

                    paramContainer = new QueryData.Builder(userID)
                        .addItemName(incomeName)
                        .addItemValue(incomeValue)
                        .addTypeName(incomeTypeName)
                        .addItemCreationDate(incomeDate)
                        .build();
                    break;

                //Expense insertion object configuration
                //Intentional fall-through the cases because both types need the same data
                case BudgetItemType.SAVING_ACCOUNT_EXPENSE:
                case BudgetItemType.GENERAL_EXPENSE:
                    String expenseName = itemNameTextBox.Text;
                    //int expenseTypeID = getID(sqlStatementSelectExpenseTypeID, expenseTypeComboBox.Text);
                    String expenseTypeName = expenseTypeComboBox.Text;
                    int expenseValue = Convert.ToInt32(itemValueTextBox.Text);
                    String expenseDate = datePicker.Value.ToString("yyyy-MM-dd");//Getting date as String
                    IncomeSource incomeSource = getIncomeSource();

                    paramContainer = new QueryData.Builder(userID)

                        .addItemName(expenseName)
                        .addItemValue(expenseValue)
                        .addTypeName(expenseTypeName)
                        .addItemCreationDate(expenseDate)
                        .addIncomeSource(incomeSource)
                        .build();
                    break;

                //Debt insertion object configuration
                case BudgetItemType.DEBT:
                    //Getting the necessary data
                    String debtName = itemNameTextBox.Text;
                    int debtValue = Convert.ToInt32(itemValueTextBox.Text);
                    String creditorName = creditorNameComboBox.Text;
                    String debtDate = datePicker.Value.ToString("yyyy-MM-dd");

                    paramContainer = new QueryData.Builder(userID)
                        .addItemName(debtName)
                        .addItemValue(debtValue)
                        .addCreditorName(creditorName)
                        .addItemCreationDate(debtDate)
                        .build();
                    break;

                //Receivable insertion object configuration
                case BudgetItemType.RECEIVABLE:
                    String receivableName = itemNameTextBox.Text;
                    int receivableValue = Convert.ToInt32(itemValueTextBox.Text);
                    int totalPaidAmount = 0;//the total paid amount is set to 0 since this is a new record and there were no money paid yet
                    String debtorName = debtorNameComboBox.Text;
                    String receivableStartDate = datePicker.Value.ToString("yyy-MM-dd");
                    String receivableEndDate = receivableDueDatePicker.Value.ToString("yyy-MM-dd");
                    IncomeSource receivableIncomeSource = getIncomeSource();

                    paramContainer = new QueryData.Builder(userID)
                        .addItemName(receivableName)
                        .addItemValue(receivableValue)
                        .addDebtorName(debtorName)
                        .addStartDate(receivableStartDate)
                        .addEndDate(receivableEndDate)
                        .addIncomeSource(receivableIncomeSource)
                        .addPaidAmount(totalPaidAmount)
                        .build();
                    break;

                //Saving insertion object configuration
                case BudgetItemType.SAVING:
                    //Getting the necessary data
                    String savingName = itemNameTextBox.Text;
                    int savingValue = Convert.ToInt32(itemValueTextBox.Text);
                    String savingDate = datePicker.Value.ToString("yyyy-MM-dd");

                    paramContainer = new QueryData.Builder(userID)
                        .addItemName(savingName)
                        .addItemValue(savingValue)
                        .addItemCreationDate(savingDate)
                        .build();
                    break;

                //Creditor insertion object configuration
                case BudgetItemType.CREDITOR:
                    String insertedCreditorName = itemNameTextBox.Text;

                    paramContainer = new QueryData.Builder(userID)
                        .addCreditorName(insertedCreditorName)
                        .build();
                    break;

                //Debtor insertion object configuration
                case BudgetItemType.DEBTOR:
                    String insertedDebtorName = itemNameTextBox.Text;

                    paramContainer = new QueryData.Builder(userID)
                        .addDebtorName(insertedDebtorName)
                        .build();
                    break;

                default:
                    break;
            }

            /* NOTE!
            The parameter container is not used for saving account interest insertion since the necessary data is transfered to the DB using a DTO(see below) */

                    return paramContainer;
        }

        //Method for testing a future refactoring(using DTO classes instead of QueryData class)
        private IDataInsertionDTO configureDataInsertionDTO(BudgetItemType selectedItemType) {
            IDataInsertionDTO dataInsertionDTO = null;

            switch(selectedItemType) {

                case BudgetItemType.SAVING_ACCOUNT_INTEREST:
                    String interestCreationDate = datePicker.Value.ToString("yyyy-MM-dd");
                    String interestName = itemNameTextBox.Text;
                    String accountName = savingAccountComboBox.Text;
                    String interestType = interestTypeComboBox.Text;
                    String paymentType = paymentTypeComboBox.Text;
                    String transactionID = !transactionIDTextBox.Text.Equals("") ? transactionIDTextBox.Text : null; //If the transaction ID field remains empty then a null value will be inserted in the database
                    double interestRate = Convert.ToDouble(interestRateTextBox.Text);
                    double interestValue = Convert.ToDouble(itemValueTextBox.Text);

                    dataInsertionDTO = new SavingAccountInterestDTO(interestCreationDate, interestName, accountName, interestType, paymentType, interestRate, interestValue, transactionID, userID);              
                    break;
            }

            return dataInsertionDTO;
        }


        private int performDataChecks() {
            int allChecksExecutionResult = -1;
            int generalCheckExecutionResult = -1;
            int budgetPlanCheckExecutionResult = -1;
            //int dataInsertionExecutionResult = -1;

            int selectedIndex = itemTypeSelectionComboBox.SelectedIndex;
            String selectedItemName = itemTypeSelectionComboBox.Text;

            QueryData paramContainerGeneralCheck = null;
            QueryData paramContainerBPCheck = null;
            DataInsertionCheckerContext dataInsertionCheckContext = null;
            GeneralInsertionCheckStrategy generalCheckStrategy = null;
            int valueToInsert = 0;

            //Check if it can be improved
            if (!selectedItemName.Equals("Debtor") && !selectedItemName.Equals("Creditor") && !selectedItemName.Equals("Income")) {
                //Checks if the user has enough money left to insert the selected item value
                valueToInsert = Convert.ToInt32(itemValueTextBox.Text);
                int selectedMonth = datePicker.Value.Month;
                int selectedYear = datePicker.Value.Year;
                IncomeSource incomeSource = getIncomeSource();
                //QueryData paramContainer = new QueryData(userID, selectedMonth, selectedYear);
                //Query data parameter object for general checks
                paramContainerGeneralCheck = new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).addIncomeSource(incomeSource).build(); //CHANGE
                                                                                                                                                                //Query data parameter object for budget plan checks
                paramContainerBPCheck = new QueryData.Builder(userID).addItemCreationDate(datePicker.Value.ToString("yyyy-MM-dd")).addBudgetItemType(getSelectedType(itemTypeSelectionComboBox)).build();

                dataInsertionCheckContext = new DataInsertionCheckerContext();
                generalCheckStrategy = new GeneralInsertionCheckStrategy();

            }
            switch (selectedIndex) {
                //Income
                case 0:
                    allChecksExecutionResult = 0;
                    break;
                //Expense
                case 1:
                //Debt                                 
                case 2:

                //Saving
                case 4:                   
                    dataInsertionCheckContext.setStrategy(generalCheckStrategy);

                    generalCheckExecutionResult = dataInsertionCheckContext.invoke(paramContainerGeneralCheck, selectedItemName, valueToInsert);

                    BudgetPlanCheckStrategy budgetPlanCheckStrategy = new BudgetPlanCheckStrategy();
                    dataInsertionCheckContext.setStrategy(budgetPlanCheckStrategy);

                    budgetPlanCheckExecutionResult = dataInsertionCheckContext.invoke(paramContainerBPCheck, selectedItemName, valueToInsert);

                    //If the general check fails(not enough money) then the general check execution result will rmain -1 (no data can be inserted)
                    //Else, if the general check is passed and the budget plan check returns -1 (fail because there might not be a budget plan in place) the data can be inserted
                    //Otherwise the allChecksExecutionResult keeps its initial value(-1) and no data will be inserted(for example if a warning message is shown during budget plan checks due to the inserted value being higher than the value allowed by the budget plan item limit) 
                    if (generalCheckExecutionResult == -1) {
                        break;
                    } else if (generalCheckExecutionResult == 0 && budgetPlanCheckExecutionResult == -1) {
                        allChecksExecutionResult = 0;
                    }

                    break;
                //Receivables  
                case 3:
                    //Checks if the start and end dates for the receivable are in chronological order
                    if (checkReceivableDates() == -1) {
                        break;
                    }
                    dataInsertionCheckContext.setStrategy(generalCheckStrategy);
                    generalCheckExecutionResult = dataInsertionCheckContext.invoke(paramContainerGeneralCheck, selectedItemName, valueToInsert);

                    if (generalCheckExecutionResult == -1) {
                        break;
                    } else {
                        allChecksExecutionResult = 0;
                    }

                    break;
                //Creditor
                case 5:
                    allChecksExecutionResult = 0;
                    break;

                //Debtor
                case 6:
                    allChecksExecutionResult = 0;
                    break;
                
                //Saving account interest
                case 7:
                    allChecksExecutionResult = 0;
                    break;

                default:
                    break;
            }

            return allChecksExecutionResult;
        }



        //Method for retrieving the description of an enum value
        public static String getEnumDescriptionAttribute(Enum value) {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute = (DescriptionAttribute) Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }
    }
}

