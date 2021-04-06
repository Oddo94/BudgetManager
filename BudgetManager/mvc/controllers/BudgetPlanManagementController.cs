using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.controllers {
    class BudgetPlanManagementController : IUpdaterControl {
        private IView view;
        private IUpdaterModel model;

        public BudgetPlanManagementController(IView view, IUpdaterModel model) {
            this.view = view;
            this.model = model;
        }

        public BudgetPlanManagementController() {

        }

        //NEEDS FURTHER CHECKS!! WHAT HAPPENS IF A MODEL IMPLEMENTING IMODEL CLASS IS CASTED TO A MODEL IMPLEMENTING IUPDATERMODEL? WHAT HAPPENS TO THE UPDATE AND DELETE METHODS?
        public void setModel(IModel model) {
            this.model = (IUpdaterModel)model;

            if (!model.hasDBConnection()) {
                view.disableControls();
            }
        }

        public void setView(IView view) {
            this.view = view;
        }

        public void disableViewControls() {
            view.disableControls();
        }

        public void enableViewControls() {
            view.enableControls();
        }

        public void requestData(QueryType option, QueryData paramContainer) {
            //NOTE:
            //DYNAMIC_DATASOURCE_1 - used when requesting the list of budget plans for a month/ year(first position in the DataSources array)
            //DYNAMIC_DATASOURCE_2 - used when requesting info about the selected budget plan(second position in the DataSources array)
  

            //Retrieving the DataTable object containing the data from the DB
            //DataTable staticDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);

                //Updating the data when the budget plan display is requested(single month/full year)
                if (option == QueryType.SINGLE_MONTH || option == QueryType.FULL_YEAR) {
                DataTable dynamicDataTable = model.getNewData(option, paramContainer, SelectedDataSource.DYNAMIC_DATASOURCE_1);

                DataTable[] updatedDataSources = model.DataSources;
                updatedDataSources[0] = dynamicDataTable;
                model.DataSources = updatedDataSources;//Assigns the updated DataTable array to the DataSources array reference of the model, which triggers the update of the View through the notifyObservers() method

            //Updating the data when info about the selected budget plan is requested
            } else if (option == QueryType.BUDGET_PLAN_INFO) {
                DataTable dynamicDataTable2 = model.getNewData(option, paramContainer, SelectedDataSource.DYNAMIC_DATASOURCE_2);

                DataTable[] updatedDataSources = model.DataSources;
                updatedDataSources[1] = dynamicDataTable2;
                model.DataSources = updatedDataSources;
            }   
        }

        public int requestUpdate(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            int executionResult = model.updateData(option, paramContainer, sourceDataTable);

            return executionResult;
        }

        public int requestDelete(string tableName, int itemID) {
            int executionResult = model.deleteData(tableName, itemID);

            return executionResult;
        }
    }
}
