using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    //Enum pt tipul de fraza SQL executat(o singura luna/mai multe luni)
    public enum QueryType {
        SINGLE_MONTH,
        MULTIPLE_MONTHS,
        MONTHLY_TOTALS,
        FULL_YEAR,//option used for the model that also updates data
        BUDGET_PLAN_INFO,//option added for the queries that retrieve data for the currently selected budget plan
        UNDEFINED  //option added as default value to return when the conditinos for the other options are not met     
    }

    //Enum for the data source that is about to be populated
    //DYNAMIC_DATASOURCE_1-data grid view data source
    //DYNAMIC_DATASOURCE_2- pie chart data source
    //STATIC_DATASOURCE -column chart data source (the chart the shows the monthly evolution of an element for the selected year)
    public enum SelectedDataSource {
        DYNAMIC_DATASOURCE_1,
        DYNAMIC_DATASOURCE_2,
        STATIC_DATASOURCE
    }
    
    public interface IModel {
        DataTable[] DataSources { get; set; }
             
        DataTable getNewData(QueryType option, QueryData paramContainer,SelectedDataSource dataSource);
        void notifyObservers();       
        void addObserver(IView observer);//The parameter used is of IView type so any object whose class implements this interface can be used as an argument
        void removeObserver(IView observer);
        bool hasDBConnection();
    }
}
