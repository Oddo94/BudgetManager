using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    class IncomeInsertionStrategy : DataInsertionStrategy {
        //SQL queries for inserting incomes, expenses, debts and savings
        private String sqlStatementInsertIncome = @"INSERT INTO incomes(user_ID, name, incomeType, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";

        //SQL queries for selecting the ID's used in the INSERT commands
        private String sqlStatementSelectIncomeTypeID = @"SELECT typeID FROM income_types WHERE typeName = @paramTypeName";


        public int execute(QueryData paramContainer) {           
            //Getting the necessary data
            int userID = paramContainer.UserID;
            String incomeName = paramContainer.ItemName;
            int incomeTypeID = getID(sqlStatementSelectIncomeTypeID, paramContainer.TypeName);//Ia ca argumente fraza SQL si denumirea tipului de venit selectat
            int incomeValue = Convert.ToInt32(paramContainer.ItemValue);
            String incomeDate = paramContainer.ItemCreationDate; //Obtinere data sub forma de String

            //Creating command for income insertion
            MySqlCommand incomeInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementInsertIncome, userID, incomeName, incomeTypeID, incomeValue, incomeDate);
            //Getting the execution command result
            int executionResult = DBConnectionManager.insertData(incomeInsertionCommand);

            return executionResult;
        }


        private int getID(String sqlStatement, String typeName) {
            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", typeName);

            DataTable typeIDTable = DBConnectionManager.getData(getTypeIDCommand);

            if (typeIDTable != null && typeIDTable.Rows.Count == 1) {
                int typeID = Convert.ToInt32(typeIDTable.Rows[0].ItemArray[0]);
                return typeID;
            }


            return -1;
        }
    }
}
