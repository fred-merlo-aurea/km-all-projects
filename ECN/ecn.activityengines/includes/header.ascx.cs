//namespace ecn.activityengines.includes
//{
//    using System;
//    using System.Data;
//    using System.Data.SqlClient;
//    using System.Drawing;
//    using System.Web;
//    using System.Web.UI.WebControls;
//    using System.Web.UI.HtmlControls;
//    using System.Web.Security;
//    using System.Security.Principal;
//    using ecn.communicator.classes;
//    using ecn.common.classes;

	
//    ///		Summary description for header.
	
//    public partial  class header : System.Web.UI.UserControl {
		
//        private ChannelBlock cb = new ChannelBlock();
		
//        private string PageTitle="PageTitle";
//        private string MainTitle="MainTitle";
//        private string HelpBoxTitle="HelpBoxTitle";
//        private string HelpBoxContent="HelpBoxContent";
//        private string MainMenu="MainMenu";
//        private string SubMenu="SubMenu";
//        public string strMainMenu = "";
//        public string strSubMenu = "";
//        private string PopUp="";


//        public string divContentTitle {
//            set {
//                MainTitle = value;
//            }
//        }
//        public string divHelpBoxTitle {
//            set {
//                HelpBoxTitle = value;
//            }
//        }
//        public string divHelpBox {
//            set {
//                HelpBoxContent = value;
//            }
//        }
//        public string ecnMenu {
//            set {
//                strMainMenu = value;
//            }
//        }
//        public string ecnSubMenu 
//        {
//            set 
//            {
//                strSubMenu = value;
//            }
//        }
//        public string loginBlock {
//            get {
//                return cb.LoginBlock(getUser(),getError(),System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"]+"/includes/login.aspx");
//            }
//        }
//        public string popUp {
//            set {
//                PopUp=value;
//            }
//        }

//        public void popMsg(string msg){
//            Response.Write("<script>window.open(\"http://www.teckman.com\");</script>");
//        }

//        protected void Page_Load(object sender, System.EventArgs e) {
			
//            ECN_Framework.Common.ECNSession es = ECN_Framework.Common.ECNSession.CurrentSession();

//            MainMenu = cb.MainMenu(strMainMenu);
//            SubMenu = cb.SubMenu(strMainMenu, strSubMenu);

//            string HeaderSource = TemplateFunctions.channelTemplate(
//                es.HeaderSource("communicator"), es.UserName, System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/" + es.BaseChannelID,
//                PageTitle, MainTitle, HelpBoxTitle, HelpBoxContent,
//                MainMenu, SubMenu, es.BaseChannelName
//                );
//            headerOutput.Text = HeaderSource;

//        }

//        private string getUser() {
//            try {
//                return Request["user"].ToString();
//            }
//            catch {
//                return string.Empty;
//            }
			
//        }
//        private string getError() {
			
//            try {
//                return Request["error"].ToString();
//            }
//            catch {
//                return string.Empty;
//            }
			
//        }

//        #region Web Form Designer generated code
//        override protected void OnInit(EventArgs e) {
//            //
//            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
//            //
//            InitializeComponent();
//            base.OnInit(e);
//        }
		
//        ///		Required method for Designer support - do not modify
//        ///		the contents of this method with the code editor.
		
//        private void InitializeComponent() {

//        }
//        #endregion
//    }
//}
