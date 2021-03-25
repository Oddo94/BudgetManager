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
        FULL_YEAR //optiune adaugata pentru modelul folosit la actualizarea datelor       
    }

    //Enum pt sursa de date ce urmeaza a fi populata
    //DYNAMIC_DATASOURCE_1-sursa de date pt tabel
    //DYNAMIC_DATASOURCE_2- sursa de date pt pie chart
    //STATIC_DATASOURCE = sursa de date pt column chart(graficul care afiseaza situatia pe lunile unui an selectat, ex:veniturile lunare pt anul 2020)
    public enum SelectedDataSource {
        DYNAMIC_DATASOURCE_1,
        DYNAMIC_DATASOURCE_2,
        STATIC_DATASOURCE
    }
    
    public interface IModel {
        DataTable[] DataSources { get; set; }
             
        DataTable getNewData(QueryType option, QueryData paramContainer,SelectedDataSource dataSource);
        void notifyObservers();       
        void addObserver(IView observer);//Se foloseste ca parametru referinta catre interfata IView asfel că orice clasa care o implementează va putea fi transmisa ca argument
        void removeObserver(IView observer);
        bool hasDBConnection();
    }
}
