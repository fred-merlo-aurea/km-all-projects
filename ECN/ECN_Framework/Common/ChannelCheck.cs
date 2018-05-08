using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using ECN_Framework.Accounts.Entity;
using ECN_Framework.Accounts;

namespace ECN_Framework.Common
{
    public class ChannelCheck
    {
        public SecurityCheck sc = new SecurityCheck();
        public ECN_Framework_Entities.Accounts.BaseChannel bc = new ECN_Framework_Entities.Accounts.BaseChannel();

        public ChannelCheck()
        {
            _customer_id = 0;
            _channel_id = 0;
            bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(ChannelID);
        }

        public ChannelCheck(int customerID)
        {
            _customer_id = customerID;
            _channel_id = 0;
            bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(ChannelID);
        }

        private int _channel_id = 0;
        public int ChannelID
        {
            get
            {
                int CurrentChannelID = 0;

                try
                {
                    CurrentChannelID = Convert.ToInt32(sc.BasechannelID());
                }
                catch
                {
                    CurrentChannelID = 1;
                }

                if ((CurrentChannelID != 0 && _channel_id != CurrentChannelID) || CurrentChannelID == 0)
                    _channel_id = Convert.ToInt32(getChannelID());

                return this._channel_id;
            }
        }

        private int _customer_id;
        public void CustomerID(int id)
        {
            _customer_id = id;
        }

        public string getHostName()
        {
            return bc.ChannelURL;
        }

        public string getBounceDomain()
        {
            return bc.BounceDomain;
        }

        public string getHeaderSource(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode productType)
        {
            return (bc.IsBranding != null && bc.IsBranding.Value) ? ECN_Framework_BusinessLayer.Accounts.Channel.GetByProductTypeAndID(productType, ChannelID).HeaderSource : ECN_Framework_BusinessLayer.Accounts.Channel.GetByProductTypeAndID(productType, 1).HeaderSource;
        }

        public string getFooterSource(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode productType)
        {
            return (bc.IsBranding != null && bc.IsBranding.Value) ? ECN_Framework_BusinessLayer.Accounts.Channel.GetByProductTypeAndID(productType, ChannelID).FooterSource : ECN_Framework_BusinessLayer.Accounts.Channel.GetByProductTypeAndID(productType, 1).FooterSource;
        }

        public string getChannelName()
        {
            return bc.BaseChannelName;
        }

        private string getChannelID()
        {
            if (_customer_id > 0)
            {
                return ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(_customer_id, false).BaseChannelID.ToString();
            }
            string theChannelID = "";
            // get it from the querystring
            try
            {
                if (HttpContext.Current != null)
                {
                    theChannelID = HttpContext.Current.Request.QueryString["channel"].ToString();
                }
            }
            catch { }

            // get it from the cookie
            if (theChannelID == "")
            {
                try
                {
                    theChannelID = sc.BasechannelID().ToString();
                }
                catch { }
            }

            // get it from the host
            if (theChannelID == "")
            {
                string HostName = string.Empty;
                try
                {
                    HostName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
                    theChannelID = getChannelID(HostName);
                }
                catch { }
            }

            // default it to 1
            if (theChannelID == "")
            {
                theChannelID = "1";
            }
            return theChannelID;
        }

        private string getChannelID(string HostName)
        {
            return ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll().Find(x => x.ChannelURL.ToUpper() == HostName.ToUpper()).BaseChannelID.ToString();
        }
    }
}
