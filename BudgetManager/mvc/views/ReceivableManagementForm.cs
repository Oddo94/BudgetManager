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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.mvc.views {
    public partial class ReceivableManagementForm : Form {
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





        private ArrayList activeControls;

        public ReceivableManagementForm(int userID) {
            InitializeComponent();
            fillDataGridView();
            createTextBoxes();
            createComboBoxes();
            createDateTimePickers();
            createButtons();
            createLabels();
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

            sourceDataTable.Rows.Add(1, "Receivable January 2022", 100, 2, 0, 1, "2022-01-30", "2022-12-30");
            sourceDataTable.Rows.Add(2, "Receivable April 2022", 500, 3, 0, 1, "2022-04-30", "2022-12-30");
            sourceDataTable.Rows.Add(3, "Receivable May 2022", 100, 4, 0, 1, "2022-05-30", "2022-12-30");
            sourceDataTable.Rows.Add(4, "Receivable September 2022", 1000, 4, 300, 1, "2022-09-30", "2022-12-30");

            receivableManagementDgv.DataSource = sourceDataTable;

            DataTable newDataTable = sourceDataTable.Clone();//Will be used to get a copy of the original DataTable onto which the changes will be performed by the user
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
                    MessageBox.Show(message, messageBoxTitle);
                    //receivablesManagementPanel.Visible = false;
                    break;

                case "partialPaymentItem":
                    message = String.Format(template, "'Partial payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    MessageBox.Show(message, messageBoxTitle);
                    //receivablesManagementPanel.Visible = false;
                    setupLayout(UIContainerLayout.INSERT_LAYOUT);
                    break;

                case "updateDetailsItem":
                    message = String.Format(template, "'Update payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    MessageBox.Show(message, messageBoxTitle);
                    //addControlsToMainPanel();
                    //receivablesManagementPanel.Visible = true;
                    setupLayout(UIContainerLayout.UPDATE_LAYOUT);
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
                    activeControls = new ArrayList() { new FormFieldWrapper(itemNameTextBox, true), new FormFieldWrapper(itemValueTextBox, true), new FormFieldWrapper(receivableDebtorComboBox, true), new FormFieldWrapper(receivableCreatedDatePicker, true), new FormFieldWrapper(receivableDueDatePicker, true), new FormFieldWrapper(updateDgvRecordButton, false) };
                    break;

                case UIContainerLayout.INSERT_LAYOUT:
                    activeControls = new ArrayList() { new FormFieldWrapper(itemNameTextBox, true), new FormFieldWrapper(itemValueTextBox, true), new FormFieldWrapper(partialPaymentDatePicker, true), new FormFieldWrapper(insertPartialPaymentButton, false) };
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
            updateDgvRecordButton.Enabled = false;

            insertPartialPaymentButton = new Button();
            insertPartialPaymentButton.Size = new Size(105, 23);
            insertPartialPaymentButton.Margin = new Padding(0, 20, 0, 0);
            insertPartialPaymentButton.Text = "Add payment";
            insertPartialPaymentButton.Enabled = false;

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

        //private void addControlsToMainPanel() {
        //    receivablesManagementPanel.Controls.Add(receivableNameLabel);
        //    receivablesManagementPanel.Controls.Add(itemNameTextBox);

        //    receivablesManagementPanel.Controls.Add(receivableValueLabel);
        //    receivablesManagementPanel.Controls.Add(itemValueTextBox);

        //    receivablesManagementPanel.Controls.Add(receivableDebtorCbxLabel);
        //    receivablesManagementPanel.Controls.Add(receivableDebtorComboBox);

        //    receivablesManagementPanel.Controls.Add(receivableCreatedDateLabel);
        //    receivablesManagementPanel.Controls.Add(receivableCreatedDatePicker);

        //    receivablesManagementPanel.Controls.Add(receivableDueDateLabel);
        //    receivablesManagementPanel.Controls.Add(receivableDueDatePicker);

        //    receivablesManagementPanel.Controls.Add(updateDgvRecordButton);
        //}
    }
}
