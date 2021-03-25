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
        private IControl budgetPlanManagementController;
        private IModel budgetPlanManagementModel;


        public BudgetPlanManagementForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            this.buttons = new Button[] {submitButtonBPManagement, deleteButtonBPManagement};           
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

        }


        //Method for sending the correct data to the controller acording to user timespan selection
        private void sendDataToController(DateTimePickerType pickerType, DateTimePicker dateTimePicker) {
            //If the month records checkbox is selected then the month and year is retrieved from the provided dateTimePicker and the QueryData object is created
            if (pickerType == DateTimePickerType.MONTHLY_PICKER) {
                QueryData paramContainer = new QueryData.Builder(userID).addMonth(dateTimePicker.Value.Month).addYear(dateTimePicker.Value.Year).build();
            //If the year record checkbox is selected then only the year is retrieved from the prvided dateTimePicker and the QueryData object is created
            } else if (pickerType == DateTimePickerType.YEARLY_PICKER) {
                QueryData paramContainer = new QueryData.Builder(userID).addYear(dateTimePicker.Value.Year).build();
            }
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
    }
}
