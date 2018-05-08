using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KM.Common.Tools;

namespace KMPS_Tools
{
    public partial class Main : Form, IMainToolWindow
    {
        public static bool isDEMO;

        public Main()
        {
            InitializeComponent();

        }

        public void ToggleMenus(bool show)
        {
            menuStrip1.Enabled = show;
        }

        private void menuWQTImport_Click(object sender, EventArgs e)
        {
            ToggleMenus(false);
            WQTImport frm = new WQTImport();
            frm.MdiParent = this;
            frm.Location = new Point(0, 0);
            frm.Show();
        }

        
    }
}
