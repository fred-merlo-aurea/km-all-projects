//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Collections;
//using System.Web;
//using ECN_Framework.Accounts.Entity;
//using ECN_Framework.Accounts; 
//using ECN_Framework.Accounts.Object;
//using System.Linq;

//namespace ECN_Framework.Common
//{
//    public class ECNSession
//    {
//        private const string SMART_SESSION = "ECNSession";

//        KMPlatform.Entity.User CurrentUser = new KMPlatform.Entity.User();
//        ChannelCheck cc = new ChannelCheck();

//        private int _basechannelID = 0;
//        private string _basechannelname = string.Empty;

//        private string _hostname = string.Empty;
//        private string _bouncedomain = string.Empty;

//        private string _headersource = string.Empty;
//        private string _footersource = string.Empty;


//        private Hashtable _HTheadersource = new Hashtable();
//        private Hashtable _HTfootersource = new Hashtable();

//        private List<UserPermissions> _userpermissions = new List<UserPermissions>();
//        private List<ProductFeatures> _productfeatures = new List<ProductFeatures>();

//        private int _userID = 0;
//        private string _username = string.Empty;

//        private int _customerID = 0;

//        private ECNSession()
//        {
//            RefreshSession();
//        }

//        /// <summary>
//        /// Reset the session variables when user logging into system (or) customer login from customer list(or) user login from user list
//        /// </summary>
//        public void RefreshSession()
//        {
//            SecurityCheck sc = new SecurityCheck();

//            _basechannelID = cc.ChannelID;
//            _basechannelname = cc.getChannelName();

//            _hostname = cc.getHostName();
//            _bouncedomain = cc.getBounceDomain();

//            try
//            {
//                _userID = Convert.ToInt32(sc.UserID());
//            }
//            catch
//            {
//                _userID = 0;
//            }

//            try
//            {
//                _customerID = Convert.ToInt32(sc.CustomerID());
//            }
//            catch
//            {
//                _customerID = 0;
//            }

//            CurrentUser = new KMPlatform.BusinessLogic.User().SelectUser(_userID, true);

//            _HTheadersource = new Hashtable();
//            _HTfootersource = new Hashtable();

//            //_headersource = new Hashtable();
//            //_footersource = new Hashtable();
//            //_userpermissions = new Hashtable();
//            //_productfeatures = new Hashtable();
//        }

//        public string UserName
//        {
//            get
//            {
//                //SecurityCheck sc = new SecurityCheck();

//                //int currentUserID = 0;
//                //try
//                //{
//                //    currentUserID = Convert.ToInt32(sc.CurrentUser.UserID); 
//                //}
//                //catch
//                //{
//                //    currentUserID = 0;
//                //}

//                //if ((currentUserID != 0 && _userID != currentUserID) || _username == string.Empty)
//                //{
//                //    _userID = sc.CurrentUser.UserID;
//                //    _username = sc.CurrentUser.UserName;
//                //}

//                return this.CurrentUser.UserName; 
//            }

//        }

//        public int CustomerID
//        {
//            get
//            {
//                return this.CurrentUser.DefaultClientID; 
//                //SecurityCheck sc = new SecurityCheck();

//                //int customerID = 0;
//                //try
//                //{
//                //    customerID = Convert.ToInt32(sc.CustomerID());
//                //}
//                //catch
//                //{
//                //    customerID = 0;
//                //}
//                //return customerID;
//            }
//        }

//        public int BaseChannelID
//        {
//            get
//            {
//                return this._basechannelID;
//            }
//        }

//        public string BaseChannelName
//        {
//            get
//            {
//                return this._basechannelname;
//            }
//        }

//        public string Hostname
//        {
//            get
//            {
//                return this._hostname;
//            }
//        }

//        public string BounceDomain
//        {
//            get
//            {
//                return this._bouncedomain;
//            }
//        }

//        /// <summary>
//        /// Get the Header source for specified product and store it in session
//        /// </summary>
//        /// <param name="ProductType"></param>
//        /// <returns></returns>
//        public string HeaderSource(string ProductType)
//        {

//            if (!_HTheadersource.Contains(ProductType.ToUpper()))
//            {

//                _HTheadersource.Add(ProductType.ToUpper(), cc.getHeaderSource((ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode)Enum.Parse(typeof(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode), ProductType)));
//            }

//            return _HTheadersource[ProductType.ToUpper()].ToString();

//            //SecurityCheck sc = new SecurityCheck();

//            //if (_headersource == string.Empty)
//            //{
//            //    ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetBaseChannelByID(Convert.ToInt32(sc.ChannelID()));

//            //    if (bc.IsBranding && bc.HeaderSource != null)
//            //    {
//            //        _headersource = bc.HeaderSource;
//            //    }
//            //}

//            //if (_headersource == string.Empty)
//            //{
                
//            //}

//            //return _headersource;
//        }

