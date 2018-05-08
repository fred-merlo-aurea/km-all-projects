using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;
using AjaxControlToolkit;
using System.Linq;

namespace ecn.communicator.main.blasts
{
    public class SearchFilter
    {
        public string Subject
        {
            get;
            set;
        }

        public string GroupName   
        {
            get;
            set;
        }

        public string User
        {
            get;
            set;
        }

        public string Campaign
        {
            get;
            set;
        }

        public string blastType
        {
            get;
            set;
        }
    }
    
    public partial class BlastCalendarView : ECN_Framework.WebPageHelper
    {
        private readonly string xslDaily = "~/xsl/DailyBlast.xsl";   
        private readonly string xslWeekly = "~/xsl/Weekly.xsl";
        private DataTable dtBlastData = new DataTable();
        private DateTime currReportDay; 

        public DateTime CalendarStartDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(Request.QueryString["bdt"].ToString());   
                }
                catch
                {
                    return DateTime.Now;    
                }
            }
        }
        
        public string CalendarType
        {
            get
            {
                try
                {
                    return Request.QueryString["ct"].ToString();   
                }
                catch
                {
                    return "monthly"; 
                }
            }
        }
       
        public DateTime CalendarCurrentDateDaily
        {
            get
            {
                try
                {
                    return (DateTime)ViewState["CalendarCurrentDateDaily"];
                }
                catch
                {
                    return DateTime.Now;  
                }

            }
            set
            {
                ViewState["CalendarCurrentDateDaily"] = value;      
            }
        }
      
        public DateTime CalendarCurrentDateMonthly
        {
            get
            {
                try
                {
                    return (DateTime)ViewState["CalendarCurrentDateMonthly"];    
                }
                catch
                {
                    return Convert.ToDateTime(DateTime.Today.Month.ToString() + "/01/" + DateTime.Now.Year.ToString());
                }

            }
            set
            {
                ViewState["CalendarCurrentDateMonthly"] = value;    
            }
        }
     
        public DateTime CalendarCurrentDateWeekly
        {
            get
            {
                try
                {
                    return (DateTime)ViewState["CalendarCurrentDateWeekly"];
                }
                catch
                {
                    return CalculateWeekDays(DateTime.Now.ToShortDateString()); 
                }

            }
            set
            {
                ViewState["CalendarCurrentDateWeekly"] = value;
            } 
        } 
      
        public string ReportType
        {
            get
            {
                try
                {
                    return rbBlastCalType.SelectedItem.Value.ToUpper();                         
                }
                catch
                {
                    return "SUM";  
                }
            } 
        }            

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "blast status";
            Master.Heading = "View Blast Calendar";
            Master.HelpContent = "<b onclick=return toggleMe('par1')>Testing your Campaign</b><br/><div id='par1' style='display:none'><ul><li>Use the Message dropdown list to select the Message you want to send.</li><li>Use the Groups dropdown list to select the group of email addresses you want to send to.</li><li>Use the Filter dropdown list to select a filter, if applicable.</li><li>Type your email address in the From Email field. When you press enter, your email address will automatically appear in the Reply To field.</li><li>In the From Name field, enter your business name.</li><li>In the Subject field, enter a subject phrase.<br/><em class='note'>Note: The info you enter here will appear on the subject line of the email received by the person you are sending to. Also, be careful not to use words in the Subject that might trigger a spam response. Examples might include: Sale, Free, Viagra, Marketing, Specials, Mortgage, etc.</em></li>&#13;&#10;<li>Click the <em>Test Blast</em> box (limit 10 recipients).</li><li>Click <em>Get SPAM Score</em> to view results of how your email message scores against the Spam filters and the ability for your email to successfully deliver.  Scores are based on a 5 point scale.  A score under 3 is Optimal.  If the score is over 5, please review and edit your content to lower your Spam score and increase your deliverability success rate.</li> &#13;&#10;<li>Click <em>Blast Now</em>.<br/><em class='note'>Note: The ECN system will not allow the same message to be sent to the same individual within 7 days; thus, the Test Blast feature allows you to send out the same email message to the same recipient as often as needed to ensure your message will appear the way you intended.</em></li></ul>&#13;&#10;</div><b>Setting up a 1-time Blast</b><br/><div id='par2' style='display:none'><ul><li>Use the Message dropdown list to select the Message you want to send.</li><li>Use the Groups dropdown list to select the group of email addresses you want to send to.</li>&#13;&#10;<li>Use the Filter dropdown list to select a filter, if applicable.</li><li>Type your email address in the From Email field.<br/>When you press enter, your email address will automatically appear in the Reply To field.</li><li>In the From Name field, enter your business name.</li>&#13;&#10;<li>In the Subject field, enter a subject phrase.<br/><em class='note'>Note: The info you enter here will appear on the subject line of the email received by the person you are sending to. Also, be careful not to use words in the Subject that might trigger a spam response. Examples might include: Sale, Free, Viagra, Marketing, Specials, Mortgage, etc.</em></li>&#13;&#10;<li>Click <em>Blast Now</em>.</li></ul></div><b>Setting up a scheduled Blast</b><br/><div id='par3' style='display:none'><ul><li>Use the Message dropdown list to select the Message you want to send.</li><li>Use the Groups dropdown list to select the group of email addresses you want to send to.</li><li>Use the Filter dropdown list to select a filter, if applicable.</li>&#13;&#10;<li>Type your email address in the From Email field. When you press enter, your email address will automatically appear in the Reply To field.</li><li>In the From Name field, enter your business name.</li><li>In the Subject field, enter a subject phrase.<br/><em class='note'>Note: The info you enter here will appear on the subject line of the email received by the person you are sending to. Also, be careful not to use words in the Subject that might trigger a spam response. Examples might include: Sale, Free, Viagra, Marketing, Specials, Mortgage, etc.</em></li>&#13;&#10;<li>In the Schedule Type field, use the drop down list to select whether you want to send the blast once (on a specific date), daily, weekly, or monthly.</li><li>For sending once on a specific date, the screen will refresh and you can select the date and time.</li><li>For Daily blasts of the same message, you can choose the time of day.</li><li>For Weekly scheduled blasts, you can select the time and day of the week.</li><li>For Monthly scheduled blasts, you can select the time and which date of the month.</li>&#13;&#10;<li>Click <em>Schedule Blast.</em></li>";
            Master.HelpTitle = "Blast Manager";	

            if (IsPostBack)
            {
                string script = " $(function() { $('#TabContainer1_TabPanel3_cal a[title]').tooltip({ position: 'center right',offset: [-2, 10], effect: 'fade', opacity: 0.7});});";
                script += "      $(function() { $('#TabContainer1_TabPanel2 a[title]').tooltip({ position: 'center right',offset: [-2, 10], effect: 'fade', opacity: 0.7});});";
                script += "      $(function() { $('#TabContainer1_TabPanel1 a[title]').tooltip({ position: 'center right',offset: [-2, 10], effect: 'fade', opacity: 0.7});});";                  
                ScriptManager.RegisterStartupScript(this, this.GetType(), "tooltip", script, true);

                SearchFilter filter = new SearchFilter();
                filter.User = SentUserID.SelectedItem.Value;
                filter.Subject = txtSubjectSearch.Text.ToString();    
                filter.GroupName = txtGroupSearch.Text.ToString();                               
                filter.blastType = BlastTypeDD.SelectedItem.Value;                
                if((drpCampaigns.Items !=null) && (drpCampaigns.Items.Count>0))
                    filter.Campaign = drpCampaigns.SelectedItem.Value; 
                Session["filter"] = filter;     
            }
             
            if (!IsPostBack)
            {
                loadCampaignsDR(Master.UserSession.CurrentUser.CustomerID);
                LoadUserDD();

                rbBlastCalType.ClearSelection();
                rbBlastCalType.Items.FindByValue("sum").Selected = true;

                CalendarCurrentDateMonthly = Convert.ToDateTime(DateTime.Today.Month.ToString() + "/01/" + DateTime.Now.Year.ToString());
                LoadFilterFromSession(); 

                if (CalendarType.ToUpper() == "DAILY")
                {
                    CalendarCurrentDateDaily = CalendarStartDate; 
                    txtDailyCalendar.Text = CalendarCurrentDateDaily.ToShortDateString();
                    DailyCalendar(); 
                }
                else if (CalendarType.ToUpper() == "WEEKLY")
                {
                    CalendarCurrentDateWeekly = CalendarStartDate;                    
                    WeeklyCalendar(CalendarCurrentDateWeekly.ToShortDateString());     
                }
                else
                {
                    CalendarCurrentDateMonthly = Convert.ToDateTime(DateTime.Today.Month.ToString() + "/01/" + DateTime.Now.Year.ToString());  
                    MonthCalender.VisibleDate = CalendarCurrentDateMonthly;     
                    BindMonthlyCalendar();
                }
            }              
        }

        private void LoadUserDD()
        {
            List<KMPlatform.Entity.User> userList = KMPlatform.BusinessLogic.User.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID);
            if (userList.Count(x => x.UserID == Master.UserSession.CurrentUser.UserID) == 0)
                userList.Add(Master.UserSession.CurrentUser);
            SentUserID.DataSource = userList.OrderBy(x => x.UserName);
            SentUserID.DataValueField = "UserID";
            SentUserID.DataTextField = "UserName";
            SentUserID.DataBind();
            SentUserID.Items.Insert(0, new ListItem("All", "-1"));
            SentUserID.Items.FindByValue(Master.UserSession.CurrentUser.UserID.ToString()).Selected = true;       
            if (SentUserID.Items.Count == 1)
            {
                SentUserID.Visible = false;
            }
        }

        private void LoadFilterFromSession()
        {
            SearchFilter filter = (SearchFilter)Session["filter"];

            if (filter != null)
            {
                txtSubjectSearch.Text = filter.Subject;
                txtGroupSearch.Text = filter.GroupName;
                SentUserID.ClearSelection();

                try
                {
                    SentUserID.Items.FindByValue(filter.User).Selected = true;
                }
                catch
                {
                    SentUserID.Items[0].Selected = true;
                }

                BlastTypeDD.ClearSelection();

                try
                {
                    BlastTypeDD.Items.FindByValue(filter.blastType).Selected = true;
                }
                catch
                {
                    BlastTypeDD.Items[0].Selected = true;
                }

                drpCampaigns.ClearSelection();

                if (drpCampaigns.Visible )
                {
                    try
                    {
                        drpCampaigns.Items.FindByValue(filter.Campaign).Selected = true;
                    }
                    catch
                    {
                        drpCampaigns.Items[0].Selected = true;
                    }
                }
            }          
        }

        private void loadCampaignsDR(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Campaign> clist=
            ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID(customerID, Master.UserSession.CurrentUser, false);
            drpCampaigns.DataSource = clist;
            drpCampaigns.DataBind();
            if (drpCampaigns.Items.Count > 0)
            {
                drpCampaigns.Items.Insert(0, new ListItem("--All--", "0"));
                drpCampaigns.Visible = true;
            }
            else
            {
                drpCampaigns.Visible = false; 
            }
        }    

        public void SentUserID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {                       
            if (TabContainer1.ActiveTabIndex == 0)    
            {
                try
                {
                    CalendarCurrentDateDaily = Convert.ToDateTime(txtDailyCalendar.Text);
                }
                catch
                {
                    CalendarCurrentDateDaily = DateTime.Now; 
                }
                
                DailyCalendar();               
            }
            else if (TabContainer1.ActiveTabIndex == 2)     
            {               
                BindMonthlyCalendar();   
            }
            else if (TabContainer1.ActiveTabIndex == 1)       
            {
                WeeklyCalendar(DateTime.Now.ToShortDateString());     
            }
        }

        private void BindMonthlyCalendar()
        {
            rbBlastCalType.Visible = true;
            MonthCalender.VisibleDate = CalendarCurrentDateMonthly;

            Xml1.Visible = false;
            XmlWeekly.Visible = false;

            DateTime StartDate = CalendarCurrentDateMonthly;
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1);
            //DropDownList ddlClient = (DropDownList)Master.FindControl("drpClient");
            if (!(drpCampaigns.SelectedValue.Equals("")))
            {
                
                this.dtBlastData = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastCalendarDetails(ReportType == "SUM" ? 1 : 0, StartDate, EndDate, Convert.ToInt32(drpCampaigns.SelectedValue), BlastTypeDD.SelectedItem.Value, txtSubjectSearch.Text, txtGroupSearch.Text, Convert.ToInt32(SentUserID.SelectedItem.Value), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);;
            }
        }

        private void BindWeeklyCalendar(string startBlastDate)
        {
            DateTime startDate = CalculateWeekDays(startBlastDate);
            DateTime endDate = startDate.AddDays(6);
            //DropDownList ddlClient = (DropDownList)Master.FindControl("drpClient");
            if (!(drpCampaigns.SelectedValue.Equals("")))
                this.dtBlastData = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastCalendarDetails(0, startDate, endDate, Convert.ToInt32(drpCampaigns.SelectedValue), BlastTypeDD.SelectedItem.Value, txtSubjectSearch.Text, txtGroupSearch.Text, Convert.ToInt32(SentUserID.SelectedItem.Value), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
        }

        private void BindDailyCalendar()
        {
            DateTime StartDate = CalendarCurrentDateDaily;
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1);
            //DropDownList ddlClient = (DropDownList)Master.FindControl("drpClient");
            if (!(drpCampaigns.SelectedValue.Equals("")))
                this.dtBlastData = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastCalendarDaily(StartDate, EndDate, Convert.ToInt32(drpCampaigns.SelectedValue), BlastTypeDD.SelectedItem.Value, txtSubjectSearch.Text, txtGroupSearch.Text, Convert.ToInt32(SentUserID.SelectedItem.Value), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
        }


        protected void btnDailyBlast_Click(object sender, EventArgs e)
        {                 
            try
            {
               CalendarCurrentDateDaily = Convert.ToDateTime(txtDailyCalendar.Text);      
            }
            catch
            {
               CalendarCurrentDateDaily = DateTime.Now;  
            }
            
            DailyCalendar();  
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer1.ActiveTabIndex == 2)
            {
                BindMonthlyCalendar();
            }
            else if (TabContainer1.ActiveTabIndex == 0)
            {
                DailyCalendar();
            }
            else if (TabContainer1.ActiveTabIndex == 1)
            {
                WeeklyCalendar(DateTime.Now.ToShortDateString());
            }
        } 

        public string EscapeXml(string input)           
        {
            string output = string.Empty; 
            output = input.Replace("&", "&amp;");
            output = output.Replace("'", "&apos;");
            output = output.Replace("\"", "&quot;");
            output = output.Replace(">", "&gt;");
            output = output.Replace("<", "&lt;");             
            return output;
        }  

        private void DailyCalendar()
        {
            MonthCalender.Visible = false;
            TabContainer1.ActiveTabIndex = 0;
            XmlWeekly.Visible = false;
            Xml1.Visible = true; 
            
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);            
            XmlElement rootNode = xmlDoc.CreateElement("BlastDaily"); 
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode); 

            XmlElement blastDateElem = xmlDoc.CreateElement("BlastDate");
            blastDateElem.InnerXml = CalendarCurrentDateDaily.ToLongDateString(); 
            XmlElement prevDayElem = xmlDoc.CreateElement("PrevDayLink");
            prevDayElem.InnerXml = "<![CDATA[" + "http://" + Request.ServerVariables["HTTP_HOST"].ToString() +  "/ecn.communicator/main/blasts/BlastCalendarView.aspx?ct=daily&bdt=" + (CalendarCurrentDateDaily.AddDays(-1)).ToShortDateString() + "]]>";                           
            XmlElement nextDayElem = xmlDoc.CreateElement("NextDayLink");
            nextDayElem.InnerXml = "<![CDATA[" + "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/ecn.communicator/main/blasts/BlastCalendarView.aspx?ct=daily&bdt=" + (CalendarCurrentDateDaily.AddDays(1)).ToShortDateString() + "]]>";                

            rootNode.AppendChild(blastDateElem);  
            rootNode.AppendChild(prevDayElem);
            rootNode.AppendChild(nextDayElem);

            this.currReportDay = DateTime.Now;                         
            string altTEXT = string.Empty; 

            BindDailyCalendar();

            for (int i = 0; i < 24; i++)  
            {
                StringBuilder sbBlast = new StringBuilder();    
                XmlElement DayBlast = xmlDoc.CreateElement("DayBlast");    
                XmlElement Header = xmlDoc.CreateElement("Header");
                XmlElement Subject = xmlDoc.CreateElement("Subject");  
                DateTime startTime = DateTime.Parse(CalendarCurrentDateDaily.ToShortDateString() + " 12:00 AM").AddHours(i);             
                DateTime blastTimeDT;

                Header.InnerXml = startTime.ToShortTimeString();    

                /* Loop can be replaced by select for efficient search*/   

                foreach (DataRow dr in dtBlastData.Rows)
                {
                    blastTimeDT =  Convert.ToDateTime(dr["SendTime"].ToString());                        
                    bool dispCondition = startTime.Hour == blastTimeDT.Hour && CalendarCurrentDateDaily.ToShortDateString() == blastTimeDT.ToShortDateString();
                    string color = GetColor(dr["StatusCode"].ToString()); 

                    if (dispCondition)
                    {
                        altTEXT = string.Format("SendTime : {0} <br/> Group Name: {1} <br/> Subject: {2}", Convert.ToDateTime(dr["SendTime"]).ToShortTimeString(), dr["GroupName"].ToString(), dr["EmailSubject"].ToString());
                        sbBlast.Append("<a style = 'color:" + color + "' class='subject' title = '" + EscapeXml(altTEXT) + "' href='" + "/ecn.communicator/main/blasts/reports.aspx?blastID=" + dr["BlastID"].ToString() + "'>" +  EscapeXml(dr["GroupName"].ToString()) + " ( " + EscapeXml(ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString())) + " )" + "</a>");  
                    }    
                }

                Subject.InnerXml = sbBlast.ToString(); 
                DayBlast.AppendChild(Header);
                DayBlast.AppendChild(Subject);  
                rootNode.AppendChild(DayBlast);         
            }

            Xml1.DocumentContent = xmlDoc.InnerXml.ToString();             
            Xml1.TransformSource = xslDaily;         
        }
                                                             
        private void WeeklyCalendar(string startBlastDate)
        {
            TabContainer1.ActiveTabIndex = 1; 
            MonthCalender.Visible = false;
            Xml1.Visible = false;
            XmlWeekly.Visible = true; 
            DateTime startDate = CalculateWeekDays(startBlastDate);  
            DateTime endDate = startDate.AddDays(6);      

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);     
            XmlElement rootNode = xmlDoc.CreateElement("BlastWeekly");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement); 
            xmlDoc.AppendChild(rootNode);                       
            XmlElement blastWeek = xmlDoc.CreateElement("BlastWeek");
            XmlElement prevWeek = xmlDoc.CreateElement("PrevWeekLink");
            XmlElement nextWeek = xmlDoc.CreateElement("NextWeekLink");

            XmlElement DayHead1 = xmlDoc.CreateElement("Head1");
            XmlElement DayHead2 = xmlDoc.CreateElement("Head2");
            XmlElement DayHead3 = xmlDoc.CreateElement("Head3");
            XmlElement DayHead4 = xmlDoc.CreateElement("Head4");
            XmlElement DayHead5 = xmlDoc.CreateElement("Head5");
            XmlElement DayHead6 = xmlDoc.CreateElement("Head6");
            XmlElement DayHead7 = xmlDoc.CreateElement("Head7");

            XmlElement DaySub1 = xmlDoc.CreateElement("Day1");
            XmlElement DaySub2 = xmlDoc.CreateElement("Day2");
            XmlElement DaySub3 = xmlDoc.CreateElement("Day3");
            XmlElement DaySub4 = xmlDoc.CreateElement("Day4");
            XmlElement DaySub5 = xmlDoc.CreateElement("Day5");
            XmlElement DaySub6 = xmlDoc.CreateElement("Day6");
            XmlElement DaySub7 = xmlDoc.CreateElement("Day7");

            XmlText NodeText;

            NodeText = xmlDoc.CreateTextNode(startDate.ToLongDateString() + " to " + startDate.AddDays(6).ToLongDateString());
            blastWeek.AppendChild(NodeText);
            rootNode.AppendChild(blastWeek);
            prevWeek.InnerXml = "<![CDATA[" + "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/ecn.communicator/main/blasts/BlastCalendarView.aspx?ct=weekly&bdt=" + CalculateWeekDays((CalendarCurrentDateWeekly.AddDays(-7)).ToShortDateString()).ToShortDateString() + "]]>";                            
            rootNode.AppendChild(prevWeek);
            nextWeek.InnerXml = "<![CDATA[" + "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/ecn.communicator/main/blasts/BlastCalendarView.aspx?ct=weekly&bdt=" + CalculateWeekDays((CalendarCurrentDateWeekly.AddDays(7)).ToShortDateString()).ToShortDateString() + "]]>";                                      
            rootNode.AppendChild(nextWeek);              

            string [] HeaderList = new string[7];
            string[] HeaderLinks = new string[7]; 
            StringBuilder [] DaySubject = new StringBuilder[7];

            for (int i = 0; i <= 6; i++)
            {
                DaySubject[i] = new StringBuilder();  
            }

            BindWeeklyCalendar(startBlastDate);
            DataView dvBlast = dtBlastData.DefaultView; 

            for (int i = 0; i <= 6; i++)
            {                                
                if (dvBlast.Table.Rows.Count > 0)  
                {
                    string URL = "";
                    string aTitle = ""; 

                    foreach (DataRow dr in dvBlast.Table.Select("SendDate = '" + startDate.AddDays(i).ToString("MM/dd/yyyy") + "'"))                   
                    {
                        aTitle = EscapeXml(string.Format("SendTime : {0} <br /> Group Name: {1} <br /> Subject: {2} <br /> Send Total: {3}", Convert.ToDateTime(dr["SendTime"]).ToShortTimeString(), dr["GroupName"].ToString(), dr["EmailSubject"].ToString(), dr["sendtotal"].ToString())); 
                        URL = "<a title='" + aTitle + "' class='subject' style = 'color:" + GetColor(dr["StatusCode"].ToString()) + "' href='" + "/ecn.communicator/main/blasts/reports.aspx?blastID=" + dr["BlastID"].ToString() + "'>" + " " + "<![CDATA[" + Convert.ToDateTime(dr["SendTime"]).ToShortTimeString() + " " + dr["GroupName"].ToString() + "..." + "]]>" + "</a>"; 
                        string chkDayName = Convert.ToDateTime(dr["SendTime"]).ToLongDateString().Substring(0, 3);      

                        switch (chkDayName)  
                        {
                            case "Mon":
                                DaySubject[0].Append(URL);                     
                                break;
                            case "Tue":
                                DaySubject[1].Append(URL);
                                break;
                            case "Wed":
                                DaySubject[2].Append(URL);
                                break;
                            case "Thu":
                                DaySubject[3].Append(URL);    
                                break;
                            case "Fri":
                                DaySubject[4].Append(URL);     
                                break;
                            case "Sat":
                                DaySubject[5].Append(URL);
                                break;
                            case "Sun":
                                DaySubject[6].Append(URL);  
                                break;
                        }

                        URL = "";                          
                    }                    
                }

                HeaderList[i] = CalculateWeekDays(startDate.ToShortDateString()).AddDays(i).ToLongDateString();  
                HeaderLinks[i] = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/ecn.communicator/main/blasts/BlastCalendarView.aspx?ct=daily&bdt=" + CalculateWeekDays(startDate.ToShortDateString()).AddDays(i).ToShortDateString();                                  
            }

            DayHead1.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[0].ToString();
            DayHead1.InnerText = HeaderList[0].ToString();
            DayHead2.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[1].ToString();
            DayHead2.InnerText = HeaderList[1].ToString();
            DayHead3.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[2].ToString();
            DayHead3.InnerText = HeaderList[2].ToString();
            DayHead4.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[3].ToString();
            DayHead4.InnerText = HeaderList[3].ToString();
            DayHead5.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[4].ToString();
            DayHead5.InnerText = HeaderList[4].ToString();
            DayHead6.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[5].ToString();
            DayHead6.InnerText = HeaderList[5].ToString();
            DayHead7.Attributes.Append(xmlDoc.CreateAttribute("link")).Value = HeaderLinks[6].ToString();  
            DayHead7.InnerText = HeaderList[6].ToString();

            DaySub1.InnerXml = DaySubject[0].ToString();
            DaySub2.InnerXml = DaySubject[1].ToString();
            DaySub3.InnerXml = DaySubject[2].ToString();
            DaySub4.InnerXml = DaySubject[3].ToString();
            DaySub5.InnerXml = DaySubject[4].ToString();
            DaySub6.InnerXml = DaySubject[5].ToString();
            DaySub7.InnerXml = DaySubject[6].ToString();   

            rootNode.AppendChild(DayHead1);
            rootNode.AppendChild(DayHead2);
            rootNode.AppendChild(DayHead3);
            rootNode.AppendChild(DayHead4);
            rootNode.AppendChild(DayHead5);
            rootNode.AppendChild(DayHead6);
            rootNode.AppendChild(DayHead7);

            rootNode.AppendChild(DaySub1);
            rootNode.AppendChild(DaySub2);
            rootNode.AppendChild(DaySub3);
            rootNode.AppendChild(DaySub4);
            rootNode.AppendChild(DaySub5);
            rootNode.AppendChild(DaySub6);
            rootNode.AppendChild(DaySub7);

            XmlWeekly.DocumentContent = xmlDoc.InnerXml.ToString();          
            XmlWeekly.TransformSource = xslWeekly;                   
        }

        private DateTime CalculateWeekDays(string calendardate)
        {
            DateTime StartDate = Convert.ToDateTime(calendardate);
            string dtName = StartDate.DayOfWeek.ToString().Substring(0, 3);   
            switch (dtName)
            {
                case "Mon":
                    break;
                case "Tue":
                    StartDate = StartDate.AddDays(-1);
                    break;
                case "Wed":
                    StartDate = StartDate.AddDays(-2);
                    break;
                case "Thu":
                    StartDate = StartDate.AddDays(-3);
                    break;
                case "Fri":
                    StartDate = StartDate.AddDays(-4);
                    break;
                case "Sat":
                    StartDate = StartDate.AddDays(-5);
                    break;
                case "Sun":
                    StartDate = StartDate.AddDays(-6);
                    break;
            }
            return StartDate.Date;
        }
        
        public string GetColor(string status)
        {
            string color = string.Empty;

            if (status.ToUpper() == "SENT")
            {
                color = "";
            }
            else if (status.ToUpper() == "ACTIVE")
            {
                color = "green";
            }
            else if (status.ToUpper() == "PENDING")
            {
                color = "#78797B";
            }
            else
            {
                color = "blue";
            }

            return color;
        }

        protected void MonthCalender_SelectionChanged(object sender, EventArgs e)
        {
            BindMonthlyCalendar();
        }

        protected void MonthCalender_OnVisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            CalendarCurrentDateMonthly = e.NewDate;
            BindMonthlyCalendar();
        }

        protected void MonthCalender_DayRender(object sender, DayRenderEventArgs e)
        {
            if (ReportType.ToUpper() == "SUM")
            {
                StringBuilder sbcuDayBlastString = new StringBuilder();
                sbcuDayBlastString.Append(e.Day.Date.Day.ToString() + "<br /><br />");

                foreach (DataRow dr in dtBlastData.Rows)
                {
                    if (Convert.ToDateTime(dr["SendDate"].ToString()).ToShortDateString() == e.Day.Date.ToShortDateString() && !e.Day.IsOtherMonth)
                    {
                        int sentBlastCount = Convert.ToInt32(dr["SentTotal"].ToString());
                        int pendingBlastCount = Convert.ToInt32(dr["Pending"].ToString());
                        int activeBlastCount = Convert.ToInt32(dr["Active"].ToString());

                        if (sentBlastCount > 0)
                        {
                            sbcuDayBlastString.Append("<a runat='server' href= '/ecn.communicator/main/blasts/BlastCalendarView.aspx?bdt=" + e.Day.Date.ToShortDateString() + "&ct=daily'" + "" + "<span style='color:black'>Sent: " + sentBlastCount.ToString() + "</span></a><br/>");
                        }

                        if (pendingBlastCount > 0)
                        {
                            sbcuDayBlastString.Append("<a runat='server' href= '/ecn.communicator/main/blasts/BlastCalendarView.aspx?bdt=" + e.Day.Date.ToShortDateString() + "&ct=daily'" + "" + "<span style='color:#78797B'>Pending: " + pendingBlastCount.ToString() + "</span><br/>");
                        }

                        if (activeBlastCount > 0)
                        {
                            sbcuDayBlastString.Append("<a runat='server' href= '/ecn.communicator/main/blasts/BlastCalendarView.aspx?bdt=" + e.Day.Date.ToShortDateString() + "&ct=daily'" + "" + "<span style='color:green'>Active: " + activeBlastCount.ToString() + "</span><br/>");
                        }
                    }
                }

                e.Cell.Text = sbcuDayBlastString.ToString();
            }
            else if (ReportType.ToUpper() == "DET")
            {
                StringBuilder details = new StringBuilder();

                foreach (DataRow dr in dtBlastData.Rows)
                {
                    if (Convert.ToDateTime(dr["SendTime"].ToString()).ToShortDateString() == e.Day.Date.ToShortDateString() && !e.Day.IsOtherMonth)
                    {
                        string displayText = string.Empty;
                        string altTEXT = string.Empty;
                        string color = GetColor(dr["StatusCode"].ToString());

                        try
                        {
                            displayText = Convert.ToDateTime(dr["SendTime"]).ToShortTimeString() + " " + dr["GroupName"].ToString().Substring(0, 5) + "...";
                            altTEXT = string.Format("SendTime : {0} <br /> Group Name: {1} <br /> Subject: {2} <br /> Send Total: {3}", Convert.ToDateTime(dr["SendTime"]).ToShortTimeString(), dr["GroupName"].ToString(), dr["EmailSubject"].ToString(), dr["sendtotal"].ToString());
                        }
                        catch
                        {
                            displayText = Convert.ToDateTime(dr["SendTime"]).ToShortTimeString() + " " + dr["GroupName"].ToString() + "...";
                            altTEXT = string.Format("SendTime : {0} / Group Name: {1} / Subject: {2} / Send Total: {3}", Convert.ToDateTime(dr["SendTime"]).ToShortTimeString(), dr["GroupName"].ToString(), dr["EmailSubject"].ToString(), dr["sendtotal"].ToString());
                        }

                        details.Append("<a title='" + altTEXT.Replace("'", "") + "' style=color:" + color + " href=" + "/ecn.communicator/main/blasts/reports.aspx?blastID=" + dr["BlastID"].ToString() + ">" + " " + displayText + "</a><br />");
                        e.Cell.Text = e.Day.Date.Day.ToString() + "<br />" + details.ToString();
                        e.Cell.ForeColor = System.Drawing.Color.Blue;
                    }
                }
            }
        }
        
        protected void rbBlastCalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMonthlyCalendar();
        } 
    }
}
