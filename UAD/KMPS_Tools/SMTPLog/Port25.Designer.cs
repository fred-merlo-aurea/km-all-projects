namespace KMPS_Tools
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnFileLocation = new System.Windows.Forms.Button();
            this.txtFolderLocation = new System.Windows.Forms.TextBox();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreateLogFile = new System.Windows.Forms.Button();
            this.lblBlastID = new System.Windows.Forms.Label();
            this.txtBlastID = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.txtDigitalSplit = new System.Windows.Forms.TextBox();
            this.cbxDigitalSplit = new System.Windows.Forms.CheckBox();
            this.btnDigitalSplit = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.rbBlastID = new System.Windows.Forms.RadioButton();
            this.rbGroup = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlGroupSelect = new System.Windows.Forms.Panel();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGroupID = new System.Windows.Forms.TextBox();
            this.pnlGroupSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(85, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Save File to: ";
            // 
            // btnFileLocation
            // 
            this.btnFileLocation.Location = new System.Drawing.Point(581, 162);
            this.btnFileLocation.Name = "btnFileLocation";
            this.btnFileLocation.Size = new System.Drawing.Size(83, 24);
            this.btnFileLocation.TabIndex = 25;
            this.btnFileLocation.Text = "Select Folder";
            this.btnFileLocation.UseVisualStyleBackColor = true;
            this.btnFileLocation.Click += new System.EventHandler(this.btnFileLocation_Click);
            // 
            // txtFolderLocation
            // 
            this.txtFolderLocation.Location = new System.Drawing.Point(154, 165);
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.ReadOnly = true;
            this.txtFolderLocation.Size = new System.Drawing.Size(409, 20);
            this.txtFolderLocation.TabIndex = 24;
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.Location = new System.Drawing.Point(24, 232);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(663, 199);
            this.lstMessage.TabIndex = 23;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(444, 464);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(151, 43);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(276, 464);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 43);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCreateLogFile
            // 
            this.btnCreateLogFile.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCreateLogFile.Location = new System.Drawing.Point(104, 464);
            this.btnCreateLogFile.Name = "btnCreateLogFile";
            this.btnCreateLogFile.Size = new System.Drawing.Size(151, 43);
            this.btnCreateLogFile.TabIndex = 20;
            this.btnCreateLogFile.Text = "Create Logfile";
            this.btnCreateLogFile.UseVisualStyleBackColor = true;
            this.btnCreateLogFile.Click += new System.EventHandler(this.btnCreateLogFile_Click);
            // 
            // lblBlastID
            // 
            this.lblBlastID.AutoSize = true;
            this.lblBlastID.Location = new System.Drawing.Point(99, 132);
            this.lblBlastID.Name = "lblBlastID";
            this.lblBlastID.Size = new System.Drawing.Size(52, 13);
            this.lblBlastID.TabIndex = 29;
            this.lblBlastID.Text = "BlastIDs: ";
            // 
            // txtBlastID
            // 
            this.txtBlastID.Location = new System.Drawing.Point(154, 129);
            this.txtBlastID.Name = "txtBlastID";
            this.txtBlastID.Size = new System.Drawing.Size(409, 20);
            this.txtBlastID.TabIndex = 30;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // txtDigitalSplit
            // 
            this.txtDigitalSplit.Location = new System.Drawing.Point(154, 200);
            this.txtDigitalSplit.Name = "txtDigitalSplit";
            this.txtDigitalSplit.ReadOnly = true;
            this.txtDigitalSplit.Size = new System.Drawing.Size(409, 20);
            this.txtDigitalSplit.TabIndex = 31;
            // 
            // cbxDigitalSplit
            // 
            this.cbxDigitalSplit.AutoSize = true;
            this.cbxDigitalSplit.Location = new System.Drawing.Point(28, 200);
            this.cbxDigitalSplit.Name = "cbxDigitalSplit";
            this.cbxDigitalSplit.Size = new System.Drawing.Size(128, 17);
            this.cbxDigitalSplit.TabIndex = 33;
            this.cbxDigitalSplit.Text = "Import Digital Split to: ";
            this.cbxDigitalSplit.UseVisualStyleBackColor = true;
            this.cbxDigitalSplit.CheckedChanged += new System.EventHandler(this.cbxDigitalSplit_CheckedChanged);
            // 
            // btnDigitalSplit
            // 
            this.btnDigitalSplit.Enabled = false;
            this.btnDigitalSplit.Location = new System.Drawing.Point(581, 200);
            this.btnDigitalSplit.Name = "btnDigitalSplit";
            this.btnDigitalSplit.Size = new System.Drawing.Size(83, 24);
            this.btnDigitalSplit.TabIndex = 34;
            this.btnDigitalSplit.Text = "Select File";
            this.btnDigitalSplit.UseVisualStyleBackColor = true;
            this.btnDigitalSplit.Click += new System.EventHandler(this.btnDigitalSplit_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // rbBlastID
            // 
            this.rbBlastID.AutoSize = true;
            this.rbBlastID.Checked = true;
            this.rbBlastID.Location = new System.Drawing.Point(154, 29);
            this.rbBlastID.Name = "rbBlastID";
            this.rbBlastID.Size = new System.Drawing.Size(67, 17);
            this.rbBlastID.TabIndex = 35;
            this.rbBlastID.TabStop = true;
            this.rbBlastID.Text = "Blast IDs";
            this.rbBlastID.UseVisualStyleBackColor = true;
            this.rbBlastID.CheckedChanged += new System.EventHandler(this.rbBlastID_CheckedChanged);
            // 
            // rbGroup
            // 
            this.rbGroup.AutoSize = true;
            this.rbGroup.Location = new System.Drawing.Point(227, 29);
            this.rbGroup.Name = "rbGroup";
            this.rbGroup.Size = new System.Drawing.Size(68, 17);
            this.rbGroup.TabIndex = 36;
            this.rbGroup.Text = "Group ID";
            this.rbGroup.UseVisualStyleBackColor = true;
            this.rbGroup.CheckedChanged += new System.EventHandler(this.rbGroup_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Export Option:";
            // 
            // pnlGroupSelect
            // 
            this.pnlGroupSelect.Controls.Add(this.txtGroupID);
            this.pnlGroupSelect.Controls.Add(this.dtTo);
            this.pnlGroupSelect.Controls.Add(this.dtFrom);
            this.pnlGroupSelect.Controls.Add(this.label5);
            this.pnlGroupSelect.Controls.Add(this.label4);
            this.pnlGroupSelect.Controls.Add(this.label2);
            this.pnlGroupSelect.Enabled = false;
            this.pnlGroupSelect.Location = new System.Drawing.Point(77, 53);
            this.pnlGroupSelect.Name = "pnlGroupSelect";
            this.pnlGroupSelect.Size = new System.Drawing.Size(587, 70);
            this.pnlGroupSelect.TabIndex = 38;
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(318, 36);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 20);
            this.dtTo.TabIndex = 44;
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(77, 36);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 20);
            this.dtFrom.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "To: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "From: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Group ID: ";
            // 
            // txtGroupID
            // 
            this.txtGroupID.Location = new System.Drawing.Point(78, 9);
            this.txtGroupID.Name = "txtGroupID";
            this.txtGroupID.Size = new System.Drawing.Size(440, 20);
            this.txtGroupID.TabIndex = 45;
            // 
            // Port25
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 537);
            this.Controls.Add(this.pnlGroupSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbGroup);
            this.Controls.Add(this.rbBlastID);
            this.Controls.Add(this.txtFolderLocation);
            this.Controls.Add(this.txtDigitalSplit);
            this.Controls.Add(this.btnDigitalSplit);
            this.Controls.Add(this.cbxDigitalSplit);
            this.Controls.Add(this.txtBlastID);
            this.Controls.Add(this.lblBlastID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFileLocation);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateLogFile);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Port25";
            this.Text = "Port25";
            this.pnlGroupSelect.ResumeLayout(false);
            this.pnlGroupSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFileLocation;
        private System.Windows.Forms.TextBox txtFolderLocation;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreateLogFile;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblBlastID;
        private System.Windows.Forms.TextBox txtBlastID;
        private System.Windows.Forms.TextBox txtDigitalSplit;
        private System.Windows.Forms.CheckBox cbxDigitalSplit;
        private System.Windows.Forms.Button btnDigitalSplit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RadioButton rbBlastID;
        private System.Windows.Forms.RadioButton rbGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlGroupSelect;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGroupID;
    }
}