namespace ActiveUp.WebControls
{
    partial class BarcodePropertyBuilderForm
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._bApply = new System.Windows.Forms.Button();
            this.grouper5 = new ActiveUp.WebControls.Grouper();
            this._pbBarcode = new System.Windows.Forms.PictureBox();
            this.grouper6 = new ActiveUp.WebControls.Grouper();
            this._gbHIBCType = new System.Windows.Forms.GroupBox();
            this._rbHIBC = new System.Windows.Forms.RadioButton();
            this._rbStandard = new System.Windows.Forms.RadioButton();
            this._bGenerate = new System.Windows.Forms.Button();
            this._nupDpi = new System.Windows.Forms.NumericUpDown();
            this._lDpi = new System.Windows.Forms.Label();
            this._gbHIBC = new ActiveUp.WebControls.Grouper();
            this.grouper4 = new ActiveUp.WebControls.Grouper();
            this._gbLevel = new System.Windows.Forms.GroupBox();
            this._rbSecondary = new System.Windows.Forms.RadioButton();
            this._rbPrimary = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this._comboSecondaryMode = new System.Windows.Forms.ComboBox();
            this._comboDateFormat = new System.Windows.Forms.ComboBox();
            this._tbSerial = new System.Windows.Forms.TextBox();
            this._tbQuantity = new System.Windows.Forms.TextBox();
            this._tbDate = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._rbHIBCGS1 = new System.Windows.Forms.RadioButton();
            this._rbHIBCLIC = new System.Windows.Forms.RadioButton();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this._comboPrimaryMode = new System.Windows.Forms.ComboBox();
            this._comboUnitOfMeasurement = new System.Windows.Forms.ComboBox();
            this._tbPCN = new System.Windows.Forms.TextBox();
            this._tbLICorGS1 = new System.Windows.Forms.TextBox();
            this._gbStandard = new ActiveUp.WebControls.Grouper();
            this.grouper2 = new ActiveUp.WebControls.Grouper();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._cbGuarded = new System.Windows.Forms.CheckBox();
            this._cbNotched = new System.Windows.Forms.CheckBox();
            this._cbBoxed = new System.Windows.Forms.CheckBox();
            this._cbNumbered = new System.Windows.Forms.CheckBox();
            this._cbBraced = new System.Windows.Forms.CheckBox();
            this._lDirectories = new System.Windows.Forms.Label();
            this._rbModule = new System.Windows.Forms.RadioButton();
            this._rbDesiredWidth = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._nudHeight = new System.Windows.Forms.NumericUpDown();
            this._nudDesiredWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._nudModuleWidth = new System.Windows.Forms.NumericUpDown();
            this._tbData = new System.Windows.Forms.TextBox();
            this._comboType = new System.Windows.Forms.ComboBox();
            this.grouper5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pbBarcode)).BeginInit();
            this.grouper6.SuspendLayout();
            this._gbHIBCType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nupDpi)).BeginInit();
            this._gbHIBC.SuspendLayout();
            this.grouper4.SuspendLayout();
            this._gbLevel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this._gbStandard.SuspendLayout();
            this.grouper2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudDesiredWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudModuleWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // _bApply
            // 
            this._bApply.Location = new System.Drawing.Point(98, 87);
            this._bApply.Name = "_bApply";
            this._bApply.Size = new System.Drawing.Size(75, 23);
            this._bApply.TabIndex = 4;
            this._bApply.Text = "Apply";
            this._bApply.UseVisualStyleBackColor = true;
            this._bApply.Click += new System.EventHandler(this._bApply_Click);
            // 
            // grouper5
            // 
            this.grouper5.BackgroundColor = System.Drawing.Color.White;
            this.grouper5.BackgroundGradientColor = System.Drawing.SystemColors.Desktop;
            this.grouper5.BackgroundGradientMode = ActiveUp.WebControls.Grouper.GroupBoxGradientMode.Vertical;
            this.grouper5.BorderColor = System.Drawing.Color.Black;
            this.grouper5.BorderThickness = 1F;
            this.grouper5.Controls.Add(this._pbBarcode);
            this.grouper5.Controls.Add(this.grouper6);
            this.grouper5.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouper5.GroupImage = null;
            this.grouper5.GroupTitle = "Barcode";
            this.grouper5.Location = new System.Drawing.Point(12, 10);
            this.grouper5.Name = "grouper5";
            this.grouper5.Padding = new System.Windows.Forms.Padding(20);
            this.grouper5.PaintGroupBox = false;
            this.grouper5.RoundCorners = 15;
            this.grouper5.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper5.ShadowControl = true;
            this.grouper5.ShadowThickness = 5;
            this.grouper5.Size = new System.Drawing.Size(562, 159);
            this.grouper5.TabIndex = 0;
            // 
            // _pbBarcode
            // 
            this._pbBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pbBarcode.Location = new System.Drawing.Point(209, 18);
            this._pbBarcode.Name = "_pbBarcode";
            this._pbBarcode.Size = new System.Drawing.Size(338, 128);
            this._pbBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._pbBarcode.TabIndex = 2;
            this._pbBarcode.TabStop = false;
            // 
            // grouper6
            // 
            this.grouper6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grouper6.BackgroundColor = System.Drawing.Color.White;
            this.grouper6.BackgroundGradientColor = System.Drawing.Color.White;
            this.grouper6.BackgroundGradientMode = ActiveUp.WebControls.Grouper.GroupBoxGradientMode.None;
            this.grouper6.BorderColor = System.Drawing.Color.Black;
            this.grouper6.BorderThickness = 1F;
            this.grouper6.Controls.Add(this._bApply);
            this.grouper6.Controls.Add(this._gbHIBCType);
            this.grouper6.Controls.Add(this._bGenerate);
            this.grouper6.Controls.Add(this._nupDpi);
            this.grouper6.Controls.Add(this._lDpi);
            this.grouper6.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouper6.GroupImage = null;
            this.grouper6.GroupTitle = "";
            this.grouper6.Location = new System.Drawing.Point(15, 23);
            this.grouper6.Name = "grouper6";
            this.grouper6.Padding = new System.Windows.Forms.Padding(20);
            this.grouper6.PaintGroupBox = false;
            this.grouper6.RoundCorners = 10;
            this.grouper6.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper6.ShadowControl = false;
            this.grouper6.ShadowThickness = 3;
            this.grouper6.Size = new System.Drawing.Size(188, 123);
            this.grouper6.TabIndex = 0;
            // 
            // _gbHIBCType
            // 
            this._gbHIBCType.Controls.Add(this._rbHIBC);
            this._gbHIBCType.Controls.Add(this._rbStandard);
            this._gbHIBCType.Location = new System.Drawing.Point(10, 13);
            this._gbHIBCType.Name = "_gbHIBCType";
            this._gbHIBCType.Size = new System.Drawing.Size(163, 42);
            this._gbHIBCType.TabIndex = 0;
            this._gbHIBCType.TabStop = false;
            this._gbHIBCType.Text = "HIBC Type";
            // 
            // _rbHIBC
            // 
            this._rbHIBC.AutoSize = true;
            this._rbHIBC.Location = new System.Drawing.Point(83, 19);
            this._rbHIBC.Name = "_rbHIBC";
            this._rbHIBC.Size = new System.Drawing.Size(50, 17);
            this._rbHIBC.TabIndex = 1;
            this._rbHIBC.Text = "HIBC";
            this._rbHIBC.UseVisualStyleBackColor = true;
            this._rbHIBC.CheckedChanged += new System.EventHandler(this._rbHIBC_CheckedChanged);
            // 
            // _rbStandard
            // 
            this._rbStandard.AutoSize = true;
            this._rbStandard.Checked = true;
            this._rbStandard.Location = new System.Drawing.Point(7, 19);
            this._rbStandard.Name = "_rbStandard";
            this._rbStandard.Size = new System.Drawing.Size(68, 17);
            this._rbStandard.TabIndex = 0;
            this._rbStandard.TabStop = true;
            this._rbStandard.Text = "Standard";
            this._rbStandard.UseVisualStyleBackColor = true;
            this._rbStandard.CheckedChanged += new System.EventHandler(this._rbStandard_CheckedChanged);
            // 
            // _bGenerate
            // 
            this._bGenerate.Location = new System.Drawing.Point(8, 87);
            this._bGenerate.Name = "_bGenerate";
            this._bGenerate.Size = new System.Drawing.Size(75, 23);
            this._bGenerate.TabIndex = 3;
            this._bGenerate.Text = "Generate";
            this._bGenerate.UseVisualStyleBackColor = true;
            this._bGenerate.Click += new System.EventHandler(this._bGenerate_Click);
            // 
            // _nupDpi
            // 
            this._nupDpi.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nupDpi.Location = new System.Drawing.Point(53, 61);
            this._nupDpi.Maximum = new decimal(new int[] {
            12000,
            0,
            0,
            0});
            this._nupDpi.Name = "_nupDpi";
            this._nupDpi.Size = new System.Drawing.Size(120, 20);
            this._nupDpi.TabIndex = 2;
            this._nupDpi.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // _lDpi
            // 
            this._lDpi.AutoSize = true;
            this._lDpi.Location = new System.Drawing.Point(18, 63);
            this._lDpi.Name = "_lDpi";
            this._lDpi.Size = new System.Drawing.Size(25, 13);
            this._lDpi.TabIndex = 1;
            this._lDpi.Text = "DPI";
            // 
            // _gbHIBC
            // 
            this._gbHIBC.BackgroundColor = System.Drawing.Color.White;
            this._gbHIBC.BackgroundGradientColor = System.Drawing.SystemColors.Desktop;
            this._gbHIBC.BackgroundGradientMode = ActiveUp.WebControls.Grouper.GroupBoxGradientMode.Vertical;
            this._gbHIBC.BorderColor = System.Drawing.Color.Black;
            this._gbHIBC.BorderThickness = 1F;
            this._gbHIBC.Controls.Add(this.grouper4);
            this._gbHIBC.CustomGroupBoxColor = System.Drawing.Color.White;
            this._gbHIBC.GroupImage = null;
            this._gbHIBC.GroupTitle = "HIBC";
            this._gbHIBC.Location = new System.Drawing.Point(12, 401);
            this._gbHIBC.Name = "_gbHIBC";
            this._gbHIBC.Padding = new System.Windows.Forms.Padding(20);
            this._gbHIBC.PaintGroupBox = false;
            this._gbHIBC.RoundCorners = 15;
            this._gbHIBC.ShadowColor = System.Drawing.Color.DarkGray;
            this._gbHIBC.ShadowControl = true;
            this._gbHIBC.ShadowThickness = 5;
            this._gbHIBC.Size = new System.Drawing.Size(562, 231);
            this._gbHIBC.TabIndex = 2;
            this._gbHIBC.Visible = false;
            // 
            // grouper4
            // 
            this.grouper4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grouper4.BackgroundColor = System.Drawing.Color.White;
            this.grouper4.BackgroundGradientColor = System.Drawing.Color.White;
            this.grouper4.BackgroundGradientMode = ActiveUp.WebControls.Grouper.GroupBoxGradientMode.None;
            this.grouper4.BorderColor = System.Drawing.Color.Black;
            this.grouper4.BorderThickness = 1F;
            this.grouper4.Controls.Add(this._gbLevel);
            this.grouper4.Controls.Add(this.groupBox1);
            this.grouper4.Controls.Add(this.groupBox2);
            this.grouper4.Controls.Add(this.label18);
            this.grouper4.Controls.Add(this.label17);
            this.grouper4.Controls.Add(this.label16);
            this.grouper4.Controls.Add(this.label15);
            this.grouper4.Controls.Add(this._comboPrimaryMode);
            this.grouper4.Controls.Add(this._comboUnitOfMeasurement);
            this.grouper4.Controls.Add(this._tbPCN);
            this.grouper4.Controls.Add(this._tbLICorGS1);
            this.grouper4.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouper4.GroupImage = null;
            this.grouper4.GroupTitle = "";
            this.grouper4.Location = new System.Drawing.Point(15, 23);
            this.grouper4.Name = "grouper4";
            this.grouper4.Padding = new System.Windows.Forms.Padding(20);
            this.grouper4.PaintGroupBox = false;
            this.grouper4.RoundCorners = 10;
            this.grouper4.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper4.ShadowControl = false;
            this.grouper4.ShadowThickness = 3;
            this.grouper4.Size = new System.Drawing.Size(532, 195);
            this.grouper4.TabIndex = 0;
            // 
            // _gbLevel
            // 
            this._gbLevel.Controls.Add(this._rbSecondary);
            this._gbLevel.Controls.Add(this._rbPrimary);
            this._gbLevel.Location = new System.Drawing.Point(10, 124);
            this._gbLevel.Name = "_gbLevel";
            this._gbLevel.Size = new System.Drawing.Size(133, 61);
            this._gbLevel.TabIndex = 8;
            this._gbLevel.TabStop = false;
            this._gbLevel.Text = "Level";
            // 
            // _rbSecondary
            // 
            this._rbSecondary.AutoSize = true;
            this._rbSecondary.Location = new System.Drawing.Point(16, 39);
            this._rbSecondary.Name = "_rbSecondary";
            this._rbSecondary.Size = new System.Drawing.Size(76, 17);
            this._rbSecondary.TabIndex = 1;
            this._rbSecondary.Text = "Secondary";
            this._rbSecondary.UseVisualStyleBackColor = true;
            this._rbSecondary.CheckedChanged += new System.EventHandler(this._rbSecondary_CheckedChanged);
            // 
            // _rbPrimary
            // 
            this._rbPrimary.AutoSize = true;
            this._rbPrimary.Checked = true;
            this._rbPrimary.Location = new System.Drawing.Point(16, 18);
            this._rbPrimary.Name = "_rbPrimary";
            this._rbPrimary.Size = new System.Drawing.Size(59, 17);
            this._rbPrimary.TabIndex = 0;
            this._rbPrimary.TabStop = true;
            this._rbPrimary.Text = "Primary";
            this._rbPrimary.UseVisualStyleBackColor = true;
            this._rbPrimary.CheckedChanged += new System.EventHandler(this._rbPrimary_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this._comboSecondaryMode);
            this.groupBox1.Controls.Add(this._comboDateFormat);
            this.groupBox1.Controls.Add(this._tbSerial);
            this.groupBox1.Controls.Add(this._tbQuantity);
            this.groupBox1.Controls.Add(this._tbDate);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(253, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 168);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Secondary Data";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(10, 129);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(106, 13);
            this.label20.TabIndex = 8;
            this.label20.Text = "Secondary Encoding";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(51, 101);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 13);
            this.label19.TabIndex = 6;
            this.label19.Text = "Date Format";
            // 
            // _comboSecondaryMode
            // 
            this._comboSecondaryMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboSecondaryMode.FormattingEnabled = true;
            this._comboSecondaryMode.Items.AddRange(new object[] {
            "None",
            "Combined",
            "Code39",
            "Code128",
            "EAN128"});
            this._comboSecondaryMode.Location = new System.Drawing.Point(122, 126);
            this._comboSecondaryMode.Name = "_comboSecondaryMode";
            this._comboSecondaryMode.Size = new System.Drawing.Size(143, 21);
            this._comboSecondaryMode.TabIndex = 9;
            // 
            // _comboDateFormat
            // 
            this._comboDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboDateFormat.FormattingEnabled = true;
            this._comboDateFormat.Items.AddRange(new object[] {
            "MonthDayYear",
            "MonthYear",
            "YearJulian",
            "YearJulianHour",
            "YearMonthDay",
            "YearMonthDayHour"});
            this._comboDateFormat.Location = new System.Drawing.Point(122, 98);
            this._comboDateFormat.Name = "_comboDateFormat";
            this._comboDateFormat.Size = new System.Drawing.Size(143, 21);
            this._comboDateFormat.TabIndex = 7;
            // 
            // _tbSerial
            // 
            this._tbSerial.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._tbSerial.Location = new System.Drawing.Point(122, 71);
            this._tbSerial.MaxLength = 13;
            this._tbSerial.Name = "_tbSerial";
            this._tbSerial.Size = new System.Drawing.Size(143, 20);
            this._tbSerial.TabIndex = 5;
            // 
            // _tbQuantity
            // 
            this._tbQuantity.Location = new System.Drawing.Point(122, 45);
            this._tbQuantity.MaxLength = 5;
            this._tbQuantity.Name = "_tbQuantity";
            this._tbQuantity.Size = new System.Drawing.Size(143, 20);
            this._tbQuantity.TabIndex = 3;
            // 
            // _tbDate
            // 
            this._tbDate.Location = new System.Drawing.Point(122, 19);
            this._tbDate.MaxLength = 50;
            this._tbDate.Name = "_tbDate";
            this._tbDate.Size = new System.Drawing.Size(143, 20);
            this._tbDate.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(63, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Serial/Lot";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(70, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Quantity";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(86, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._rbHIBCGS1);
            this.groupBox2.Controls.Add(this._rbHIBCLIC);
            this.groupBox2.Location = new System.Drawing.Point(149, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(98, 61);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HIBC Type";
            // 
            // _rbHIBCGS1
            // 
            this._rbHIBCGS1.AutoSize = true;
            this._rbHIBCGS1.Location = new System.Drawing.Point(11, 39);
            this._rbHIBCGS1.Name = "_rbHIBCGS1";
            this._rbHIBCGS1.Size = new System.Drawing.Size(74, 17);
            this._rbHIBCGS1.TabIndex = 1;
            this._rbHIBCGS1.Text = "HIBC GS1";
            this._rbHIBCGS1.UseVisualStyleBackColor = true;
            // 
            // _rbHIBCLIC
            // 
            this._rbHIBCLIC.AutoSize = true;
            this._rbHIBCLIC.Checked = true;
            this._rbHIBCLIC.Location = new System.Drawing.Point(11, 18);
            this._rbHIBCLIC.Name = "_rbHIBCLIC";
            this._rbHIBCLIC.Size = new System.Drawing.Size(69, 17);
            this._rbHIBCLIC.TabIndex = 0;
            this._rbHIBCLIC.TabStop = true;
            this._rbHIBCLIC.Text = "HIBC LIC";
            this._rbHIBCLIC.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 100);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Primary Encoding";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(14, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Unit of Measure";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(67, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "PCN";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(47, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "LIC/GS1";
            // 
            // _comboPrimaryMode
            // 
            this._comboPrimaryMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboPrimaryMode.FormattingEnabled = true;
            this._comboPrimaryMode.Items.AddRange(new object[] {
            "Code39",
            "Code128",
            "EAN128",
            "I2of5"});
            this._comboPrimaryMode.Location = new System.Drawing.Point(102, 97);
            this._comboPrimaryMode.Name = "_comboPrimaryMode";
            this._comboPrimaryMode.Size = new System.Drawing.Size(137, 21);
            this._comboPrimaryMode.TabIndex = 7;
            // 
            // _comboUnitOfMeasurement
            // 
            this._comboUnitOfMeasurement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboUnitOfMeasurement.FormattingEnabled = true;
            this._comboUnitOfMeasurement.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this._comboUnitOfMeasurement.Location = new System.Drawing.Point(102, 70);
            this._comboUnitOfMeasurement.MaxDropDownItems = 10;
            this._comboUnitOfMeasurement.Name = "_comboUnitOfMeasurement";
            this._comboUnitOfMeasurement.Size = new System.Drawing.Size(137, 21);
            this._comboUnitOfMeasurement.TabIndex = 5;
            // 
            // _tbPCN
            // 
            this._tbPCN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._tbPCN.Location = new System.Drawing.Point(102, 43);
            this._tbPCN.Name = "_tbPCN";
            this._tbPCN.Size = new System.Drawing.Size(138, 20);
            this._tbPCN.TabIndex = 3;
            // 
            // _tbLICorGS1
            // 
            this._tbLICorGS1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._tbLICorGS1.Location = new System.Drawing.Point(102, 17);
            this._tbLICorGS1.Name = "_tbLICorGS1";
            this._tbLICorGS1.Size = new System.Drawing.Size(138, 20);
            this._tbLICorGS1.TabIndex = 1;
            // 
            // _gbStandard
            // 
            this._gbStandard.BackgroundColor = System.Drawing.Color.White;
            this._gbStandard.BackgroundGradientColor = System.Drawing.SystemColors.Desktop;
            this._gbStandard.BackgroundGradientMode = ActiveUp.WebControls.Grouper.GroupBoxGradientMode.Vertical;
            this._gbStandard.BorderColor = System.Drawing.Color.Black;
            this._gbStandard.BorderThickness = 1F;
            this._gbStandard.Controls.Add(this.grouper2);
            this._gbStandard.CustomGroupBoxColor = System.Drawing.Color.White;
            this._gbStandard.GroupImage = null;
            this._gbStandard.GroupTitle = "Standard";
            this._gbStandard.Location = new System.Drawing.Point(12, 175);
            this._gbStandard.Name = "_gbStandard";
            this._gbStandard.Padding = new System.Windows.Forms.Padding(20);
            this._gbStandard.PaintGroupBox = false;
            this._gbStandard.RoundCorners = 15;
            this._gbStandard.ShadowColor = System.Drawing.Color.DarkGray;
            this._gbStandard.ShadowControl = true;
            this._gbStandard.ShadowThickness = 5;
            this._gbStandard.Size = new System.Drawing.Size(562, 220);
            this._gbStandard.TabIndex = 1;
            // 
            // grouper2
            // 
            this.grouper2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grouper2.BackgroundColor = System.Drawing.Color.White;
            this.grouper2.BackgroundGradientColor = System.Drawing.Color.White;
            this.grouper2.BackgroundGradientMode = ActiveUp.WebControls.Grouper.GroupBoxGradientMode.None;
            this.grouper2.BorderColor = System.Drawing.Color.Black;
            this.grouper2.BorderThickness = 1F;
            this.grouper2.Controls.Add(this.groupBox3);
            this.grouper2.Controls.Add(this._lDirectories);
            this.grouper2.Controls.Add(this._rbModule);
            this.grouper2.Controls.Add(this._rbDesiredWidth);
            this.grouper2.Controls.Add(this.label11);
            this.grouper2.Controls.Add(this.label3);
            this.grouper2.Controls.Add(this.label8);
            this.grouper2.Controls.Add(this.label7);
            this.grouper2.Controls.Add(this._nudHeight);
            this.grouper2.Controls.Add(this._nudDesiredWidth);
            this.grouper2.Controls.Add(this.label2);
            this.grouper2.Controls.Add(this.label1);
            this.grouper2.Controls.Add(this.label6);
            this.grouper2.Controls.Add(this._nudModuleWidth);
            this.grouper2.Controls.Add(this._tbData);
            this.grouper2.Controls.Add(this._comboType);
            this.grouper2.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouper2.GroupImage = null;
            this.grouper2.GroupTitle = "";
            this.grouper2.Location = new System.Drawing.Point(15, 23);
            this.grouper2.Name = "grouper2";
            this.grouper2.Padding = new System.Windows.Forms.Padding(20);
            this.grouper2.PaintGroupBox = false;
            this.grouper2.RoundCorners = 10;
            this.grouper2.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper2.ShadowControl = false;
            this.grouper2.ShadowThickness = 3;
            this.grouper2.Size = new System.Drawing.Size(532, 184);
            this.grouper2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._cbGuarded);
            this.groupBox3.Controls.Add(this._cbNotched);
            this.groupBox3.Controls.Add(this._cbBoxed);
            this.groupBox3.Controls.Add(this._cbNumbered);
            this.groupBox3.Controls.Add(this._cbBraced);
            this.groupBox3.Location = new System.Drawing.Point(417, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(109, 153);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // _cbGuarded
            // 
            this._cbGuarded.AutoSize = true;
            this._cbGuarded.Location = new System.Drawing.Point(15, 31);
            this._cbGuarded.Name = "_cbGuarded";
            this._cbGuarded.Size = new System.Drawing.Size(67, 17);
            this._cbGuarded.TabIndex = 0;
            this._cbGuarded.Text = "Guarded";
            this._cbGuarded.UseVisualStyleBackColor = true;
            // 
            // _cbNotched
            // 
            this._cbNotched.AutoSize = true;
            this._cbNotched.Location = new System.Drawing.Point(15, 55);
            this._cbNotched.Name = "_cbNotched";
            this._cbNotched.Size = new System.Drawing.Size(67, 17);
            this._cbNotched.TabIndex = 1;
            this._cbNotched.Text = "Notched";
            this._cbNotched.UseVisualStyleBackColor = true;
            // 
            // _cbBoxed
            // 
            this._cbBoxed.AutoSize = true;
            this._cbBoxed.Location = new System.Drawing.Point(15, 126);
            this._cbBoxed.Name = "_cbBoxed";
            this._cbBoxed.Size = new System.Drawing.Size(56, 17);
            this._cbBoxed.TabIndex = 4;
            this._cbBoxed.Text = "Boxed";
            this._cbBoxed.UseVisualStyleBackColor = true;
            // 
            // _cbNumbered
            // 
            this._cbNumbered.AutoSize = true;
            this._cbNumbered.Location = new System.Drawing.Point(15, 79);
            this._cbNumbered.Name = "_cbNumbered";
            this._cbNumbered.Size = new System.Drawing.Size(75, 17);
            this._cbNumbered.TabIndex = 2;
            this._cbNumbered.Text = "Numbered";
            this._cbNumbered.UseVisualStyleBackColor = true;
            // 
            // _cbBraced
            // 
            this._cbBraced.AutoSize = true;
            this._cbBraced.Location = new System.Drawing.Point(15, 102);
            this._cbBraced.Name = "_cbBraced";
            this._cbBraced.Size = new System.Drawing.Size(60, 17);
            this._cbBraced.TabIndex = 3;
            this._cbBraced.Text = "Braced";
            this._cbBraced.UseVisualStyleBackColor = true;
            // 
            // _lDirectories
            // 
            this._lDirectories.AutoSize = true;
            this._lDirectories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lDirectories.Location = new System.Drawing.Point(13, 83);
            this._lDirectories.Name = "_lDirectories";
            this._lDirectories.Size = new System.Drawing.Size(52, 13);
            this._lDirectories.TabIndex = 4;
            this._lDirectories.Text = "Height :";
            // 
            // _rbModule
            // 
            this._rbModule.AutoSize = true;
            this._rbModule.Location = new System.Drawing.Point(102, 152);
            this._rbModule.Name = "_rbModule";
            this._rbModule.Size = new System.Drawing.Size(14, 13);
            this._rbModule.TabIndex = 11;
            this._rbModule.UseVisualStyleBackColor = true;
            // 
            // _rbDesiredWidth
            // 
            this._rbDesiredWidth.AutoSize = true;
            this._rbDesiredWidth.Checked = true;
            this._rbDesiredWidth.Location = new System.Drawing.Point(102, 127);
            this._rbDesiredWidth.Name = "_rbDesiredWidth";
            this._rbDesiredWidth.Size = new System.Drawing.Size(14, 13);
            this._rbDesiredWidth.TabIndex = 8;
            this._rbDesiredWidth.TabStop = true;
            this._rbDesiredWidth.UseVisualStyleBackColor = true;
            this._rbDesiredWidth.CheckedChanged += new System.EventHandler(this._rbDesiredWidth_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(372, 124);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 13);
            this.label11.TabIndex = 72;
            this.label11.Text = "Inches";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Height";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(372, 150);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 69;
            this.label8.Text = "Mils";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Desired Width:";
            // 
            // _nudHeight
            // 
            this._nudHeight.Location = new System.Drawing.Point(124, 98);
            this._nudHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this._nudHeight.Name = "_nudHeight";
            this._nudHeight.Size = new System.Drawing.Size(245, 20);
            this._nudHeight.TabIndex = 6;
            // 
            // _nudDesiredWidth
            // 
            this._nudDesiredWidth.DecimalPlaces = 1;
            this._nudDesiredWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this._nudDesiredWidth.Location = new System.Drawing.Point(124, 124);
            this._nudDesiredWidth.Name = "_nudDesiredWidth";
            this._nudDesiredWidth.Size = new System.Drawing.Size(245, 20);
            this._nudDesiredWidth.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Data:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Module:";
            // 
            // _nudModuleWidth
            // 
            this._nudModuleWidth.DecimalPlaces = 1;
            this._nudModuleWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this._nudModuleWidth.Location = new System.Drawing.Point(124, 150);
            this._nudModuleWidth.Name = "_nudModuleWidth";
            this._nudModuleWidth.ReadOnly = true;
            this._nudModuleWidth.Size = new System.Drawing.Size(245, 20);
            this._nudModuleWidth.TabIndex = 12;
            // 
            // _tbData
            // 
            this._tbData.Location = new System.Drawing.Point(53, 23);
            this._tbData.Name = "_tbData";
            this._tbData.Size = new System.Drawing.Size(356, 20);
            this._tbData.TabIndex = 1;
            // 
            // _comboType
            // 
            this._comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboType.FormattingEnabled = true;
            this._comboType.Items.AddRange(new object[] {
            "EAN13",
            "EAN8",
            "UPCA",
            "UPCE",
            "UPC2/5",
            "ISBN",
            "ISBN13",
            "ISSN",
            "EAN Composite",
            "Postnet",
            "Planet",
            "Code11",
            "Code39",
            "Code93",
            "Code128",
            "MSI",
            "S2of5",
            "I2of5",
            "Codabar",
            "ITF14",
            "HIBC",
            "EAN128"});
            this._comboType.Location = new System.Drawing.Point(53, 49);
            this._comboType.Name = "_comboType";
            this._comboType.Size = new System.Drawing.Size(356, 21);
            this._comboType.TabIndex = 3;
            // 
            // BarecodePropertyBuilderForm
            // 
            this.AcceptButton = this._bApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(582, 631);
            this.Controls.Add(this.grouper5);
            this.Controls.Add(this._gbHIBC);
            this.Controls.Add(this._gbStandard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BarecodePropertyBuilderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Barecode Property Builder";
            this.Load += new System.EventHandler(this.BarecodePropertyBuilderForm_Load);
            this.grouper5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pbBarcode)).EndInit();
            this.grouper6.ResumeLayout(false);
            this.grouper6.PerformLayout();
            this._gbHIBCType.ResumeLayout(false);
            this._gbHIBCType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nupDpi)).EndInit();
            this._gbHIBC.ResumeLayout(false);
            this.grouper4.ResumeLayout(false);
            this.grouper4.PerformLayout();
            this._gbLevel.ResumeLayout(false);
            this._gbLevel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this._gbStandard.ResumeLayout(false);
            this.grouper2.ResumeLayout(false);
            this.grouper2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudDesiredWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudModuleWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Grouper _gbStandard;
        private Grouper grouper2;
        private System.Windows.Forms.CheckBox _cbBoxed;
        private System.Windows.Forms.CheckBox _cbBraced;
        private System.Windows.Forms.RadioButton _rbModule;
        private System.Windows.Forms.RadioButton _rbDesiredWidth;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown _nudHeight;
        private System.Windows.Forms.NumericUpDown _nudDesiredWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _cbNumbered;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox _cbNotched;
        private System.Windows.Forms.NumericUpDown _nudModuleWidth;
        private System.Windows.Forms.CheckBox _cbGuarded;
        private System.Windows.Forms.TextBox _tbData;
        private System.Windows.Forms.ComboBox _comboType;
        private System.Windows.Forms.Label _lDirectories;
        private Grouper _gbHIBC;
        private Grouper grouper4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox _comboSecondaryMode;
        private System.Windows.Forms.ComboBox _comboDateFormat;
        private System.Windows.Forms.TextBox _tbSerial;
        private System.Windows.Forms.TextBox _tbQuantity;
        private System.Windows.Forms.TextBox _tbDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton _rbHIBCGS1;
        private System.Windows.Forms.RadioButton _rbHIBCLIC;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox _comboPrimaryMode;
        private System.Windows.Forms.ComboBox _comboUnitOfMeasurement;
        private System.Windows.Forms.TextBox _tbPCN;
        private System.Windows.Forms.TextBox _tbLICorGS1;
        private System.Windows.Forms.GroupBox groupBox3;
        private Grouper grouper5;
        private Grouper grouper6;
        private System.Windows.Forms.GroupBox _gbHIBCType;
        private System.Windows.Forms.RadioButton _rbHIBC;
        private System.Windows.Forms.RadioButton _rbStandard;
        private System.Windows.Forms.Button _bGenerate;
        private System.Windows.Forms.NumericUpDown _nupDpi;
        private System.Windows.Forms.Label _lDpi;
        private System.Windows.Forms.PictureBox _pbBarcode;
        private System.Windows.Forms.Button _bApply;
        private System.Windows.Forms.GroupBox _gbLevel;
        private System.Windows.Forms.RadioButton _rbSecondary;
        private System.Windows.Forms.RadioButton _rbPrimary;


    }
}