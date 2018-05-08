using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections;
using KMPS.MD.Objects;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;

namespace KMPS.MD.Main.Widgets
{
    public partial class BlastComparision : System.Web.UI.UserControl, IWidgetControl
    {
        public ArrayList selectedgroups = new ArrayList();
        public ArrayList selectedReportOn = new ArrayList();
        public ArrayList selectedReportOnNames = new ArrayList();
        List<BlastComparisions> bclist = new List<BlastComparisions>();
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
                loadlist();
                CreateChart();          
            }
            
        }

        public void loadlist()
        {
            try
            {
                if (listboxGroups.Items.Count == 0)
                {
                    listboxGroups.DataSource = Groups.Get(clientconnections);
                    listboxGroups.DataTextField = "GroupName";
                    listboxGroups.DataValueField = "GroupID";
                    listboxGroups.DataBind();
                    listboxGroups.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
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
            return new UpdatePanel[] { UpdatePanelBlastComparision };
                  
        }

        public void InitControl(Kalitte.Dashboard.Framework.WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
        }

        #endregion

        public void DrawChart(object sender, System.EventArgs e)
        {
            CreateChart();
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        protected void reset_chart(Chart chart)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();
        }

        public void CreateChart()
        {
            //Reset Chart
            reset_chart(chtBlastComparision);
            selectedReportOn.Clear();
            selectedReportOnNames.Clear();
            GetGroupData();

            try
            {
                foreach (ListItem item in ReportOn.Items)
                {
                    if (item.Selected)
                    {
                        selectedReportOn.Add(item.Value.ToString());
                        selectedReportOnNames.Add(item.Text.ToString());
                    }
                }

                for (int i = 0; i < selectedReportOn.Count; i++)
                {
                    Series series = new Series(selectedReportOnNames[i].ToString());
                    series.ChartType = SeriesChartType.Line;
                    series.ShadowOffset = 3;
                    chtBlastComparision.Series.Add(series);
                    var query = (from src in bclist
                                 where src.ActionTypeCode == selectedReportOn[i].ToString()
                                 orderby src.BlastID
                                 select new
                                 {
                                     Blast = src.BlastID,
                                     Count = src.Perc
                                 });


                    series.ShadowOffset = 4;
                    series.ToolTip = "#VALY{G}";
                    series.MarkerSize = 10;
                    series.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                    series.BorderWidth = 5;

                    for (int j = 0; j < query.ToList().Count; j++)
                    {
                        series.Points.AddXY(query.ToList()[j].Blast, Math.Round(Convert.ToDouble(query.ToList()[j].Count), 2));
                        series.Label = "#VALY" + "%";
                    }
                }

                //Chart Border Customization
                chtBlastComparision.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                chtBlastComparision.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chtBlastComparision.BorderlineDashStyle = ChartDashStyle.Solid;
                chtBlastComparision.BorderWidth = 8;

                //Adding ChartArea
                chtBlastComparision.ChartAreas.Add("ChartArea1");
                chtBlastComparision.ChartAreas["ChartArea1"].AxisX.Title = "Blasts";
                chtBlastComparision.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                chtBlastComparision.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                chtBlastComparision.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtBlastComparision.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtBlastComparision.ChartAreas["ChartArea1"].AxisX.LineColor = System.Drawing.Color.LightGray;
                chtBlastComparision.ChartAreas["ChartArea1"].AxisY.LineColor = System.Drawing.Color.LightGray;
                chtBlastComparision.ChartAreas["ChartArea1"].AxisX.Interval = 1;

                chtBlastComparision.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.Transparent;
                chtBlastComparision.ChartAreas["ChartArea1"].ShadowColor = System.Drawing.Color.Transparent;

                //Image Size
                if (Request.Cookies["screenInfo"] != null)
                {
                    chtBlastComparision.Height = Convert.ToInt32((Int32.Parse(Request.Cookies["screenInfo"]["ScreenHeight"].ToString()) / 2.8));
                    chtBlastComparision.Width = Convert.ToInt32((Int32.Parse(Request.Cookies["screenInfo"]["ScreenWidth"].ToString()) / 3.8));
                }
                else
                {
                    chtBlastComparision.Height = 350;
                    chtBlastComparision.Width = 450;
                }
                //chtBlastComparision.Height = Convert.ToInt32((Int32.Parse(Request.QueryString["_height"].ToString()) / 2.8));
                //chtBlastComparision.Width = Convert.ToInt32((Int32.Parse(Request.QueryString["_width"].ToString()) / 3.8)); 


                //Adding Title
                chtBlastComparision.Titles.Add("Blast Comparision");

                //Adding legend
                chtBlastComparision.Legends.Add("Legends1");
                chtBlastComparision.Legends[0].Enabled = true;
                chtBlastComparision.Legends[0].Docking = Docking.Bottom;
                chtBlastComparision.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chtBlastComparision.Legends[0].IsEquallySpacedItems = true;
                chtBlastComparision.Legends[0].TextWrapThreshold = 0;
                chtBlastComparision.Legends[0].IsTextAutoFit = true;

                chtBlastComparision.Legends[0].BackColor = System.Drawing.Color.Transparent;
                chtBlastComparision.Legends[0].ShadowColor = System.Drawing.Color.Transparent;


            }
            catch (Exception ex)
            {
                Label1.Text =ex.Message;
            }
        }

        public void GetGroupIDs()
        {
            if (listboxGroups.Items.Count==0)
            {
                listboxGroups.Items.Clear();
                SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnections);
                SqlCommand cmd = new SqlCommand("SELECT [GroupID] FROM [Pubs] " +
                                                "where [GroupID] is not null", conn);
                 StringBuilder sb = new StringBuilder();
                cmd.CommandTimeout = 0;
                try
                {
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();                   
                    DataTable dt = new DataTable();
                    dt.Load(rdr);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append(dt.Rows[i][0].ToString());
                        if(i!=dt.Rows.Count-1)
                            sb.Append(",");
                    }

                }
                catch (Exception ex)
                {
                    Label1.Text = ex.Message;
                }
                finally
                {
                    conn.Close();
                }

                GetGroupNames(sb.ToString());
            }
        }

        public void GetGroupNames(string groupids)
        {
            listboxGroups.Items.Clear();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand(" SELECT [GroupID], [GroupName] FROM [Groups] " +
                                            " where [GroupID] IN (" + groupids + ")", conn);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();               
                while (rdr.Read())
                {
                    ListItem item = new ListItem(rdr["GroupName"].ToString(), rdr["GroupID"].ToString());
                    listboxGroups.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            if (listboxGroups.Items.Count > 0)
            {
                listboxGroups.SelectedIndex = 0;
            }
            else
            {
                Label1.Text = "No Data";
            }

        }

        public void GetGroupData()
        {
            selectedgroups.Clear();
            foreach (ListItem item in listboxGroups.Items)
            {
                if (item.Selected)
                {
                    selectedgroups.Add(item.Value.ToString());
                }
            }

            string sqlparam = "";
            for (int i = 0; i < selectedgroups.Count; i++)
            {
                sqlparam += "<Groups GroupID=\"" + selectedgroups[i] + "\"/>";
            }
            bclist = BlastComparisions.GetData(sqlparam,Int32.Parse(dropdownBlastsNum.SelectedValue));    
        }       

    }
}