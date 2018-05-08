namespace ECNTools.BPALog
{
    partial class BPALogFix2
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBPAFileName = new System.Windows.Forms.TextBox();
            this.txtFolderLocation = new System.Windows.Forms.TextBox();
            this.btnChooseLogFile = new System.Windows.Forms.Button();
            this.btnFileLocation = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.gridBlastReport = new System.Windows.Forms.DataGridView();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridBlastReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BPA File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Save File to:";
            // 
            // txtBPAFileName
            // 
            this.txtBPAFileName.Location = new System.Drawing.Point(104, 16);
            this.txtBPAFileName.Name = "txtBPAFileName";
            this.txtBPAFileName.Size = new System.Drawing.Size(416, 20);
            this.txtBPAFileName.TabIndex = 2;
            // 
            // txtFolderLocation
            // 
            this.txtFolderLocation.Location = new System.Drawing.Point(104, 48);
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.Size = new System.Drawing.Size(416, 20);
            this.txtFolderLocation.TabIndex = 3;
            // 
            // btnChooseLogFile
            // 
            this.btnChooseLogFile.Location = new System.Drawing.Point(528, 16);
            this.btnChooseLogFile.Name = "btnChooseLogFile";
            this.btnChooseLogFile.Size = new System.Drawing.Size(75, 23);
            this.btnChooseLogFile.TabIndex = 4;
            this.btnChooseLogFile.Text = "Select Log";
            this.btnChooseLogFile.UseVisualStyleBackColor = true;
            this.btnChooseLogFile.Click += new System.EventHandler(this.btnChooseDBF_Click);
            // 
            // btnFileLocation
            // 
            this.btnFileLocation.Location = new System.Drawing.Point(528, 48);
            this.btnFileLocation.Name = "btnFileLocation";
            this.btnFileLocation.Size = new System.Drawing.Size(75, 23);
            this.btnFileLocation.TabIndex = 5;
            this.btnFileLocation.Text = "Select Folder";
            this.btnFileLocation.UseVisualStyleBackColor = true;
            this.btnFileLocation.Click += new System.EventHandler(this.btnFileLocation_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "ECN Stats:";
            // 
            // gridBlastReport
            // 
            this.gridBlastReport.AutoGenerateColumns = false;
            this.gridBlastReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBlastReport.DataSource = this.bindingSource1;
            this.gridBlastReport.Location = new System.Drawing.Point(16, 112);
            this.gridBlastReport.Name = "gridBlastReport";
            this.gridBlastReport.Size = new System.Drawing.Size(600, 88);
            this.gridBlastReport.TabIndex = 7;
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.Location = new System.Drawing.Point(16, 208);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(600, 277);
            this.lstMessage.TabIndex = 8;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(16, 496);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(600, 120);
            this.txtMessage.TabIndex = 9;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(32, 624);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(136, 40);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start Processing";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(248, 624);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 40);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel Processing";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(464, 624);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(136, 40);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.openFileDialog1.Filter = "TXT File (*.tdt)|*.txt";
            // 
            // BPALogFix2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 679);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.gridBlastReport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFileLocation);
            this.Controls.Add(this.btnChooseLogFile);
            this.Controls.Add(this.txtFolderLocation);
            this.Controls.Add(this.txtBPAFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BPALogFix2";
            this.Text = "BPALogFix2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BPALogFix_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridBlastReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBPAFileName;
        private System.Windows.Forms.Button btnChooseLogFile;
        private System.Windows.Forms.Button btnFileLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView gridBlastReport;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}