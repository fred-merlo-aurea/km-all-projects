using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Web.Mail;
using ecn.common.classes;
using ECN_Framework_Common.Functions;

namespace ecn.CustomerDiskUsageMonitor {

	class DiskUsageMonitor {
		//public static ApplicationLog appLog = null;
		
		#region getters & setters
		public static string filePath {
			get { return (ConfigurationSettings.AppSettings["FileCreationPath"]); }
		}

		public static string connString {
			get { return (ConfigurationSettings.AppSettings["connString"]); }
		}

		public static string SMTPServer {
			get { return (ConfigurationSettings.AppSettings["SmtpServer"]); }
		}
		public static string adminEmail {
			get { return (ConfigurationSettings.AppSettings["ADMIN_EMAIL"]); }
		}
		#endregion
		
		[STAThread]
		static void Main(string[] args) {
			string baseChannelSelect = "SELECT BaseChannelID FROM BaseChannel ORDER BY BaseChannelID";
			DataTable baseChannelDT = DataFunctions.GetDataTable(baseChannelSelect, connString);

			foreach(DataRow channelDR in baseChannelDT.Rows){
				var baseChannelID = channelDR["BaseChannelID"].ToString();
				var customerSelect = "SELECT CustomerID FROM Customer WHERE BaseChannelID = "+baseChannelID+" ORDER BY CustomerID";
				
				DataTable customersDT = DataFunctions.GetDataTable(customerSelect, connString);
				foreach(DataRow customersDR in customersDT.Rows){
					var custID = customersDR["CustomerID"].ToString();
					var imgPath	= filePath+"images";
                    imgPath = imgPath.Replace("%%channelID%%", baseChannelID);
                    imgPath = imgPath.Replace("%%customerID%%", custID);

					string dataPath= filePath+"data";
                    dataPath = dataPath.Replace("%%channelID%%", baseChannelID);
                    dataPath = dataPath.Replace("%%customerID%%", custID);

					double size = 0;
					var imgStorageSize = new DataStorageCalculator();
                    if (Directory.Exists(imgPath))
                    {
						size += imgStorageSize.GetSizeInBytes(imgPath);
					}                    

				    var dataStorageSize = new DataStorageCalculator();
					try
                    {
						size += dataStorageSize.GetSizeInBytes(dataPath);
					}
                    catch (DirectoryNotFoundException)
                    { 
						// do nothing the 
					}

					string insertSQL = string.Format(@"INSERT INTO CustomerDiskUsage (ChannelID, CustomerID, SizeInBytes) values ({0}, {1}, {2})",baseChannelID, custID, size.ToString());
					try
                    {
						DataFunctions.Execute(insertSQL);	
					}
                    catch (Exception ex)
                    {
						NotifyAdmin("DISKUsage Monitor Error on CustomerID: "+custID, ex.ToString());
					}
					//break;
				}
				//break;
			}
		}

		#region Notify Admin
		public static void NotifyAdmin(string subject, string body) {
			SmtpMail.SmtpServer = SMTPServer;
			MailMessage message = new MailMessage();
			message.To = adminEmail;
			message.From = "domain_admin@teckman.com";
			message.Subject = subject;
			message.Body = body;
			SmtpMail.Send(message);
		}
		#endregion
	}
}
