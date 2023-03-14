using BudgetManager.mvc.views;
using BudgetManager.utils;
using BudgetManager.utils.ui_controls;
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

namespace BudgetManager.non_mvc {
    public partial class DiscardReceivableChangeForm : Form {
        private DataTable originalReceivableManagementDT;
        private DataTable copyReceivableManagementDT;
        private ReceivableManagementForm receivableManagementForm;
        private String pendingChangesInfoMessage;
        private CheckBox headerCheckBox;
        private int primaryKeyColumnIndex;
        private int currentPendingChanges;
        private bool hasAlreadySelectedWindowClosing;

        public DiscardReceivableChangeForm(DataTable originalReceivableManagementDT, ReceivableManagementForm receivableManagementForm) {
            InitializeComponent();
            //Sets the original receivable DataTable
            this.originalReceivableManagementDT = originalReceivableManagementDT;
            /*Sets the copy of the original receivable DataTable
             All the changes will be performed on the copied object so that if the user wants to eliminate them he can do so by closing the window or selecting the cancel button
             If the user wants to keep the cages then the copied object will be sent to the receivable management form and will be used for further updates in the database*/
            this.copyReceivableManagementDT = originalReceivableManagementDT.Copy();
            this.pendingChangesInfoMessage = "You have {0} pending change{1}";
            this.headerCheckBox = new CheckBox();
            this.headerCheckBox.CheckStateChanged += new EventHandler(headerCheckBox_CheckStateChanged);
            this.primaryKeyColumnIndex = 0;
            this.receivableManagementForm = receivableManagementForm;

            setupDiscardChangesDisplay(copyReceivableManagementDT);

        }



        private void setupDiscardChangesDisplay(DataTable copyReceivableManagementDT) {
            Guard.notNull(copyReceivableManagementDT, "The data table containing the changes performed on the receivables cannot be null!");

            DataTable changesToDiscardDT = copyReceivableManagementDT.GetChanges();

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
                if (currentColumnIndex == 0) {
                    currentColumn.ReadOnly = false;
                    continue;
                }

                currentColumn.ReadOnly = true;
            }


            int currentPendingChanges = changesToDiscardDT.Rows.Count;

            pendingChangesInfoLabel.Text = String.Format(pendingChangesInfoMessage, currentPendingChanges, currentPendingChanges > 1 ? "s" : "");


        }

        private void backButton_Click(object sender, EventArgs e) {
            //Send the DataTable object onto which the changes were performed to the ReceivableManagementForm so that it will be used for updating the data from the DB
            receivableManagementForm.updateFormAfterDiscardingChanges(copyReceivableManagementDT);
            
            //Sets window closing flag so that the closing message is not showed incorrectly
            hasAlreadySelectedWindowClosing = true;

            this.Dispose();        
        }

        private void DiscardReceivableChangeForm_FormClosing(object sender, FormClosingEventArgs e) {
            //Prevents the confirmation message from being shown incorrectly
            if (hasAlreadySelectedWindowClosing) {
                return;
            }

            DataTable changesToCancelDT = copyReceivableManagementDT.GetChanges();
            

            //If the changesToCancelDT is null it means that the user hasn't discarded any change/s
            if (changesToCancelDT == null) {
                this.Dispose();
                return;
            }

            //Calculates the number of discarded changes
            int initialChangesToDiscard = originalReceivableManagementDT.GetChanges().Rows.Count;
            int remainingChangesToDiscard = copyReceivableManagementDT.GetChanges().Rows.Count;
            int totalChangesToCancel = initialChangesToDiscard - remainingChangesToDiscard;

            if (totalChangesToCancel > 0) {
                String suffix = totalChangesToCancel > 1 ? "s" : "";

                String cancelChangesMessage = String.Format("Are you sure that you want to cancel {0} discarded change{1}? Please use the 'Back' button if you want to discard the selected change{2} and return to the receivable management form.", totalChangesToCancel, suffix, suffix);
                DialogResult userOption = MessageBox.Show(cancelChangesMessage, "Discard receivable changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (userOption == DialogResult.No) {
                    //Cancels the closing event if this parameter is supplied from the calling code
                    e.Cancel = true;
                    hasAlreadySelectedWindowClosing = false;//Restores the window closing flag state
                    return;
                }
            }

            //Sets flag so that the message displayed when the user selects the form closing button is shown only once
            hasAlreadySelectedWindowClosing = true;
            this.Dispose();
        }

