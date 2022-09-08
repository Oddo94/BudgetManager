using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils {
    class DataInsertionCheckerContext {
        private DataInsertionCheckStrategy dataCheckStrategy;

        public DataInsertionCheckerContext() { }

        public DataInsertionCheckerContext(DataInsertionCheckStrategy dataCheckStrategy) {
            this.dataCheckStrategy = dataCheckStrategy;
        }

        public void setStrategy(DataInsertionCheckStrategy dataCheckStrategy) {
            this.dataCheckStrategy = dataCheckStrategy;
        } 

        public int invoke(QueryData inputData, String selectedItemName, int valueToInsert) {

            int executionResult = dataCheckStrategy.performCheck(inputData, selectedItemName, valueToInsert);

            return executionResult;
        }
       
    }
}
