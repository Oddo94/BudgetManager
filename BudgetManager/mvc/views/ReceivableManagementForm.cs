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

        public ReceivableManagementForm(int userID) {
            InitializeComponent();
            fillDataGridView();
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
                    break;

                case "partialPaymentItem":              
                    message = String.Format(template, "'Partial payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    MessageBox.Show(message, messageBoxTitle);
                    break;

                case "updateDetailsItem":
                    message = String.Format(template, "'Update payment'", rowIndexOnRightClick, columnIndexOnRightClick);
                    MessageBox.Show(message, messageBoxTitle);
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
    }
}
