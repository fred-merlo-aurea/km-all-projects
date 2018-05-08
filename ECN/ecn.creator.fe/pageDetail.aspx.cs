using System;
using System.Web;
using System.Data;
using System.Configuration;
using ecn.creator.classes;
using ecn.common.classes;
using System.Data.SqlClient;

namespace ecn.creator.fe.pages
{
    /// <summary>
    /// Summary description for preview.
    /// </summary>
    public class pageDetail : System.Web.UI.Page
    {

        protected System.Web.UI.WebControls.Label LabelPreview;

        public static string communicatordb = ConfigurationSettings.AppSettings["communicatordb"];
        public static string accountsdb = ConfigurationSettings.AppSettings["accountsdb"];
        public static int customerID = Convert.ToInt32(ConfigurationSettings.AppSettings["CustomerID"]);
        public static string body = "";
        public static string keywords = "";
        public static string StyleSheet = "";
        public static string javaScriptCode = "";
        public static string pageTitle = "";
        public static string pageProps = "";
        public static string searchString = "";

        private void Page_Load(object sender, System.EventArgs e)
        {
            int requestPageID = getPageID();
            string requestPageIdentifier = getPageIdentifier(); //queryvalue

            string HostName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();

            if (HostName.ToLower().ToString() == "localhost")
                HostName = "www.goldensupplyinc.com";

            customerID = getCustomerID(HostName);

            try
            {
                searchString = Request["web_site_search"];
            }
            catch (Exception ex)
            {
                searchString = "";
            }

            ShowPage(requestPageIdentifier, customerID);
        }

        private int getCustomerID(string hostName)
        {
            int theCustomerID = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("select CustomerID from dbo.Customer where WebAddress= @hostName");
                cmd.Parameters.AddWithValue("@hostName", hostName);

                theCustomerID = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()));
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCustomerID;
        }

        private int getPageID()
        {
            int thePageID = 0;
            try
            {
                thePageID = Convert.ToInt32(Request.QueryString["PageID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return thePageID;
        }

        private string getPageIdentifier()
        {
            string thePageIdentifier = "";
            try
            {
                thePageIdentifier = Request.QueryString["pgID"].ToString();
                if (thePageIdentifier.Length > 49)
                {
                    thePageIdentifier = thePageIdentifier.Substring(0, 49);
                }
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return thePageIdentifier;
        }

        private string getSearchResultsData(string searchStr, int customerID)
        {
            if (searchStr.Length > 49)
            {
                return "Search string too long. Max length of Search string is 50 characters. Please try your search again.";
            }
            else
            {
                string sql = " SELECT p.PageName, p.QueryValue FROM Pages p LEFT OUTER JOIN " + communicatordb + ".dbo.content c ON " +
                   " (p.contentSlot1 = c.contentID OR p.contentSlot2 = c.contentID OR p.contentSlot3 = c.contentID OR " +
                   " p.contentSlot4 = c.contentID OR p.contentSlot5 = c.contentID OR p.contentSlot6 = c.contentID) " +
                   " WHERE c.customerID = @customerID AND c.ContentSource like @searchString " +
                   " GROUP BY p.PageName, p.QueryValue ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@customerID", customerID);
                cmd.Parameters.AddWithValue("@searchString", "%" + searchStr + "%");

                string pgName = "", qryValue = "";
                string srchResults = "<table border=0 colspacing=0 cellpadding=3 width=100% >";
                srchResults += "<tr><td><font face=verdana size=2><b> Search Results for: <i>" + searchStr + "</i><b></font></td></tr>";
                DataTable dt = DataFunctions.GetDataTable("creator", cmd);
                foreach (DataRow dr in dt.Rows)
                {
                    pgName = dr["PageName"].ToString();
                    qryValue = dr["QueryValue"].ToString();
                    srchResults += "<tr><td style=\"BORDER-RIGHT: silver 1px dotted; BORDER-TOP: silver 1px dotted; BORDER-LEFT: silver 1px dotted; BORDER-BOTTOM: silver 1px dotted\"><a href='pageDetail.aspx?pgID=" + qryValue + "'><font face=verdana size=2>" + pgName + "</font></a></td></tr>";
                }
                if (dt.Rows.Count > 0)
                {
                    return srchResults;
                }
                else
                {
                    return "Search returned no data. Please try your search again.";
                }
            }
        }

        private void ShowPage(string setPageIdentifier, int customerID)
        {
            string sqlQuery = "";
            string TemplateSource = "";
            string HeaderCode = "";
            string FooterCode = "";
            int Slot1 = 0;
            int Slot2 = 0;
            int Slot3 = 0;
            int Slot4 = 0;
            int Slot5 = 0;
            int Slot6 = 0;
            int Slot7 = 0;
            int Slot8 = 0;
            int Slot9 = 0;
            SqlCommand cmd = new SqlCommand();
            if (setPageIdentifier.Length > 0 && setPageIdentifier.Length < 50)
            {
                sqlQuery =
                    " select * from Pages p JOIN " + communicatordb + ".dbo.templates t ON p.templateID=t.templateID " +
                    " JOIN HeaderFooters hf ON p.HeaderFooterID = hf.HeaderFooterID " +
                    " where p.QueryValue= @pageIdentifier " +
                    " and p.customerID =  @CustomerID ";
                cmd = new SqlCommand(sqlQuery);
                cmd.Parameters.AddWithValue("@pageIdentifier", setPageIdentifier);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
            }
            else
            {
                sqlQuery =
                    " select * from Pages p JOIN " + communicatordb + ".dbo.templates t ON p.templateID=t.templateID " +
                    " JOIN HeaderFooters hf ON p.HeaderFooterID = hf.HeaderFooterID " +
                    " where p.customerID = @CustomerID and p.HomePageFlag = 'Y'";
                cmd = new SqlCommand(sqlQuery);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
            }

            DataTable dt = DataFunctions.GetDataTable("creator", cmd);
            foreach (DataRow dr in dt.Rows)
            {
                TemplateSource = dr["TemplateSource"].ToString();
                Slot1 = (int)dr["ContentSlot1"];
                Slot2 = (int)dr["ContentSlot2"];
                Slot3 = (int)dr["ContentSlot3"];
                Slot4 = (int)dr["ContentSlot4"];
                Slot5 = (int)dr["ContentSlot5"];
                Slot6 = (int)dr["ContentSlot6"];
                Slot7 = (int)dr["ContentSlot7"];
                Slot8 = (int)dr["ContentSlot8"];
                Slot9 = (int)dr["ContentSlot9"];
                HeaderCode = dr["HeaderCode"].ToString();
                FooterCode = dr["FooterCode"].ToString();
                keywords = dr["keywords"].ToString();
                StyleSheet = dr["StyleSheet"].ToString();
                javaScriptCode = dr["JavaScriptCode"].ToString();
                pageTitle = dr["PageName"].ToString();
                pageProps = dr["PageProperties"].ToString();
            }

            Response.Write(HeaderCode);

            body = TemplateFunctions.HTMLPagePreview(setPageIdentifier, HeaderCode, FooterCode, TemplateSource, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9, customerID);

            //check if there's a search string:
            string results = "";
            try
            {
                if (searchString.Trim().Length > 0)
                {
                    results = getSearchResultsData(searchString.Trim(), customerID);
                }
                else
                {
                    results = "";
                }
            }
            catch (Exception ex)
            {
                //means nothing in the search box. !!
            }
            body = body.Replace("%%searchResults%%", results);
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
    }
}

