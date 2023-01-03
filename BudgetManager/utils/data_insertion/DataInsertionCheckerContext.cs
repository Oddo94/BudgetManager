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

        //Method aadded to provide moreflexibility when using the invoker
        //The necessary data for performing the checks will be encapsualted in the strategy objects hence there will be no need to pass it all the way through the invoker
        public int invoke() {
            return dataCheckStrategy.performCheck();
        }
       
    }
}
