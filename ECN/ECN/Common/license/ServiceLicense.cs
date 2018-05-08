using System;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes.billing;

namespace ecn.common.classes.license {	
	public class ServiceLicense : LicenseBase {
		public ServiceLicense(QuoteItem quoteItem, Service service, DateTime start, DateTime end) :base(quoteItem)  {
			_service = service;			
			_usage = new UsageLicense(service.AllowedInquirieCount, 0, start, end);
		}

		public ServiceLicense(UsageLicense usage) : base(null) {
			_usage = usage;
		}

		private UsageLicense _usage;
		public UsageLicense Usage {
			get {
				return (this._usage);
			}		
		}

		private Service _service;
		public Service Service {
			get {
				return (this._service);
			}
			set {
				this._service = value;
			}
		}		

		public override string ToString() {
			return string.Format("{0}/{1}(Used/Total) \nStart Date: {2} Expiration Date: {3}", Usage.UsedCount, Usage.Quantity, Usage.StartDate.ToShortDateString(), Usage.EndDate.ToShortDateString());
		}

		#region Overriden Methods
		public override void Enable() {
			SetUsageLicense(Usage, _service.ServiceType.ToString(), true); 
		}

		public override void Disable() {
			SetUsageLicense(Usage, _service.ServiceType.ToString(), false); 
		}
		#endregion

		#region Datebase Methods
		public void UpdateUsage() {
			if (ID <= 0) {
				throw new ApplicationException("Can't update usage for new licenese.");
			}

			DataFunctions.Execute(string.Format("UPDATE [CustomerLicense] SET Used = {0} WHERE CLID = {1};", Usage.UsedCount, ID));
		}
		#endregion

		#region Static Database methods
		// This method doesn't need to populate the quote item it refered to.
		public static ServiceLicense GetServiceLicenseByCustomerID(ServiceTypeEnum serviceType, int customerID) {
            string sql = string.Format("SELECT * FROM [CustomerLicense] where CustomerID = {0} AND IsActive = 1 AND IsDeleted = 0 AND LicenseTypeCode = '{1}' And ExpirationDate > '{2}';", customerID, serviceType.ToString(), DateTime.Now.ToShortDateString());
			DataTable dt = DataFunctions.GetDataTable(sql);
						
			if (dt.Rows.Count == 0) {
				return null;
			}

			DataRow row = dt.Rows[0];
			return GetServiceLicense(row);			
		}

		public static ServiceLicense GetServiceLicenseByID(int licenseID) {
            string sql = string.Format("SELECT * FROM [CustomerLicense] where CLID = {0} AND IsDeleted = 0;", licenseID);
			DataTable dt = DataFunctions.GetDataTable(sql);
						
			if (dt.Rows.Count == 0) {
				return null;
			}

			DataRow row = dt.Rows[0];
			return GetServiceLicense(row);			
		}

		private static ServiceLicense GetServiceLicense(DataRow row) {
			ServiceLicense license = new ServiceLicense(new UsageLicense(Convert.ToInt64(row["Quantity"]), Convert.ToInt64(row["Used"]), Convert.ToDateTime(row["AddDate"]), Convert.ToDateTime(row["ExpirationDate"])));
			license.ID = Convert.ToInt32(row["CLID"]);			
			license.QuoteItem = QuoteItem.GetQuoteItemByID(Convert.ToInt32(row["QuoteItemID"]));
			license.Service = license.QuoteItem.Services.FindByServiceName(Convert.ToString(row["LicenseTypeCode"]));
			return license;
		}
		#endregion
	}
}
