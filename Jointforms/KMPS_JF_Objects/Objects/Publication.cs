using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Collections;
using System.Linq;
using System.Configuration;
using System.Web;
using KM.Common;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class Publication
    {
        #region Private & Public variables

        private int _PubID = 0;
        private string _PubName = string.Empty;
        private string _PubCode = string.Empty;
        private int _ECNCustomerID = 0;
        private int _ECNDefaultGroupID = 0;
        private int _ECNSFID = 0;

        private string[] _CSS;
        private string _HeaderHTML = string.Empty;
        private string _FooterHTML = string.Empty;
        private string _HomePageDesc = string.Empty;
        private string _Step2PageDesc = string.Empty;
        private string _NewPageDesc = string.Empty;
        private string _RenewPageDesc = string.Empty;
        private string _LoginPageDesc = string.Empty;
        private string _ForgotPasswordHTML = string.Empty;
        
        private string _LoginVerfication = string.Empty;
        private string _ThankYouPageLink = string.Empty;
        private string _ThankYouPageHTML = string.Empty;
        private string _CustomerServicePageHTML = string.Empty;
        private string _FAQPageHTML = string.Empty;

        private bool _IsDefaultGroupEnabled = false;
        private bool _IsActive = false;
        private bool _ShowNewSubLink = false;
        private bool _ShowRenewSubLink = false;
        private bool _ShowCustomerServiceLink = false;
        private bool _ShowTradeShowLink = false;
        private bool _ShowNewsletters = false;

        private DateTime _DateAdded = DateTime.Now;
        private DateTime _DateModified = DateTime.Now;
        private string _AddedBy = string.Empty;
        private string _ModifiedBy = string.Empty;

        private bool _HasPaid = false;
        private string _PayflowAccount = string.Empty;
        private string _PayflowPassword = string.Empty;
        private string _PayflowSignature = string.Empty;
        private string _PayflowPageStyle = string.Empty;

        private string _PayflowPartner = string.Empty;
        private string _PayflowVendor = string.Empty;

        private string _AuthorizeDotNetAccount = string.Empty;
        private string _AuthorizeDotNetSignature = string.Empty;
        private Enums.CCProcessors _paymentGateway;

        private bool _ProcessExternal = false;
        private string _ProcessExternalURL = string.Empty;

        private string _NewSubscriptionHeader = string.Empty;
        private string _NewSubscriptionLink = string.Empty;
        private string _ManageSubscriptionHeader = string.Empty;
        private string _ManageSubscriptionLink = string.Empty;
        private string _RequiredFieldHTML = string.Empty;
        private string _NewsletterHeaderHTML = string.Empty;

        private bool _IsCacheClear = false;
        private bool _DisableSubcriberLogin = false;
        private bool _DisablePassword = false;
        private bool _DisableEmail = false;
        private string _MailingLabel = string.Empty;

        private string _Width = string.Empty;
        private string _ColumnFormat = string.Empty;
        private string _redirectLink = string.Empty;
        private string _redirectHTML = string.Empty;

        private string _MagCoverImage = string.Empty;
        private bool _checkSubscriber = true;

        private string _paidThankYouPageHTML = string.Empty;
        private string _paidResponseEmail = string.Empty;
        private string _compResponseEmail = string.Empty;
        private string _paidPageFromEmail = string.Empty;
        private string _paidFormResponseEmailSubject = string.Empty;
        private string _paidPageFromName = string.Empty;
        private bool _showShippingAddress = false;
        private string _paidPageThankYouLink = string.Empty;
        private string _pageTitle = string.Empty;

        private bool _repeatEmails = false;
        private string _repeatEmailsMessage = "";

        private List<PubCountry> _Countries;
        private List<int> _autogroups;

        #endregion

        #region public variables

        public List<int> AutoGroups
        {
            get { return _autogroups; }
            set { _autogroups = value; }
        }
        public int PubID
        {
            get { return _PubID; }
            set { _PubID = value; }
        }
        public string PubName
        {
            get { return _PubName; }
            set { _PubName = value; }
        }
        public string PubCode
        {
            get { return _PubCode; }
            set { _PubCode = value; }
        }
        public int ECNCustomerID
        {
            get { return _ECNCustomerID; }
            set { _ECNCustomerID = value; }
        }
        public int ECNDefaultGroupID
        {
            get { return _ECNDefaultGroupID; }
            set { _ECNDefaultGroupID = value; }
        }
        public int ECNSFID
        {
            get { return _ECNSFID; }
            set { _ECNSFID = value; }
        }

        public string[] CSS
        {
            get { return _CSS; }
            set { _CSS = value; }
        }
        public string HeaderHTML
        {
            get { return _HeaderHTML; }
            set { _HeaderHTML = value; }
        }
        public string FooterHTML
        {
            get { return _FooterHTML; }
            set { _FooterHTML = value; }
        }
        public string HomePageDesc
        {
            get { return _HomePageDesc; }
            set { _HomePageDesc = value; }
        }

        public string Step2PageDesc
        {
            get { return _Step2PageDesc; }
            set { _Step2PageDesc = value; }
        }

        public string NewPageDesc
        {
            get { return _NewPageDesc; }
            set { _NewPageDesc = value; }
        }
        public string RenewPageDesc
        {
            get { return _RenewPageDesc; }
            set { _RenewPageDesc = value; }
        }
        public string LoginPageDesc
        {
            get { return _LoginPageDesc; }
            set { _LoginPageDesc = value; }
        }

        public string ForgotPasswordHTML
        {
            get { return _ForgotPasswordHTML; }
            set { _ForgotPasswordHTML = value; }
        }

        public string LoginVerfication
        {
            get { return _LoginVerfication; }
            set { _LoginVerfication = value; }
        }
        public string ThankYouPageLink
        {
            get { return _ThankYouPageLink; }
            set { _ThankYouPageLink = value; }
        }
        public string ThankYouPageHTML
        {
            get { return _ThankYouPageHTML; }
            set { _ThankYouPageHTML = value; }
        }

        public string CustomerServicePageHTML
        {
            get { return _CustomerServicePageHTML; }
            set { _CustomerServicePageHTML = value; }
        }

        public string FAQPageHTML
        {
            get { return _FAQPageHTML; }
            set { _FAQPageHTML = value; }
        }

        public bool IsDefaultGroupEnabled
        {
            get { return _IsDefaultGroupEnabled; }
            set { _IsDefaultGroupEnabled = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        public bool ShowNewSubLink
        {
            get { return _ShowNewSubLink; }
            set { _ShowNewSubLink = value; }
        }
        public bool ShowRenewSubLink
        {
            get { return _ShowRenewSubLink; }
            set { _ShowRenewSubLink = value; }
        }
        public bool ShowCustomerServiceLink
        {
            get { return _ShowCustomerServiceLink; }
            set { _ShowCustomerServiceLink = value; }
        }
        public bool ShowTradeShowLink
        {
            get { return _ShowTradeShowLink; }
            set { _ShowTradeShowLink = value; }
        }
        public bool ShowNewsletters
        {
            get { return _ShowNewsletters; }
            set { _ShowNewsletters = value; }
        }

        public DateTime DateAdded
        {
            get { return _DateAdded; }
            set { _DateAdded = value; }
        }
        public DateTime DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }
        public string AddedBy
        {
            get { return _AddedBy; }
            set { _AddedBy = value; }
        }
        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }

        public bool HasPaid
        {
            get { return _HasPaid; }
            set { _HasPaid = value; }
        }

        public string PayflowAccount
        {
            get { return _PayflowAccount; }
            set { _PayflowAccount = value; }
        }

        public string PayflowPassword
        {
            get { return _PayflowPassword; }
            set { _PayflowPassword = value; }
        }

        public string PayflowSignature
        {
            get { return _PayflowSignature; }
            set { _PayflowSignature = value; }
        }

        public string PayflowPageStyle
        {
            get { return _PayflowPageStyle; }
            set { _PayflowPageStyle = value; }
        }

        public string PayflowPartner
        {
            get { return _PayflowPartner; }
            set { _PayflowPartner = value; }
        }

        public string PayflowVendor
        {
            get { return _PayflowVendor; }
            set { _PayflowVendor = value; }
        }

        public bool ProcessExternal
        {
            get { return _ProcessExternal; }
            set { _ProcessExternal = value; }
        }

        public string ProcessExternalURL
        {
            get { return _ProcessExternalURL; }
            set { _ProcessExternalURL = value; }
        }

        public string AuthorizeDotNetAccount
        {
            get { return _AuthorizeDotNetAccount; }
            set { _AuthorizeDotNetAccount = value; }
        }

        public string AuthorizeDotNetSignature
        {
            get { return _AuthorizeDotNetSignature; }
            set { _AuthorizeDotNetSignature = value; }
        }

        public Enums.CCProcessors PaymentGateway
        {
            get { return _paymentGateway; }
            set { _paymentGateway = value; }
        }

        public string NewSubscriptionHeader
        {
            get { return _NewSubscriptionHeader; }
            set { _NewSubscriptionHeader = value; }
        }

        public string ManageSubscriptionHeader
        {
            get { return _ManageSubscriptionHeader; }
            set { _ManageSubscriptionHeader = value; }
        }

        public string ManageSubscriptionLink
        {
            get { return _ManageSubscriptionLink; }
            set { _ManageSubscriptionLink = value; }
        }

        public string RequiredFieldHTML
        {
            get { return _RequiredFieldHTML; }
            set { _RequiredFieldHTML = value; }
        }

        public string NewsletterHeaderHTML
        {
            get { return _NewsletterHeaderHTML; }
            set { _NewsletterHeaderHTML = value; }
        }

        public string NewSubscriptionLink
        {
            get { return _NewSubscriptionLink; }
            set { _NewSubscriptionLink = value; }
        }

        public bool IsCacheClear
        {
            get { return _IsCacheClear; }
            set { _IsCacheClear = value; }
        }

        public bool DisableSubcriberLogin
        {
            get { return _DisableSubcriberLogin; }
            set { _DisableSubcriberLogin = value; }
        }

        public bool DisablePassword
        {
            get { return _DisablePassword; }
            set { _DisablePassword = value; }
        }

        public bool DisableEmail
        {
            get { return _DisableEmail; }
            set { _DisableEmail = value; }
        }

        public string MailingLabel
        {
            get { return _MailingLabel; }
            set { _MailingLabel = value; }
        }

        public string Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public string ColumnFormat
        {
            get { return _ColumnFormat; }
            set { _ColumnFormat = value; }
        }

        public string RedirectLink
        {
            get { return _redirectLink; }
            set { _redirectLink = value; }
        }

        public string RedirectHTML
        {
            get { return _redirectHTML; }
            set { _redirectHTML = value; }
        }

        public string MagCoverImage
        {
            get { return _MagCoverImage; }
            set { _MagCoverImage = value; }
        }

        public bool CheckSubscriber
        {
            get { return _checkSubscriber; }
            set { _checkSubscriber = value; }
        }

        public string PaidThankYouPageHTML
        {
            get { return _paidThankYouPageHTML; }
            set { _paidThankYouPageHTML = value; }
        }

        public string PaidResponseEmail
        {
            get { return _paidResponseEmail; }
            set { _paidResponseEmail = value; }
        }

        public string PaidPageFromEmail
        {
            get { return _paidPageFromEmail; }
            set { _paidPageFromEmail = value; }
        }

        public string PaidFormResponseEmailSubject
        {
            get { return _paidFormResponseEmailSubject; }
            set { _paidFormResponseEmailSubject = value; }
        }

        public string PaidPageFromName
        {
            get { return _paidPageFromName; }
            set { _paidPageFromName = value; }
        }

        public bool ShowShippingAddress
        {
            get { return _showShippingAddress; }
            set { _showShippingAddress = value; }
        }

        public string PaidPageThankyouLink
        {
            get { return _paidPageThankYouLink; }
            set { _paidPageThankYouLink = value; }
        }

        public string CompResponseEmailHTML
        {
            get { return _compResponseEmail; }
            set { _compResponseEmail = value; }
        }

        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; }
        }

        public bool RepeatEmails
        {
            get { return _repeatEmails; }
            set { _repeatEmails = value; }
        }

        public string RepeatEmailsMessage
        {
            get { return _repeatEmailsMessage; }
            set { _repeatEmailsMessage = value; }     
        }

        public List<PubCountry> Countries
        {
            get
            {
                if (_Countries == null)
                    _Countries = PubCountry.GetCountriesForPub(this._PubID);

                return _Countries;
            }
            set { _Countries = value; }
        }

        #endregion

        #region public methods

        public string GetCSS()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConfigurationManager.AppSettings["Pub_CSS"].ToString());

            sb = sb.Replace("%%PBGColor%%", (this.CSS[0] == string.Empty ? "#e0e0e0" : this.CSS[0]));
            sb = sb.Replace("%%PageFontSize%%", (this.CSS[4] == string.Empty ? "12px" : this.CSS[4]));
            sb = sb.Replace("%%PageFont%%", (this.CSS[3] == string.Empty ? "Arial, Helvetica, sans-serif" : this.CSS[3]));
            sb = sb.Replace("%%FBGColor%%", (this.CSS[1] == string.Empty ? "#ffffff" : this.CSS[1]));
            sb = sb.Replace("%%PageBorder%%", (this.CSS[2] == string.Empty ? "2" : this.CSS[2]));

            sb = sb.Replace("%%CBGColor%%", (this.CSS[5] == string.Empty ? "#EDEDF5" : this.CSS[5]));
            sb = sb.Replace("%%CatFontSize%%", (this.CSS[6] == string.Empty ? "12px" : this.CSS[6]));
            sb = sb.Replace("%%CFColor%%", (this.CSS[7] == string.Empty ? "black" : this.CSS[7]));

            sb = sb.Replace("%%QFSize%%", (this.CSS[8] == string.Empty ? "12px" : this.CSS[8]));
            sb = sb.Replace("%%QFColor%%", (this.CSS[9] == string.Empty ? "#840000" : this.CSS[9]));
            sb = sb.Replace("%%QFBold%%", (this.CSS[10] == string.Empty ? "bold" : this.CSS[10]));

            sb = sb.Replace("%%AFSize%%", (this.CSS[11] == string.Empty ? "12px" : this.CSS[11]));
            sb = sb.Replace("%%AFColor%%", (this.CSS[12] == string.Empty ? "black" : this.CSS[12]));
            sb = sb.Replace("%%AFBold%%", (this.CSS[13] == string.Empty ? "normal" : this.CSS[13]));

            sb = sb.Replace("%%FormWidth%%", this.Width);
            return sb.ToString();
        }

        public PubCountry GetPubCountry(int CountryID)
        {
            return (PubCountry)this.Countries.SingleOrDefault(c => c.CountryID == CountryID);
        }

        public PubCountry GetPubCountry(string CountryName)
        {
            return (PubCountry)this.Countries.SingleOrDefault(c => c.CountryName.ToLower() == CountryName.ToLower());
        }
        #endregion

        #region Static Methods

        public static Publication GetPublicationbyID(int pubID, string pubCode) 
        {
            Publication pub = null;

            if (CacheUtil.IsCacheEnabled())
            {
                pub = (Publication)CacheUtil.GetFromCache("Pub_" + pubCode.ToUpper(), "JOINTFORMS");

                if (pub == null)
                {
                    pub = GetData(pubID, pubCode);
                    CacheUtil.AddToCache("Pub_" + pub.PubCode.ToUpper(), pub, "JOINTFORMS");
                }

                return pub;
            }
            else
            {
                return GetData(pubID, pubCode);
            }
        }

        private static Publication GetData(int pubID, string pubCode)
        {
            Publication pub = null;

            SqlCommand cmd = new SqlCommand("select * from Publications p  with (NOLOCK) where p.PubID = @pubID or p.PubCode = @pubCode");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@pubID", SqlDbType.Int));
            cmd.Parameters["@pubID"].Value = pubID;

            cmd.Parameters.Add(new SqlParameter("@pubCode", SqlDbType.VarChar, 15));
            cmd.Parameters["@pubCode"].Value = pubCode;

            DataTable dtPub = DataFunctions.GetDataTable(cmd);

            if (dtPub.Rows.Count > 0)
            {
                pub = new Publication();
                pub.PubID = Convert.ToInt32(dtPub.Rows[0]["PubID"].ToString());

                pub.PubName = dtPub.Rows[0]["PubName"].ToString();
                pub.PubCode = dtPub.Rows[0]["PubCode"].ToString();

                if (!dtPub.Rows[0].IsNull("ECNCustomerID"))
                    pub.ECNCustomerID = Convert.ToInt32(dtPub.Rows[0]["ECNCustomerID"]);

                if (!dtPub.Rows[0].IsNull("ECNDefaultGroupID"))
                    pub.ECNDefaultGroupID = Convert.ToInt32(dtPub.Rows[0]["ECNDefaultGroupID"]);

                pub.IsDefaultGroupEnabled = Convert.ToBoolean(dtPub.Rows[0]["IsDefaultGroupEnabled"]);

                if (!dtPub.Rows[0].IsNull("ECNSFID"))
                    pub.ECNSFID = Convert.ToInt32(dtPub.Rows[0]["ECNSFID"]);

                pub.IsActive = Convert.ToBoolean(dtPub.Rows[0]["IsActive"]);

                pub.CSS = dtPub.Rows[0]["CSS"].ToString().Split('|');
                pub.HeaderHTML = dtPub.Rows[0]["HeaderHTML"].ToString();
                pub.HomePageDesc = dtPub.Rows[0]["HomePageDesc"].ToString();
                pub.Step2PageDesc = dtPub.Rows[0]["Step2PageDesc"].ToString();
                pub.NewPageDesc = dtPub.Rows[0]["NewPageDesc"].ToString();
                pub.RenewPageDesc = dtPub.Rows[0]["RenewPageDesc"].ToString();

                pub.LoginPageDesc = dtPub.Rows[0]["LoginPageDesc"].ToString();
                pub.ForgotPasswordHTML = dtPub.Rows[0]["ForgotPasswordHTML"].ToString();
                pub.LoginVerfication = dtPub.Rows[0]["LoginVerfication"].ToString();
                pub.ThankYouPageLink = dtPub.Rows[0]["ThankYouPageLink"].ToString();
                pub.ThankYouPageHTML = dtPub.Rows[0]["ThankYouPageHTML"].ToString();
                pub.RenewPageDesc = dtPub.Rows[0]["RenewPageDesc"].ToString();

                pub.ShowNewSubLink = Convert.ToBoolean(dtPub.Rows[0]["ShowNewSubLink"]);
                pub.ShowRenewSubLink = Convert.ToBoolean(dtPub.Rows[0]["ShowRenewSubLink"]);
                pub.ShowCustomerServiceLink = Convert.ToBoolean(dtPub.Rows[0]["ShowCustomerServiceLink"]);
                pub.ShowTradeShowLink = Convert.ToBoolean(dtPub.Rows[0]["ShowTradeShowLink"]);
                pub.ShowNewsletters = Convert.ToBoolean(dtPub.Rows[0]["ShowNewsletters"]);

                pub.CustomerServicePageHTML = dtPub.Rows[0]["CustomerServicePageHTML"].ToString();
                pub.FAQPageHTML = dtPub.Rows[0]["FAQPageHTML"].ToString();
                pub.FooterHTML = dtPub.Rows[0]["FooterHTML"].ToString();

                pub.DateAdded = Convert.ToDateTime(dtPub.Rows[0]["DateAdded"]);
                pub.DateModified = Convert.ToDateTime(dtPub.Rows[0]["DateModified"]);
                pub.AddedBy = dtPub.Rows[0]["AddedBy"].ToString();
                pub.ModifiedBy = dtPub.Rows[0]["ModifiedBy"].ToString();

                pub.HasPaid = Convert.ToBoolean(dtPub.Rows[0].IsNull("HasPaid") ? false : dtPub.Rows[0]["HasPaid"]);
                pub.PayflowAccount = dtPub.Rows[0]["PayflowAccount"].ToString();
                pub.PayflowPassword = dtPub.Rows[0]["PayflowPassword"].ToString();
                pub.PayflowSignature = dtPub.Rows[0]["PayflowSignature"].ToString();
                pub.PayflowPageStyle = dtPub.Rows[0]["PayflowPageStyle"].ToString();

                pub.PayflowPartner = dtPub.Rows[0]["PayflowPartner"].ToString();
                pub.PayflowVendor = dtPub.Rows[0]["PayflowVendor"].ToString();

                pub.AuthorizeDotNetAccount = !dtPub.Rows[0].IsNull("AuthorizeDotNetAccount") ? dtPub.Rows[0]["AuthorizeDotNetAccount"].ToString() : string.Empty;
                pub.AuthorizeDotNetSignature = !dtPub.Rows[0].IsNull("AuthorizeDotNetSignature") ? dtPub.Rows[0]["AuthorizeDotNetSignature"].ToString() : string.Empty;
                //pub.MagCoverImage = !dtPub.Rows[0].IsNull("MagCoverImage") ? dtPub.Rows[0]["MagCoverImage"].ToString() : string.Empty;

                pub.PaidResponseEmail = !dtPub.Rows[0].IsNull("PaidResponseEmail") ? dtPub.Rows[0]["PaidResponseEmail"].ToString() : string.Empty;
                pub.PaidThankYouPageHTML = !dtPub.Rows[0].IsNull("PaidThankYouPageHTML") ? dtPub.Rows[0]["PaidThankYouPageHTML"].ToString() : string.Empty;

                try
                {
                    pub.PaymentGateway = (Enums.CCProcessors)Enum.Parse(typeof(Enums.CCProcessors), dtPub.Rows[0]["PaymentGateway"].ToString());
                }
                catch
                {
                    pub.PaymentGateway = Enums.CCProcessors.Paypal;
                }

                pub.ProcessExternal = Convert.ToBoolean(dtPub.Rows[0].IsNull("ProcessExternal") ? false : dtPub.Rows[0]["ProcessExternal"]);
                pub.ProcessExternalURL = dtPub.Rows[0]["ProcessExternalURL"].ToString();

                pub.NewSubscriptionHeader = dtPub.Rows[0]["NewSubscriptionHeader"].ToString();
                pub.NewSubscriptionLink = dtPub.Rows[0]["NewSubscriptionLink"].ToString();
                pub.ManageSubscriptionHeader = dtPub.Rows[0]["ManageSubscriptionHeader"].ToString();
                pub.ManageSubscriptionLink = dtPub.Rows[0]["ManageSubscriptionLink"].ToString();
                pub.RequiredFieldHTML = dtPub.Rows[0]["RequiredFieldHTML"].ToString();
                pub.NewsletterHeaderHTML = dtPub.Rows[0]["NewsletterHeaderHTML"].ToString();

                pub.DisableSubcriberLogin = Convert.ToBoolean(dtPub.Rows[0].IsNull("DisableSubcriberLogin") ? false : dtPub.Rows[0]["DisableSubcriberLogin"]);
                pub.DisablePassword = Convert.ToBoolean(dtPub.Rows[0].IsNull("DisablePassword") ? false : dtPub.Rows[0]["DisablePassword"]);
                pub.DisableEmail = Convert.ToBoolean(dtPub.Rows[0].IsNull("DisableEmail") ? false : dtPub.Rows[0]["DisableEmail"]);
                pub.Countries = PubCountry.GetCountriesForPub(pub.PubID);
                pub.MailingLabel = dtPub.Rows[0].IsNull("MailingLabel") ? string.Empty : dtPub.Rows[0]["MailingLabel"].ToString();

                pub.Width = dtPub.Rows[0]["Width"] == null ? string.Empty : dtPub.Rows[0]["Width"].ToString();
                pub.ColumnFormat = dtPub.Rows[0]["ColumnFormat"] == null ? "" : dtPub.Rows[0]["ColumnFormat"].ToString();
                pub.RedirectLink = dtPub.Rows[0].IsNull("RedirectLink") ? string.Empty : dtPub.Rows[0]["RedirectLink"].ToString();
                pub.RedirectHTML = dtPub.Rows[0].IsNull("RedirectHTML") ? string.Empty : dtPub.Rows[0]["RedirectHTML"].ToString();
                pub.CheckSubscriber = dtPub.Rows[0].IsNull("CheckSubscriber") ? true : Convert.ToBoolean(dtPub.Rows[0]["CheckSubscriber"].ToString());
                pub.PaidPageFromEmail = dtPub.Rows[0].IsNull("PaidPageFromEmail") ? string.Empty : dtPub.Rows[0]["PaidPageFromEmail"].ToString();
                pub.PaidFormResponseEmailSubject = dtPub.Rows[0].IsNull("PaidFormResponseEmailSubject") ? string.Empty : dtPub.Rows[0]["PaidFormResponseEmailSubject"].ToString();
                pub.PaidPageFromName = dtPub.Rows[0].IsNull("PaidPageFromName") ? string.Empty : dtPub.Rows[0]["PaidPageFromName"].ToString();
                pub.ShowShippingAddress = dtPub.Rows[0].IsNull("ShowShippingAddress") ? false : Convert.ToBoolean(dtPub.Rows[0]["ShowShippingAddress"].ToString());
                pub.PaidPageThankyouLink = dtPub.Rows[0].IsNull("PaidPageThankyouLink") ? string.Empty : dtPub.Rows[0]["PaidPageThankyouLink"].ToString();

                pub.CompResponseEmailHTML = dtPub.Rows[0].IsNull("CompResponseEmailHTML") ? string.Empty : dtPub.Rows[0]["CompResponseEmailHTML"].ToString();
                pub.PageTitle = dtPub.Rows[0].IsNull("PageTitle") ? string.Empty : dtPub.Rows[0]["PageTitle"].ToString();
                pub.RepeatEmails = dtPub.Rows[0].IsNull("RepeatEmails") ? false : Convert.ToBoolean(dtPub.Rows[0]["RepeatEmails"].ToString());

                pub.RepeatEmailsMessage = "The Email Address %%EmailAddress%% is already registered. Please use a different email address or Click <a href='subscription.aspx?pubcode=" + pub.PubCode + "&step=login" + "'>here</a> to manage your account.";

            }
            else
            {
                throw new ApplicationException("Invalid Publication.");
            }

            return pub;
        }

        public static Dictionary<int, int> GetAutoSubscriptions(int pubID)
        {
            SqlCommand cmdAutoSubs = new SqlCommand("spGetPubAutoSubscription");
            cmdAutoSubs.CommandType = CommandType.StoredProcedure;
            cmdAutoSubs.Parameters.Add(new SqlParameter("@pubID", pubID.ToString()));

            Dictionary<int, int> dictAutoSubs = new Dictionary<int, int>();

            if (CacheUtil.IsCacheEnabled())
            {
                dictAutoSubs = (Dictionary<int, int>)CacheUtil.GetFromCache("PUBAUTOSUBSCRIPTION_" + pubID.ToString(), "JOINTFORMS");

                if (dictAutoSubs == null)
                {
                    dictAutoSubs = new Dictionary<int, int>();

                    DataTable dtAutoSubs = DataFunctions.GetDataTable(cmdAutoSubs);

                    foreach (DataRow dr in dtAutoSubs.Rows)
                        dictAutoSubs.Add(Convert.ToInt32(dr["PubAutoGroupID"]), Convert.ToInt32(dr["PubAutoCustID"]));

                    CacheUtil.AddToCache("PUBAUTOSUBSCRIPTION_" + pubID.ToString(), dictAutoSubs, "JOINTFORMS");
                }

                return dictAutoSubs;
            }
            else
            {
                DataTable dtAutoSubs = DataFunctions.GetDataTable(cmdAutoSubs);

                foreach (DataRow dr in dtAutoSubs.Rows)
                    dictAutoSubs.Add(Convert.ToInt32(dr["PubAutoGroupID"]), Convert.ToInt32(dr["PubAutoCustID"]));

                return dictAutoSubs;
            }
        }

        public static DataTable GetPublicationEvents(int pubID)
        {
            SqlCommand cmdGetPublicationEvents = new SqlCommand("select * from PubEvents with (NOLOCK) where isactive=1 and pubID = @pubID");
            cmdGetPublicationEvents.CommandType = CommandType.Text;
            cmdGetPublicationEvents.Parameters.Add(new SqlParameter("@pubID", pubID));

            DataTable dtPublicationEvents = null;

            if (CacheUtil.IsCacheEnabled())
            {
                dtPublicationEvents = (DataTable)CacheUtil.GetFromCache("PUBEVENTS_" + pubID.ToString(), "JOINTFORMS");

                if (dtPublicationEvents == null)
                {
                    dtPublicationEvents = DataFunctions.GetDataTable(cmdGetPublicationEvents);

                    CacheUtil.AddToCache("PUBEVENTS_" + pubID.ToString(), dtPublicationEvents, "JOINTFORMS");
                }
                return dtPublicationEvents;
            }
            else
            {
                return DataFunctions.GetDataTable(cmdGetPublicationEvents);
            }

        }

        public static DataTable GetPublicationCustomPages(int pubID)
        {
            SqlCommand GetPublicationCustomPages = new SqlCommand("select * from PubCustomPages  with (NOLOCK) where isactive=1 and pubID = @pubID");
            GetPublicationCustomPages.CommandType = CommandType.Text;
            GetPublicationCustomPages.Parameters.Add(new SqlParameter("@pubID", @pubID));

            DataTable dtPublicationCustomPages = null;

            if (CacheUtil.IsCacheEnabled())
            {
                dtPublicationCustomPages = (DataTable)CacheUtil.GetFromCache("PUBLICATIONCUSTOMPAGES_" + pubID.ToString(), "JOINTFORMS");

                if (dtPublicationCustomPages == null)
                {
                    dtPublicationCustomPages = DataFunctions.GetDataTable(GetPublicationCustomPages);

                    CacheUtil.AddToCache("PUBLICATIONCUSTOMPAGES_" + pubID.ToString(), dtPublicationCustomPages, "JOINTFORMS");
                }

                return dtPublicationCustomPages;
            }
            else
            {
                return DataFunctions.GetDataTable(GetPublicationCustomPages);
            }
        }

        public static List<PubForm> GetPubForms(int pubID)
        {
            SqlCommand cmdGetPubForms = new SqlCommand("select PFID, ShowPrint, PubID from pubforms  with (NOLOCK) where PubID = @pubID");
            cmdGetPubForms.CommandType = CommandType.Text;
            cmdGetPubForms.Parameters.Add(new SqlParameter("@pubID", pubID));
            DataTable dt = DataFunctions.GetDataTable(cmdGetPubForms);
            List<PubForm> pfList = new List<PubForm>();

            foreach (DataRow dr in dt.Rows)
            {
                PubForm pf = new PubForm();
                pf.PFID = Convert.ToInt32(dr["PFID"].ToString());
                pf.ShowPrint = Convert.ToBoolean(dr["ShowPrint"].ToString());
                pfList.Add(pf);
            }

            return pfList;
        }


        #endregion
    }
}
