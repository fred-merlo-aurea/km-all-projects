namespace KMPS_Tools
{
    partial class FPImportNitely
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.btnChooseDBF = new System.Windows.Forms.Button();
            this.txtDBFFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(621, 484);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(151, 43);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(453, 484);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 43);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel Import";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.Location = new System.Drawing.Point(13, 74);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(960, 368);
            this.lstMessage.TabIndex = 22;
            // 
            // btnChooseDBF
            // 
            this.btnChooseDBF.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnChooseDBF.Location = new System.Drawing.Point(590, 16);
            this.btnChooseDBF.Name = "btnChooseDBF";
            this.btnChooseDBF.Size = new System.Drawing.Size(83, 20);
            this.btnChooseDBF.TabIndex = 21;
            this.btnChooseDBF.Text = "Select DBF";
            this.btnChooseDBF.UseVisualStyleBackColor = true;
            this.btnChooseDBF.Click += new System.EventHandler(this.btnChooseDBF_Click);
            // 
            // txtDBFFile
            // 
            this.txtDBFFile.Location = new System.Drawing.Point(83, 17);
            this.txtDBFFile.Name = "txtDBFFile";
            this.txtDBFFile.ReadOnly = true;
            this.txtDBFFile.Size = new System.Drawing.Size(501, 20);
            this.txtDBFFile.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "DBF File : ";
            // 
            // btnImport
            // 
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnImport.Location = new System.Drawing.Point(281, 484);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(151, 43);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Start Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
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
            this.openFileDialog1.Filter = "DBF File (*.dbf)|*.dbf";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(83, 45);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Publication : ";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 448);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(961, 21);
            this.progressBar1.TabIndex = 29;
            // 
            // FPImportNitely
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 543);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnChooseDBF);
            this.Controls.Add(this.txtDBFFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImport);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPImportNitely";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FP Import Nitely";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FPImportNitely_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.Button btnChooseDBF;
        private System.Windows.Forms.TextBox txtDBFFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImport;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}