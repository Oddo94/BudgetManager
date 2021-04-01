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

namespace BudgetManager.mvc.views {
    public partial class BudgetPlanManagementForm : Form, IView{
        private int userID;
        private Button[] buttons;
        private IUpdaterControl controller;
        private IUpdaterModel model;
        private DateTimePicker[] datePickers;
        private int selectedRowIndex;//the variable that holds the value of the DataGridView row that contains the user edited value


        public BudgetPlanManagementForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            this.buttons = new Button[] {submitButtonBPManagement, deleteButtonBPManagement};
            this.datePickers = new DateTimePicker[] { dateTimePickerBPManagement};

            //Sets the default date of the date time picker as the first day of the current month of the current year
            setDateTimePickerDefaultDate(datePickers);

            controller = new BudgetPlanManagementController();
            model = new BudgetPlanManagementModel();

         
            wireUp(controller, model);        
        }

        //CONTROLS METHODS
        private void monthRecordsCheckboxBP_CheckedChanged(object sender, EventArgs e) {
            if (monthRecordsCheckboxBP.Checked == true) {
                yearRecordsCheckboxBP.Checked = false;
                yearRecordsCheckboxBP.Enabled = false;
                dateTimePickerBPManagement.CustomFormat = "MM/yyyy";
                dateTimePickerBPManagement.Enabled = true;
            } else {
                yearRecordsCheckboxBP.Enabled = true;
                dateTimePickerBPManagement.Enabled = false;
            }
        }

        private void yearRecordsCheckboxBP_CheckedChanged(object sender, EventArgs e) {
            if (yearRecordsCheckboxBP.Checked == true) {
                monthRecordsCheckboxBP.Checked = false;
                monthRecordsCheckboxBP.Enabled = false;
                dateTimePickerBPManagement.CustomFormat = "yyyy";
                dateTimePickerBPManagement.Enabled = true;
            } else {
                monthRecordsCheckboxBP.Enabled = true;
                dateTimePickerBPManagement.Enabled = false;
            }
        }

        private void dateTimePickerBPManagement_ValueChanged(object sender, EventArgs e) {
            DateTimePickerType pickerType = getDateTimePickerType(dateTimePickerBPManagement);

            //If the returned value is UNDEFINED it means that something went wrong the control returns from the method meaning and as a result no data is sent to the controller
            if (pickerType == DateTimePickerType.UNDEFINED) {
                return;
            }

            sendDataToController(pickerType, dateTimePickerBPManagement);


        }

