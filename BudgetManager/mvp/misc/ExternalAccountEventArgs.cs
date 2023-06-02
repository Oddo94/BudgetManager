using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.misc {
    internal class ExternalAccountEventArgs : EventArgs {
        private int userId;
        private String accountName;

        public ExternalAccountEventArgs(int userId, String accountName) {
            this.userId = userId;
            this.accountName = accountName;
        }

        public ExternalAccountEventArgs() { }

        public int UserId { get => userId; set => userId = value; }
        public String AccountName { get => accountName; set => accountName = value; }
    }
}
