using System;
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

        private TextBox receivableNameTextBox;
        private TextBox receivableValueTextBox;
        private ComboBox receivableDebtorComboBox;
        private DateTimePicker receivableCreatedDatePicker;
        private DateTimePicker receivableDueDatePicker;
        private Button updateDgvRecordButton;
        private Label receivableNameLabel;
        private Label receivableValueLabel;
        private Label receivableDebtorCbxLabel;
        private Label receivableCreatedDateLabel;
        private Label receivableDueDateLabel;

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
                    receivablesManagementPanel.Visible = false;
                    break;

                case "partialPaymentItem":              
                    message = String.Format(template, "'Partial payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    MessageBox.Show(message, messageBoxTitle);
                    receivablesManagementPanel.Visible = false;
                    break;

                case "updateDetailsItem":
                    message = String.Format(template, "'Update payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    MessageBox.Show(message, messageBoxTitle);
                    addControlsToMainPanel();
                    receivablesManagementPanel.Visible = true;
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

        //COMPONENTS CREATION SECTION
        private void createTextBoxes() {
            receivableNameTextBox = new TextBox();
            receivableNameTextBox.Width = 200;
            receivableNameTextBox.Margin = new Padding(0, 0, 0, 0);

            receivableValueTextBox = new TextBox();
            receivableValueTextBox.Width = 200;
            receivableValueTextBox.Margin = new Padding(0, 0, 0, 0);
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
        }

        private void createButtons() {
            updateDgvRecordButton = new Button();
            updateDgvRecordButton.Size = new Size(99, 23);
            updateDgvRecordButton.Margin = new Padding(0, 20, 0, 0);
            updateDgvRecordButton.Text = "Update record";
            updateDgvRecordButton.Enabled = false;
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

        }

        private void addControlsToMainPanel() {
            receivablesManagementPanel.Controls.Add(receivableNameLabel);
            receivablesManagementPanel.Controls.Add(receivableNameTextBox);

            receivablesManagementPanel.Controls.Add(receivableValueLabel);
            receivablesManagementPanel.Controls.Add(receivableValueTextBox);

            receivablesManagementPanel.Controls.Add(receivableDebtorCbxLabel);
            receivablesManagementPanel.Controls.Add(receivableDebtorComboBox);

            receivablesManagementPanel.Controls.Add(receivableCreatedDateLabel);
            receivablesManagementPanel.Controls.Add(receivableCreatedDatePicker);

            receivablesManagementPanel.Controls.Add(receivableDueDateLabel);
            receivablesManagementPanel.Controls.Add(receivableDueDatePicker);

            receivablesManagementPanel.Controls.Add(updateDgvRecordButton);
        }
    }
}
