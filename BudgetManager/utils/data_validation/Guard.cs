using System;
using System.Data;
using System.Windows.Forms;

namespace BudgetManager.utils {
    public static class Guard {

        //Method for checking if the provided argument is null and if so displays an error message based on the argument name  
        public static void notNull(Object value , String paramName) {
            if (value == null) {
                throw new ArgumentNullException(paramName, String.Format("The provided {0} argument cannot be null", paramName));
            }
        }

        public static void notNull(Object value, String paramName, String messageToDisplay) {
            if (value == null) {
                throw new ArgumentNullException(paramName, messageToDisplay);
            }
        }

        //Method for checking if the provided row index of the DataGridView object is in range
        public static void inRange(DataGridView gridView, int rowIndex) {
            int maxRowIndex = gridView.Rows.Count - 1;

            if (rowIndex < 0 || rowIndex >= gridView.Rows.Count) {
                throw new ArgumentOutOfRangeException($"Row index {rowIndex} out of the allowed range 0-{maxRowIndex}");
            }
        }

        //Method for checking if the provided row index and column index of the DataGridView object are in range
        public static void inRange(DataGridView gridView, int rowIndex, int columnIndex) {
            int maxRowIndex = gridView.Rows.Count - 1;
            int maxColumnIndex = gridView.Columns.Count - 1;

            if (rowIndex < 0 || rowIndex >= gridView.Rows.Count) {
                throw new ArgumentOutOfRangeException($"Row index {rowIndex} out of the allowed range 0-{maxRowIndex}");
            } else if (columnIndex < 0 || columnIndex >= gridView.Columns.Count) {
                throw new ArgumentOutOfRangeException($"Column index {columnIndex} out of allowed range 0-{maxColumnIndex}");
            }
        }

        public static void inRange(DataTable dataTable, int columnIndex) {
            int minColumnIndex = 0;
            int maxColumnIndex = dataTable.Columns.Count - 1;

            if (columnIndex < minColumnIndex || columnIndex > maxColumnIndex) {
                throw new ArgumentOutOfRangeException($"Column index {columnIndex} is out of allowed range 0-{maxColumnIndex}");
            }
        }
    }

}
