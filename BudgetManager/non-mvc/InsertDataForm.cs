using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {

    public enum BudgetItemType {
        INCOME,
        EXPENSE,
        DEBT,
        SAVING,
        UNSPECIFIED
    }

    public partial class InsertDataForm : Form {
     
        private int userID;
        private Control[] inputFields;
        private bool hasClearedFields = false;

        //Fraze SQL pt selectarea valorilor ce populeaza combobox-uri
        private String sqlStatementSelectIncomeTypes = @"SELECT typeName FROM income_types";
        private String sqlStatementSelectExpenseTypes = @"SELECT categoryName FROM expense_types";
        private String sqlStatementSelectCreditors = @"SELECT creditorName
                FROM users INNER JOIN users_creditors ON users.userID = users_creditors.user_ID
                INNER JOIN creditors ON users_creditors.creditor_ID = creditors.creditorID
                WHERE users_creditors.user_ID = @paramUserID";
        private String sqlStatementCheckCreditorExistence = @"SELECT creditorName FROM creditors WHERE creditorName = @paramCreditorName";

        //Fraze SQL pt selectarea ID-urilor folosite in comenzile INSERT
        private String sqlStatementSelectIncomeTypeID = @"SELECT typeID FROM income_types WHERE typeName = @paramTypeName";
        private String sqlStatementSelectExpenseTypeID = @"SELECT categoryID FROM expense_types WHERE categoryName = @paramTypeName";
        private String sqlStatementSelectCreditorID = @"SELECT creditorID FROM creditors WHERE creditorName = @paramTypeName";

        //Fraza SQL pt verificarea existentei unui creditor in lista de creditori a utilizatorului curent
        private String sqlStatementCheckCreditorExistenceInUserList = @"SELECT user_ID, creditor_ID FROM users_creditors WHERE user_ID = @paramUserID AND creditor_ID = @paramCreditorID";


        //Fraze SQL pt introducere venituri, cheltuieli, datorii,economii, creditori si id de creditor
        private String sqlStatementInsertIncome = @"INSERT INTO incomes(user_ID, name, incomeType, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";
        private String sqlStatementInsertExpense = @"INSERT INTO expenses(user_ID, name, type, value, date) VALUES(@paramID, @paramItemName, @paramTypeID, @paramItemValue, @paramItemDate)";
        private String sqlStatementInsertDebt = @"INSERT INTO debts(user_ID, name, value, creditor_ID, date) VALUES(@paramID, @paramDebtName, @paramDebtValue, @paramCreditorID, @paramDebtDate)";
        private String sqlStatementInsertSaving = @"INSERT INTO savings(user_ID, name, value, date) VALUES(@paramID, @paramSavingName, @paramSavingValue, @paramSavingDate)";
        private String sqlStatementInsertCreditor = @"INSERT INTO creditors(creditorName) VALUES(@paramCreditorName)";
        private String sqlStatementInsertCreditorID = @"INSERT INTO users_creditors(user_ID, creditor_ID) VALUES(@paramUserID, @paramCreditorID)";

        //SQL phrases for getting the total value of budget elements for a month in order to allow further checks
        private String sqlStatementSingleMonthIncomes = @"SELECT SUM(value) from incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthExpenses = @"SELECT SUM(value) from expenses WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthDebts = @"SELECT SUM(value) from debts WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthSavings = @"SELECT SUM(value) from savings WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";

        public InsertDataForm(int userID) {
            InitializeComponent();
            this.userID = userID;
            inputFields = new Control[] { nameTextBox, valueTextBox, incomeTypeComboBox, expenseTypeComboBox, creditorNameComboBox };

            //Populare incomeTypeComboBox cu tipurile de venituri
            MySqlCommand fillIncomeTypesCommand = new MySqlCommand(sqlStatementSelectIncomeTypes);
            DataTable dTableIncomeTypes = DBConnectionManager.getData(fillIncomeTypesCommand);

            fillComboBox(incomeTypeComboBox, dTableIncomeTypes);

            //Populare expenseTypeComboBox cu tipurile de cheltuieli
            MySqlCommand fillExpenseTypesCommand = new MySqlCommand(sqlStatementSelectExpenseTypes);
            DataTable dTableExpenseTypes = DBConnectionManager.getData(fillExpenseTypesCommand);

            fillComboBox(expenseTypeComboBox, dTableExpenseTypes);
        
            fillCreditorsComboBox();

            //Dezactivarea campurilor de introducere a datelor
            toggleInputFormFieldsState(inputFields, false);
        }


        //CONTROLS METHODS
        //La selectarea fiecarei optiuni se curata campurile de introducere date si se activeaza doar campurile necesare pentru introducerea elementului selectat in combobox      
        private void budgetItemComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedIndex = budgetItemComboBox.SelectedIndex;
                      
            switch (selectedIndex) {
                //Venituri
                case 0:
                    clearFields(inputFields);//Golire campuri completate anterior
                    toggleInputFormFieldsState(inputFields, true);//Activarea tuturor campurilor 
                    //incomeTypeComboBox.Enabled = true;
                    expenseTypeComboBox.Enabled = false;//Dezactivare campuri care nu sunt necesare pt introducerea de date legate de elementul curent selectat
                    creditorNameComboBox.Enabled = false;
                    break;

                //Cheltuieli
                case 1:
                    clearFields(inputFields);
                    toggleInputFormFieldsState(inputFields, true);
                    incomeTypeComboBox.Enabled = false;
                    //expenseTypeComboBox.Enabled = true;
                    creditorNameComboBox.Enabled = false;
                    break;

                //Datorii
                case 2:
                    clearFields(inputFields);
                    fillCreditorsComboBox();
                    toggleInputFormFieldsState(inputFields, true);
                    incomeTypeComboBox.Enabled = false;
                    expenseTypeComboBox.Enabled = false;
                    //creditorNameComboBox.Enabled = true;
                    break;

                //Economii
                case 3:
                    clearFields(inputFields);
                    toggleInputFormFieldsState(inputFields, true);
                    incomeTypeComboBox.Enabled = false;
                    expenseTypeComboBox.Enabled = false;
                    creditorNameComboBox.Enabled = false;
                    break;

                //Creditori
                case 4:
                    clearFields(inputFields);
                    toggleInputFormFieldsState(inputFields, true);
                    valueTextBox.Enabled = false;
                    incomeTypeComboBox.Enabled = false;
                    expenseTypeComboBox.Enabled = false;
                    creditorNameComboBox.Enabled = false;
                    break;
            }        
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e) {
            //Regex pt identificarea cifrelor
            Regex numberRegex = new Regex("\\b[\\d]+\\b", RegexOptions.Compiled);
         
            //Daca in camp se introduc alte caractere in afara de cifre atunci continutul textbox-ului va fi sters automat
            if (!numberRegex.IsMatch(valueTextBox.Text)) {
                valueTextBox.Text = "";
            }
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }


        private void incomeTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void expenseTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void creditorNameComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (hasDataOnActiveFields(inputFields)) {
                addEntryButton.Enabled = true;
            } else {
                addEntryButton.Enabled = false;
            }
        }

        private void addEntryButton_Click(object sender, EventArgs e) {
            //Indexul elementului curent selectat
            int selectedItemIndex = budgetItemComboBox.SelectedIndex;

            DialogResult userOption = MessageBox.Show("Are you sure that you want to insert the provided data?", "Data insertion", MessageBoxButtons.YesNo);

            if (userOption == DialogResult.No) {
                return;
            }

            //Checks if the user has enough money left to insert the selected item value
           

            String selectedItem = budgetItemComboBox.Text;
            if (!"Creditor".Equals(selectedItem, StringComparison.InvariantCultureIgnoreCase)) {
                int insertedValue = Convert.ToInt32(valueTextBox.Text);
                int selectedMonth = newEntryDateTimePicker.Value.Month;
                int selectedYear = newEntryDateTimePicker.Value.Year;
                QueryData paramContainer = new QueryData(userID, selectedMonth, selectedYear);

                if (!hasEnoughMoney(insertedValue, paramContainer)) {
                    MessageBox.Show( String.Format("The inserted value for the current {0} is higher than the available amount! You cannot exceed the maximum incomes for the current month.", selectedItem), "Data insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            int executionResult = 0;
            switch (selectedItemIndex) {
                //Inserare venituri
                case 0:
                    //Obtinere date pt inserare venituri
                    String incomeName = nameTextBox.Text;
                    int incomeTypeID = getID(sqlStatementSelectIncomeTypeID, incomeTypeComboBox.Text);//Ia ca argumente fraza SQL si denumirea tipului de venit selectat
                    int incomeValue = Convert.ToInt32(valueTextBox.Text);
                    String incomeDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd"); //Obtinere data sub forma de String

                    //Creare comanda de inserare venituri
                    MySqlCommand incomeInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementInsertIncome, userID, incomeName, incomeTypeID, incomeValue, incomeDate);
                    //Rezultat executie comanda
                    executionResult = DBConnectionManager.insertData(incomeInsertionCommand);
                    break;

                case 1:
                    //Obtinere date pt inserare cheltuieli
                    String expenseName = nameTextBox.Text;
                    int expenseTypeID = getID(sqlStatementSelectExpenseTypeID, expenseTypeComboBox.Text);
                    int expenseValue = Convert.ToInt32(valueTextBox.Text);
                    String expenseDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");//Obtinere data sub forma de String

                    //Creare comanda de inserare cheltuieli
                    MySqlCommand expenseInsertionCommand = SQLCommandBuilder.getInsertCommandForMultipleTypeItem(sqlStatementInsertExpense, userID, expenseName, expenseTypeID, expenseValue, expenseDate);
                    //Rezultat executie comanda
                    executionResult = DBConnectionManager.insertData(expenseInsertionCommand);
                    break;

                case 2:
                    //Obtinere date pt inserare datorii
                    String debtName = nameTextBox.Text;
                    int debtValue = Convert.ToInt32(valueTextBox.Text);
                    int creditorID = getID(sqlStatementSelectCreditorID, creditorNameComboBox.Text);
                    String debtDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");

                    //Creare comanda de inserare datorii
                    MySqlCommand debtInsertionCommand = SQLCommandBuilder.getDebtInsertionCommand(sqlStatementInsertDebt, userID, debtName, debtValue, creditorID, debtDate);
                    executionResult = DBConnectionManager.insertData(debtInsertionCommand);
                    break;

                case 3:
                    //Obtinere date pt inserare economii
                    String savingName = nameTextBox.Text;
                    int savingValue = Convert.ToInt32(valueTextBox.Text);
                    String savingDate = newEntryDateTimePicker.Value.ToString("yyyy-MM-dd");

                    //Creare comanda de inserare economii
                    MySqlCommand savingInsertionCommand = SQLCommandBuilder.getSavingInsertionCommand(sqlStatementInsertSaving, userID, savingName, savingValue, savingDate);               
                    executionResult = DBConnectionManager.insertData(savingInsertionCommand);
                    break;


                case 4:
                    //Inserare creditor nou

                    //Verificare existenta creditor introdus in tabelul de creditori
                    MySqlCommand creditorSelectionCommand = new MySqlCommand(sqlStatementCheckCreditorExistence);
                    creditorSelectionCommand.Parameters.AddWithValue("@paramCreditorName", nameTextBox.Text);
                    if (entryIsPresent(creditorSelectionCommand, nameTextBox.Text)) {
                        DialogResult userChoice = MessageBox.Show("The provided creditor name already exists. Do you want to add it to your creditors list?", "Data insertion", MessageBoxButtons.YesNoCancel);
                        if (userChoice == DialogResult.Yes) {
                            //Verificare daca creditorul introdus este prezent deja in lista de creditori a utilizatorului curent
                            MySqlCommand creditorPresenceInListCommand = new MySqlCommand(sqlStatementCheckCreditorExistenceInUserList);
                            creditorPresenceInListCommand.Parameters.AddWithValue("@paramUserID", userID);
                            creditorPresenceInListCommand.Parameters.AddWithValue("@paramCreditorID", getID(sqlStatementSelectCreditorID, nameTextBox.Text));//Se incearca obtinerea id-ului creditorului a carui nume a fost introdus pt a se verfica daca el exista deja in lista de creditori a utilizatorului
                            if (isPresentInUserCreditorList(creditorPresenceInListCommand)) {
                                MessageBox.Show("The provided creditor is already present in your creditor list and cannot be assigned again! Please enter a different creditor", "Data insertion");
                                return;
                            } else {
                                //Daca creditorul exista deja si nu este atribuit utilizatorului atunci se va crea o noua inregistrare in tabelul users_creditors
                                MySqlCommand creditorIDInsertCommandForExistingEntry = new MySqlCommand(sqlStatementInsertCreditorID);
                                creditorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramUserID", userID);
                                creditorIDInsertCommandForExistingEntry.Parameters.AddWithValue("@paramCreditorID", getID(sqlStatementSelectCreditorID, nameTextBox.Text));
                                executionResult = DBConnectionManager.insertData(creditorIDInsertCommandForExistingEntry);

                            }
                                                    
                        } else {
                            //Daca optiunea utilizatorului este 'No' se iese din metoda
                            return;
                        }
                    } else {
                        //Inserare creditor nou in tabelul de creditori
                        MySqlCommand creditorInsertCommand = new MySqlCommand(sqlStatementInsertCreditor);
                        creditorInsertCommand.Parameters.AddWithValue("@paramCreditorName", nameTextBox.Text);
                        executionResult = DBConnectionManager.insertData(creditorInsertCommand);

                        //Inserare id creditor nou creat in tabelul de legatura users_creditors pt a atribui noua inregistrare utilizatorului curent
                        MySqlCommand creditorIDInsertCommand = new MySqlCommand(sqlStatementInsertCreditorID);
                        creditorIDInsertCommand.Parameters.AddWithValue("@paramUserID", userID);
                        creditorIDInsertCommand.Parameters.AddWithValue("@paramCreditorID", getID(sqlStatementSelectCreditorID, nameTextBox.Text));
                        executionResult = DBConnectionManager.insertData(creditorIDInsertCommand);
                    }
                    break;



                default:
                    MessageBox.Show("Invalid option", "Warning");
                    break;
            }

            if (executionResult != -1) {
                MessageBox.Show("Data inserted successfully!", "Data insertion");
            } else {
                MessageBox.Show("Unable to insert the input data! Please try again.", "Data insertion");
            }

        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Visible = false;
        }

        private void resetButton_Click(object sender, EventArgs e) {
            clearFields(inputFields);
        }


        //UTIL METHODS
        private void fillBudgetItemElements(ComboBox cBox, String[] inputElements) {

            foreach (String element in inputElements) {
                cBox.Items.Add(element);
            }
        }

        private void setComponentVisibility(Control[] components, Boolean isVisible) {
            foreach (Control currentComponent in components) {
                currentComponent.Visible = isVisible;
            }

        }

        private void toggleInputFormFieldsState(Control[] fields, bool enabled) {
            foreach (Control field in fields) {
                field.Enabled = enabled;
            }

        }

        private void clearFields(Control[] fields) {
            hasClearedFields = true;
            foreach (Control field in fields) {
                if (field.GetType() == typeof(ComboBox)) {
                    ((ComboBox)field).SelectedIndex = -1;
                }

                field.Text = "";

            }
        }

        private bool hasDataOnActiveFields(Control[] fields) {
            if (hasClearedFields) {
                hasClearedFields = false;
                return false;
            }

            foreach (Control field in fields) {
                if (field.Enabled == true && "".Equals(field.Text)) {
                    return false;
                }
            }

            return true;
        }

        private void fillComboBox(ComboBox comboBox, DataTable dataTable) {

            for (int i = 0; i < dataTable.Rows.Count; i++) {
                String currentElement = (String)dataTable.Rows[i].ItemArray[0];
                comboBox.Items.Add(currentElement);
            }

        }

        private void fillCreditorsComboBox() {     
            MySqlCommand fillCreditorsCommand = new MySqlCommand(sqlStatementSelectCreditors);
            fillCreditorsCommand.Parameters.AddWithValue("@paramUserID", userID);

            DataTable dTableCreditors = DBConnectionManager.getData(fillCreditorsCommand);

            creditorNameComboBox.DataSource = dTableCreditors;
            creditorNameComboBox.DisplayMember = "creditorName";
        }

        private int getID(String sqlStatement, String typeName) {       
            MySqlCommand getTypeIDCommand = new MySqlCommand(sqlStatement);
            getTypeIDCommand.Parameters.AddWithValue("@paramTypeName", typeName);
           
            DataTable typeIDTable = DBConnectionManager.getData(getTypeIDCommand);

            if (typeIDTable!= null && typeIDTable.Rows.Count == 1) {
               int typeID = Convert.ToInt32(typeIDTable.Rows[0].ItemArray[0]);
                return typeID;
            }
            

            return -1;
        }

        private bool entryIsPresent(MySqlCommand command, String entryName) {
            //Executa comanda de obtinere a datelor folosind numele creditorului introdus        
            DataTable entryDataTable = DBConnectionManager.getData(command);
           
            if (entryDataTable != null) {
                if (entryDataTable.Rows.Count > 0) {
                    for (int i = 0; i < entryDataTable.Rows.Count; i++) {
                        //Verifica daca numele creditorului obtinut in urma rularii comenzii este acelasi cu numele creditorului ce se doreste a fi introdus(verificarea se face cu ignorarea caracterelor mari/mici-case insensitive)
                        if (entryName.Equals(entryDataTable.Rows[i].ItemArray[0].ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        private bool isPresentInUserCreditorList(MySqlCommand command) {
            DataTable creditorListPresenceTable = DBConnectionManager.getData(command);

            if (creditorListPresenceTable != null && creditorListPresenceTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }

        private bool hasEnoughMoney(int valueToInsert, QueryData paramContainer) {        
                    
            int totalIncomes = getTotalValueForSelectedElement(BudgetItemType.INCOME, sqlStatementSingleMonthIncomes, paramContainer);
            int totalExpenses = getTotalValueForSelectedElement(BudgetItemType.EXPENSE, sqlStatementSingleMonthExpenses, paramContainer);
            int totalDebts = getTotalValueForSelectedElement(BudgetItemType.DEBT, sqlStatementSingleMonthDebts, paramContainer);
            int totalSavings = getTotalValueForSelectedElement(BudgetItemType.SAVING, sqlStatementSingleMonthSavings, paramContainer);

            int amountLeft = getAvailableAmount(totalIncomes,totalExpenses, totalDebts, totalSavings);

            if (valueToInsert <= amountLeft) {
                return true;
            }

            return false;
        }


        private int getAvailableAmount(int totalIncomes, int totalExpenses, int totalDebts, int totalSavings) {

            return totalIncomes - (totalExpenses + totalDebts + totalSavings);

        }


        private int getTotalValueForSelectedElement(BudgetItemType itemType, String sqlStatement, QueryData paramContainer) {
            int totalValue = 0;

            MySqlCommand command = getCommand(itemType, sqlStatement, paramContainer);

            if (command == null) {
                return -1;
            }

            DataTable resultDataTable = DBConnectionManager.getData(command);

            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                Object result = resultDataTable.Rows[0].ItemArray[0];
                totalValue = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                return totalValue;
            }

            return -1;

        }
        

        private MySqlCommand getCommand(BudgetItemType itemType, String sqlStatement, QueryData paramContainer) {
            switch (itemType) {
                case BudgetItemType.INCOME:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.EXPENSE:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.DEBT:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.SAVING:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                default:
                    return null;
            }
        }

        private BudgetItemType getSelectedType(ComboBox comboBox) {
            int selectedIndex = comboBox.SelectedIndex;

            switch (selectedIndex) {
                case 1:
                    return BudgetItemType.EXPENSE;

                case 2:
                    return BudgetItemType.DEBT;

                case 3:
                    return BudgetItemType.SAVING;

                default:
                    return BudgetItemType.UNSPECIFIED;
            }
        }
    }
}