        //The method that activates the delete button when a cell from the dataGridView is clicked
        private void dataGridViewBPManagement_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            deleteButtonBPManagement.Enabled = true;
        }

        //the method that activates the submit button when the value of a cell from the dataGridView changes and one of the timespan selection checkboxes is checked
        private void dataGridViewBPManagement_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (monthRecordsCheckboxBP.Checked == true || yearRecordsCheckboxBP.Checked == true) {
                submitButtonBPManagement.Enabled = true;
            }

           selectedRowIndex = dataGridViewBPManagement.CurrentCell.RowIndex;//gets the index of the currently selected row
        }

        private void dataGridViewBPManagement_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show("Invalid data inserted into one/more cells of the DataGridView! Please amend it before continuing.", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
        }

        //VIEW METHODS
        //Method for setting the data source of the DataTable displayed in the GUI
        private void fillDataGridViewBP(DataTable inputDataTable) {

            dataGridViewBPManagement.DataSource = inputDataTable;            
        }

        public void updateView(IModel model) {
            fillDataGridViewBP(model.DataSources[0]);
            disableDataGridViewRows(dataGridViewBPManagement);

            //Array containing the indexes of the columns that will be made non-editable(currently the record ID(primary key), budget plan type, start date and end date are included)
            int[] columnIndexes = new int[] {0, 5, 8, 9 };
            disableDataGridViewColumns(dataGridViewBPManagement, columnIndexes);
        }

        public void disableControls() {
            foreach (Button currentButton in buttons) {
                currentButton.Enabled = false;
            }
        }

        public void enableControls() {
            foreach (Button currentButton in buttons) {
                currentButton.Enabled = true;
            }
        }

        private void wireUp(IUpdaterControl paramController, IUpdaterModel paramModel) {
            if (model != null) {
                model.removeObserver(this);
            }

            this.controller = paramController;
            this.model = paramModel;

            controller.setModel(model);
            controller.setView(this);

            model.addObserver(this);
        }



        //UTIL METHODS
        private DateTimePickerType getDateTimePickerType(DateTimePicker dateTimePicker) {
            //Sets the default value for the picker type
            DateTimePickerType pickerType = DateTimePickerType.UNDEFINED;
            
            if ("MM/yyyy".Equals(dateTimePicker.CustomFormat)) {
                pickerType = DateTimePickerType.MONTHLY_PICKER;
            } else if ("yyyy".Equals(dateTimePicker.CustomFormat)) {
                pickerType = DateTimePickerType.YEARLY_PICKER;
            }

            return pickerType;

        }


        //Method for sending the correct data to the controller acording to user timespan selection
        private void sendDataToController(DateTimePickerType pickerType, DateTimePicker dateTimePicker) {
            QueryData paramContainer = null;
            //If the month records checkbox is selected then the month and year is retrieved from the provided dateTimePicker and the QueryData object is created
            if (pickerType == DateTimePickerType.MONTHLY_PICKER) {
                paramContainer = new QueryData.Builder(userID).addMonth(dateTimePicker.Value.Month).addYear(dateTimePicker.Value.Year).build();
            //If the year record checkbox is selected then only the year is retrieved from the prvided dateTimePicker and the QueryData object is created
            } else if (pickerType == DateTimePickerType.YEARLY_PICKER) {
                 paramContainer = new QueryData.Builder(userID).addYear(dateTimePicker.Value.Year).build();
            }

            QueryType option = getQueryTypeOption();

            //If there is no data in the paramContainer or the option is UNDEFINED then the control will return from the method and no data will be sent to the controller
            if (paramContainer == null || option == QueryType.UNDEFINED) {
                return;
            }

            controller.requestData(option, paramContainer);
        }


        private void submitButtonBPManagement_Click(object sender, EventArgs e) {
            //Asksfor user to confirm the update decision
            DialogResult userOptionConfirmUpdate = MessageBox.Show("Are you sure that you want to update the selected budget plan?", "Budget plan mamagement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //If the user selects the 'No' option then no budget plan is updated
            if (userOptionConfirmUpdate == DialogResult.No) {
                return;
            }

            //Checks if the percentage limit for items were correctly set (if the total sum equals 100%)          
            if (calculatePercentagesSum(selectedRowIndex, dataGridViewBPManagement) < 100 || calculatePercentagesSum(selectedRowIndex, dataGridViewBPManagement) > 100) {
                MessageBox.Show("The sum of the specified percentages for budget items does not equal 100%! Please amend the necessary limits before continuing.", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            //Creates the planChecker object for accessing the methods inside the BudgetPlanChecker class
            BudgetPlanChecker planChecker = new BudgetPlanChecker(userID, DateTime.Now.ToString("yyyy-MM-dd"));
            String[] selectedRowDates = getDatesFromSelectedRow(selectedRowIndex, dataGridViewBPManagement);

            if (selectedRowDates == null || selectedRowDates.Length < 2) {
                return;
            }

            String startDate = selectedRowDates[0];
            String endDate = selectedRowDates[1];

            int[] itemTotals = planChecker.getTotalValuesForAllBudgetItems(startDate, endDate);

            if (itemTotals == null || itemTotals.Length != 3) {
                return;
            }

            int[] userSetPercentages = getItemsPercentagesFromSelectedRow(selectedRowIndex, dataGridViewBPManagement);

            //Checks to see if the percentage set by the user is higher/equal to the percentage of the current item total value calculated in relation to the total incomes from the timespan between start date and end date
            if (!planChecker.isLowerThanCurrentItemPercentage(userSetPercentages, itemTotals, startDate, endDate)) {
                MessageBox.Show("One of the modified item percentages is lower than the actual percentage calculated based on the current item total value! The percentage that you set must be higher or at least equal to the current percentage.", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            QueryType option = getQueryTypeOption();

            //If the option is equal to 0 it means that something went wrong and the control is returns from the method
            if (option == 0) {
                return;
            }

            QueryData paramContainer = getDataContainer(option, dateTimePickerBPManagement);
                   
            if (paramContainer == null) {
                return;
            }

            //Getting the DataTable object representing the data source for the dataGridView
            DataTable sourceDataTable = (DataTable)dataGridViewBPManagement.DataSource;
            int executionResult = controller.requestUpdate(option, paramContainer, sourceDataTable);

            if (executionResult != -1) {
                MessageBox.Show("The selected data was updated successfully!", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("Unable to update the selected data! Please try again", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            submitButtonBPManagement.Enabled = false;
            deleteButtonBPManagement.Enabled = false;
            
        }

        private void deleteButtonBPManagement_Click(object sender, EventArgs e) {
            //Asks the user to confirm the delete decision
            DialogResult userOptionConfirmDelete = MessageBox.Show("Are you sure that you want to delete the selected budget plan?", "Budget plan mamagement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            //If the user selects the 'No' option then no budget plan is deleted
            if (userOptionConfirmDelete == DialogResult.No) {
                return;
            } 
                
            int itemID = getSelectedItemID();

            if (itemID == -1) {
                return;
            } 

            int executionResult = controller.requestDelete("budget_plans", itemID);

            if (executionResult != -1) {
                MessageBox.Show("The selected budget plan was successfully deleted!", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Retrieves the row index of the currrently selected cell
                int selectedIndex = dataGridViewBPManagement.CurrentCell.RowIndex;
                //Removes the row at the selected index from the GUI table
                dataGridViewBPManagement.Rows.RemoveAt(selectedIndex);
            } else {
                MessageBox.Show("Unable to delete the selected budget plan! Please try again.", "Budget plan management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private QueryType getQueryTypeOption() {
            if (monthRecordsCheckboxBP.Checked == true) {
                return QueryType.SINGLE_MONTH;

            } else if (yearRecordsCheckboxBP.Checked == true) {
                return QueryType.FULL_YEAR;
            }

            return QueryType.UNDEFINED;
        }

        //Method for creating the correctly configured QueryData object(data container) according to the timespan selection(single month/full year)
        private QueryData getDataContainer(QueryType option, DateTimePicker dateTimePicker) {
            if (option == QueryType.SINGLE_MONTH) {
                int month = dateTimePicker.Value.Month;
                int year = dateTimePicker.Value.Year;

                return new QueryData.Builder(userID).addMonth(month).addYear(year).build();

            } else if (option == QueryType.FULL_YEAR) {
                int year = dateTimePicker.Value.Year;

                return new QueryData.Builder(userID).addYear(year).build();
            }

            return null;
        }

        private int getSelectedItemID() {
            //Getting the ID of the selected record from the DataTable object that represents the data source of the table displayed in the GUI
            CurrencyManager currencyManager = (CurrencyManager)dataGridViewBPManagement.BindingContext[dataGridViewBPManagement.DataSource, dataGridViewBPManagement.DataMember];
            DataRowView selectedDataRow = (DataRowView)currencyManager.Current;

            int itemID = selectedDataRow.Row.ItemArray[0] != null ? Convert.ToInt32(selectedDataRow.Row.ItemArray[0]) : -1;

            return itemID;
        }

        //Method for making data grid view rows that contain budget plans for which the start and end dates don't meet the specified criteria non-editable
        private void disableDataGridViewRows(DataGridView dataGridView) {
            foreach (DataGridViewRow currentRow in dataGridView.Rows) {
                if (currentRow == null) {
                    return;
                }
                //Retrieves the date strings from the current row of the DataGridView
                String startDateString = currentRow.Cells[8].Value != null ? currentRow.Cells[8].FormattedValue.ToString() : "";
                String endDateString = currentRow.Cells[9].Value != null ? currentRow.Cells[9].FormattedValue.ToString() : "";

                if ("".Equals(startDateString) || "".Equals(endDateString)) {
                    currentRow.ReadOnly = true;
                    return;
                }

                DateTime startDate = DateTime.Parse(startDateString);
                DateTime endDate = DateTime.Parse(endDateString);

                if (!isEditableBasedOnDate(startDate, endDate)) {
                    currentRow.ReadOnly = true;
                }
            }
        }

        //Method for disabling specific columns from the DataGridGriew(the arguments are the specified DataGridView and an array containing the indexes of the columns which need to be non-editable)
        private void disableDataGridViewColumns(DataGridView dataGridView, int[] columnIndexes) {
            if (dataGridView == null || columnIndexes == null) {
                return;
            }
            //Each index from the array is checked to see if is higher than the index of the last column in the DataGridView
            foreach (int currentIndex in columnIndexes) {
                if(currentIndex > dataGridView.Columns.Count - 1) {
                    return;
                }

                dataGridView.Columns[currentIndex].ReadOnly = true;
            }
        }


        //Method for checking if the budget plan contained in a DataGridView row can be edited
        //Editing is allowed only for current budget plans, future budget plans and for plans that started in the past but will continue for some time in the future after the start of the current month 
        private bool isEditableBasedOnDate(DateTime startDate, DateTime endDate) {
            //Gets the current date
            DateTime currentDate = DateTime.Now;
            //Creates a DateTime object representing the date of the first day of the current month and year 
            DateTime startOfCurrentMonthDate =new DateTime(currentDate.Year, currentDate.Month, 1);

            //Compares the two dates with the startOfCurrentMonthDate value
            int comparisonResultStartDate = DateTime.Compare(startDate, startOfCurrentMonthDate);
            int comparisonResultEndDate = DateTime.Compare(endDate, startOfCurrentMonthDate);

            //Case 1- start date and end date are both before the start of the current month
            if (comparisonResultStartDate < 0 && comparisonResultEndDate < 0) {
                return false;
            //Case 2- start date and end date are both after the start of the current month
            } else if (comparisonResultStartDate > 0 && comparisonResultEndDate > 0) {
                return true;
            //Case 3- start date is before the start of the current month and end date is after the start of the current month
            } else if (comparisonResultStartDate < 0 && comparisonResultEndDate > 0) {
                return true;
            //Case 4- start date is the the same as the start of the current month and the end date if after the start of the current month
            } else if (comparisonResultStartDate == 0 && comparisonResultEndDate > 0) {
                return true;
            }

            return false;
        } 

        //Method for retrieving the start and end date from the currently selected row of the DataGridView
        private String[] getDatesFromSelectedRow(int selectedRowIndex, DataGridView dataGridView) {
            //Arguments checks
            if(dataGridView == null) {
                return null;
            }

            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView.Rows.Count) {
                return null;
            }

            //Getting the selected row
            DataGridViewRow selectedRow = dataGridView.Rows[selectedRowIndex];

            //Gets the dates from the selected row as String objects
            String startDateString = selectedRow.Cells[8].Value != null ? selectedRow.Cells[8].Value.ToString() : "";
            String endDateString = selectedRow.Cells[9].Value != null ? selectedRow.Cells[9].Value.ToString() : "";

            //Changes the format of the date string from MM/dd/yyyy to yyyy/MM/dd so that they can correctly processed by the MySql database
            String sqlFormatStartDate = DateTime.Parse(startDateString).ToString("yyyy-MM-dd");
            String sqlFormatEndDate = DateTime.Parse(endDateString).ToString("yyyy-MM-dd");

            String[] selectedRowDates = new String[] { sqlFormatStartDate, sqlFormatEndDate };

            return selectedRowDates;

        }

        private int[] getItemsPercentagesFromSelectedRow(int selectedRowIndex, DataGridView dataGridView) {
            //Arguments checks
            if (dataGridView == null) {
                return null;
            }

            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView.Rows.Count) {
                return null;
            }

            //Getting the selected row
            DataGridViewRow selectedRow = dataGridView.Rows[selectedRowIndex];

            int expensesPercentage = selectedRow.Cells[2].Value != null ? Convert.ToInt32(selectedRow.Cells[2].Value) : 0;
            int debtsPercentage = selectedRow.Cells[3].Value != null ? Convert.ToInt32(selectedRow.Cells[3].Value) : 0;
            int savingsPercentage = selectedRow.Cells[4].Value != null ? Convert.ToInt32(selectedRow.Cells[4].Value) : 0;

            int[] userSetPercentages = new int[] {expensesPercentage, debtsPercentage, savingsPercentage};

            return userSetPercentages;
        }
        
        //Method for calculating the sum of percentages limit for all budget items
        private int calculatePercentagesSum(int selectedRowIndex, DataGridView dataGridView) {
            if (dataGridView == null || dataGridView.Rows.Count == 0) {
                return -1;
            }

            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView.Rows.Count) {
                return -1;
            }

            //Gets the selected row from the DataGridView
            DataGridViewRow selectedRow = dataGridView.Rows[selectedRowIndex];
            //Converts the value at the respective cell and if this value is null the result will be 0
            int expensePercentage = selectedRow.Cells[2].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells[2].Value) : 0;
            int debtPercentage = selectedRow.Cells[3].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells[3].Value) : 0;
            int savingPercentage = selectedRow.Cells[4].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells[4].Value) : 0;

            int percentagesSum = expensePercentage + debtPercentage + savingPercentage;

            return percentagesSum;

        }

        //Seteaza data curenta a obiectelor de tip DateTimePicker ca prima zi a lunii curente a anului curent
        private void setDateTimePickerDefaultDate(DateTimePicker[] dateTimePickers) {
            //Creaza o instanta a datei curente
            DateTime defaultDate = DateTime.Now;

            //Obtine valoarea anului si a lunii din obiectul creat ca argument si seteaza valoarea zilei la 1
            int month = defaultDate.Month;
            int year = defaultDate.Year;
            int day = 1;

            foreach (DateTimePicker currentPicker in dateTimePickers) {
                //Seteaza valoarea variabilei la true ca sa nu se apeleze metoda asociata atunci cand se modifica data
                //hasResetDatePickers = true;

                //Seteaza data reprezentand prima zi a lunii curente a anului curent in DateTimePicker
                currentPicker.Value = new DateTime(year, month, day);
            }
        }


    }
}
