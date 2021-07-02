﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    class DataGridViewManager {
        private DataGridView targetDataGridView;

        public DataGridViewManager(DataGridView targetDataGridView) {
            //Null check inside the constructor in order to ensure a fail fast solution and to avoid the need for further null checks inside each method of the class
            if (targetDataGridView == null) {
                throw new ArgumentNullException(nameof(targetDataGridView));
            }

            this.targetDataGridView = targetDataGridView;
        }

        public void fillDataGridView(DataGridTableStyle inputDataTable) {
            targetDataGridView.DataSource = inputDataTable;
        }

        //Method for making data grid view rows that contain budget plans for which the start and end dates don't meet the specified criteria non-editable
        private void disableDataGridViewRows(int startDateCellIndex, int endDateCellIndex) {
            if (startDateCellIndex < 0 || endDateCellIndex < 0) {
                return;
            }

            if (startDateCellIndex > targetDataGridView.ColumnCount - 1 || endDateCellIndex > targetDataGridView.ColumnCount - 1) {
                return;
            }

            foreach (DataGridViewRow currentRow in targetDataGridView.Rows) {
                if (currentRow == null) {
                    return;
                }
                //Retrieves the date strings from the current row of the DataGridView
                String startDateString = currentRow.Cells[startDateCellIndex].Value != null ? currentRow.Cells[startDateCellIndex].FormattedValue.ToString() : "";
                String endDateString = currentRow.Cells[endDateCellIndex].Value != null ? currentRow.Cells[endDateCellIndex].FormattedValue.ToString() : "";

                if ("".Equals(startDateString) || "".Equals(endDateString)) {
                    currentRow.ReadOnly = true;
                    return;
                }

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                try {
                   startDate = DateTime.Parse(startDateString);
                   endDate = DateTime.Parse(endDateString);
                } catch (FormatException ex) {
                   Console.WriteLine(ex.StackTrace);
                   continue;

                } catch (ArgumentNullException ex) {
                    Console.WriteLine(ex.StackTrace);
                    continue;
                }
               

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
                if (currentIndex > dataGridView.Columns.Count - 1) {
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
            DateTime startOfCurrentMonthDate = new DateTime(currentDate.Year, currentDate.Month, 1);

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
            if (dataGridView == null) {
                return null;
            }

            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView.Rows.Count) {
                return null;
            }

            //Getting the selected row
            DataGridViewRow selectedRow = dataGridView.Rows[selectedRowIndex];


            //Gets the dates from the selected row as String objects(if the date cells are empty the null value is assigned)
            String startDateString = !"".Equals(selectedRow.Cells[8].FormattedValue) ? selectedRow.Cells[8].Value.ToString() : null;
            String endDateString = !"".Equals(selectedRow.Cells[8].FormattedValue) ? selectedRow.Cells[9].Value.ToString() : null;

            //Checks for the null value of any of the two string dates and if so returns(to avoid NPE)
            if (startDateString == null || endDateString == null) {
                return null;
            }

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

            int[] userSetPercentages = new int[] { expensesPercentage, debtsPercentage, savingsPercentage };

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





    }
}
