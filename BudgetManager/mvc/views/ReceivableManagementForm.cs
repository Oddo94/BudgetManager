using BudgetManager.mvc.controllers;
using BudgetManager.mvc.models;
using BudgetManager.mvc.models.dto;
using BudgetManager.non_mvc;
using BudgetManager.utils;
using BudgetManager.utils.data_insertion;
using BudgetManager.utils.enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BudgetManager.mvc.views {
    public partial class ReceivableManagementForm : Form, IView {
        private int userID;
        private int rowIndexOnRightClick;
        private int columnIndexOnRightClick;
        private int totalPendingChanges;

        //General components
        private TextBox itemNameTextBox;
        private TextBox itemValueTextBox;

        //Receivable update components
        private ComboBox receivableDebtorComboBox;
        private DateTimePicker receivableCreatedDatePicker;
        private DateTimePicker receivableDueDatePicker;
        private Button updateDgvRecordButton;
        private Label receivableNameLabel;
        private Label receivableValueLabel;
        private Label receivableDebtorCbxLabel;
        private Label receivableCreatedDateLabel;
        private Label receivableDueDateLabel;

        //Partial payment insertion components
        private DateTimePicker partialPaymentDatePicker;
        private Button insertPartialPaymentButton;
        private Label partialPaymentNameLabel;
        private Label partialPaymentValueLabel;
        private Label partialPaymentDateLabel;

        //ArrayList containing the controls currently used in the data insertion/update form
        private List<FormFieldWrapper> activeControls;
        private UserControlsManager controlsManager;
        //Variable used for keeping track of the current data insertion form layout
        private UIContainerLayout currentLayout;

        private IUpdaterModel model;
        private IUpdaterControl controller;

        private ArrayList dateTimeColumnIndexes = new ArrayList();

        public ReceivableManagementForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            controlsManager = new UserControlsManager();
            currentLayout = UIContainerLayout.UNDEFINED;

            //fillDataGridView();        
            createTextBoxes();
            createComboBoxes();
            createDateTimePickers();
            createButtons();
            createLabels();
            fillComboBoxes();

            model = new ReceivableManagementModel();
            controller = new ReceivableManagementController();
            wireUp(controller, model);
        }

        private void fillDataGridView() {
            DataTable sourceDataTable = new DataTable();

            sourceDataTable.Columns.Add("Id");
            sourceDataTable.Columns.Add("Name");
            sourceDataTable.Columns.Add("Value");
            sourceDataTable.Columns.Add("Debtor");
            sourceDataTable.Columns.Add("Total paid amount");
            sourceDataTable.Columns.Add("Status");
            sourceDataTable.Columns.Add("Created date");
            sourceDataTable.Columns.Add("Due date");

            sourceDataTable.Rows.Add(1, "Receivable 1", 700, "Jim", 0, "New", "2022-01-30", "2022-12-30");
            sourceDataTable.Rows.Add(2, "Receivable 2", 300, "John", 0, "Partially paid", "2022-04-30", "2022-12-30");
            sourceDataTable.Rows.Add(3, "Receivable 3", 500, "Adam", 500, "Paid", "2022-05-30", "2022-12-30");
            sourceDataTable.Rows.Add(4, "Trigger test", 1000, "Mike", 300, "Overdue", "2022-09-30", "2022-12-30");

            //receivableManagementDgv.DefaultCellStyle.ForeColor = Color.Green;           

            sourceDataTable.AcceptChanges();

            receivableManagementDgv.DataSource = sourceDataTable;


            DataTable newDataTable = sourceDataTable.Clone();//Will be used to get a copy of the original DataTable onto which the changes will be performed by the user
        }

        private void fillComboBoxes() {
            //Populates the debtor combo box as this will be needed for receivable update
            DataProvider dataProvider = new DataProvider();
            dataProvider.fillComboBox(receivableDebtorComboBox, DataProvider.ComboBoxType.DEBTOR_COMBOBOX, userID);
        }

        private void receivableManagementDgv_ColumnAdded(object sender, DataGridViewColumnEventArgs e) {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void receivableManagementDgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e) {
            int lowerBound = 0;
            int upperBound = receivableManagementDgv.Rows.Count - 2;

            //Displays the update receivable context menu only if the user clicked on a row that contains data
            if (rowIndexOnRightClick >= lowerBound && rowIndexOnRightClick <= upperBound) {
                DataGridViewRow selectedReceivableData = retrieveDataFromSelectedRow(rowIndexOnRightClick, receivableManagementDgv);
                String receivableStatus = selectedReceivableData.Cells[5].Value.ToString();

                //Displays the context menu on right click only if the receivable has one of the following status: 'New', 'Partially paid', 'Overdue'
                if (!"Paid".Equals(receivableStatus)) {
                    //Sets the context menu to be displayed
                    e.ContextMenuStrip = updateReceivableCtxMenu;
                    //Makes the context menu visible
                    updateReceivableCtxMenu.Visible = true;

                    //Clears the previous row selection(if any) and selects the row on which the right click was performed
                    receivableManagementDgv.ClearSelection();
                    receivableManagementDgv.Rows[rowIndexOnRightClick].Selected = true;
                }

            }

        }

        private void monthRecordsRadioButton_CheckedChanged(object sender, EventArgs e) {
            //receivableManagemenStartDatePicker.CustomFormat = "MM/yyyy";
        }

        private void yearRecordsRadioButton_CheckedChanged(object sender, EventArgs e) {
            //receivableManagemenStartDatePicker.CustomFormat = "yyyy";
        }

        private void displayReceivablesButton_Click(object sender, EventArgs e) {
            DateTime startDate = receivableManagemenStartDatePicker.Value;
            DateTime endDate = receivableManagementEndDatePicker.Value;

            //Checks that the search interval is correct
            if (!isValidSearchInterval(startDate, endDate)) {
                return;
            }

            //String searchIntervalStartDate = startDate.ToString("yyyy-MM-dd");
            //String searchIntervalEndDate = endDate.ToString("yyyy-MM-dd");

            //    QueryData paramContainer = new QueryData.Builder(userID)
            //    .addStartDate(searchIntervalStartDate)
            //    .addEndDate(searchIntervalEndDate)
            //    .build();

            QueryData paramContainer = configureParamContainer();

            //sendDataToController(paramContainer);
            controller.requestData(QueryType.DATE_INTERVAL, paramContainer);

            DataTable retrievedData = (DataTable)receivableManagementDgv.DataSource;
            //Checks if the search has returned any results
            if (!hasFoundData(retrievedData)) {
                MessageBox.Show("No receivables found for the specified time interval!", "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void updateReceivableCtxMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            String clickedItem = e.ClickedItem.Name;
            String messageBoxTitle = "Receivables management";
            String template = "You clicked the {0} item at row index {1} and cell index {2}.";
            String message = null;

            switch (clickedItem) {
                case "markAsPaidItem":
                    message = String.Format(template, "'Mark as paid'", rowIndexOnRightClick, columnIndexOnRightClick);
                    //MessageBox.Show(message, messageBoxTitle);
                    //receivablesManagementPanel.Visible = false;
                    break;

                case "partialPaymentItem":
                    message = String.Format(template, "'Partial payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    //MessageBox.Show(message, messageBoxTitle);
                    //receivablesManagementPanel.Visible = false;
                    setupLayout(UIContainerLayout.INSERT_LAYOUT);
                    insertPartialPaymentButton.Enabled = false;
                    break;

                case "updateDetailsItem":
                    message = String.Format(template, "'Update payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    //MessageBox.Show(message, messageBoxTitle);
                    //addControlsToMainPanel();
                    //receivablesManagementPanel.Visible = true;
                    setupLayout(UIContainerLayout.UPDATE_LAYOUT);
                    DataGridViewRow currentRowData = retrieveDataFromSelectedRow(rowIndexOnRightClick, receivableManagementDgv);
                    try {
                        populateFormFields(currentRowData);
                        updateDgvRecordButton.Enabled = false;
                    } catch (FormatException ex) {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Invalid date format for the receivable due/created date! Unable to populate the update data form.", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } finally {
                        //updateDgvRecordButton.Enabled = false;
                    }

                    break;

                default:
                    break;
            }

        }

        private void receivableManagementDgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            MouseButtons pressedMouseBtn = e.Button;

            if (pressedMouseBtn == MouseButtons.Right) {
                rowIndexOnRightClick = e.RowIndex;
                columnIndexOnRightClick = e.ColumnIndex;
            }
        }

        private void receivableManagementDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            DataTable sourceDataTable = (DataTable)receivableManagementDgv.DataSource;

            for (int i = 0; i < sourceDataTable.Rows.Count; i++) {
                String currentStatus = Convert.ToString(sourceDataTable.Rows[i].ItemArray[5].ToString());
                Color statusColor = new Color();

                switch (currentStatus) {
                    case "New":
                        statusColor = Color.GreenYellow;
                        break;

                    case "Partially paid":
                        statusColor = Color.Orange;
                        break;

                    case "Paid":
                        statusColor = Color.Aquamarine;
                        break;

                    case "Overdue":
                        statusColor = Color.Red;
                        break;
                }

                receivableManagementDgv.Rows[i].DefaultCellStyle.BackColor = statusColor;
            }
        }

        private void updateDgvRecordButton_Click(object sender, EventArgs e) {
            DateTime createdDate = receivableCreatedDatePicker.Value;
            DateTime dueDate = receivableDueDatePicker.Value;

            int nameColumnIndex = 1;
            int debtorColumnIndex = 2;
            int valueColumnIndex = 4;
            int createdDateColumnIndex = 6;
            int dueDateColumnIndex = 7;

            String receivableName = itemNameTextBox.Text;
            String receivableValue = itemValueTextBox.Text;
            String receivableDebtorName = receivableDebtorComboBox.Text;
            String receivableCreatedDate = receivableCreatedDatePicker.Value.ToString("yyyy-MM-dd");
            String receivableDueDate = receivableDueDatePicker.Value.ToString("yyyy-MM-dd");

            Dictionary<int, String> cellIndexValueDictionary = new Dictionary<int, string>();
            cellIndexValueDictionary.Add(nameColumnIndex, receivableName);
            cellIndexValueDictionary.Add(valueColumnIndex, receivableValue);
            cellIndexValueDictionary.Add(debtorColumnIndex, receivableDebtorName);
            cellIndexValueDictionary.Add(createdDateColumnIndex, receivableCreatedDate);
            cellIndexValueDictionary.Add(dueDateColumnIndex, receivableDueDate);

            //Checks if the user has sperformed any changes on the data submitted for update
            DataGridViewRow currentSelectedRow = receivableManagementDgv.Rows[rowIndexOnRightClick];           
            if (!hasPerformedChanges(currentSelectedRow, cellIndexValueDictionary)) {
                String noChangesInfoMessage = "Cannot update the selected receivable because there were no changes performed on the submitted data! Please change the value of at least one of the form fields and try again.";
                MessageBox.Show(noChangesInfoMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            //Checks if the created date and due date of the receivable are in chronological order
            if (createdDate > dueDate || dueDate < createdDate) {
                MessageBox.Show("Invalid date selection! The receivable creation date must precede the due date!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            try {
                DataTable receivableDgvDataSource = (DataTable)receivableManagementDgv.DataSource;
                UserControlsManager.updateDataTable(receivableDgvDataSource, rowIndexOnRightClick, cellIndexValueDictionary);

                //Updates the number of pending changes and sets the corresponding message to the label that informs the user about them
                totalPendingChanges++;
                pendingChangesLabel.Text = String.Format("You have {0} pending {1}", totalPendingChanges, totalPendingChanges == 1 ? "change" : "changes");
                pendingChangesLabel.Visible = true;

            } catch (Exception ex) {
                string errorMessage = string.Format("Unable to update the specified row! Reason: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                rowIndexOnRightClick = -1;
                updateDgvRecordButton.Enabled = false;

            }

            UserControlsManager.clearActiveControls(activeControls);
            saveReceivableChangesButton.Enabled = true;
            discardChangesButton.Enabled = true;

        }

        public void insertPartialPaymentButton_Click(object sender, EventArgs e) {
            DataGridViewRow selectedReceivableData = retrieveDataFromSelectedRow(rowIndexOnRightClick, receivableManagementDgv);
            int selectedReceivableID = Convert.ToInt32(selectedReceivableData.Cells[0].Value);
            String selectedReceivableName = Convert.ToString(selectedReceivableData.Cells[1].Value);
            int selectedReceivableValue = Convert.ToInt32(selectedReceivableData.Cells[2].Value);
            String paymentName = itemNameTextBox.Text;
            int paymentValue = Convert.ToInt32(itemValueTextBox.Text);
            String paymentDate = partialPaymentDatePicker.Value.ToString("yyyy-MM-dd");

            PartialPaymentDTO partialPaymentDTO = new PartialPaymentDTO(selectedReceivableID, paymentName, paymentValue, paymentDate);

            DataInsertionCheckStrategy partialPaymentCheckStrategy = new PartialPaymentInsertionCheckStrategy(partialPaymentDTO);
            DataInsertionCheckerContext dataCheckContext = new DataInsertionCheckerContext();
            dataCheckContext.setStrategy(partialPaymentCheckStrategy);

            int checkResult = dataCheckContext.invoke();

            String userPrompt = String.Format("Are you sure that you want to insert a partial payment of {0} for the receivable '{1}'", paymentValue, selectedReceivableName);
            DialogResult userOption = MessageBox.Show(userPrompt, "Receivables management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOption == DialogResult.No) {
                return;
            }

            if (paymentValue >= selectedReceivableValue) {
                MessageBox.Show("The partial payment value must be lower than the receivable value!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            if (checkResult == -1) {
                MessageBox.Show("The partial payment value is higher than the amount left to be paid for the currently selected receivable!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataInsertionStrategy partialPaymentInsertionStrategy = new PartialPaymentInsertionStrategy();
            DataInsertionContext dataInsertionContext = new DataInsertionContext();
            dataInsertionContext.setStrategy(partialPaymentInsertionStrategy);

            int insertExecutionResult = dataInsertionContext.invoke(partialPaymentDTO);//Returns the number of affected rows by the insert query execution

            if (insertExecutionResult > 0) {
                String successMessage = String.Format("The partial payment for receivable '{0}' was successfully inserted!", selectedReceivableName);
                MessageBox.Show(successMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Clears the controls and disables the buttonused for inserting the partial payment
                UserControlsManager.clearActiveControls(activeControls);
                insertPartialPaymentButton.Enabled = false;
            } else {
                String errorMessage = String.Format("Unable to insert the partial payment for the receivable '{0}'!", selectedReceivableName);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Calls the procedure that updates the receivable status after the partial payment is inserted
            int statusUpdateExecutionResult = updateReceivableStatus(selectedReceivableID);

            if (statusUpdateExecutionResult == -1) {
                MessageBox.Show("Error while trying to update the receivable status!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void saveReceivableChangesButton_Click(object sender, EventArgs e) {
            //ONLY FOR TEST
            //DateTime startDate = receivableManagemenStartDatePicker.Value;
            //DateTime endDate = receivableManagementEndDatePicker.Value;

            //String searchIntervalStartDate = startDate.ToString("yyyy-MM-dd");
            //String searchIntervalEndDate = endDate.ToString("yyyy-MM-dd");

            //QueryData paramContainer = new QueryData.Builder(userID)
            //  .addStartDate(searchIntervalStartDate)
            //  .addEndDate(searchIntervalEndDate)
            //  .build();
            String messageTemplate = "Are you sure that you want to update the selected receivable{0}?";
            String confirmationMessage = totalPendingChanges == 1 ? String.Format(messageTemplate, "") : String.Format(messageTemplate, "s");

            DialogResult userOption = MessageBox.Show(confirmationMessage, "Receivables management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOption == DialogResult.No) {
                return;
            }

            int executionResult = -1;
            try {
                DataTable receivableManagementDT = (DataTable)receivableManagementDgv.DataSource;
                QueryData paramContainer = configureParamContainer();
                executionResult = controller.requestUpdate(QueryType.DATE_INTERVAL, paramContainer, receivableManagementDT);

            } catch (MySqlException ex) {
                MessageBox.Show("Unable to update the specified receivable/s! An error occured when trying to perform the operation.", "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //int executionResult = model.updateData(QueryType.UNDEFINED, paramContainer, (DataTable)receivableManagementDgv.DataSource);

            if (executionResult != -1) {
                String successMessage = totalPendingChanges == 1 ? "The selected receivable was successfully updated!" : "The selected receivable/s were successfully updated!";
                MessageBox.Show(successMessage, "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //The variables that track the changes are reset only if the update is successful, otherwise the user should have the chance to retry the saving of those changes
                totalPendingChanges = 0;
                pendingChangesLabel.Visible = false;
                saveReceivableChangesButton.Enabled = false;
                discardChangesButton.Enabled = false;
            } else {
                String errorMessage = totalPendingChanges == 1 ? "Unable to update the selected receivable!" : "Unable to update the selected receivables!";
                MessageBox.Show(errorMessage, "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void discardChangesButton_Click(object sender, EventArgs e) {
            DialogResult userOption = MessageBox.Show("Please select the changes that you want to discard", "Receivable management", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (userOption == DialogResult.OK) {
                DataTable receivableDgvDataSource = (DataTable)receivableManagementDgv.DataSource;
                //receivableDgvDataSource.RejectChanges();

                //totalPendingChanges = 0;
                //saveReceivableChangesButton.Enabled = false;
                //discardChangesButton.Enabled = false;
                //pendingChangesLabel.Visible = false;

                new DiscardReceivableChangeForm(receivableDgvDataSource).ShowDialog();
            }
        }

        private void exitButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void ReceivableManagementForm_Load(object sender, EventArgs e) {
            itemNameTextBox.TextChanged += new EventHandler(itemNameTextBox_TextChanged);
            itemValueTextBox.TextChanged += new EventHandler(itemValueTextBox_TextChanged);
            receivableDebtorComboBox.TextChanged += new EventHandler(receivableDebtorComboBox_TextChanged);
            receivableCreatedDatePicker.TextChanged += new EventHandler(receivableCreatedDatePicker_TextChanged);
            receivableDueDatePicker.TextChanged += new EventHandler(receivableDueDatePicker_TextChanged);
            updateDgvRecordButton.Click += new EventHandler(updateDgvRecordButton_Click);
            insertPartialPaymentButton.Click += new EventHandler(insertPartialPaymentButton_Click);

        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e) {
            Button currentlyActiveButton = null;

            //Sets the correct button as the currently active one based on the layout.This will ensure that the right button is enabled/disabled when form field data is present/missing
            if (currentLayout == UIContainerLayout.INSERT_LAYOUT) {
                currentlyActiveButton = insertPartialPaymentButton;
            } else if (currentLayout == UIContainerLayout.UPDATE_LAYOUT) {
                currentlyActiveButton = updateDgvRecordButton;
            }
            //Enables/or disables the update receivable button based on the presence/absence of data on the required form fields
            if (rowIndexOnRightClick != -1) {
                UserControlsManager.setButtonState(currentlyActiveButton, activeControls);
            }
        }

        private void itemValueTextBox_TextChanged(object sender, EventArgs e) {
            //Regex forbiddenCharacters = new Regex("[^0-9]+", RegexOptions.Compiled);
            Regex allowedCharacters = new Regex("^[1-9][0-9]*$", RegexOptions.Compiled);

            //Checks if the value text box contains non-digit characters and in that case it clears its content
            if (!allowedCharacters.IsMatch(itemValueTextBox.Text)) {
                itemValueTextBox.Text = "";
            }

            Button currentlyActiveButton = null;

            //Sets the correct button as the currently active one based on the layout.This will ensure that the right button is enabled/disabled when form field data is present/missing
            if (currentLayout == UIContainerLayout.INSERT_LAYOUT) {
                currentlyActiveButton = insertPartialPaymentButton;
            } else if (currentLayout == UIContainerLayout.UPDATE_LAYOUT) {
                currentlyActiveButton = updateDgvRecordButton;
            }

            //Enables/or disables the update receivable button based on the presence/absence of data on the required form fields
            if (rowIndexOnRightClick != -1) {
                UserControlsManager.setButtonState(currentlyActiveButton, activeControls);
            }
        }

        private void receivableDebtorComboBox_TextChanged(object sender, EventArgs e) {
            if (rowIndexOnRightClick != -1) {
                UserControlsManager.setButtonState(updateDgvRecordButton, activeControls);
            }
        }

        private void receivableCreatedDatePicker_TextChanged(object sender, EventArgs e) {
            if (rowIndexOnRightClick != -1) {
                UserControlsManager.setButtonState(updateDgvRecordButton, activeControls);
            }
        }

        private void receivableDueDatePicker_TextChanged(object sender, EventArgs e) {
            if (rowIndexOnRightClick != -1) {
                UserControlsManager.setButtonState(updateDgvRecordButton, activeControls);
            }
        }

        private void setComponentsLayout() {

        }

        //CONTROLS LAYOUT SETUP SECTION
        private void setupLayout(UIContainerLayout selectedOperationLayout) {
            //Clears the panel from the existing controls
            receivablesManagementPanel.Controls.Clear();

            if (activeControls != null) {
                //Clears the content of active controls
                UserControlsManager.clearActiveControls(activeControls);
            }

            List<Control> currentLayoutControlsList = null;
            //Creates the list of necessary components for the current operation(UPDATE/INSERT)
            switch (selectedOperationLayout) {
                case UIContainerLayout.UPDATE_LAYOUT:
                    currentLayoutControlsList = new List<Control> { receivableNameLabel, itemNameTextBox, receivableValueLabel, itemValueTextBox, receivableDebtorCbxLabel, receivableDebtorComboBox,
                    receivableCreatedDateLabel, receivableCreatedDatePicker, receivableDueDateLabel, receivableDueDatePicker, updateDgvRecordButton };
                    groupBox1.Text = "Update receivable";
                    break;

                case UIContainerLayout.INSERT_LAYOUT:
                    currentLayoutControlsList = new List<Control> { partialPaymentNameLabel, itemNameTextBox, partialPaymentValueLabel, itemValueTextBox, partialPaymentDateLabel, partialPaymentDatePicker, insertPartialPaymentButton };
                    groupBox1.Text = "Add partial payment";
                    break;

                default:
                    break;
            }

            //Adds the components to the selected container
            UserControlsManager.addControlsToContainer(receivablesManagementPanel, currentLayoutControlsList);
            //Populates the active controls list based on the components needed for the current operation(UPDATE/INSERT)
            populateActiveControlsList(selectedOperationLayout);
        }


        private void populateActiveControlsList(UIContainerLayout selectedOperationLayout) {
            switch (selectedOperationLayout) {
                case UIContainerLayout.UPDATE_LAYOUT:
                    activeControls = new List<FormFieldWrapper>() { new FormFieldWrapper(itemNameTextBox, true), new FormFieldWrapper(itemValueTextBox, true), new FormFieldWrapper(receivableDebtorComboBox, true), new FormFieldWrapper(receivableCreatedDatePicker, true), new FormFieldWrapper(receivableDueDatePicker, true), new FormFieldWrapper(updateDgvRecordButton, false) };
                    currentLayout = UIContainerLayout.UPDATE_LAYOUT;//Sets the current layout so that the form field data validation will be performed correctly(e.g: enabling/disabling the correct button in case of missing form field data)
                    break;

                case UIContainerLayout.INSERT_LAYOUT:
                    activeControls = new List<FormFieldWrapper>() { new FormFieldWrapper(itemNameTextBox, true), new FormFieldWrapper(itemValueTextBox, true), new FormFieldWrapper(partialPaymentDatePicker, true), new FormFieldWrapper(insertPartialPaymentButton, false) };
                    currentLayout = UIContainerLayout.INSERT_LAYOUT;
                    break;

                default:
                    break;
            }

        }

        //COMPONENTS CREATION SECTION
        private void createTextBoxes() {
            itemNameTextBox = new TextBox();
            itemNameTextBox.Width = 200;
            itemNameTextBox.Margin = new Padding(0, 0, 0, 0);

            itemValueTextBox = new TextBox();
            itemValueTextBox.Width = 200;
            itemValueTextBox.Margin = new Padding(0, 0, 0, 0);
        }

        private void createComboBoxes() {
            receivableDebtorComboBox = new ComboBox();
            receivableDebtorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            receivableDebtorComboBox.Margin = new Padding(0, 0, 0, 0);

        }

        private void createDateTimePickers() {
            receivableCreatedDatePicker = new DateTimePicker();
            receivableCreatedDatePicker.Format = DateTimePickerFormat.Custom;
            receivableCreatedDatePicker.CustomFormat = "dd-MM-yyyy";
            receivableCreatedDatePicker.Margin = new Padding(0, 0, 0, 0);

            receivableDueDatePicker = new DateTimePicker();
            receivableDueDatePicker.Format = DateTimePickerFormat.Custom;
            receivableDueDatePicker.CustomFormat = "dd-MM-yyyy";
            receivableDueDatePicker.Margin = new Padding(0, 0, 0, 0);

            partialPaymentDatePicker = new DateTimePicker();
            partialPaymentDatePicker.Margin = new Padding(0, 0, 0, 0);
        }

        private void createButtons() {
            updateDgvRecordButton = new Button();
            updateDgvRecordButton.Size = new Size(99, 23);
            updateDgvRecordButton.Margin = new Padding(0, 20, 0, 0);
            updateDgvRecordButton.Text = "Update record";
            updateDgvRecordButton.Name = "updateDgvRecordButton";
            //updateDgvRecordButton.Enabled = false;

            insertPartialPaymentButton = new Button();
            insertPartialPaymentButton.Size = new Size(105, 23);
            insertPartialPaymentButton.Margin = new Padding(0, 20, 0, 0);
            insertPartialPaymentButton.Text = "Add payment";
            insertPartialPaymentButton.Name = "insertPartialPaymentButton";
            //insertPartialPaymentButton.Enabled = false;

        }

        private void createLabels() {
            receivableNameLabel = new Label();
            receivableNameLabel.Text = "Receivable name";
            receivableNameLabel.Margin = new Padding(0, 10, 0, 0);

            receivableValueLabel = new Label();
            receivableValueLabel.Text = "Receivable value";
            receivableValueLabel.Margin = new Padding(0, 10, 0, 0);

            receivableDebtorCbxLabel = new Label();
            receivableDebtorCbxLabel.Text = "Receivable debtor";
            receivableDebtorCbxLabel.Margin = new Padding(0, 10, 0, 0);

            receivableCreatedDateLabel = new Label();
            receivableCreatedDateLabel.Text = "Creation date";
            receivableCreatedDateLabel.Margin = new Padding(0, 10, 0, 0);

            receivableDueDateLabel = new Label();
            receivableDueDateLabel.Text = "Due date";
            receivableDueDateLabel.Margin = new Padding(0, 10, 0, 0);

            partialPaymentNameLabel = new Label();
            partialPaymentNameLabel.Text = "Payment name";
            partialPaymentNameLabel.Margin = new Padding(0, 10, 0, 0);

            partialPaymentValueLabel = new Label();
            partialPaymentValueLabel.Text = "Payment value";
            partialPaymentValueLabel.Margin = new Padding(0, 10, 0, 0);

            partialPaymentDateLabel = new Label();
            partialPaymentDateLabel.Text = "Payment date";
            partialPaymentDateLabel.Margin = new Padding(0, 10, 0, 0);

        }

        private void wireUp(IUpdaterControl paramController, IUpdaterModel paramModel) {
            if (model != null) {
                model.removeObserver(this);
            }

            this.model = paramModel;
            this.controller = paramController;

            controller.setView(this);
            controller.setModel(model);

            model.addObserver(this);
        }

        public void updateView(IModel model) {
            receivableManagementDgv.DataSource = model.DataSources[0];
            //Sets the format for the date columns
            setFormatForDateTimeColumns(receivableManagementDgv, "dd-MM-yyyy");
        }

        public void disableControls() {
            displayReceivablesButton.Enabled = false;
        }

        public void enableControls() {
            displayReceivablesButton.Enabled = true;
        }

        //UTILITY METHODS

        //Method that retreves data from the selected row when the user selects the "Update data" option
        private DataGridViewRow retrieveDataFromSelectedRow(int selectedRowIndex, DataGridView targetDataGridView) {
            Guard.notNull(targetDataGridView, "receivable management grid view", "Unable to retrieve data from a null object.");

            //Checks that the row index is within bounds
            if (selectedRowIndex < 0 || selectedRowIndex > targetDataGridView.Rows.Count) {
                return new DataGridViewRow();
            }

            //String receivableID = targetDataGridView.Rows[selectedRowIndex].Cells[0].Value.ToString();
            //String receivableName = targetDataGridView.Rows[selectedRowIndex].Cells[1].Value.ToString();
            //String receivableValue = targetDataGridView.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //String receivableDebtorName = targetDataGridView.Rows[selectedRowIndex].Cells[3].Value.ToString();
            //String receivableStatus = targetDataGridView.Rows[selectedRowIndex].Cells[5].Value.ToString();
            //String createdDate = targetDataGridView.Rows[selectedRowIndex].Cells[6].Value.ToString();
            //String dueDate = targetDataGridView.Rows[selectedRowIndex].Cells[7].Value.ToString();

            //ArrayList selectedRowData = new ArrayList() { receivableID, receivableName, receivableValue, receivableDebtorName, receivableStatus, createdDate, dueDate };

            return receivableManagementDgv.Rows[selectedRowIndex];
        }

        //Method that populates the update data form fields with the data from the currently selected row when the user selects the "Update data" option
        private void populateFormFields(DataGridViewRow selectedDgvRow) {
            Guard.notNull(selectedDgvRow, "form fields data source", "Unable to populate the form fields because the data source is null!");

            int requiredElements = 5;

            if (selectedDgvRow.Cells.Count < requiredElements) {
                return;
            }

            itemNameTextBox.Text = selectedDgvRow.Cells[1].Value.ToString();
            receivableDebtorComboBox.Text = selectedDgvRow.Cells[2].Value.ToString();
            itemValueTextBox.Text = selectedDgvRow.Cells[4].Value.ToString();


            DateTime createdDate;
            DateTime dueDate;


            bool canParseCreatedDate = DateTime.TryParse(selectedDgvRow.Cells[6].ToString(), out createdDate);
            bool canParseDueDate = DateTime.TryParse(selectedDgvRow.Cells[7].ToString(), out dueDate);

            ////FORMAT CHANGE
            //bool canParseCreatedDate = DateTime.TryParseExact(selectedDgvRow.Cells[6].ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"), DateTimeStyles.AssumeLocal, out createdDate);
            //bool canParseDueDate = DateTime.TryParseExact(selectedDgvRow.Cells[7].ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"), DateTimeStyles.AssumeLocal, out dueDate);

            //if (canParseCreatedDate && canParseDueDate) {
            //    receivableCreatedDatePicker.Value = createdDate;
            //    receivableDueDatePicker.Value = dueDate;
            //} else {
            //    String illegalFormatString = canParseCreatedDate == false ? dataSource[3].ToString() : dataSource[4].ToString();
            //    throw new FormatException(String.Format("Invalid format for date string: {0}", illegalFormatString));
            //}

            //OLD
            //receivableCreatedDatePicker.Value = DateTime.ParseExact(selectedDgvRow.Cells[6].Value.ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"));
            //receivableDueDatePicker.Value = DateTime.ParseExact(selectedDgvRow.Cells[7].Value.ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"));

            //NEW
            receivableCreatedDatePicker.Value = (DateTime)selectedDgvRow.Cells[6].Value;
            receivableDueDatePicker.Value = (DateTime)selectedDgvRow.Cells[7].Value;
        }

        ////Method that retreves data from the selected row when the user selects the "Update data" option
        //private ArrayList retrieveDataFromSelectedRow(int selectedRowIndex, DataGridView targetDataGridView) {
        //    Guard.notNull(targetDataGridView, "receivable management grid view", "Unable to retrieve data from a null object.");

        //    //Checks that the row index is within bounds
        //    if (selectedRowIndex < 0 || selectedRowIndex > targetDataGridView.Rows.Count) {
        //        return new ArrayList();
        //    }

        //    receivableManagementDgv.Rows[selectedRowIndex];

        //    String receivableID = targetDataGridView.Rows[selectedRowIndex].Cells[0].Value.ToString();
        //    String receivableName = targetDataGridView.Rows[selectedRowIndex].Cells[1].Value.ToString();
        //    String receivableValue = targetDataGridView.Rows[selectedRowIndex].Cells[2].Value.ToString();
        //    String receivableDebtorName = targetDataGridView.Rows[selectedRowIndex].Cells[3].Value.ToString();
        //    String receivableStatus = targetDataGridView.Rows[selectedRowIndex].Cells[5].Value.ToString();
        //    String createdDate = targetDataGridView.Rows[selectedRowIndex].Cells[6].Value.ToString();
        //    String dueDate = targetDataGridView.Rows[selectedRowIndex].Cells[7].Value.ToString();

        //    ArrayList selectedRowData = new ArrayList() { receivableID, receivableName, receivableValue, receivableDebtorName, receivableStatus, createdDate, dueDate };

        //    return selectedRowData;
        //}

        ////Method that populates the update data form fields with the data from the currently selected row when the user selects the "Update data" option
        //private void populateFormFields(ArrayList dataSource) {
        //    Guard.notNull(dataSource, "form fields data source", "Unable to populate the form fields because the data source is null!");

        //    int requiredElements = 5;

        //    if (dataSource.Count < requiredElements) {
        //        return;
        //    }

        //    itemNameTextBox.Text = dataSource[1].ToString();
        //    itemValueTextBox.Text = dataSource[3].ToString();
        //    receivableDebtorComboBox.Text = dataSource[2].ToString();

        //    DateTime createdDate;
        //    DateTime dueDate;


        //    //bool canParseCreatedDate = DateTime.TryParse(dataSource[3].ToString(), out createdDate);
        //    //bool canParseDueDate = DateTime.TryParse(dataSource[4].ToString(), out dueDate);

        //    //FORMAT CHANGE
        //    bool canParseCreatedDate = DateTime.TryParseExact(dataSource[5].ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"), DateTimeStyles.AssumeLocal, out createdDate);
        //    bool canParseDueDate = DateTime.TryParseExact(dataSource[6].ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"), DateTimeStyles.AssumeLocal, out dueDate);

        //    //if (canParseCreatedDate && canParseDueDate) {
        //    //    receivableCreatedDatePicker.Value = createdDate;
        //    //    receivableDueDatePicker.Value = dueDate;
        //    //} else {
        //    //    String illegalFormatString = canParseCreatedDate == false ? dataSource[3].ToString() : dataSource[4].ToString();
        //    //    throw new FormatException(String.Format("Invalid format for date string: {0}", illegalFormatString));
        //    //}

        //    receivableCreatedDatePicker.Value = DateTime.ParseExact(dataSource[5].ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"));
        //    receivableDueDatePicker.Value = DateTime.ParseExact(dataSource[6].ToString(), "dd-MM-yyyy", new CultureInfo("fr-FR"));

        //}

        private int updateReceivableStatus(int receivableID) {
            String procedureName = "set_receivable_status";

            //Creates the input parameter
            MySqlParameter receivableIDParam = new MySqlParameter("p_receivable_ID", receivableID);

            //Creates the lists that will contain the input and output parameters. In this case the output parameters list is empty because the called procedure doesn't have output parameter/s
            List<MySqlParameter> inputParamsList = new List<MySqlParameter>() { receivableIDParam };
            List<MySqlParameter> outputParamsList = new List<MySqlParameter>();

            try {

                DBConnectionManager.callDatabaseStoredProcedure(procedureName, inputParamsList, outputParamsList);

            } catch (MySqlException ex) {
                Console.WriteLine(String.Format("Error while trying to update the receivable status.Reason: {0}", ex.Message));
                return -1;
            }

            return 0;
        }

        //Method that manages the sending of data to the controller object based on the specified CRUD operation 
        private void sendDataToController(CRUDOperation dataOperation, QueryData paramContainer) {
            //switch(dataOperation) {
            //    case CRUDOperation.READ:
            //        controller.requestData(QueryType.DATE_INTERVAL, paramContainer);
            //        break;

            //    case CRUDOperation.UPDATE:
            //        //Retrieves the data table which will be used for update
            //        DataTable receivablesManagementDT = (DataTable) receivableManagementDgv.DataSource;
            //        controller.requestUpdate(QueryType.DATE_INTERVAL, paramContainer, receivablesManagementDT);
            //        break;

            //    default:
            //        break;
            //}
            controller.requestData(QueryType.DATE_INTERVAL, paramContainer);
        }

        private bool isValidSearchInterval(DateTime startDate, DateTime endDate) {
            String message = "Invalid date selection!{0}";
            String reason = "";

            if (startDate.Date > endDate.Date) {
                reason = "The start date cannot be after the end date.";
                message = String.Format(message, reason);
            } else if (endDate.Date < startDate.Date) {
                reason = "The end date cannot be before the start date.";
                message = String.Format(message, reason);
            }

            //Show the warning message only if the reason is not empty(which means that one of the situations checked by the previous if clause is applicable)
            if (!"".Equals(reason)) {
                MessageBox.Show(message, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        //Checks if the retrieved data table contains data
        private bool hasFoundData(DataTable dataTable) {
            if (dataTable == null) {
                return false;
            }

            return dataTable.Rows.Count > 0;
        }

        private void setFormatForDateTimeColumns(DataGridView dataGridView, String customFormat) {
            if (dataGridView == null) {
                return;
            }

            try {
                String formattedDate = DateTime.Now.ToString(customFormat);
            } catch (FormatException ex) {
                Console.WriteLine("The specified format is invalid! It cannot be applied to the DataGridView column.");
                return;
            }

            foreach (DataGridViewColumn currentColumn in dataGridView.Columns) {
                if (currentColumn.ValueType == typeof(DateTime)) {
                    currentColumn.DefaultCellStyle.Format = customFormat;
                }
            }

        }

        private QueryData configureParamContainer() {
            String searchIntervalStartDate = receivableManagemenStartDatePicker.Value.ToString("yyyy-MM-dd");
            String searchIntervalEndDate = receivableManagementEndDatePicker.Value.ToString("yyyy-MM-dd");

            QueryData paramContainer = new QueryData.Builder(userID)
            .addStartDate(searchIntervalStartDate)
            .addEndDate(searchIntervalEndDate)
            .build();

            return paramContainer;
        }

        //Method used for checking if the user has modified any of the values from the update receivable form
        private bool hasPerformedChanges(DataGridViewRow originalReceivableData, Dictionary<int, String> fieldDataDictionary) {
            foreach (DataGridViewCell currentCell in originalReceivableData.Cells) {
                int currentCellIndex = currentCell.ColumnIndex;

                if (fieldDataDictionary.ContainsKey(currentCellIndex)) {
                    String fieldValue = fieldDataDictionary[currentCellIndex];
                    String cellValue = currentCell.Value.ToString();
                   
                    //This if statement is added in order to compare the date values as objects and not as strings because they are in different formats(cell value format-> dd-MM-yyyy; field value format-> yyyy-MM-dd)
                    if (currentCell.Value.GetType() == typeof(DateTime)) {
                        DateTime cellDate;
                        DateTime fieldDate;

                        //Date parsing precheck
                        bool canParseCellContent = DateTime.TryParse(cellValue, out cellDate);
                        bool canParseFieldContent = DateTime.TryParse(fieldValue, out fieldDate);

                        //Checks if the date values can be parsed an if they are different
                        if ((canParseCellContent && canParseFieldContent) && cellDate.CompareTo(fieldDate) != 0) {
                            return true;
                        }
                    } else {
                        //If at least one value is changed then the method will return true as changes have been performed on the data
                        if (!fieldValue.Equals(currentCell.Value.ToString())) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        private void receivableManagementDgv_DataSourceChanged(object sender, EventArgs e) {
            //DataColumn discardChangeColumn = new DataColumn("Discard change", typeof(bool));
            //discardChangeColumn.DefaultValue = false;
            //((DataTable)receivableManagementDgv.DataSource).GetChanges().Columns.Add(discardChangeColumn);
        }

        private void receivableManagementDgv_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e) {
            //If the current column contains a datetime value then it will be formatted to "dd-MM-yyyy" format
            //if (e.Column.ValueType == typeof(DateTime)) {
            //    dateTimeColumnIndexes.Add(e.Column.Index);
            //}
        }
    }
}
