using BudgetManager.mvc.views;
using BudgetManager.mvp.views;
using BudgetManager.non_mvc;
using BudgetManager.utils;
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
    public enum ApplicationCloseMode {
        EXIT,
        LOGOUT,
        UNDEFINED
    }

    public partial class UserDashboard : Form, IView {
        private readonly int userID;
        private IControl controller = new MainController();
        private IModel model = new BudgetSummaryModel();
        private DataGridViewManager gridViewManager;
        //private LoginForm loginForm;

        //The list that contains the DateTimePicker objects that will be deactivated when there's no conection to the database
        private DateTimePicker[] datePickers = new DateTimePicker[] { };
        private bool hasResetDatePickers = false;
        private bool userAgreedToExit = false;
        private bool hasRestartedApplication = false;


        public UserDashboard(int userID, String userName) {
            InitializeComponent();
            this.userID = userID;
            //this.loginForm = loginForm;
            datePickers = new DateTimePicker[] { dateTimePickerStartBS, dateTimePickerEndBS, dateTimePickerStartIncomes, dateTimePickerEndIncomes, dateTimePickerMonthlyIncomes, dateTimePickerStartExpenses, dateTimePickerEndExpenses, dateTimePickerMonthlyExpenses,
                       dateTimePickerStartDebts, dateTimePickerEndDebts, dateTimePickerMonthlyDebts, dateTimePickerStartSavings, dateTimePickerEndSavings, dateTimePickerMonthlySavings};
            this.Text = String.Format("Budget Manager-{0}'s dashboard", userName);
            setDateTimePickerDefaultDate(datePickers);

            //Configuring DataGridViewManager object
            //It initially receives the dataGridViewIncomes object and later this can be changed as needed through the setDataGridView() method
            gridViewManager = new utils.DataGridViewManager(dataGridViewIncomes);

            wireUp(controller, model);


        }

        //General MVC pattern methods
        public void disableControls() {
            foreach (DateTimePicker currentPicker in datePickers) {
                currentPicker.Enabled = false;
            }
            //Disables the newly added button for accessing the saving account window
            savingAccountButton.Enabled = false;
        }

        public void enableControls() {
            foreach (DateTimePicker currentPicker in datePickers) {
                currentPicker.Enabled = true;
            }
            //Enables the newly added button for accessing the saving account window
            savingAccountButton.Enabled = true;
        }

        public void updateView(IModel model) {
            int tabIndex = mainTabControl.SelectedIndex;

            switch (tabIndex) {
                case 0:
                    //Calls the update method of the budget summary tab which in turn updates the data grid view and the pie chart of this tab
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
            //Checks if there's an active refernce to a model object
            //If this is the case there's no point in monitoring the old model so the observer reference is removed
            if (model != null) {
                model.removeObserver(this);
            }

            //Sets up the references to the model and controller in this View
            this.model = paramModel;
            this.controller = paramController;


            //Sets up the links inside the controller to the new View and the new Model
            //Changes the order in which the two methods are called to avoid NPE
            controller.setView(this);
            controller.setModel(model);

            //Adds the current View to the model object(the view will be notified of any changes performed to the state of the model)
            model.addObserver(this);
        }

        //The method that changes the currently used model when a different tab is selected
        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e) {
            //Getting the index of the currently selected tab
            int tabIndex = mainTabControl.SelectedIndex;

            switch (tabIndex) {
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


        private void createPlanToolStripMenuItem_Click(object sender, EventArgs e) {
            new BudgetPlanCreator(userID).ShowDialog();
        }

        private void editDeleteExistingPlansToolStripMenuItem_Click(object sender, EventArgs e) {
            new BudgetPlanManagementForm(userID).ShowDialog();
        }
        //Control method for the button that displays the aving account manager window
        private void savingAccountButton_Click(object sender, EventArgs e) {
            new SavingAccountForm(userID).ShowDialog();
        }

        private void UserDashboard_FormClosing(object sender, FormClosingEventArgs e) {
            //The exit message is displayed only if the application hasn't been restarted (that takes place when the user selects the logout option from the toolstrip menu)
            if (!hasRestartedApplication) {
                if (userAgreedToExit) {
                    return;
                }

                DialogResult userOption = displayApplicationCloseMessage(ApplicationCloseMode.EXIT);

                if (userOption == DialogResult.Yes) {
                    userAgreedToExit = true;
                    //hasRequestedExitFromToolStripOption = true;
                    Application.Exit();
                } else {
                    //Cancels the form closing event if the user selects the "No" option
                    e.Cancel = true;
                }
            }
        }

        //1.BUDGET SUMMARY METHODS
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
            //Checks if the DateTimePicker was reset and if so it will not execute the event handling code
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelBS, dateTimePickerStartBS, dateTimePickerEndBS, monthPickerPanelBS);
            }

            sendDataToController(DataUpdateControl.START_PICKER, intervalCheckBoxBS, dateTimePickerStartBS, dateTimePickerEndBS);
        }

        private void dateTimePickerEndBS_ValueChanged(object sender, EventArgs e) {
            String message = "Budget summary";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelBS, dateTimePickerStartBS, dateTimePickerEndBS, monthPickerPanelBS);
            }

            sendDataToController(DataUpdateControl.END_PICKER, intervalCheckBoxBS, dateTimePickerStartBS, dateTimePickerEndBS);
        }

        private void fillDataGridViewBS(DataTable inputDataTable) {
            if (inputDataTable.Rows.Count == 1) {
                int gridViewBSRows = 5;//The total number of rows contained by the table present in the Budget summary tab               
                //Filling the second column with the general budget data
                for (int i = 0; i < inputDataTable.Rows[0].ItemArray.Length; i++) {
                    dataGridViewBS.Rows[i].Cells[1].Value = inputDataTable.Rows[0].ItemArray[i];
                }
                //Calculating the amount left to spend for the selected interval(data is retrieved from the DB on a single row which contains multiple columns)
                dataGridViewBS.Rows[gridViewBSRows - 1].Cells[1].Value = calculateAmountLeftToSpend(Array.ConvertAll(inputDataTable.Rows[0].ItemArray, x => x != DBNull.Value ? Convert.ToInt32(x) : 0));

                try {
                    //Converting the income value from String to int
                    int totalIncome = 0;
                    if (inputDataTable.Rows[0].ItemArray[0] != DBNull.Value) {
                        totalIncome = Convert.ToInt32(inputDataTable.Rows[0].ItemArray[0]);
                    } else {
                        clearDataGridColumn(dataGridViewBS, 2);
                        return;
                    }
                    //Calculating the percentage value of each element from the second column of the table based on the total income value
                    for (int i = 0; i < gridViewBSRows; i++) {
                        int currentValue = 0;
                        if (dataGridViewBS.Rows[i].Cells[1].Value != DBNull.Value) {
                            currentValue = Convert.ToInt32(dataGridViewBS.Rows[i].Cells[1].Value);
                        }
                        //Inserting the percentage values into the third column of the table
                        dataGridViewBS.Rows[i].Cells[2].Value = String.Format("{0}%", ViewCalculator.calculatePercentage(currentValue, totalIncome));
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Special method for populating the Budget summary pie chart with data
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

                //Adding points on the pie chart
                for (int i = 0; i < pointNames.Length; i++) {
                    //If the total value of incomes is lower than or equal to zero then nothing is diplayed on the pie chart 
                    if (totalAmount <= 0) {
                        return;
                    }

                    //If the current value is lower than or equal to zero then it will not be added to the pie chart and the next element will be processed
                    if (finalPieChartValues[i] <= 0) {
                        continue;
                    }

                    //Check out the index value used to acces data from the Points collection in case of issues              
                    pieChartBS.Series[0].Points.AddXY(pointNames[i], finalPieChartValues[i]);
                    //Selects the most recent point(latest) from the list of points that are present in the pie chart and adds the corresponding label from the String array
                    int lastPointIndex = pieChartBS.Series[0].Points.Count - 1;
                    pieChartBS.Series[0].Points[lastPointIndex].LegendText = pointNames[i];

                    //Calculates the percentage value of each type based on the previously calculated total value                
                    double percentage = ViewCalculator.calculatePercentage(finalPieChartValues[i], totalAmount);
                    if (percentage > 0) {
                        //If the percentage value is greater than 0 the respective avalue is added to the pie chart                     
                        pieChartBS.Series[0].Points[lastPointIndex].Label = percentage + "%";
                    } else {
                        //If the percentage is zero then nothing is added to label text
                        pieChartBS.Series[0].Points[lastPointIndex].Label = " ";
                    }
                }
            }
        }

        //General method for updating the components that are present in the Budget summary tab(it calls the specific methods for updating each component)
        private void updateBudgetSummaryTab(IModel model) {
            fillDataGridViewBS(model.DataSources[0]);
            fillPieChartBS(model.DataSources[0]);
        }


        //2.INCOMES TAB METHODS
        private void intervalCheckBoxIncomes_CheckedChanged(object sender, EventArgs e) {
            setEndPickerPanelVisibility(intervalCheckBoxIncomes, monthPickerPanelIncomes, startLabelIncomes);
        }

        private void dateTimePickerStartIncomes_ValueChanged(object sender, EventArgs e) {
            String message = "Incomes list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes, monthPickerPanelIncomes);

            }

            sendDataToController(DataUpdateControl.START_PICKER, intervalCheckBoxIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes);

        }

        private void dateTimePickerEndIncomes_ValueChanged(object sender, EventArgs e) {
            String message = "Incomes list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes, monthPickerPanelIncomes);
            }

            sendDataToController(DataUpdateControl.END_PICKER, intervalCheckBoxIncomes, dateTimePickerStartIncomes, dateTimePickerEndIncomes);

        }

        private void dateTimePickerMonthlyIncomes_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DataUpdateControl.MONTHLY_PICKER, intervalCheckBoxIncomes, dateTimePickerMonthlyIncomes, dateTimePickerEndIncomes);
        }

        //Method for displaying the income column chart column values(month and income value) on mouse hover
        private void columnChartIncomes_MouseHover(object sender, EventArgs e) {
            //Sets the date format of the value returned from the X axis of the column chart(#VALX) to display only the month
            //Retrieves the value of the Y axis(#VALY) which represents the total sum of incomes
            columnChartIncomes.Series[0].ToolTip = String.Format("Total incomes for {0}: {1}", "#VALX{MMMM}", "#VALY");

        }

        //General method for updating the components that are present in the Incomea tab(it calls the specific methods for updating each component)
        private void updateIncomesTab(IModel model) {
            String[] typeNames = new String[] { "Active income", "Passive income" };
            String title = "Monthly income evolution";
            int currentYear = dateTimePickerMonthlyIncomes.Value.Year;

            gridViewManager.setDataGridView(dataGridViewIncomes);
            gridViewManager.fillDataGridView(model.DataSources[0]);

            //Displays the number of results found after executing the query for the specified time interval
            displayDgvResultsCount(displayedIncomesCountLabel, model.DataSources[0]);

            fillDynamicTypePieChart(pieChartIncomes, model.DataSources[1]);
            fillColumnChart(columnChartIncomes, model.DataSources[2], currentYear, title);


        }

        //3.EXPENSES TAB METHODS
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

            sendDataToController(DataUpdateControl.START_PICKER, intervalCheckBoxExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses);
        }

        private void dateTimePickerEndExpenses_ValueChanged(object sender, EventArgs e) {
            String message = "Expenses list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses, monthPickerPanelExpenses);
            }

            sendDataToController(DataUpdateControl.END_PICKER, intervalCheckBoxExpenses, dateTimePickerStartExpenses, dateTimePickerEndExpenses);
        }

        private void dateTimePickerMonthlyExpenses_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DataUpdateControl.MONTHLY_PICKER, intervalCheckBoxExpenses, dateTimePickerMonthlyExpenses, dateTimePickerEndExpenses);
        }

        private void columnChartExpenses_MouseHover(object sender, EventArgs e) {

            columnChartExpenses.Series[0].ToolTip = String.Format("Total expenses for {0}: {1}", "#VALX{MMMM}", "#VALY");

        }


        private void updateExpensesTab(IModel model) {
            String[] typeNames = new String[] { "Fixed expenses", "Periodic expenses", "Variable expenses" };
            String title = "Monthly expense evolution";
            int currentYear = dateTimePickerMonthlyExpenses.Value.Year;

            //Sets the DataGridView object of the gridViewManager object and fills it with the provided data
            gridViewManager.setDataGridView(dataGridViewExpenses);
            gridViewManager.fillDataGridView(model.DataSources[0]);

            //Displays the number of results found after executing the query for the specified time interval
            displayDgvResultsCount(displayedExpensesCountLabel, model.DataSources[0]);

            fillDynamicTypePieChart(pieChartExpenses, model.DataSources[1]);
            fillColumnChart(columnChartExpenses, model.DataSources[2], currentYear, title);

        }


        //4.DEBTS TAB METHODS
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

            sendDataToController(DataUpdateControl.START_PICKER, intervalCheckBoxDebts, dateTimePickerStartDebts, dateTimePickerEndDebts);

        }

        private void dateTimePickerEndDebts_ValueChanged(object sender, EventArgs e) {
            String message = "Debts list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelExpenses, dateTimePickerStartDebts, dateTimePickerEndDebts, monthPickerPanelDebts);
            }

            sendDataToController(DataUpdateControl.END_PICKER, intervalCheckBoxDebts, dateTimePickerStartDebts, dateTimePickerEndDebts);

        }

        private void dateTimePickerMonthlyDebts_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DataUpdateControl.MONTHLY_PICKER, intervalCheckBoxDebts, dateTimePickerMonthlyDebts, dateTimePickerEndDebts);
        }

        private void columnChartDebts_MouseHover(object sender, EventArgs e) {

            columnChartDebts.Series[0].ToolTip = String.Format("Total debts for {0}: {1}", "#VALX{MMMM}", "#VALY");
        }

        private void updateDebtsTab(IModel model) {
            String title = "Monthly debts";
            int currentYear = dateTimePickerMonthlyDebts.Value.Year;

            gridViewManager.setDataGridView(dataGridViewDebts);
            gridViewManager.fillDataGridView(model.DataSources[0]);

            //Displays the number of results found after executing the query for the specified time interval
            displayDgvResultsCount(displayedDebtsCountLabel, model.DataSources[0]);

            fillPieChartWithVariablePoints(pieChartDebts, model.DataSources[1]);
            fillColumnChart(columnChartDebts, model.DataSources[2], currentYear, title);
        }


        //SAVINGS TAB METHODS
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

            sendDataToController(DataUpdateControl.START_PICKER, intervalCheckBoxSavings, dateTimePickerStartSavings, dateTimePickerEndSavings);
        }

        private void dateTimePickerEndSavings_ValueChanged(object sender, EventArgs e) {
            String message = "Savings list";

            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            } else {
                setMonthInfoSelectionMessage(message, infoLabelSavings, dateTimePickerStartSavings, dateTimePickerEndSavings, monthPickerPanelSavings);
            }

            sendDataToController(DataUpdateControl.END_PICKER, intervalCheckBoxSavings, dateTimePickerStartSavings, dateTimePickerEndSavings);
        }

        private void dateTimePickerMonthlySavings_ValueChanged(object sender, EventArgs e) {
            if (hasResetDatePickers) {
                hasResetDatePickers = false;
                return;
            }

            sendDataToController(DataUpdateControl.MONTHLY_PICKER, intervalCheckBoxSavings, dateTimePickerMonthlySavings, dateTimePickerEndSavings);
        }

        private void columnChartSavings_MouseHover(object sender, EventArgs e) {

            columnChartSavings.Series[0].ToolTip = String.Format("Total savings for {0}: {1}", "#VALX{MMMM}", "#VALY");
        }

        private void updateSavingsTab(IModel model) {
            String title = "Monthly savings";
            String[] typeNames = new String[] { "Total savings", "Total incomes" };
            int currentYear = dateTimePickerMonthlySavings.Value.Year;

            gridViewManager.setDataGridView(dataGridViewSavings);
            gridViewManager.fillDataGridView(model.DataSources[0]);

            //Displays the number of results found after executing the query for the specified time interval
            displayDgvResultsCount(displayedSavingsCountLabel, model.DataSources[0]);

            fillStaticTypePieChart(pieChartSavings, model.DataSources[1], typeNames);
            fillColumnChart(columnChartSavings, model.DataSources[2], currentYear, title);
        }

        //Generic method for filling ColumnChart objects with data
        private void fillColumnChart(Chart chart, DataTable dTable, int currentYear, String title) {
            //Removes all the existing points from the chart
            chart.Series[0].Points.Clear();

            if (dTable != null && dTable.Rows.Count > 0) {
                //Creates a list of dates(the first day of each month of the selected year)
                List<DateTime> dates = new List<DateTime>();
                for (int i = 1; i <= 12; i++) {
                    dates.Add(new DateTime(currentYear, i, 1));
                }

                //Ads the month names to the chart
                foreach (DateTime currentDate in dates) {
                    chart.Series[0].Points.AddXY(currentDate, 0);
                }

                int j = 0;//the index of the current row of the DataTable retrieved from the DB
                for (int i = 0; i < chart.Series[0].Points.Count; i++) {
                    //If all the rows from the DataTable were processed(e.g. there aren;t results for each month of the year) the 0 value is added for the respective month
                    if (j > dTable.Rows.Count - 1) {
                        chart.Series[0].Points[i].SetValueY(0);
                        continue;
                    }

                    //A DateTime object is created for the X value of the current chart point
                    System.DateTime currentPointDate = System.DateTime.FromOADate(chart.Series[0].Points[i].XValue);
                    //The month value is extracted from the previously created DateTime object
                    int currentChartMonth = Convert.ToInt32(DateTime.ParseExact(currentPointDate.ToString("MMM"), "MMM", CultureInfo.CurrentCulture).Month);
                    int currentDataTableMonth = Convert.ToInt32(dTable.Rows[j].ItemArray[0]);

                    //A check is performed to see if the value of the current chart point is the same as the value of the month present in the DataTable retrieved from the DB(at the corresponding index) 
                    if (currentChartMonth == currentDataTableMonth) {
                        int currentDataTableMonthValue = Convert.ToInt32(dTable.Rows[j].ItemArray[1]);
                        chart.Series[0].Points[i].SetValueY(currentDataTableMonthValue);//the respective value is added if the month value is identical in the previously mentioned sources
                        j++;//the value of the current index from the DataTable is incremented
                    } else {
                        chart.Series[0].Points[i].SetValueY(0);//otherwise the 0 value is added since there's no match between the DataTable record and the current month from the chart                   
                    }
                }
            }
            //Sets the column chart title
            chart.Titles[0].Text = String.Format("{0} for {1}", title, currentYear);
        }


        //General method for filling PieChart objects with data
        private void fillDynamicTypePieChart(Chart chart, DataTable dTable) {
            //Eliminare conținut anterior din grafic
            chart.Series[0].Points.Clear();

            if (dTable != null && dTable.Rows.Count > 0) {
                if ("pieChartExpenses".Equals(chart.Name) || "pieChartIncomes".Equals(chart.Name)) {
                    String[] typeNamesExpenses = dTable.AsEnumerable().Select(expenseTypeName => expenseTypeName.Field<String>(0)).ToArray();
                    Type fieldType = dTable.Columns[1].DataType;
                    decimal[] itemTypeSumsExpenses = dTable.AsEnumerable().Select(expenseTypeSum => expenseTypeSum.Field<decimal>(1)).ToArray();
                    Type fieldType2 = dTable.Columns[2].DataType;
                    String[] itemTypePercentagesExpenses = dTable.AsEnumerable().Select(expenseTypePercentage => expenseTypePercentage.Field<String>(2)).ToArray();

                    //Adaugare puncte pe grafic
                    for (int i = 0; i < typeNamesExpenses.Length; i++) {
                        chart.Series[0].Points.AddXY(typeNamesExpenses[i], itemTypeSumsExpenses[i]);
                        chart.Series[0].Points[i].LegendText = typeNamesExpenses[i];

                        String percentage = itemTypePercentagesExpenses[i];
                        if (Convert.ToDouble(percentage) > 0) {
                            //Adds label for the current element
                            chart.Series[0].Points[i].Label = percentage + "%";
                        } else {
                            chart.Series[0].Points[i].Label = " ";//daca procentajul e zero nu se adauga nimic in textul etichetei
                        }
                    }
                }
                //} else if ("pieChartIncomes".Equals(chart.Name) || "pieChartSavings".Equals(chart.Name)) {
                //    //Transforms the object array into an int array for further processing
                //    int[] itemTypeSums = Array.ConvertAll(dTable.Rows[0].ItemArray, x => x != DBNull.Value ? Convert.ToInt32(x) : 0);

                //    //If the length of the arrays is not equal the graph will not be populated with values
                //    if (typeNames.Length != itemTypeSums.Length) {
                //        return;
                //    }

                //    int totalAmount = 0;
                //    //For the expense and incomes pie charts the total value is calculated by adding all the item values present in the array while for the savings the total value is represented by the total income sum(the array contains only two values, total savings and total incomes)
                //    if ("pieChartIncomes".Equals(chart.Name)) {
                //        totalAmount = ViewCalculator.calculateSum(itemTypeSums);
                //    } else if ("pieChartSavings".Equals(chart.Name)) {
                //        totalAmount = itemTypeSums[itemTypeSums.Length - 1];
                //    }


                //    //Adds points on the chart
                //    for (int i = 0; i < typeNames.Length; i++) {
                //        chart.Series[0].Points.AddXY(typeNames[i], itemTypeSums[i]);
                //        chart.Series[0].Points[i].LegendText = typeNames[i];

                //        //Calculating the percentage for each present type based on the previously calculated total value
                //        double percentage = ViewCalculator.calculatePercentage(itemTypeSums[i], totalAmount);
                //        if (percentage > 0) {
                //            //Adds label for the current element
                //            chart.Series[0].Points[i].Label = percentage + "%";
                //        } else {
                //            chart.Series[0].Points[i].Label = " ";//daca procentajul e zero nu se adauga nimic in textul etichetei
                //        }
                //    }
                //} else {
                //    return;
                //}



            }
        }

        private void fillStaticTypePieChart(Chart chart, DataTable dTable, String[] typeNames) {
            chart.Series[0].Points.Clear();

            if (dTable != null && dTable.Rows.Count > 0) {
                //Transforms the object array into an int array for further processing
                int[] itemTypeSums = Array.ConvertAll(dTable.Rows[0].ItemArray, x => x != DBNull.Value ? Convert.ToInt32(x) : 0);

                //If the length of the arrays is not equal the graph will not be populated with values
                if (typeNames.Length != itemTypeSums.Length) {
                    return;
                }

                int totalAmount = 0;
                //For the expense and incomes pie charts the total value is calculated by adding all the item values present in the array while for the savings the total value is represented by the total income sum(the array contains only two values, total savings and total incomes)
                if ("pieChartIncomes".Equals(chart.Name)) {
                    totalAmount = ViewCalculator.calculateSum(itemTypeSums);
                } else if ("pieChartSavings".Equals(chart.Name)) {
                    totalAmount = itemTypeSums[itemTypeSums.Length - 1];
                }


                //Adds points on the chart
                for (int i = 0; i < typeNames.Length; i++) {
                    chart.Series[0].Points.AddXY(typeNames[i], itemTypeSums[i]);
                    chart.Series[0].Points[i].LegendText = typeNames[i];

                    //Calculating the percentage for each present type based on the previously calculated total value
                    double percentage = ViewCalculator.calculatePercentage(itemTypeSums[i], totalAmount);
                    if (percentage > 0) {
                        //Adds label for the current element
                        chart.Series[0].Points[i].Label = percentage + "%";
                    } else {
                        chart.Series[0].Points[i].Label = " ";//if the percentage value is equal to 0 then nothing will be added to the text of the label
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
                int[] debtValues = getColumnData(inputDataTable, 1) != null ? getColumnData(inputDataTable, 1).ToArray() : new int[] { };

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
                    double percentage = ViewCalculator.calculatePercentage(debtValueForCurrentCreditor, totalAmount);

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


        //Generic method for displaying the message regarding the selected time interval (the label above the DataGridViews present in the main dashboard tabs)
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
                } else {
                    targetLabel.Text = String.Format("{0} for {1}-{2} {3}", message, startPicker.Value.ToString("MMMM"), endPicker.Value.ToString("MMMM"), startPicker.Value.ToString("yyyy"));
                }

            } else if (dateTimePickerContainer.Visible == true && !isValidDateSelection(startPicker, endPicker)) {
                resetDateTimePicker(startPicker);
                resetDateTimePicker(endPicker);

                MessageBox.Show("Invalid date selection!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            } else if (dateTimePickerContainer.Visible == false) {
                targetLabel.Text = String.Format("{0} for {1} {2}", message, startPicker.Value.ToString("MMMM"), startPicker.Value.ToString("yyyy"));
            }
        }

        //Methods for checking if the user selected start/end dates of the interval are correct
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
            //Adds the names of the budget elements in the first column of DataGridView present in the Budget summary tab
            dataGridViewBS.Rows.Add(5);
            dataGridViewBS[0, 0].Value = "Incomes";
            dataGridViewBS[0, 1].Value = "Expenses";
            dataGridViewBS[0, 2].Value = "Debts";
            dataGridViewBS[0, 3].Value = "Savings";
            dataGridViewBS[0, 4].Value = "Left to spend";
        }

        //Method for converting the date retrieved from the provided DatePicker object into an SQL compatible format
        private String getDateStringInSQLFormat(DateTimePicker datePicker, DateType dateType) {
            if (datePicker == null) {
                return "";
            }
            String sqlFormatDateString = "";
            //If this is the end date of the interval it will be modified so that the last day of the month will also be taken into account
            if (dateType == DateType.END_DATE) {
                //Takes the current date from the provided DateTimePicker, adds a month to its value and subtracts a day(in order to get the date of the last day of the month)
                sqlFormatDateString = datePicker.Value.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            } else {
                sqlFormatDateString = datePicker.Value.ToString("yyyy-MM-dd");//if this is the start date, the value of the DateTimePicker wil be retrieved and no changes will be performed because it already starts with the first day of the month(check the designer settings)
            }


            return sqlFormatDateString;
        }


        private void resetDateTimePicker(DateTimePicker targetPicker) {
            if (targetPicker == null) {
                return;
            }

            //Retrieves the number of days passed from the beginning of the month
            int daysPassedFromMonthStart = DateTime.Now.Day;
            hasResetDatePickers = true;//sets the flag to true in order to avoid the execution of the DateTimePicker event handler method          
            targetPicker.Value = DateTime.Now.AddDays(-daysPassedFromMonthStart + 1);//subtracts the number of days passed from the beginning of the month and adds one day to the resulted value (otherwise the first day of the month would become 0 which is incorrect)

        }

        //Generic method for sendung data to the controller
        private void sendDataToController(DataUpdateControl pickerType, CheckBox checkBox, DateTimePicker startPicker, DateTimePicker endPicker) {
            //Checks the type of control whose state has been modified
            if (pickerType == DataUpdateControl.START_PICKER) {
                //If the interval checkbox is checked it means that the user wants to get data for multiple months
                if (checkBox.Checked == true) {
                    //Selects the multiple months option                 
                    QueryType option = QueryType.MULTIPLE_MONTHS;

                    //The start and end dates are retrieved, having the format required by the MySQL database
                    String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                    String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);

                    //The object used for data transfer is configured and it is sent to the controller alongside the type of query that will be performed                 
                    QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).build();

                    controller.requestData(option, paramContainer);

                } else {
                    //Otherwise the single month query type is selected
                    QueryType option = QueryType.SINGLE_MONTH;

                    //The start and end dates of the selected year are retrieved
                    int month = startPicker.Value.Month;
                    int year = startPicker.Value.Year;

                    //The object used for data transfer is configured and it is sent to the controller alongside the type of query that will be performed                      
                    QueryData paramContainer = new QueryData.Builder(userID).addMonth(month).addYear(year).build();

                    controller.requestData(option, paramContainer);


                }
            } else if (pickerType == DataUpdateControl.END_PICKER) {
                //Selectes multiple months data if the end date picker is the one whose state was modified by the user
                QueryType option = QueryType.MULTIPLE_MONTHS;

                String startDate = getDateStringInSQLFormat(startPicker, DateType.START_DATE);
                String endDate = getDateStringInSQLFormat(endPicker, DateType.END_DATE);

                QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).build(); //CHANGE
                controller.requestData(option, paramContainer);

            } else if (pickerType == DataUpdateControl.MONTHLY_PICKER) {
                //Selects the query type that will lead to the calculation of monthly total values for each month of the selected year
                QueryType option = QueryType.MONTHLY_TOTALS;

                //Retrieves the start and end dates
                int month = startPicker.Value.Month;
                int year = startPicker.Value.Year;

                //Sends data to the controller just as before              
                QueryData paramContainerTest = new QueryData.Builder(userID).addMonth(month).addYear(year).build(); //CHANGE
                controller.requestData(option, paramContainerTest);
            }
        }

        private List<int> getColumnData(DataTable inputDataTable, int columnIndex) {
            //Checks if the DataTable object contains values and is not null
            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                List<int> columnData = new List<int>();

                //Iterates over the rows of the DataTable
                for (int i = 0; i < inputDataTable.Rows.Count; i++) {
                    //Retrieves the value from the current row and the specified column to the ArrayList
                    Object currentValue = inputDataTable.Rows[i][columnIndex];
                    //Transforms the object into an int value and adds it to the list (if any of these objects is null then the default value set for it will be 0)
                    columnData.Add(currentValue != DBNull.Value ? Convert.ToInt32(currentValue) : 0);

                }
                //Returns the created list
                return columnData;
            }
            //Returns null if the DataTable object does not contain data
            return null;
        }

        //Method used for displaying the number of results displayed in the DataGridView
        private void displayDgvResultsCount(Label targetLabel, DataTable dgvDataSource) {
            //Checks if the data source object is null in order to prevent NPE when trying to fetch its size
            if(dgvDataSource == null ) {
                return;
            }

            String noResultsFoundMessage = "No results found!";
            String resultsFoundMessage = "Displaying {0} result{1}";

            int dgvDataSourceSize = dgvDataSource.Rows.Count;

            if (dgvDataSourceSize > 0) {
                targetLabel.Text = String.Format(resultsFoundMessage, dgvDataSourceSize, dgvDataSourceSize > 1 ? "s" : "");
            } else {
                targetLabel.Text = noResultsFoundMessage;
            }
        }

        private void updateDataToolStripMenuItem_Click(object sender, EventArgs e) {
            new UpdateUserDataForm(userID).ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult userOption = displayApplicationCloseMessage(ApplicationCloseMode.LOGOUT);

            if (userOption == DialogResult.Yes) {
                //this.Visible = false;               
                //new LoginForm().Visible = true;
                hasRestartedApplication = true;
                Application.Restart();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult userOption = displayApplicationCloseMessage(ApplicationCloseMode.EXIT);

            if (userOption == DialogResult.Yes) {
                userAgreedToExit = true;
                Application.Exit();
            }
        }

        private void insertDataToolStripMenuItem_Click(object sender, EventArgs e) {
            //The ShowDialog() method is used in order to prevent the usage of the background window while the selected one is open(it also prevents the opening of multiple instances of the same window)
            new InsertDataForm(userID).ShowDialog();
        }

        private void createExternalAccountToolStripMenuItem_Click(object sender, EventArgs e) {
            new ExternalAccountsInsertionForm(userID).ShowDialog();
        }

        //Sets the default date for the DateTimePicker objects as the first day of the current month of the current year
        private void setDateTimePickerDefaultDate(DateTimePicker[] dateTimePickers) {
            //Creaza o instanta a datei curente
            //Creates an instance of the current date
            DateTime defaultDate = DateTime.Now;

            //Retrieves the year and month value from the previously created DateTime object and sets the value day value to 1
            int month = defaultDate.Month;
            int year = defaultDate.Year;
            int day = 1;

            foreach (DateTimePicker currentPicker in dateTimePickers) {
                //Sets the flag value to true so that the event handler method of the DateTimePicker will not be called when the date is changed
                hasResetDatePickers = true;

                //Sets the date as the first day of the current month of the current year
                currentPicker.Value = new DateTime(year, month, day);
            }
        }

        //Method for displaying the correct message to the user according to the close option selected(Logout/Exit)
        //It displays the message and returns the value of the user option to the calling method
        private DialogResult displayApplicationCloseMessage(ApplicationCloseMode closeMode) {
            Guard.notNull(closeMode, "close mode");

            DialogResult userOption = DialogResult.None;
            switch (closeMode) {
                case ApplicationCloseMode.EXIT:
                    userOption = MessageBox.Show("Are you sure that you want to exit?", "Budget manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    break;

                case ApplicationCloseMode.LOGOUT:
                    userOption = MessageBox.Show("Are you sure that you want to logout?", "Budget manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    break;

                case ApplicationCloseMode.UNDEFINED:
                    break;

            }

            return userOption;
        }

        private void externalAccountTransfersToolStripMenuItem_Click(object sender, EventArgs e) {
            new ExternalAccountTransfersForm(userID).ShowDialog();
        }

        private void updateReceivablesToolStripMenuItem_Click(object sender, EventArgs e) {
            new ReceivableManagementForm(userID).ShowDialog();
        }

        private void externalAccountStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExternalAccountStatisticsForm().ShowDialog();
        }
    }
}



