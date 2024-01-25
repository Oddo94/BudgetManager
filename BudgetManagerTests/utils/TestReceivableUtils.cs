using BudgetManager;
using BudgetManager.mvc.models.dto;
using BudgetManager.non_mvc;
using BudgetManager.utils.enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManagerTests.utils {
    internal class TestReceivableUtils {
        private String receivableName;
        private int receivableValue;
        private int totalPaidAmount;
        private String debtorName;
        private String sourceAccountName;
        private ReceivableStatus receivableStatus;
        private String receivableCreationDate;
        private String receivableDueDate;
        private int userId;

        private String sqlStatementDeleteTestReceivable = "DELETE FROM receivables WHERE name = @paramName";

        public TestReceivableUtils(String receivableName, int receivableValue, int totalPaidAmount, String debtorName, String sourceAccountName, ReceivableStatus receivableStatus, String receivableCreationDate, String receivableDueDate, int userId) {
            this.receivableName = receivableName;
            this.receivableValue = receivableValue;
            this.totalPaidAmount = totalPaidAmount;
            this.debtorName = debtorName;
            this.sourceAccountName = sourceAccountName;
            this.receivableStatus = receivableStatus;
            this.receivableCreationDate = receivableCreationDate;
            this.receivableDueDate = receivableDueDate;
            this.userId = userId;
        }

        public int insertTestReceivableIntoDb() {
            ReceivableDTO dataInsertionDTO = new ReceivableDTO(receivableName, receivableValue, debtorName, sourceAccountName, totalPaidAmount, receivableStatus, receivableCreationDate, receivableDueDate, userId);
            
            ReceivableInsertionStrategy receivableInsertionStrategy = new ReceivableInsertionStrategy();
            DataInsertionContext dataInsertionContext = new DataInsertionContext();
            dataInsertionContext.setStrategy(receivableInsertionStrategy);

            int executionResult = dataInsertionContext.invoke(dataInsertionDTO);

            return executionResult;
        }

        public int deleteTestReceivableFromDb() {
            MySqlCommand deleteTestReceivableCommand = new MySqlCommand(sqlStatementDeleteTestReceivable);
            deleteTestReceivableCommand.Parameters.AddWithValue("@paramName", receivableName);

            int executionResult = DBConnectionManager.deleteData(deleteTestReceivableCommand);

            return executionResult;

        }
    }
}
