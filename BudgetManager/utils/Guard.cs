using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    public static class Guard {

        public static void notNull(Object value , String paramName) {
            if (value == null) {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void inRange(DataGridView gridView, int rowIndex) {
            int maxRowIndex = gridView.Rows.Count - 1;

            if (rowIndex < 0 || rowIndex >= gridView.Rows.Count) {
                throw new ArgumentOutOfRangeException($"Row index {rowIndex} out of the allowed range 0-{maxRowIndex}");
            }
        }

        public static void inRange(DataGridView gridView, int rowIndex, int columnIndex) {
            int maxRowIndex = gridView.Rows.Count - 1;
            int maxColumnIndex = gridView.Columns.Count - 1;

            if (rowIndex < 0 || rowIndex >= gridView.Rows.Count) {
                throw new ArgumentOutOfRangeException($"Row index {rowIndex} out of the allowed range 0-{maxRowIndex}");
            } else if (columnIndex < 0 || columnIndex >= gridView.Columns.Count) {
                throw new ArgumentOutOfRangeException($"Column index {columnIndex} out of allowed range 0-{maxColumnIndex}");
            }
        }
    }

}
