//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Collections;
//using System.Web;

//namespace ecn.common.classes
//{
//    public class ecnSession
//    {
//        private const string SMART_SESSION = "ECNSession";
//        ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();

//        private int _basechannelID = 0;
//        private string _basechannelname = string.Empty;

//        private string _hostname = string.Empty;
//        private string _bouncedomain = string.Empty;

//        private Hashtable _headersource = new Hashtable();
//        private Hashtable _footersource = new Hashtable();
//        private Hashtable _userpermissions = new Hashtable();
//        private Hashtable _productfeatures = new Hashtable();

//        private int _userID = 0;
//        private string _username = string.Empty;

//        private ecnSession()
//        {
//            RefreshSession();
//        }

//        /// <summary>
//        /// Reset the session variables when user logging into system (or) customer login from customer list(or) user login from user list
//        /// </summary>
//        public void RefreshSession()
//        {
//            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

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

//            _username = string.Empty;
           
//            _headersource = new Hashtable();
//            _footersource = new Hashtable();
//            _userpermissions = new Hashtable();
//            _productfeatures = new Hashtable();
//        }

//        public string UserName
//        {
//            get
//            {
//                ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

//                int currentUserID = 0;
//                try
//                {
//                    currentUserID = Convert.ToInt32(sc.UserID());
//                }
//                catch
//                {
//                    currentUserID = 0;
//                }

//                if ((currentUserID != 0 && _userID != currentUserID) || _username == string.Empty)
//                {
//                    _userID = currentUserID;
//                    _username = DataFunctions.GetUser(currentUserID);
//                }
                
//                return this._username;
//            }

//        }

//        public int CustomerID
//        {
//            get
//            {
//                ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

//                int customerID = 0;
//                try
//                {
//                    customerID = Convert.ToInt32(sc.CustomerID());
//                }
//                catch
//                {
//                    customerID = 0;
//                }
//                return customerID;
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
           
//            if (!_headersource.Contains(ProductType.ToUpper()))
//            {
                
//                _headersource.Add(ProductType.ToUpper(), cc.getHeaderSource(ProductType));
//            }

//            return _headersource[ProductType.ToUpper()].ToString();
//        }

//        /// <summary>
//        /// Get the Footer source for specified product and store it in session
//        /// </summary>
//        /// <param name="ProductType"></param>
//        /// <returns></returns>
//        public string FooterSource(string ProductType)
//        {

//            if (!_footersource.Contains(ProductType.ToUpper()))
//            {
//                _footersource.Add(ProductType.ToUpper(), cc.getFooterSource(ProductType));
//            }

//            return _footersource[ProductType.ToUpper()].ToString();
//        }

//        /// <summary>
//        /// Check User Permissions for Individual Actions in the specified product  and store it in session
//        /// </summary>
//        /// <param name="ProductName"></param>
//        /// <param name="ActionCode"></param>
//        /// <returns></returns>

//        public bool HasPermission(string ProductName, string ActionCode)
//        {
//            if (_userpermissions.Count == 0)
//            {
//                DataTable dtUserActions = DataFunctions.GetDataTable("select p.productname, a.actioncode, UA.active from useractions UA join Action a on ua.actionID = a.actionID join product p on p.productID = a.productID where UserID=" + _userID, System.Configuration.ConfigurationManager.AppSettings["act"]);

//                foreach (DataRow drActions in dtUserActions.Rows)
//                {
//                    if (!_userpermissions.Contains(drActions["productname"].ToString().ToLower() + "," + drActions["actioncode"].ToString().ToLower()))
//                    {
//                        _userpermissions.Add(drActions["productname"].ToString().ToLower() + "," + drActions["actioncode"].ToString().ToLower(), (drActions["active"].ToString().ToLower() == "y" ? true : false));
//                    }
//                }
//            }

//            if (_userpermissions.Contains(ProductName.ToLower() + "," + ActionCode.ToLower()))
//                return Convert.ToBoolean(_userpermissions[ProductName.ToLower() + "," + ActionCode.ToLower()]);
//            else
//                return false;
//        }

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
//                DataTable dtproducts = DataFunctions.GetDataTable("SELECT p.productname, pd.productdetailName as featurename, cp.Active FROM CustomerProduct cp JOIN ProductDetail pd ON cp.ProductDetailID = pd.ProductDetailID JOIN Product p ON pd.ProductID = p.ProductID JOIN Users U on U.customerID = cp.customerID where userID = " + _userID, System.Configuration.ConfigurationManager.AppSettings["act"]);

//                foreach (DataRow drproducts in dtproducts.Rows)
//                {
//                    if (!_productfeatures.Contains(drproducts["productname"].ToString().ToLower() + "," + drproducts["FeatureName"].ToString().ToLower()))
//                    {
//                        _productfeatures.Add(drproducts["productname"].ToString().ToLower() + "," + drproducts["FeatureName"].ToString().ToLower(), (drproducts["active"].ToString().ToLower() == "y" ? true : false));
//                    }
//                }
//            }

//            if (_productfeatures.Contains(ProductName.ToLower() + "," + FeatureName.ToLower()))
//                return Convert.ToBoolean(_productfeatures[ProductName.ToLower() + "," + FeatureName.ToLower()]);
//            else
//                return false;
//        }

//        /// <summary>
//        /// Create the Singleton Session object if not exists.
//        /// </summary>
//        /// <returns></returns>
//        public static ecnSession CurrentSession()
//        {
//            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            
//            ecnSession _ecnSession;
//            if (null == HttpContext.Current.Session[SMART_SESSION + "_"+ sc.UserID().ToString()])
//            {
//                _ecnSession = new ecnSession();
//                System.Web.HttpContext.Current.Session[SMART_SESSION + "_" + sc.UserID().ToString()] = _ecnSession;
//            }
//            else
//            {
//                _ecnSession = (ecnSession)System.Web.HttpContext.Current.Session[SMART_SESSION + "_" + sc.UserID().ToString()];  

//                string scChannelID = sc.ChannelID();

//                if (_ecnSession._basechannelID != (scChannelID == "" ? 1 : Convert.ToInt32(scChannelID)))
//                    _ecnSession.RefreshSession();
//            }
//            return _ecnSession;

//        }
//    }
//}
