using System;
using ecn.common.classes;
using System.Data;
namespace ecn.common.classes
{
	
	
	
	public class CustomerConfig
	{
		public CustomerConfig()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// Gets Variable address from Config
		


		// Gets the CustomerConfigID for the 'PickupPath'
		public static int GetPickupConfigID(int cust_id) 
		{
			object number = DataFunctions.ExecuteScalar("accounts","select CustomerConfigID from CustomerConfig where CustomerID = "+cust_id+" and ProductID = 100 and ConfigName = 'PickupPath'");
			if(number == null) return 0;
			return Convert.ToInt32(number);
		}

		
		/// Gets the Pickup directory. If the customer doesn't have one set, it gets the default for the channel.
		
		/// <param name="cust_id">Customer ID to get output path for</param>
		/// <returns>Output Path</returns>
		public static string GetPickupPath(int cust_id) 
		{
			int id = GetPickupConfigID(cust_id);
			if(id == 0) 
			{
				string com_id = DataFunctions.ExecuteScalar("accounts","select CommunicatorChannelID from Customer where CustomerID = " + cust_id).ToString();
				return DataFunctions.ExecuteScalar("accounts","select PickupPath from Channel where ChannelID = " + com_id).ToString();
			}
			return GetConfigValue( id);

		}

		public static string GetConfigValue(int ccid) 
		{
			return DataFunctions.ExecuteScalar("accounts", "select ConfigValue from CustomerConfig where CustomerConfigID = " + ccid).ToString();

		}

		public static int GetPickupConfig(int cust_id) 
		{
			return GetComConfigID(cust_id,"PickupPath");
		}

		public static int GetComConfigID(int cust_id, string name) 
		{
			object number = DataFunctions.ExecuteScalar("accounts","select CustomerConfigID from CustomerConfig where CustomerID = "+cust_id+" and ProductID = 100 and ConfigName = '"+name +"'");
			if(number == null) return 0;
			return Convert.ToInt32(number);
		}

		public static int GetPickupConfig(string cust_id) 
		{
			return GetPickupConfig(Convert.ToInt32(cust_id));
		}

		public static string GetIP(int cust_id) 
		{
			int config_id = GetComConfigID(cust_id,"MailingIP");
			if (config_id == 0) 
			{
				string com_id = DataFunctions.ExecuteScalar("accounts","select CommunicatorChannelID from Customer where CustomerID = " + cust_id).ToString();
				return DataFunctions.ExecuteScalar("accounts","select MailingIP from Channel where ChannelID = " + com_id).ToString();
			} 
			else 
			{
				return GetMailingIP(cust_id);	
			}
			
		}
		public static string GetMailingIP(int cust_id) 
		{
			return DataFunctions.ExecuteScalar("accounts","select ConfigValue from CustomerConfig where CustomerID = "+cust_id+" and ProductID = 100 and ConfigName = 'MailingIP'").ToString();
		}

		public static void CreatePickupConfig(int cust_id, string path) 
		{
			CreateComConfig(cust_id,"PickupPath",path);
		}

		public static void CreateComConfig(int cust_id, string name, string configvalue) 
		{
			DataFunctions.Execute("insert into CustomerConfig (CustomerID , ProductID, ConfigName, ConfigValue) values (" + cust_id + ",100,'"+name +"','" + configvalue + "')");
		}

		public static void UpdateComConfig(int cust_id, string name, string configvalue)  
		{
			DataFunctions.Execute("update CustomerConfig set ConfigValue = '" + configvalue
				+ "' WHERE CustomerID = " + cust_id + " and ProductID = 100 and ConfigName = '"+name+"'");
		}
	}
}
