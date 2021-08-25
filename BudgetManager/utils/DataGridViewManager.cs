using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Data;

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
            Guard.notNull(targetDataGridView, "DataGridView");

            this.targetDataGridView = targetDataGridView;
        }

        public void setDataGridView(DataGridView targetDataGridView) {
            Guard.notNull(targetDataGridView, "DataGridView");

            this.targetDataGridView = targetDataGridView;
        }

        //No parameter checking since sometimes it might be necessary to set the DataGridView data source to null
        public void fillDataGridView(DataTable inputDataTable) {
            targetDataGridView.DataSource = inputDataTable;
        }

        //Method for making data grid view rows for which the record start and end dates don't meet the specified criteria non-editable
        public void disableRowsBasedOnDate(int startDateCellIndex, int endDateCellIndex) {
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

        //Method for disabling specific columns from the DataGridGriew(the parameter is an array containing the indexes of the columns which need to be made non-editable)
        public void disableDataGridViewColumns(int[] columnIndexes) {
            Guard.notNull(columnIndexes, "Column indexes array");

            //Each index from the array is checked to see if is higher than the index of the last column in the DataGridView
            foreach (int currentIndex in columnIndexes) {
                //If the current index is outside of the column index range then it will be skipped and no column will be disabled
                if (!areIndexesInRange(new int[] { currentIndex }, IndexCheckType.ROW_INDEX_CHECK)) {
                    continue;
                }

                targetDataGridView.Columns[currentIndex].ReadOnly = true;
            }
        }

        //Method for checking if the data contained in a DataGridView row can be edited(usually for budget plan reciords displayed in the DataGridView)
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

        //Method for retrieving multiple values from a specified DataGridView row
        public int[] getMultipleItemValuesFromSelectedRow(int selectedRowIndex, int[] itemIndexes) {
            //Arguments checks         
            if (!areIndexesInRange(new int[] { selectedRowIndex }, IndexCheckType.ROW_INDEX_CHECK)) {
                return null;
            }

            if (itemIndexes == null || !areIndexesInRange(itemIndexes, IndexCheckType.COLUMN_INDEX_CHECK)) {
                return null;
            }
            //Getting the selected row
            DataGridViewRow selectedRow = targetDataGridView.Rows[selectedRowIndex];

            //Creates a list for storing the values
            List<int> itemDataList = new List<int>();
            //Iterates through the item index array retrieving the value at each index and adding it to the list
            foreach (int currentIndex in itemIndexes) {
                int currentItemValue = selectedRow.Cells[currentIndex].Value != null ? Convert.ToInt32(selectedRow.Cells[currentIndex].Value) : 0;
                itemDataList.Add(currentItemValue);
            }

            //Converting the List<int> object to an int[] array and returning the result
            return itemDataList.ToArray();
        }

        //Method for calculating the sum of the specified column values
        public int calculateItemsValueSum(int selectedRowIndex, int[] summedItemsIndexes ) {
            //Arguments checks         
            if (!areIndexesInRange(new int[] { selectedRowIndex }, IndexCheckType.ROW_INDEX_CHECK)) {
                return -1;
            }

            if (summedItemsIndexes == null || !areIndexesInRange(summedItemsIndexes, IndexCheckType.COLUMN_INDEX_CHECK)) {
                return -1;
            }

            //Gets the selected row from the DataGridView
            DataGridViewRow selectedRow = targetDataGridView.Rows[selectedRowIndex];

            int selectedItemsSum = 0;
            //Iterates through the item index array retrieving the value at each index and adding it to the list
            foreach (int currentIndex in summedItemsIndexes) {
                //Converts the value at the respective cell and if this value is null the result will be 0
                int currentItemValue = selectedRow.Cells[currentIndex].Value != null ? Convert.ToInt32(selectedRow.Cells[currentIndex].Value) : 0;
                selectedItemsSum += currentItemValue;
            }
                            
            return selectedItemsSum;
        }

        //Method for checking if the provided column indexes are inside the column index range of the current DataGridView
        //The checks are different based on the type specified (column check/row check)
        private bool areIndexesInRange(int[] indexList, IndexCheckType performedCheck) {
            bool inRange = true;

            int lowerBoundIndex = 0;
            int upperBoundIndex = 0;

            //The upper bound index max limit is calculated based on the performed check(column check/row check) but the lower bound index is always 0
            if (performedCheck == IndexCheckType.COLUMN_INDEX_CHECK) {
                upperBoundIndex = targetDataGridView.Columns.Count - 1;
            } else if (performedCheck == IndexCheckType.ROW_INDEX_CHECK) {
                upperBoundIndex = targetDataGridView.Rows.Count - 1;
            }

            //If any of the specified indexes is out of range then false value will be returned from the method
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
