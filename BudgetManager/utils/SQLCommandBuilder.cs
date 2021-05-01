using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    //Clasa utilitara folosita pt crearea de comenzi SQL(pentru una sau mai  multe luni)
    class SQLCommandBuilder {

        //Metoda generica pt crearea de comenzi SQL pt o singura luna
        public static MySqlCommand getSingleMonthCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand singleMonthCommand = new MySqlCommand(sqlStatement, DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING));//Nu e neaparata adaugarea conexiunii la crearea comenzii intrucat metoda getData a clasei DBConnectionmanager face deja asta in mod implicit
            singleMonthCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            singleMonthCommand.Parameters.AddWithValue("@paramMonth", paramContainer.Month);
            singleMonthCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            return singleMonthCommand;
        }

        //Metoda pentru selectarea tuturor inregistrarilor de pe un an pt un utilizator
        public static MySqlCommand getFullYearRecordsCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand fullYearRecordsCommand = new MySqlCommand(sqlStatement);
            fullYearRecordsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            fullYearRecordsCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            return fullYearRecordsCommand;

        }

        //Metoda generica pt crearea de comenzi SQL pt mai multe luni
        public static MySqlCommand getMultipleMonthsCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand multipleMonthsCommand = new MySqlCommand(sqlStatement, DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING));//Nu e neaparata adaugarea conexiunii la crearea comenzii intrucat metoda getData a clasei DBConnectionmanager face deja asta in mod implicit
            multipleMonthsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            multipleMonthsCommand.Parameters.AddWithValue("@paramStartDate", paramContainer.StartDate);
            multipleMonthsCommand.Parameters.AddWithValue("@paramEndDate", paramContainer.EndDate);           

            return multipleMonthsCommand;
        }

        //Metoda pt crearea comenzii SQL care aduce date pt column chart care afiseaza situatia anuala pe luni 
        public static MySqlCommand getMonthlyTotalsCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand monthlyTotalsCommand = new MySqlCommand(sqlStatement, DBConnectionManager.getConnection(DBConnectionManager.BUDGET_MANAGER_CONN_STRING));//Nu e neaparata adaugarea conexiunii la crearea comenzii intrucat metoda getData a clasei DBConnectionmanager face deja asta in mod implicit
            monthlyTotalsCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            monthlyTotalsCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            return monthlyTotalsCommand;
        }

        //Metoda pt crearea comenzii SQL ce insereaza un nou utilizator in baza de date
        public static MySqlCommand getNewUserCreationCommand(String sqlStatement, String userName, byte[] salt, String hashCode, String emailAddress) {
            MySqlCommand userCreationCommand = new MySqlCommand(sqlStatement);
            userCreationCommand.Parameters.AddWithValue("@paramUserName", userName);
            userCreationCommand.Parameters.AddWithValue("@paramSalt", salt);
            userCreationCommand.Parameters.AddWithValue("@paramHashCode", hashCode);
            userCreationCommand.Parameters.AddWithValue("@paramEmailAddress", emailAddress);

            return userCreationCommand;
        }

        public static MySqlCommand getUpdatePasswordCommand(String sqlStatement, byte[] salt, String hashCode, int userID) {
            MySqlCommand updatePasswordCommand = new MySqlCommand(sqlStatement);
            updatePasswordCommand.Parameters.AddWithValue("@paramSalt", salt);
            updatePasswordCommand.Parameters.AddWithValue("@paramHashCode", hashCode);
            updatePasswordCommand.Parameters.AddWithValue("@paramID", userID);

            return updatePasswordCommand;
        }

        //Metoda pentru crearea de comenzi pt inserarea de elemente ce au anumite sub-tipuri (ex: venituri, cheltuieli etc)
        public static MySqlCommand getInsertCommandForMultipleTypeItem(String sqlStatement, int userID, String itemName, int typeID, int itemValue, String itemDate) {
            MySqlCommand insertMultipleTypeItemCommand = new MySqlCommand(sqlStatement);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramID", userID );
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramItemName", itemName);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramTypeID", typeID);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramItemValue", itemValue);
            insertMultipleTypeItemCommand.Parameters.AddWithValue("@paramItemDate", itemDate);

            return insertMultipleTypeItemCommand;

        }

        //Metoda de inserare a inregistrarilor in tabelul de datorii
        public static MySqlCommand getDebtInsertionCommand(String sqlStatement, int userID, String debtName, int debtValue, int creditorID, String debtDate) {
            MySqlCommand debtInsertionCommand = new MySqlCommand(sqlStatement);
            debtInsertionCommand.Parameters.AddWithValue("@paramID", userID);
            debtInsertionCommand.Parameters.AddWithValue("@paramDebtName", debtName);
            debtInsertionCommand.Parameters.AddWithValue("@paramDebtValue", debtValue);
            debtInsertionCommand.Parameters.AddWithValue("@paramCreditorID", creditorID);
            debtInsertionCommand.Parameters.AddWithValue("@paramDebtDate", debtDate);

            return debtInsertionCommand;
        }

        //Method for getting the command necessary for inserting savings in the DB table
        public static MySqlCommand getSavingInsertionCommand(String sqlStatement, int userID, String savingName, int savingValue, String savingDate) {
            MySqlCommand savingInsertionCommand = new MySqlCommand(sqlStatement);
            savingInsertionCommand.Parameters.AddWithValue("@paramID", userID);
            savingInsertionCommand.Parameters.AddWithValue("@paramSavingName", savingName);
            savingInsertionCommand.Parameters.AddWithValue("@paramSavingValue", savingValue);
            savingInsertionCommand.Parameters.AddWithValue("@paramSavingDate", savingDate);

            return savingInsertionCommand;
        }

        public static MySqlCommand getBalanceRecordInsertionCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand balanceRecordInsertionCommand = new MySqlCommand(sqlStatement);
            balanceRecordInsertionCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            balanceRecordInsertionCommand.Parameters.AddWithValue("@paramValue", paramContainer.ItemValue);
            balanceRecordInsertionCommand.Parameters.AddWithValue("paramMonth", paramContainer.Month);
            balanceRecordInsertionCommand.Parameters.AddWithValue("paramYear", paramContainer.Year);

            return balanceRecordInsertionCommand;
        }

        //Method for getting the command that helps check if there is an existing budget plan for the specified time interval
        public static MySqlCommand getBudgetPlanCheckCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand budgetPlanCheckCommand = new MySqlCommand(sqlStatement);
            budgetPlanCheckCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            budgetPlanCheckCommand.Parameters.AddWithValue("@paramDate", paramContainer.StartDate);

            return budgetPlanCheckCommand;
        }

        //Method for retrieving the ID of a specified element type from the DB (e.g. budget plan)
        public static MySqlCommand getTypeIDForItemCommand(String sqlStatement, String typeName) {
            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", typeName);

            return getTypeIDCommand;
        } 

        //Method for getting the command used to insert a new budget plan into the DB
        public static MySqlCommand getBudgetPlanCreationCommand(String sqlStatement, QueryData paramContainer) {
            MySqlCommand budgetPlanCreationCommand = new MySqlCommand(sqlStatement);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramPlanName", paramContainer.BudgetPlanName);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramExpenseLimit", paramContainer.ExpenseLimit);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramDebtLimit", paramContainer.DebtLimit);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramSavingLimit", paramContainer.SavingLimit);            
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramPlanTypeID", paramContainer.PlanTypeID);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramAlarmExistence", paramContainer.AlarmExistenceValue);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramThresholdPercentage", paramContainer.ThresholdPercentage);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramStartDate", paramContainer.StartDate);
            budgetPlanCreationCommand.Parameters.AddWithValue("@paramEndDate", paramContainer.EndDate);

            return budgetPlanCreationCommand;
        }
    }
}
