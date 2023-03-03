using BudgetManager.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.non_mvc {
    public partial class DiscardReceivableChangeForm : Form {
        private DataTable receivableManagementDT;
        private String pendingChangesInfoMessage;
        private CheckBox headerCheckBox;

        public DiscardReceivableChangeForm(DataTable receivableManagementDT) {          
            InitializeComponent();
            this.receivableManagementDT = receivableManagementDT;
            this.pendingChangesInfoMessage = "You have {0} pending change{1}";
            this.headerCheckBox = new CheckBox();
            this.headerCheckBox.CheckStateChanged += new EventHandler(headerCheckBox_CheckStateChanged);


            setupDiscardChangesDisplay(receivableManagementDT);
        }

        private void setupDiscardChangesDisplay(DataTable receivableManagementDT) {
            Guard.notNull(receivableManagementDT, "The data table containing the changes performed on the receivables cannot be null!");

            DataTable changesToDiscardDT = receivableManagementDT.GetChanges();

            //Populates the DataGridView object with data from the DB
            receivableChangesToDiscardDgv.DataSource = changesToDiscardDT;

            //Adding boolean column to DGV
            DataGridViewCheckBoxColumn discardChangeCheckBoxColumn = new DataGridViewCheckBoxColumn();
            discardChangeCheckBoxColumn.ValueType = typeof(bool);
   

           
            Point headerCellLocation = this.receivableChangesToDiscardDgv.GetCellDisplayRectangle(0, -1, true).Location;
            headerCheckBox.Location = new Point(headerCellLocation.X + 85, headerCellLocation.Y + 10);
            headerCheckBox.BackColor = Color.White;
            headerCheckBox.Size = new Size(18, 18);

            receivableChangesToDiscardDgv.Controls.Add(headerCheckBox);

            

            receivableChangesToDiscardDgv.Columns.Insert(0, discardChangeCheckBoxColumn);

            //receivableChangesToDiscardDgv.Columns[0].ReadOnly = false;
            foreach (DataGridViewColumn currentColumn in receivableChangesToDiscardDgv.Columns) {
                int currentColumnIndex = currentColumn.Index;
                if(currentColumnIndex == 0) {
                    currentColumn.ReadOnly = false;
                    continue;
                }

                currentColumn.ReadOnly = true;
            }
            

            int currentPendingChanges = changesToDiscardDT.Rows.Count;

            pendingChangesInfoLabel.Text = String.Format(pendingChangesInfoMessage, currentPendingChanges, currentPendingChanges > 1 ? "s": "");

            
        }

        private void backButton_Click(object sender, EventArgs e) {
            Console.WriteLine("BEFORE REMOVING CHANGES: " + receivableManagementDT.GetChanges().Rows.Count + " changes");

            receivableManagementDT.Rows[1].RejectChanges();

            Console.WriteLine("AFTER REMOVING CHANGES: " + receivableManagementDT.GetChanges().Rows.Count + " changes");
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void headerCheckBox_CheckStateChanged(object sender, EventArgs e) {

            if(headerCheckBox.Checked == true) {
                setDiscardCheckBoxColumnState(receivableChangesToDiscardDgv, 0, true);
            } else {
                setDiscardCheckBoxColumnState(receivableChangesToDiscardDgv, 0, false);
            }
        }

        private void setDiscardCheckBoxColumnState(DataGridView inputDataGridView, int checkBoxColumnIndex, bool state) {
            Guard.notNull(inputDataGridView, "The DataGridView containing the checkbox column whose state needs to be changed cannot be null!");

            foreach(DataGridViewRow currentRow in inputDataGridView.Rows) {
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)currentRow.Cells[checkBoxColumnIndex];
                checkBoxCell.Value = state;
            }
        }
    }
}
