using Microsoft.VisualBasic;
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
    public partial class LoginForm : Form {
        public static int userID;
        public static String userName;
        public static bool isOpeningTheUserRegistrationForm;

        private String sqlStatementGetAuthenticationData = @"SELECT userID, username, salt, password FROM users WHERE username = @paramUserName";
        private String sqlStatementGetDataForPasswordReset = @"SELECT userID, username, email FROM users WHERE username = @paramUserName";
        private int minimumPasswordLength;
        private bool isSuccessfullyAuthenticated;


        public LoginForm() {
            InitializeComponent();
            minimumPasswordLength = 10;
        }

        private void resetPasswordCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (resetPasswordCheckBox.Checked == true) {
                resetPasswordPanel.Visible = true;
                loginButton.Enabled = false;
            } else {
                resetPasswordPanel.Visible = false;               
            }

        }

        private void loginButton_Click(object sender, EventArgs e) {
            if (!DBConnectionManager.hasConnection()) {             
                MessageBox.Show(this, "No database connection! Unable to login.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            String inputUserName = userNameTextBox.Text;
            String inputPassword = passwordTextBox.Text;

           
            MySqlCommand authenticationDataCommand = new MySqlCommand(sqlStatementGetAuthenticationData);
            authenticationDataCommand.Parameters.AddWithValue("@paramUserName", inputUserName);

            //Retrieves authentication data from the database
            DataTable authenticationData = DBConnectionManager.getData(authenticationDataCommand);

            //Checks that the user exists and his credentials are correct
            if (userExists(authenticationData) && hasValidCredentials(authenticationData, inputPassword)) {              
                //Extracts the user ID
                userID = getUserID(authenticationData);
                userName = inputUserName;
                //this.Visible = false;
                this.Hide();

                ////Sends the user ID to the UserDashboard class constructor in order to use it later for extracting data from the database
                //UserDashboard userDashboard = new UserDashboard(userID, userName);
                //userDashboard.Visible = true;

                DialogResult = DialogResult.OK;                
                isSuccessfullyAuthenticated = true;

                //Sends the current form instance as an argument to the UserDashboard class so that it can later be used to show the login form on n 
                new UserDashboard(userID, userName).Visible = true;

            } else {             
                MessageBox.Show("Invalid username and/or password! Please try again", "Login",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void registerButton_Click(object sender, EventArgs e) {
            if (!DBConnectionManager.hasConnection()) {
                MessageBox.Show(this, "No database connection! Unable to create new user/s.", "Register");
                return;
            }

            //Hides the login form        
            //this.Visible = false;
            this.Hide();
            isOpeningTheUserRegistrationForm = true;

            ///Shows the register form and sends the current instance as an argument to the constructor (so the reference can later be used to show the login form when closing the register form)
            new RegisterForm(this).Visible = true;
        }

        private void resetPasswordButton_Click(object sender, EventArgs e) {         
            if (!DBConnectionManager.hasConnection()) {          
                MessageBox.Show(this, "No database connection! Unable to reset your password.", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          
            //Asks the user to confirm the password reset
            DialogResult userOption = MessageBox.Show(this, "Are you sure that you want to reset your password?", "Password reset manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //If the user selects the "No" option the password reset process is cancelled by returning from the method
            if (userOption == DialogResult.No) {            
                return;
            }

            
            //Retrieves the necessary data for password reset
            String userName = userNameTextBox.Text;
            String newPassword = newPasswordTextBox.Text;
            String confirmationPassword = confirmPasswordTextBox.Text;

            //Checks if the new password and the confirmation password are identical
            if (!newPassword.Equals(confirmationPassword)) {              
                MessageBox.Show("The input passwords don't match! Please try again!", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            //Checks if the password meets the complexity standards
            if (newPassword.Length < minimumPasswordLength) {               
                MessageBox.Show("Your password should be at least 10 characters long! Please try again.", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isValidPassword(newPassword)) {           
                MessageBox.Show("Invalid password! Your password must contain:\n1.Lowercase and uppercase letters (a-zA-z) \n2.Digits (0-9) \n3.Special characters (@#$%<>?)", "Password reset manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            MySqlCommand retrieveResetPasswordDataCommand = new MySqlCommand(sqlStatementGetDataForPasswordReset);
            retrieveResetPasswordDataCommand.Parameters.AddWithValue("@paramUserName", userName);

            DataTable resetPasswordDataTable = DBConnectionManager.getData(retrieveResetPasswordDataCommand);
          
            //Checks if the user exists
            if (userExists(resetPasswordDataTable)) {
                Object emailData = resetPasswordDataTable.Rows[0].ItemArray[2];
                String userEmail = emailData != DBNull.Value ? emailData.ToString() : "";

                //Checks if the user has an email address associated to his account
                if ("".Equals(userEmail)) {
                    //MessageBox.Show("Unable to retrieve the email address for the selected user!", "Password reset manager");
                    MessageBox.Show("Unable to retrieve the email address for the selected user!", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //The necessary objects for the password reset process are created below

                //The object responsible for the actual password reset
                PasswordResetManager passwordResetManager = new PasswordResetManager();
          
                //The object responsible for sending the confirmation code 
                ConfirmationSender confirmationSender = new ConfirmationSender();
                          
                //The necessary data for sending the confirmation email
                string emailSubject = "BudgetManager-password reset";
                string emailBody = "A password reset was requested for the BudgetManager application account associated to this email address.\nPlease enter the following code to finish the password reset process: {0} \nIf you have not requested the password reset please ignore this email and delete it immediately.";
                string onSuccessMessage = "An email containing the reset password procedure has been sent to your email address";
                string parentWindowName = "Password reset manager";

                string generatedConfirmationCode = confirmationSender.generateConfirmationCode();
                confirmationSender.sendConfirmationEmail(userEmail, emailSubject, emailBody, generatedConfirmationCode, onSuccessMessage, parentWindowName);

                String userInputConfirmationCode = Interaction.InputBox("Enter the code received on your email to finish the reset process:", "Confirmation Code", "Enter code", 200, 200);

                if (confirmationSender.confirmationCodesMatch(generatedConfirmationCode, userInputConfirmationCode)) {
                    int userID = Convert.ToInt32(resetPasswordDataTable.Rows[0].ItemArray[0]);
                    int executionResult = passwordResetManager.resetPassword(newPassword, userID);//If the data (salt and hashcode) cannot be inserted into the database the method will return -1

                    if (executionResult == -1) {                
                        MessageBox.Show("Could not reset your password!", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
              
                    MessageBox.Show("Your password has been succesfully reset!", "Password reset manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {                  
                    //Shows the message only if the input code does not match the generated code but not when the user selects the Cancel option or closes the dialog
                    if (!"".Equals(userInputConfirmationCode)) {                    
                        MessageBox.Show("Invalid confirmation code! Please try again.", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                   
                }

            } else {
                MessageBox.Show("Invalid username!", "Password reset manager",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
               
            }

            TextBox[] textBoxes = new TextBox[] { newPasswordTextBox, confirmPasswordTextBox };
            clearInputFields(textBoxes);
            resetPasswordButton.Enabled = false;
                
        }

        private void toggleButtonState(Button targetButton, TextBox[] textBoxes) {
            //Regex for checking the existence of a single/multiple empty spaces       
            Regex regex = new Regex("^\\s+");
            bool hasValidData = true;

            foreach (TextBox currentTextBox in textBoxes) {
                //Checks if data is present in the current field or if it contains one or more empty spaces
                if (currentTextBox.Text.Length == 0 || regex.IsMatch(currentTextBox.Text)) {
                    hasValidData = false;
                    break;
                }
            }

            //The button provided as argument is enabled/disabled based on the existence/inexistence of valid data
            if (hasValidData) {
                targetButton.Enabled = true;
            } else {
                targetButton.Enabled = false;
            }
        }


        private void userNameTextBox_TextChanged(object sender, EventArgs e) {
            TextBox[] textBoxes = new TextBox[] { };

            //Checks if the password reset checkbox was selected 
            if (resetPasswordCheckBox.Checked == true) {
                //Checks three fields in order to decide if the reset password button needs to be activated or not
                textBoxes = new TextBox[] { userNameTextBox, newPasswordTextBox, confirmPasswordTextBox };
                toggleButtonState(resetPasswordButton, textBoxes);
            } else {
                //Otherwise it checks only two fields to decide if the login button needs to be activated or not
                textBoxes = new TextBox[] { userNameTextBox, passwordTextBox};
                toggleButtonState(loginButton, textBoxes);
            }
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e) {
            TextBox[] textBoxes = new TextBox[] { userNameTextBox, passwordTextBox };
            toggleButtonState(loginButton, textBoxes);
        }

        private void newPasswordTextBox_TextChanged(object sender, EventArgs e) {
            TextBox[] textBoxes = new TextBox[] { userNameTextBox, newPasswordTextBox, confirmPasswordTextBox};
            toggleButtonState(resetPasswordButton, textBoxes);
        }

        private void confirmPasswordTextBox_TextChanged(object sender, EventArgs e) {
            TextBox[] textBoxes = new TextBox[] { userNameTextBox, newPasswordTextBox, confirmPasswordTextBox};
            toggleButtonState(resetPasswordButton, textBoxes);
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (!isSuccessfullyAuthenticated && !isOpeningTheUserRegistrationForm) {
                DialogResult userOption = MessageBox.Show("Are you sure that you want to exit?", "Login form", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
               
                if (userOption == DialogResult.No) {
                    e.Cancel = true;
                } else {
                    Environment.Exit(0);
                }
            }
        }

        private bool userExists(DataTable inputDataTable) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                return true;
            }

            return false;
        }

        private bool hasValidCredentials(DataTable inputDataTable, String userInputPassword) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                //Extracts the stored salt and hashcode for the input password
                byte[] salt = (byte[]) inputDataTable.Rows[0].ItemArray[2];
                String storedHash = inputDataTable.Rows[0].ItemArray[3].ToString();

                PasswordSecurityManager securityManager = new PasswordSecurityManager();

                //Generates the hashcode for the input password using the stored salt 
                String actualHash = securityManager.createPasswordHash(userInputPassword, salt);

                //Checks if the two hashcodes are identical
                return storedHash.Equals(actualHash);
            }

            return false;
        }

        private bool isValidPassword(String password) {
            //Checks if the password contains lowercase, uppercase characters and digits
            Regex firstRegexPattern = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\d]).+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //Checks if the password contains special characters
            Regex secondRegexPattern = new Regex(".*[!@#\\$%^&*()_\\+\\-\\=\\[\\[{};'\\:\"\\|,.\\/<>\\?`~]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);


            MatchCollection match1 = firstRegexPattern.Matches(password);
            MatchCollection match2 = secondRegexPattern.Matches(password);


            if (firstRegexPattern.IsMatch(password) && secondRegexPattern.IsMatch(password)) {
                return true;
            }

            return false;
        }

        private int getUserID(DataTable inputDataTable) {
            if( inputDataTable != null && inputDataTable.Rows.Count == 1) {
                Object retrievedID = inputDataTable.Rows[0].ItemArray[0];
                int userID = retrievedID != DBNull.Value ? Convert.ToInt32(retrievedID) : -1;

                return userID;
            }

            return -2;
        }

        private void clearInputFields(TextBox[] textBoxes) {
            foreach (TextBox currentTextBox in textBoxes) {
                currentTextBox.Text = "";
            }
        }
    }
}
