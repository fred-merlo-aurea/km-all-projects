//using System;
//using System.Data;
//using System.Data.SqlClient;
//using ecn.common.classes;

//namespace ecn.communicator.classes
//{
//    public class BlastLinks
//    {
//        #region Private Properties
//        private int _BlastID;
//        private string _LinkURL;
//        private int _BlastLinkID;
//        #endregion

//        #region Public Properties
//        public int BlastID
//        {
//            get 
//            { 
//                return _BlastID; 
//            }
//            set 
//            { 
//                _BlastID = value; 
//            }
//        }
//        public int BlastLinkID
//        {
//            get
//            {
//                return _BlastLinkID;
//            }
//            set
//            {
//                _BlastLinkID = value;
//            }
//        }
//        public string LinkURL
//        {
//            get
//            {
//                return _LinkURL;
//            }
//            set
//            {
//                _LinkURL = value;
//            }
//        }
//        #endregion

//        //Constructor
//        public BlastLinks():base() 
//        {
//        }

//        #region Public Methods
//        public int Create()
//        {
//            if (_BlastID == 0 || _LinkURL.Trim().Length <= 0)
//            {
//                return -1;
//            }
//            string sqlQuery = "INSERT INTO BlastLinks (BlastID, LinkURL) VALUES (@BlastID, @LinkURL); SELECT @@IDENTITY;";
//            SqlCommand cmd = new SqlCommand(sqlQuery);
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.Add(new SqlParameter("@BlastID", _BlastID));
//            cmd.Parameters.Add(new SqlParameter("@LinkURL", LinkCleanUP(_LinkURL)));
//            int newBlastLinkID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmd));
//            return newBlastLinkID;
//        }

//        private static string LinkCleanUP(string link)
//        {
//            link = link.Replace("&amp;", "&");
//            link = link.Replace("&lt;", "<");
//            link = link.Replace("&gt;", ">");
//            link = link.Replace("%23", "#");

//            return link;
//        }

//        public static BlastLinks GetBlastLink(int blastLinkID, int blastID)
//        {
//            if (blastLinkID == 0)
//            {
//                return null;
//            }
//            string sqlQuery = "SELECT * FROM BlastLinks WHERE BlastLinkID = @BlastLinkID and BlastID = @BlastID";
//            SqlCommand cmd = new SqlCommand(sqlQuery);
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.Add(new SqlParameter("@BlastLinkID", blastLinkID));
//            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
//            DataTable dt = DataFunctions.GetDataTable(cmd);
//            if (dt.Rows.Count == 0)
//            {
//                return null;
//            }
//            BlastLinks blastLink = new BlastLinks();
//            blastLink.BlastID = blastLinkID;
//            blastLink.BlastID = Convert.ToInt32(dt.Rows[0]["BlastID"].ToString());
//            blastLink.LinkURL = dt.Rows[0]["LinkURL"].ToString();
//            return blastLink;
//        }

//        public static BlastLinks GetBlastLink(int blastID, string linkURL)
//        {
//            if (blastID == 0 || linkURL.Length <= 0)
//            {
//                return null;
//            }
//            linkURL = LinkCleanUP(linkURL);
//            string sqlQuery = "SELECT * FROM BlastLinks WHERE BlastID = @BlastID AND LinkURL = @LinkURL";
//            SqlCommand cmd = new SqlCommand(sqlQuery);
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
//            cmd.Parameters.Add(new SqlParameter("@LinkURL", linkURL));
//            DataTable dt = DataFunctions.GetDataTable(cmd);
//            if (dt.Rows.Count == 0)
//            {
//                return null;
//            }
//            BlastLinks blastLink = new BlastLinks();
//            blastLink.BlastID = blastID;
//            blastLink.BlastLinkID = Convert.ToInt32(dt.Rows[0]["BlastLinkID"].ToString());
//            blastLink.LinkURL = linkURL;
//            return blastLink;
//        }
//        #endregion
//    }
//}
