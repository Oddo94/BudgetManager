using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.models {
    class ReceivableManagementModel : IUpdaterModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];

        //SQL statement for selecting receivables created between two specfied dates
        String sqlStatementReceivableRetrieval = @"SELECT
	                                                        rcs.receivableID AS 'Receivable ID',
	                                                        rcs.name AS 'Receivable name',
	                                                        dbs.debtorName AS 'Debtor name',
	                                                        rcs.totalPaidAmount AS 'Total paid amount',
	                                                        rcs.value AS 'Receivable value',
	                                                        rst.statusDescription AS 'Status',
	                                                        rcs.createdDate AS 'Creation date',
	                                                        rcs.dueDate AS 'Due date'
                                                        FROM
	                                                        receivables rcs
                                                        INNER JOIN debtors dbs ON
	                                                        rcs.debtor_ID = dbs.debtorID
                                                        INNER JOIN receivable_status rst ON
	                                                        rcs.status_ID = rst.statusID	
                                                        INNER JOIN saving_accounts sac ON
	                                                        rcs.account_ID = sac.accountID
                                                        INNER JOIN users usr ON
	                                                        usr.userID = sac.user_ID
                                                        WHERE
	                                                        usr.userID = @paramID
                                                        AND
	                                                        rcs.createdDate BETWEEN @paramStartDate AND @paramEndDate                                                           
                                                        GROUP BY
	                                                        dbs.debtorID,
	                                                        rcs.createdDate
                                                        ORDER BY
	                                                        YEAR(rcs.createdDate),
	                                                        MONTH(rcs.createdDate);";


        public DataTable[] DataSources {
            get {
                return this.dataSources;
            }

            set {
                this.dataSources = value;
                notifyObservers();
            }
        }

        public DataTable getNewData(QueryType option, QueryData paramContainer, SelectedDataSource dataSource) {
            DataTable receivableDataTable = new DataTable();

            if (option == QueryType.DATE_INTERVAL) {
                MySqlCommand receivableRetrievalCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementReceivableRetrieval, paramContainer);

                receivableDataTable = DBConnectionManager.getData(receivableRetrievalCommand);

            }

            return receivableDataTable;
        }

        public int updateData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            throw new NotImplementedException();
        }

        public int deleteData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            throw new NotImplementedException();
        }

        public bool hasDBConnection() {
            return DBConnectionManager.hasConnection();
        }

        public void addObserver(IView observer) {
            this.observerList.Add(observer);
        }

        public void removeObserver(IView observer) {
            this.observerList.Remove(observer);
        }

        public void notifyObservers() {
            foreach (IView currentObserver in observerList) {
                currentObserver.updateView(this);
            }

        }

        //UTILITY METHODS
    }
}
