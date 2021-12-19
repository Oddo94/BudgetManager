using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils {
    interface DataInsertionCheckStrategy {

        int performCheck(QueryData inputData, String selectedItemName, int valueToInsert);
    }
}
