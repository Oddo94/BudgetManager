using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.misc {
    public class ExternalAccountEventArgs : EventArgs {
        private int userId;
        private String accountName;

        public ExternalAccountEventArgs(int userId, String accountName) {
            this.userId = userId;
            this.accountName = accountName;
        }

        private ExternalAccountEventArgs() { }

        public int UserId { get => userId; }
        public String AccountName { get => accountName; }

        public class Builder {
            private int userId;
            private String accountName;

            public int UserId { set => this.userId = value; }
            public String AccountName { set => this.accountName = value; }

            public Builder(int userId) {
                this.userId = userId;
            }

            public Builder addAccountName(String accountName) {
                this.accountName = accountName;

                return this;
            }

            public ExternalAccountEventArgs build() {
                return new ExternalAccountEventArgs {
                    userId = this.userId,
                    accountName = this.accountName
                };
            }
        }
    }
}
