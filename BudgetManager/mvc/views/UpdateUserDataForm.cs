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

            //Am inversat apelul metodelor pt a evita NPE
            controller.setView(this);
            controller.setModel(model);

            model.addObserver(this);
        }

        private void tableSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e) {          
            //Creare obiect ce stocheaza tipul de selector
            DateTimePickerType pickerType = 0;

            //Daca e bifat checbox-ul obtinere de date pe o lună se atribuie obiectului creat anterior valoarea MONTHLY_PICKER
            //altfel se atribuie valoarea YEARLY_PICKER
            if (monthRecordsCheckBox.Checked == true) {
                pickerType = DateTimePickerType.MONTHLY_PICKER;
            } else if (yearRecordsCheckBox.Checked == true) {
                pickerType = DateTimePickerType.YEARLY_PICKER;
            }

            //Trimitere date catre metoda specializata de comunicare cu controllerul
            sendDataToController(pickerType, tableSelectionComboBox, dateTimePickerTimeSpanSelection);

        }

        private void submitButton_Click(object sender, EventArgs e) {          
            //Verifica daca sunt selectate checkbox-urile pentru alegerea intervalului de timp, anterior selectarii butonului Submit changes 
            if (monthRecordsCheckBox.Checked == false && yearRecordsCheckBox.Checked == false) {              
                MessageBox.Show("Please select a time interval first!", "Update data");
                return;
            }

            //Afisare mesaj de confirmare a intentiei de a modifica datele si obtinerea raspunsului utilizatorului
            DialogResult userOption = MessageBox.Show("Are you sure that you want to submit the changes to the database?", "Data update form", MessageBoxButtons.YesNo);

            if(userOption == DialogResult.No) {
                return;
            }
        
            //Obtinerea numelui tabelului selectat
            String tableName = tableSelectionComboBox.SelectedItem.ToString();
 
            QueryType option = 0;
            QueryData paramContainer = null;
            
            //Obtinere date pentru refacerea comenzii SQL de afisare a datelor din tabel
            if (monthRecordsCheckBox.Checked == true) {
                option = QueryType.SINGLE_MONTH;
                int currentMonth = dateTimePickerTimeSpanSelection.Value.Month;
                int currentYear = dateTimePickerTimeSpanSelection.Value.Year;
                //paramContainer = new QueryData(userID, currentMonth, currentYear, tableName);
                paramContainer = new QueryData.Builder(userID).addMonth(currentMonth).addYear(currentYear).addTableName(tableName).build(); //CHANGE
            } else if (yearRecordsCheckBox.Checked == true) {
                option = QueryType.FULL_YEAR;
                int currentMonth = dateTimePickerTimeSpanSelection.Value.Month;
                int currentYear = dateTimePickerTimeSpanSelection.Value.Year;
                //paramContainer = new QueryData(userID,currentYear, tableName);
                paramContainer = new QueryData.Builder(userID).addYear(currentYear).addTableName(tableName).build(); //CHANGE
            }

            //Obtinerea sursei de date a tabelului afisat in interfata
            DataTable sourceDataTable = (DataTable)dataGridViewTableDisplay.DataSource;

            //Trimitere date catre controller si verificare rezultat executie
            int executionResult = controller.requestUpdate(option, paramContainer, sourceDataTable);
            if (executionResult != -1) {
                MessageBox.Show("The selected data was updated successfully!", "Update data");
            } else {               
                MessageBox.Show("Unable to update the selected data! Please try again.", "Update data");
            }

            submitButton.Enabled = false;//Dezactiveaza butonul de submit dupa modificarea datelor din tabel
            deleteButton.Enabled = false;//Dezactiveaza butonul de delete dupa modificarea datelor din tabel
        }


        private void dataGridViewTableDisplay_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            //MODIFICARE
            if (monthRecordsCheckBox.Checked == true || yearRecordsCheckBox.Checked == true) {
                submitButton.Enabled = true;
            }
            
        }

        private void deleteButton_Click(object sender, EventArgs e) {
            //Se obtine indexul randului selectat
            int selectedRowIndex = dataGridViewTableDisplay.CurrentCell.RowIndex;
       
            //MessageBox.Show("Current selected row item index " + itemID);
            String confirmationMessage = String.Format("Are you sure that you want to delete row number {0}?", selectedRowIndex);
            DialogResult userOption1 = MessageBox.Show(confirmationMessage, "Data update form", MessageBoxButtons.YesNo);

            if (userOption1 == DialogResult.No) {
                return;
            }
           
            //Obținere nume tabel curent
            String tableName = tableSelectionComboBox.Text;

            //Obținere id înregistrare din obiectul DataTable ce reprezinta sursa de date a tabelului curent
            CurrencyManager currencyManager = (CurrencyManager)dataGridViewTableDisplay.BindingContext[dataGridViewTableDisplay.DataSource, dataGridViewTableDisplay.DataMember];
            DataRowView selectedDataRow = (DataRowView)currencyManager.Current;
            int itemID = selectedDataRow.Row.ItemArray[0] != null ? Convert.ToInt32(selectedDataRow.Row.ItemArray[0]) : -1;
          
            //Obtinere rezultat executie prin apelul metodei requestDelete a controllerului
            int executionResult = controller.requestDelete(tableName, itemID);
            
            //Afișare mesaj de informare legat de execuția operației de stergere
            if (executionResult != -1) {
                MessageBox.Show("The selected data was successfully deleted !", "Data update");
                //Stergere rand din tabelul prezent in interfata
                dataGridViewTableDisplay.Rows.RemoveAt(selectedRowIndex);
            } else {
                MessageBox.Show("Unable to delete the selected data! Please try again.", "Data update");
            } 


            submitButton.Enabled = false;//Dezactiveaza butonul de submit dupa stergerea datelor din tabel
            deleteButton.Enabled = false;//Dezactiveaza butonul de delete dupa stergerea datelor din tabel
        }


        private void dataGridViewTableDisplay_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            deleteButton.Enabled = true;
        }

        private void monthRecordsCheckBox_CheckedChanged(object sender, EventArgs e) {
            if(monthRecordsCheckBox.Checked == true) {
               yearRecordsCheckBox.Checked = false;
               yearRecordsCheckBox.Enabled = false;
               dateTimePickerTimeSpanSelection.CustomFormat = "MM/yyyy";//Selectare format dateTimePicker
               dateTimePickerTimeSpanSelection.Enabled = true;//Activare dateTimePicker
               tableSelectionComboBox.Enabled = true;//Activare combobox selectare tabel
               
            } else {
               dateTimePickerTimeSpanSelection.Enabled = false;//Dezactivare dateTimePicker
               yearRecordsCheckBox.Enabled = true;
               tableSelectionComboBox.Enabled = false;//Dezactivare combobox selectare tabel            
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
            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                //Impune sursa de date pentru tabel
                gridView.DataSource = inputDataTable;
                //Dezactiveaza ediatrea primei coloane care contine intotdeauna cheile primare ale inregistrarilor
                dataGridViewTableDisplay.Columns[0].ReadOnly = true;
            }
        }

        private void sendDataToController(DateTimePickerType pickerType, ComboBox itemComboBox, DateTimePicker datePicker) {
            //Obtinere date pe o singură lună
            if (pickerType == DateTimePickerType.MONTHLY_PICKER) {
                QueryType option = QueryType.SINGLE_MONTH;

                int selectedMonth = datePicker.Value.Month;
                int selectedYear = datePicker.Value.Year;
                String tableName = itemComboBox.Text;
                //Creare obiect de stocare a datelor ce vor fi transmise catre controller
                //QueryData paramContainer = new QueryData(userID, selectedMonth, selectedYear, tableName);
                QueryData paramContainer = new QueryData.Builder(userID).addMonth(selectedMonth).addYear(selectedYear).addTableName(tableName).build(); //CHANGE

                controller.requestData(option, paramContainer);
         
              //Obtinere date pe mai multe luni
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
            //Creaza o instanta a datei curente
            DateTime defaultDate = DateTime.Now;

            //Obtine valoarea anului si a lunii din obiectul creat si seteaza valoarea zilei la 1
            int month = defaultDate.Month;
            int year = defaultDate.Year;
            int day = 1;

            foreach (DateTimePicker currentPicker in dateTimePickers) {                      
                //Seteaza data reprezentand prima zi a lunii curente a anului curent in DateTimePicker
                currentPicker.Value = new DateTime(year, month, day);
            }
        }
    }
}

