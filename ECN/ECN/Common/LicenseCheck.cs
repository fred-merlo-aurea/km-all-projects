using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Web.Security;

namespace ecn.common.classes {
	public class LicenseCheck {
    public static string accountsdb		= ConfigurationManager.AppSettings["accountsdb"];
		public LicenseCheck(){
		}

        public bool IsFreeTrial(string CustomerID)
        {
            try
            {
                if (Convert.ToInt32(DataFunctions.ExecuteScalar("accounts","SELECT count(CLID)  FROM " + accountsdb + ".dbo.CustomerLicense WHERE CustomerID=" + CustomerID + " AND LicenseTypeCode = 'emailblock10k' and IsDeleted = 0")) > 0)
                    return false;
                else
                    return true;
            }
            catch 
            {
                return false;
            }
        }

		public int CollectorCheck(string CustomerID) {
			int collectorLevel = 0;
			try {
				string sqlquery = "SELECT CollectorLevel "+
					" FROM "+accountsdb+ ".dbo.Customer "+
                    " WHERE and IsDeleted = 0 and CustomerID=" + CustomerID;
				collectorLevel = Convert.ToInt32(DataFunctions.ExecuteScalar("accounts",sqlquery));

			}
			catch (Exception e) {
				string devnull=e.ToString();
			}
			return collectorLevel;
		}


        public string Current(string CustomerID)
        {
            int theCurrent = 0;
            try
            {
                string sqlquery = "SELECT isnull(SUM(Quantity),0) " +
                    " FROM " + accountsdb + ".dbo.CustomerLicense " +
                    " WHERE CustomerID=" + CustomerID +
                    " AND LicenseTypeCode = 'emailblock10k'" +
                    " AND ExpirationDate>GetDate() " +
                    " AND AddDate<GetDate() and IsDeleted = 0";
                theCurrent = Convert.ToInt32(DataFunctions.ExecuteScalar("accounts",sqlquery).ToString());
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            if (theCurrent == 0)
            {
                return "NO LICENSE";
            }
            else if (theCurrent <= -1)
            {
                return "UNLIMITED";
            }
            return theCurrent.ToString();
        }

        public string Current(string CustomerID, string LicenseTypeCode)
        {
            int theCurrent = 0;
            try
            {
                string sqlquery = "SELECT isnull(SUM(Quantity),0) " +
                    " FROM " + accountsdb + ".dbo.CustomerLicense " +
                    " WHERE CustomerID=" + CustomerID +
                    " AND LicenseTypeCode = '" + LicenseTypeCode + "'" +
                    " AND ExpirationDate>GetDate() " +
                    " AND AddDate<GetDate() and IsDeleted = 0";
                theCurrent = Convert.ToInt32(DataFunctions.ExecuteScalar("accounts",sqlquery).ToString());
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            if (theCurrent == 0)
            {
                return "NO LICENSE";
            }
            else if (theCurrent <= -1)
            {
                return "UNLIMITED";
            }
            return theCurrent.ToString();
        }

		public string Used(string CustomerID) {
			string theUsed="0";
			try {
				string sqlquery = "SELECT SUM(Used) "+
					" FROM "+accountsdb+ ".dbo.CustomerLicense "+
					" WHERE CustomerID="+CustomerID+
					" AND ExpirationDate>GetDate() "+
                    " AND AddDate<GetDate() and IsDeleted = 0";
				theUsed=DataFunctions.ExecuteScalar("accounts",sqlquery).ToString();
			}
			catch (Exception e) {
				string devnull=e.ToString();
			}
			if (theUsed=="") {
				theUsed="NO LICENSE";
			}
			return theUsed;
		}

        public string Used(string CustomerID, string LicenseTypeCode)
        {
            string theUsed = "0";
            try
            {
                string sqlquery = "SELECT SUM(Used) " +
                    " FROM " + accountsdb + ".dbo.CustomerLicense " +
                    " WHERE CustomerID=" + CustomerID +
                    " AND LicenseTypeCode = '" + LicenseTypeCode + "'" +
                    " AND ExpirationDate>GetDate() " +
                    " AND AddDate<GetDate() and IsDeleted = 0";
                theUsed = DataFunctions.ExecuteScalar("accounts",sqlquery).ToString();
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            if (theUsed == "")
            {
                theUsed = "NO LICENSE";
            }
            return theUsed;
        }

		public string Available(string CustomerID) {
			string theAvailable="0";
			try {
				string sqlquery = "SELECT SUM(Quantity-Used) "+
					" FROM "+accountsdb+ ".dbo.CustomerLicense "+
					" WHERE CustomerID="+CustomerID+
					" AND LicenseTypeCode = 'emailblock10k'" +
					" AND ExpirationDate>GetDate() "+
                    " AND AddDate<GetDate() and IsDeleted = 0";
				theAvailable=DataFunctions.ExecuteScalar("accounts",sqlquery).ToString();
			}
			catch (Exception e) {
				string devnull=e.ToString();
			}
			if (theAvailable=="") {
				theAvailable="NO LICENSE";
			} else if (Convert.ToInt32(theAvailable) == -1) {
				theAvailable="N/A";
            } else if(Convert.ToInt32(theAvailable) < -1) {
                theAvailable = "NO LICENSE";
            }
			return theAvailable;
		}

        public string Available(string CustomerID, string LicenseTypeCode)
        {
            string theAvailable = "0";
            try
            {
                string sqlquery = "SELECT SUM(Quantity-Used) " +
                    " FROM " + accountsdb + ".dbo.CustomerLicense " +
                    " WHERE CustomerID=" + CustomerID +
                     " AND LicenseTypeCode = '" + LicenseTypeCode + "'" +
                    " AND ExpirationDate>GetDate() " +
                    " AND AddDate<GetDate() and IsDeleted = 0";
                theAvailable = DataFunctions.ExecuteScalar("accounts",sqlquery).ToString();
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            if (theAvailable == "")
            {
                theAvailable = "NO LICENSE";
            }
            else if (Convert.ToInt32(theAvailable) == -1)
            {
                theAvailable = "N/A";
            }
            else if (Convert.ToInt32(theAvailable) < -1)
            {
                theAvailable = "NO LICENSE";
            }
            return theAvailable;
        }

		public string BlastCheck(int CustomerID, int GroupID, int FilterID, int refBlastID, string Suppressionlist) {
			string theBC="";

			//-- Modified to use new proc -- Sunil 04/26/07
			SqlCommand emailsListCmd	 = new SqlCommand("spGetBlastEmailsList");
			emailsListCmd.CommandTimeout = 0; 
			emailsListCmd.CommandType= CommandType.StoredProcedure;
			try {
				string actionType = "";

				if(FilterID > 0 && refBlastID > 0) 
				{
					SmartSegmentFilter filter = new SmartSegmentFilter(FilterID);

					if (filter.IsSmartSegmentFilter) 
						actionType = filter.GetWhereClause();
				}

				emailsListCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
				emailsListCmd.Parameters["@CustomerID"].Value = CustomerID;

				emailsListCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
				emailsListCmd.Parameters["@BlastID"].Value = 0;

				emailsListCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
				emailsListCmd.Parameters["@GroupID"].Value = GroupID;

				emailsListCmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.Int));
				emailsListCmd.Parameters["@FilterID"].Value = FilterID;		
		
				emailsListCmd.Parameters.Add(new SqlParameter("@BlastID_and_BounceDomain", SqlDbType.VarChar,250));
				emailsListCmd.Parameters["@BlastID_and_BounceDomain"].Value = "";

				//--ActionType for smartSegment which is "unOpened / unClicked "
				emailsListCmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar,10));
				emailsListCmd.Parameters["@ActionType"].Value = actionType.ToString();

				//-- Reference BlastID for smartSegment				
				emailsListCmd.Parameters.Add(new SqlParameter("@refBlastID", SqlDbType.VarChar));
				emailsListCmd.Parameters["@refBlastID"].Value = refBlastID.ToString();

				emailsListCmd.Parameters.Add(new SqlParameter("@SupressionList", SqlDbType.VarChar,2000));
				emailsListCmd.Parameters["@SupressionList"].Value = Suppressionlist;

				emailsListCmd.Parameters.Add(new SqlParameter("@OnlyCounts", SqlDbType.Bit));
				emailsListCmd.Parameters["@OnlyCounts"].Value = 1;
				
				string count = string.Empty;
				count = DataFunctions.ExecuteScalar("activity", emailsListCmd).ToString();
				
				return count;
			}
			catch (Exception e) 
			{
				string devnull=e.ToString();
				theBC += devnull;
			} 
			return theBC;
		}

