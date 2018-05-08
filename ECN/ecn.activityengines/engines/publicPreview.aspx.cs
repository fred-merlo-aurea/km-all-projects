using System;
using System.Collections;
using System.Configuration;
using System.Linq;

namespace ecn.activityengines
{
    public partial class publicPreview : PreviewBasePage
    {
        private int BlastID = 0;
        private int LayoutID = 0;
        private int EmailID = 0;
        int _transnippetsCount;
        public int TransnippetsCount
        {
            get { return _transnippetsCount; }
            set { _transnippetsCount = value; }
        }
        ArrayList _transnippet;

        public ArrayList Transnippet
        {
            get { return _transnippet; }
            set { _transnippet = value; }
        }

        private void WriteToLog(string text)
        {
            try
            {
                if (ConfigurationManager.AppSettings["LogToFile"].ToString().ToLower().Equals("true"))
                    System.IO.File.AppendAllText(ConfigurationManager.AppSettings["LogFilePath"].ToString() + "previewLog.txt", "DateTime:" + DateTime.Now.ToString() + " | " + text + "\\r\\n");
            }
            catch { }
        }

        

        private void GetValuesFromQuerystring(string queryString)
        {
            try
            {
                ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
                try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.LayoutID).ParameterValue, out LayoutID); }
                catch (Exception) { }
                try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailID).ParameterValue, out EmailID); }
                catch (Exception) { }
                try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BlastID).ParameterValue, out BlastID); }
                catch (Exception) { }
            }
            catch { }
        }

        private string QSCleanUP(string querystring)
        {
            try
            {
                querystring = querystring.Replace("&amp;", "&");
                querystring = querystring.Replace("&lt;", "<");
                querystring = querystring.Replace("&gt;", ">");
            }
            catch (Exception)
            {
            }

            return querystring.Trim();
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }
    public class LinkMatch
    {
        public LinkMatch() { linkOld = string.Empty; linkNew = string.Empty; }

        public string linkOld { get; set; }

        public string linkNew { get; set; }

    }
}