using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using KM.Common.Tools;

namespace ECNTools
{
    public partial class Main2 : Form, IMainToolWindow
    {
        public static KMPlatform.Entity.User user;
        public Main2()
        {
            
            InitializeComponent();
        }
        public void ToggleMenus(bool show)
        {
            menuStrip1.Enabled = show;
        }


        private void interimDBExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleMenus(false);
            ECNTools.InterimDataExport.InterimDataExport2 frm = new ECNTools.InterimDataExport.InterimDataExport2();
            frm.MdiParent = this;
            frm.Location = new Point(0, 0);
            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.Show();
        }

        private void mnuBPALogCleaner_Click(object sender, EventArgs e)
        {

            ToggleMenus(false);
            ECNTools.BPALog.BPALogFix2 frm = new ECNTools.BPALog.BPALogFix2();
            frm.MdiParent = this;
            frm.Location = new Point(0, 0);
            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.Show();
        }

        private void dataImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleMenus(false);
            ECNTools.ECN_Import.FPImport2 frm = new ECNTools.ECN_Import.FPImport2();
            frm.MdiParent = this;
            frm.Location = new Point(0, 0);
            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.Show();
        }

        private void fPImportNitelyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleMenus(false);
            ECNTools.ECN_Import.FPImportNightly frm = new ECNTools.ECN_Import.FPImportNightly();
            frm.MdiParent = this;
            frm.Location = new Point(0, 0);
            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.Show();
        }

        private void stmpPort25LogCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleMenus(false);
            ECNTools.SMTPLog.Port25 frm = new ECNTools.SMTPLog.Port25();
            frm.MdiParent = this;
            frm.Location = new Point(0, 0);
            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.Show();
        }
    }
}
