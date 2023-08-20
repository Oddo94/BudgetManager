using BudgetManager.mvp.misc;
using BudgetManager.mvp.models;
using BudgetManager.utils;
using BudgetManager.utils.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BudgetManager.mvp.views {
    /*NOTE-MVC FLOW DESCRIPTION
     This implementation of the MVC pattern uses events and binding sources for transferring data between the MODEL, VIEW and PRESENTER
     Each DataGridView present in the GUI has its DataSource set to a BindingSource object. Whenever the user clicks on a button to display data the appropriate event is raised.
     The event is intercepted by the PRESENTER which uses the REPOSITORY's exposed methods to retrieve the appropriate data. 
     The PRESENTER also uses the public fields of the VIEW to get the data that needs to be used for querying the database and sends it to the REPOSITORY.
     Once the data is retrieved as a DataTable object it is set as the DataSource of the BindingSource mentioned earlier. As a result, whenever the DataSource changes the DataGridView from the GUI will be updated automatically
    */
    public partial class ExternalAccountStatisticsForm : Form, IExternalAccountStatisticsView {
        //Events triggered when the user requests data
        public event EventHandler loadUserAccountsEvent;
        public event EventHandler displayAccountStatisticsEvent;
        public event EventHandler displayAccountTransfersEvent;
        public event EventHandler displayAccountInterestsEvent;
        public event EventHandler displayAccountTransfersActivityEvent;
        public event EventHandler displayAccountBalanceMonthlyEvolutionEvent;

        //Fields used to store the values that will be used when querying the database
        public int userId;
        public String accountName;
        public String startDate;
        public String endDate;
        public int transfersActivityYear;
        public int monthlyAccountBalanceYear;
        public BudgetItemType selectedItemType;

        public ExternalAccountStatisticsForm(int userId) {
            this.userId = userId;
            InitializeComponent();
            associateAndRaiseEvents();
            userAccountsComboBox.SelectedItem = null;
            selectedItemType = BudgetItemType.ACCOUNT_TRANSFER;//The transfers item type will always be selected by default
        }

        
        int IExternalAccountStatisticsView.userId { get => userId; set => this.userId = value; }
        string IExternalAccountStatisticsView.accountName { get => this.userAccountsComboBox.Text; set => this.accountName = this.userAccountsComboBox.Text; }
        String IExternalAccountStatisticsView.startDate { get => startDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); set => this.startDate = startDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); }
        String IExternalAccountStatisticsView.endDate { get => endDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); set => this.endDate = endDateTransfersDTPicker.Value.Date.ToString("yyyy-MM-dd"); }
        int IExternalAccountStatisticsView.transfersActivityYear { get => transfersActivityDateTimePicker.Value.Year; set => this.transfersActivityYear = transfersActivityDateTimePicker.Value.Year; }
        int IExternalAccountStatisticsView.monthlyAccountBalanceYear { get => monthlyBalanceDateTimePicker.Value.Year; set => this.monthlyAccountBalanceYear = monthlyBalanceDateTimePicker.Value.Year; }
        BudgetItemType IExternalAccountStatisticsView.selectedItemType { get => this.selectedItemType; }

        public void setControlsBindingSource(BindingSource userAccountsBindingSource, BindingSource accountStatisticsBindingSource, BindingSource accountTransfersOrInterestsBindingSource, BindingSource accountTransfersActivityBindingSource, BindingSource monthlyAccountBalanceBindingSource) {

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

            accountTransfersOrInterestsDgv.DataSource = accountTransfersOrInterestsBindingSource;
            accountTransfersActivityChart.DataSource = accountTransfersActivityBindingSource;
            monthlyAccountBalanceChart.DataSource = monthlyAccountBalanceBindingSource;
        }

        private void associateAndRaiseEvents() {
            ExternalAccountEventArgs eventArgs = new ExternalAccountEventArgs.Builder(userId)
                .addAccountName(accountName)
                .build();

            this.Shown += delegate { loadUserAccountsEvent?.Invoke(this, eventArgs); };

            this.userAccountsComboBox.SelectedValueChanged += delegate { displayAccountStatisticsEvent?.Invoke(this, EventArgs.Empty); };
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
                displayAccountTransfersOrInterestsButton.Enabled = true;
            } else {
                displayAccountTransfersOrInterestsButton.Enabled = false;
            }
        }

        private void endDateTransfersDTPicker_ValueChanged(object sender, EventArgs e) {
            if (UserControlsManager.isValidDateSelection(startDateTransfersDTPicker, endDateTransfersDTPicker)) {
                displayAccountTransfersOrInterestsButton.Enabled = true;
            } else {
                displayAccountTransfersOrInterestsButton.Enabled = false;
            }
        }

        private void accountTransfersOrInterestsDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            //Cell formatting logic for transfers display
            if (radioButtonTransfers.Checked == true) {
                if (accountTransfersOrInterestsDgv.Rows.Count >= 1) {
                    Dictionary<String, Color> valueToColorDictionary = new Dictionary<String, Color> {
                { "In", Color.GreenYellow },
                { "Out", Color.Tomato }
            };

                    int columnIndex = 4;

                    DataGridViewManager dgvManager = new DataGridViewManager(this.accountTransfersOrInterestsDgv);
                    dgvManager.highlightRowsBasedOnCondition(valueToColorDictionary, columnIndex);
                }
            } else if (radioButtonInterests.Checked == true) {
                //Cell formatting logic for interests display
                for (int i = 0; i < accountTransfersOrInterestsDgv.Rows.Count - 1; i++) {
                    DataGridViewRow currentRow = accountTransfersOrInterestsDgv.Rows[i];
                    currentRow.DefaultCellStyle.BackColor = Color.GreenYellow;
                }
            }
        }

        private void refreshAccountTransfersActivityChart() {
            if (accountTransfersActivityChart != null) {
                accountTransfersActivityChart.DataBind();
            }

        }

        private void monthlyTransferEvolutionDisplayButton_Click(object sender, EventArgs e) {
            displayAccountTransfersActivityEvent?.Invoke(this, EventArgs.Empty);
            accountTransfersActivityChart.DataBind();

            //Logic for displaying an error message when no data is found for the specified year
            DataTable currentDataSource = (DataTable) ((BindingSource) accountTransfersActivityChart.DataSource).DataSource;

            bool hasZeroBalanceForAllMonths = true;

            foreach (DataRow currentRow in currentDataSource.Rows) {
                double totalInTransfersForCurrentMonth = 0;
                double totalOutTransfersForCurrentMonth = 0;

                bool canParseInTransfers = Double.TryParse(currentRow.ItemArray[1].ToString(), out totalInTransfersForCurrentMonth);
                bool canParseOutTransfers = Double.TryParse(currentRow.ItemArray[2].ToString(), out totalOutTransfersForCurrentMonth);

                //If the IN transfers for at least one of the twelve months is greater/lower than zero the flag will be set to false and the message won't be displayed
                if (totalInTransfersForCurrentMonth > 0 || totalInTransfersForCurrentMonth < 0) {
                    hasZeroBalanceForAllMonths = false;
                    break;
                }

                //If the OUT transfers for at least one of the twelve months is greater/lower than zero the flag will be set to false and the message won't be displayed
                if (totalOutTransfersForCurrentMonth > 0 || totalOutTransfersForCurrentMonth < 0) {
                    hasZeroBalanceForAllMonths = false;
                    break;
                }
            }

            if (hasZeroBalanceForAllMonths) {
                MessageBox.Show("No data found for the selected year!", "External account statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void accountBalanceEvolutionDisplayButton_Click(object sender, EventArgs e) {
            displayAccountBalanceMonthlyEvolutionEvent?.Invoke(this, EventArgs.Empty);

            monthlyAccountBalanceChart.DataBind();

            //Logic for displaying an error message when no data is found for the specified year
            DataTable currentDataSource = (DataTable) ((BindingSource) monthlyAccountBalanceChart.DataSource).DataSource;

            bool hasZeroBalanceForAllMonths = true;

            foreach(DataRow currentRow in currentDataSource.Rows) {
                double currentMonthBalance = 0;
                bool canParse = Double.TryParse(currentRow.ItemArray[2].ToString(), out currentMonthBalance);

                //If the balance for at least one of the twelve months is greater/lower than 0 the flag will be set to false and the message will not be displayed
                if(currentMonthBalance > 0 || currentMonthBalance < 0) {
                    hasZeroBalanceForAllMonths = false;
                    break;
                }
            }

            if (hasZeroBalanceForAllMonths) {
                MessageBox.Show("No data found for the selected year!", "External account statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displayAccountTransfersOrInterestsButton_Click(object sender, EventArgs e) {
            if (radioButtonTransfers.Checked) {
                displayAccountTransfersEvent?.Invoke(this, EventArgs.Empty);
                //Console.WriteLine("Successfully fired account transfers event!");
            } else if (radioButtonInterests.Checked) {
                displayAccountInterestsEvent?.Invoke(this, EventArgs.Empty);
                //Console.WriteLine("Successfully fired account interest event!");
            }

            //Logic for displaying an error message when no data is found for the specified time interval
            DataTable currentDataSource = (DataTable) ((BindingSource) accountTransfersOrInterestsDgv.DataSource).DataSource;
            int retrievedRows = currentDataSource.Rows.Count;

            if(retrievedRows <= 0) {
                MessageBox.Show("No data found for the specified time interval!", "External account statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonTransfers_Click(object sender, EventArgs e) {
            displayAccountTransfersOrInterestsButton.Text = "Show transfers";
            selectedItemType = BudgetItemType.ACCOUNT_TRANSFER;
            Console.WriteLine("SELECTED BUDGET ITEM TYPE: {0}", selectedItemType.ToString());

        }

        private void radioButtonInterests_Click(object sender, EventArgs e) {
            displayAccountTransfersOrInterestsButton.Text = "Show interests";
            selectedItemType = BudgetItemType.SAVING_ACCOUNT_INTEREST;
            Console.WriteLine("SELECTED BUDGET ITEM TYPE: {0}", selectedItemType.ToString());
        }

        private void accountTransfersActivityChart_MouseHover(object sender, EventArgs e) {
            accountTransfersActivityChart.Series[0].XValueType = ChartValueType.String;
            accountTransfersActivityChart.Series[0].ToolTip = "Total IN transfers for #VALX: #VALY";

            accountTransfersActivityChart.Series[1].XValueType = ChartValueType.String;
            accountTransfersActivityChart.Series[1].ToolTip = "Total OUT transfers for #VALX: #VALY";
        }

        private void monthlyAccountBalanceChart_MouseHover(object sender, EventArgs e) {
            monthlyAccountBalanceChart.Series[0].XValueType = ChartValueType.String;
            monthlyAccountBalanceChart.Series[0].ToolTip = "#VALX balance: #VALY";
        }
    }
}
