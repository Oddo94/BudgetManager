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

        private String sqlStatementGetAuthenticationData = @"SELECT userID, username, salt, password FROM users WHERE username = @paramUserName";
        private String sqlStatementGetDataForPasswordReset = @"SELECT userID, username, email FROM users WHERE username = @paramUserName";
        private int minimumPasswordLength;

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
                MessageBox.Show(this, "No database connection! Unable to login.", "Login");
                return;
            }
            String userName = userNameTextBox.Text;
            String password = passwordTextBox.Text;

           
            MySqlCommand authenticationDataCommand = new MySqlCommand(sqlStatementGetAuthenticationData);
            authenticationDataCommand.Parameters.AddWithValue("@paramUserName", userName);

            //Aducere informatii din baza de date
            DataTable authenticationData = DBConnectionManager.getData(authenticationDataCommand);

            //Verificarea existentei utilizatorului si a corectitudinii datelor introduse
            if (userExists(authenticationData) && hasValidCredentials(authenticationData, password)) {
                //Se extrage id-ul de utilizator
                int userID = getUserID(authenticationData);
                this.Visible = false;

                //Se trimite id-ul ca argument constructorului clasei UserDashboard pt a putea fi utilizat ulterior la interogarea bazei de date
                UserDashboard userDashboard = new UserDashboard(userID, userName);

                userDashboard.Visible = true;
            } else {
                MessageBox.Show("Invalid username and/or password! Please try again", "Login");
            }     
        }

        private void registerButton_Click(object sender, EventArgs e) {
            if (!DBConnectionManager.hasConnection()) {
                MessageBox.Show(this, "No database connection! Unable to create new user/s.", "Register");
                return;
            }

            //Inchide formularul de autentificare
            this.Visible = false;
            //Afiseaza formularul de creare al unui nou utilizator
            new RegisterForm().Visible = true;
        }

        private void resetPasswordButton_Click(object sender, EventArgs e) {         
            if (!DBConnectionManager.hasConnection()) {
                MessageBox.Show(this, "No database connection! Unable to reset your password.", "Password reset manager");
                return;
            }
            //Cere confirmarea utilizatorului pentru resetarea parolei si inregistreaza rezultatul 
            DialogResult userOption = MessageBox.Show(this, "Are you sure that you want to reset your password?", "Password reset manager", MessageBoxButtons.YesNo);

            //Daca se selecteaza optiunea "No" se iese din metoda si se opreste procesul de resetare
            if (userOption == DialogResult.No) {
                //Console.WriteLine("User selected NO option");
                return;
            }

            //Adunare date necesare resetarii parolei
            String userName = userNameTextBox.Text;
            String newPassword = newPasswordTextBox.Text;
            String confirmationPassword = confirmPasswordTextBox.Text;

            //Verifica daca noua parola si parola de confirmare sunt identice
            if (!newPassword.Equals(confirmationPassword)) {
                MessageBox.Show("The input passwords don't match! Please try again!", "Password reset manager");
                return;
            }

            //Verifica daca parola respecta regulile de complexitate
            if (newPassword.Length < minimumPasswordLength) {
                MessageBox.Show("Your password should be at least 10 characters long! Please try again.", "Password reset manager");
                return;
            }

            if (!isValidPassword(newPassword)) {
                MessageBox.Show("Invalid password! Your password must contain:\n1.Lowercase and uppercase letters (a-zA-z) \n2.Digits (0-9) \n3.Special characters (@#$%<>?)", "Password reset manager");
                return;
            }


            MySqlCommand retrieveResetPasswordDataCommand = new MySqlCommand(sqlStatementGetDataForPasswordReset);
            retrieveResetPasswordDataCommand.Parameters.AddWithValue("@paramUserName", userName);

            DataTable resetPasswordDataTable = DBConnectionManager.getData(retrieveResetPasswordDataCommand);

            //Se verifica daca exista utilizatorul
            if (userExists(resetPasswordDataTable)) {
                Object emailData = resetPasswordDataTable.Rows[0].ItemArray[2];
                String userEmail = emailData != DBNull.Value ? emailData.ToString() : "";

                //Se verifica daca utilizatorul are setata o adresa de email 
                if ("".Equals(userEmail)) {
                    MessageBox.Show("Unable to retrieve the email address for the selected user!", "Password reset manager");
                    return;
                }

                //Se creeaza obiectele necesare pt procesul de resetare doar daca sunt indeplinite toate conditiile anterioare
                //Obiectul pt resetarea efectiva a parolei
                PasswordResetManager passwordResetManager = new PasswordResetManager();

                //Obiectul pt trimiterea codului de confirmare
                ConfirmationSender confirmationSender = new ConfirmationSender();
              
                //Date necesare pt trimiterea emailului
                string emailSubject = "Password reset";
                string emailBody = "A password reset was requested for the account associated to this email address.\nPlease enter the following code to finish the password reset process: {0} \nIf you have not requested the password reset please ignore this email and delete it.";
                string onSuccessMessage = "An email containing the reset password procedure has been sent to your email address";
                string parentWindowName = "Password reset manager";

                string generatedConfirmationCode = confirmationSender.generateConfirmationCode();
                confirmationSender.sendConfirmationEmail(userEmail, emailSubject, emailBody, generatedConfirmationCode, onSuccessMessage, parentWindowName);

                String userInputConfirmationCode = Interaction.InputBox("Enter the code received on your email to finish the reset process:", "Confirmation Code", "Enter code", 200, 200);

                if (confirmationSender.confirmationCodesMatch(generatedConfirmationCode, userInputConfirmationCode)) {
                    int userID = Convert.ToInt32(resetPasswordDataTable.Rows[0].ItemArray[0]);
                    int executionResult = passwordResetManager.resetPassword(newPassword, userID);//Daca din diverse motive nu se pot insera in baza de date noile informatii(salt si hashcode) metoda returneaza -1

                    if (executionResult == -1) {
                        MessageBox.Show("Could not reset your password!", "Password reset manager");
                        return;
                    }

                    MessageBox.Show("Your password has been succesfully reset!", "Password reset manager");
                } else {
                    //Se afiseaza mesajul doar daca codul introdus nu se potriveste cu cel generat nu si in situatia in care
                    //utilizatorul selecteaza optiunea Cancel sau inchide fereastra
                    if (!"".Equals(userInputConfirmationCode)) {
                        MessageBox.Show("Invalid confirmation code! Please try again.", "Password reset manager");
                    }
                   
                }

            } else {
            
                MessageBox.Show("Invalid username!", "Password reset manager");
                return;
               
            }

            TextBox[] textBoxes = new TextBox[] { newPasswordTextBox, confirmPasswordTextBox };
            clearInputFields(textBoxes);
            resetPasswordButton.Enabled = false;
                
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


        private void userNameTextBox_TextChanged(object sender, EventArgs e) {
            TextBox[] textBoxes = new TextBox[] { };

            //Verifica daca s-a selectat optiunea de resetare a parolei 
            if (resetPasswordCheckBox.Checked == true) {
                //Se verifica trei campuri pt activarea/dezactivarea butonului de resetare
                textBoxes = new TextBox[] { userNameTextBox, newPasswordTextBox, confirmPasswordTextBox };
                toggleButtonState(resetPasswordButton, textBoxes);
            } else {
                //Se verifica doua campuri pt activarea/dezactivarea butonului de autentificare
                textBoxes = new TextBox[] { userNameTextBox, passwordTextBox };
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

        private bool userExists(DataTable inputDataTable) {
            //Se verifica daca obiectul de tip DataTable furnizat contine date
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                return true;
            }

            return false;
        }

        private bool hasValidCredentials(DataTable inputDataTable, String userInputPassword) {
            //Verificare existenta date in DataTable
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                //Se extrage salt si hashcode-ul parolei
                byte[] salt = (byte[]) inputDataTable.Rows[0].ItemArray[2];
                String storedHash = inputDataTable.Rows[0].ItemArray[3].ToString();

                PasswordSecurityManager securityManager = new PasswordSecurityManager();

                //Se genereaza hashcode-ul parolei introduse de utilizator la autentificare
                String actualHash = securityManager.createPasswordHash(userInputPassword, salt);

                //Se compara daca cele doua hashcode-uri sunt identice
                return storedHash.Equals(actualHash);
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
