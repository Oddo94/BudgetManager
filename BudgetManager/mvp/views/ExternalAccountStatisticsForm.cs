using BudgetManager.mvp.misc;
using BudgetManager.mvp.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.mvp.views {
    public partial class ExternalAccountStatisticsForm : Form, IExternalAccountStatisticsView {
        public event EventHandler loadUserAccountsEvent;
        public event EventHandler displayAccountStatisticsEvent;
        public int userId;
        public String accountName;
        public ExternalAccountStatisticsForm(int userId) {
            this.userId = userId;
            InitializeComponent();
            associateAndRaiseEvents();
            userAccountsComboBox.SelectedIndex = -1;
        }

        int IExternalAccountStatisticsView.userId { get => userId; set => this.userId = value; }
        string IExternalAccountStatisticsView.accountName { get => this.userAccountsComboBox.Text; set => this.accountName = this.userAccountsComboBox.Text; }

        public void setControlsBindingSource(BindingSource userAccountsBindingSource, BindingSource accountStatisticsBindingSource) {

        userAccountsComboBox.DataSource = userAccountsBindingSource;

        ExternalAccountDetailsModel model = new ExternalAccountDetailsModel("", "", DateTime.Now.ToString(), 0, 0, 0, 0, 0);
        accountStatisticsBindingSource.DataSource = model;

        externalAccountDetailsModelBindingSource.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource1.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource2.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource3.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource4.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource5.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource6.DataSource = accountStatisticsBindingSource;
        externalAccountDetailsModelBindingSource7.DataSource = accountStatisticsBindingSource;

    }

    private void associateAndRaiseEvents() {
        ExternalAccountEventArgs eventArgs = new ExternalAccountEventArgs.Builder(userId)
            .addAccountName(accountName)
            .build();

        //userAccountsComboBox.Click += delegate { loadUserAccountsEvent?.Invoke(this, EventArgs.Empty); };
        this.Shown += delegate { loadUserAccountsEvent?.Invoke(this, eventArgs); };

        //displayAccountStatisticsButton.Click += delegate { displayAccountStatisticsEvent?.Invoke(this, eventArgs); };
        userAccountsComboBox.SelectedValueChanged += delegate { displayAccountStatisticsEvent?.Invoke(this, EventArgs.Empty); };
    }

    //private void displayAccountStatisticsButton_Click(object sender, EventArgs e)
    //{
    //    //associateAndRaiseEvents();
    //    //ExternalAccountDetailsModel accountDetails = new ExternalAccountDetailsModel("Test Razvan", "Banca Romaneasca", DateTime.Now, 10000, 4000, 3000, 0, 999);

    //    //this.externalAccountDetailsModelBindingSource.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource1.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource2.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource3.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource4.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource5.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource6.DataSource = accountDetails;
    //    //this.externalAccountDetailsModelBindingSource7.DataSource = accountDetails;

    //}

    //private void externalAccountDetailsModelBindingSource_CurrentChanged(object sender, EventArgs e) {

    //}

}
}
