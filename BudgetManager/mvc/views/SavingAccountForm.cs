using BudgetManager.mvc.controllers;
using BudgetManager.mvc.models;
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
        private bool hasResetDatePickers = false;

        public SavingAccountForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            datePickers = new DateTimePicker[] { dateTimePickerStartSavingAccount, dateTimePickerEndSavingAccount, dateTimePickerMonthlyBalance};                     
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
            fillDataGridView(dataGridViewSavingAccount, results[0]);
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
            sendDataToController(DataUpdateControl.MONTHLY_PICKER, intervalCheckBoxSavingAccount, dateTimePickerMonthlyBalance, dateTimePickerEndSavingAccount);
        }

        private void columnChartMonthlyBalance_MouseHover(object sender, EventArgs e) {
            //Displays the current value of the column on which the mouse hovers 
            columnChartMonthlyBalance.Series[0].ToolTip = "Monthly balance: #VALY";
        }

        //UTIL METHODS SECTION
        private void fillDataGridView(DataGridView gridView, DataTable inputDataTable) {
            if(gridView == null) {
                return;
            }
            gridView.DataSource = inputDataTable;
        }

        private void fillColumnChart(Chart chart, DataTable inputDataTable, int currentYear, String title) {
            //Eliminare puncte din grafic
            chart.Series[0].Points.Clear();

            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                //Creare lista de date(prima zi a fiecarei luni a anului selectat)
                List<DateTime> dates = new List<DateTime>();
                for (int i = 1; i <= 12; i++) {
                    dates.Add(new DateTime(currentYear, i, 1));
                }

                //Adaugare nume luni in grafic
                foreach (DateTime currentDate in dates) {
                    chart.Series[0].Points.AddXY(currentDate, 0);
                }

                int j = 0;//indexul randului curent din tabelul de date provenit din DB
                for (int i = 0; i < chart.Series[0].Points.Count; i++) {
                    //Daca s-au epuizat randurile din DataTable(de ex daca nu avem rezultate pt fiecare luna a anului) se adauga valoarea 0 pt luna respectiva din grafic
                    if (j > inputDataTable.Rows.Count - 1) {
                        chart.Series[0].Points[i].SetValueY(0);
                        continue;
                    }

                    //Se creaza un obiect de tip DateTime pt valoarea X a punctului curent al graficului
                    System.DateTime currentPointDate = System.DateTime.FromOADate(chart.Series[0].Points[i].XValue);
                    //Se extrage valoarea lunii din obiectul de tip DateTime creat anterior
                    int currentChartMonth = Convert.ToInt32(DateTime.ParseExact(currentPointDate.ToString("MMM"), "MMM", CultureInfo.CurrentCulture).Month);
                    int currentDataTableMonth = Convert.ToInt32(inputDataTable.Rows[j].ItemArray[1]);

                    //Se compara daca luna punctului curent din grafic este egala cu luna din tabelul de date provenit din DB
                    if (currentChartMonth == currentDataTableMonth) {
                        int currentDataTableMonthValue = Convert.ToInt32(inputDataTable.Rows[j].ItemArray[2]);
                        chart.Series[0].Points[i].SetValueY(currentDataTableMonthValue);//se adauga valoarea corespunzatoare daca cele doua luni sunt identice
                        j++;//se incrementeaza valoarea indexului randului curent din tabelul de date
                    } else {
                        chart.Series[0].Points[i].SetValueY(0);// in caz contrar se adauga valoare 0 intrucat nu avem o corespondenta intre inregistrarea din tabel si luna curenta din grafic                   
                    }
                }
            }
            //Setare titlu pentru grafic
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
                //return;    

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

            //Obtine numarul de zile trecute de la inceputul lunii
            int daysPassedFromMonthStart = DateTime.Now.Day;
            hasResetDatePickers = true;//seteaza flagul ca true pt a evita executia metodei asociate date time picker-ului selectat          
            targetPicker.Value = DateTime.Now.AddDays(-daysPassedFromMonthStart + 1);//scade numarul de zile trecute de la inceputul lunii si adauga 1 (pentru ca altfel prima zi a lunii ar deveni 0)

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
            //int selectedIndex = comboBox.SelectedIndex;
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
           
            //Se verfica tipul de selector a carui stare a fost modificata
            if (updateControl == DataUpdateControl.START_PICKER) {
                String tableName = getSelectedTableName(savingAccountComboBox);
                //Daca este bifat controlul de tip checkbox pt interval inseamna ca se doreste selectarea datelor pe mai multe luni
                if (checkBox.Checked == true) {
                    //Selectare optiune pt mai multe luni                  
                    QueryType option = QueryType.MULTIPLE_MONTHS;

                    //Se obține data de început și cea de final in formatul cerut de baza de date MySql
                    String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                    String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);                   

                    //Se configurează obiectul de stocare al datelor si se trimit controllerului tipul de interogare și respectivul obiect                  
                    QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).addTableName(tableName).build(); //CHANGE

                    controller.requestData(option, paramContainer);

                } else {
                    //Altfel, se selecteaza datele pe o singura luna
                    QueryType option = QueryType.SINGLE_MONTH;

                    //Se preiau valorile lunii si anului selectat
                    int month = startPicker.Value.Month;
                    int year = startPicker.Value.Year;

                    //Se configurează obiectul de stocare al datelor si se trimit controllerului tipul de interogare și respectivul obiect                    
                    QueryData paramContainer = new QueryData.Builder(userID).addMonth(month).addYear(year).addTableName(tableName).build(); //CHANGE

                    controller.requestData(option, paramContainer);


                }
            } else if (updateControl == DataUpdateControl.END_PICKER) {
                String tableName = getSelectedTableName(savingAccountComboBox);
                //Selectare date pe mai multe luni daca selectorul este cel pt data de final
                QueryType option = QueryType.MULTIPLE_MONTHS;

                String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);

                QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).addTableName(tableName).build(); //CHANGE
                controller.requestData(option, paramContainer);

            } else if (updateControl == DataUpdateControl.MONTHLY_PICKER) {
                //Se alege interogarea ce insumeaza valorile elementului pt fiecare luna a anului selectat
                QueryType option = QueryType.MONTHLY_TOTALS;

                //La fel ca inainte se preia luna si anul din control
                int month = startPicker.Value.Month;
                int year = startPicker.Value.Year;

                //Se trimit datele               
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
            //Daca e data de final a intervalului se va modifica data astfel incat sa se ia in considerare inclusiv ultima zi din luna
            if (dateType == DateType.END_DATE) {
                //Ia data curenta din date picker la care adauga o luna si scade o zi(pt a obtine data ultimei zile din luna)
                sqlFormatDateString = datePicker.Value.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            } else {
                sqlFormatDateString = datePicker.Value.ToString("yyyy-MM-dd");// daca e data de inceput se va lua doar data din dateTimePicker intrucat aceasta incepe deja de la prima zi a lunii(vezi setarea din designer)
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
