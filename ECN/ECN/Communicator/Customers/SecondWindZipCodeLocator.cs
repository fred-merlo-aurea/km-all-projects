using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ecn.communicator.classes.Customers
{
	public class SecondWindZipCodeLocator : IZipCodeLocator {
		#region IZipCodeLocator Members
		public int GetIDOfNearestObject(string zipcode) {			
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["2ndWind"]);
			string cleanedZipCode = zipcode.Trim().Substring(0, 5);
			SqlCommand cmd = new SqlCommand(string.Format("select dbo.uFN_GetNearestStore('{0}')", cleanedZipCode),conn);
			try {
				conn.Open();
				return Convert.ToInt32(cmd.ExecuteScalar());
			} catch(Exception e) {
				string errorMessage = e.Message;
				return 0;
			}
			finally {
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}			
		}
		#endregion
	}
}
