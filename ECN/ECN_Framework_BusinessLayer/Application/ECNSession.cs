using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ECN_Framework_BusinessLayer.Application
{
    public class ECNSession
    {
        private const string SMART_SESSION = "ECNUSERSession";


        public KMPlatform.Entity.User CurrentUser = new KMPlatform.Entity.User();
        public ECN_Framework_Entities.Accounts.Customer CurrentCustomer = new ECN_Framework_Entities.Accounts.Customer();
        public ECN_Framework_Entities.Accounts.BaseChannel CurrentBaseChannel = new ECN_Framework_Entities.Accounts.BaseChannel();
        //public List<KMPlatform.Entity.ClientGroup> CurrentUserClientGroups = new List<KMPlatform.Entity.ClientGroup>();
        public List<KMPlatform.Entity.Client> CurrentUserClientGroupClients = new List<KMPlatform.Entity.Client>();


        public int UserID { get; internal set; }
        public int CustomerID { get; internal set; }
        public int BaseChannelID { get; internal set; }
        public int ClientGroupID { get; internal set; }
        public int ClientID { get; internal set; }
        public int ProductID { get;  set; }

        public static ECNSession CurrentSession()
        {
            ECNSession _ecnSession = null;
            ECN_Framework_Entities.Application.AuthenticationTicket AuthTicket = ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket();

            if (AuthTicket != null)
            {
                if (null == HttpContext.Current.Session[SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString())])
                {
                    string CacheID = string.Format("{0}_{1}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString());

                    if (null != HttpContext.Current.Cache["CurrentUser" + "_" + CacheID])
                    {
                        System.Web.HttpContext.Current.Cache.Remove("CurrentUser" + "_" + CacheID);
                    }

                    if (null != HttpContext.Current.Cache["CurrentCustomer" + "_" + AuthTicket.CustomerID.ToString()])
                    {
                        System.Web.HttpContext.Current.Cache.Remove("CurrentCustomer" + "_" + AuthTicket.CustomerID.ToString());
                    }

                    if (null != HttpContext.Current.Cache["CurrentBaseChannel" + "_" + AuthTicket.BaseChannelID.ToString()])
                    {
                        System.Web.HttpContext.Current.Cache.Remove("CurrentBaseChannel" + "_" + AuthTicket.BaseChannelID.ToString());
                    }

                    if (null != HttpContext.Current.Cache["CurrentUserClientGroupClients" + "_" + AuthTicket.UserID.ToString() + "_" + AuthTicket.ClientGroupID.ToString()])
                    {
                        System.Web.HttpContext.Current.Cache.Remove("CurrentUserClientGroupClients" + "_" + AuthTicket.UserID.ToString() + "_" + AuthTicket.ClientGroupID.ToString());
                    }

                    _ecnSession = new ECNSession();
                    System.Web.HttpContext.Current.Session[SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString())] = _ecnSession;
                }
                else
                {
                    _ecnSession = (ECNSession)System.Web.HttpContext.Current.Session[SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString())];
                    _ecnSession.RefreshSession();
                }
            }

            return _ecnSession;
        }

        private ECNSession()
        {
            RefreshSession();
        }

        public void ClearSession()
        {
            ECN_Framework_Entities.Application.AuthenticationTicket AuthTicket = ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket();

            if (null == HttpContext.Current.Session[SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString())])
            {
                HttpContext.Current.Session.Remove(SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString()));
            }
        }

        public void RemoveSession()
        {
            ECN_Framework_Entities.Application.AuthenticationTicket AuthTicket = ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket();

            if (null != HttpContext.Current.Session[SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString())])
            {
                HttpContext.Current.Session.Remove(SMART_SESSION + "_" + string.Format("{0}_{1}_{2}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString(), AuthTicket.IssueDate.ToString()));
            }
        }

        public void RefreshSession()
        {
            ECN_Framework_Entities.Application.AuthenticationTicket AuthTicket = ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket();

            UserID = AuthTicket.UserID;
            CustomerID = AuthTicket.CustomerID;
            BaseChannelID = AuthTicket.BaseChannelID;
            ClientGroupID = AuthTicket.ClientGroupID;
            ClientID = AuthTicket.ClientID;
            ProductID = AuthTicket.ProductID;

            string CacheID = string.Format("{0}_{1}", AuthTicket.UserID.ToString(), AuthTicket.ClientID.ToString());

            if (null == HttpContext.Current.Cache["CurrentUser" + "_" + CacheID])
            {
                KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
                CurrentUser = uWorker.SelectUser(AuthTicket.UserID, false);

                CurrentUser = uWorker.SetAuthorizedUserObjects(CurrentUser, ClientGroupID, ClientID);
                //CurrentUser = KMPlatform.BusinessLogic.User.GetByUserID(AuthTicket.UserID, AuthTicket.CustomerID, true);
                if (CurrentUser != null)
                    System.Web.HttpContext.Current.Cache.Insert("CurrentUser" + "_" + CacheID, CurrentUser);
            }
            else
            {
                CurrentUser = (KMPlatform.Entity.User)System.Web.HttpContext.Current.Cache["CurrentUser" + "_" + CacheID];
            }


            if (null == HttpContext.Current.Cache["CurrentCustomer" + "_" + AuthTicket.CustomerID.ToString()])
            {
                CurrentCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(AuthTicket.CustomerID, true);
                if (CurrentCustomer != null)
                    System.Web.HttpContext.Current.Cache.Insert("CurrentCustomer" + "_" + AuthTicket.CustomerID.ToString(), CurrentCustomer);
            }
            else
            {
                CurrentCustomer = (ECN_Framework_Entities.Accounts.Customer)System.Web.HttpContext.Current.Cache["CurrentCustomer" + "_" + AuthTicket.CustomerID.ToString()];
            }

            if (null == HttpContext.Current.Cache["CurrentBaseChannel" + "_" + AuthTicket.BaseChannelID.ToString()])
            {
                CurrentBaseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(CurrentCustomer.BaseChannelID.Value);
                if (CurrentBaseChannel != null)
                {
                    CurrentBaseChannel.IsPublisher = CurrentBaseChannel.BounceDomain == "kmpsgroupbounce.com";
                    System.Web.HttpContext.Current.Cache.Insert("CurrentBaseChannel" + "_" + AuthTicket.BaseChannelID.ToString(), CurrentBaseChannel);
                }
            }
            else
            {
                CurrentBaseChannel = (ECN_Framework_Entities.Accounts.BaseChannel)System.Web.HttpContext.Current.Cache["CurrentBaseChannel" + "_" + AuthTicket.BaseChannelID.ToString()];
            }


            //if (null == HttpContext.Current.Cache["CurrentUserClientGroups" + "_" + AuthTicket.UserID.ToString()])
            //{
            //    if (KM.Platform.User.IsSystemAdministrator(CurrentUser))
            //    {
            //        CurrentUserClientGroups = (new KMPlatform.BusinessLogic.ClientGroup()).Select(false);
            //    }
            //    else
            //    {
            //        CurrentUserClientGroups = (new KMPlatform.BusinessLogic.ClientGroup()).SelectForUser(CurrentUser.UserID, false);
            //    }


            //    if (CurrentUserClientGroups != null)
            //    {
            //        System.Web.HttpContext.Current.Cache.Insert("CurrentUserClientGroups" + "_" + AuthTicket.UserID.ToString(), CurrentUserClientGroups.OrderBy(o => o.ClientGroupName).ToList());
            //    }
            //}
            //else
            //{
            //    CurrentUserClientGroups = (List<KMPlatform.Entity.ClientGroup>)System.Web.HttpContext.Current.Cache["CurrentUserClientGroups" + "_" + AuthTicket.UserID.ToString()];
            //}

            if (null == HttpContext.Current.Cache["CurrentUserClientGroupClients" + "_" + AuthTicket.UserID.ToString() + "_" + ClientGroupID.ToString()])
            {
                if (KM.Platform.User.IsSystemAdministrator(CurrentUser))
                {
                    CurrentUserClientGroupClients = (new KMPlatform.BusinessLogic.Client()).SelectForClientGroup(ClientGroupID, false);
                }
                else
                {
                    CurrentUserClientGroupClients = (new KMPlatform.BusinessLogic.Client()).SelectbyUserIDclientgroupID(UserID, ClientGroupID, false);
                }

                if (CurrentUserClientGroupClients != null)
                    System.Web.HttpContext.Current.Cache.Insert("CurrentUserClientGroupClients" + "_" + AuthTicket.UserID.ToString() + "_" + ClientGroupID.ToString(), CurrentUserClientGroupClients.OrderBy(o => o.ClientName).ToList());
            }
            else
            {
                CurrentUserClientGroupClients = (List<KMPlatform.Entity.Client>)System.Web.HttpContext.Current.Cache["CurrentUserClientGroupClients" + "_" + AuthTicket.UserID.ToString() + "_" + ClientGroupID.ToString()];
            }

            //CurrentUser = KMPlatform.BusinessLogic.User.GetByUserID(AuthTicket.UserID, AuthTicket.CustomerID, true);
            //CurrentCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(AuthTicket.CustomerID, true);
            //CurrentBaseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetBaseChannelByID(CurrentCustomer.BaseChannelID.Value);
        }

    }
}
