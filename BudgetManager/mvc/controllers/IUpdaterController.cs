using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    interface IUpdaterControl : IControl {

        int requestUpdate(QueryType option, QueryData paramContainer, DataTable sourceDataTable);
        int requestDelete(String tableName, int itemID);
    }
}
