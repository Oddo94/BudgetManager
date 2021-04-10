using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    interface IUpdaterModel : IModel {
        
        int updateData(QueryType option, QueryData paramContainer, DataTable sourceDataTable);
        int deleteData(String tableName, int itemID);
        int deleteData2(QueryType option, QueryData paramContainer,DataTable sourceDataTable);

    }
}
