namespace KMPS_Tools
{
    partial class WQTImport
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnImport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBFFile = new System.Windows.Forms.TextBox();
            this.btnChooseDBF = new System.Windows.Forms.Button();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblIssueDate = new System.Windows.Forms.Label();
            this.dtIssueDate = new System.Windows.Forms.DateTimePicker();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "DBF File (*.dbf)|*.dbf";
            // 
            // btnImport
            // 
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnImport.Location = new System.Drawing.Point(280, 481);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(151, 43);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Start Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "DBF File : ";
            // 
            // txtDBFFile
            // 
            this.txtDBFFile.Location = new System.Drawing.Point(76, 14);
            this.txtDBFFile.Name = "txtDBFFile";
            this.txtDBFFile.ReadOnly = true;
            this.txtDBFFile.Size = new System.Drawing.Size(501, 20);
            this.txtDBFFile.TabIndex = 3;
            // 
            // btnChooseDBF
            // 
            this.btnChooseDBF.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnChooseDBF.Location = new System.Drawing.Point(583, 13);
            this.btnChooseDBF.Name = "btnChooseDBF";
            this.btnChooseDBF.Size = new System.Drawing.Size(83, 20);
            this.btnChooseDBF.TabIndex = 4;
            this.btnChooseDBF.Text = "Select DBF";
            this.btnChooseDBF.UseVisualStyleBackColor = true;
            this.btnChooseDBF.Click += new System.EventHandler(this.btnChooseDBF_Click);
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.Location = new System.Drawing.Point(12, 58);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(960, 407);
            this.lstMessage.TabIndex = 5;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(452, 481);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 43);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel Import";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.AutoSize = true;
            this.lblIssueDate.Location = new System.Drawing.Point(767, 15);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(67, 13);
            this.lblIssueDate.TabIndex = 8;
            this.lblIssueDate.Text = "Issue Date : ";
            // 
            // dtIssueDate
            // 
            this.dtIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtIssueDate.Location = new System.Drawing.Point(834, 13);
            this.dtIssueDate.Name = "dtIssueDate";
            this.dtIssueDate.Size = new System.Drawing.Size(98, 20);
            this.dtIssueDate.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(620, 481);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(151, 43);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // WQTImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 543);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dtIssueDate);
            this.Controls.Add(this.lblIssueDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnChooseDBF);
            this.Controls.Add(this.txtDBFFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImport);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WQTImport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "WQT Import";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WQTImport_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBFFile;
        private System.Windows.Forms.Button btnChooseDBF;
        private System.Windows.Forms.ListBox lstMessage;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblIssueDate;
        private System.Windows.Forms.DateTimePicker dtIssueDate;
        private System.Windows.Forms.Button btnClose;

    }
}

