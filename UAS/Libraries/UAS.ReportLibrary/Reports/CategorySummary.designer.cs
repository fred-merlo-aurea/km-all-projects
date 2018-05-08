using System.Configuration;

namespace UAS.ReportLibrary.Reports
{
    partial class CategorySummary
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
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup7 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup8 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup9 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup10 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup11 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.CatSummary = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.ReportNameTextBox = new Telerik.Reporting.TextBox();
            this.textBox29 = new Telerik.Reporting.TextBox();
            this.textBox28 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.textBox26 = new Telerik.Reporting.TextBox();
            this.textBox25 = new Telerik.Reporting.TextBox();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.table1 = new Telerik.Reporting.Table();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.ReportPageNumberTextBox = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // textBox3
            // 
            this.textBox3.CanShrink = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4479166269302368D), Telerik.Reporting.Drawing.Unit.Inch(0.26041650772094727D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.StyleName = "";
            this.textBox3.Value = "= Fields.Demo7";
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.6250002384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.26041650772094727D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.StyleName = "";
            this.textBox5.Value = "Total Copies";
            // 
            // textBox16
            // 
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.26041650772094727D));
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.StyleName = "";
            this.textBox16.Value = "Total Records";
            // 
            // textBox11
            // 
            this.textBox11.CanShrink = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5312504768371582D), Telerik.Reporting.Drawing.Unit.Inch(0.24999983608722687D));
            this.textBox11.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox11.StyleName = "";
            this.textBox11.Value = "= Fields.Category";
            // 
            // textBox1
            // 
            this.textBox1.CanShrink = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5312504768371582D), Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D));
            this.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.StyleName = "";
            this.textBox1.Value = " ";
            // 
            // textBox9
            // 
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.48958322405815125D));
            this.textBox9.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.StyleName = "";
            this.textBox9.Value = "= Fields.CategoryType";
            // 
            // textBox13
            // 
            this.textBox13.CanShrink = true;
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5312504768371582D), Telerik.Reporting.Drawing.Unit.Inch(0.333333283662796D));
            this.textBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox13.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox13.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.StyleName = "";
            this.textBox13.Value = " ";
            // 
            // textBox8
            // 
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.0000004768371582D), Telerik.Reporting.Drawing.Unit.Inch(0.333333283662796D));
            this.textBox8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.StyleName = "";
            this.textBox8.Value = "Grand Total";
            // 
            // CatSummary
            // 
            this.CatSummary.ConnectionString = "Data Source=IT-LF\\LOCALGT;Initial Catalog=MTGMasterDB;User ID=sa;Password=sa";
            this.CatSummary.Name = "CatSummary";
            this.CatSummary.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@IssueID", System.Data.DbType.Int32, "= Parameters.IssueID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@Queries", System.Data.DbType.AnsiString, "= Parameters.FilterQuery.Value")});
            this.CatSummary.ProviderName = "System.Data.SqlClient";
            this.CatSummary.SelectCommand = "dbo.rpt_CategorySummaryMVC";
            this.CatSummary.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.ReportNameTextBox,
            this.textBox29,
            this.textBox28,
            this.textBox27,
            this.textBox26,
            this.textBox25,
            this.textBox24});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // ReportNameTextBox
            // 
            this.ReportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.ReportNameTextBox.Name = "ReportNameTextBox";
            this.ReportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4999606609344482D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D));
            this.ReportNameTextBox.Style.Font.Bold = true;
            this.ReportNameTextBox.Style.Font.Name = "Segoe UI";
            this.ReportNameTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.ReportNameTextBox.Value = "Category Summary";
            // 
            // textBox29
            // 
            this.textBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.29170608520507812D));
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1457940340042114D), Telerik.Reporting.Drawing.Unit.Inch(0.19992129504680634D));
            this.textBox29.Style.Font.Bold = true;
            this.textBox29.Value = "For Product: ";
            // 
            // textBox28
            // 
            this.textBox28.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1458727121353149D), Telerik.Reporting.Drawing.Unit.Inch(0.29170608520507812D));
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.3541269302368164D), Telerik.Reporting.Drawing.Unit.Inch(0.20000004768371582D));
            this.textBox28.Value = "= Parameters.ProductName.Value";
            // 
            // textBox27
            // 
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.50003939867019653D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1458333730697632D), Telerik.Reporting.Drawing.Unit.Inch(0.19992136955261231D));
            this.textBox27.Style.Font.Bold = true;
            this.textBox27.Value = "Issue:";
            // 
            // textBox26
            // 
            this.textBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1458727121353149D), Telerik.Reporting.Drawing.Unit.Inch(0.50003939867019653D));
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.3541269302368164D), Telerik.Reporting.Drawing.Unit.Inch(0.19992136955261231D));
            this.textBox26.Value = "=Parameters.IssueName.Value";
            // 
            // textBox25
            // 
            this.textBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.69795608520507812D));
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1457939147949219D), Telerik.Reporting.Drawing.Unit.Inch(0.1999211311340332D));
            this.textBox25.Style.Font.Bold = true;
            this.textBox25.Value = "As of Date: ";
            // 
            // textBox24
            // 
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1458727121353149D), Telerik.Reporting.Drawing.Unit.Inch(0.69795608520507812D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.3541274070739746D), Telerik.Reporting.Drawing.Unit.Inch(0.19992136955261231D));
            this.textBox24.Value = "=Now()";
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.9499996900558472D);
            this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.table1});
            this.detailSection1.Name = "detailSection1";
            // 
            // table1
            // 
            this.table1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.4479169845581055D)));
            this.table1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.62500035762786865D)));
            this.table1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1D)));
            this.table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24999983608722687D)));
            this.table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D)));
            this.table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.33333322405815125D)));
            this.table1.Body.SetCellContent(0, 0, this.textBox4);
            this.table1.Body.SetCellContent(0, 1, this.textBox6);
            this.table1.Body.SetCellContent(1, 0, this.textBox2);
            this.table1.Body.SetCellContent(1, 1, this.textBox7);
            this.table1.Body.SetCellContent(2, 0, this.textBox14);
            this.table1.Body.SetCellContent(2, 1, this.textBox15);
            this.table1.Body.SetCellContent(0, 2, this.textBox17);
            this.table1.Body.SetCellContent(1, 2, this.textBox18);
            this.table1.Body.SetCellContent(2, 2, this.textBox19);
            tableGroup1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.Demo7"));
            tableGroup1.Name = "Demo7";
            tableGroup1.ReportItem = this.textBox3;
            tableGroup1.Sortings.Add(new Telerik.Reporting.Sorting("= Fields.Demo7", Telerik.Reporting.SortDirection.Desc));
            tableGroup2.Name = "group";
            tableGroup2.ReportItem = this.textBox5;
            tableGroup3.Name = "group6";
            tableGroup3.ReportItem = this.textBox16;
            this.table1.ColumnGroups.Add(tableGroup1);
            this.table1.ColumnGroups.Add(tableGroup2);
            this.table1.ColumnGroups.Add(tableGroup3);
            this.table1.Corner.SetCellContent(0, 0, this.textBox10);
            this.table1.Corner.SetCellContent(0, 1, this.textBox12);
            this.table1.DataSource = null;
            this.table1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox10,
            this.textBox12,
            this.textBox4,
            this.textBox6,
            this.textBox17,
            this.textBox2,
            this.textBox7,
            this.textBox18,
            this.textBox14,
            this.textBox15,
            this.textBox19,
            this.textBox3,
            this.textBox5,
            this.textBox16,
            this.textBox9,
            this.textBox11,
            this.textBox1,
            this.textBox8,
            this.textBox13});
            this.table1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.table1.Name = "table1";
            tableGroup6.Name = "detailTableGroup";
            tableGroup5.ChildGroups.Add(tableGroup6);
            tableGroup5.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.Category"));
            tableGroup5.Name = "Category";
            tableGroup5.ReportItem = this.textBox11;
            tableGroup5.Sortings.Add(new Telerik.Reporting.Sorting("= Fields.CodeValue", Telerik.Reporting.SortDirection.Asc));
            tableGroup8.Name = "group2";
            tableGroup7.ChildGroups.Add(tableGroup8);
            tableGroup7.Name = "group1";
            tableGroup7.ReportItem = this.textBox1;
            tableGroup4.ChildGroups.Add(tableGroup5);
            tableGroup4.ChildGroups.Add(tableGroup7);
            tableGroup4.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.CategoryType"));
            tableGroup4.Name = "CategoryType";
            tableGroup4.ReportItem = this.textBox9;
            tableGroup4.Sortings.Add(new Telerik.Reporting.Sorting("= Fields.Order", Telerik.Reporting.SortDirection.Asc));
            tableGroup11.Name = "group5";
            tableGroup10.ChildGroups.Add(tableGroup11);
            tableGroup10.Name = "group4";
            tableGroup10.ReportItem = this.textBox13;
            tableGroup9.ChildGroups.Add(tableGroup10);
            tableGroup9.Name = "group3";
            tableGroup9.ReportItem = this.textBox8;
            this.table1.RowGroups.Add(tableGroup4);
            this.table1.RowGroups.Add(tableGroup9);
            this.table1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6041679382324219D), Telerik.Reporting.Drawing.Unit.Inch(1.083332896232605D));
            // 
            // textBox4
            // 
            this.textBox4.CanShrink = true;
            this.textBox4.Format = "{0:N0}";
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4479166269302368D), Telerik.Reporting.Drawing.Unit.Inch(0.24999983608722687D));
            this.textBox4.StyleName = "";
            this.textBox4.Value = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
            // 
            // textBox6
            // 
            this.textBox6.Format = "{0:N0}";
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.6250002384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.24999982118606567D));
            this.textBox6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.StyleName = "";
            this.textBox6.Value = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
            // 
            // textBox2
            // 
            this.textBox2.CanShrink = true;
            this.textBox2.Format = "{0:N0}";
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4479166269302368D), Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D));
            this.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.StyleName = "";
            this.textBox2.Value = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
            // 
            // textBox7
            // 
            this.textBox7.Format = "{0:N0}";
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.6250002384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.23958331346511841D));
            this.textBox7.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox7.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox7.StyleName = "";
            this.textBox7.Value = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
            // 
            // textBox14
            // 
            this.textBox14.CanShrink = true;
            this.textBox14.Format = "{0:N0}";
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4479169845581055D), Telerik.Reporting.Drawing.Unit.Inch(0.333333283662796D));
            this.textBox14.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.Font.Bold = true;
            this.textBox14.StyleName = "";
            this.textBox14.Value = "= SUM(Fields.Copies)";
            // 
            // textBox15
            // 
            this.textBox15.Format = "{0:N0}";
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.62500029802322388D), Telerik.Reporting.Drawing.Unit.Inch(0.333333283662796D));
            this.textBox15.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox15.Style.Font.Bold = true;
            this.textBox15.StyleName = "";
            this.textBox15.Value = "= SUM(Fields.Copies)";
            // 
            // textBox17
            // 
            this.textBox17.Format = "{0:N0}";
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.24999983608722687D));
            this.textBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.StyleName = "";
            this.textBox17.Value = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.RecordCount), 0)";
            // 
            // textBox18
            // 
            this.textBox18.Format = "{0:N0}";
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D));
            this.textBox18.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.StyleName = "";
            this.textBox18.Value = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.RecordCount), 0)";
            // 
            // textBox19
            // 
            this.textBox19.Format = "{0:N0}";
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.333333283662796D));
            this.textBox19.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.StyleName = "";
            this.textBox19.Value = "= Sum(Fields.RecordCount)";
            // 
            // textBox10
            // 
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.26041650772094727D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.StyleName = "";
            this.textBox10.Value = "Category Type";
            // 
            // textBox12
            // 
            this.textBox12.CanShrink = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5312504768371582D), Telerik.Reporting.Drawing.Unit.Inch(0.26041644811630249D));
            this.textBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox12.StyleName = "";
            this.textBox12.Value = "Category";
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
            this.ReportPageNumberTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.725196361541748D), Telerik.Reporting.Drawing.Unit.Inch(0.10625997930765152D));
            this.ReportPageNumberTextBox.Name = "ReportPageNumberTextBox";
            this.ReportPageNumberTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2748038768768311D), Telerik.Reporting.Drawing.Unit.Inch(0.39370077848434448D));
            this.ReportPageNumberTextBox.Style.Font.Name = "Segoe UI";
            this.ReportPageNumberTextBox.Value = "Page: {PageNumber}";
            // 
            // CategorySummary
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1});
            this.Name = "CategorySummary";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.5D), Telerik.Reporting.Drawing.Unit.Inch(15D));
            reportParameter1.Name = "ProductName";
            reportParameter1.Value = "Product Name test";
            reportParameter2.Name = "IssueName";
            reportParameter2.Value = "Issue Name";
            reportParameter3.Name = "IssueID";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter3.Value = "0";
            reportParameter4.Name = "FilterQuery";
            reportParameter4.Value = "";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
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
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.0000391006469727D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource CatSummary;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.TextBox ReportNameTextBox;
        private Telerik.Reporting.TextBox textBox29;
        private Telerik.Reporting.TextBox textBox28;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox26;
        private Telerik.Reporting.TextBox textBox25;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.Table table1;
        private Telerik.Reporting.TextBox textBox4;
        //private SubscriberDetails subscriberDetails1;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox ReportPageNumberTextBox;
    }
}