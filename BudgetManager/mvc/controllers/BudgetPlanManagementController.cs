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
            //Retrieving the DataTable object containing the data from the DB
            DataTable staticDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);

            //Retrieving the array containing the DataTable objects from the model
            DataTable[] updatedDataSources = model.DataSources;
            //Updating the element at index 0
            updatedDataSources[0] = staticDataTable;

            //Setting the DataSource array reference from the model to point to the updated array(this will trigger the notifyObservers method which will update the view)
            model.DataSources = updatedDataSources;
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
