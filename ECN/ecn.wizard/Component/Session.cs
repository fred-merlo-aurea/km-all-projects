using System;
using System.Web;
using System.Web.SessionState;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for WizardSession.
	/// </summary>
	public class Session
	{
		private const string SESSION_SINGLETON = "WIZARD";

		int _groupID = -1;
		int _templateID = -1;
		int _contentID = -1;
		int _layoutID = -1;
		int _blastID = -1;
		decimal _amount = 0;
		string _messagetitle = "";
		string _emailaddress = "";
		string _name = "";
		string _emailsubject ="";
		string _salutation = "";
		string _footername = "";
		string _footertitle = "";
		string _footercompany = "";
		string _footerphone = "";
		string _contextsource = "";
		string _contexttext = "";
		bool   _processCC = false;	
		bool   _IsCustomHeader = false;
		string _headerImage = "";
		bool _sendsingle = false;

		string _fname = "";
		string _lname = "";
		string _email = "";

		private Session()
		{
			
		}

		//Create as a static method so this can be called using
		// just the class name (no object instance is required).
		// It simplifies other code because it will always return
		// the single instance of this class, either newly created
		// or from the session
		public static Session GetCurrentSession()
		{
			Session obj;

			if (null == System.Web.HttpContext.Current.Session[SESSION_SINGLETON])
			{
				//No current session object exists, use private constructor to 
				// create an instance, place it into the session
				obj = new Session();
				System.Web.HttpContext.Current.Session[SESSION_SINGLETON] = obj;
			}
			else
			{
				//Retrieve the already instance that was already created
				obj = (Session)System.Web.HttpContext.Current.Session[SESSION_SINGLETON];
			}

			//Return the single instance of this class that was stored in the session
			return obj;
		}


		public static void Dispose()
		{
			//Cleanup this object so that GC can reclaim space
			System.Web.HttpContext.Current.Session.Remove(SESSION_SINGLETON);
		}


		public int GroupID 
		{
			get 
			{
				return _groupID;
			}
			set 
			{ 
				_groupID = value; 
			}
		}

		public int TemplateID 
		{
			get 
			{
				return _templateID;
			}
			set {  _templateID = value; }
		}

		public int ContentID 
		{
			get 
			{
				return _contentID;
			}
			set {  _contentID = value; }
		}

		public int LayoutID 
		{
			get 
			{
				return _layoutID;
			}
			set {  _layoutID = value; }
		}

		public int BlastID 
		{
			get 
			{
				return _blastID;
			}
			set {  _blastID = value; }
		}

		public string MessageTitle 
		{
			get 
			{
				return _messagetitle;
			}
			set { _messagetitle = value; }
		}


		public string EmailAddress 
		{
			get 
			{
				return _emailaddress;
			}
			set { _emailaddress = value; }
		}


		public string Name 
		{
			get 
			{
				return _name;
			}
			set { _name = value; }
		}


		public string EmailSubject 
		{
			get 
			{
				return _emailsubject;
			}
			set { _emailsubject = value; }
		}


		public string Salutation 
		{
			get 
			{
				return _salutation;			}
			set { _salutation = value; }
		}

		public string FooterName 
		{
			get 
			{
				return _footername;
			}
			set { _footername = value; }
		}

		public string FooterTitle 
		{
			get 
			{
				return _footertitle;
			}
			set { _footertitle = value; }
		}

		public string FooterCompany
		{
			get 
			{
				return _footercompany;
			}
			set { _footercompany = value; }
		}

		public string FooterPhone 
		{
			get 
			{
				return _footerphone;
			}
			set { _footerphone = value; }
		}

		public string ContentSource 
		{
			get 
			{
				return _contextsource;
			}
			set { _contextsource = value; }
		}

		public string ContentText 
		{
			get 
			{
				return _contexttext;
			}
			set { _contexttext = value; }
		}

		public bool ProcessCC
		{
			get 
			{
				return _processCC;
			}
			set { _processCC = value; }
		}

		public bool IsCustomHeader
		{
			get 
			{
				return _IsCustomHeader;
			}
			set { _IsCustomHeader = value; }
		}

		public string HeaderImage 
		{
			get 
			{
				return _headerImage;
			}
			set { _headerImage = value; }
		}


		public Decimal Amount
		{
			get 
			{
				return _amount;
			}
			set { _amount = value; }
		}

		public bool SendSingle
		{
			get 
			{
				return _sendsingle;
			}
			set { _sendsingle = value; }
		}

		public string FName 
		{
			get 
			{
				return _fname;
			}
			set { _fname = value; }
		}
		public string LName 
		{
			get 
			{
				return _lname;
			}
			set { _lname = value; }
		}
		public string Email 
		{
			get 
			{
				return _email;
			}
			set { _email = value; }
		}

	}
}
