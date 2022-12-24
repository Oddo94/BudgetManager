using BudgetManager.utils;
using BudgetManager.utils.data_insertion;
using BudgetManager.utils.enums;
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

namespace BudgetManager.mvc.views {
    public partial class ReceivableManagementForm : Form {
        private int userID;
        private int rowIndexOnRightClick;
        private int columnIndexOnRightClick;

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

        public ReceivableManagementForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            controlsManager = new UserControlsManager();
            currentLayout = UIContainerLayout.UNDEFINED;

            fillDataGridView();        
            createTextBoxes();
            createComboBoxes();
            createDateTimePickers();
            createButtons();
            createLabels();
            fillComboBoxes();
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

            sourceDataTable.Rows.Add(1, "Receivable January 2022", 100, "Jim", 0, 1, "2022-01-30", "2022-12-30");
            sourceDataTable.Rows.Add(2, "Receivable April 2022", 500, "John", 0, 1, "2022-04-30", "2022-12-30");
            sourceDataTable.Rows.Add(3, "Receivable May 2022", 100, "Adam", 0, 1, "2022-05-30", "2022-12-30");
            sourceDataTable.Rows.Add(4, "Receivable September 2022", 1000, "Mike", 300, 1, "2022-09-30", "2022-12-30");

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
            e.ContextMenuStrip = updateReceivableCtxMenu;
            updateReceivableCtxMenu.Visible = true;
        }

        private void monthRecordsRadioButton_CheckedChanged(object sender, EventArgs e) {
            receivableManagemenDatePicker.CustomFormat = "MM/yyyy";
        }

