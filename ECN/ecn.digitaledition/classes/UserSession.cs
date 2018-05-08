using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ecn.common.classes;

namespace ecn.digitaledition.classes
{
    public class UserSession
    {
        string _publicationCode = string.Empty;
        int _editionID = 0;
        int _emailID = 0;
        int _blastID = 0;
        int _totalpages = 0;
        bool _IsLoginRequired = false;
        bool _IsAuthenticated = false;
        string _publicationtype = string.Empty;
        string _imagepath = string.Empty;

        public string PublicationCode
        {
            get
            {
                return _publicationCode;
            }
            set
            {
                _publicationCode = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return _imagepath;
            }
            set
            {
                _imagepath = value;
            }
        }

        public string Publicationtype
        {
            get
            {
                return _publicationtype;
            }
            set
            {
                _publicationtype = value;
            }
        }

        public int EditionID
        {
            get
            {
                return _editionID;
            }
            set
            {
                _editionID = value;
            }
        }

        public int EmailID
        {
            get
            {
                return _emailID;
            }
            set
            {
                _emailID = value;
            }
        }

        public int BlastID
        {
            get
            {
                return _blastID;
            }
            set
            {
                _blastID = value;
            }
        }

        public int Totalpages
        {
            get
            {
                return _totalpages;
            }
            set
            {
                _totalpages = value;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _IsAuthenticated;
            }
            set
            {
                _IsAuthenticated = value;
            }
        }

        public bool IsLoginRequired
        {
            get
            {
                return _IsLoginRequired;
            }
            set
            {
                _IsLoginRequired = value;
            }
        }


        public static UserSession GetCurrentSession(int EditionID)
        {
            UserSession obj = null;

            if (null != System.Web.HttpContext.Current.Session["Edition_" + EditionID.ToString()])
            {
                //Retrieve the instance that was already created
                obj = (UserSession)System.Web.HttpContext.Current.Session["Edition_" + EditionID.ToString()];
            }

            //Return the single instance of this class that was stored in the session
            return obj;
        }

        public static UserSession CreateUserSession(string Publicationcode, int EditionID)
        {
            UserSession obj = null;

            try
            {
                string sql = "select top 1 EditionID, Publicationcode, isnull(EnableSubscription,0) as EnableSubscription, isnull(PublicationType,'Magazine') as PublicationType, '/ecn.images/customers/' + convert(varchar,c.customerID) + '/publisher/' + convert(varchar,editionID) + '/' as imgpath, Pages, IsNull(IsLoginRequired,0) as IsLoginRequired from editions e join Publications m on e.PublicationID = m.PublicationID join ecn5_accounts..customer c on m.customerID = c.customerID where e.Status in('Active','Archieve') and m.active=1 ";

                if (Publicationcode != string.Empty)
                    sql += " and Publicationcode = '" + Publicationcode + "' ";

                if (EditionID > 0)
                    sql += " and EditionID = " + EditionID.ToString();

                sql += " order by EditionID desc";

                DataTable dt = DataFunctions.GetDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    if (null == System.Web.HttpContext.Current.Session["Edition_" + dt.Rows[0]["EditionID"].ToString()])
                    {
                        obj = new UserSession();
                        obj.EditionID = Convert.ToInt32(dt.Rows[0]["EditionID"].ToString());
                        obj.PublicationCode = dt.Rows[0]["Publicationcode"].ToString();
                        obj.Totalpages = Convert.ToInt32(dt.Rows[0]["Pages"].ToString());
                        obj.IsLoginRequired = Convert.ToBoolean(dt.Rows[0]["IsLoginRequired"].ToString());
                        obj.ImagePath = dt.Rows[0]["imgpath"].ToString();
                        obj.Publicationtype = dt.Rows[0]["PublicationType"].ToString();

                        System.Web.HttpContext.Current.Session["Edition_" + dt.Rows[0]["EditionID"].ToString()] = obj;
                    }
                    else
                    {
                        //Retrieve the already instance that was already created
                        obj = (UserSession)System.Web.HttpContext.Current.Session["Edition_" + dt.Rows[0]["EditionID"].ToString()];
                    }

                }
            }
            catch
            {
            }

            //Return the single instance of this class that was stored in the session
            return obj;
        }
    }
}
