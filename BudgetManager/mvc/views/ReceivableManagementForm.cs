using BudgetManager.mvc.controllers;
using BudgetManager.mvc.models;
using BudgetManager.mvc.models.dto;
using BudgetManager.non_mvc;
using BudgetManager.utils;
using BudgetManager.utils.data_insertion;
using BudgetManager.utils.enums;
using BudgetManager.utils.ui_controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BudgetManager.mvc.views {
    public partial class ReceivableManagementForm : Form, IView {
        private int userID;
        private int rowIndexOnRightClick;
        private int columnIndexOnRightClick;
        private String selectedReceivableID;
        private int totalPendingChanges;
        private String pendingChangesMessage;

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
            this.pendingChangesMessage = "You have {0} pending change{1}";
            controlsManager = new UserControlsManager();
            currentLayout = UIContainerLayout.UNDEFINED;
            selectedReceivableID = "";

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

            QueryData paramContainer = configureParamContainer();

            //sendDataToController(paramContainer);
            controller.requestData(QueryType.DATE_INTERVAL, paramContainer);

            DataTable retrievedData = (DataTable)receivableManagementDgv.DataSource;

            /*Resets the total pending changes when the user selects the "Display receivables" button so that the user can't discard changes which are no longer present
            (e.g: user performs a change, wants to see receivables and presses the display button and then tries to discard the change that was made in the first place(which is now gone because the data source was reset)*/
            totalPendingChanges = 0;
            pendingChangesLabel.Text = "";
            discardChangesButton.Enabled = false;

            //Checks if the search has returned any results
            if (!hasFoundData(retrievedData)) {
                MessageBox.Show("No receivables found for the specified time interval!", "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void updateReceivableCtxMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            String clickedItem = e.ClickedItem.Name;
            String template = "You clicked the {0} item at row index {1} and cell index {2}.";
            String message = null;
            DataGridViewRow currentRowData = null;
            DataTable dataSource = (DataTable)receivableManagementDgv.DataSource;

            switch (clickedItem) {
                case "markAsPaidItem":
                    message = String.Format(template, "'Mark as paid'", rowIndexOnRightClick, columnIndexOnRightClick);
                    markReceivableAsPaid(rowIndexOnRightClick, dataSource);
                    break;

                case "partialPaymentItem":
                    message = String.Format(template, "'Partial payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    setupLayout(UIContainerLayout.INSERT_LAYOUT);
                    //Receivable ID retrieval
                    currentRowData = retrieveDataFromSelectedRow(rowIndexOnRightClick, receivableManagementDgv);
                    selectedReceivableID = currentRowData.Cells[0].Value.ToString();
                    insertPartialPaymentButton.Enabled = false;
                    //Enables the partial payment name and value textboxes
                    itemNameTextBox.Enabled = true;
                    itemValueTextBox.Enabled = true;
                    break;

                case "updateDetailsItem":
                    message = String.Format(template, "'Update payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    setupLayout(UIContainerLayout.UPDATE_LAYOUT);
                    //Receivable ID retrieval
                    currentRowData = retrieveDataFromSelectedRow(rowIndexOnRightClick, receivableManagementDgv);
                    selectedReceivableID = currentRowData.Cells[0].Value.ToString();
                    try {
                        populateFormFields(currentRowData);
                        updateDgvRecordButton.Enabled = false;
                        setUpdateFormComponentsState(true);//Enables the update form components after the user presses the "Update details" option from the context menu 
                    } catch (FormatException ex) {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Invalid date format for the receivable due/created date! Unable to populate the update data form.", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } finally {
                    }

                    break;

                case "deleteItem":
                    deleteSelectedReceivable(rowIndexOnRightClick, dataSource);
                    break;

                default:
                    break;
            }

        }

        private void receivableManagementDgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            MouseButtons pressedMouseBtn = e.Button;

            if (pressedMouseBtn == MouseButtons.Right) {
                updateDgvRecordButton.Enabled = false;
                rowIndexOnRightClick = e.RowIndex;
                columnIndexOnRightClick = e.ColumnIndex;
            }
        }

        private void receivableManagementDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            DataTable sourceDataTable = (DataTable)receivableManagementDgv.DataSource;

            for (int i = 0; i < sourceDataTable.Rows.Count; i++) {
                //This check will prevent DeletedRowInaccessibleException after deleting a row
                if (sourceDataTable.Rows[i].RowState == DataRowState.Deleted) {
                    continue;
                }
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
            int paidAmountColumnIndex = 3;
            int valueColumnIndex = 4;
            int statusColumnIndex = 5;
            int createdDateColumnIndex = 6;
            int dueDateColumnIndex = 7;

            String receivableName = itemNameTextBox.Text;
            String receivableDebtorName = receivableDebtorComboBox.Text;
            String receivableValue = itemValueTextBox.Text;
            String receivableCreatedDate = receivableCreatedDatePicker.Value.ToString("yyyy-MM-dd");
            String receivableDueDate = receivableDueDatePicker.Value.ToString("yyyy-MM-dd");

            Dictionary<int, String> cellIndexValueDictionary = new Dictionary<int, string>();
            cellIndexValueDictionary.Add(nameColumnIndex, receivableName);
            cellIndexValueDictionary.Add(valueColumnIndex, receivableValue);
            cellIndexValueDictionary.Add(debtorColumnIndex, receivableDebtorName);
            cellIndexValueDictionary.Add(createdDateColumnIndex, receivableCreatedDate);
            cellIndexValueDictionary.Add(dueDateColumnIndex, receivableDueDate);

            //Calls the update prechecks method. The column index parameters are necessary in order for this method to correctly extract the data from the cellIndexValueDictionary object
            int updatePrechecksResult = performUpdatePrechecks(cellIndexValueDictionary, nameColumnIndex, paidAmountColumnIndex, valueColumnIndex, statusColumnIndex, createdDateColumnIndex, dueDateColumnIndex);

            //When the method return -1 it means that the precheck has failed and when it returns 1 it means that the user selected 'No' when prompted to take a decision*/
            if (updatePrechecksResult == -1 || updatePrechecksResult == 1) {
                return;
            }

            try {
                //Retrieves the row index on which the update option was selected using the receivable ID (this way, no matter where the user clicks after selecting the update option, only the correct receivable will be updated)
                DataTable receivableDgvDataSource = (DataTable)receivableManagementDgv.DataSource;
                int receivableIDColumnIndex = 0;
                int selectedReceivableRowIndex = DataSourceManager.getRowIndexBasedOnID(selectedReceivableID, receivableIDColumnIndex, receivableDgvDataSource);
                DataSourceManager.updateDataTable(receivableDgvDataSource, selectedReceivableRowIndex, cellIndexValueDictionary);

                //Message for informing the user about the receivable update
                String updateConfirmationMessage = String.Format("The receivable '{0}' was successfully updated in the table. Click the 'Save changes' button if you want to permanently save the changes.", receivableName);
                MessageBox.Show(updateConfirmationMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Updates the number of pending changes and sets the corresponding message to the label that informs the user about them
                totalPendingChanges = receivableDgvDataSource.GetChanges().Rows.Count;
                pendingChangesLabel.Text = String.Format(pendingChangesMessage, totalPendingChanges, totalPendingChanges > 1 ? "s" : "");
                pendingChangesLabel.Visible = true;

            } catch (Exception ex) {
                string errorMessage = string.Format("Unable to update the specified row! Reason: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                rowIndexOnRightClick = -1;
                selectedReceivableID = "";//Resets the receivable ID regardless of the update result
                updateDgvRecordButton.Enabled = false;
                setUpdateFormComponentsState(false);//Disables the update form components after the user presses the "Update record" button
            }

            UserControlsManager.clearActiveControls(activeControls);
            saveReceivableChangesButton.Enabled = true;
            discardChangesButton.Enabled = true;

        }

        public void insertPartialPaymentButton_Click(object sender, EventArgs e) {
            //Checks if the user tries to insert a partial payment without selecting a receivable(e.g: by trying to add a partial payment right after another partial payment was inserted without re-selecting the receivable)
            if ("".Equals(selectedReceivableID)) {
                MessageBox.Show("Please select the receivable for which you want to insert the partial payment!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /*Retrieves the row index on which the partial payment option was selected using the receivable ID 
            (this way, no matter where the user clicks after selecting the partial payment option, the partial payment will be inserted only for the correct receivable)*/
            DataTable receivableDgvDataSource = (DataTable)receivableManagementDgv.DataSource;
            int receivableIDColumnIndex = 0;
            int selectedReceivableRowIndex = DataSourceManager.getRowIndexBasedOnID(selectedReceivableID, receivableIDColumnIndex, receivableDgvDataSource);
            DataGridViewRow selectedReceivableData = retrieveDataFromSelectedRow(selectedReceivableRowIndex, receivableManagementDgv);

            int parsedReceivableID = Convert.ToInt32(selectedReceivableID);
            String selectedReceivableName = Convert.ToString(selectedReceivableData.Cells[1].Value);
            int selectedReceivableValue = Convert.ToInt32(selectedReceivableData.Cells[4].Value);
            String paymentName = itemNameTextBox.Text;
            int paymentValue = Convert.ToInt32(itemValueTextBox.Text);
            String paymentDate = partialPaymentDatePicker.Value.ToString("yyyy-MM-dd");

            PartialPaymentDTO partialPaymentDTO = new PartialPaymentDTO(parsedReceivableID, paymentName, paymentValue, paymentDate);

            DataInsertionCheckStrategy partialPaymentCheckStrategy = new PartialPaymentInsertionCheckStrategy(partialPaymentDTO);
            DataInsertionCheckerContext dataCheckContext = new DataInsertionCheckerContext();
            dataCheckContext.setStrategy(partialPaymentCheckStrategy);

            DataCheckResponse partialPaymentCheckResponse = dataCheckContext.invoke();

            String userPrompt = String.Format("Are you sure that you want to insert a partial payment of {0} for the receivable '{1}'?", paymentValue, selectedReceivableName);
            DialogResult userOption = MessageBox.Show(userPrompt, "Receivables management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOption == DialogResult.No) {
                return;
            }

            if (paymentValue >= selectedReceivableValue) {
                MessageBox.Show("The partial payment value must be lower than the receivable value!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            if (partialPaymentCheckResponse.ExecutionResult == -1) {
                MessageBox.Show(partialPaymentCheckResponse.ErrorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                //Automatic refresh of the receivable data after a successfull partial payment insertion
                refreshReceivableManagementDgv();
            } else {
                String errorMessage = String.Format("Unable to insert the partial payment for the receivable '{0}'!", selectedReceivableName);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Clears the value of the receivable ID whose row was selected regardless of the partial payment insertion outcome
            selectedReceivableID = "";
        }

        private void saveReceivableChangesButton_Click(object sender, EventArgs e) {
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

                //Commits all the changes performed on the source data table so that after the changes are saved to the database the pending changes count is reset(calling the GetChanges() method on this data table will return 0)
                receivableManagementDT.AcceptChanges();

            } catch (MySqlException ex) {
                MessageBox.Show("Unable to update the specified receivable/s! An error occured when trying to perform the operation.", "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (executionResult != -1) {
                String successMessage = totalPendingChanges == 1 ? "The selected receivable was successfully updated!" : "The selected receivables were successfully updated!";
                MessageBox.Show(successMessage, "Receivables management", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //The variables that track the changes are reset only if the update is successful, otherwise the user should have the chance to retry the saving of those changes
                totalPendingChanges = 0;
                pendingChangesLabel.Visible = false;
                saveReceivableChangesButton.Enabled = false;
                discardChangesButton.Enabled = false;

            } else {
                String errorMessage = totalPendingChanges == 1 ? "Unable to update the selected receivable!" : "Unable to update the selected receivables!";
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void discardChangesButton_Click(object sender, EventArgs e) {
            DialogResult userOption = MessageBox.Show("Please select the changes that you want to discard!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (userOption == DialogResult.OK) {
                DataTable receivableDgvDataSource = (DataTable)receivableManagementDgv.DataSource;

                new DiscardReceivableChangeForm(receivableDgvDataSource, this).ShowDialog();
            }
        }

        private void exitButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void ReceivableManagementForm_Load(object sender, EventArgs e) {
            itemNameTextBox.TextChanged += new EventHandler(itemNameTextBox_TextChanged);
            itemValueTextBox.TextChanged += new EventHandler(itemValueTextBox_TextChanged);
            receivableDebtorComboBox.SelectedIndexChanged += new EventHandler(receivableDebtorComboBox_SelectedIndexChanged);
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

        private void receivableDebtorComboBox_SelectedIndexChanged(object sender, EventArgs e) {
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

            insertPartialPaymentButton = new Button();
            insertPartialPaymentButton.Size = new Size(105, 23);
            insertPartialPaymentButton.Margin = new Padding(0, 20, 0, 0);
            insertPartialPaymentButton.Text = "Add payment";
            insertPartialPaymentButton.Name = "insertPartialPaymentButton";
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
            //Disables column sorting for data grid view columns
            receivableManagementDgv
                .Columns
                .Cast<DataGridViewColumn>()
                .ToList()
                .ForEach(currentColumn => currentColumn.SortMode = DataGridViewColumnSortMode.NotSortable);
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

            receivableCreatedDatePicker.Value = (DateTime)selectedDgvRow.Cells[6].Value;
            receivableDueDatePicker.Value = (DateTime)selectedDgvRow.Cells[7].Value;
        }

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

        /*Method for performing the prechecks related to the receivable update
         Returns:
         -1 -> when the precheck fails
          0 -> when the precheck is passed
          1 -> when the user selects 'No" when prompted to take a decision */
        private int performUpdatePrechecks(Dictionary<int, String> cellIndexValueDictionary, int receivableNameColumnIndex, int totalPaidAmountColumnIndex, int receivableValueColumnIndex, int receivableStatusColumnIndex, int createdDateColumnIndex, int dueDateColumnIndex) {
            String receivableName = cellIndexValueDictionary[receivableNameColumnIndex];

            //Checks if the user has performed any changes on the data submitted for update
            DataGridViewRow currentSelectedRow = receivableManagementDgv.Rows[rowIndexOnRightClick];
            if (!hasPerformedChanges(currentSelectedRow, cellIndexValueDictionary)) {
                String noChangesInfoMessage = "Cannot update the selected receivable because there were no changes performed on the submitted data! Please change the value of at least one of the form fields and try again.";
                MessageBox.Show(noChangesInfoMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return -1;
            }

            DateTime createdDate = DateTime.Parse(cellIndexValueDictionary[createdDateColumnIndex]);
            DateTime dueDate = DateTime.Parse(cellIndexValueDictionary[dueDateColumnIndex]);

            //Checks if the created date and due date of the receivable are in chronological order
            if (createdDate > dueDate) {
                MessageBox.Show("Invalid date selection! The receivable creation date must precede the due date!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return -1;
            }

            //Retrieves the value of the total paid amount column for the selected receivable 
            String receivableValue = cellIndexValueDictionary[receivableValueColumnIndex];
            int totalPaidAmount;
            int receivableAmount;
            bool canParseTotalPaidAmountValue = int.TryParse(currentSelectedRow.Cells[totalPaidAmountColumnIndex].Value.ToString(), out totalPaidAmount);
            bool canParseReceivableValue = int.TryParse(receivableValue, out receivableAmount);

            if (canParseTotalPaidAmountValue && canParseReceivableValue) {
                //If the receivable value is equal to the total paid the amount the receivable is considered paid and its status will be set accordingly
                if (receivableAmount == totalPaidAmount) {
                    String automaticStatusUpdateMessage = String.Format("The receivable '{0}' was updated and its value is now equal to the total paid amount. As a result, the receivable status will automatically be changed to 'Paid'. Are you sure that you want to continue?", receivableName);
                    DialogResult userOption = MessageBox.Show(automaticStatusUpdateMessage, "Receivable management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (userOption == DialogResult.No) {
                        return 1;
                    }

                    String newReceivableStatus = "Paid";
                    cellIndexValueDictionary.Add(receivableStatusColumnIndex, newReceivableStatus);
                } else if (receivableAmount < totalPaidAmount) {
                    //The updated receivable value cannot be lower than that of the total paid amount value
                    String invalidReceivableValueMessage = String.Format("You are not allowed to set a value which is lower than the total paid amount for the receivable '{0}'! Please correct the value and try again.", receivableName);
                    MessageBox.Show(invalidReceivableValueMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
                }
            } else {
                //Handles the situation when any of the two values(receivable value or total paid amount) cannot be parsed
                String valueParsingErrorMessage = String.Format("Unable to parse the receivable value/total paid amount value and perform all the required update prechecks! The receivable '{0}' cannot be currently updated!", receivableName);
                MessageBox.Show(valueParsingErrorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            return 0;
        }

        public void updateFormAfterDiscardingChanges(DataTable dataTableWithDiscardedChanges) {
            //If the DataTable object received from the DiscardReceivableChangeForm is null it means that none of the changes were discarded so no other action should be performed
            if (dataTableWithDiscardedChanges == null) {
                return;
            }

            //Sets the data source of the receivable management grid view as the DataTable with the discarded changes
            receivableManagementDgv.DataSource = dataTableWithDiscardedChanges;
            DataTable pendingChangesDT = dataTableWithDiscardedChanges.GetChanges();

            if (pendingChangesDT == null) {
                totalPendingChanges = 0;
                pendingChangesLabel.Text = "";
                //Disables the "Discard changes" button only if there are no pending changes left
                discardChangesButton.Enabled = false;
            } else {
                totalPendingChanges = pendingChangesDT.Rows.Count;
                pendingChangesLabel.Text = String.Format(pendingChangesMessage, totalPendingChanges, totalPendingChanges > 1 ? "s" : "");
            }
        }

        private void markReceivableAsPaid(int markedReceivableRowIndex, DataTable sourceDataTable) {
            //Retrieves the row containing the receivable to be marked as paid
            DataGridViewRow rowToMarkAsPaid = retrieveDataFromSelectedRow(markedReceivableRowIndex, receivableManagementDgv);
            String receivableName = rowToMarkAsPaid.Cells[1].Value.ToString();

            String confirmationMessage = String.Format("Are you sure that you want to mark the receivable '{0}' as paid? By continuing, the total/remaining receivable value will be returned to the source account and it will be considered that the debtor has fully paid his debt.", receivableName);
            DialogResult userOption = MessageBox.Show(confirmationMessage, "Receivable management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOption == DialogResult.No) {
                return;
            }

            String receivableValue = rowToMarkAsPaid.Cells[4].Value.ToString();
            String newStatus = "Paid";
            String payOffDate = DateTime.Now.ToString("yyyy-MM-dd");

            Dictionary<int, String> cellIndexValueDictionary = new Dictionary<int, string>();
            cellIndexValueDictionary.Add(3, receivableValue);//in order to mark the receivable as paid the total paid amount must be set to the value of the receivable
            cellIndexValueDictionary.Add(5, newStatus);//the status is set to "Paid"
            cellIndexValueDictionary.Add(8, payOffDate);//sets the pay off date which indicates the date when the receivable was fully paid

            int executionResult = -1;
            try {
                //Updates the data source of the DataGridView
                DataSourceManager.updateDataTable(sourceDataTable, markedReceivableRowIndex, cellIndexValueDictionary);
                //Prepares the required parameters for calling the update method
                QueryType option = QueryType.DATE_INTERVAL;
                QueryData paramContainer = configureParamContainer();
                executionResult = controller.requestUpdate(option, paramContainer, sourceDataTable);
            } catch (MySqlException ex) {
                String markAsPaidErrorMessage = String.Format("Unable to mark the receivable '{0}' as paid! An error occured while trying to perform the operation.", receivableName);
                MessageBox.Show(markAsPaidErrorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (executionResult != -1) {
                String successMessage = String.Format("The receivable '{0}' was successfully marked as paid!", receivableName);
                MessageBox.Show(successMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                String errorMessage = String.Format("Unable to mark the receivable '{0}' as paid!", receivableName);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void deleteSelectedReceivable(int deletedRowIndex, DataTable sourceDataTable) {
            DataGridViewRow rowToDelete = retrieveDataFromSelectedRow(deletedRowIndex, receivableManagementDgv);
            String receivableName = rowToDelete.Cells[1].Value.ToString();
            String deleteConfirmationMessage = String.Format("Are you sure that you want to delete the receivable '{0}'? This will remove the receivable itself, all the associated partial payments(if any) and will rollback all the changes made to the source account balance.", receivableName);

            DialogResult userOption = MessageBox.Show(deleteConfirmationMessage, "Receivable management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOption == DialogResult.No) {
                return;
            }

            if (totalPendingChanges > 0) {
                MessageBox.Show("You cannot delete receivables if you have pending updates! Please save/discard the updated and then try again!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int executionResult = -1;
            try {
                DataSourceManager.deleteDataTableRow(deletedRowIndex, sourceDataTable);
                QueryType option = QueryType.DATE_INTERVAL;
                QueryData paramContainer = configureParamContainer();
                executionResult = controller.requestDelete(option, paramContainer, sourceDataTable);

            } catch (MySqlException ex) {
                String deleteErrorMessage = String.Format("Unable to delete the receivable '{0}'! An error occured while trying to perform the operation.", receivableName);
                MessageBox.Show(deleteErrorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (executionResult != -1) {
                String successMessage = String.Format("The receivable '{0}' was successfully deleted!", receivableName);
                MessageBox.Show(successMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                String errorMessage = String.Format("Unable to delete the receivable '{0}'!", receivableName);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Method used to refresh the data after the user performs an action on a receivable
        private void refreshReceivableManagementDgv() {
            DateTime startDate = receivableManagemenStartDatePicker.Value;
            DateTime endDate = receivableManagementEndDatePicker.Value;

            QueryData paramContainer = configureParamContainer();

            controller.requestData(QueryType.DATE_INTERVAL, paramContainer);
        }

        private void setUpdateFormComponentsState(Boolean isEnabled) {
            itemNameTextBox.Enabled = isEnabled;
            itemValueTextBox.Enabled = isEnabled;
            receivableDebtorComboBox.Enabled = isEnabled;
            receivableCreatedDatePicker.Enabled = isEnabled;
            receivableDueDatePicker.Enabled = isEnabled;
        }
    }
}
