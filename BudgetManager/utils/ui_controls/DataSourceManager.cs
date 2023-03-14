using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils.ui_controls {
    //Class that aims to provide utility methods for manipulating the data sources for certain user controls(e.g. DataGridView)
    class DataSourceManager {

        //Method used for updating the values of a row from a DataTable object
        public static void updateDataTable(DataTable dataTable, int updatedRowIndex, Dictionary<int, String> cellIndexValueDictionary) {
            //Parameters validation
            Guard.notNull(dataTable, "updated data table", "The data table that needs to be updated cannot be null!");
            Guard.notNull(cellIndexValueDictionary, "cell index-value dictionary", "The dictionary that contains the cell index-value mapping cannot be null!");

            if (updatedRowIndex < 0 || updatedRowIndex > dataTable.Rows.Count - 1) {
                throw new IndexOutOfRangeException("The index of the row that needs to be updated is out of bounds!");
            }

            foreach (int currentCellIndex in cellIndexValueDictionary.Keys) {
                int lowerBound = 0;
                int upperBound = dataTable.Rows[updatedRowIndex].ItemArray.Length - 1;

                if (currentCellIndex < lowerBound || currentCellIndex > upperBound) {
                    throw new IndexOutOfRangeException("The index of the cell that needs to be updated is out of bounds!");
                }
            }

            //Updates the specified row of the data table with the value from the dictionary containing the cell index-value mapping
            foreach (KeyValuePair<int, String> currentEntry in cellIndexValueDictionary) {
                int updatedCellIndex = currentEntry.Key;
                String updatedCellValue = currentEntry.Value;

                dataTable.Rows[updatedRowIndex].SetField(updatedCellIndex, updatedCellValue);
            }

        }

        //Method used to selectively/completely discard changes from a DataTable object
        public static void discardDataTableChanges(DataTable sourceDataTable, List<int> primaryKeyList, int primaryKeyColumnIndex, bool discardAllChanges = false) {
            //Parameters check
            Guard.notNull(sourceDataTable, "The source data table containing the changes to be discarded cannot be null!");
            Guard.notNull(primaryKeyList, "The primary key list cannot be null!");
            Guard.inRange(sourceDataTable, primaryKeyColumnIndex);

            if (discardAllChanges) {
                sourceDataTable.RejectChanges();
                return;
            }

            foreach (DataRow currentRow in sourceDataTable.Rows) {
                String[] currentRowValues = Array.ConvertAll(currentRow.ItemArray, x => x != DBNull.Value ? Convert.ToString(x) : "");
                int currentPrimaryKey = Convert.ToInt32(currentRowValues[primaryKeyColumnIndex]);

                if (primaryKeyList.Contains(currentPrimaryKey)) {
                    currentRow.RejectChanges();
                }
            }

        }

        //Method used for removing a row from a DataTable object
        public static void deleteDataTableRow(int deletedRowIndex, DataTable sourceDataTable) {
            //Parameters validation
            Guard.notNull(sourceDataTable, "updated data table", "The data table containing the row that needs to be deleted cannot be null!");
            
            if (deletedRowIndex < 0 || deletedRowIndex > sourceDataTable.Rows.Count - 1) {
                throw new IndexOutOfRangeException("The index of the row that needs to be deleted is out of bounds!");
            }

            sourceDataTable.Rows[deletedRowIndex].Delete();

        }
    }
}
