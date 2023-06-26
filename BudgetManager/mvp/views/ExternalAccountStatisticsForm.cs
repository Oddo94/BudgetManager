using BudgetManager.mvp.misc;
using BudgetManager.mvp.models;
using BudgetManager.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BudgetManager.mvp.views {
    public partial class ExternalAccountStatisticsForm : Form, IExternalAccountStatisticsView {
        public event EventHandler loadUserAccountsEvent;
        public event EventHandler displayAccountStatisticsEvent;
        public event EventHandler displayAccountTransfersEvent;
        public event EventHandler displayAccountTransfersActivityEvent;
        public event EventHandler displayAccountBalanceMonthlyEvolutionEvent;

        public int userId;
        public String accountName;
        public String startDate;
        public String endDate;
        public int transfersActivityYear;
        public int monthlyAccountBalanceYear;

        //TEST ONLY
        BindingSource mockBindingSource;
        public List<int> inTransferValues = new List<int>() { 10, 100, 500, 458, 620, 700, 511, 300, 29, 100, 444, 777 };
        public List<int> outTransferValues = new List<int>() { 400, 500, 433, 879, 230, 378, 289, 600, 80, 145, 766, 210 };

        public ExternalAccountStatisticsForm(int userId) {
            this.userId = userId;
            InitializeComponent();
            associateAndRaiseEvents();
            userAccountsComboBox.SelectedIndex = -1;
        }

        int IExternalAccountStatisticsView.userId { get => userId; set => this.userId = value; }
        string IExternalAccountStatisticsView.accountName { get => this.userAccountsComboBox.Text; set => this.accountName = this.userAccountsComboBox.Text; }
        String IExternalAccountStatisticsView.startDate { get => startDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); set => this.startDate = startDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); }
        String IExternalAccountStatisticsView.endDate { get => endDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); set => this.endDate = endDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); }
        int IExternalAccountStatisticsView.transfersActivityYear { get => transfersActivityDateTimePicker.Value.Year; set => this.transfersActivityYear = transfersActivityDateTimePicker.Value.Year; }
        int IExternalAccountStatisticsView.monthlyAccountBalanceYear { get => monthlyBalanceDateTimePicker.Value.Year; set => this.monthlyAccountBalanceYear = monthlyBalanceDateTimePicker.Value.Year; }

        public void setControlsBindingSource(BindingSource userAccountsBindingSource, BindingSource accountStatisticsBindingSource, BindingSource accountTransfersBindingSource, BindingSource accountTransfersActivityBindingSource, BindingSource monthlyAccountBalanceBindingSource) {

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

            mockBindingSource = new BindingSource();

            accountTransfersActivityChart.DataSource = accountTransfersActivityBindingSource;
            monthlyAccountBalanceChart.DataSource = monthlyAccountBalanceBindingSource;
        }

        private void associateAndRaiseEvents() {
            ExternalAccountEventArgs eventArgs = new ExternalAccountEventArgs.Builder(userId)
                .addAccountName(accountName)
                .build();

            this.Shown += delegate { loadUserAccountsEvent?.Invoke(this, eventArgs); };

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
                { "Out", Color.Tomato }
            };

                int columnIndex = 4;

                DataGridViewManager dgvManager = new DataGridViewManager(this.accountTransfersDgv);
                dgvManager.highlightRowsBasedOnCondition(valueToColorDictionary, columnIndex);
            }
        }

        private void accountTransfersDgv_DataSourceChanged(object sender, EventArgs e) {
            
        }

        private void refreshAccountTransfersActivityChart() {
            if(accountTransfersActivityChart != null) {
                accountTransfersActivityChart.DataBind();
            }
            
        }

        private void monthlyTransferEvolutionDisplayButton_Click(object sender, EventArgs e) {
            //isTransferActivityEvent = true;
            displayAccountTransfersActivityEvent?.Invoke(this, EventArgs.Empty);

            accountTransfersActivityChart.DataBind();

            //isTransferActivityEvent = false;



            //DataTable mockDT = new DataTable();
            //mockDT.Columns.Add("Month");
            //mockDT.Columns.Add("Total in transfers");
            //mockDT.Columns.Add("Total out transfers");

            //mockDT.Rows.Add("Jan", 500, 100);
            //mockDT.Rows.Add("Feb", 0, 0);
            //mockDT.Rows.Add("Mar", 500, 100);
            //mockDT.Rows.Add("Apr", 500, 100);
            //mockDT.Rows.Add("May", 0, 0);
            //mockDT.Rows.Add("Jun", 500, 100);
            //mockDT.Rows.Add("Jul", 0, 0);
            //mockDT.Rows.Add("Aug", 500, 100);
            //mockDT.Rows.Add("Sep", 500, 100);
            //mockDT.Rows.Add("Oct", 500, 100);
            //mockDT.Rows.Add("Nov", 0, 0);
            //mockDT.Rows.Add("Dec", 500, 100);

            //mockBindingSource.DataSource = mockDT;
            //accountTransfersActivityChart.DataBind();

            // DialogResult userOption =  MessageBox.Show("Do you want to change the displayed data?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (userOption == DialogResult.Yes) {
            //    mockDT.Rows.Clear();
            //    mockDT.Rows.Add("Jan", 500, 100);
            //    mockDT.Rows.Add("Feb", 970, 222);
            //    mockDT.Rows.Add("Mar", 411, 100);
            //    mockDT.Rows.Add("Apr", 550, 390);
            //    mockDT.Rows.Add("May", 877, 23);
            //    mockDT.Rows.Add("Jun", 599, 665);
            //    mockDT.Rows.Add("Jul", 111, 388);
            //    mockDT.Rows.Add("Aug", 412, 907);
            //    mockDT.Rows.Add("Sep", 675, 180);
            //    mockDT.Rows.Add("Oct", 999, 1500);
            //    mockDT.Rows.Add("Nov", 89, 45);
            //    mockDT.Rows.Add("Dec", 710, 333);

            //    accountTransfersActivityChart.DataBind();
            //}
            //accountTransferActivityChart.DataSource = mockDT;

            //BindingSource chartBinding = (BindingSource) accountTransferActivityChart.DataBindings[0];
            //DataTable testDT = (DataTable)chartBinding.DataSource;

            //accountTransferActivityChart.DataSource = testDT;


            //List<String> monthsList = new List<String>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };


            //accountTransferActivityChart.Series["IN transfers"].Points.DataBindXY(monthsList, inTransferValues);
            //accountTransferActivityChart.Series["OUT transfers"].Points.DataBindXY(monthsList, outTransferValues);
        }

        private void accountBalanceEvolutionDisplayButton_Click(object sender, EventArgs e) {
            displayAccountBalanceMonthlyEvolutionEvent?.Invoke(this, EventArgs.Empty);

           monthlyAccountBalanceChart.DataBind();
        }
    }
}
