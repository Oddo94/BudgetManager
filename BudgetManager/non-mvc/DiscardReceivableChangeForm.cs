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

        public DiscardReceivableChangeForm(DataTable receivableManagementDT) {          
            InitializeComponent();
            this.receivableManagementDT = receivableManagementDT;
            this.pendingChangesInfoMessage = "You have {0} pending change{1}";
            setupDiscardChangesDisplay(receivableManagementDT);
        }

        private void setupDiscardChangesDisplay(DataTable receivableManagementDT) {
            Guard.notNull(receivableManagementDT, "The data table containing the changes performed on the receivables cannot be null!");

            DataTable changesToDiscardDT = receivableManagementDT.GetChanges();
            DataColumn discardChangeColumn = new DataColumn("Discard change", typeof(bool));
            discardChangeColumn.DefaultValue = false;
            //discardChangeColumn.ReadOnly = false;          
            changesToDiscardDT.Columns.Add(discardChangeColumn);
            discardChangeColumn.SetOrdinal(0);

            receivableChangesToDiscardDgv.DataSource = changesToDiscardDT;

            //receivableChangesToDiscardDgv.Columns[0].ReadOnly = false;
            foreach(DataGridViewColumn currentColumn in receivableChangesToDiscardDgv.Columns) {
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

        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Dispose();
        }
    }
}
