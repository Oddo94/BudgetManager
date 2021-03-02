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
        private int planTypeID;
        private int thresholdPercentage;
        private int alarmExistenceValue;

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
        //Constructorul care initializeaza datele pt un obiect de tip query pt un an intreg
        public QueryData(int userID, int year) {
            this.userID = userID;
            this.year = year;
        }

        //Constructorul care initializeaza datele pt un obiect de tip query pt o singura luna
        public QueryData(int userID, int month, int year) {
            this.userID = userID;
            this.month = month;
            this.year = year;

        }

        public QueryData(int userID, String startDate) {
            this.userID = userID;
            this.startDate = startDate;
        }

        ////Constructorul care initializeaza datele pt un obiect de tip query pt mai multe luni
        public QueryData(int userID, String startDate, String endDate) {
            this.userID = userID;
            this.startDate = startDate;
            this.endDate = endDate;
            
        }

        public QueryData(int userID, int month, int year, String tableName) {
            this.userID = userID;
            this.month = month;
            this.year = year;
            this.tableName = tableName;

        }

        public QueryData(int userID,int year, String tableName) {
            this.userID = userID;          
            this.year = year;
            this.tableName = tableName;

        }

        public QueryData (String typeName) {
            this.typeName = typeName;
        }

        public QueryData(int userID, String planName, int expenseLimit, int debtLimit, int savingLimit, int planTypeID, int thresholdPercentage, int alarmExistenceValue, String startDate, String endDate) {
            this.userID = userID;
            this.budgetPlanName = planName;
            this.expenseLimit = expenseLimit;
            this.debtLimit = debtLimit;
            this.savingLimit = savingLimit;
            this.planTypeID = planTypeID;
            this.thresholdPercentage = thresholdPercentage;
            this.alarmExistenceValue = alarmExistenceValue;
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }
}
