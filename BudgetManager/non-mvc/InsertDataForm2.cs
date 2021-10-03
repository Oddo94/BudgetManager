using BudgetManager.utils;
using System;
using System.Collections;
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
    public partial class InsertDataForm2 : Form {
      
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
        private RadioButton radioButtonGeneralIncomes;
        private RadioButton radioButtonSavingAccount;

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


        //Other variables
        private ArrayList activeControls;

        public InsertDataForm2(int userID) {
            InitializeComponent();
            createLabels();
            createTextBoxes();
            createComboBoxes();
            createRadioButtons();
            createDatePickers();
            createContainer();

            groupBox1.Controls.Add(container);
        }



        private void InsertDataForm2_Load(object sender, EventArgs e) {
            itemNameTextBox.TextChanged += new EventHandler(itemNameTextBox_TextChanged);
            itemValueTextBox.TextChanged += new EventHandler(itemValueTextBox_TextChanged);
            incomeTypeComboBox.SelectedIndexChanged += new EventHandler(incomeTypeComboBox_SelectedIndexChanged);
            expenseTypeComboBox.SelectedIndexChanged += new EventHandler(incomeTypeComboBox_SelectedIndexChanged);
            creditorNameComboBox.SelectedIndexChanged += new EventHandler(expenseTypeComboBox_SelectedIndexChanged);
            debtorNameComboBox.SelectedIndexChanged += new EventHandler(debtorNameComboBox_IndexChanged);
            receivableDueDatePicker.ValueChanged += new EventHandler(receivableDueDatePicker_ValueChanged);       
        }

        private void itemTypeSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedIndex = itemTypeSelectionComboBox.SelectedIndex;



            switch (selectedIndex) {
                //Incomes insertion layout
                case 0:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    List<Control> controlsListIncomes = new List<Control>() { incomeTypeLabel, incomeTypeComboBox, incomeSourceLabel, radioButtonGeneralIncomes, radioButtonSavingAccount };
                    addControlsToContainer(container, controlsListIncomes);
                    populateControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    incomeTypeComboBox.SelectedIndex = -1;
                    break;

                //Expenses insertion layout
                case 1:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    List<Control> controlsListExpenses = new List<Control>() { expenseTypeLabel, expenseTypeComboBox };
                    addControlsToContainer(container, controlsListExpenses);
                    populateControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    expenseTypeComboBox.SelectedIndex = -1;
                    break;

                //Debt insertion layout
                case 2:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    List<Control> controlsListDebts = new List<Control>() { creditorNameLabel, creditorNameComboBox };
                    addControlsToContainer(container, controlsListDebts);
                    populateControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    creditorNameComboBox.SelectedIndex = -1;
                    break;

                //Receivables insertion layout
                case 3:
                    container.Controls.Clear();
                    List<Control> controlsListReceivables = new List<Control>() { receivableCreationDateLabel, datePicker, receivableDueDateLabel, receivableDueDatePicker, itemNameLabel,
                    itemNameTextBox, itemValueLabel, itemValueTextBox, debtorSelectionLabel, debtorNameComboBox };
                    addControlsToContainer(container, controlsListReceivables);
                    populateControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    debtorNameComboBox.SelectedIndex = -1;
                    break;

                //Savings insertion layout
                case 4:
                    container.Controls.Clear();
                    addGeneralPurposeControls();
                    populateControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    break;

                //Creditor and debtor insertion layout(the layout is identical hence the intentional fall-through)
                case 5:
                case 6:
                    container.Controls.Clear();
                    List<Control> controlsListCreditorDebtor = new List<Control>() { itemNameLabel, itemNameTextBox };
                    addControlsToContainer(container, controlsListCreditorDebtor);
                    populateControlsList(itemTypeSelectionComboBox);
                    clearActiveControls(activeControls);
                    break;

                default:
                    break;
            }
        }

        private void itemValueTextBox_TextChanged(object sender, EventArgs e) {
            Regex numberRegex = new Regex("\\b[0-9]+\\b", RegexOptions.Compiled);
            Regex specialCharacterRegex = new Regex("[^\\w\\d\\s]", RegexOptions.Compiled);

            String value = itemValueTextBox.Text;
            if (!numberRegex.IsMatch(value) || specialCharacterRegex.IsMatch(value)) {
                itemValueTextBox.Text = "";
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

            if (!isChronological(startDate, endDate)) {
                MessageBox.Show("The receivable creation date must be before the due date!");
                addEntryButton.Enabled = false;
            } else {
                addEntryButton.Enabled = true;
            }

        }


        private void addEntryButton_Click(object sender, EventArgs e) {
            int selectedIndex = itemTypeSelectionComboBox.SelectedIndex;

            switch (selectedIndex) {
                case 0:
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    DateTime startDate = datePicker.Value;
                    DateTime endDate = receivableDueDatePicker.Value;
                    if (!isChronological(startDate, endDate)) {
                        MessageBox.Show("The creation date and due date of the receivable must be in chronological order or at least equal!", "Data insertion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case 4:
                    break;

                case 5:
                    break;
            }
        }


        private void resetButton_Click(object sender, EventArgs e) {
            //incomeTypeComboBox.SelectedIndex = -1;
            clearActiveControls(activeControls);
        }

        private void createTextBoxes() {
            itemNameTextBox = new TextBox();
            itemValueTextBox = new TextBox();
        }

        private void createComboBoxes() {
            incomeTypeComboBox = new ComboBox();
            incomeTypeComboBox.DataSource = new List<String>() { "Active income", "Passive income" };
            incomeTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;


            expenseTypeComboBox = new ComboBox();
            expenseTypeComboBox.DataSource = new List<String>() { "Fixed expense", "Periodic expense", "Variable expense" };
            expenseTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            creditorNameComboBox = new ComboBox();
            creditorNameComboBox.DataSource = new List<String>() { "John", "David", "Andrew", "Steven" };
            creditorNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            debtorNameComboBox = new ComboBox();
            debtorNameComboBox.DataSource = new List<String>() { "Michael", "Gerard", "Adam", "James" };
            debtorNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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

            itemValueLabel = new Label();
            itemValueLabel.Text = "Value";

            incomeTypeLabel = new Label();
            incomeTypeLabel.Text = "Income type";

            incomeSourceLabel = new Label();
            incomeSourceLabel.Text = "Income source";

            expenseTypeLabel = new Label();
            expenseTypeLabel.Text = "Expense type";

            creditorNameLabel = new Label();
            creditorNameLabel.Text = "Creditor name";

            receivableCreationDateLabel = new Label();
            receivableCreationDateLabel.Text = "Creation date";

            receivableDueDateLabel = new Label();
            receivableDueDateLabel.Text = "Due date";

            debtorSelectionLabel = new Label();
            debtorSelectionLabel.Text = "Select debtor";

        }

        private void createRadioButtons() {
            radioButtonGeneralIncomes = new RadioButton();
            radioButtonGeneralIncomes.Text = "General incomes";
            radioButtonGeneralIncomes.Name = "radioButtonGeneralIncomes";
            radioButtonGeneralIncomes.Checked = true;

            radioButtonSavingAccount = new RadioButton();
            radioButtonSavingAccount.Text = "Saving account";
            radioButtonSavingAccount.Name = "radioButtonSavingAccount";
        }

        private void createDatePickers() {
            datePicker = new DateTimePicker();
            receivableDueDatePicker = new DateTimePicker();
        }

        private void createContainer() {
            container = new FlowLayoutPanel();
            container.FlowDirection = FlowDirection.TopDown;
            container.Dock = DockStyle.Fill;

        }

        private void addGeneralPurposeControls() {
            //container.Controls.Clear();
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

            //targetContainer.Controls.Clear();
            foreach (Control currentControl in controlsList) {
                targetContainer.Controls.Add(currentControl);
            }
        }


        private void populateControlsList(ComboBox comboBox) {
            Guard.notNull(comboBox, "Item selection combo box");

            int selectedIndex = comboBox.SelectedIndex;

            switch (selectedIndex) {
                //Selected item -> incomes
                case 0:
                    activeControls = new ArrayList() { datePicker, itemNameTextBox, itemValueTextBox, incomeTypeComboBox, radioButtonGeneralIncomes, radioButtonSavingAccount };
                    break;

                //Selected item -> expenses   
                case 1:
                    activeControls = new ArrayList() { datePicker, itemNameTextBox, itemValueTextBox, expenseTypeComboBox };
                    break;

                //Selected item -> debts
                case 2:
                    activeControls = new ArrayList() { datePicker, itemNameTextBox, itemValueTextBox, creditorNameComboBox };
                    break;

                //Selected item -> receivables
                case 3:
                    activeControls = new ArrayList() { datePicker, receivableDueDatePicker, itemNameTextBox, itemValueTextBox, debtorNameComboBox };
                    break;

                //Selected item -> savings
                case 4:
                    activeControls = new ArrayList() { datePicker, itemNameTextBox, itemValueTextBox };
                    break;

                //Selected item -> creditors
                case 5:
                    activeControls = new ArrayList() { itemNameTextBox };
                    break;

                default:
                    return;

            }
        }

        private bool hasDataOnActiveFields(ArrayList activeControls) {

            foreach (Control control in activeControls) {
                if ("".Equals(control.Text)) {
                    return false;
                }
            }

            return true;
        }

        private void clearActiveControls(ArrayList activeControls) {
            Guard.notNull(activeControls, "The active controls list cannot be null");

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (Control control in activeControls) {
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

            return startDate < endDate;
        }

        private void setAddEntryButtonState(ArrayList activeControls) {
            Guard.notNull(activeControls, "Active controls list cannot be null");

            if (hasDataOnActiveFields(activeControls)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }
    }
}

