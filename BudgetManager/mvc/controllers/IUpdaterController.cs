using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    interface IUpdaterControl : IControl {

        //CHECK AND CORRECT IMPLEMENTATION IN THE IUPDATERCONTROLLER CLASS
        //void setModel(IUpdaterModel model);
        int requestUpdate(QueryType option, QueryData paramContainer, DataTable sourceDataTable);
        int requestDelete(String tableName, int itemID);
        int requestDelete2(QueryType option, QueryData paramContainer, DataTable sourceDataTable);
    }
}
