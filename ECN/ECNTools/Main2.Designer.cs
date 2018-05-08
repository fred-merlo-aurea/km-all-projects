namespace ECNTools
{
    partial class Main2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuInterimDBExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBPALog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBPALogCleaner = new System.Windows.Forms.ToolStripMenuItem();
            this.eCNImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fPImportNitelyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stmpLogCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stmpPort25LogCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInterimDBExport,
            this.mnuBPALog,
            this.eCNImportToolStripMenuItem,
            this.stmpLogCreatorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1125, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuInterimDBExport
            // 
            this.mnuInterimDBExport.Name = "mnuInterimDBExport";
            this.mnuInterimDBExport.Size = new System.Drawing.Size(117, 20);
            this.mnuInterimDBExport.Text = "Interim DataExport";
            this.mnuInterimDBExport.Click += new System.EventHandler(this.interimDBExportToolStripMenuItem_Click);
            // 
            // mnuBPALog
            // 
            this.mnuBPALog.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBPALogCleaner});
            this.mnuBPALog.Name = "mnuBPALog";
            this.mnuBPALog.Size = new System.Drawing.Size(64, 20);
            this.mnuBPALog.Text = "BPA Log";
            // 
            // mnuBPALogCleaner
            // 
            this.mnuBPALogCleaner.Name = "mnuBPALogCleaner";
            this.mnuBPALogCleaner.Size = new System.Drawing.Size(162, 22);
            this.mnuBPALogCleaner.Text = "BPA Log Cleaner";
            this.mnuBPALogCleaner.Click += new System.EventHandler(this.mnuBPALogCleaner_Click);
            // 
            // eCNImportToolStripMenuItem
            // 
            this.eCNImportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataImportToolStripMenuItem,
            this.fPImportNitelyToolStripMenuItem});
            this.eCNImportToolStripMenuItem.Name = "eCNImportToolStripMenuItem";
            this.eCNImportToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.eCNImportToolStripMenuItem.Text = "ECN Import";
            // 
            // dataImportToolStripMenuItem
            // 
            this.dataImportToolStripMenuItem.Name = "dataImportToolStripMenuItem";
            this.dataImportToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.dataImportToolStripMenuItem.Text = "FP Import - Add";
            this.dataImportToolStripMenuItem.Click += new System.EventHandler(this.dataImportToolStripMenuItem_Click);
            // 
            // fPImportNitelyToolStripMenuItem
            // 
            this.fPImportNitelyToolStripMenuItem.Name = "fPImportNitelyToolStripMenuItem";
            this.fPImportNitelyToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.fPImportNitelyToolStripMenuItem.Text = "FP Import - Update";
            this.fPImportNitelyToolStripMenuItem.Click += new System.EventHandler(this.fPImportNitelyToolStripMenuItem_Click);
            // 
            // stmpLogCreatorToolStripMenuItem
            // 
            this.stmpLogCreatorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stmpPort25LogCreatorToolStripMenuItem});
            this.stmpLogCreatorToolStripMenuItem.Name = "stmpLogCreatorToolStripMenuItem";
            this.stmpLogCreatorToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.stmpLogCreatorToolStripMenuItem.Text = "SMTP Log Creator";
            // 
            // stmpPort25LogCreatorToolStripMenuItem
            // 
            this.stmpPort25LogCreatorToolStripMenuItem.Name = "stmpPort25LogCreatorToolStripMenuItem";
            this.stmpPort25LogCreatorToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.stmpPort25LogCreatorToolStripMenuItem.Text = "Port 25";
            this.stmpPort25LogCreatorToolStripMenuItem.Click += new System.EventHandler(this.stmpPort25LogCreatorToolStripMenuItem_Click);
            // 
            // Main2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 731);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main2";
            this.Text = "Main2";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuInterimDBExport;
        private System.Windows.Forms.ToolStripMenuItem mnuBPALog;
        private System.Windows.Forms.ToolStripMenuItem mnuBPALogCleaner;
        private System.Windows.Forms.ToolStripMenuItem eCNImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fPImportNitelyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stmpLogCreatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stmpPort25LogCreatorToolStripMenuItem;
    }
}