        private void yearRecordsRadioButton_CheckedChanged(object sender, EventArgs e) {
            receivableManagemenDatePicker.CustomFormat = "yyyy";
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
                    ArrayList currentRowData = retrieveDataFromSelectedRow(rowIndexOnRightClick, receivableManagementDgv);
                    try {
                        populateFormFields(currentRowData);
                    } catch(FormatException ex) {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Invalid date format for the receivable due/created date! Unable to populate the update data form.", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } finally {
                        updateDgvRecordButton.Enabled = false;
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

        private void updateDgvRecordButton_Click(object sender, EventArgs e) {
            DateTime createdDate = receivableCreatedDatePicker.Value;
            DateTime dueDate = receivableDueDatePicker.Value;

            if(createdDate > dueDate || dueDate < createdDate) {
                MessageBox.Show("Invalid date selection! The receivable creation date must precede the due date!", "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int nameColumnIndex = 1;
            int valueColumnIndex = 2;
            int debtorColumnIndex = 3;
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

            try {
                DataTable receivableDgvDataSource = (DataTable)receivableManagementDgv.DataSource;
                UserControlsManager.updateDataTable(receivableDgvDataSource, rowIndexOnRightClick, cellIndexValueDictionary);

            } catch(Exception ex) {
                string errorMessage = string.Format("Unable to update the specified row! Reason: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Receivable management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                rowIndexOnRightClick = -1;
            }

        }

        private void ReceivableManagementForm_Load(object sender, EventArgs e) {
            itemNameTextBox.TextChanged += new EventHandler(itemNameTextBox_TextChanged);
            itemValueTextBox.TextChanged += new EventHandler(itemValueTextBox_TextChanged);
            receivableDebtorComboBox.TextChanged += new EventHandler(receivableDebtorComboBox_TextChanged);
            receivableCreatedDatePicker.TextChanged += new EventHandler(receivableCreatedDatePicker_TextChanged);
            receivableDueDatePicker.TextChanged += new EventHandler(receivableDueDatePicker_TextChanged);
            updateDgvRecordButton.Click += new EventHandler(updateDgvRecordButton_Click);

        }

        private void exitButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e) {
            Button currentlyActiveButton = null;
            
            //Sets the correct button as the currently active one based on the layout.This will ensure that the right button is enabled/disabled when form field data is present/missing
            if(currentLayout == UIContainerLayout.INSERT_LAYOUT) {
                currentlyActiveButton = insertPartialPaymentButton;
            } else if(currentLayout == UIContainerLayout.UPDATE_LAYOUT) {
                currentlyActiveButton = updateDgvRecordButton;
            } 
            //Enables/or disables the update receivable button based on the presence/absence of data on the required form fields
            UserControlsManager.setButtonState(currentlyActiveButton, activeControls);
        }

        private void itemValueTextBox_TextChanged(object sender, EventArgs e) {
            Regex forbiddenCharacters = new Regex("[^0-9]+", RegexOptions.Compiled);

            //Checks if the value text box contains non-digit characters and in that case it clears its content
            if (forbiddenCharacters.IsMatch(itemValueTextBox.Text)) {
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
            UserControlsManager.setButtonState(currentlyActiveButton, activeControls);  
        }

        private void receivableDebtorComboBox_TextChanged(object sender, EventArgs e) {
            UserControlsManager.setButtonState(updateDgvRecordButton, activeControls);
        }

        private void receivableCreatedDatePicker_TextChanged(object sender, EventArgs e) {
            UserControlsManager.setButtonState(updateDgvRecordButton, activeControls);
        }

        private void receivableDueDatePicker_TextChanged(object sender, EventArgs e) {
            UserControlsManager.setButtonState(updateDgvRecordButton, activeControls);
        }

        //private void itemValueTextBox_TextChanged(object sender, EventArgs e) {
        //    Regex forbiddenCharacters = new Regex("[^0-9]+", RegexOptions.Compiled);

        //    //Checks if the value text box contains non-digit characters and in that case it clears its content
        //    if(forbiddenCharacters.IsMatch(itemValueTextBox.Text)) {
        //        itemValueTextBox.Text = "";
        //    }
        //}


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
            receivableCreatedDatePicker.Margin = new Padding(0, 0, 0, 0);

            receivableDueDatePicker = new DateTimePicker();
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

        //Method that retreves data from the selected row when the user selects the "Update data" option
        private ArrayList retrieveDataFromSelectedRow(int selectedRowIndex, DataGridView targetDataGridView) {
            Guard.notNull(targetDataGridView, "receivable management grid view", "Unable to retrieve data from a null object.");

            //Checks that the row index is within bounds
            if (selectedRowIndex < 0 || selectedRowIndex > targetDataGridView.Rows.Count) {
                return new ArrayList();
            }

            String receivableName = targetDataGridView.Rows[selectedRowIndex].Cells[1].Value.ToString();
            String receivableValue = targetDataGridView.Rows[selectedRowIndex].Cells[2].Value.ToString();
            String receivableDebtorName = targetDataGridView.Rows[selectedRowIndex].Cells[3].Value.ToString();
            String createdDate = targetDataGridView.Rows[selectedRowIndex].Cells[6].Value.ToString();
            String dueDate = targetDataGridView.Rows[selectedRowIndex].Cells[7].Value.ToString();

            ArrayList selectedRowData = new ArrayList() { receivableName, receivableValue, receivableDebtorName, createdDate, dueDate };

            return selectedRowData;
        }

        //Method that populates the update data form fields with the data from the currently selected row when the user selects the "Update data" option
        private void populateFormFields(ArrayList dataSource) {
            Guard.notNull(dataSource, "form fields data source", "Unable to populate the form fields because the data source is null!");

            int requiredElements = 5;

            if (dataSource.Count < requiredElements) {
                return;
            }

            itemNameTextBox.Text = dataSource[0].ToString();
            itemValueTextBox.Text = dataSource[1].ToString();
            receivableDebtorComboBox.Text = dataSource[2].ToString();

            DateTime createdDate;
            DateTime dueDate;

            bool canParseCreatedDate = DateTime.TryParse(dataSource[3].ToString(), out createdDate);
            bool canParseDueDate = DateTime.TryParse(dataSource[4].ToString(), out dueDate);

            if (canParseCreatedDate && canParseDueDate) {
                receivableCreatedDatePicker.Value = createdDate;
                receivableDueDatePicker.Value = dueDate;
            } else {
                String illegalFormatString = canParseCreatedDate == false ? dataSource[3].ToString() : dataSource[4].ToString();
                throw new FormatException(String.Format("Invalid format for date string: {0}", illegalFormatString));
            }

        }
    }
}
