using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ECNTools
{
    static class Program
    {
        public static bool login = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ECNTools.Login frmL = new ECNTools.Login();
            
            //Login frmL = new Login();
            frmL.ShowDialog();
            if (login == true)
                Application.Run(new Main2());
            else
                frmL.ShowDialog();
        }
    }
}
