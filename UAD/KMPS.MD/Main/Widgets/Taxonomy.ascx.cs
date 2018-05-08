using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;

namespace KMPS.MD.Main.Widgets
{
    public partial class Taxonomy : System.Web.UI.UserControl, IWidgetControl
    {
        
        DashboardSurface surface;

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateChart();
            }
        }

        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        public UpdatePanel[] Command(WidgetInstance instance, Kalitte.Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            if (commandData.CommandName.Equals("Settings"))
            {
                if (PnlChart.Visible)
                {
                    PnlSettings.Visible = true;
                    PnlChart.Visible = false;
                }
            }
            return new UpdatePanel[] { UpdatePanelTaxonomy };
        }

        public void InitControl(Kalitte.Dashboard.Framework.WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
        }

        #endregion

        protected void reset_chart(Chart chart)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();
        }


        private void CreateChart()
        {

            List<Taxonomys> txlist = Taxonomys.Get(clientconnections);

            //Reset Chart
            reset_chart(chtTaxonomy);

            try
            {
                int filter = Int32.Parse(dropdownMonths.SelectedValue)-1;
                DateTime date_filter = DateTime.Now.AddMonths(Int32.Parse("-" + filter.ToString()));
                int month_filter = date_filter.Month;
                int year_filter = date_filter.Year;
                DateTime date_filter_month = new DateTime(year_filter, month_filter, 1);

                var months = (from src in txlist
                               where src.MonthFirstDate>= new DateTime(year_filter, month_filter, 1)
                               orderby src.yeardt, src.monthdt
                                    select new
                                    {
                                        src.MonthFirstDate,
                                        src.Month
                                        
                                    }).Distinct().Reverse();

                 for (int i = 0; i < months.ToList().Count; i++)
                 {
                     Series series = new Series(months.ToList()[i].Month);
                     series.ChartType = SeriesChartType.Column;
                     series.ShadowOffset = 3;
                     chtTaxonomy.Series.Add(series);
                     if (i == 0)
                     {
                         var querytopics = (from src in txlist
                                            where src.MonthFirstDate == months.ToList()[i].MonthFirstDate
                                            //orderby src.subscriptions
                                            select new
                                            {
                                                Topic = src.masterdesc,
                                                Opens = src.subscriptions
                                            });
                         for (int j = 0; j < querytopics.ToList().Count; j++)
                         {
                             series.Points.AddXY(querytopics.ToList()[j].Topic, Convert.ToDouble(querytopics.ToList()[j].Opens));
                         }
                     }
                     else
                     {
                         var querytopics = (from src in txlist
                                            where src.MonthFirstDate == months.ToList()[i].MonthFirstDate
                                            select new
                                            {
                                                Topic = src.masterdesc,
                                                Opens = src.subscriptions
                                            });
                         for (int j = 0; j < querytopics.ToList().Count; j++)
                         {
                             series.Points.AddXY(querytopics.ToList()[j].Topic, Convert.ToDouble(querytopics.ToList()[j].Opens));
                         }
                     }                     
                 }
                              
                //Chart Border Customization
                chtTaxonomy.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                chtTaxonomy.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chtTaxonomy.BorderlineDashStyle = ChartDashStyle.Solid;
                chtTaxonomy.BorderWidth = 8;            

                //Adding ChartArea
                chtTaxonomy.ChartAreas.Add("ChartArea1");
                chtTaxonomy.ChartAreas["ChartArea1"].AxisX.Title = "Topic";
                chtTaxonomy.ChartAreas["ChartArea1"].AxisY.Title = "Clicks";
                chtTaxonomy.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                chtTaxonomy.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                chtTaxonomy.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                chtTaxonomy.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtTaxonomy.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;

                //Image Size
                if (Request.Cookies["screenInfo"] != null)
                {
                    chtTaxonomy.Height = Convert.ToInt32((Int32.Parse(Request.Cookies["screenInfo"]["ScreenHeight"].ToString()) / 2.8));
                    chtTaxonomy.Width = Convert.ToInt32((Int32.Parse(Request.Cookies["screenInfo"]["ScreenWidth"].ToString()) / 3.8));
                }
                else
                {
                    chtTaxonomy.Height = 350;
                    chtTaxonomy.Width = 450;
                }
               // chtTaxonomy.Height = Convert.ToInt32((Int32.Parse(Request.QueryString["_height"].ToString()) / 2.8));
               // chtTaxonomy.Width = Convert.ToInt32((Int32.Parse(Request.QueryString["_width"].ToString()) / 3.8)); 


                //Adding Title
                chtTaxonomy.Titles.Add("Title1");
                chtTaxonomy.Titles["Title1"].Text = "Popular Topics this month";

                //Adding legend
                chtTaxonomy.Legends.Add("Legends1");
                chtTaxonomy.Legends[0].Enabled = true;
                chtTaxonomy.Legends[0].Docking = Docking.Bottom;
                chtTaxonomy.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chtTaxonomy.Legends[0].IsEquallySpacedItems = true;
                chtTaxonomy.Legends[0].TextWrapThreshold = 0;
                chtTaxonomy.Legends[0].IsTextAutoFit = true;


          
                //chtTaxonomy.DataBind();
            }                
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
         
         
        }

        protected void lbtnApplyFilters_Click(object sender, EventArgs e)
        {
            CreateChart();
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }
    }
}