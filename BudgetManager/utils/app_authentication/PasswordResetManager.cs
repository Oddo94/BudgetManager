using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    class PasswordResetManager {
        public static readonly int CONFIRMATION_CODE_SIZE = 32;
        private static String confirmationCode { get; set; }
        private String sqlStatementUpdatePassword = @"UPDATE users SET users.salt = @paramSalt, users.password = @paramHashCode WHERE users.userID = @paramID";
        private String sqlStatementRetrieveUserEmail = @"SELECT email FROM users WHERE username = @paramUserName";
        private PasswordSecurityManager securityManager;
        
        public PasswordResetManager() {
            this.securityManager = new PasswordSecurityManager();
        }

        public String ConfirmationCode {
            get {
                return confirmationCode;
            }

            set {
                confirmationCode = value;
            }
        }


        public String generateResetConfirmationCode(int size) {
            if (size <= 0) {
                return null;
            }

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[size];
            crypto.GetNonZeroBytes(randomBytes);

            String confirmationCode = convertBinaryToHex(randomBytes);


            return confirmationCode;

        }

        //Metoda de trimitere a email-ului continand codul de confirmare
        public void sendResetPasswordEmail(String emailAddress) {
            string messageTemplate = "A password reset was requested for the account associated to this email address.\nPlease enter the following code to finish the password reset process: {0} \nIf you have not requested the password reset please ignore this email and delete it.";

            //Se atribuie variabilei valoarea codului de confirmare generat pt a fi stocat in vederea comparatiei ulterioare cu codul introdus de utilizator
            confirmationCode = generateResetConfirmationCode(CONFIRMATION_CODE_SIZE);


            try {
                //Creare email si setarea adresei expeditorului(aplicatia se conecteaza la o adresa de email) 
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("budget.manager.agent@gmail.com");
                mail.To.Add(emailAddress);
                mail.Subject = "Password reset";
                mail.Body = String.Format(messageTemplate, confirmationCode);//Inserts the code String inside the message sent to the user

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("budget.manager.agent", "As6(Fo9#Yb");//Furnizare date de conectare pt adresa de email la care se conecteaza aplicatia
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("An email containing the reset password procedure has been sent to your email address", "Password reset manager");
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }




        //Creaza un sir de bytes nou si un hashcode pt parola nou introdusa folosind functiile puse la dispozitie de clasa PasswordSecurityManager
        public int resetPassword(String newPassword, int userID) {
            //Creare salt nou
            byte[] newSalt = securityManager.getSalt(16);
            //Generare hashcode pe baza salt
            String hashedPassword = securityManager.createPasswordHash(newPassword, newSalt);

           int executionResult = updatePassword(newSalt, hashedPassword, userID);

            return executionResult;

        }

        //Creaza comanda SQL de actualizarea a datelor in baza de date utilizand ca parametri un sir de bytes, hashcode-ul parolei si id-ul usrrului care cere resetarea parolei
        private int updatePassword(byte[] inputSalt, String inputHash, int userID) {                 
            MySqlCommand updatePasswordCommand = SQLCommandBuilder.getUpdatePasswordCommand(sqlStatementUpdatePassword, inputSalt, inputHash, userID);

            //Trimite comanda pt a fi executata de metoda specializata a clasei DBConnectionManager
            int executionResult = DBConnectionManager.insertData(updatePasswordCommand);

            return executionResult;
        }

        //Aduce adresa de email a utilizatorului care cere resetarea parolei, din baza de date
        private String getUserEmail(String userName) {
            MySqlCommand retrieveUserEmailCommand = new MySqlCommand(sqlStatementRetrieveUserEmail);
            retrieveUserEmailCommand.Parameters.AddWithValue("@paramUserName", userName);

            DataTable userEmailTable = DBConnectionManager.getData(retrieveUserEmailCommand);

            String userEmail = "";
            if (userEmailTable != null && userEmailTable.Rows.Count == 1) {
                userEmail = userEmailTable.Rows[0].ItemArray[0].ToString();
            }

            return userEmail;

        }

        private String convertBinaryToHex(byte[] inputArray) {

            StringBuilder resultArray = new StringBuilder(inputArray.Length * 2);

            foreach (byte currentByte in inputArray) {
                resultArray.AppendFormat("{0:x2}", currentByte);
            }

            return resultArray.ToString();
        }

        //Verifica daca codul de confirmare generat se potriveste cu cel introdus de utilizator
        public bool confirmationCodesMatch(String generatedConfirmationCode, String userInputConfirmationCode) {

            return generatedConfirmationCode.Equals(userInputConfirmationCode);
        }
    }
}
