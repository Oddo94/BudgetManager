using BudgetManager.mvp.misc;
using BudgetManager.mvp.models;
using BudgetManager.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.mvp.views {
    public partial class ExternalAccountStatisticsForm : Form, IExternalAccountStatisticsView {
        public event EventHandler loadUserAccountsEvent;
        public event EventHandler displayAccountStatisticsEvent;
        public event EventHandler displayAccountTransfersEvent;
        public int userId;
        public String accountName;
        public String startDate;
        public String endDate;

        public ExternalAccountStatisticsForm(int userId) {
            this.userId = userId;
            InitializeComponent();
            associateAndRaiseEvents();
            userAccountsComboBox.SelectedIndex = -1;
        }

        int IExternalAccountStatisticsView.userId { get => userId; set => this.userId = value; }
        string IExternalAccountStatisticsView.accountName { get => this.userAccountsComboBox.Text; set => this.accountName = this.userAccountsComboBox.Text; }
        String IExternalAccountStatisticsView.startDate { get => startDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); set => startDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); }
        String IExternalAccountStatisticsView.endDate { get => endDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); set => endDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); }

        public void setControlsBindingSource(BindingSource userAccountsBindingSource, BindingSource accountStatisticsBindingSource, BindingSource accountTransfersBindingSource) {

            userAccountsComboBox.DataSource = userAccountsBindingSource;

            ExternalAccountDetailsModel model = new ExternalAccountDetailsModel("", "", "", DateTime.Now.ToString(), 0, 0, 0, 0, 0);
            accountStatisticsBindingSource.DataSource = model;

            externalAccountDetailsModelBindingSource.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource1.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource2.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource3.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource4.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource5.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource6.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource7.DataSource = accountStatisticsBindingSource;
            externalAccountDetailsModelBindingSource8.DataSource = accountStatisticsBindingSource;

            accountTransfersDgv.DataSource = accountTransfersBindingSource;

        }

        private void associateAndRaiseEvents() {
            ExternalAccountEventArgs eventArgs = new ExternalAccountEventArgs.Builder(userId)
                .addAccountName(accountName)
                .build();

            //userAccountsComboBox.Click += delegate { loadUserAccountsEvent?.Invoke(this, EventArgs.Empty); };
            this.Shown += delegate { loadUserAccountsEvent?.Invoke(this, eventArgs); };

            //displayAccountStatisticsButton.Click += delegate { displayAccountStatisticsEvent?.Invoke(this, eventArgs); };
            this.userAccountsComboBox.SelectedValueChanged += delegate { displayAccountStatisticsEvent?.Invoke(this, EventArgs.Empty); };
            this.displayAccountTransfersButton.Click += delegate { displayAccountTransfersEvent?.Invoke(this, EventArgs.Empty); };
        }

        //Method which raises the event which leads to account transfers retrieval only if the date selection is valid
        private EventHandler fireEventBasedOnCondition() {
            if (UserControlsManager.isValidDateSelection(startDateTransfersDTPicker, endDateTransfersDTPicker)) {
                return delegate { displayAccountTransfersEvent?.Invoke(this, EventArgs.Empty); };
            }

            return null;
        }

        private void startDateTransfersDTPicker_ValueChanged(object sender, EventArgs e) {
            if (UserControlsManager.isValidDateSelection(startDateTransfersDTPicker, endDateTransfersDTPicker)) {
                displayAccountTransfersButton.Enabled = true;
            } else {
                displayAccountTransfersButton.Enabled = false;
            }
        }

        private void endDateTransfersDTPicker_ValueChanged(object sender, EventArgs e) {
            if (UserControlsManager.isValidDateSelection(startDateTransfersDTPicker, endDateTransfersDTPicker)) {
                displayAccountTransfersButton.Enabled = true;
            } else {
                displayAccountTransfersButton.Enabled = false;
            }
        }

        private void accountTransfersDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (accountTransfersDgv.Rows.Count >= 1) {
                Dictionary<String, Color> valueToColorDictionary = new Dictionary<String, Color> {
                { "In", Color.GreenYellow },
                { "Out", Color.Red }
            };

                int columnIndex = 4;

                DataGridViewManager dgvManager = new DataGridViewManager(this.accountTransfersDgv);
                dgvManager.highlightRowsBasedOnCondition(valueToColorDictionary, columnIndex);
            }
        }

        private void accountTransfersDgv_DataSourceChanged(object sender, EventArgs e) {
            
        }
    }
}
