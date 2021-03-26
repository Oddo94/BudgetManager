using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    //Clasa utilitara ca ajuta la crearea unui obiect care incapsuleaza datele necesare pt executia frazei SQL(parametri efectivi ce vor fi folositi pt inlocuirea placeholders)
    public class QueryData {
        private int userID;
        private int month;
        private int year;
        private String startDate;
        private String endDate;
        private String tableName;
        private String budgetPlanName;
        private String typeName;
        private int expenseLimit;
        private int debtLimit;
        private int savingLimit;
        private int estimatedIncome;        
        private int planTypeID;
        private int thresholdPercentage;
        private int alarmExistenceValue;

        private QueryData() {

        }

        public int UserID {
            get {
                return this.userID;
            }
           
        }
        public int Month {
            get {
                return this.month;
            }         
        }
        public int Year {
            get {
                return this.year;
            }         
        }
     
          public String StartDate {
               get {
                return this.startDate;
              }
         }

        public String EndDate {
            get {
                return this.endDate;
            }
        }

        public String TableName {
            get {
                return this.tableName;
            }
        }

        public int ExpenseLimit {
            get {
                return this.expenseLimit;
            }
        }

        public int DebtLimit {
            get {
                return this.debtLimit;
            }
        }

        public int SavingLimit {
            get {
                return this.savingLimit;
            }
        }

        public int EstimatedIncome {
            get {
                return this.estimatedIncome;
            }
        }

        public int PlanTypeID {
            get {
                return this.planTypeID;
            }
        }

        public int ThresholdPercentage {
            get {
                return this.thresholdPercentage;
            }
        }

        public int AlarmExistenceValue {
            get {
                return this.alarmExistenceValue;
            }
        }

        public String TypeName {
            get {
                return this.typeName;
            }
        }

        public String BudgetPlanName {
            get {
                return this.budgetPlanName;
            }
        }


        public class Builder {
            private int userID;
            private int month;
            private int year;
            private String startDate;
            private String endDate;
            private String tableName;
            private String budgetPlanName;
            private String typeName;
            private int expenseLimit;
            private int debtLimit;
            private int savingLimit;
            private int estimatedIncome;
            private int planTypeID;
            private int thresholdPercentage;
            private int alarmExistenceValue;


            public Builder(int userID) {
                this.userID = userID;
            }

            public Builder addMonth(int month) {
                this.month = month;

                return this;
            }

            public Builder addYear(int year) {
                this.year = year;

                return this;
            }

            public Builder addStartDate(String startDate) {
                this.startDate = startDate;

                return this;
            }

            public Builder addEndDate(String endDate) {
                this.endDate = endDate;

                return this;
            }

            public Builder addTableName(String tableName) {
                this.tableName = tableName;

                return this;
            }

            public Builder addBudgetPlanName(String budgetPlanName) {
                this.budgetPlanName = budgetPlanName;

                return this;
            }

            public Builder addTypeName(String typeName) {
                this.typeName = typeName;

                return this;
            }

            public Builder addExpenseLimit(int expenseLimit) {
                this.expenseLimit = expenseLimit;

                return this;
            }

            public Builder addDebtLimit(int debtLimit) {
                this.debtLimit = debtLimit;

                return this;
            }

            public Builder addSavingLimit(int savingLimit) {
                this.savingLimit = savingLimit;

                return this;
            }

            public Builder addEstimatedIncome(int estimatedIncome) {
                this.estimatedIncome = estimatedIncome;

                return this;
            }

            public Builder addPlanTypeID(int planTypeID) {
                this.planTypeID = planTypeID;

                return this;
            }

            public Builder addThresholdPercentage(int thresholdPercentage) {
                this.thresholdPercentage = thresholdPercentage;

                return this;
            }

            public Builder addAlarmExistenceValue(int alarmExistenceValue) {
                this.alarmExistenceValue = alarmExistenceValue;

                return this;
            }

            public QueryData build() {
                return new QueryData {
                    userID = this.userID,
                    month = this.month,
                    year = this.year,
                    startDate = this.startDate,
                    endDate = this.endDate,
                    tableName = this.tableName,
                    budgetPlanName = this.budgetPlanName,
                    typeName = this.typeName,
                    expenseLimit = this.expenseLimit,
                    debtLimit = this.debtLimit,
                    savingLimit = this.savingLimit,
                    estimatedIncome = this.estimatedIncome,
                    planTypeID = this.planTypeID,
                    thresholdPercentage = this.thresholdPercentage,
                    alarmExistenceValue = this.alarmExistenceValue
                };
            }
        }
    }
}
