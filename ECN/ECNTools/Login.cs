using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace ECNTools
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbUserName.Text.Equals("a") && tbPassword.Text.Equals("a"))
            {
                Main2.user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationSettings.AppSettings["ecnAccessKey"].ToString(), false);
                Program.login = true;
                this.Close();
            }
            else
                lbMessage.Text = "Invalid User Name or Password.";
        }
    }
}
