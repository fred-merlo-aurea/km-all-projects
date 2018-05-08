namespace DBFtoUAD_Circ_Migration
{
    partial class Form1
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
            this.cbClient = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPub = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnChooseLogFile = new System.Windows.Forms.Button();
            this.txtDBFFileName = new System.Windows.Forms.TextBox();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.lblDBFfile = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // cbClient
            // 
            this.cbClient.DisplayMember = "clientName";
            this.cbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClient.FormattingEnabled = true;
            this.cbClient.Location = new System.Drawing.Point(100, 17);
            this.cbClient.Name = "cbClient";
            this.cbClient.Size = new System.Drawing.Size(318, 21);
            this.cbClient.TabIndex = 0;
            this.cbClient.ValueMember = "clientID";
            this.cbClient.SelectedIndexChanged += new System.EventHandler(this.cbClient_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Client";
            // 
            // cbPub
            // 
            this.cbPub.DisplayMember = "PubName";
            this.cbPub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPub.FormattingEnabled = true;
            this.cbPub.Location = new System.Drawing.Point(100, 49);
            this.cbPub.Name = "cbPub";
            this.cbPub.Size = new System.Drawing.Size(318, 21);
            this.cbPub.TabIndex = 2;
            this.cbPub.ValueMember = "PubCode";
            this.cbPub.SelectedIndexChanged += new System.EventHandler(this.cbPub_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pub";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnChooseLogFile
            // 
            this.btnChooseLogFile.Location = new System.Drawing.Point(425, 81);
            this.btnChooseLogFile.Name = "btnChooseLogFile";
            this.btnChooseLogFile.Size = new System.Drawing.Size(94, 24);
            this.btnChooseLogFile.TabIndex = 6;
            this.btnChooseLogFile.Text = "Select DBF File";
            this.btnChooseLogFile.UseVisualStyleBackColor = true;
            this.btnChooseLogFile.Click += new System.EventHandler(this.btnChooseLogFile_Click);
            // 
            // txtDBFFileName
            // 
            this.txtDBFFileName.Location = new System.Drawing.Point(100, 83);
            this.txtDBFFileName.Name = "txtDBFFileName";
            this.txtDBFFileName.ReadOnly = true;
            this.txtDBFFileName.Size = new System.Drawing.Size(318, 20);
            this.txtDBFFileName.TabIndex = 5;
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.HorizontalScrollbar = true;
            this.lstMessage.Location = new System.Drawing.Point(15, 140);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(595, 238);
            this.lstMessage.TabIndex = 7;
            // 
            // lblDBFfile
            // 
            this.lblDBFfile.AutoSize = true;
            this.lblDBFfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBFfile.Location = new System.Drawing.Point(29, 83);
            this.lblDBFfile.Name = "lblDBFfile";
            this.lblDBFfile.Size = new System.Drawing.Size(55, 13);
            this.lblDBFfile.TabIndex = 8;
            this.lblDBFfile.Text = "DBF File";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(237, 397);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(151, 43);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start Import";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 455);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblDBFfile);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnChooseLogFile);
            this.Controls.Add(this.txtDBFFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbPub);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbClient);
            this.Name = "Form1";
            this.Text = "DBF Import";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbClient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPub;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnChooseLogFile;
        private System.Windows.Forms.ListBox lstMessage;
        public System.Windows.Forms.TextBox txtDBFFileName;
        private System.Windows.Forms.Label lblDBFfile;
        private System.Windows.Forms.Button btnStart;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

