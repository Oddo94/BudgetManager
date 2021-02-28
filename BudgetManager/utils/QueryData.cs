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
    }
}
