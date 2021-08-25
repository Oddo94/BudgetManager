using BudgetManager.mvc.controllers;
using BudgetManager.mvc.models;
using BudgetManager.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BudgetManager.mvc.views {
    public partial class SavingAccountForm : Form, IView {
        private int userID;
        private IControl controller = new SavingAccountController();
        private IModel model = new SavingAccountModel();

        private DateTimePicker[] datePickers = new DateTimePicker[] { };
        private DataGridViewManager gridViewManager;
        private bool hasResetDatePickers = false;

        public SavingAccountForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            datePickers = new DateTimePicker[] { dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount, dateTimePickerMonthlyBalance};
            gridViewManager = new DataGridViewManager(dataGridViewSavingAccount);                 
            wireUp(controller, model);
            dateTimePickerStartSavingAccount.Enabled = false;
        }


        //GENERAL MVC METHODS
        public void disableControls() {
            foreach (DateTimePicker currentPicker in datePickers) {
                currentPicker.Enabled = false;
            }

            refreshBalanceDataButton.Enabled = false;
            savingAccountComboBox.Enabled = false;
        }

        public void enableControls() {
            foreach (DateTimePicker currentPicker in datePickers) {
                currentPicker.Enabled = true;
            }

            refreshBalanceDataButton.Enabled = true;
            savingAccountComboBox.Enabled = true;
        }

        public void updateView(IModel model) {           
            String title = "Saving account balance evolution for";
            int currentYear = dateTimePickerMonthlyBalance.Value.Year;

            DataTable[] results = model.DataSources;
            //fillDataGridView(dataGridViewSavingAccount, results[0]);
            //CHANGE-DGW MANAGEMENT
            gridViewManager.fillDataGridView(results[0]);
            fillColumnChart(columnChartMonthlyBalance, results[1], currentYear, title);
            updateAvailableBalanceLabel(savingAccountBalanceLabel, results[2]);           
        }

        public void wireUp(IControl paramController, IModel paramModel) {
            if (model != null) {
                model.removeObserver(this);
            }

            this.model = paramModel;
            this.controller = paramController;

            controller.setView(this);
            controller.setModel(model);

            model.addObserver(this);
        }

        //CONTROLS METHODS
        private void intervalCheckBoxSavingAccount_CheckedChanged(object sender, EventArgs e) {
            setEndPickerPanelVisibility(intervalCheckBoxSavingAccount, monthPickerPanelSavingAccount, startLabelSavingAccount);
        }

        private void dateTimePickerStartSavingAccount_ValueChanged(object sender, EventArgs e) {
            String message = getSelectedTableName(savingAccountComboBox);
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelSavingAccount, dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount, monthPickerPanelSavingAccount);
            }

            sendDataToController(DataUpdateControl.START_PICKER, intervalCheckBoxSavingAccount, dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount);
        }

        private void dateTimePickerEndSavingAccount_ValueChanged(object sender, EventArgs e) {
            String message = getSelectedTableName(savingAccountComboBox);
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelSavingAccount, dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount, monthPickerPanelSavingAccount);
            }

            sendDataToController(DataUpdateControl.END_PICKER, intervalCheckBoxSavingAccount, dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount);
        }

        private void refreshBalanceDataButton_Click(object sender, EventArgs e) {
            sendDataToController(DataUpdateControl.REFRESH_BUTTON, intervalCheckBoxSavingAccount, dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount);
        }

        private void dateTimePickerMonthlyBalance_ValueChanged(object sender, EventArgs e) {            
            sendDataToController(DataUpdateControl.YEARLY_PICKER, intervalCheckBoxSavingAccount, dateTimePickerMonthlyBalance, dateTimePickerEndSavingAccount);
        }

        private void columnChartMonthlyBalance_MouseHover(object sender, EventArgs e) {
            //Displays the current value of the column on which the mouse hovers 
            columnChartMonthlyBalance.Series[0].ToolTip = "Monthly balance: #VALY";
        }

        //UTIL METHODS SECTION
        //private void fillDataGridView(DataGridView gridView, DataTable inputDataTable) {
        //    if(gridView == null) {
        //        return;
        //    }
        //    gridView.DataSource = null;                     
        //    gridView.DataSource = inputDataTable;          
        //}

        private void fillColumnChart(Chart chart, DataTable inputDataTable, int currentYear, String title) {           
            //Eliminates the existing chart points
            chart.Series[0].Points.Clear();

            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {                
                //Creates a dates list(representing the first day of each month of the selected year)
                List<DateTime> dates = new List<DateTime>();
                for (int i = 1; i <= 12; i++) {
                    dates.Add(new DateTime(currentYear, i, 1));
                }
                
                //Adds the names of the months to the chart
                foreach (DateTime currentDate in dates) {
                    chart.Series[0].Points.AddXY(currentDate, 0);
                }

                int j = 0;//the index of the current row from the DataTable object retrieved from the DB
                for (int i = 0; i < chart.Series[0].Points.Count; i++) {                    
                    //If there are no more rows in the DataTable(for example there may be months for which there are no records) the ) value is added to the respectiv month from the chart
                    if (j > inputDataTable.Rows.Count - 1) {
                        chart.Series[0].Points[i].SetValueY(0);
                        continue;
                    }
                   
                    //A DateTime object for the X value of the current chart point is created
                    System.DateTime currentPointDate = System.DateTime.FromOADate(chart.Series[0].Points[i].XValue);                   
                    //The month value is extracted from the DateTime object that was previously created
                    int currentChartMonth = Convert.ToInt32(DateTime.ParseExact(currentPointDate.ToString("MMM"), "MMM", CultureInfo.CurrentCulture).Month);
                    int currentDataTableMonth = Convert.ToInt32(inputDataTable.Rows[j].ItemArray[1]);
                   
                    //A comparison is made to see if the month represented by the current chart point is the same as the one from the DataTable object retrieved from the DB
                    if (currentChartMonth == currentDataTableMonth) {
                        int currentDataTableMonthValue = Convert.ToInt32(inputDataTable.Rows[j].ItemArray[2]);
                        chart.Series[0].Points[i].SetValueY(currentDataTableMonthValue);//if the two values are identical the corresponding value is added to the column chart
                        j++;//the value of the current row index from the DataTable object is incremented
                    } else {
                        chart.Series[0].Points[i].SetValueY(0);//if the values don't match then the 0 value is added to the respective column chart point since there is no correspondence between the DataTable record and the current column chart point                   
                    }
                }
            }
           
            //Setting chart title
            chart.Titles[0].Text = String.Format("{0} for {1}", title, currentYear);
        }

        private void updateAvailableBalanceLabel(Label label, DataTable inputDataTable) {
            if (inputDataTable == null || inputDataTable.Rows.Count <= 0 ) {
                return;
            }

            String availableBalance = inputDataTable.Rows[0].ItemArray[0] != DBNull.Value ? Convert.ToString(inputDataTable.Rows[0].ItemArray[0]) : null;
            label.Text = availableBalance;
        }

        private void setMonthInfoSelectionMessage(String message, Label targetLabel, DateTimePicker startPicker, DateTimePicker endPicker, Panel dateTimePickerContainer) {

            if (dateTimePickerContainer.Visible == true && isValidDateSelection(startPicker, endPicker)) {
                int startMonth = startPicker.Value.Month;
                int endMonth = endPicker.Value.Month;
                int startYear = startPicker.Value.Year;
                int endYear = endPicker.Value.Year;

                if (startYear != endYear) {
                    string startMonthString = startPicker.Value.ToString("MMMM");
                    string endMonthString = endPicker.Value.ToString("MMMM");
                    string startYearString = startPicker.Value.ToString("yyyy");
                    string endYearString = endPicker.Value.ToString("yyyy");

                    targetLabel.Text = String.Format("{0} for {1} {2}-{3} {4}", message, startMonthString, startYearString, endMonthString, endYearString);
                    //return;
                } else {
                    targetLabel.Text = String.Format("{0} for {1}-{2} {3}", message, startPicker.Value.ToString("MMMM"), endPicker.Value.ToString("MMMM"), startPicker.Value.ToString("yyyy"));
                    //return;
                }

            } else if (dateTimePickerContainer.Visible == true && !isValidDateSelection(startPicker, endPicker)) {
                MessageBox.Show("Invalid date selection!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                resetDateTimePicker(startPicker);
                resetDateTimePicker(endPicker);
               
            } else if (dateTimePickerContainer.Visible == false) {
                targetLabel.Text = String.Format("{0} for {1} {2}", message, startPicker.Value.ToString("MMMM"), startPicker.Value.ToString("yyyy"));                  
            }
        }

        private bool isValidDateSelection(DateTimePicker startPicker, DateTimePicker endPicker) {
            int startMonth = startPicker.Value.Month;
            int startYear = startPicker.Value.Year;
            int endMonth = endPicker.Value.Month;
            int endYear = endPicker.Value.Year;

            if (startMonth <= endMonth && startYear <= endYear) {
                return true;
            } else if (startMonth > endMonth && startYear < endYear) {
                return true;
            }

            return false;
        }

        private void resetDateTimePicker(DateTimePicker targetPicker) {
            if (targetPicker == null) {
                return;
            }
            
            //Retrieves the number of days passed from the start of the month
            int daysPassedFromMonthStart = DateTime.Now.Day;
            hasResetDatePickers = true;//sets the flag to true in order to avoid the execution of the method associated to the selected DateTimePicker         
            targetPicker.Value = DateTime.Now.AddDays(-daysPassedFromMonthStart + 1);//subtracts the number of days passed from the beginning of the month and adds one more day(otherwise the first day of the month would be assigned value 0 which is incorrect)

        }
      
        private String getSelectedTableName(ComboBox comboBox) {
            if(comboBox == null) {
                return "";
            }
           
            BudgetItemType itemType = getSelectedBudgetItemType(comboBox);

            switch(itemType) {
                case BudgetItemType.INCOME:
                    return "Incomes";

                case BudgetItemType.GENERAL_EXPENSE:
                    return "General expenses";

                case BudgetItemType.SAVING_ACCOUNT_EXPENSE:
                    return "Saving account expenses";

                case BudgetItemType.DEBT:
                    return "Debts";

                case BudgetItemType.SAVING:
                    return "Savings";

                case BudgetItemType.UNDEFINED:
                    return "";

                default:
                    return "";
            }

        }

        private BudgetItemType getSelectedBudgetItemType(ComboBox comboBox) {           
            String selectedIndexText = comboBox.SelectedItem.ToString();

            switch (selectedIndexText.ToLower()) {
                case "income":
                    return BudgetItemType.INCOME;

                case "general expense":
                    return BudgetItemType.GENERAL_EXPENSE;

                case "saving account expense":
                    return BudgetItemType.SAVING_ACCOUNT_EXPENSE;

                case "debt":
                    return BudgetItemType.DEBT;

                case "saving":
                    return BudgetItemType.SAVING;

                default:
                    return BudgetItemType.UNDEFINED;

            }
        }

        private void setEndPickerPanelVisibility(CheckBox checkbox, Panel panel, Label label) {
            if (checkbox.Checked == true) {
                panel.Visible = true;
                label.Text = "Starting month";
            } else {
                panel.Visible = false;
                label.Text = "Month";
            }
        }
  
        private void sendDataToController(DataUpdateControl updateControl, CheckBox checkBox, DateTimePicker startPicker, DateTimePicker endPicker) {                    
            //Checking the control type whose state was modified
            if (updateControl == DataUpdateControl.START_PICKER) {
                String tableName = getSelectedTableName(savingAccountComboBox);                
                //If the interval checkbox is selected it means that multiple months data is being requested from the DB
                if (checkBox.Checked == true) {                   
                    //Selecting the multiple months option                  
                    QueryType option = QueryType.MULTIPLE_MONTHS;
                  
                    //Retrieving the start and end date in the format used by the MySqL database
                    String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                    String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);
               
                    //Configures the object that is used to store the parameter values for the SQL query and sends it to the controller alongside the type of the query that will be executed         
                    QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).addTableName(tableName).build();

                    controller.requestData(option, paramContainer);

                } else {                    
                    //Otherwise single month data is selected
                    QueryType option = QueryType.SINGLE_MONTH;
                   
                    //Retrieving the month and year values
                    int month = startPicker.Value.Month;
                    int year = startPicker.Value.Year;

                    //Configures the object that is used to store the parameter values for the SQL query and sends it to the controller alongside the type of the query that will be executed                    
                    QueryData paramContainer = new QueryData.Builder(userID).addMonth(month).addYear(year).addTableName(tableName).build();

                    controller.requestData(option, paramContainer);


                }
            } else if (updateControl == DataUpdateControl.END_PICKER) {
                String tableName = getSelectedTableName(savingAccountComboBox);                
                //Selecting multiple months data if the control whose state was modified is the DateTimePicker used to set the end month
                QueryType option = QueryType.MULTIPLE_MONTHS;

                String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);

                QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).addTableName(tableName).build(); //CHANGE
                controller.requestData(option, paramContainer);

            } else if (updateControl == DataUpdateControl.YEARLY_PICKER) {              
                //Selecting the query type option that will sum the selected element values for each month of the selected year.
                QueryType option = QueryType.MONTHLY_TOTALS;
               
                //Just like before the month and year values are retrieved
                int month = startPicker.Value.Month;
                int year = startPicker.Value.Year;

                //Creating the QueryData object and sending the data to the controller           
                QueryData paramContainer = new QueryData.Builder(userID).addMonth(month).addYear(year).build(); //CHANGE
                controller.requestData(option, paramContainer);
            } else if (updateControl == DataUpdateControl.REFRESH_BUTTON) {
                //Current account balance is calculated based on multiple months data
                QueryType option = QueryType.TOTAL_VALUE;

                //Only the userID is needed since the current saving account balance calculation is always made from the first existing balance record to the record of the current month
                QueryData paramContainer = new QueryData.Builder(userID).build();

                controller.requestData(option, paramContainer);
            }
        }

        private String getDateStringInSQLFormat(DateTimePicker datePicker, DateType dateType) {
            if (datePicker == null) {
                return "";
            }
            String sqlFormatDateString = "";          
            //If the end date of the interval is being processed it will be modified so that it will contain the last day of the respective month 
            if (dateType == DateType.END_DATE) {                
                //Retrieves the current date from the DateTimePicker control, adds a month an subtracts a day(in order to obtain the date last of the last day of that month)
                sqlFormatDateString = datePicker.Value.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            } else {
                sqlFormatDateString = datePicker.Value.ToString("yyyy-MM-dd");//if the start date of the interval is being processed then it will be simply retrieved from the DateTimePicker control since it already contains the first day of the month (see the Designer setting)
            }


            return sqlFormatDateString;
        }

        private void savingAccountComboBox_SelectedIndexChanged(object sender, EventArgs e) {           
            int selectedIndex = savingAccountComboBox.SelectedIndex;
            String selectedText = savingAccountComboBox.Text;
            //Console.WriteLine(selectedIndex);
            if (selectedIndex < 0 || "".Equals(selectedText)) {
                intervalCheckBoxSavingAccount.Enabled = false;
                dateTimePickerStartSavingAccount.Enabled = false;               
            } else {
                intervalCheckBoxSavingAccount.Enabled = true;
                dateTimePickerStartSavingAccount.Enabled = true;
            }
        }

      
    }
}
