﻿using BudgetManager.utils;
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
        //The variable holding the value of the last modifed row
        private int changedRowIndex;
        //The variables holding the new and old values of the modified record
        private int oldRecordValue;
        private int newRecordValue;
        //The variables holding the value and date of the deleted record
        private int deletedRecordValue;
        private DateTime deletedRecordDate;
        //The variables holding the new and old dates of the modified record 
        private DateTime oldRecordDate;
        private DateTime newRecordDate;

        private bool hasChangedRecordValue;
        private bool hasChangedRecordDate;



        public UpdateUserDataForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            controls = new ArrayList() { tableSelectionComboBox, deleteButton, submitButton };
            dateTimePickers = new DateTimePicker[] { dateTimePickerTimeSpanSelection };
            changedRowIndex = -1;
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

            //Balance record update section
            if (getSelectedBudgetItemType() == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {
                //If only the record value is changed by the user then the new record value is set to it
                if (hasChangedRecordDate && !hasChangedRecordValue) {
                    //newRecordValue = getSelectedRecordValue();
                    newRecordValue = getValueFromRow(changedRowIndex, 3);//CHANGE(takes the value from the actual changed row not from the currently selected one which can be different)
                 
                }

                int month = 0;
                int year = 0;
                DateTime selectedRecordDate = DateTime.MinValue;//CHANGE

                //If the record date was modified then the month and year variables will be assigned the corresponding values from the old date otherwise the current date of the record will be used
                if (hasChangedRecordDate) {
                    month = oldRecordDate.Month;
                    year = oldRecordDate.Year;
                    selectedRecordDate = oldRecordDate;//Selected record date will be the old record date since we need to save the date that existed previous to the change
                } else {
                    //DateTime selectedRecordDate = getSelectedRecordDate();
                    selectedRecordDate = getDateFromRow(changedRowIndex, 4);//CHANGE(takes the date from the actual changed row not from the currently selected one which can be different)
                    month = selectedRecordDate.Month;
                    year = selectedRecordDate.Year;
                }

         
                //Retrieving the execution result of the method responsible for updating the balance record table               
                int savingAccountBalanceUpdateResult = updateSavingAccountBalanceTable(userID, month, year, selectedRecordDate, getSelectedBudgetItemType(), false);

                if (savingAccountBalanceUpdateResult == -1) {
                    MessageBox.Show("Unable to update the saving account balance record.", "Data update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            submitButton.Enabled = false;//Disables the Submit button after deleting data from the table
            deleteButton.Enabled = false;//Disables the Delete button after deleting data from the table
    
            clearFlags();
            resetSavedValues();         
        }

        private void deleteButton_Click(object sender, EventArgs e) {
            //Getting the selected row index
            int selectedRowIndex = dataGridViewTableDisplay.CurrentCell.RowIndex;
            //The value of the deleted record and its date are saved before the actual deletion takes place because otherwise they would be lost and the update of the corresponding saving account balance record would be impossible        
            int selectedRowForDeletionIndex = dataGridViewTableDisplay.CurrentCell.RowIndex;
            deletedRecordValue = getValueFromRow(selectedRowForDeletionIndex, 3);
            deletedRecordDate = getDateFromRow(selectedRowForDeletionIndex, 4);

            String confirmationMessage = String.Format("Are you sure that you want to delete row number {0}?", selectedRowIndex);
            DialogResult userOption1 = MessageBox.Show(confirmationMessage, "Data update form", MessageBoxButtons.YesNo);

            if (userOption1 == DialogResult.No) {
                return;
            }
           
            if (hasChangedRows()) {
                MessageBox.Show("You cannot delete the selected row before submitting or discarding the currently pending change/s!", "Data update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            //Getting the currently selected table name
            String tableName = tableSelectionComboBox.Text;

            //Getting the ID of the selected record from the DataTable object that represents the data source of the table displayed in the GUI
            CurrencyManager currencyManager = (CurrencyManager)dataGridViewTableDisplay.BindingContext[dataGridViewTableDisplay.DataSource, dataGridViewTableDisplay.DataMember];
            DataRowView selectedDataRow = (DataRowView)currencyManager.Current;
            int itemID = selectedDataRow.Row.ItemArray[0] != null ? Convert.ToInt32(selectedDataRow.Row.ItemArray[0]) : -1;
          
            //CHANGE!!!!
            QueryType option = 0;
            QueryData paramContainer = null;

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

            //Retrieves the DataTable object representing the data source of the DataGridView
            DataTable sourceDataTable = (DataTable)dataGridViewTableDisplay.DataSource;
            
            sourceDataTable.Rows[selectedRowIndex].Delete();//Deletes the row from the DataTable object


            int executionResult = controller.requestDelete(option, paramContainer, sourceDataTable);

           
            //Displaying info message regarding the delete operation result
            if (executionResult != -1) {
                MessageBox.Show("The selected data was successfully deleted !", "Data update");              
            } else {
                MessageBox.Show("Unable to delete the selected data! Please try again.", "Data update");
            }

            if (getSelectedBudgetItemType() == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {               
                int month = deletedRecordDate.Month;
                int year = deletedRecordDate.Year;
                
                int savingAccountBalanceUpdateResult = updateSavingAccountBalanceTable(userID, month, year, deletedRecordDate, getSelectedBudgetItemType(), true);

                if (savingAccountBalanceUpdateResult == -1) {
                    MessageBox.Show("Unable to update the saving account balance record", "Data update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            submitButton.Enabled = false;//Disables the Submit button after deleting data from the table
            deleteButton.Enabled = false;//Disables the Delete button after deleting data from the table
          
            clearFlags();
            resetSavedValues();
            

        }

        private void dataGridViewTableDisplay_CellClick(object sender, DataGridViewCellEventArgs e) {
            //Default values for expected column indexes
            int expectedValueColumnIndex = -1;
            int expectedDateColumnIndex = -1;
            //The actual values for the expected column indexes are set based on the selected budget item type(because the layout of the displayed DataGridView for each of them is different)
            if(getSelectedBudgetItemType() == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {
                expectedValueColumnIndex = 3;
                expectedDateColumnIndex = 4;
            } else if (getSelectedBudgetItemType() == BudgetItemType.SAVING) {
                expectedValueColumnIndex = 2;
                expectedDateColumnIndex = 3;
            } else {
                return;
            }

            //if (getSelectedBudgetItemType() == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {
                if (!hasChangedRecordDate || !hasChangedRecordValue) {
                    //Retrieving the column index of the modified cell
                    int currentCellColumn = e.ColumnIndex;

                    //The variable used for storing the value of the modified cell(the values are saved only if changes are performed to the value or date columns)
                    object selectedCellValue = null;
                    if (currentCellColumn == expectedValueColumnIndex) {
                        selectedCellValue = dataGridViewTableDisplay.CurrentCell.Value;
                        object dateCellValue = dataGridViewTableDisplay.CurrentRow.Cells[currentCellColumn].Value;
                        oldRecordValue = selectedCellValue != DBNull.Value ? Convert.ToInt32(selectedCellValue) : -1;
                        MessageBox.Show("Selected cell value: " + oldRecordValue);

                    } else if (currentCellColumn == expectedDateColumnIndex) {
                        DateTime temp;
                        selectedCellValue = dataGridViewTableDisplay.CurrentCell.Value;
                        object valueCellValue = dataGridViewTableDisplay.CurrentRow.Cells[currentCellColumn].Value;
                        oldRecordDate = DateTime.TryParse(Convert.ToString(selectedCellValue), out temp) ? DateTime.Parse(Convert.ToString(selectedCellValue)) : DateTime.MinValue;
                        MessageBox.Show("Selected cell value: " + oldRecordDate);
                }
                }
           // }
        }

        //Saving the original values of value and date columns when the saving account expenses data is shown in the DataGridView and one of these cells is clicked
        private void dataGridViewTableDisplay_CellEnter(object sender, DataGridViewCellEventArgs e) {
            //if (getSelectedBudgetItemType() == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {                                         
            //    //Retrieving the column index of the modified cell
            //    int currentCellColumn = e.ColumnIndex;

            //    //The variable used for storing the value of the modified cell(the values are saved only if changes are performed to the value or date columns)
            //    object selectedCellValue = null;                           
            //    if (currentCellColumn == 3) {
            //        selectedCellValue = dataGridViewTableDisplay.CurrentCell.Value;
            //        object dateCellValue = dataGridViewTableDisplay.CurrentRow.Cells[4].Value;
            //        oldRecordValue = selectedCellValue != DBNull.Value ? Convert.ToInt32(selectedCellValue) : -1;
            //        //oldRecordDate = dateCellValue != DBNull.Value ? DateTime.Parse(Convert.ToString(dateCellValue)) : DateTime.MinValue;//CHANGE
            //        //MessageBox.Show(String.Format("Old record value: {0} \n Old record date:{1}", oldRecordValue, Convert.ToString(oldRecordDate)));
            //    } else if (currentCellColumn == 4) {
            //        selectedCellValue = dataGridViewTableDisplay.CurrentCell.Value;
            //        object valueCellValue = dataGridViewTableDisplay.CurrentRow.Cells[3].Value;
            //        oldRecordValue = valueCellValue != DBNull.Value ? Convert.ToInt32(valueCellValue) : -1;
            //        oldRecordDate = selectedCellValue != DBNull.Value ? DateTime.Parse(Convert.ToString(selectedCellValue)) : DateTime.MinValue;
            //        //MessageBox.Show(String.Format("Old record value: {0} \n Old record date:{1}", oldRecordValue, Convert.ToString(oldRecordDate)));
            //    }
            //}
        }

        //Saving the new values of value and date columns when the saving account expenses data is shown in the DataGridView and one of these cells' content is modified
        private void dataGridViewTableDisplay_CellValueChanged(object sender, DataGridViewCellEventArgs e) {           
            if (getSelectedBudgetItemType() == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {                             
                //Retrieving the column index of the modified cell(the new values are saved only if changes are performed to the value or date columns) 
                int changedCellColumn = e.ColumnIndex;

                object changedCellValue = null;
                if (changedCellColumn == 3) {
                    changedCellValue = dataGridViewTableDisplay.CurrentCell.Value;
                    newRecordValue = changedCellValue != DBNull.Value ? Convert.ToInt32(changedCellValue) : -1;
                    hasChangedRecordValue = true;
                   
                    changedRowIndex = dataGridViewTableDisplay.CurrentCell.RowIndex;
                } else if (changedCellColumn == 4) {
                    changedCellValue = dataGridViewTableDisplay.CurrentCell.Value;
                    newRecordDate = changedCellValue != DBNull.Value ? DateTime.Parse(Convert.ToString(changedCellValue)) : DateTime.MinValue;
                    hasChangedRecordDate = true;
                    
                    changedRowIndex = dataGridViewTableDisplay.CurrentCell.RowIndex;
                }

                setRowsEditableProperty(dataGridViewTableDisplay, true, e.RowIndex);//Makes all the rows of the DataGridView non-editable except for the one containing the changed values 

            }

            DateTime temp;
            if ((monthRecordsCheckBox.Checked == true || yearRecordsCheckBox.Checked == true)) {
                if (hasChangedRecordValue && newRecordValue <= 0) {
                    return;
                } else if (hasChangedRecordDate && !DateTime.TryParse(newRecordDate.ToString(), out temp)) {
                    return;
                }

                submitButton.Enabled = true;
            }
            
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

        /*****************************************************************************/
        //UTILITY METHODS SECTION

        private void fillDataGridView(DataGridView gridView, DataTable inputDataTable) {
            //Null check and row count check(the condition for row count is set to >= 0 so that if a user selects an item with no available records, 
            //an empty table will be displayed and it will be easier to understand the current status of that item for the selected month)
            if (inputDataTable != null && inputDataTable.Rows.Count >= 0) {
                //Sets the data source for the table
                gridView.DataSource = inputDataTable;             
                //Deactivates the editing for the first table column because it will always contain the primary keys of the records and modifying these values would alter the DB structure
                dataGridViewTableDisplay.Columns[0].ReadOnly = true;

                //oldDataSource = inputDataTable;//CHANGE!!!
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
                QueryData paramContainer = new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).addTableName(tableName).build(); //CHANGE

                controller.requestData(option, paramContainer);
         
              //Multiple months data selection
            } else if (pickerType == DateTimePickerType.YEARLY_PICKER) {
                QueryType option = QueryType.FULL_YEAR;

                int selectedYear = datePicker.Value.Year;
                String tableName = itemComboBox.Text;
                
                QueryData paramContainer = new QueryData.Builder(userID).addYear(selectedYear).addTableName(tableName).build(); //CHANGE

                controller.requestData(option, paramContainer);
            }
        }


        //Method for updating the records in the saving account balance table and creating records in the saving account expense table when needed(secondary task)  
        private int updateSavingAccountBalanceTable(int userID, int month, int year, DateTime date, BudgetItemType selectedItemType, bool performsDeletion) {
            int executionResult = -1;

            //Object that manages the update of the tables that are part of the saving account system(initial object which represents the balance record of the unmodified date)
            SavingAccountBalanceManager balanceManager = new SavingAccountBalanceManager(userID, month, year, date);

            //Balance record table update when only the values of the saving/saving account expense records are modified
            if (!performsDeletion) {
                if (balanceManager.hasBalanceRecord()) {
                    int recordComparisonResult = balanceManager.compareRecordValues(oldRecordValue, newRecordValue);
                    int oldBalanceRecordValue = balanceManager.getRecordValue();
                    int newBalanceRecordValue = 0;
                    int recordDifferenceValue = 0;

                    //Applies when the user has changed the record date or both the record date and record value
                    if (hasChangedRecordDate || (hasChangedRecordValue && hasChangedRecordDate)) {
                        int newMonth = newRecordDate.Month;
                        int newYear = newRecordDate.Year;


                        //A new SavingAccountBalanceManager is created which represents the balance record of the newly modified date
                        SavingAccountBalanceManager newBalanceManager = new SavingAccountBalanceManager(userID, newMonth, newYear, newRecordDate);

                        if (!hasChangedRecordValue) {
                            oldRecordValue = newRecordValue;//Current value of the record(unmodified)
                        }

                        if (newBalanceManager.hasBalanceRecord()) {
                            //If a balance record already exists it will be updated(the record to which the modified value is "moved")
                            newBalanceRecordValue = newBalanceManager.getRecordValue() - newRecordValue;//The new balance record value for the corresponding month to which the date was modified is obtained by subtracting the new expense value from its current value
                            executionResult = newBalanceManager.updateBalanceRecord(newBalanceRecordValue);

                            //After that the balance record representing the old date is also updated(the record from which the modified value is "moved")
                            int previousMonthBalanceUpdatedRecordValue = balanceManager.getRecordValue() + oldRecordValue;//The balance of the month from where the expense was "moved" will increase
                            executionResult = balanceManager.updateBalanceRecord(previousMonthBalanceUpdatedRecordValue);
                        } else {
                            //If not a new record with the newly modified value will be created
                            executionResult = newBalanceManager.createBalanceRecord(newRecordValue);
                        }

                    } else if (hasChangedRecordValue) {
                        if (recordComparisonResult == -1) {
                            recordDifferenceValue = oldRecordValue - newRecordValue;//CASE:new value is lower than the old value
                            newBalanceRecordValue = balanceManager.getRecordValue() + recordDifferenceValue;//The difference is added to the old record value since the balance increases by entering a smaller value expense                            
                            executionResult = balanceManager.updateBalanceRecord(newBalanceRecordValue);
                        } else if (recordComparisonResult == 1) {
                            recordDifferenceValue = newRecordValue - oldRecordValue;//CASE:new value is higher than the old value
                            newBalanceRecordValue = balanceManager.getRecordValue() - recordDifferenceValue;//The difference is subtracted from the old record value since the balance decreases by entering a higher value expense
                            executionResult = balanceManager.updateBalanceRecord(newBalanceRecordValue);
                        }
                    }
                }

            } else {
                //Balance record table update when a saving/saving account expense record is deleted
                SavingAccountBalanceManager deletionBalanceManager = new SavingAccountBalanceManager(userID, month, year, date);
                int currentBalanceRecordValue = deletionBalanceManager.getRecordValue();                
                int newBalanceRecordValue = currentBalanceRecordValue + deletedRecordValue;
                executionResult = deletionBalanceManager.updateBalanceRecord(newBalanceRecordValue); 
            }

            return executionResult;
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

        //Method for enabling/disabling all DataGridView rows except for the specified one(if all rows need to be made editable then -1 value can be provided as value for the exceptedRowIndex alongside true as the value for isEditable flag)
        private void setRowsEditableProperty(DataGridView dataGridView, bool isEditable, int exceptedRowIndex) {
            if (dataGridView == null && dataGridView.Rows.Count == 0) {
                return;
            }

            for(int i = 0; i < dataGridView.Rows.Count; i++) {
                if (exceptedRowIndex != i) {
                    dataGridView.Rows[i].ReadOnly = isEditable;
                }
            }
        }

        //Method for checking if the selected row for deletion was previously changed
        private bool hasChangedRows() {
            //Retrieves the DataTable object containing the modified rows from the DataGridView
            DataTable changedRowsDataTable = ((DataTable)dataGridViewTableDisplay.DataSource).GetChanges();

            //If the DataTable is not null it means that there are pending changes so the method will return true
            if (changedRowsDataTable != null) {
                return true;
            }
    
            return false;
        }


        private BudgetItemType getSelectedBudgetItemType() {
            int selectedIndex = tableSelectionComboBox.SelectedIndex;

            switch(selectedIndex) {
                case 0:
                    return BudgetItemType.INCOME;
                    
                case 1:
                    return BudgetItemType.GENERAL_EXPENSE;

                case 2:
                    return BudgetItemType.SAVING_ACCOUNT_EXPENSE;

                case 3:
                    return BudgetItemType.DEBT;

                case 4:
                    return BudgetItemType.SAVING;

                default:
                    return BudgetItemType.UNDEFINED;

            }
        }

        //Method for calculating the new record value after changing the current value
        private int calculateNewRecordValue(int currentRecordValue, int recordDifferenceValue, int comparisonResult, BudgetItemType selectedItemType) {
            int newRecordValue = -1;

            if (selectedItemType == BudgetItemType.SAVING_ACCOUNT_EXPENSE) {
                if (comparisonResult == -1) {
                    newRecordValue = currentRecordValue + recordDifferenceValue;//The new value is smaller than the old value->the new record value increases accordingly(higher balance due to smaller expenses)
                } else if(comparisonResult == 1) {
                    newRecordValue = currentRecordValue - recordDifferenceValue;//The new value is greater than the old value->the new record value decreases accordingly(lower balance due to higher expenses)
                }
            } else if(selectedItemType == BudgetItemType.SAVING) {
                if(comparisonResult == -1) {
                    newRecordValue = currentRecordValue - recordDifferenceValue;//The new value is smaller than the old value->the new record value decreases accordingly(lower balance due to smaller savings)
                } else if(comparisonResult == 1) {
                    newRecordValue = currentRecordValue + recordDifferenceValue;//The new value is greater than the old value->the new record value increases accordingly(higher balance due to higher savings)
                } 
            }

            return newRecordValue;
        }

        //Method for clearing the flags set when changing record value/date of a saving account expense or saving
        private void clearFlags() {
            this.hasChangedRecordValue = false;
            this.hasChangedRecordDate = false;
        }

        //Method for resetting the values saved when changing record value/date of a saving account expense or saving
        private void resetSavedValues() {
            this.oldRecordValue = 0;
            this.newRecordValue = 0;

            this.oldRecordDate = DateTime.MinValue;
            this.newRecordDate = DateTime.MinValue;

            this.deletedRecordValue = 0;
            this.deletedRecordDate = DateTime.MinValue;

            this.changedRowIndex = -1;

        }

        //Method for retrieving the record date from the specified row of the DataGridView
        private DateTime getDateFromRow(int rowIndex, int columnIndex) {
            if(dataGridViewTableDisplay == null || dataGridViewTableDisplay.Rows.Count < rowIndex || rowIndex == -1) {
                return DateTime.MinValue;
            }

            if (dataGridViewTableDisplay.Columns.Count - 1 < columnIndex || columnIndex == -1) {
                return DateTime.MinValue;
            }
            
            DataRow specifiedRow = ((DataTable)dataGridViewTableDisplay.DataSource).Rows[rowIndex];          
            object rowDateObject = specifiedRow.ItemArray[columnIndex];
            DateTime rowDateValue = rowDateObject != DBNull.Value ? DateTime.Parse(Convert.ToString(rowDateObject)) : DateTime.MinValue;

            return rowDateValue;   
        }

        ////Method for retrieving the record value from the specified row of the DataGridView
        private int getValueFromRow(int rowIndex, int columnIndex) {
            if (dataGridViewTableDisplay == null || dataGridViewTableDisplay.Rows.Count - 1 < rowIndex || rowIndex == -1) {
                return -1;
            }

            if (dataGridViewTableDisplay.Columns.Count - 1 < columnIndex || columnIndex == -1) {
                return -1;
            }
         
            DataRow specifiedRow = ((DataTable)dataGridViewTableDisplay.DataSource).Rows[rowIndex];            
            object rowDateObject = specifiedRow.ItemArray[columnIndex];
            int recordValue = rowDateObject != DBNull.Value ? Convert.ToInt32(rowDateObject) : -1;

            return recordValue;

        }
    }
}

