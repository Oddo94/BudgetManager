using MySql.Data.MySqlClient;
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

namespace BudgetManager {
    public partial class UpdateUserDataForm : Form, IView {
        private IUpdaterModel model = new UpdateUserDataModel();
        private IUpdaterControl controller = new UpdateUserDataController();
        private ArrayList controls = new ArrayList();
        private DateTimePicker[] dateTimePickers = new DateTimePicker[] { };
        private int userID;

        public UpdateUserDataForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            controls = new ArrayList() { tableSelectionComboBox, deleteButton, submitButton };
            dateTimePickers = new DateTimePicker[] { dateTimePickerTimeSpanSelection };
            setDateTimePickerDefaultDate(dateTimePickers);
            wireUp(controller, model);

        }

        private void wireUp(IUpdaterControl paramController, IUpdaterModel paramModel) {
            if (model != null) {
                model.removeObserver(this);
            }

            this.model = paramModel;
            this.controller = paramController;

            //Reversing the method calls
            controller.setView(this);
            controller.setModel(model);

            model.addObserver(this);
        }

        private void tableSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e) {                     
            //Creating the object that will hold the DateTimePicker selector type
            DateTimePickerType pickerType = 0;
          
            //If the checkbox for getting data for a single month is selected then the previously created object is assigned the MONTHLY_PICKER value
            //else it is assigned the YEARLY_PICKER value
            if (monthRecordsCheckBox.Checked == true) {
                pickerType = DateTimePickerType.MONTHLY_PICKER;
            } else if (yearRecordsCheckBox.Checked == true) {
                pickerType = DateTimePickerType.YEARLY_PICKER;
            }

            //Sending data to the specialized method for communicating with the controller
            sendDataToController(pickerType, tableSelectionComboBox, dateTimePickerTimeSpanSelection);

        }

        private void submitButton_Click(object sender, EventArgs e) {                      
            //Checks if the timespan selection combobxes are selected before the Submit button is pressed
            if (monthRecordsCheckBox.Checked == false && yearRecordsCheckBox.Checked == false) {              
                MessageBox.Show("Please select a time interval first!", "Update data");
                return;
            }
          
            //Displaying a message asking the user to confirm his intention of modifying the data and getting the result of his selection 
            DialogResult userOption = MessageBox.Show("Are you sure that you want to submit the changes to the database?", "Data update form", MessageBoxButtons.YesNo);

            if(userOption == DialogResult.No) {
                return;
            }
                    
            //Getting the currently selected table name
            String tableName = tableSelectionComboBox.SelectedItem.ToString();
 
            QueryType option = 0;
            QueryData paramContainer = null;
          
            //Getting the necessary data for restoring the SQL query used for displaying the data present in the table
            //(for generating the commands that will automatically update th DB with the changes that the user made in the GUI)
            if (monthRecordsCheckBox.Checked == true) {
                option = QueryType.SINGLE_MONTH;
                int currentMonth = dateTimePickerTimeSpanSelection.Value.Month;
                int currentYear = dateTimePickerTimeSpanSelection.Value.Year;                
                paramContainer = new QueryData.Builder(userID).addMonth(currentMonth).addYear(currentYear).addTableName(tableName).build(); //CHANGE
            } else if (yearRecordsCheckBox.Checked == true) {
                option = QueryType.FULL_YEAR;
                int currentMonth = dateTimePickerTimeSpanSelection.Value.Month;
                int currentYear = dateTimePickerTimeSpanSelection.Value.Year;              
                paramContainer = new QueryData.Builder(userID).addYear(currentYear).addTableName(tableName).build(); //CHANGE
            }

            //Getting the data source of the table displayed in the GUI
            DataTable sourceDataTable = (DataTable)dataGridViewTableDisplay.DataSource;

            //Sending data to controller and checking the execution result
            int executionResult = controller.requestUpdate(option, paramContainer, sourceDataTable);
            if (executionResult != -1) {
                MessageBox.Show("The selected data was updated successfully!", "Update data");
            } else {               
                MessageBox.Show("Unable to update the selected data! Please try again.", "Update data");
            }

            submitButton.Enabled = false;//Disables the Submit button after deleting data from the table
            deleteButton.Enabled = false;//Disables the Delete button after deleting data from the table
        }


        private void dataGridViewTableDisplay_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
           
            if (monthRecordsCheckBox.Checked == true || yearRecordsCheckBox.Checked == true) {
                submitButton.Enabled = true;
            }
            
        }

        private void deleteButton_Click(object sender, EventArgs e) {
            //Getting the selected row index
            int selectedRowIndex = dataGridViewTableDisplay.CurrentCell.RowIndex;
                   
            String confirmationMessage = String.Format("Are you sure that you want to delete row number {0}?", selectedRowIndex);
            DialogResult userOption1 = MessageBox.Show(confirmationMessage, "Data update form", MessageBoxButtons.YesNo);

            if (userOption1 == DialogResult.No) {
                return;
            }
                    
            //Getting the currently selected table name
            String tableName = tableSelectionComboBox.Text;
           
            //Getting the ID of the selected record from the DataTable object that represents the data source of the table displayed in the GUI
            CurrencyManager currencyManager = (CurrencyManager)dataGridViewTableDisplay.BindingContext[dataGridViewTableDisplay.DataSource, dataGridViewTableDisplay.DataMember];
            DataRowView selectedDataRow = (DataRowView)currencyManager.Current;
            int itemID = selectedDataRow.Row.ItemArray[0] != null ? Convert.ToInt32(selectedDataRow.Row.ItemArray[0]) : -1;
                     
            //Getting the result of executing the delete command by calling the requestDelete method of the controller (which in turn calls the delete method of the model)
            int executionResult = controller.requestDelete(tableName, itemID);
                       
            //Displaying info message regarding the delete operation result
            if (executionResult != -1) {
                MessageBox.Show("The selected data was successfully deleted !", "Data update");                
                //Deleting row from the table displayed in the GUI if the delete operation was successfull
                dataGridViewTableDisplay.Rows.RemoveAt(selectedRowIndex);
            } else {
                MessageBox.Show("Unable to delete the selected data! Please try again.", "Data update");
            } 


            submitButton.Enabled = false;//Disables the Submit button after deleting data from the table
            deleteButton.Enabled = false;//Disables the Delete button after deleting data from the table
        }


        private void dataGridViewTableDisplay_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            deleteButton.Enabled = true;
        }

        private void monthRecordsCheckBox_CheckedChanged(object sender, EventArgs e) {
            if(monthRecordsCheckBox.Checked == true) {
               yearRecordsCheckBox.Checked = false;
               yearRecordsCheckBox.Enabled = false;
               dateTimePickerTimeSpanSelection.CustomFormat = "MM/yyyy";//DateTimePicker format setting
               dateTimePickerTimeSpanSelection.Enabled = true;//DateTimePicker control enabling
                tableSelectionComboBox.Enabled = true;//Table selection combobox enabling
               
            } else {
               dateTimePickerTimeSpanSelection.Enabled = false;//DateTimePicker disabling
               yearRecordsCheckBox.Enabled = true;
               tableSelectionComboBox.Enabled = false;//Table selection combobox disabling          
            }           
        }

        private void yearRecordsCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (yearRecordsCheckBox.Checked == true) {
                monthRecordsCheckBox.Checked = false;
                monthRecordsCheckBox.Enabled = false;
                dateTimePickerTimeSpanSelection.CustomFormat = "yyyy";
                dateTimePickerTimeSpanSelection.Enabled = true;
                tableSelectionComboBox.Enabled = true;
               
            } else {
                dateTimePickerTimeSpanSelection.Enabled = false;
                monthRecordsCheckBox.Enabled = true;
                tableSelectionComboBox.Enabled = false;               
            }
        }

        public void updateView(IModel model) {
            fillDataGridView(dataGridViewTableDisplay, model.DataSources[0]);

        }

        public void disableControls() {
            foreach(Control currentControl in controls) {
                currentControl.Enabled = false;
            }
        }

        public void enableControls() {
            foreach (Control currentControl in controls) {
                currentControl.Enabled = true;
            }
        }

        private void fillDataGridView(DataGridView gridView, DataTable inputDataTable) {
            //Null check and row count check(the condition for row count is set to >= 0 so that if a user selects an item with no available records, 
            //an empty table will be displayed and it will be easier to understand the current status of that item for the selected month)
            if (inputDataTable != null && inputDataTable.Rows.Count >= 0) {
                //Sets the data source for the table
                gridView.DataSource = inputDataTable;             
                //Deactivates the editing for the first table column because it will always contain the primary keys of the records and modifying these values would alter the DB structure
                dataGridViewTableDisplay.Columns[0].ReadOnly = true;
            }
        }

        private void sendDataToController(DateTimePickerType pickerType, ComboBox itemComboBox, DateTimePicker datePicker) {
            //Single month data selection
            if (pickerType == DateTimePickerType.MONTHLY_PICKER) {
                QueryType option = QueryType.SINGLE_MONTH;

                int selectedMonth = datePicker.Value.Month;
                int selectedYear = datePicker.Value.Year;
                String tableName = itemComboBox.Text;               
                //Creating the storage object for the data that will be passed to the controller
                //QueryData paramContainer = new QueryData(userID, selectedMonth, selectedYear, tableName);
                QueryData paramContainer = new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).addTableName(tableName).build(); //CHANGE

                controller.requestData(option, paramContainer);
         
              //Multiple months data selection
            } else if (pickerType == DateTimePickerType.YEARLY_PICKER) {
                QueryType option = QueryType.FULL_YEAR;

                int selectedYear = datePicker.Value.Year;
                String tableName = itemComboBox.Text;

                //QueryData paramContainer = new QueryData(userID, selectedYear,tableName);
                QueryData paramContainer = new QueryData.Builder(userID).addYear(selectedYear).addTableName(tableName).build(); //CHANGE

                controller.requestData(option, paramContainer);
            }
        }

        private void setDateTimePickerDefaultDate(DateTimePicker[] dateTimePickers) {            
            //Creating an instance of the current date
            DateTime defaultDate = DateTime.Now;
           
            //Getting the value of the month and year from the created object and setting the value of the day to 1 (start of the month)
            int month = defaultDate.Month;
            int year = defaultDate.Year;
            int day = 1;

            foreach (DateTimePicker currentPicker in dateTimePickers) {                                     
                //Sets the DateTimePicker date to the first day of the current month from the current year
                currentPicker.Value = new DateTime(year, month, day);
            }
        }
    }
}

