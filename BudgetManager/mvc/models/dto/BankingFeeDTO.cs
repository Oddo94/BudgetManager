using BudgetManager.utils.enums;
using System;

namespace BudgetManager.mvc.models.dto {
    public class BankingFeeDTO : IDataInsertionDTO {
        private String accountName;
        private String name;
        private double value;
        private String description;
        private String createdDate;
        private int userID;

        public BankingFeeDTO() { }

        public BankingFeeDTO(String accountName, String name, double value, String description, String createdDate, int userID) {
            this.accountName = accountName;
            this.name = name;
            this.value = value;
            this.description = description;
            this.createdDate = createdDate;
            this.userID = userID;
        }

        public String AccountName {
            get {
                return this.accountName;
            }

            set {
                this.accountName = value;
            }
        }

        public String Name {
            get {
                return this.name;
            }

            set {
                this.name = value;
            }
        }

        public double Value {
            get {
                return this.value;
            }

            set {
                this.value = value;
            }
        }

        public String Description {
            get {
                return this.description;
            }

            set {
                this.description = value;
            }
        }

        public String CreatedDate {
            get {
                return this.createdDate;
            }

            set {
                this.createdDate = value;
            }
        }

        public int UserID {
            get {
                return this.userID;
            }

            set {
                this.userID = value;
            }
        }

        public BudgetItemType getBudgetItemType() {
            return BudgetItemType.EXTERNAL_ACCOUNT_BANKING_FEE;
        }
    }
}