//        /// <summary>
//        /// Get the Footer source for specified product and store it in session
//        /// </summary>
//        /// <param name="ProductType"></param>
//        /// <returns></returns>
//        public string FooterSource(string ProductType)
//        {
//            //SecurityCheck sc = new SecurityCheck();

//            //if (_footersource == string.Empty)
//            //{
//            //    ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetBaseChannelByID(Convert.ToInt32(sc.ChannelID()));

//            //    if (bc.IsBranding && bc.FooterSource != null)
//            //    {
//            //        _footersource = bc.FooterSource;
//            //    }
//            //}

//            //return _footersource;

         

//                if (!_HTfootersource.Contains(ProductType.ToUpper()))
//                {
//                    _HTfootersource.Add(ProductType.ToUpper(), cc.getFooterSource((ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode)Enum.Parse(typeof(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode), ProductType)));
//                }

//                return _HTfootersource[ProductType.ToUpper()].ToString();
//        }

//        /// <summary>
//        /// Check User Permissions for Individual Actions in the specified product  and store it in session
//        /// </summary>
//        /// <param name="ProductName"></param>
//        /// <param name="ActionCode"></param>
//        /// <returns></returns>

//        //public bool HasPermission(string ProductName, string ActionCode)
//        //{
//        //    //return ECN_Framework.Accounts.Object.UserPermissions.HasPermission(_userID, ProductName, ActionCode);

//        //    if (_userpermissions.Count == 0)
//        //    {
//        //        _userpermissions = ECN_Framework.Accounts.Object.UserPermissions.Get(_userID);
//        //    }
//        //    UserPermissions up = _userpermissions.SingleOrDefault(x => x.ProductName.ToLower() == ProductName.ToLower() && x.ActionCode.ToLower()  == ActionCode.ToLower());

//        //    return (up == null? false:up.Active);
//        //}

//        /// <summary>
//        /// Check Customer Permissions for product features  and store it in session
//        /// </summary>
//        /// <param name="ProductName"></param>
//        /// <param name="ActionCode"></param>
//        /// <returns></returns>

//        public bool HasProductFeature(string ProductName, string FeatureName)
//        {

//            if (_productfeatures.Count == 0)
//            {
//                _productfeatures = ECN_Framework.Accounts.Object.ProductFeatures.Get(_userID);
//            }

//            ProductFeatures pf = _productfeatures.SingleOrDefault(x => x.ProductName.ToLower() == ProductName.ToLower() && x.FeatureName.ToLower() == FeatureName.ToLower());

//            return (pf == null ? false : pf.Active);

//            //return false;

//            //return ECN_Framework.Accounts.Object.ProductFeatures.HasFeature(_userID, ProductName, FeatureName);

//            //if (_productfeatures.Count == 0)
//            //{
//            //    DataTable dtproducts = ECN_Framework_DataLayer.DataFunctions.GetDataTable("SELECT p.productname, pd.productdetailName as featurename, cp.Active FROM CustomerProduct cp JOIN ProductDetails pd ON cp.ProductDetailID = pd.ProductDetailID JOIN Products p ON pd.ProductID = p.ProductID JOIN Users U on U.customerID = cp.customerID where userID = " + _userID, System.Configuration.ConfigurationManager.AppSettings["act"]);

//            //    foreach (DataRow drproducts in dtproducts.Rows)
//            //    {
//            //        if (!_productfeatures.Contains(drproducts["productname"].ToString().ToLower() + "," + drproducts["FeatureName"].ToString().ToLower()))
//            //        {
//            //            _productfeatures.Add(drproducts["productname"].ToString().ToLower() + "," + drproducts["FeatureName"].ToString().ToLower(), (drproducts["active"].ToString().ToLower() == "y" ? true : false));
//            //        }
//            //    }
//            //}

//            //if (_productfeatures.Contains(ProductName.ToLower() + "," + FeatureName.ToLower()))
//            //    return Convert.ToBoolean(_productfeatures[ProductName.ToLower() + "," + FeatureName.ToLower()]);
//            //else
//            //    return false;

//            //return false; 
//        }

//        /// <summary>
//        /// Create the Singleton Session object if not exists.
//        /// </summary>
//        /// <returns></returns>
//        public static ECNSession CurrentSession()
//        {
//            SecurityCheck sc = new SecurityCheck();

//            ECNSession _ecnSession;
//            if (null == HttpContext.Current.Session[SMART_SESSION + "_" + sc.UserID().ToString()])
//            {
//                _ecnSession = new ECNSession();
//                System.Web.HttpContext.Current.Session[SMART_SESSION + "_" + sc.UserID().ToString()] = _ecnSession;
//            }
//            else
//            {
//                _ecnSession = (ECNSession)System.Web.HttpContext.Current.Session[SMART_SESSION + "_" + sc.UserID().ToString()];

//                string scChannelID = sc.ChannelID();

//                if (_ecnSession._basechannelID != (scChannelID == "" ? 1 : Convert.ToInt32(scChannelID)))
//                    _ecnSession.RefreshSession();
//            }

//            return _ecnSession;
//        }
//    }
//}
