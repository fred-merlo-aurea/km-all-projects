using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace ecn.showcare.webservice.Objects {
	public class ChannelCheck {
		public static string accountsdb=ConfigurationManager.AppSettings["accountsdb"];
		public SecurityCheck sc = new SecurityCheck();
		int _customer_id;

		public ChannelCheck(){
			_customer_id = 0;
		}
		public string getAssetsPath(string productType) {
			//string sqlquery="SELECT AssetsPath FROM Channel WHERE ChannelID="+getChannelID();
			string sqlquery="SELECT AssetsPath FROM "+accountsdb+".dbo.Channel WHERE ChannelTypeCode='"+productType+"' AND BaseChannelID = '"+getChannelID()+"'";
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}
		public string getVirtualPath(string productType) {
			//string sqlquery="SELECT VirtualPath FROM Channel WHERE ChannelID="+getChannelID();
			string sqlquery="SELECT VirtualPath FROM "+accountsdb+".dbo.Channel WHERE ChannelTypeCode='"+productType+"' AND BaseChannelID = "+getChannelID();
			//return sqlquery;
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}
		public string getHostName() { //channelURL
			//string sqlquery="SELECT ChannelURL FROM Channel WHERE ChannelID="+getChannelID();
			string sqlquery="SELECT ChannelURL FROM "+accountsdb+".dbo.BaseChannel WHERE BaseChannelID = '"+getChannelID()+"'";
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}
		public string getBounceDomain() {
			//string sqlquery="SELECT BounceDomain FROM Channel WHERE ChannelID="+getChannelID();
			string sqlquery="SELECT BounceDomain FROM "+accountsdb+".dbo.BaseChannel WHERE BaseChannelID = '"+getChannelID()+"'";
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}
		public string getHeaderSource(string productType) {
			//string sqlquery="SELECT HeaderSource FROM Channel WHERE ChannelID="+getChannelID();
			string sqlquery="SELECT HeaderSource FROM "+accountsdb+".dbo.Channel WHERE ChannelTypeCode='"+productType+"' AND BaseChannelID = '"+getChannelID()+"'";
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}
		public string getFooterSource(string productType) {
			//string sqlquery="SELECT FooterSource FROM Channel WHERE ChannelID="+getChannelID();
			string sqlquery="SELECT FooterSource FROM "+accountsdb+".dbo.Channel WHERE ChannelTypeCode='"+productType+"'  AND BaseChannelID = '"+getChannelID()+"'";
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}
		public string getChannelID(string HostName) {
			string sqlquery="SELECT BaseChannelID FROM "+accountsdb+".dbo.BaseChannel WHERE ChannelURL='"+HostName+"'";
			return DataFunctions.ExecuteScalar(sqlquery).ToString();
		}

		public void CustomerID(int id) {
			_customer_id = id;
		}

		public string getChannelID() {
			if(_customer_id > 0) {
				string sqlquery="SELECT BaseChannelID FROM "+accountsdb+".dbo.Customer WHERE CustomerID=" + _customer_id;
				return DataFunctions.ExecuteScalar(sqlquery).ToString();
			}
			string theChannelID = "";
			// get it from the querystring
			try {
				theChannelID = HttpContext.Current.Request.QueryString["channel"].ToString();
			} catch (Exception E) {
				string devnull=E.ToString();
			}
			// get it from the cookie
			if (theChannelID=="") {
				try {
					theChannelID = sc.ChannelID();
				} catch (Exception E) {
					string devnull=E.ToString();
				}
			}
			// get it from the host
			if (theChannelID=="") {
				string HostName=HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
				try {
					theChannelID = getChannelID(HostName);
				} catch (Exception E) {
					string devnull=E.ToString();
				}
			}
			// default it to 1
			if (theChannelID=="") {
				theChannelID="1";
			}
			return theChannelID;
		}

		public string getBaseChannelID() {
			string theBaseChannelID = "";
			// get it from the querystring
			try {
				theBaseChannelID = HttpContext.Current.Request.QueryString["Channel"].ToString();
			} catch (Exception E) {
				string devnull=E.ToString();
			}
			// get it from the cookie
			if (theBaseChannelID=="") {
				try {
					theBaseChannelID = sc.ChannelID();
				} catch (Exception E) {
					string devnull=E.ToString();
				}
			}
			// get it from the host
			if (theBaseChannelID=="") {
				string HostName=HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
				try {
					theBaseChannelID = getChannelID(HostName);
				} catch (Exception E) {
					string devnull=E.ToString();
				}
			}
			// default it to 1
			if (theBaseChannelID=="") {
				theBaseChannelID="1";
			}
			return theBaseChannelID;
		}
	}
}
