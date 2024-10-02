using BudgetManager.utils.data_insertion;
using BudgetManager.utils.enums;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BudgetManager {
    public partial class RegisterForm : Form {
        private TextBox[] textBoxes;
        private int maximumUsernameLength;
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
        private LoginForm loginForm;
        private AccountUtils accountUtils;
        private LoginForm loginForm;
        

        public RegisterForm(LoginForm loginForm) {
            InitializeComponent();
            textBoxes = new TextBox[] { userNameTextBox, passwordTextBox, emailTextBox };
            maximumUsernameLength = 30;
            minimumPasswordLength = 10;

            accountUtils = new AccountUtils();
            this.loginForm = loginForm;
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

            if (userName.Length > maximumUsernameLength) {
                MessageBox.Show("Your username length cannot exceed 30 characters! Please try again.", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < minimumPasswordLength) {
                MessageBox.Show("Your password should be at least 10 characters long! Please try again.", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }         

            if (!isValidPassword(password)) {
                MessageBox.Show("Invalid password! Your password must contain:\n1.Lowercase and uppercase letters (a-zA-z) \n2.Digits (0-9) \n3.Special characters (@#$%<>?)\n4.No whitespaces", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!isValidEmail(emailAddress)) {
                MessageBox.Show("Invalid email address!", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (userExists(getUser(sqlStatementCheckUserExistence, userName))) {
                MessageBox.Show("Invalid username! Please choose a different one and try again.", "User registration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            ConfirmationSender emailSender = new ConfirmationSender();

            string emailSubject = "New user creation";
            string emailBody = "A user creation request was made for an account that will be associated to this email address.\nPlease enter the following code to finish the user creation process and confirm your email: {0} \nIf you haven't made such a request please ignore this email and delete it.";
            string onSuccessMessage = "An email containing the confirmation code for the new user creation was sent to the specified email address.";
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
                String defaultAccountName = "SYSTEM_DEFINED_SAVING_ACCOUNT";
                int savingAccountCreationResult = createDefaultSavingAccount(sqlStatementCreateDefaultSavingAccount, defaultAccountName, userName); 

                if (userCreationResult == -1) {
                    MessageBox.Show("Could not create the requested user!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (savingAccountCreationResult == -1) {
                    MessageBox.Show("Could not create the default saving account for the registered user! Please contact the application administrator for fixing this issue.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                int savingAccountBalanceStorageRecordCreationResult = accountUtils.createAccountBalanceStorageRecordForAccount(userName, null, AccountType.DEFAULT_ACCOUNT, defaultAccountName);

                if (savingAccountBalanceStorageRecordCreationResult == -1) {
                    MessageBox.Show("Could not create the balance storage record for the user's default saving account! Please contact the application administrator for fixing this issue.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //Regex for checking if the textbox contains one/more empty spaces        
            Regex regex = new Regex("^\\s+");
            bool hasValidData = true;

            foreach (TextBox currentTextBox in textBoxes) {
                //Checks if the current field contains data or if it contains only empty spaces
                if (currentTextBox.Text.Length == 0 || regex.IsMatch(currentTextBox.Text)) {
                    hasValidData = false;
                    break;
                }
            }

            //Enables/disables the button received as argument based on the textbox content validation
            if (hasValidData) {
                targetButton.Enabled = true;
            } else {
                targetButton.Enabled = false;
            }
        }

        private bool isValidUserName(String userName) {
            //Regex for checking if the username contains only lowercase letters, uppercase letters, digits and underscores
            Regex validationRegex = new Regex("^[a-zA-Z0-9_]{3,}$");

            if (validationRegex.IsMatch(userName)) {
                return true;
            }

            return false;
        }

        private bool isValidPassword(String password) {
            //Checks if the password contains uppercase letters, lowercase letters and digits
            //Regex firstRegexPattern = new Regex("^(?=.*[a - z])(?=.*[A - Z])(?=.*[\\d]).+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //Checks if the password contains special characters
            //Regex secondRegexPattern = new Regex(".*[!@#\\$%^&*()_\\+\\-\\=\\[\\[{};'\\:\"\\|,.\\/<>\\?`~]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);


            //MatchCollection match1 = firstRegexPattern.Matches(password);
            //MatchCollection match2 = secondRegexPattern.Matches(password);


            //if (firstRegexPattern.IsMatch(password) && secondRegexPattern.IsMatch(password)) {
            //    return true;
            //}

            Regex nonWhitespaceCheckingRegex = new Regex("^[\\S]+$", RegexOptions.Compiled);
            Regex lowercaseCheckRegex = new Regex("[a-z]+", RegexOptions.Compiled);
            Regex uppercaseCheckRegex = new Regex("[A-Z]+", RegexOptions.Compiled);
            Regex digitsCheckRegex = new Regex("[0-9]+", RegexOptions.Compiled);
            Regex specialCharacterRegex = new Regex("[^\\w]+");


            bool hasPassedGeneralChecks = lowercaseCheckRegex.IsMatch(password) && uppercaseCheckRegex.IsMatch(password) && digitsCheckRegex.IsMatch(password);
            bool hasPassedSpecialCheck = specialCharacterRegex.IsMatch(password);



            if (nonWhitespaceCheckingRegex.IsMatch(password)) {
                if (hasPassedGeneralChecks && hasPassedSpecialCheck) {
                    return true;
                }
            }

            return false;
        }

        private Boolean isValidEmail(String inputEmailAddress) {
            try {
                MailAddress address = new MailAddress(inputEmailAddress);

                //Compares the email address of the newly created object with the provided email address and returns the corresponding boolean value            
                return address.Address.Equals(inputEmailAddress);

              //Handles the exception generated when the email address is represented by an empty string            
            } catch (ArgumentException ex) {
                Console.WriteLine(ex.Message);
                return false;

            //Handles the exception generated when the email address has an invalid format
            } catch (FormatException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private int createDefaultSavingAccount(String sqlStatement, String defaultAccountName, String userName) {
            if(userName == null) {
                return -1;
            }

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

        //Method that checks if the specified user exists
        private bool userExists(DataTable inputDataTable) {
            //Checks if the DataTable object contains data
            if (inputDataTable != null && inputDataTable.Rows.Count > 0) {
                return true;
            }

            return false;
        }

        //Method which retrieves the specified user from the database
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
            loginForm.Show();
        }
    }    
}
