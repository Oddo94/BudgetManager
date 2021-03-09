﻿using BudgetManager.non_mvc;
using System;
using System.Collections;
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
using static BudgetManager.QueryData;

namespace BudgetManager {
    public partial class UserDashboard : Form, IView {     
        private readonly int userID;//ar trebui transformat in variabila statica pt a se pastra valoarea pe toata executia programului(mai ales pt situatia in care utilizatorul navigheaza in alte ferestre si dupa aceea revine la dashboard)       
        private IControl controller = new MainController();
        private IModel model = new BudgetSummaryModel();

        //Lista DateTimePicker care vor fi dezactivate in situatia in care nu exista conexiune la baza de date
        private DateTimePicker[] datePickers = new DateTimePicker[]{};
        private bool hasResetDatePickers = false;

        
        public UserDashboard(int userID, String userName) {
            InitializeComponent();
            this.userID = userID;
            datePickers = new DateTimePicker[] { dateTimePickerStartBS, dateTimePickerEndBS, dateTimePickerStartIncomes, dateTimePickerEndIncomes, dateTimePickerMonthlyIncomes, dateTimePickerStartExpenses, dateTimePickerEndExpenses, dateTimePickerMonthlyExpenses,
                       dateTimePickerStartDebts, dateTimePickerEndDebts, dateTimePickerMonthlyDebts, dateTimePickerStartSavings, dateTimePickerEndSavings, dateTimePickerMonthlySavings};
            this.Text = String.Format("Budget Manager-{0}'s dashboard", userName);
            setDateTimePickerDefaultDate(datePickers);

            wireUp(controller, model);
            

        }

        //Metode generale MVC
        public void disableControls() {
            foreach (DateTimePicker currentPicker in datePickers) {
                currentPicker.Enabled = false;
            }
        }

        public void enableControls() {
            foreach (DateTimePicker currentPicker in datePickers) {
                currentPicker.Enabled = true;
            }
        }

        public void updateView(IModel model) {
            int tabIndex = mainTabControl.SelectedIndex;

            switch (tabIndex) {
                case 0:
                    //Se apeleaza metoda de actualizare a tabului budget summary care actualizeaza la randul ei grid view si pie chart
                    updateBudgetSummaryTab(model);
                    break;

                case 1:
                    updateIncomesTab(model);
                    break;

                case 2:
                    updateExpensesTab(model);
                    break;

                case 3:
                    updateDebtsTab(model);
                    break;

                case 4:
                    updateSavingsTab(model);
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        public void wireUp(IControl paramController, IModel paramModel) {
            //Se verifica daca exista o referintă spre un model
            //In caz afirmativ, prin trecerea la un alt model nu are sens urmarirea vechiului model deci se elimina observerul
            if (model != null) {
                model.removeObserver(this);
            }

            //Stabilire referinte pt model si controller in View
            this.model = paramModel;
            this.controller = paramController;

            //Am inversat apelul metodelor pt a evita NPE
            //Creare legaturi în controller spre noul View si noul Model
            controller.setView(this);
            controller.setModel(model);

            //Inregistrare observer pentru componenta View curenta in model
            model.addObserver(this);
        }

        //Metoda care schimba modelul curent folosit, in momentul in care se selecteaza alt tab
        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e) {
            //Obtinere index pentru pagina curenta
            int tabIndex = mainTabControl.SelectedIndex;

            switch(tabIndex) {
                case 0:
                    model = new BudgetSummaryModel();
                    controller = new MainController();
                    wireUp(controller, model);
                    break;

                case 1:
                    model = new IncomesModel();
                    controller = new MainController();
                    wireUp(controller, model);
                    break;

                case 2:
                    model = new ExpensesModel();
                    controller = new MainController();
                    wireUp(controller, model);
                    break;

                case 3:
                    model = new DebtsModel();
                    controller = new MainController();
                    wireUp(controller, model);
                    break;

                case 4:
                    model = new SavingsModel();
                    controller = new MainController();
                    wireUp(controller, model);
                    break;

                default:
                    break;
            }

        }

        //1.METODE TAB BUDGET SUMMARY
        private void intervalCheckBoxBS_CheckedChanged(object sender, EventArgs e) {
            if (intervalCheckBoxBS.Checked) {
                monthPickerPanelBS.Visible = true;
                startLabelBS.Text = "Starting month";
            } else {
                monthPickerPanelBS.Visible = false;
                startLabelBS.Text = "Month";
            }
           
        }

        private void dateTimePickerStartBS_ValueChanged(object sender, EventArgs e) {          
            String message = "Budget summary";
            //Verifica daca controlul a fost resetat caz in care nu se mai executa din nou intreaga metoda
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelBS, dateTimePickerStartBS, dateTimePickerEndBS, monthPickerPanelBS);
            }

