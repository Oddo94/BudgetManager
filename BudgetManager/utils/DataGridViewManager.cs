using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

namespace BudgetManager.utils {
    //Enum that contains the type of index checks that can be made on the current DataGridView object
    public enum IndexCheckType {
       COLUMN_INDEX_CHECK,
       ROW_INDEX_CHECK,
       UNDEFINED
    }
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
        public void disableDataGridViewRows(int startDateCellIndex, int endDateCellIndex) {
            int[] columnIndexes = new int[] { startDateCellIndex, endDateCellIndex };

            if (!areIndexesInRange(columnIndexes, IndexCheckType.COLUMN_INDEX_CHECK)) {
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
        public void disableDataGridViewColumns(int[] columnIndexes) {
            if (columnIndexes == null) {
                return;
            }
            //Each index from the array is checked to see if is higher than the index of the last column in the DataGridView
            foreach (int currentIndex in columnIndexes) {
                //If the current index is outside of the column index range then it will be skipped and no column will be disabled
                if (!areIndexesInRange(new int[] { currentIndex }, IndexCheckType.ROW_INDEX_CHECK)) {
                    continue;
                }

                targetDataGridView.Columns[currentIndex].ReadOnly = true;
            }
        }

        //Method for checking if the budget plan contained in a DataGridView row can be edited
        //Editing is allowed only for current budget plans, future budget plans and for plans that started in the past but will continue for some time in the future after the start of the current month 
        public bool isEditableBasedOnDate(DateTime startDate, DateTime endDate) {
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
        public String[] getDatesFromSelectedRow(int selectedRowIndex, int startDateCellIndex, int endDateCellIndex) {
            //Arguments check
            if (!areIndexesInRange(new int[]{selectedRowIndex}, IndexCheckType.ROW_INDEX_CHECK)) {
                return null;
            }

            if (!areIndexesInRange(new int[] { startDateCellIndex, endDateCellIndex},IndexCheckType.COLUMN_INDEX_CHECK)) {
                return null;
            }


            //Getting the selected row
            DataGridViewRow selectedRow = targetDataGridView.Rows[selectedRowIndex];


            //Gets the dates from the selected row as String objects(if the date cells are empty the null value is assigned)
            String startDateString = !"".Equals(selectedRow.Cells[startDateCellIndex].FormattedValue) ? selectedRow.Cells[startDateCellIndex].Value.ToString() : null;
            String endDateString = !"".Equals(selectedRow.Cells[endDateCellIndex].FormattedValue) ? selectedRow.Cells[endDateCellIndex].Value.ToString() : null;

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

        private int[] getItemsPercentagesFromSelectedRow(int selectedRowIndex) {
            //Arguments checks         
            if (!areIndexesInRange(new int[] { selectedRowIndex }, IndexCheckType.ROW_INDEX_CHECK)) {
                return null;
            }
            //Getting the selected row
            DataGridViewRow selectedRow = targetDataGridView.Rows[selectedRowIndex];

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

        //Method for checking if the provided column indexes are inside the column index range of the current DataGridView
        //The checks are different based on the type specified (column check/row check)
        private bool areIndexesInRange(int[] indexList, IndexCheckType performedCheck) {
            bool inRange = true;

            int lowerBoundIndex = 0;
            int upperBoundIndex = 0;

            if (performedCheck == IndexCheckType.COLUMN_INDEX_CHECK) {
                upperBoundIndex = targetDataGridView.Columns.Count - 1;
            } else if (performedCheck == IndexCheckType.ROW_INDEX_CHECK) {
                upperBoundIndex = targetDataGridView.Rows.Count - 1;
            }

            foreach (int index in indexList) {
                if (index < lowerBoundIndex || index > upperBoundIndex) {
                    inRange = false;
                    break;
                }
            }
            return inRange;
        }
    }
}
