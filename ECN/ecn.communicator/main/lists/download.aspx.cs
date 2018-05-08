using System;
using System.Collections;
using System.Data;
using System.Web;

namespace ecn.communicator.main.lists
{
    public partial class download : ECN_Framework.WebPageHelper
    {
        private const string SubscribeTypeAll = " 'S', 'U', 'D', 'P', 'B', 'M' ";

        private int FilterID
        {
            get
            {
                int filterID = 0;
                if (Request.QueryString["FilterID"] != null)
                {
                    filterID = Convert.ToInt32(Request.QueryString["FilterID"]);
                }
                return filterID;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                //                
            }
        }

        public ArrayList GetDataTableColumns(DataTable dataTable)
        {
            int nColumns = dataTable.Columns.Count;
            ArrayList columnHeadings = new ArrayList();
            DataColumn dataColumn = null;
            for (int i = 0; i < nColumns; i++)
            {
                dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName.ToString());
            }
            return columnHeadings;
        }

        public void downloadFile(object sender, System.EventArgs e)
        {
            string channelID = Request.QueryString["chID"].ToString();
            string customerID = Request.QueryString["custID"].ToString();
            string groupID = Request.QueryString["grpID"].ToString();
            string emailAddr = Request.QueryString["srchEm"].ToString();
            emailAddr = emailAddr.Replace("_", "[_]");
            string subscribeType = Request.QueryString["subType"].ToString();
            string searchType = Request.QueryString["srchType"].ToString();
            string downloadType = Request.QueryString["fileType"].ToString();

            string profFilter = Request.QueryString["profFilter"] != null ? Request.QueryString["profFilter"].ToString() : "ProfilePlusAllUDFs";

            string OSFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/downloads/");

            string delimiter;

            var filter = PopulateFilter(downloadType, emailAddr, searchType, FilterID, SubscribeTypeAll, ref subscribeType, out delimiter);

            DataTable emailProfilesDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(Convert.ToInt32(groupID), Convert.ToInt32(customerID), filter, subscribeType, profFilter);
            String tfile = customerID + "-" + groupID + "emails" + downloadType;
            string outfileName = OSFilePath + tfile;

            PopulateResponse(OSFilePath, outfileName, downloadType, emailProfilesDT, delimiter, tfile, new HttpResponseWrapper(Response));
        }
    }
}
