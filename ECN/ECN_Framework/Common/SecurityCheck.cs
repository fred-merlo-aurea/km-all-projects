using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Web.Security;
using ECN_Framework.Accounts.Entity;


namespace ECN_Framework.Common
{
    public class SecurityCheck
    {

        public SecurityCheck()
        {
           // _currentUser = ECN_Framework.Accounts.Entity.User.Get(Convert.ToInt32(UserID()));
        }

        //public ECN_Framework.Accounts.Entity.User CurrentUser
        //{
        //    get
        //    {
        //        return _currentUser;
        //    }
        //    set
        //    {
        //        _currentUser = ECN_Framework.Accounts.Entity.User.Get(Convert.ToInt32(UserID()));
        //    }
        //}

        //get UserID
        public string UserID()
        {
            try
            {
                return getCookie().Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        //get CustomerID
        public int CustomerID()
        {
            try
            {
                string[] myIDs = getCookie().UserData.Split(',');
                return int.Parse(myIDs[0]);
            }
            catch
            {
                return 0;
            }
        }

        //get the BaseChannelID  as ChannelID
        public int BasechannelID()
        {
            try
            {
                string[] myIDs = getCookie().UserData.Split(',');
                return int.Parse(myIDs[1]);
            }
            catch
            {
                return 0;
            }
        }

        public int ClientGroupID()
        {
            try
            {
                string[] myIDs = getCookie().UserData.Split(',');
                return int.Parse(myIDs[2]);
            }
            catch
            {
                return 0;
            }
        }

        public int ClientID()
        {
            try
            {
                string[] myIDs = getCookie().UserData.Split(',');
                return int.Parse(myIDs[3]);
            }
            catch
            {
                return 0;
            }
        }

        ////get CommunicatorChannelID  for the Customer
        //public string CommunicatorChannelID()
        //{
        //    return "0"; // TODO
        //    //string commChannelID = "";
        //    //char[] channelArray;
        //    //try
        //    //{
        //    //    string[] myIDs = getCookie().UserData.Split(',');
        //    //    commChannelID = myIDs[2];
        //    //    channelArray = commChannelID.ToCharArray();
        //    //    commChannelID = channelArray[0].ToString();
        //    //}
        //    //catch
        //    //{

        //    //    commChannelID = "0";
        //    //}
        //    //return commChannelID;
        //}

        //get Sec Options
        //public string SecurityOptions()
        //{
        //    try
        //    {
        //        // TODO
        //        return "";
        //        //string[] myIDs = getCookie().UserData.Split(',');
        //        //return myIDs[3];
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}



        //Sys Admin 
        public bool CheckSysAdmin()
        {
            KMPlatform.Entity.User _currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;

            return KM.Platform.User.IsSystemAdministrator(_currentUser);
        }
        //Channel Admin
        public bool CheckChannelAdmin()
        {
            KMPlatform.Entity.User _currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;

            return KM.Platform.User.IsChannelAdministrator(_currentUser);
        }

        ////Admin
        public bool CheckAdmin()
        {
            KMPlatform.Entity.User _currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;

            return KM.Platform.User.IsAdministrator(_currentUser);
        }

        ////Admin
        //public bool CheckUserAccess()
        //{
        //    secOptionsArray = SecurityOptions().ToCharArray();
        //    if (secOptionsArray[3] == '1')
        //        return true;
        //    else
        //        return false;
        //}

        //Admin
        //public bool CheckCustomerAccess()
        //{
        //    secOptionsArray = SecurityOptions().ToCharArray();
        //    if (secOptionsArray[4] == '1')
        //        return true;
        //    else
        //        return false;
        //}

        ////Admin
        //public bool CheckChannelAccess()
        //{
        //    secOptionsArray = SecurityOptions().ToCharArray();
        //    if (secOptionsArray[5] == '1')
        //        return true;
        //    else
        //        return false;
        //}

        // Use ECNSession object to find out permissions.
        // Sunil - 10/19/2007
        #region  Use ECNSession object to find out permissions.

        #endregion

        ////get the Customer Levels
        //public string CustomerLevel()
        //{
        //    try
        //    {
        //        return ""; 
        //        //string[] myIDs = getCookie().UserData.Split(',');
        //        //return myIDs[4];
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        ////get the Customer Levels
        //public int MasterUserID()
        //{
        //    try
        //    {
        //        return 0; // TODO
        //        //string[] myIDs = getCookie().UserData.Split(',');
        //        //return Convert.ToInt32(myIDs[5]);
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        //public string getCommunicatorLevel()
        //{
        //    customerLevelsArray = CustomerLevel().ToCharArray();
        //    return customerLevelsArray[0].ToString();
        //}

        ////Collector Level
        //public string getCollectorLevel()
        //{
        //    customerLevelsArray = CustomerLevel().ToCharArray();
        //    return customerLevelsArray[1].ToString();
        //}

        ////Creator Level
        //public string getCreatorLevel()
        //{
        //    throw new NotImplementedException();
        //    //KM.Platform.User.HasApplication()
        //    //customerLevelsArray = CustomerLevel().ToCharArray();
        //    //return customerLevelsArray[2].ToString();
        //}

        ////Accounts Level
        //public string getAccountsLevel()
        //{
        //    customerLevelsArray = CustomerLevel().ToCharArray();
        //    return customerLevelsArray[3].ToString();
        //}

        ////Publisher Level
        //public string getPublisherLevel()
        //{
        //    customerLevelsArray = CustomerLevel().ToCharArray();
        //    return customerLevelsArray[4].ToString();
        //}

        ////Charity Level
        //public string getCharityLevel()
        //{
        //    customerLevelsArray = CustomerLevel().ToCharArray();
        //    return customerLevelsArray[5].ToString();
        //}

        public FormsAuthenticationTicket getCookie()
        {
            FormsAuthenticationTicket ticket = null;

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        System.Web.Security.FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        ticket = id.Ticket;
                    //    string AuthData = ticket.UserData;
                    //    string AuthName = ticket.Name;
                    //    string AuthTime = ticket.IssueDate.ToString();
                    //    string[] myIDs = AuthData.Split(',');
                    }
                }
            }

            return ticket;
        }

        public string GetHashedPass(String aPassword)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword, "sha1");
        }
    }
}
