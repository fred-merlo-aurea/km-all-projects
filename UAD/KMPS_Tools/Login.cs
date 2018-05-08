using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KMPS_Tools
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
                Program.login = true;
                this.Close();
            }
            else
                lbMessage.Text = "Invalid User Name or Password.";
        }
    }
}
