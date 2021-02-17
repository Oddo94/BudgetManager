using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class PasswordSecurityManager {
        //String hashedPassword = "";
        //byte[] retrievedSalt = new byte[16];

        //public static void Main(String[] args) {
        //    PasswordSecurityManager manager = new PasswordSecurityManager();
        //    //manager.retrieveStoredData(manager.getData(6));
        //    byte[] salt = manager.getSalt(16);
        //    //byte[] salt = manager.retrievedSalt;
        //    Console.WriteLine("Salt: ");
        //    foreach (byte b in salt) {
        //        Console.Write(b + ",");
        //    }
        //    //Console.WriteLine(manager.convertBinaryToHex(salt));
        //    String password = "3PnEgRN2010RZV^";
        //    String result = manager.createPasswordHash(password, salt);

        //    //Console.WriteLine("Password: " + result);
        //    //String previousHash = manager.hashedPassword;

        //    Console.WriteLine("Original password: " + password);
        //    Console.WriteLine("Hashed password: " + result);

        //    manager.writeAuthenticationDataToDB(salt, result, 3);

        //    //Console.WriteLine("Previous hash matches current hash: " + result.Equals(previousHash));
        //}

        //Metoda de creare a hashcode-ului
        public String createPasswordHash(String password, byte[] salt) {
            //Furnizare parola, salt si numar de iteratii
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            byte[] hashBytes = pbkdf2.GetBytes(32);

            //Transformare hashcode din sir de octeti in sir de caractere
            String hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }



        //Metoda de generare a salt
        public byte[] getSalt(int size) {
            if (size < 16) {
                return null;
            }
            byte[] salt = new byte[size];
            //Generare salt in mod aleatoriu folosind o metoda sigura din punct de vedere criptografic
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            random.GetBytes(salt);

            return salt;
        }

        private String convertBinaryToHex(byte[] inputArray) {

            StringBuilder resultArray = new StringBuilder(inputArray.Length * 2);

            foreach(byte currentByte in inputArray) {
                resultArray.AppendFormat("{0:x2}", currentByte);
            }

            return resultArray.ToString();
        }

        private void writeAuthenticationDataToDB(byte[] salt, String hashedPassword, int userID) {
            String sqlStatementInsertAuthenticationData = @"UPDATE users SET users.salt = @paramSalt, users.password = @paramPassword WHERE users.userID = @paramID";
            MySqlCommand insertCommand = new MySqlCommand(sqlStatementInsertAuthenticationData);
            insertCommand.Parameters.AddWithValue("@paramSalt", salt);
            insertCommand.Parameters.AddWithValue("@paramPassword", hashedPassword);
            insertCommand.Parameters.AddWithValue("@paramID", userID);

            DBConnectionManager.insertData(insertCommand);

        }

        private DataTable getData(int userID) {
            String sqlStatementGetAuthenticationData = @"SELECT username, salt, password FROM users WHERE userID = @paramID";
            MySqlCommand command = new MySqlCommand(sqlStatementGetAuthenticationData);
            command.Parameters.AddWithValue("@paramID", userID);

            return DBConnectionManager.getData(command);

        }
    }
}
