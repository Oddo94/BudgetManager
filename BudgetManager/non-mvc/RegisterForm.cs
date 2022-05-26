using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    public partial class RegisterForm : Form {
        private TextBox[] textBoxes;
        private int minimumPasswordLength;
        private String sqlStatementCheckUserExistence = "SELECT userID, username FROM users WHERE username = @paramUserName";
        private String sqlStatementCreateNewUser = @"INSERT INTO users(username, salt, password, email) VALUES(@paramUserName, @paramSalt, @paramHashCode, @paramEmailAddress)";
        private String sqlStatementCreateDefaultSavingAccount = @"INSERT INTO saving_accounts(accountName, user_ID, type_ID, bank_ID, currency_ID, creationDate) 
                                                                  VALUES(@paramAccountName, 
                                                                        (SELECT userID FROM users WHERE username = @paramUserName), 
                                                                        (SELECT typeID FROM saving_account_types WHERE typeName = @paramAccountTypeName), 
                                                                        (SELECT bankID FROM banks WHERE bankName = @paramBankName), 
                                                                        (SELECT currencyID FROM currencies WHERE currencyName = @paramCurrencyName)
                                                                        ,@paramCreationDate)";

        public RegisterForm() {
            InitializeComponent();
            textBoxes = new TextBox[] { userNameTextBox, passwordTextBox, emailTextBox };
            minimumPasswordLength = 10;
        }

        private void userNameTextBox_TextChanged(object sender, EventArgs e) {
            toggleButtonState(registerButton, textBoxes);
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e) {
            toggleButtonState(registerButton, textBoxes);
        }

        private void emailTextBox_TextChanged(object sender, EventArgs e) {
            toggleButtonState(registerButton, textBoxes);
        }

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (showPasswordCheckBox.Checked == true) {
                passwordTextBox.UseSystemPasswordChar = false;
            } else {
                passwordTextBox.UseSystemPasswordChar = true;

            }
        }

        private void registerButton_Click(object sender, EventArgs e) {
            string userName = userNameTextBox.Text;
            string password = passwordTextBox.Text;
            string emailAddress = emailTextBox.Text;

            if (!isValidUserName(userName)) {
                MessageBox.Show("The username must have at least 3 characters and can contain only lowercase(a-z) and uppercase(A-Z) letters, digits(0-9) and underscores(_)!", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (password.Length < minimumPasswordLength) {
                MessageBox.Show("Your password should be at least 10 characters long! Please try again.", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isValidPassword(password)) {
                MessageBox.Show("Invalid password! Your password must contain:\n1.Lowercase and uppercase letters (a-zA-z) \n2.Digits (0-9) \n3.Special characters (@#$%<>?)", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!isValidEmail(emailAddress)) {
                MessageBox.Show("Invalid email address!", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (userExists(getUser(sqlStatementCheckUserExistence, userName))) {
                MessageBox.Show("The selected username already exists! Please try again", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            ConfirmationSender emailSender = new ConfirmationSender();

            string emailSubject = "New user creation";
            string emailBody = "A user creation request was made for an account that will associated to this email address.\nPlease enter the following code to finish user creation process and confirm your email: {0} \nIf you have not made such a request please ignore this email and delete it.";
            string onSuccessMessage = "An email containing the confirmation code for the new user creation was sent to the specified email address";
            string parentWindowName = "Register";
            
            string generatedConfirmationCode = emailSender.generateConfirmationCode();
            emailSender.sendConfirmationEmail(emailAddress, emailSubject, emailBody, generatedConfirmationCode, onSuccessMessage, parentWindowName);

            String userInputConfirmationCode = Interaction.InputBox("Enter the code received on your email to finish the user creation process:", "Confirmation Code", "Enter code", 200, 200);

            if (emailSender.confirmationCodesMatch(generatedConfirmationCode, userInputConfirmationCode)) {
                //User creation
                PasswordSecurityManager securityManager = new PasswordSecurityManager();
                byte[] salt = securityManager.getSalt(16);
                string hashCode = securityManager.createPasswordHash(password, salt);
                MySqlCommand userCreationCommand = SQLCommandBuilder.getNewUserCreationCommand(sqlStatementCreateNewUser, userName, salt, hashCode, emailAddress);
                int userCreationResult = DBConnectionManager.insertData(userCreationCommand);

                //Default saving account creation
                int savingAccountCreationResult = createDefaultSavingAccount(sqlStatementCreateDefaultSavingAccount, userName); 

                if (userCreationResult == -1) {
                    MessageBox.Show("Could not create the requested user!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (savingAccountCreationResult == -1) {
                    MessageBox.Show("Could not create the default saving account for the registered user! Please contact the application administrator for fixing this issue.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }          

                MessageBox.Show("Your user was succesfully created!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearInputFields(textBoxes);
                registerButton.Enabled = false;


            } else {
                MessageBox.Show("Invalid confirmation code! Please try again.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         
        }

        private void loginRedirectButton_Click(object sender, EventArgs e) {
            this.Visible = false;
            new LoginForm().Visible = true;
        }

        


        private void toggleButtonState(Button targetButton, TextBox[] textBoxes) {
            //Regex care identifica existenta unuia sau mai multor spatii goale        
            Regex regex = new Regex("^\\s+");
            bool hasValidData = true;

            foreach (TextBox currentTextBox in textBoxes) {
                //Se verifica daca exista date in campul curent respectiv daca acesta contine doar spatii
                if (currentTextBox.Text.Length == 0 || regex.IsMatch(currentTextBox.Text)) {
                    hasValidData = false;
                    break;
                }
            }

            //In functie de indeplinirea/neindeplinirea conditiilor se activeaza sau se dezactiveaza butonul furnizat ca argument
            if (hasValidData) {
                targetButton.Enabled = true;
            } else {
                targetButton.Enabled = false;
            }
        }

        private bool isValidUserName(String userName) {
            //Regex ce verifica daca numele de utilizator contine doar litere mari,mici,cifre si underscore 
            Regex validationRegex = new Regex("^[a-zA-Z0-9_]{3,}$");

            if (validationRegex.IsMatch(userName)) {
                return true;
            }

            return false;
        }

        private bool isValidPassword(String password) {
            //Verifica daca parola contine litere mari, litere mici si cifre
            Regex firstRegexPattern = new Regex("^(?=.*[a - z])(?=.*[A - Z])(?=.*[\\d]).+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //Verifca daca parola contine caractere speciale
            Regex secondRegexPattern = new Regex(".*[!@#\\$%^&*()_\\+\\-\\=\\[\\[{};'\\:\"\\|,.\\/<>\\?`~]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);


            MatchCollection match1 = firstRegexPattern.Matches(password);
            MatchCollection match2 = secondRegexPattern.Matches(password);


            if (firstRegexPattern.IsMatch(password) && secondRegexPattern.IsMatch(password)) {
                return true;
            }

            return false;
        }

        private Boolean isValidEmail(String inputEmailAddress) {
            //Se creaza un obiect de tip MailAddress
            try {
                MailAddress address = new MailAddress(inputEmailAddress);

                //Se compara adresa de email a obiectului nou creat cu adresa furnizata si se returneaza valoarea booleana corespunzatoare
                return address.Address.Equals(inputEmailAddress);

                //Se trateaza exceptia ridicata daca adresa este  reprezentata de un sir vid ""             
            } catch (ArgumentException ex) {
                Console.WriteLine(ex.Message);
                return false;
                //Se trateaza exceptia ridicata daca adresa nu are format valid
            } catch (FormatException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //private int getIdForNewlyRegisteredUser(String sqlStatement, String userName) {
        //    if(sqlStatement == null || userName == null) {
        //        return -1;
        //    }

        //    MySqlCommand getUserIdCommand = new MySqlCommand(sqlStatement);
        //    getUserIdCommand
        //}

        private int createDefaultSavingAccount(String sqlStatement, String userName) {
            if(userName == null) {
                return -1;
            }

            String defaultAccountName = "SYSTEM_DEFINED_SAVING_ACCOUNT";
            String accountTypeName = "SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT";
            String bankName = "NO_BANK";
            String currencyName = "RON";
            String accountCreationDate = DateTime.Now.ToString("yyyy-MM-dd");

            QueryData paramContainer = new QueryData.Builder()
                .addItemName(defaultAccountName)
                .addUserName(userName)
                .addTypeName(accountTypeName)
                .addBankName(bankName)
                .addCurrencyName(currencyName)
                .addItemCreationDate(accountCreationDate)
                .build();

            MySqlCommand getAccountCreationCommand = SQLCommandBuilder.getDefaultSavingAccountCreationCommand(sqlStatementCreateDefaultSavingAccount, paramContainer);

            int executionResult = DBConnectionManager.insertData(getAccountCreationCommand);

            //If a value greater than 0 is returned by the data insertion method it means that the operation was successful
            if(executionResult > 0) {
                return 0;
            }

            return -1; ;
        }

        //Metoda de verificare a existentei utilizatorului
        private bool userExists(DataTable inputDataTable) {
            //Se verifica daca obiectul de tip DataTable furnizat contine date
            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }

        //Metoda ce executa comanda SQL de interogare a bazei de date cu privire la utilizatorul specificat
        private DataTable getUser(String sqlStatement, String userName) {
            MySqlCommand userExistenceCheckCommand = new MySqlCommand(sqlStatement);
            userExistenceCheckCommand.Parameters.AddWithValue("@paramUserName", userName);

            return DBConnectionManager.getData(userExistenceCheckCommand);
        }


        private void clearInputFields(TextBox[] textBoxes) {
            foreach (TextBox currentTextBox in textBoxes) {
                currentTextBox.Text = "";
            }
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e) {
            new LoginForm().Visible = true;
        }

    }    
}
