using BudgetManager.mvc.models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    class DataInsertionContext {
        DataInsertionStrategy insertionStrategy;

        public DataInsertionContext() { }

        public DataInsertionContext(DataInsertionStrategy insertionStrategy) {
            this.insertionStrategy = insertionStrategy;
        }

        public void setStrategy(DataInsertionStrategy insertionStrategy) {
            this.insertionStrategy = insertionStrategy;
        }

        public int invoke(QueryData paramContainer) {
            int result = this.insertionStrategy.execute(paramContainer);

            return result;
        }

        //Future refactoring test(change QueryData class to specific DTO class containing the specific fields for the inserted item)
        public int invoke(IDataInsertionDTO dataInsertionDTO) {
            int result = this.insertionStrategy.execute(dataInsertionDTO);

            return result;
        }


    }
}
