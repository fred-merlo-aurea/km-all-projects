using System;
using System.Web;
using System.Text;
using System.Collections;
using System.Web.SessionState;

namespace ecn.showcare.wizard.main {
	/// <summary>
	/// Summary description for wizard.
	/// </summary>
	public abstract class Wizard {
		protected virtual void ToSession (string Name, object Value) {
			HttpContext.Current.Session[Name] = Value;
		}
		
		public virtual void Clear (string Name) {
			HttpContext.Current.Session.Remove(Name);
		}

		public abstract void ClearAll ();

		public static Hashtable ParseUrl (string url) {
			url = url.Substring(url.IndexOf("?")+1);		// Skip the url path
			string []str = url.Split('&');							// Seperate parameters
			Hashtable ht = new Hashtable();
			try {
				for (int i=0; i<str.Length-2; i++) {
					ht.Add(str[i].Substring(0,str[i].IndexOf("=")), str[i].Substring(str[i].IndexOf("=")+1));
				}
			} catch {}
			return ht;
		}
	}
		
	public struct Steps {
		private static int currentstep;
		private static int lastvisitedstep;

		public int Current {
			get {
				currentstep = Convert.ToInt32(HttpContext.Current.Session["currentstep"]);
				return currentstep;
			}
		}

		public int LastVisited {
			get {
				lastvisitedstep = Convert.ToInt32(HttpContext.Current.Session["lastvisitedstep"]);
				return lastvisitedstep;
			}
		}

		public void increamentCurrentStep() {
			currentstep++;
			HttpContext.Current.Session["currentstep"] = currentstep;
		}

		public void decreamentCurrentStep() {
			currentstep--;
			HttpContext.Current.Session["currentstep"] = currentstep;
		}

		public void nextStep() {
			lastvisitedstep++;
			HttpContext.Current.Session["lastvisitedstep"] = lastvisitedstep;
		}
	}

	public class Step1 : Wizard {
		private string tid;
		private string cid;
		private string uid;
		private string gid;
		private bool isCustomHeader = false;
			
		public string TemplateID {
			get {
				return tid;
			} set {
				  tid = value;
			  }
		}

		public bool IsCustomHeader
		{
			get {return isCustomHeader;} 
			set {isCustomHeader = value;}
		}

		public string CustomerID {
			get {
				return cid;
			} set {
				  cid = value;
			  }
		}

		public string UserID {
			get {
				return uid;
			} set {
				  uid = value;
			  }
		}

		public string GroupID {
			get {
				return gid;
			} set {
				  gid = value;
			  }
		}

		public Step1(string TemplateID, string CustomerID, string GroupID, string UserID, bool CustomHeader) {
			tid = TemplateID;
			cid = CustomerID;
			gid = GroupID;
			uid = UserID;
			isCustomHeader = CustomHeader;
			base.ToSession("step1",this);
		}

		public Step1 () {
			tid = "";
			uid = gid = cid = "";
			isCustomHeader = false;
		}

		public void ToSession () {
			//base.ToSession("TemplateID", tid);
			base.ToSession("step1",this);
		}

		public override void Clear (string name) {
			base.Clear(name);
		}

		public override void ClearAll () {
			Clear ("step1");
		}
	}

	public class Step2 : Wizard {
		private string messagename;
		private string emailaddress;
		private string headerfromname;
		private string headerimage="";

		private string emailsubject;
		private string salutation;
		private string footername;
		private string footertitle;
		private string footercompany;
		private string footerphone;
		private StringBuilder contentsource;
		private StringBuilder contenttext;

		public string MessageName {
			get {
				return messagename;
			} set {
				  messagename = value;
//				  base.ToSession("MessageName", messagename);
			  }
		}

		public string EmailAddress {
			get {
				return emailaddress;
			} set {
				  emailaddress = value;
//				  base.ToSession("EmailAddress", emailaddress);
			  }
		}

		public string HeaderImage
		{
			get {return headerimage;} 
			set {headerimage = value;}
		}

		public string HeaderFromName {
			get {
				return headerfromname;
			} set {
				  headerfromname = value;
//				  base.ToSession("HeaderFromName", headerfromname);
			  }
		}

		public string EmailSubject {
			get {
				return emailsubject;
			} set {
				  emailsubject = value;
//				  base.ToSession("EmailSubject", emailsubject);
			  }
		}

		public string Salutation {
			get {
				return salutation;
			} set {
				  salutation = value;
//				  base.ToSession("Salutation", salutation);
			  }
		}

		public string FooterName {
			get {
				return footername;
			} set {
				  footername = value;
//				  base.ToSession("FooterName", footername);
			  }
		}

		public string FooterTitle {
			get {
				return footertitle;
			} set {
				  footertitle = value;
//				  base.ToSession("FooterTitle", footertitle);
			  }
		}

		public string FooterCompany {
			get {
				return footercompany;
			} set {
				  footercompany = value;
//				  base.ToSession("FooterCompany", footercompany);
			  }
		}

		public string FooterPhone {
			get {
				return footerphone;
			} set {
				  footerphone = value;
//				  base.ToSession("FooterPhone", footerphone);
			  }
		}

		public string ContentSource {
			get {
				return contentsource.ToString(0, contentsource.Length);
			} set {
				  contentsource = new StringBuilder(value);
//				  base.ToSession("Contents", contents);
			  }
		}

