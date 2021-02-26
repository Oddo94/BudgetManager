using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    class ConfirmationSender {
        private string senderAddress;
        private string senderUserName;
        private string senderPassword;
        private int confirmationCodeSize { get; set; }
        private string confirmationCode { get; set; }
        

        public ConfirmationSender() {
            //this.senderAddress = "budget.manager.agent@gmail.com";
            //this.senderUserName = "budget.manager.agent";
            //this.senderPassword = "As6(Fo9#Yb";
            this.senderAddress = Properties.Settings.Default.SenderAddress;
            this.senderUserName = Properties.Settings.Default.SenderUserName;
            this.senderPassword = Properties.Settings.Default.SenderPassword;

            this.confirmationCodeSize = 32;
            confirmationCode = null;
        }

        public string ConfirmationCode {
            get {
                return this.confirmationCode;
            }

            set {
                this.confirmationCode = value;
            }
         }

        public int ConfirmationCodeSize {
            get {
                return this.confirmationCodeSize;
            }

            set {
                if (value >= 16 ) {
                    this.confirmationCodeSize = value;
                }
            }
        }

        //Metoda de trimitere a email-ului continand codul de confirmare
        public void sendConfirmationEmail(string emailAddress, string emailSubject, string emailBody, string confirmationCode, string onSuccessMessage, string parentWindowName) {           
            //Se atribuie variabilei valoarea codului de confirmare generat pt a fi stocat in vederea comparatiei ulterioare cu codul introdus de utilizator
            //confirmationCode = generateConfirmationCode();

            try {
                //Creare email si setarea adresei expeditorului(aplicatia se conecteaza la o adresa de email) 
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(senderAddress);
                mail.To.Add(emailAddress);
                mail.Subject = emailSubject;
                mail.Body = String.Format(emailBody, confirmationCode);//Insereaza codul de confirmare generat in mesajul trimis catre destinatar

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(senderUserName, senderPassword);//Furnizare date de conectare pt adresa de email la care se conecteaza aplicatia
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show(onSuccessMessage, parentWindowName);
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
           
        }

        public String generateConfirmationCode() {
            //if (size <= 0) {
            //    return null;
            //}

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[confirmationCodeSize];
            crypto.GetNonZeroBytes(randomBytes);

            String confirmationCode = convertBinaryToHex(randomBytes);


            return confirmationCode;

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
