using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class UpdateUserDataController : IUpdaterControl {
        private IView view;
        private IUpdaterModel model;

        public UpdateUserDataController(IView view, IUpdaterModel model) {
            this.view = view;
            this.model = model;
        }

        public UpdateUserDataController() {

        }

        public void requestData(QueryType option, QueryData paramContainer) {
            DataTable staticDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);
            DataTable[] updatedDataSources = model.DataSources;
            updatedDataSources[0] = staticDataTable;

            model.DataSources = updatedDataSources;

        }

        public int requestDelete(string tableName, int itemID) {
           int executionResult = model.deleteData(tableName, itemID);

            return executionResult;
        }

        public int requestUpdate(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            int executionResult = model.updateData(option, paramContainer, sourceDataTable);

            return executionResult;
        }
         

        public void setModel(IModel model) {
           this.model = (IUpdaterModel) model;

            //La momentul setarii model se verfica daca exista conexiune la baza de date si se dezactiveaza/activeaza controlalele din interfata
            if (!model.hasDBConnection()) {
                disableViewControls();
            } else {
                //enableViewControls();
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
    }
}
