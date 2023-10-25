using BudgetManager.mvc.models;
using BudgetManager.mvc.models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils {
    interface DataInsertionCheckStrategy {

        DataCheckResponse performCheck(QueryData inputData, String selectedItemName, int valueToInsert);
        DataCheckResponse performCheck(QueryData inputData, String selectedItemName, double valueToInsert);
        DataCheckResponse performCheck();
    }
}
