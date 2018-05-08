namespace ECNTools.ECN_Import
{
    partial class FPImport2
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
            this.txtDBFFile = new System.Windows.Forms.TextBox();
            this.btnChooseDBF = new System.Windows.Forms.Button();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DBF File:";
            // 
            // txtDBFFile
            // 
            this.txtDBFFile.BackColor = System.Drawing.SystemColors.Control;
            this.txtDBFFile.Location = new System.Drawing.Point(72, 8);
            this.txtDBFFile.Name = "txtDBFFile";
            this.txtDBFFile.ReadOnly = true;
            this.txtDBFFile.Size = new System.Drawing.Size(344, 20);
            this.txtDBFFile.TabIndex = 1;
            // 
            // btnChooseDBF
            // 
            this.btnChooseDBF.Location = new System.Drawing.Point(424, 8);
            this.btnChooseDBF.Name = "btnChooseDBF";
            this.btnChooseDBF.Size = new System.Drawing.Size(75, 23);
            this.btnChooseDBF.TabIndex = 2;
            this.btnChooseDBF.Text = "Select DBF";
            this.btnChooseDBF.UseVisualStyleBackColor = true;
            this.btnChooseDBF.Click += new System.EventHandler(this.btnChooseDBF_Click);
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.Location = new System.Drawing.Point(16, 48);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(824, 368);
            this.lstMessage.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 424);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(824, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(128, 472);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(144, 40);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Start Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 472);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel Import";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(496, 472);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(136, 40);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "DBF File (*.dbf)|*.dbf";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FPImport2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 572);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnChooseDBF);
            this.Controls.Add(this.txtDBFFile);
            this.Controls.Add(this.label1);
            this.Name = "FPImport2";
            this.Text = "FPImport2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FPImport_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBFFile;
        private System.Windows.Forms.Button btnChooseDBF;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}