        private void headerCheckBox_CheckStateChanged(object sender, EventArgs e) {

            if (headerCheckBox.Checked == true) {
                setDiscardCheckBoxColumnState(receivableChangesToDiscardDgv, 0, true);
            } else {
                setDiscardCheckBoxColumnState(receivableChangesToDiscardDgv, 0, false);
            }
        }

        private void setDiscardCheckBoxColumnState(DataGridView inputDataGridView, int checkBoxColumnIndex, bool state) {
            Guard.notNull(inputDataGridView, "The DataGridView containing the checkbox column whose state needs to be changed cannot be null!");

            foreach (DataGridViewRow currentRow in inputDataGridView.Rows) {
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)currentRow.Cells[checkBoxColumnIndex];
                checkBoxCell.Value = state;
            }

            //Fixes issue with the currently selected checkbox cell not being checked when its underlying "Value" property was changed programatically
            receivableChangesToDiscardDgv.RefreshEdit();
        }

        private void discardChangesButton_Click(object sender, EventArgs e) {
            int selectedChangesToDiscard = getSelectedChangesToDiscard();
            String confirmationMessage = String.Format("Are you sure that you want to discard the selected change{0}?", selectedChangesToDiscard > 1 ? "s" : "");

            DialogResult userOption = MessageBox.Show(confirmationMessage, "Discard receivable changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userOption == DialogResult.No) {
                return;
            }

            List<int> primaryKeyList = getPrimaryKeysForDiscardedChanges();

            DataSourceManager.discardDataTableChanges(copyReceivableManagementDT, primaryKeyList, primaryKeyColumnIndex);

            DataTable newPendingChangesTable = copyReceivableManagementDT.GetChanges();

            if (newPendingChangesTable == null) {
                MessageBox.Show("All changes have been discarded! You will be redirected to the receivable management window.", "Discard receivable changes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Dispose();
                //If all changes have been discarded then the object sent to ReceivableManagementForm will be the DataTable copy object onto which the changes were performed
                receivableManagementForm.updateFormAfterDiscardingChanges(copyReceivableManagementDT);
                return;
            }

            currentPendingChanges = newPendingChangesTable.Rows.Count;
            Console.WriteLine("Current pending changes: " + currentPendingChanges);

            receivableChangesToDiscardDgv.DataSource = newPendingChangesTable;

            pendingChangesInfoLabel.Text = String.Format(pendingChangesInfoMessage, currentPendingChanges, currentPendingChanges > 1 ? "s" : "");
        }


        private List<int> getPrimaryKeysForDiscardedChanges() {
            List<int> primaryKeyList = new List<int>();

            foreach (DataGridViewRow currentRow in receivableChangesToDiscardDgv.Rows) {
                bool isChecked = Convert.ToBoolean(currentRow.Cells[0].EditedFormattedValue);

                //Console.WriteLine(String.Format("Current row index {0} -> Current value: {1}", currentRow.Index, isChecked));             
                if (isChecked) {
                    int rowIndex = currentRow.Index;
                    int receivableID = Convert.ToInt32(currentRow.Cells[1].Value.ToString());
                    primaryKeyList.Add(receivableID);
                }
            }

            return primaryKeyList;
        }

        private int getSelectedChangesToDiscard() {
            int selectedChangesToDiscard = 0;
            foreach (DataGridViewRow currentRow in receivableChangesToDiscardDgv.Rows) {
                bool isChecked = Convert.ToBoolean(currentRow.Cells[0].EditedFormattedValue);

                if (isChecked) {
                    selectedChangesToDiscard++;
                }
            }

            return selectedChangesToDiscard;
        }
    }
}
