using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.Web.UI.DataVisualization.Charting;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;


namespace KMPS.MD.Main.Widgets
{
    public partial class AudienceEngagement : System.Web.UI.UserControl, IWidgetControl
    {
        DashboardSurface surface;
        List<AudienceEngagementReport> advertiserEngagement = new List<AudienceEngagementReport>();
        List<Groups> grplist = new List<Groups>();

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
            
            //KMPS.MD.MasterPages.Site masterPage = (KMPS.MD.MasterPages.Site)this.Page.Master;           
            //masterPage.Menu
        }
        public void loadlist()
        {
            try
            {
                if (drpGroup.Items.Count == 0)
                {
                    drpGroup.DataTextField = "GroupName";
                    drpGroup.DataValueField = "GroupID";
                    drpGroup.DataSource = Groups.Get(clientconnections);
                    drpGroup.DataBind();
                    drpGroup.SelectedIndex = 0;
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
            return new UpdatePanel[] { UpdatePanelAudienceEngagement };           
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

        public void CreateChart()
        {
            
            reset_chart(chtAudienceEngagement);
            advertiserEngagement = AudienceEngagementReport.Get(Convert.ToInt32(drpGroup.SelectedValue), Convert.ToInt32(txtClickPercent.Text), Convert.ToInt32(txtDays.Text), "N", "");
            string title = drpGroup.SelectedItem.Text + " Clicks Perc:" + txtClickPercent.Text + " Days:" + txtDays.Text;
            
            try
            {

                chtAudienceEngagement.DataSource = advertiserEngagement.OrderBy(x => x.Counts).Reverse().ToList();
               
                chtAudienceEngagement.Series.Add("Engagement");

                chtAudienceEngagement.Series["Engagement"].XValueMember = "SubscriberType";


                chtAudienceEngagement.Series["Engagement"].YValueMembers = "Counts";
                chtAudienceEngagement.Series["Engagement"].IsValueShownAsLabel = true;
                chtAudienceEngagement.Series["Engagement"].IsVisibleInLegend = true;
                chtAudienceEngagement.Series["Engagement"].ChartType = SeriesChartType.Funnel;
                chtAudienceEngagement.Series["Engagement"].ToolTip = "#VALX";
                chtAudienceEngagement.Series["Engagement"]["FunnelPointGap"] = "3";

                

                //Chart Border Customization
                chtAudienceEngagement.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                chtAudienceEngagement.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chtAudienceEngagement.BorderlineDashStyle = ChartDashStyle.Solid;
                chtAudienceEngagement.BorderWidth = 8;

                //Adding ChartArea
                chtAudienceEngagement.ChartAreas.Add("ChartArea1");
                chtAudienceEngagement.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                chtAudienceEngagement.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                chtAudienceEngagement.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtAudienceEngagement.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtAudienceEngagement.ChartAreas["ChartArea1"].AxisX.LineColor = System.Drawing.Color.LightGray;
                chtAudienceEngagement.ChartAreas["ChartArea1"].AxisY.LineColor = System.Drawing.Color.LightGray;
                chtAudienceEngagement.ChartAreas["ChartArea1"].BackSecondaryColor=System.Drawing.Color.White ;
                chtAudienceEngagement.ChartAreas["ChartArea1"].BackColor=System.Drawing.Color.Transparent ;
                chtAudienceEngagement.ChartAreas["ChartArea1"].ShadowColor =System.Drawing.Color.Transparent;
                chtAudienceEngagement.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                chtAudienceEngagement.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = false;

	            // Set 3D angle
                //chtAudienceEngagement.Series["Engagement"]["Funnel3DRotationAngle"] = "7";

	            // Set 3D drawing style
               // chtAudienceEngagement.Series["Engagement"]["Funnel3DDrawingStyle"] = "CircularBase";

                //Image Size   
                if (Request.Cookies["screenInfo"] != null)
                {
                    chtAudienceEngagement.Height = Convert.ToInt32((Int32.Parse(Request.Cookies["screenInfo"]["ScreenHeight"].ToString()) / 2.8));
                    chtAudienceEngagement.Width = Convert.ToInt32((Int32.Parse(Request.Cookies["screenInfo"]["ScreenWidth"].ToString()) / 3.8));
                }
                else
                {
                    chtAudienceEngagement.Height = 350;
                    chtAudienceEngagement.Width = 450;              
                }

                //chtAudienceEngagement.Height = Convert.ToInt32((Int32.Parse(Request.QueryString["_height"].ToString()) / 2.8));
                //chtAudienceEngagement.Width = Convert.ToInt32((Int32.Parse(Request.QueryString["_width"].ToString()) / 3.8)); 

                //Adding Title
                chtAudienceEngagement.Titles.Add("Audience Engagement");
                chtAudienceEngagement.Titles.Add(title);

                //Adding legend
                chtAudienceEngagement.Legends.Add("Legends1");
                chtAudienceEngagement.Legends[0].Enabled = true;
                chtAudienceEngagement.Legends[0].Docking = Docking.Bottom;
                chtAudienceEngagement.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chtAudienceEngagement.Legends[0].IsEquallySpacedItems = true;
                chtAudienceEngagement.Legends[0].TextWrapThreshold = 0;
                chtAudienceEngagement.Legends[0].IsTextAutoFit = true;
                chtAudienceEngagement.Legends[0].BackColor = System.Drawing.Color.Transparent;
                chtAudienceEngagement.Legends[0].ShadowColor = System.Drawing.Color.Transparent;
                chtAudienceEngagement.DataBind();

            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            CreateChart();
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        public void GetGroupIDs()
        {
            if (drpGroup.Items.Count == 0)
            {
                drpGroup.Items.Clear();

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
                        if (i != dt.Rows.Count - 1)
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
            drpGroup.Items.Clear();
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
                    drpGroup.Items.Add(item);
                }

                //drpGroup.Items.Insert(0, new ListItem("Please Select a Group", "0"));
                //drpGroup.Items.FindByValue("0").Selected = true;
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }


       

    }
}