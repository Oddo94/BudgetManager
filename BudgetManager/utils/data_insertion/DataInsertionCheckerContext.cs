using BudgetManager.mvc.models;
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

        public DataCheckResponse invoke(QueryData inputData, String selectedItemName, int valueToInsert) {

            DataCheckResponse executionResult = dataCheckStrategy.performCheck(inputData, selectedItemName, valueToInsert);

            return executionResult;
        }

        public DataCheckResponse invoke(QueryData inputData, String selectedItemName, double valueToInsert) {

            DataCheckResponse executionResult = dataCheckStrategy.performCheck(inputData, selectedItemName, valueToInsert);

            return executionResult;
        }

        //Method aadded to provide more flexibility when using the invoker
        //The necessary data for performing the checks will be encapsualted in the strategy objects hence there will be no need to pass it all the way through the invoker
        public DataCheckResponse invoke() {
            return dataCheckStrategy.performCheck();
        }
       
    }
}
