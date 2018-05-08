using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes.license;

namespace ecn.common.classes
{
	
	
	
	public class CustomerInquirie
	{
        public CustomerInquirie(ECN_Framework_Entities.Accounts.Customer customer, string firstName, string lastName, DateTime dateOfInquirie)
            : this(customer, -1, firstName, lastName, dateOfInquirie)
		{
			
		}

        public CustomerInquirie(ECN_Framework_Entities.Accounts.Customer customer, int licenseID, string firstName, string lastName, DateTime dateOfInquirie)
        {
			_customer = customer;
			_firstName = firstName;
			_lastName = lastName;
			_licenseID = licenseID;
			_dateOfInquirie = dateOfInquirie;
		}

		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		public bool IsNew {
			get { return _id <=0;}
		}

		private string _firstName;
		public string FirstName {
			get {
				return (this._firstName);
			}
			set {
				this._firstName = value;
			}
		}

		private string _lastName;
		public string LastName {
			get {
				return (this._lastName);
			}
			set {
				this._lastName = value;
			}
		}

		private DateTime _dateOfInquirie;
		public DateTime DateOfInquirie {
			get {
				return (this._dateOfInquirie);
			}
			set {
				this._dateOfInquirie = value;
			}
		}

		private string _notes;
		public string Notes {
			get {
				return (this._notes);
			}
			set {
				this._notes = value;
			}
		}		

		private ServiceLicense _inquirieLicense;
		public ServiceLicense InquirieLicense {
			get {
				if (_inquirieLicense == null) {					
					_inquirieLicense = ServiceLicense.GetServiceLicenseByID(_licenseID);
				}
				return (this._inquirieLicense);
			}
			set {
				this._inquirieLicense = value;
			}
		}

		public int LicenseID {
			get { return InquirieLicense.ID;}
		}

		// Used only for lazy loading when initialzed from database.
		private int _licenseID;

        private ECN_Framework_Entities.Accounts.Customer _customer;
        public ECN_Framework_Entities.Accounts.Customer Customer
        {
			get {
				return (this._customer);
			}
			set {
				this._customer = value;
			}
		}

		private Staff _customerServiceStaff;
		public Staff CustomerServiceStaff {
			get {
				return (this._customerServiceStaff);
			}
			set {
				this._customerServiceStaff = value;
			}
		}

		public string CustomerServiceName {
			get { return CustomerServiceStaff.FirstName;}
		}

		#region Database method
		public void Save() {
			SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
			
			if (IsNew) {
				string insertSQL = "INSERT INTO CustomerInquiries (CustomerID, LicenseID, FirstName, LastName, DateOfInquirie, Notes, CustomerServiceStaffID) VALUES (@customerID, @licenseID, @firstName, @lastName, @dateOfInquirie, @notes, @staffID);SELECT @@IDENTITY;";
				SqlCommand insert = new SqlCommand(insertSQL, conn);
				try {
					conn.Open();
					insert.Parameters.Add("@customerID", SqlDbType.Int, 4).Value = this.Customer.CustomerID;
					insert.Parameters.Add("@LicenseID", SqlDbType.Int, 4).Value = this.InquirieLicense.ID;
					insert.Parameters.Add("@firstName", SqlDbType.VarChar, 50).Value = this.FirstName;
					insert.Parameters.Add("@lastName", SqlDbType.VarChar, 50).Value = this.LastName;
					insert.Parameters.Add("@dateOfInquirie", SqlDbType.DateTime, 8).Value = this.DateOfInquirie;
					insert.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = this.Notes;
					insert.Parameters.Add("@staffID", SqlDbType.Int, 4).Value = CustomerServiceStaff.ID;
					insert.Prepare();
					this.ID = Convert.ToInt32(insert.ExecuteScalar());

					InquirieLicense.Usage.UsedCount +=1;
					InquirieLicense.UpdateUsage();
					return;
				} finally {
					insert.Dispose();
					conn.Close();
					conn.Dispose();
				}
			}

			string updateSql = "Update CustomerInquiries SET FirstName=@firstName,LastName= @lastName, DateOfInquirie=@dateOfInquirie, Notes=@notes, CustomerServiceStaffID = @staffID where CustomerInquirieID = @customerInquirieID;";
			SqlCommand update = new SqlCommand(updateSql, conn);
			try {
				conn.Open();				
				update.Parameters.Add("@firstName", SqlDbType.VarChar, 50).Value = this.FirstName;
				update.Parameters.Add("@lastName", SqlDbType.VarChar, 50).Value = this.LastName;
				update.Parameters.Add("@dateOfInquirie", SqlDbType.DateTime, 8).Value = this.DateOfInquirie;
				update.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = this.Notes;
				update.Parameters.Add("@staffID", SqlDbType.Int, 4).Value = CustomerServiceStaff.ID;
				update.Parameters.Add("@customerInquirieID", SqlDbType.Int, 4).Value = this.ID;
				update.Prepare();
				update.ExecuteNonQuery();				
				return;
			} finally {
				update.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

		public void Delete() {
			DataFunctions.Execute("DELETE FROM CustomerInquiries where CustomerInquirieID = " + ID.ToString());

			InquirieLicense.Usage.UsedCount -= 1;
			InquirieLicense.UpdateUsage();
		}
		#endregion

		#region Static Database methods
		public static ArrayList GetInquiriesByCustomerID(int customerID) {
			ArrayList inquiries = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable("SELECT * FROM CustomerInquiries where CustomerID = " + customerID.ToString());
			foreach(DataRow row in dt.Rows) {
				inquiries.Add(GetInquirie(row));
			}
			return inquiries;
		}

		public static CustomerInquirie GetInquiriesByID(int inquirieID) {
			ArrayList inquiries = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable("SELECT * FROM CustomerInquiries where CustomerInquirieID = " + inquirieID.ToString());

			if (dt.Rows.Count == 0) {
				return null;
			}
			
			return GetInquirie(dt.Rows[0]);
		}

		private static CustomerInquirie GetInquirie(DataRow row) {
            CustomerInquirie inquirie = new CustomerInquirie(ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(Convert.ToInt32(row["CustomerID"].ToString()), false), Convert.ToInt32(row["LicenseID"]), Convert.ToString(row["FirstName"]), Convert.ToString(row["LastName"]), Convert.ToDateTime(row["DateOfInquirie"]));
			inquirie.ID = Convert.ToInt32(row["CustomerInquirieID"]);
			inquirie.Notes = Convert.ToString(row["Notes"]);
			inquirie.CustomerServiceStaff = new Staff(Convert.ToInt32(row["CustomerServiceStaffID"]));
			return inquirie;
		}
		#endregion
	}
}
