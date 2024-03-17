using BudgetManager.utils.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.models.dto {
    public interface IDataInsertionDTO {
        //String getName();
        //Method that returns the type of budget item represented by the DTO class that implements this interface
        BudgetItemType getBudgetItemType();
    }
}
