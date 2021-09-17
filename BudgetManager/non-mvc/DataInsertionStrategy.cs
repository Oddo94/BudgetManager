using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    interface DataInsertionStrategy {
      int execute(QueryData paramContainer);
    }
}
