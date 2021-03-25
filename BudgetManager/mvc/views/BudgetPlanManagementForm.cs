using BudgetManager.mvc.controllers;
using BudgetManager.mvc.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        public BudgetPlanManagementForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            this.buttons = new Button[] {submitButtonBPManagement, deleteButtonBPManagement};
            controller = new BudgetPlanManagementController();
            model = new BudgetPlanManagementModel();

            wireUp(controller, model);        
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

            //If the returned value is 0 it means that something went wrong and the control returns from the method
            if ( pickerType == 0) {
                return;
            }

            sendDataToController(pickerType, dateTimePickerBPManagement);


        }

        private DateTimePickerType getDateTimePickerType(DateTimePicker dateTimePicker) {
            DateTimePickerType pickerType = 0;
            
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

            //If there is no data in the paramContainer or the option is equal to 0 then the control will return from the method and no data will be sent to the controller
            if (paramContainer == null || option == 0) {
                return;
            }

            controller.requestData(option, paramContainer);
        }

        //Method for setting the data source of the DataTable displayed in the GUI
        private void fillDataGridViewBP(DataTable inputDataTable) {

            dataGridViewBPManagement.DataSource = inputDataTable;
        }

        public void updateView(IModel model) {
            fillDataGridViewBP(model.DataSources[0]);
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

        private void submitButtonBPManagement_Click(object sender, EventArgs e) {
            //Asksfor user to confirm the update decision
            DialogResult userOptionConfirmUpdate = MessageBox.Show("Are you sure that you want to updatethe selected budget plan?", "Budget plan mamagement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //If the user selects the 'No' option then no budget plan is updated
            if (userOptionConfirmUpdate == DialogResult.No) {
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

            return 0;
        }

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

        //The method that activates the delete button when a cell from the dataGridView is clicked
        private void dataGridViewBPManagement_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            deleteButtonBPManagement.Enabled = true;
        }

        //the method that activates the submit button when the value of a cell from the dataGridView changes and one of the timespan selection checkboxes is checked
        private void dataGridViewBPManagement_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (monthRecordsCheckboxBP.Checked == true || yearRecordsCheckboxBP.Checked == true) {
                submitButtonBPManagement.Enabled = true;
            }
        }
    }
}
