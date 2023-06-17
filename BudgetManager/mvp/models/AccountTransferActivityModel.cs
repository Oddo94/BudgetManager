using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.models {
    internal class AccountTransferActivityModel {
        private List<String> monthsList;
        private List<int> inTransfersValueList;
        List<int> outTransfersValueList;

        public AccountTransferActivityModel(List<String> monthsList, List<int> inTransfersValueList, List<int> outTransfersValueList) {
            this.monthsList = monthsList;
            this.inTransfersValueList = inTransfersValueList;
            this.outTransfersValueList = outTransfersValueList;
        }

        public AccountTransferActivityModel() { }

        public List<String> MonthsList { get => monthsList; set => monthsList = value; }
        public List<int> InTransfersValueList { get => inTransfersValueList; set => inTransfersValueList = value; }
        public List<int> OutTransfersValueList { get => outTransfersValueList; set => outTransfersValueList = value; }
    }
}