		public string ContentText {
			get {
				return contenttext.ToString(0, contenttext.Length);
			} set {
				  contenttext = new StringBuilder(value);
				  //				  base.ToSession("Contents", contents);
			  }
		}

		public Step2 (string MessageName, string EmailAddress, string FromName, string EmailSubject, string Salutation,
			string FooterName, string FooterTitle, string FooterCompany, string FooterPhone, string ContentSource, string ContentText, string Headerimage) {
			this.MessageName = MessageName;
			this.EmailAddress = EmailAddress;
			this.HeaderFromName = FromName;
			this.EmailSubject = EmailSubject;
			this.Salutation = Salutation;
			this.FooterName = FooterName;
			this.FooterTitle = FooterTitle;
			this.FooterCompany = FooterCompany;
			this.FooterPhone = FooterPhone;
			this.ContentSource = ContentSource;
			this.ContentText = ContentText;
			this.HeaderImage = Headerimage;
		}

		public Step2 (string MessageName, string EmailAddress, string FromName, string EmailSubject, string Salutation,
			string FooterName, string FooterTitle, string FooterCompany, string ContentSource, string ContentText) {
			this.MessageName = MessageName;
			this.EmailAddress = EmailAddress;
			this.HeaderFromName = FromName;
			this.EmailSubject = EmailSubject;
			this.Salutation = Salutation;
			this.FooterName = FooterName;
			this.FooterTitle = FooterTitle;
			this.FooterCompany = FooterCompany;
			this.ContentSource = ContentSource;
			this.ContentText = ContentText;
		}

		public Step2 () {
			this.MessageName = "";
			this.EmailAddress = "";
			this.HeaderFromName = "";
			this.HeaderImage = "";
			this.EmailSubject = "";
			this.Salutation = "";
			this.FooterName = "";
			this.FooterTitle = "";
			this.FooterCompany = "";
			this.FooterPhone = "";
			this.ContentSource = "";
			this.ContentText = "";
		}

		protected override void ToSession (string name, object val) {
			base.ToSession(name, val);
		}

		public void ToSession () {
/*			base.ToSession("MessageName",messagename);
			base.ToSession("EmailAddress",emailaddress);
			base.ToSession("HeaderFromName",headerfromname);
			base.ToSession("EmailSubject",emailsubject);
			base.ToSession("Salutation",salutation);
			base.ToSession("FooterName",footername);
			base.ToSession("FooterTitle",footertitle);
			base.ToSession("FooterCompany",footercompany);
			base.ToSession("FooterPhone",footerphone);
			base.ToSession("Contents",contents);*/
			base.ToSession("step2",this);
		}

		public override void Clear (string name) {
			base.Clear(name);
		}

		public override void ClearAll () {
/*			Clear("MessageName");
			Clear("EmailAddress");
			Clear("HeaderFromName");
			Clear("EmailSubject");
			Clear("Salutation");
			Clear("FooterName");
			Clear("FooterTitle");
			Clear("FooterCompany");
			Clear("FooterPhone");
			Clear("Contents");*/
			Clear("step2");
		}
	}

	public class Step3 : Wizard {
		public override void ClearAll() {}
	}

	public class Step4 : Wizard {
		private string cid;
		private string lid;
		private string bid;

		public string ContentID {
			get {
				return cid;
			} set {
				  cid = value;
			  }
		}

		public string LayoutID {
			get {
				return lid;
			} set {
				  lid = value;
			  }
		}

		public string BlastID {
			get {
				return bid;
			} set {
				  bid = value;
			  }
		}

		public Step4 (string ContentID, string LayoutID, string BlastID) {
			this.ContentID = ContentID;
			this.LayoutID = LayoutID;
			this.BlastID = BlastID;
		}

		public Step4 () {
			cid = "";
			lid = "";
			bid = "";
		}

		protected override void ToSession(string Name, object Value) {
			base.ToSession (Name, Value);
		}

		public void ToSession () {
			base.ToSession("step4", this);
		}

		public override void Clear(string Name) {
			base.Clear (Name);
		}

		public override void ClearAll() {
			Clear ("step4");
		}
	}

	public class Reporting : Wizard {
		private string cid;
		private string uid;
		private string email;
		private string bid;
		private string bcid;

		public string CustomerID {
			get {
				return cid;
			} set {
				  cid = value;
			  }
		}

		public string UserID {
			get {
				return uid;
			} set {
				  uid = value;
			  }
		}

		public string Email {
			get {
				return email;
			} set {
				  email = value;
			  }
		}

		public string BlastID {
			get {
				return bid;
			} set {
				  bid = value;
			  }
		}

		public string ChannelID {
			get {
				return bcid;
			} set {
				  bcid = value;
			  }
		}

		public Reporting () {
			uid = null;
			cid = null;
			email = null;
			bid = null;
			bcid = null;
		}

		public Reporting (string UserID, string CustomerID, string Email, string BlastID, string BaseChannelID) {
			UserID = UserID;
			CustomerID = CustomerID;
			Email = Email;
			BlastID = BlastID;
			ChannelID = BaseChannelID;
		}

		protected override void ToSession(string Name, object Value) {
			base.ToSession (Name, Value);
		}

		public void ToSession () {
			base.ToSession("reports", this);
		}

		public override void Clear(string Name) {
			base.Clear (Name);
		}

		public override void ClearAll() {
			Clear("reports");
		}
	}

}
