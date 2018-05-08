using System;
using System.IO;
using System.Configuration;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.lists
{
	public partial class emailAddressCheck : System.Web.UI.Page 
    {
	
		protected void Page_Load(object sender, System.EventArgs e) 
        {
		}

		protected void validateBtn_ServerClick(object sender, System.EventArgs e)
        {
			phResults.Visible = true;
			aspNetMX.MXValidate mx = new aspNetMX.MXValidate();
            mx.SMTPFrom = "info@knowledgemarketing.com";
            mx.SMTPHello = "mail.knowledgemarketing.com";

			//mx options
			mx.LogInMemory = true;
			//mx.SMTPTimeOut = 15000; //15 seconds 
			aspNetMX.MXValidateLevel level = mx.Validate( txtEmail.Value.Trim(), aspNetMX.MXValidateLevel.Mailbox );
			litResults.Text = txtEmail.Value.Trim();
			if( level == aspNetMX.MXValidateLevel.Mailbox)
            {
				litResults.Text += " is a valid email address.<br>";
			} 
            else
            {
				litResults.Text += " is not a valid email address.<br>";
			}

			mx.LogInMemory = false;
			aspNetMX.MXRecord[] recs = mx.GetMXServers(mx.DomainName(txtEmail.Value.Trim())).Records;
			if(( recs != null ) && ( recs.Length > 0 ) ) 
            {
				litMxRecords.Text = string.Empty;
				foreach( aspNetMX.MXRecord r in recs )
                {
					litMxRecords.Text += r.ToString()+"<BR>";
				}
			}
			else
				litMxRecords.Text = "<br>no records exist.";

			TextReader  reader = new StringReader(Server.HtmlEncode(mx.GetLog()).ToString()) ;
			string log = (reader.ReadToEnd().ToString()).Replace("\r\n", "");
			reader.Close();

			litLog.Text = log.Substring(log.LastIndexOf("-- "));

            txtLog.Text = mx.GetLog();
		}
	}
}