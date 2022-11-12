using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //LoginForm loginForm = new LoginForm();

            //if (loginForm.ShowDialog() == DialogResult.OK) {
            //    int userID = LoginForm.userID;
            //    String userName = LoginForm.userName;

            //    Application.Run(new UserDashboard(userID, userName));

            //    //!LoginForm.isOpeningTheUserRegistrationForm
            //} else {
            //    //MessageBox.Show("Invalid username and/or password! Please try again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    if(LoginForm.isOpeningTheUserRegistrationForm) {
            //        new RegisterForm().Show();
            //    }
            //    //Application.Run(new LoginForm());
            //}

            Application.Run(new LoginForm());
        }
    }
}
