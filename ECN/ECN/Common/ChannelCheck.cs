//using System;
//using System.Configuration;
//using System.Data;
//using System.Web;

//namespace ecn.common.classes
//{
//    public class ChannelCheck
//    {
//        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
//        public ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
//        int _customer_id;
//        int _channel_id = 0;

//        public ChannelCheck()
//        {
//            _customer_id = 0;
//            _channel_id = 0;
//        }

//        public int ChannelID
//        {
//            get
//            {
//                int CurrentChannelID = 0;

//                try
//                {
//                    CurrentChannelID = Convert.ToInt32(sc.ChannelID());
//                }
//                catch
//                {
//                    CurrentChannelID = 1;
//                }

//                if (CurrentChannelID != 0 && _channel_id != CurrentChannelID)
//                    _channel_id = Convert.ToInt32(getChannelID());

//                return this._channel_id;
//            }
//        }

//        public void CustomerID(int id)
//        {
//            _customer_id = id;
//        }

//        public string getHostName()
//        {
//            return DataFunctions.ExecuteScalar("SELECT ChannelURL FROM " + accountsdb + ".dbo.BaseChannel WHERE BaseChannelID = '" + ChannelID + "'").ToString();
//        }
//        public string getBounceDomain()
//        {
//            return DataFunctions.ExecuteScalar("SELECT BounceDomain FROM " + accountsdb + ".dbo.BaseChannel WHERE BaseChannelID = '" + ChannelID + "'").ToString();
//        }
//        public string getHeaderSource(string productType)
//        {
//            return DataFunctions.ExecuteScalar("SELECT HeaderSource FROM " + accountsdb + ".dbo.Channel WHERE ChannelTypeCode='" + productType + "' AND BaseChannelID = (select case when isBranding = 1 then basechannelID else 1 end from " + accountsdb + ".dbo.basechannel where basechannelID = '" + ChannelID + "')").ToString();
//        }
//        public string getFooterSource(string productType)
//        {
//            return DataFunctions.ExecuteScalar("SELECT FooterSource FROM " + accountsdb + ".dbo.Channel WHERE ChannelTypeCode='" + productType + "' AND BaseChannelID = (select case when isBranding = 1 then basechannelID else 1 end from " + accountsdb + ".dbo.basechannel where basechannelID = '" + ChannelID + "')").ToString();
//        }
//        public string getChannelName()
//        {
//            return DataFunctions.ExecuteScalar("SELECT baseChannelName FROM " + accountsdb + ".dbo.baseChannel WHERE BaseChannelID = '" + ChannelID + "'").ToString(); 
//        }

//        private string getChannelID()
//        {
//            if (_customer_id > 0)
//            {
//                return DataFunctions.ExecuteScalar("SELECT BaseChannelID FROM " + accountsdb + ".dbo.Customer WHERE CustomerID=" + _customer_id).ToString();
//            }
//            string theChannelID = "";
//            // get it from the querystring
//            try
//            {
//                theChannelID = HttpContext.Current.Request.QueryString["channel"].ToString();
//            }
//            catch { }

//            // get it from the cookie
//            if (theChannelID == "")
//            {
//                try
//                {
//                    theChannelID = sc.ChannelID();
//                }
//                catch { }
//            }

//            // get it from the host
//            if (theChannelID == "")
//            {
//                string HostName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
//                try
//                {
//                    theChannelID = getChannelID(HostName);
//                }
//                catch { }
//            }

//            // default it to 1
//            if (theChannelID == "")
//            {
//                theChannelID = "1";
//            }
//            return theChannelID;
//        }

//        private string getChannelID(string HostName)
//        {
//            return DataFunctions.ExecuteScalar("SELECT BaseChannelID FROM " + accountsdb + ".dbo.BaseChannel WHERE ChannelURL='" + HostName + "'").ToString();
//        }
//    }
//}
