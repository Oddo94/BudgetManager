using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.controllers {
    class SavingAccountController : IControl {
        IView view;
        IModel model;

        public SavingAccountController(IView view, IModel model) {
            this.view = view;
            this.model = model;
        }

        public SavingAccountController() {

        }

        public void requestData(QueryType option, QueryData paramContainer) {
            //The account balance field is updated each time the data grid view or column chart are updated
            if (option == QueryType.SINGLE_MONTH || option == QueryType.MULTIPLE_MONTHS) {
                //Calling the model's method for retrieving new data from the DB                
                DataTable dynamicDataTable1 = model.getNewData(option, paramContainer, SelectedDataSource.DYNAMIC_DATASOURCE_1);//Grid view new data
                DataTable staticDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);//Account balance field new data

                DataTable[] updatedDataSources = model.DataSources;

                //Only the grid view and saving account balance field are updated since the column chart remains unchanged when requesting single/multiple months data
                updatedDataSources[0] = dynamicDataTable1;
                updatedDataSources[2] = staticDataTable;

                //Updating the model's data sources array
                model.DataSources = updatedDataSources;

            } else if (option == QueryType.MONTHLY_TOTALS) {                
                DataTable dynamicDataTable2 = model.getNewData(option, paramContainer, SelectedDataSource.DYNAMIC_DATASOURCE_2);//Column chart new data
                DataTable staticDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);//Account balance field new data

                DataTable[] updatedDataSources = model.DataSources;

                //Only the column chart and saving account balance field are updated since the column chart remains unchanged when requesting full year monthly totals data
                updatedDataSources[1] = dynamicDataTable2;
                updatedDataSources[2] = staticDataTable;

                model.DataSources = updatedDataSources;
            }
        }

        public void disableViewControls() {
            view.disableControls();
        }

        public void enableViewControls() {
            view.enableControls();
        }

        public void setModel(IModel model) {
            this.model = model;

            if (!model.hasDBConnection()) {
                view.disableControls();
            } else {
                view.enableControls();
            }
        }

        public void setView(IView view) {
            this.view = view;
        }
    }
}
