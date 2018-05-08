using System;
using System.Windows.Forms;

namespace KM.Common.Tools
{
    public partial class BPALogFixBase : Form
    {
        public BPALogFixBase()
        {
            InitializeComponent();
        }
        
        protected void btnFileLocation_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                txtFolderLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ToggleMenus();
            Close();
        }

        protected void BPALogFix_FormClosing(object sender, FormClosingEventArgs e)
        {
            ToggleMenus();
        }

        protected void ToggleMenus()
        {
            var form = (IMainToolWindow)MdiParent;
            form.ToggleMenus(true);
        }
    }
}