            sendDataToController(DateTimePickerType.START_PICKER, intervalCheckBoxBS, dateTimePickerStartBS, dateTimePickerEndBS);
        }

        private void dateTimePickerEndBS_ValueChanged(object sender, EventArgs e) {
            String message = "Budget summary";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelBS, dateTimePickerStartBS, dateTimePickerEndBS, monthPickerPanelBS);
            }

            sendDataToController(DateTimePickerType.END_PICKER, intervalCheckBoxBS, dateTimePickerStartBS, dateTimePickerEndBS);
        }          

        private void fillDataGridViewBS(DataTable inputDataTable) {
            //dataGridView1.DataSource = dTableInput;

            if (inputDataTable.Rows.Count == 1) {
                int gridViewBSRows = 5;//Numarul total de randuri din tabelul de pe tabul budget summary
                //Populare coloana 2 din tabel cu datele generale legate de buget
                for (int i = 0; i < inputDataTable.Rows[0].ItemArray.Length; i++) {
                    dataGridViewBS.Rows[i].Cells[1].Value = inputDataTable.Rows[0].ItemArray[i];
                }
                //Calculare suma ramasa de cheltuit pt intervalul selectat(datele vin din baza de date pe un singur rand, care are mai multe coloane)
                dataGridViewBS.Rows[gridViewBSRows - 1].Cells[1].Value = calculateAmountLeftToSpend(Array.ConvertAll(inputDataTable.Rows[0].ItemArray, x => x != DBNull.Value ? Convert.ToInt32(x) : 0));

                try {
                    //Transformare valoare venit din String in int
                    int totalIncome = 0;
                    if (inputDataTable.Rows[0].ItemArray[0] != DBNull.Value) {
                        totalIncome = Convert.ToInt32(inputDataTable.Rows[0].ItemArray[0]);
                    } else {
                        clearDataGridColumn(dataGridViewBS, 2);
                        return;
                    }
                    //Calculare valoare procentuala pentru fiecare element din coloana 2 a tabelului, prin raportare la venitul total
                    for (int i = 0; i < gridViewBSRows; i++) {
                        int currentValue = 0;
                        if (dataGridViewBS.Rows[i].Cells[1].Value != DBNull.Value) {
                            currentValue = Convert.ToInt32(dataGridViewBS.Rows[i].Cells[1].Value);
                        }
                        //Introducere valori procentuale in coloana 3 a tabelului
                        dataGridViewBS.Rows[i].Cells[2].Value = String.Format("{0}%", ViewCalculator.calculatePercentage(currentValue, totalIncome));
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        
        //Metoda speciala pt popularea cu date a graficului din tabul Budget summary
        private void fillPieChartBS(DataTable inputDataTable) {
            String[] pointNames = new String[] { "Expenses", "Debts", "Savings", "Amount left" };
            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {               
                pieChartBS.Series[0].Points.Clear();
                int[] dTableValues = Array.ConvertAll(inputDataTable.Rows[0].ItemArray, x => x != DBNull.Value ? Convert.ToInt32(x) : 0);

                int expenses = dTableValues[1];
                int debts = dTableValues[2];
                int savings = dTableValues[3];



                int totalAmount = dTableValues[0];
                int amountLeft = totalAmount - (expenses + debts + savings);

                int[] finalPieChartValues = new int[] { expenses, debts, savings, amountLeft };

                //Adaugare puncte pe grafic
                for (int i = 0; i < pointNames.Length; i++) {
                    //Daca valoarea totala a veniturilor e mai mica sau egala cu zero nu se mai afiseaza nimic in grafic
                    if (totalAmount <= 0) {
                        return;
                    }
                    //Daca valoarea curenta este mai mica sau egala cu zero nu se mai adauga in grafic si se trece la urmatorul element
                    if (finalPieChartValues[i] <= 0) {
                        continue;
                    }
                    //Problema apare la points deoarece atunci cand se sare peste un element care are valoarea 0 nu se mai adauga niciun punct iar la urmatoarea executie a buclei
                    //i-ul are o valoare mai mare decat numarul maxim de puncte din colectia Points              
                    pieChartBS.Series[0].Points.AddXY(pointNames[i], finalPieChartValues[i]);
                    //Se selecteaza ultimul punct(cel mai recent) din lista de puncte existente in grafic și se adauga ca eticheta valoarea corespunzatoare din sirul de String-uri
                    int lastPointIndex = pieChartBS.Series[0].Points.Count - 1;
                    pieChartBS.Series[0].Points[lastPointIndex].LegendText = pointNames[i];

                    //Calculare procentaj pt fiecare tip prezent prin raportare la valoarea totala calculata anterior                  
                    int percentage = ViewCalculator.calculatePercentage(finalPieChartValues[i], totalAmount);
                    if (percentage > 0) {
                        //Daca procentajul e mai mare ca 0 se adauga valoarea respectiva ca eticheta                     
                        pieChartBS.Series[0].Points[lastPointIndex].Label = percentage + "%";
                    } else {
                        //Daca procentajul e zero nu se adauga nimic in textul etichetei
                        pieChartBS.Series[0].Points[lastPointIndex].Label = " ";
                    }
                }
            }
        }

        //Metoda generala de actualizare a componentelor din tabul Budget summary(apeleaza metodele specifice fiecarei componente)
        private void updateBudgetSummaryTab(IModel model) {
            fillDataGridViewBS(model.DataSources[0]);
            fillPieChartBS(model.DataSources[0]);
        }


        //2.METODE TAB INCOMES
        private void intervalCheckBoxIncomes_CheckedChanged(object sender, EventArgs e) {
            setEndPickerPanelVisibility(intervalCheckBoxIncomes, monthPickerPanelIncomes, infoLabelIncomes);
        }

        private void dateTimePickerStartIncomes_ValueChanged(object sender, EventArgs e) {
            String message = "Incomes list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes, monthPickerPanelIncomes);

            }

            sendDataToController(DateTimePickerType.START_PICKER, intervalCheckBoxIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes);

        }

        private void dateTimePickerEndIncomes_ValueChanged(object sender, EventArgs e) {
            String message = "Incomes list";
            
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes, monthPickerPanelIncomes);
            }

            sendDataToController(DateTimePickerType.END_PICKER, intervalCheckBoxIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes);

        }

        private void dateTimePickerMonthlyIncomes_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DateTimePickerType.MONTHLY_PICKER, intervalCheckBoxIncomes, dateTimePickerMonthlyIncomes, dateTimePickerEndIncomes);
        }

        //Metoda generala de actualizare a tabului Incomes(apeleaza metodele specifice de actualizare pentru fiecare componenta)
        private void updateIncomesTab(IModel model) {
            String[] typeNames = new String[] { "Active income", "Passive income" };
            String title = "Monthly income evolution";
            int currentYear = dateTimePickerMonthlyIncomes.Value.Year;
           
            DataTable[] results = model.DataSources;
            fillDataGridView(dataGridViewIncomes, (DataTable) results[0]);
            fillPieChart(pieChartIncomes, (DataTable)results[1], typeNames);          
            fillColumnChart(columnChartIncomes, (DataTable)results[2], currentYear, title);
            
            
        }

        //3.METODE TAB EXPENSES
        private void intervalCheckBoxExpenses_CheckedChanged(object sender, EventArgs e) {
            if (intervalCheckBoxExpenses.Checked) {
                monthPickerPanelExpenses.Visible = true;
                startLabelExpenses.Text = "Starting month";
            } else {
                monthPickerPanelExpenses.Visible = false;
                startLabelExpenses.Text = "Month";
            }
        }

    
        private void dateTimePickerStartExpenses_ValueChanged(object sender, EventArgs e) {
            String message = "Expenses list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses, monthPickerPanelExpenses);
            }

            sendDataToController(DateTimePickerType.START_PICKER, intervalCheckBoxExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses);
        }

        private void dateTimePickerEndExpenses_ValueChanged(object sender, EventArgs e) {
            String message = "Expenses list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses, monthPickerPanelExpenses);
            }

            sendDataToController(DateTimePickerType.END_PICKER, intervalCheckBoxExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses);
        }

        private void dateTimePickerMonthlyExpenses_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DateTimePickerType.MONTHLY_PICKER, intervalCheckBoxExpenses, dateTimePickerMonthlyExpenses, dateTimePickerEndExpenses);
        }


        private void updateExpensesTab(IModel model) {
            String[] typeNames = new String[] { "Fixed expenses", "Periodic expenses", "Variable expenses" };
            String title = "Monthly expense evolution";
            int currentYear = dateTimePickerMonthlyExpenses.Value.Year;
       
            fillDataGridView(dataGridViewExpenses, model.DataSources[0]);
            fillPieChart(pieChartExpenses, model.DataSources[1], typeNames);
            fillColumnChart(columnChartExpenses, model.DataSources[2], currentYear, title);

        }


        //4.METODE TAB DEBTS
        private void intervalCheckBoxDebts_CheckedChanged(object sender, EventArgs e) {
            if (intervalCheckBoxDebts.Checked) {
                monthPickerPanelDebts.Visible = true;
                startLabelDebts.Text = "Starting month";
            } else {
                monthPickerPanelDebts.Visible = false;
                startLabelDebts.Text = "Month";
            }
        }

        private void dateTimePickerStartDebts_ValueChanged(object sender, EventArgs e) {
            String message = "Debts list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelDebts, dateTimePickerStartDebts, dateTimePickerEndDebts, monthPickerPanelDebts);
            }

            sendDataToController(DateTimePickerType.START_PICKER, intervalCheckBoxDebts, dateTimePickerStartDebts, dateTimePickerEndDebts);

        }

        private void dateTimePickerEndDebts_ValueChanged(object sender, EventArgs e) {
            String message = "Debts list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelExpenses, dateTimePickerStartDebts, dateTimePickerEndDebts, monthPickerPanelDebts);
            }

            sendDataToController(DateTimePickerType.END_PICKER, intervalCheckBoxDebts, dateTimePickerStartDebts, dateTimePickerEndDebts);

        }

        private void dateTimePickerMonthlyDebts_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DateTimePickerType.MONTHLY_PICKER, intervalCheckBoxDebts, dateTimePickerMonthlyDebts, dateTimePickerEndDebts);
        }

        private void updateDebtsTab(IModel model) {
            String title = "Monthly debts";
            int currentYear = dateTimePickerMonthlyDebts.Value.Year;
          
            fillDataGridView(dataGridViewDebts, model.DataSources[0]);
            fillPieChartWithVariablePoints(pieChartDebts, model.DataSources[1]);
            fillColumnChart(columnChartDebts, model.DataSources[2], currentYear, title);
        }


        //METODE TAB SAVINGS
        private void intervalCheckBoxSavings_CheckedChanged(object sender, EventArgs e) {     
            if (intervalCheckBoxSavings.Checked) {
                monthPickerPanelSavings.Visible = true;
                startLabelSavings.Text = "Starting month";
            } else {
                monthPickerPanelSavings.Visible = false;
                startLabelSavings.Text = "Month";
            }
        }


        private void dateTimePickerStartSavings_ValueChanged(object sender, EventArgs e) {        
            String message = "Savings list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelSavings, dateTimePickerStartSavings, dateTimePickerEndSavings, monthPickerPanelSavings);
            }

            sendDataToController(DateTimePickerType.START_PICKER, intervalCheckBoxSavings, dateTimePickerStartSavings, dateTimePickerEndSavings);
        }

        private void dateTimePickerEndSavings_ValueChanged(object sender, EventArgs e) {
            String message = "Savings list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelSavings, dateTimePickerStartSavings, dateTimePickerEndSavings, monthPickerPanelSavings);
            }

            sendDataToController(DateTimePickerType.END_PICKER, intervalCheckBoxSavings, dateTimePickerStartSavings, dateTimePickerEndSavings);
        }

        private void dateTimePickerMonthlySavings_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DateTimePickerType.MONTHLY_PICKER, intervalCheckBoxSavings, dateTimePickerMonthlySavings, dateTimePickerEndSavings);
        }

        private void updateSavingsTab(IModel model) {
            String title = "Monthly savings";
            String[] typeNames = new String[] { "Total savings", "Total incomes"};
            int currentYear = dateTimePickerMonthlySavings.Value.Year;

            fillDataGridView(dataGridViewSavings, model.DataSources[0]);
            fillPieChart(pieChartSavings, model.DataSources[1], typeNames);
            fillColumnChart(columnChartSavings, model.DataSources[2], currentYear, title);
        }



        //Metoda generica pt populare DataGridView
        private void fillDataGridView(DataGridView gridView, DataTable inputDataTable) {
            //Impune sursa de date primita ca argument obiectului de tip DataGridView primit ca argument
            gridView.DataSource = inputDataTable;
        }

        //Metoda generica pt populare grafice de tip ColumnChart
        private void fillColumnChart(Chart chart, DataTable dTable, int currentYear, String title) {
            //Eliminare puncte din grafic
            chart.Series[0].Points.Clear();

            if (dTable != null && dTable.Rows.Count > 0) {                              
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
                    if (j > dTable.Rows.Count - 1) {
                        chart.Series[0].Points[i].SetValueY(0);
                        continue;
                    }

                    //Se creaza un obiect de tip DateTime pt valoarea X a punctului curent al graficului
                    System.DateTime currentPointDate = System.DateTime.FromOADate(chart.Series[0].Points[i].XValue);
                    //Se extrage valoarea lunii din obiectul de tip DateTime creat anterior
                    int currentChartMonth = Convert.ToInt32(DateTime.ParseExact(currentPointDate.ToString("MMM"), "MMM", CultureInfo.CurrentCulture).Month);
                    int currentDataTableMonth = Convert.ToInt32(dTable.Rows[j].ItemArray[0]);

                    //Se compara daca luna punctului curent din grafic este egala cu luna din tabelul de date provenit din DB
                    if (currentChartMonth == currentDataTableMonth) {
                        int currentDataTableMonthValue = Convert.ToInt32(dTable.Rows[j].ItemArray[1]);
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

        
        //Metoda generala de populare a graficelor de tip PieChart
        private void fillPieChart(Chart chart, DataTable dTable, String[] typeNames) {
            //Eliminare conținut anterior din grafic
            chart.Series[0].Points.Clear();

            //Verificarea continutului obiectului DataTable
            if (dTable != null && dTable.Rows.Count > 0) {                
                //Se transforma sirul de obiecte de pe randul avand inThis dexul zero in 
                int[] itemTypeSums = Array.ConvertAll(dTable.Rows[0].ItemArray, x => x != DBNull.Value ? Convert.ToInt32(x) : 0);

                //Daca valoarea sirurilor nu e egala nu se va popula graficul cu valori
                if (typeNames.Length != itemTypeSums.Length) {
                    return;
                }

                //
                int totalAmount = ViewCalculator.calculateSum(itemTypeSums);

                //Adaugare puncte pe grafic
                for (int i = 0; i < typeNames.Length; i++) {
                    chart.Series[0].Points.AddXY(typeNames[i], itemTypeSums[i]);
                    chart.Series[0].Points[i].LegendText = typeNames[i];

                    //Calculare procentaj pt fiecare tip prezent prin raportare la valoarea totala calculata anterior
                    int percentage = ViewCalculator.calculatePercentage(itemTypeSums[i], totalAmount);
                    if (percentage > 0) {
                        //Adaugare eticheta pentru elementul curent
                        chart.Series[0].Points[i].Label = percentage + "%";
                    } else {
                        chart.Series[0].Points[i].Label = " ";//daca procentajul e zero nu se adauga nimic in textul etichetei
                    }
                }
            }
        }

        //Metoda pentru popularea graficelor de tip PieChart a caror numar de puncte este variabil
        private void fillPieChartWithVariablePoints(Chart chart, DataTable inputDataTable) {
            //Eliminare date existente in grafic 
            chart.Series[0].Points.Clear();

            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                //Transforma ArrayList in int[] iar daca rezultatul returnat de metoda e null creaza un sir int[] gol
                int[] debtValues = getColumnData(inputDataTable, 1) != null ? getColumnData(inputDataTable, 1).ToArray() : new int[]{ };

                int totalAmount = ViewCalculator.calculateSum(debtValues);

                //Adaugare puncte pe grafic
                for (int i = 0; i < inputDataTable.Rows.Count; i++) {
                    //Preluare nume creditor si valoare datorie de pe randul curent
                    String currentCreditorName = inputDataTable.Rows[i].ItemArray[0].ToString();
                    Object debtValue = inputDataTable.Rows[i].ItemArray[1];
                    //Transformare valoare curentă datroie din obiect in valoare primitiva de tip int
                    int debtValueForCurrentCreditor = debtValue != DBNull.Value ? Convert.ToInt32(debtValue) : 0;

                    //Adaugare punct pe grafic cu valorile extrase anterior
                    chart.Series[0].Points.AddXY(currentCreditorName, debtValueForCurrentCreditor);
                    chart.Series[0].Points[i].LegendText = currentCreditorName;

                    //Calculare procentaj datorie din valoarea totala a datoriilor
                    int percentage = ViewCalculator.calculatePercentage(debtValueForCurrentCreditor, totalAmount);

                    //Adaugare eticheta pentru punctul curent
                    if (percentage > 0) {
                        chart.Series[0].Points[i].Label = percentage + "%";
                    } else {
                        chart.Series[0].Points[i].Label = " ";//daca procentajul e zero nu se adauga nimic in textul etichetei
                    }
                }
            }
        }

        private int calculateAmountLeftToSpend(int[] inputArray) {
            if (inputArray == null) {
                return -1;
            }

            int total = inputArray[0];

            if (total == 0) {
                return 0;
            }

            int sum = 0;
            for (int i = 1; i < inputArray.Length; i++) {
                sum += inputArray[i];
            }

            int amountLeftToSpend = total - sum;

            return amountLeftToSpend > 0 ? amountLeftToSpend : 0;
        }


        private void clearDataGridColumn(DataGridView dataGrid, int columnNumber) {
            if (columnNumber < 0 || columnNumber > dataGrid.Rows[0].Cells.Count - 1) {
                return;
            }

            for (int i = 0; i < dataGrid.RowCount; i++) {
                dataGrid.Rows[i].Cells[columnNumber].Value = "";
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

    

        //Metoda generica pentru afisarea mesajului legat de intervalul de timp selectat(labelul de deasupra tabelelor din taburi)
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
                resetDateTimePicker(startPicker);
                resetDateTimePicker(endPicker);

                MessageBox.Show("Invalid date selection!", "Warning");
            
            } else if (dateTimePickerContainer.Visible == false) {
                targetLabel.Text = String.Format("{0} for {1} {2}", message, startPicker.Value.ToString("MMMM"), startPicker.Value.ToString("yyyy"));
                //return;    

            }
        }

        //Metoda pentru verificarea corectitudinii datei de inceput/sfarsit a intervalului selectat
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

      

        private void UserDashboard_Load(object sender, EventArgs e) {
            infoLabelBS.Text = String.Format("Budget summary for {0} {1}", dateTimePickerStartBS.Value.ToString("MMMM"), dateTimePickerStartBS.Value.ToString("yyyy"));
            //Adaugare denumiri elemnte buget in coloana 1 a tabelului din tabul Budget summary
            dataGridViewBS.Rows.Add(5);
            dataGridViewBS[0, 0].Value = "Incomes";
            dataGridViewBS[0, 1].Value = "Expenses";
            dataGridViewBS[0, 2].Value = "Debts";
            dataGridViewBS[0, 3].Value = "Savings";
            dataGridViewBS[0, 4].Value = "Left to spend";
        }


        private void UserDashboard_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }

        private String getDateStringInSQLFormat(DateTimePicker datePicker, DateType dateType) {
            if(datePicker == null) {
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

        
        private void resetDateTimePicker(DateTimePicker targetPicker) {
            if(targetPicker == null) {
                return;
            }

            //Obtine numarul de zile trecute de la inceputul lunii
            int daysPassedFromMonthStart = DateTime.Now.Day;
            hasResetDatePickers = true;//seteaza flagul ca true pt a evita executia metodei asociate date time picker-ului selectat          
            targetPicker.Value = DateTime.Now.AddDays(-daysPassedFromMonthStart + 1);//scade numarul de zile trecute de la inceputul lunii si adauga 1 (pentru ca altfel prima zi a lunii ar deveni 0)

        }

        //Metoda generica pentru trimiterea datelor catre Controller
        private void sendDataToController(DateTimePickerType pickerType, CheckBox checkBox, DateTimePicker startPicker, DateTimePicker endPicker) {
            //Se verfica tipul de selector a carui stare a fost modificata
            if (pickerType == DateTimePickerType.START_PICKER) {
                //Daca este bifat controlul de tip checkbox pt interval inseamna ca se doreste selectarea datelor pe mai multe luni
                if (checkBox.Checked == true) {
                    //Selectare optiune pt mai multe luni                  
                    QueryType option = QueryType.MULTIPLE_MONTHS;

                    //Se obține data de început și cea de final in formatul cerut de baza de date MySql
                    String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                    String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);

                    //Se configurează obiectul de stocare al datelor si se trimit controllerului tipul de interogare și respectivul obiect
                    //QueryData paramContainer = new QueryData(userID, startDate, endDate);
                    QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).build(); //CHANGE

                    controller.requestData(option, paramContainer);
                  
                } else {
                    //Altfel, se selecteaza datele pe o singura luna
                    QueryType option = QueryType.SINGLE_MONTH;

                    //Se preiau valorile lunii si anului selectat
                    int month = startPicker.Value.Month;
                    int year = startPicker.Value.Year;

                    //Se configurează obiectul de stocare al datelor si se trimit controllerului tipul de interogare și respectivul obiect
                    //QueryData paramContainer = new QueryData(userID, month, year);
                    QueryData paramContainer = new QueryData.Builder(userID).addMonth(month).addYear(year).build(); //CHANGE
                    
                    controller.requestData(option, paramContainer);


                }
            } else if (pickerType == DateTimePickerType.END_PICKER) {
                //Selectare date pe mai multe luni daca selectorul este cel pt data de final
                QueryType option = QueryType.MULTIPLE_MONTHS;

                String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);

                //QueryData paramContainer = new QueryData(userID, startDate, endDate);
                QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).build(); //CHANGE
                controller.requestData(option, paramContainer);

            } else if(pickerType == DateTimePickerType.MONTHLY_PICKER) {
                //Se alege interogarea ce insumeaza valorile elementului pt fiecare luna a anului selectat
                QueryType option = QueryType.MONTHLY_TOTALS;

                //La fel ca inainte se preia luna si anul din control
                int month = startPicker.Value.Month;
                int year = startPicker.Value.Year;

                //Se trimit datele
                //QueryData paramContainerTest = new QueryData(userID, month, year);
                QueryData paramContainerTest = new QueryData.Builder(userID).addMonth(month).addYear(year).build(); //CHANGE
                controller.requestData(option, paramContainerTest);
            }
        }

        private List<int> getColumnData(DataTable inputDataTable, int columnIndex) {
            //Verifica daca obiectul DataTable contine valori si nu e nul
            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                List<int> columnData = new List<int>();

                //Preia datele de pe coloana specificata
                for (int i = 0; i < inputDataTable.Rows.Count; i++) {
                    //Adaugare date din randul curent si coloana specificata in ArrayList 
                    Object currentValue = inputDataTable.Rows[i][columnIndex];
                    //Transforma obiectele prezente in valori de tip int(daca vreunul dintre aceste obiecte este nul impune valoarea implicita 0) adaugandu-le in lista
                    columnData.Add(currentValue != DBNull.Value ? Convert.ToInt32(currentValue) : 0);
                    
                }
                //Returneaza lista obținuta
                return columnData;              
            }
            //Returneaza null daca obietul de tip DataTable nu contine date
            return null;
        }

        private void updateDataToolStripMenuItem_Click(object sender, EventArgs e) {
            new UpdateUserDataForm(userID).ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Visible = false;
            new LoginForm().Visible = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void insertDataToolStripMenuItem_Click(object sender, EventArgs e) {
            //Se foloseste metoda "ShowDialog()" pentru a nu permite utilizarea ferestrei din fundal cat timp cea selectata este deschisa; de asemenea previne deschiderea mai multor instante ale aceleasi ferestre
            new InsertDataForm(userID).ShowDialog();
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
                hasResetDatePickers = true;

                //Seteaza data reprezentand prima zi a lunii curente a anului curent in DateTimePicker
                currentPicker.Value = new DateTime(year, month, day);
            }
        }

        private void createPlanToolStripMenuItem_Click(object sender, EventArgs e) {
            new BudgetPlanCreator(userID).ShowDialog();
        }
    }
 }

