using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PayFlowPro;
using ecn.common.classes.billing;

namespace ecn.accounts.classes.PayFlowPro
{
    public class PayFlowProCreditCardProcessor
    {
        private static PayFlowProCreditCardProcessor _testInstance = null;
        public static PayFlowProCreditCardProcessor TestInstance
        {
            get
            {
                if (_testInstance == null)
                {
                    _testInstance = new PayFlowProCreditCardProcessor("kmBillingService", "knowledgeMarketing", "verisign", "km3#eDc1!qAz",
                        "test-payflow.verisign.com", 443, 30);
                }
                return _testInstance;
            }
        }

        private static PayFlowProCreditCardProcessor _instance = null;
        public static PayFlowProCreditCardProcessor Instance
        {
            get
            {
                if (_instance == null)
                {
                    //changed the gateway & all the login info to process live transactions
                    // - ashok 09/16/05
                    //_instance = new PayFlowProCreditCardProcessor("ecn5admin","knowmarketing","verisign","L!4aE2",
                    //	"payflow.verisign.com", 443,30);

                    //- the above lines are commented - after I talked to Verisign. They told me that the username is the one that's used here & not
                    // the ecn5admin & verisign needs to be a "V" (upper) - ashok 10/11/05
                    _instance = new PayFlowProCreditCardProcessor("knowmarketing", "knowmarketing", "Verisign", "admin14505",
                        "payflow.verisign.com", 443, 30);
                }
                return _instance;
            }
        }

        private PayFlowProCreditCardProcessor(string user, string vendor, string partner, string password,
            string host, int port, int timeOut)
        {
            _user = user;
            _vendor = vendor;
            _partner = partner;
            _password = password;
            _host = host;
            _port = port;
            _timeOut = timeOut;

            _generator = new ParaListGenerator();
        }

        ParaListGenerator _generator;

        #region Properties
        private string _user;
        public string User
        {
            get { return _user; }
        }

        private string _vendor;
        public string Vendor
        {
            get { return _vendor; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
        }

        private string _partner;
        public string Partner
        {
            get { return _partner; }
        }

        private string _host;
        public string Host
        {
            get { return _host; }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
        }

        private int _timeOut;
        public int TimeOut
        {
            get { return _timeOut; }
        }

        public string ProxyAddress
        {
            get { return string.Empty; }
        }

        public int ProxyPort
        {
            get { return 0; }
        }

        public string ProxyLogon
        {
            get { return string.Empty; }
        }

        public string ProxyPassword
        {
            get { return string.Empty; }
        }
        #endregion

        #region Sale Transaction Methods
        public string ProcessSalesTransaction(CreditCard creditCard, double amount, string description)
        {
            return ProcessTransaction(_generator.GetSalesTransactionParas(creditCard, amount, description));
        }

        public string VoidSaleTransaction(string transactionID)
        {
            return ProcessTransaction(_generator.GetVoidSalesTransactionParas(transactionID));
        }
        #endregion

        #region Recurring Billing Service Methods
        public string AddProfile(Profile profile, DateTime startDate)
        {
            string response = ProcessTransaction(_generator.GetParasToAddProfile(profile, startDate));
            profile.ID = ResponseParser.GetProfileID(response);
            return response;
        }

        public string CancelProfileByID(string profileID)
        {
            return ProcessTransaction(_generator.GetParasToCancelProfile(profileID));
        }

        public string ModifyProfile(Profile profile, DateTime startDate)
        {
            return ProcessTransaction(_generator.GetParasToModifyProfile(profile, startDate));
        }

        public string QueryProfileHistory(string profileID)
        {
            return ProcessTransaction(_generator.GetParasToQueryProfileHistory(profileID));
        }
        #endregion

        private string ProcessTransaction(string paraList)
        {
            string paras = _generator.GetAuthString(this) + paraList;
            PFPro pfpro = new PFPro();
            int pctlx = pfpro.CreateContext(Host, Port, TimeOut, ProxyAddress, ProxyPort, ProxyLogon, ProxyPassword);
            string response = pfpro.SubmitTransaction(pctlx, paras);
            pfpro.DestroyContext(pctlx);
            return response;
        }
    }
}
