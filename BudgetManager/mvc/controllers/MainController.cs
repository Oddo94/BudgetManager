using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    public class MainController : IControl {
        private IView view;
        private IModel model;

        public MainController(IView view, IModel model) {           
            this.view = view;
            this.model = model;
        }
        public MainController() {


        }

        //Metoda prin care Controllerul cere date noi si ulterior actualizeaza continutul datelor din model(pt a permite actualizarea simultana a unor DataTables continand date diferite)
        public void requestData(QueryType option, QueryData paramContainer) {
            //Actualizare date pentru primele doua surse din sir în cazul interogarilor pe o luna/mai multe luni                                                   
            if (option == QueryType.SINGLE_MONTH || option == QueryType.MULTIPLE_MONTHS) {            
               //Se apeleaza metoda modelului pentu a aduce date noi din baza pt fiecare din cele doua obiecte
               DataTable dynamicDataTable1 =  model.getNewData(option, paramContainer,SelectedDataSource.DYNAMIC_DATASOURCE_1);
               DataTable dynamicDataTable2 = model.getNewData(option, paramContainer, SelectedDataSource.DYNAMIC_DATASOURCE_2);
                
                //Se obtine sirul continand sursele de date in forma actuala
                DataTable[] updatedDataSources = model.DataSources;

                //Se actualizeaza pozitiile din sir cu noile elemente
                updatedDataSources[0] = dynamicDataTable1;
                updatedDataSources[1] = dynamicDataTable2;

                //Se actualizeaza sirul din model
                model.DataSources = updatedDataSources;
         
              //Actualizare date pentru cea de-a treia sursa din sir în cazul interogarilor pe tot anul
            } else if (option == QueryType.MONTHLY_TOTALS) {
                DataTable staticDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);
               
                DataTable[] updatedDataSources = model.DataSources;
                updatedDataSources[2] = staticDataTable;
                model.DataSources = updatedDataSources;
            }
        }

        public void setView(IView view) {
            this.view = view;
        }

        public void setModel(IModel model) {
            this.model = model;
            //Testeaza daca modelul are conexiune la baza de date iar daca nu are dezactiveaza date time pickers din interfata
            if(!model.hasDBConnection()) {
                disableViewControls();
            } else {
                enableViewControls();
            }
        }

        public void disableViewControls() {
            view.disableControls();
        }

        public void enableViewControls() {
            view.enableControls();
        }
    }
}