		public void UpdateUsed(int CustomerID, string LicenseType, int HowManyUsed) {
			int HowManyLeft=HowManyUsed;
			string sqlQuery	= " SELECT * From "+accountsdb+ ".dbo.CustomerLicense "+
				" WHERE CustomerID="+CustomerID+
			        " AND LicenseTypeCode='" + LicenseType + "' " + 
				" AND ExpirationDate>GetDate() "+
                " AND AddDate<GetDate()  and IsDeleted = 0" +
				" ORDER BY AddDate ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			int TotalRows=dt.Rows.Count;
			int i=0;
			foreach ( DataRow dr in dt.Rows ) {
				int CLID=(int)dr["CLID"];
				int Quantity=(int)dr["Quantity"];
				int Used=(int)dr["Used"];
				int Available=Quantity-Used;
				if (Quantity==-1) {
					string sqlupdate="UPDATE "+accountsdb+ ".dbo.CustomerLicense SET Used="+(Used+HowManyLeft)+
						" WHERE CLID="+CLID;
					DataFunctions.Execute(sqlupdate);
					HowManyLeft=0;
					break;
				} else if (Available>HowManyLeft) {
					string sqlupdate="UPDATE "+accountsdb+ ".dbo.CustomerLicense SET Used="+(Used+HowManyLeft)+
						" WHERE CLID="+CLID;
					DataFunctions.Execute(sqlupdate);
					HowManyLeft=0;
					break;
				} else {
					string sqlupdate="";
					if (i==TotalRows) {
						sqlupdate="UPDATE "+accountsdb+ ".dbo.CustomerLicense SET Used="+HowManyLeft+" WHERE CLID="+CLID;
					}else{
						sqlupdate="UPDATE "+accountsdb+ ".dbo.CustomerLicense SET Used="+Quantity+" WHERE CLID="+CLID;
					}
					DataFunctions.Execute(sqlupdate);
					HowManyLeft=HowManyLeft-Available;
				}
				i++;
			}
		}
	}
}
