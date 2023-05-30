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

namespace BudgetManager.mvp.views
{
    public partial class ExternalAccountStatisticsForm : Form
    {
        public ExternalAccountStatisticsForm()
        {
            InitializeComponent();
        }

        private void testBindingButton_Click(object sender, EventArgs e)
        {
            ExternalAccountDetailsModel accountDetails = new ExternalAccountDetailsModel("Test Razvan", "Banca Romaneasca", DateTime.Now, 10000, 4000, 3000, 0, 999);

            this.externalAccountDetailsModelBindingSource.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource1.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource2.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource3.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource4.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource5.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource6.DataSource = accountDetails;
            this.externalAccountDetailsModelBindingSource7.DataSource = accountDetails;
            
        }
    }
}
