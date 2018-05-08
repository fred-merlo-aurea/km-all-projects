namespace ECNTools.SMTPLog
{
    partial class Port25
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
            this.label1 = new System.Windows.Forms.Label();
            this.rbBlastID = new System.Windows.Forms.RadioButton();
            this.rbGroup = new System.Windows.Forms.RadioButton();
            this.pnlGroupSelect = new System.Windows.Forms.Panel();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGroupID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBlastID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFolderLocation = new System.Windows.Forms.TextBox();
            this.btnFileLocation = new System.Windows.Forms.Button();
            this.cbxDigitalSplit = new System.Windows.Forms.CheckBox();
            this.txtDigitalSplit = new System.Windows.Forms.TextBox();
            this.btnDigitalSplit = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.btnCreateLogFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlGroupSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Export Option:";
            // 
            // rbBlastID
            // 
            this.rbBlastID.AutoSize = true;
            this.rbBlastID.Location = new System.Drawing.Point(168, 40);
            this.rbBlastID.Name = "rbBlastID";
            this.rbBlastID.Size = new System.Drawing.Size(67, 17);
            this.rbBlastID.TabIndex = 1;
            this.rbBlastID.TabStop = true;
            this.rbBlastID.Text = "Blast IDs";
            this.rbBlastID.UseVisualStyleBackColor = true;
            this.rbBlastID.CheckedChanged += new System.EventHandler(this.rbBlastID_CheckedChanged);
            // 
            // rbGroup
            // 
            this.rbGroup.AutoSize = true;
            this.rbGroup.Location = new System.Drawing.Point(256, 40);
            this.rbGroup.Name = "rbGroup";
            this.rbGroup.Size = new System.Drawing.Size(68, 17);
            this.rbGroup.TabIndex = 2;
            this.rbGroup.TabStop = true;
            this.rbGroup.Text = "Group ID";
            this.rbGroup.UseVisualStyleBackColor = true;
            this.rbGroup.CheckedChanged += new System.EventHandler(this.rbGroup_CheckedChanged);
            // 
            // pnlGroupSelect
            // 
            this.pnlGroupSelect.Controls.Add(this.dtTo);
            this.pnlGroupSelect.Controls.Add(this.label4);
            this.pnlGroupSelect.Controls.Add(this.dtFrom);
            this.pnlGroupSelect.Controls.Add(this.label3);
            this.pnlGroupSelect.Controls.Add(this.txtGroupID);
            this.pnlGroupSelect.Controls.Add(this.label2);
            this.pnlGroupSelect.Enabled = false;
            this.pnlGroupSelect.Location = new System.Drawing.Point(96, 72);
            this.pnlGroupSelect.Name = "pnlGroupSelect";
            this.pnlGroupSelect.Size = new System.Drawing.Size(568, 80);
            this.pnlGroupSelect.TabIndex = 3;
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(336, 48);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 20);
            this.dtTo.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "To:";
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(80, 48);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 20);
            this.dtFrom.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "From:";
            // 
            // txtGroupID
            // 
            this.txtGroupID.Location = new System.Drawing.Point(80, 16);
            this.txtGroupID.Name = "txtGroupID";
            this.txtGroupID.Size = new System.Drawing.Size(456, 20);
            this.txtGroupID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Group ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "BlastIDs:";
            // 
            // txtBlastID
            // 
            this.txtBlastID.Location = new System.Drawing.Point(176, 168);
            this.txtBlastID.Name = "txtBlastID";
            this.txtBlastID.Size = new System.Drawing.Size(456, 20);
            this.txtBlastID.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Save File To:";
            // 
            // txtFolderLocation
            // 
            this.txtFolderLocation.BackColor = System.Drawing.SystemColors.Control;
            this.txtFolderLocation.Location = new System.Drawing.Point(176, 208);
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.ReadOnly = true;
            this.txtFolderLocation.Size = new System.Drawing.Size(456, 20);
            this.txtFolderLocation.TabIndex = 7;
            // 
            // btnFileLocation
            // 
            this.btnFileLocation.Location = new System.Drawing.Point(648, 208);
            this.btnFileLocation.Name = "btnFileLocation";
            this.btnFileLocation.Size = new System.Drawing.Size(80, 23);
            this.btnFileLocation.TabIndex = 8;
            this.btnFileLocation.Text = "Select Folder";
            this.btnFileLocation.UseVisualStyleBackColor = true;
            this.btnFileLocation.Click += new System.EventHandler(this.btnFileLocation_Click);
            // 
            // cbxDigitalSplit
            // 
            this.cbxDigitalSplit.AutoSize = true;
            this.cbxDigitalSplit.Location = new System.Drawing.Point(40, 248);
            this.cbxDigitalSplit.Name = "cbxDigitalSplit";
            this.cbxDigitalSplit.Size = new System.Drawing.Size(125, 17);
            this.cbxDigitalSplit.TabIndex = 9;
            this.cbxDigitalSplit.Text = "Import Digital Split to:";
            this.cbxDigitalSplit.UseVisualStyleBackColor = true;
            this.cbxDigitalSplit.CheckedChanged += new System.EventHandler(this.cbxDigitalSplit_CheckedChanged);
            // 
            // txtDigitalSplit
            // 
            this.txtDigitalSplit.BackColor = System.Drawing.SystemColors.Control;
            this.txtDigitalSplit.Location = new System.Drawing.Point(176, 248);
            this.txtDigitalSplit.Name = "txtDigitalSplit";
            this.txtDigitalSplit.ReadOnly = true;
            this.txtDigitalSplit.Size = new System.Drawing.Size(456, 20);
            this.txtDigitalSplit.TabIndex = 10;
            // 
            // btnDigitalSplit
            // 
            this.btnDigitalSplit.Location = new System.Drawing.Point(648, 248);
            this.btnDigitalSplit.Name = "btnDigitalSplit";
            this.btnDigitalSplit.Size = new System.Drawing.Size(80, 23);
            this.btnDigitalSplit.TabIndex = 11;
            this.btnDigitalSplit.Text = "Select File";
            this.btnDigitalSplit.UseVisualStyleBackColor = true;
            this.btnDigitalSplit.Click += new System.EventHandler(this.btnDigitalSplit_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.Location = new System.Drawing.Point(40, 288);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(688, 173);
            this.lstMessage.TabIndex = 12;
            // 
            // btnCreateLogFile
            // 
            this.btnCreateLogFile.Location = new System.Drawing.Point(128, 496);
            this.btnCreateLogFile.Name = "btnCreateLogFile";
            this.btnCreateLogFile.Size = new System.Drawing.Size(128, 40);
            this.btnCreateLogFile.TabIndex = 13;
            this.btnCreateLogFile.Text = "Create Logfile";
            this.btnCreateLogFile.UseVisualStyleBackColor = true;
            this.btnCreateLogFile.Click += new System.EventHandler(this.btnCreateLogFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(280, 496);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 40);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(440, 496);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(144, 40);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Port25
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 593);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateLogFile);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnDigitalSplit);
            this.Controls.Add(this.txtDigitalSplit);
            this.Controls.Add(this.cbxDigitalSplit);
            this.Controls.Add(this.btnFileLocation);
            this.Controls.Add(this.txtFolderLocation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtBlastID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlGroupSelect);
            this.Controls.Add(this.rbGroup);
            this.Controls.Add(this.rbBlastID);
            this.Controls.Add(this.label1);
            this.Name = "Port25";
            this.Text = "Port25";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Port25_FormClosing);
            this.pnlGroupSelect.ResumeLayout(false);
            this.pnlGroupSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbBlastID;
        private System.Windows.Forms.RadioButton rbGroup;
        private System.Windows.Forms.Panel pnlGroupSelect;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGroupID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBlastID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFolderLocation;
        private System.Windows.Forms.Button btnFileLocation;
        private System.Windows.Forms.CheckBox cbxDigitalSplit;
        private System.Windows.Forms.TextBox txtDigitalSplit;
        private System.Windows.Forms.Button btnDigitalSplit;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.Button btnCreateLogFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
    }
}