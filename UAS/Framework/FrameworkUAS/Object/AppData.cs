using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.Object
{
    public class AppData
    {
        public AppData()
        {
            AuthorizedUser = new Object.UserAuthorization();
            CurrentApp = new KMPlatform.Entity.Application();
            //SubscribersViewedList = new List<FrameworkCirculation.Entity.Subscriber>();
            SqlServer = string.Empty;
            SmtpServer = string.Empty;
            //BatchList = new List<FrameworkCirculation.Entity.Batch>();
            //OpenWindowCount = 0;
            ParentWindowUid = string.Empty;
        }
        public AppData(string parentWindowUid)
        {
            AuthorizedUser = new Object.UserAuthorization();
            CurrentApp = new KMPlatform.Entity.Application();
            //SubscribersViewedList = new List<FrameworkCirculation.Entity.Subscriber>();
            SqlServer = string.Empty;
            SmtpServer = string.Empty;
            //BatchList = new List<FrameworkCirculation.Entity.Batch>();
            //OpenWindowCount = 0;
            ParentWindowUid = parentWindowUid;
        }
        public Object.UserAuthorization AuthorizedUser { get; set; }
        public KMPlatform.Entity.Application CurrentApp { get; set; }
        //public List<FrameworkCirculation.Entity.Subscriber> SubscribersViewedList;

        public string SqlServer { get; set; }
        public string SmtpServer { get; set; }
        public string FileDirectory = @"C:\ADMS\Log\";

        /// <summary>
        /// can use this to ensure the object belongs to your instance
        /// </summary>
        public string ParentWindowUid { get; set; }
        
        #region Circulation Batch/History Management
        public int OpenWindowCount { get; set; }
        public bool NewSubscriptionOpened { get; set; }
        public List<FrameworkUAS.Object.Batch> BatchList { get; set; }

        #endregion

        public void Clear()
        {
            CurrentApp = null;
            AuthorizedUser = null;
            //SubscribersViewedList = null;
            //BatchList = null;
        }

        public static bool CheckParentWindowUid(string myUid)
        {
            return myAppData.ParentWindowUid.Equals(myUid);
        }
        public static bool IsKmUser()
        {
            return myAppData.AuthorizedUser.User.ClientGroups.Exists(x => x.Clients.Exists(c => c.ClientName.Equals("KM") == true || c.ClientName.Equals("KM") == true ||
                c.DisplayName.Equals("KM") == true || c.DisplayName.Equals("KM") == true || c.FtpFolder.Equals("KM") == true || c.FtpFolder.Equals("KM") == true));
        }

        private static AppData _appData;
        public static AppData myAppData
        {
            get
            {
                if (_appData == null)
                {
                    FrameworkUAS.Object.AppData ad = new FrameworkUAS.Object.AppData();
                    _appData = ad;
                    return _appData;
                }
                else
                    return _appData;
            }
            set
            {
                _appData = value;
            }
        }
    }
}
