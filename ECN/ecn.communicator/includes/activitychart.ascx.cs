namespace ecn.communicator.includes
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Collections;
    using WebChart;
    using ecn.communicator.classes;
    using ecn.common.classes;

    ///	Summary description for ActivityChart.
    
    public partial class activitychart : System.Web.UI.UserControl
    {
        #region properties
        public string ActionTypeCode = "open";
        public string ChartType = "line";
        public DateTime StartDate = DateTime.Now.AddDays(-30);
        public DateTime EndDate = DateTime.Now;
        public string CustomerID = "1";
        public int BlastID = 0;
        public string TimeType = "daily";
        public Color LineColor = Color.Blue;
        public string LineName = "";
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            CreateChart();
        }

        public void CreateChart()
        {
            Chart ch;
            if (ChartType == "line")
            {
                ch = new SmoothLineChart();
                ch.Line.Color = LineColor;
            }
            else if (ChartType == "bar")
            {
                ch = new StackedColumnChart();
                ch.Line.Color = Color.Transparent;
                ch.Fill.Color = LineColor;
            }
            else
            {
                ch = new SmoothLineChart();
                ch.Line.Color = LineColor;
            }

            if (TimeType == "daily")
            {
                //string sql =
                //    " SELECT CONVERT(varchar(5),ActionDate,1) AS 'Day', Count(ActionDate) AS 'Count' " +
                //    " FROM  EmailActivityLog eal, Blasts b " +
                //    " WHERE  ActionTypeCode='" + ActionTypeCode + "' " +
                //    " AND ActionDate>'" + StartDate.ToString() + "' " +
                //    " AND ActionDate<'" + EndDate.ToString() + "' " +
                //    " AND eal.BlastID=b.BlastID " +
                //    " AND b.CustomerID=" + CustomerID +
                //    " GROUP BY CONVERT(varchar(5),ActionDate,1) " +
                //    " ORDER BY CONVERT(varchar(5),ActionDate,1) ";

                string sql =
                " SELECT CONVERT(varchar(5),baop.OpenTime,1) AS 'Day', Count(baop.OpenTime) AS 'Count' " +
                " FROM ecn_Activity..BlastActivityOpens baop join ecn5_communicator..Blasts b ON baop.BlastID = b.BlastID " +
                " WHERE baop.OpenTime > '" + StartDate.ToString() + "' and baop.OpenTime < ' " + EndDate.ToString() + "' and b.CustomerID = " + CustomerID +
                " GROUP BY CONVERT(varchar(5),baop.OpenTime,1) " +
                " ORDER BY CONVERT(varchar(5),baop.OpenTime,1)";

                ch.DataXValueField = "Day";
                ch.DataYValueField = "Count";
                DataTable dt = DataFunctions.GetDataTable(sql);
                ch.DataSource = dt.DefaultView;
                ch.DataBind();
            }

            if (TimeType == "hourly")
            {
                //string sql =
                //    " SELECT CONVERT(varchar(13),ActionDate,20) AS 'Day', Count(ActionDate) AS 'Count', DatePart(hour,ActionDate) AS 'Hour' " +
                //    " FROM  EmailActivityLog eal, Blasts b " +
                //    " WHERE  ActionTypeCode='" + ActionTypeCode + "' " +
                //    " AND ActionDate>'" + StartDate.ToString() + "' " +
                //    " AND ActionDate<'" + EndDate.ToString() + "' " +
                //    " AND eal.BlastID=b.BlastID " +
                //    " AND b.CustomerID=" + CustomerID +
                //    " GROUP BY CONVERT(varchar(13),ActionDate,20), DatePart(hour,ActionDate) " +
                //    " ORDER BY CONVERT(varchar(13),ActionDate,20) ";

                string sql =
                    " SELECT CONVERT(varchar(13),baop.OpenTime,20) AS 'Day', Count(baop.OpenTime) AS 'Count', DatePart(hour,baop.OpenTime) AS 'Hour' " +
                    " FROM ecn_Activity..BlastActivityOpens baop JOIN ecn5_communicator..Blasts b ON b.BlastID = baop.BlastID " +
                    " WHERE baop.OpenTime > '" + StartDate.ToString() + "' AND baop.OpenTime < '" + EndDate.ToString() + "' AND baop.BlastID = b.BlastID AND b.CustomerID = " + CustomerID +
                    " GROUP BY CONVERT(varchar(13),baop.OpenTime,20), DatePart(hour,baop.OpenTime) " +
                    " ORDER BY CONVERT(varchar(13),baop.OpenTime,20) ";

                ch.DataXValueField = "Hour";
                ch.DataYValueField = "Count";
                DataTable dt = DataFunctions.GetDataTable(sql);
                ch.DataSource = dt.DefaultView;
                ch.DataBind();
            }

            ch.DataLabels.Visible = false;
            ActivityChart.Charts.Add(ch);

            ActivityChart.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            ActivityChart.YTitle.StringFormat.Alignment = StringAlignment.Near;
            ActivityChart.RedrawChart();
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }
}
