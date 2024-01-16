using BudgetManager.mvc.models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    public interface DataInsertionStrategy {
      int execute(QueryData paramContainer);
      int execute(IDataInsertionDTO dataInsertionDTO);//Test change for future refactoring (using DTO classes instead of QueryData class to transfer data from the GUI layer to the database layer)
      //int execute();//Future refactoring for encapsulating the DTO object inside the strategy command so that it doesn't have to be passed through the Context object
    }
}
