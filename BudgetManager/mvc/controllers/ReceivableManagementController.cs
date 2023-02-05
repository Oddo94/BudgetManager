using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.controllers {
    class ReceivableManagementController : IUpdaterControl {
        private IView view;
        private IUpdaterModel model;

        public ReceivableManagementController(IView view, IUpdaterModel model) {
            this.view = view;
            this.model = model;
        }

        public ReceivableManagementController() {

        }

        public void disableViewControls() {
            view.disableControls();
        }

        public void enableViewControls() {
            view.enableControls();
        }

        public void requestData(QueryType option, QueryData paramContainer) {
            DataTable retrievedReceivableDataTable = model.getNewData(option, paramContainer, SelectedDataSource.STATIC_DATASOURCE);
            DataTable[] updatedDataSources = model.DataSources;
            updatedDataSources[0] = retrievedReceivableDataTable;

            model.DataSources = updatedDataSources;
        }

        public int requestDelete(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            throw new NotImplementedException();
        }

        public int requestUpdate(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            throw new NotImplementedException();
        }

        public void setModel(IModel model) {
            this.model = (IUpdaterModel) model;

            if(!model.hasDBConnection()) {
                disableViewControls();
            }
        }

        public void setView(IView view) {
            this.view = view;
        }
    }
}
