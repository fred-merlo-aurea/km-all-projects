namespace UAS.ReportLibrary.Reports
{
	partial class SubFields
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for telerik Reporting designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.NavigateToReportAction navigateToReportAction1 = new Telerik.Reporting.NavigateToReportAction();
            Telerik.Reporting.InstanceReportSource instanceReportSource1 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter5 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.subscriberDetails1 = new UAS.ReportLibrary.Reports.SubscriberDetails();
            this.Demo = new Telerik.Reporting.SqlDataSource();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.DemoTable = new Telerik.Reporting.Table();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.ReportNameTextBox = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.ReportPageNumberTextBox = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.subscriberDetails1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0395833253860474D), Telerik.Reporting.Drawing.Unit.Inch(0.20000001788139343D));
            this.textBox1.StyleName = "Normal.TableHeader";
            this.textBox1.Value = "Edition";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0499999523162842D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox2.StyleName = "Normal.TableHeader";
            this.textBox2.Value = "Copies";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3749996423721314D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox7.StyleName = "Normal.TableHeader";
            this.textBox7.Value = "Percent";
            // 
            // subscriberDetails1
            // 
            this.subscriberDetails1.Name = "PubSubscriptionsReport";
            // 
            // Demo
            // 
            this.Demo.ConnectionString = "UAS.ReportLibrary.Properties.Settings.MTGMasterDB";
            this.Demo.Name = "Demo";
            this.Demo.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@Queries", System.Data.DbType.AnsiString, "= Parameters.FilterQuery.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@Demo", System.Data.DbType.AnsiString, "= Parameters.Demo.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@IssueID", System.Data.DbType.Int32, "= Parameters.IssueID.Value")});
            this.Demo.ProviderName = "System.Data.SqlClient";
            this.Demo.SelectCommand = "dbo.rpt_SubFieldsMVC";
            this.Demo.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
            this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.DemoTable});
            this.detailSection1.Name = "detailSection1";
            // 
            // DemoTable
            // 
            this.DemoTable.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0395833253860474D)));
            this.DemoTable.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0500000715255737D)));
            this.DemoTable.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.3749996423721314D)));
            this.DemoTable.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D)));
            this.DemoTable.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D)));
            this.DemoTable.Body.SetCellContent(0, 0, this.textBox3);
            this.DemoTable.Body.SetCellContent(0, 1, this.textBox4);
            this.DemoTable.Body.SetCellContent(1, 0, this.textBox5);
            this.DemoTable.Body.SetCellContent(1, 1, this.textBox6);
            this.DemoTable.Body.SetCellContent(0, 2, this.textBox8);
            this.DemoTable.Body.SetCellContent(1, 2, this.textBox9);
            tableGroup1.ReportItem = this.textBox1;
            tableGroup2.ReportItem = this.textBox2;
            tableGroup3.Name = "group1";
            tableGroup3.ReportItem = this.textBox7;
            this.DemoTable.ColumnGroups.Add(tableGroup1);
            this.DemoTable.ColumnGroups.Add(tableGroup2);
            this.DemoTable.ColumnGroups.Add(tableGroup3);
            this.DemoTable.DataSource = this.Demo;
            this.DemoTable.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3,
            this.textBox4,
            this.textBox8,
            this.textBox5,
            this.textBox6,
            this.textBox9,
            this.textBox1,
            this.textBox2,
            this.textBox7});
            this.DemoTable.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.099999904632568359D));
            this.DemoTable.Name = "DemoTable";
            tableGroup4.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup4.Name = "Detail";
            tableGroup5.Name = "group";
            this.DemoTable.RowGroups.Add(tableGroup4);
            this.DemoTable.RowGroups.Add(tableGroup5);
            this.DemoTable.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4645829200744629D), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791D));
            this.DemoTable.StyleName = "Normal.TableNormal";
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0395833253860474D), Telerik.Reporting.Drawing.Unit.Inch(0.20000001788139343D));
            this.textBox3.StyleName = "Normal.TableBody";
            this.textBox3.Value = "= Fields.DisplayName";
            // 
            // textBox4
            // 
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ProductName", "= Parameters.ProductName.Value"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ParentReport", "= Parameters.Demo.Value + \" Summary - \" + Fields.DisplayName"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("IssueName", "= Parameters.IssueName.Value"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("IssueID", "= Parameters.IssueID.Value"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("FilterQuery", "= Parameters.FilterQuery.Value"));
            instanceReportSource1.ReportDocument = this.subscriberDetails1;
            navigateToReportAction1.ReportSource = instanceReportSource1;
            this.textBox4.Action = navigateToReportAction1;
            this.textBox4.Format = "{0:N0}";
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0499999523162842D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox4.StyleName = "Normal.TableBody";
            this.textBox4.Value = "= IIf(Fields.Copies > 0, Fields.Copies, 0)";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0395833253860474D), Telerik.Reporting.Drawing.Unit.Inch(0.20000001788139343D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.StyleName = "Normal.TableBody";
            this.textBox5.Value = "Total";
            // 
            // textBox6
            // 
            this.textBox6.Format = "{0:N0}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0499999523162842D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.StyleName = "Normal.TableBody";
            this.textBox6.Value = "= SUM(Fields.Copies)";
            // 
            // textBox8
            // 
            this.textBox8.Format = "{0:0.00%}";
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3749996423721314D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox8.StyleName = "Normal.TableBody";
            this.textBox8.Value = "=CDbl( CDbl(Sum(Fields.Copies))/Exec(\"DemoTable\",CDbl(Sum(Fields.Copies))))";
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:0.00%}";
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3749996423721314D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox9.StyleName = "Normal.TableBody";
            this.textBox9.Value = "=CDbl( CDbl(Sum(Fields.Copies))/CDbl(Sum(Fields.Copies)))";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.1000001430511475D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.ReportNameTextBox,
            this.textBox18,
            this.textBox17,
            this.textBox15,
            this.textBox12,
            this.textBox11,
            this.textBox10});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // ReportNameTextBox
            // 
            this.ReportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.ReportNameTextBox.Name = "ReportNameTextBox";
            this.ReportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.9055118560791016D), Telerik.Reporting.Drawing.Unit.Inch(1.0999211072921753D));
            this.ReportNameTextBox.Style.Font.Bold = true;
            this.ReportNameTextBox.Style.Font.Name = "Segoe UI";
            this.ReportNameTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.ReportNameTextBox.Value = "= \"Media Summary\"";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1458727121353149D), Telerik.Reporting.Drawing.Unit.Inch(0.69583326578140259D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.7595996856689453D), Telerik.Reporting.Drawing.Unit.Inch(0.19783806800842285D));
            this.textBox18.Value = "=Now()";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.69583326578140259D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1457939147949219D), Telerik.Reporting.Drawing.Unit.Inch(0.19783782958984375D));
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Value = "As of Date: ";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1458727121353149D), Telerik.Reporting.Drawing.Unit.Inch(0.49791660904884338D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.7596783638000488D), Telerik.Reporting.Drawing.Unit.Inch(0.19783806800842285D));
            this.textBox15.Value = "=Parameters.IssueName.Value";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.49791660904884338D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1458333730697632D), Telerik.Reporting.Drawing.Unit.Inch(0.19783806800842285D));
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Value = "Issue:";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1458727121353149D), Telerik.Reporting.Drawing.Unit.Inch(0.29999995231628418D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.7595996856689453D), Telerik.Reporting.Drawing.Unit.Inch(0.19791674613952637D));
            this.textBox11.Value = "= Parameters.ProductName.Value";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.29999995231628418D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1457940340042114D), Telerik.Reporting.Drawing.Unit.Inch(0.19783799350261688D));
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Value = "For Product: ";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.ReportPageNumberTextBox});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // ReportPageNumberTextBox
            // 
            this.ReportPageNumberTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.925196647644043D), Telerik.Reporting.Drawing.Unit.Inch(0.1000000610947609D));
            this.ReportPageNumberTextBox.Name = "ReportPageNumberTextBox";
            this.ReportPageNumberTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5748031139373779D), Telerik.Reporting.Drawing.Unit.Inch(0.39370077848434448D));
            this.ReportPageNumberTextBox.Style.Font.Name = "Segoe UI";
            this.ReportPageNumberTextBox.Value = "Page: {PageNumber}";
            // 
            // SubFields
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detailSection1,
            this.pageHeaderSection1,
            this.pageFooterSection1});
            this.Name = "SubFields";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "Demo";
            reportParameter1.Text = "Demo";
            reportParameter1.Value = "DEMO7";
            reportParameter2.Name = "ProductName";
            reportParameter2.Value = "Test";
            reportParameter3.Name = "IssueName";
            reportParameter3.Value = "";
            reportParameter4.Name = "IssueID";
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter4.Value = "0";
            reportParameter5.Name = "FilterQuery";
            reportParameter5.Value = "";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.ReportParameters.Add(reportParameter5);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Normal.TableNormal")});
            styleRule3.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule3.Style.Color = System.Drawing.Color.Black;
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableBody")});
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule4.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableHeader")});
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule5.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule5.Style.Font.Name = "Tahoma";
            styleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6.5D);
            ((System.ComponentModel.ISupportInitialize)(this.subscriberDetails1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

            }
		#endregion

        private Telerik.Reporting.SqlDataSource Demo;
        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.Table DemoTable;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.TextBox ReportNameTextBox;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox ReportPageNumberTextBox;
        private SubscriberDetails subscriberDetails1;

    }
}