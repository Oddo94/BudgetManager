﻿using BudgetManager.utils;
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

        //SQL statement for selecting receivables created between two specified dates
        String sqlStatementReceivableRetrieval = @"SELECT
	                                                        rcs.receivableID AS 'Receivable ID',
	                                                        rcs.name AS 'Receivable name',
	                                                        dbs.debtorName AS 'Debtor name',
	                                                        rcs.totalPaidAmount AS 'Total paid amount',
	                                                        rcs.value AS 'Receivable value',
	                                                        rst.statusDescription AS 'Status',
	                                                        rcs.createdDate AS 'Creation date',
	                                                        rcs.dueDate AS 'Due date',
	                                                        rcs.payOffDate AS 'Pay off date'
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
                                                        ORDER BY
	                                                        rcs.createdDate;";

        String sqlStatementReceivableUpdate = @"UPDATE receivables
                                                SET name = @receivableName,
                                                    debtor_ID = (SELECT debtorID FROM debtors WHERE debtorName = @debtorName),
                                                    totalPaidAmount = @totalPaidAmount,
                                                    value = @receivableValue,
                                                    status_ID = (SELECT statusID FROM receivable_status WHERE statusDescription = @statusDescription),
                                                    createdDate = @createdDate,
                                                    dueDate = @dueDate,
                                                    payOffDate = @payOffDate
                                                    WHERE receivableID = @receivableID;";

        String sqlStatementReceivableDelete = "DELETE FROM receivables WHERE receivableID = @receivableID;";


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
            Guard.notNull(sourceDataTable, "update source data table");
            int executionResult = -1;

            //Retrieves the changes performed to the surce data table
            DataTable updatedReceivablesDT = sourceDataTable.GetChanges();

            //Re-creates the command used to populate the source data table when data was requestd from the DB
            MySqlCommand receivableRetrievalCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementReceivableRetrieval, paramContainer);

            //Creates the command that will be used to update the data
            MySqlCommand updateReceivablesCommand = new MySqlCommand(sqlStatementReceivableUpdate);
            updateReceivablesCommand.Parameters.Add("@receivableName", MySqlDbType.VarChar, 50, "Receivable name");
            updateReceivablesCommand.Parameters.Add("@debtorName", MySqlDbType.VarChar, 30, "Debtor name");
            updateReceivablesCommand.Parameters.Add("@totalPaidAmount", MySqlDbType.Int32, 20, "Total paid amount");
            updateReceivablesCommand.Parameters.Add("@receivableValue", MySqlDbType.Int32, 20, "Receivable value");
            updateReceivablesCommand.Parameters.Add("@statusDescription", MySqlDbType.VarChar, 50, "Status");
            updateReceivablesCommand.Parameters.Add("@createdDate", MySqlDbType.Date, 10, "Creation date");
            updateReceivablesCommand.Parameters.Add("@dueDate", MySqlDbType.Date, 10, "Due date");
            updateReceivablesCommand.Parameters.Add("@payOffDate", MySqlDbType.Date, 10, "Pay off date");

            //Sets the parameter which contains the primary key for each updated row
            MySqlParameter receivableIDParameter = new MySqlParameter("@receivableID", MySqlDbType.Int32, 20, "Receivable ID");
            receivableIDParameter.SourceColumn = "Receivable ID";
            receivableIDParameter.SourceVersion = DataRowVersion.Original;

            executionResult = DBConnectionManager.updateData(receivableRetrievalCommand, updateReceivablesCommand, receivableIDParameter, updatedReceivablesDT);


            return executionResult;
        }

        public int deleteData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            Guard.notNull(sourceDataTable, "update source data table");
            int executionResult = -1;

            DataTable deletedReceivableDT = sourceDataTable.GetChanges();

            MySqlCommand dataRetrievalCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementReceivableRetrieval, paramContainer);

            MySqlCommand deleteReceivableCommand = new MySqlCommand(sqlStatementReceivableDelete);

            MySqlParameter receivableIDParameter = new MySqlParameter("@receivableID", MySqlDbType.Int32, 20, "Receivable ID");
            receivableIDParameter.SourceColumn = "Receivable ID";
            receivableIDParameter.SourceVersion = DataRowVersion.Original;


            executionResult = DBConnectionManager.deleteData(dataRetrievalCommand, deleteReceivableCommand, receivableIDParameter, sourceDataTable);

            return executionResult;
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
    }
}
