using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ecn.communicator.classes.Customers
{
	public class Store {
		public Store(int id) {
			_id = id;
		}


		#region Properties
		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private string _name;
		public string Name {
			get {
				return (this._name);
			}
			set {
				this._name = value;
			}
		}
	
		private string _manager;
		public string Manager {
			get {
				return (this._manager);
			}
			set {
				this._manager = value;
			}
		}

		private string _blurb;
		public string Blurb {
			get {
				return (this._blurb);
			}
			set {
				this._blurb = value;
			}
		}

		private string _address1;
		public string Address1 {
			get {
				return (this._address1);
			}
			set {
				this._address1 = value;
			}
		}

		private string _address2;
		public string Address2 {
			get {
				return (this._address2);
			}
			set {
				this._address2 = value;
			}
		}

		private string _city;
		public string City {
			get {
				return (this._city);
			}
			set {
				this._city = value;
			}
		}

		private string _state;
		public string State {
			get {
				return (this._state);
			}
			set {
				this._state = value;
			}
		}

		private string _zip;
		public string Zip {
			get {
				return (this._zip);
			}
			set {
				this._zip = value;
			}
		}

		private string _email;
		public string Email {
			get {
				return (this._email);
			}
			set {
				this._email = value;
			}
		}

		private string _hours;
		public string Hours {
			get {
				return (this._hours);
			}
			set {
				this._hours = value;
			}
		}

		private string _phone;
		public string Phone {
			get {
				return (this._phone);
			}
			set {
				this._phone = value;
			}
		}

		private string _fax;
		public string Fax {
			get {
				return (this._fax);
			}
			set {
				this._fax = value;
			}
		}

		private string _headerImg;
		public string HeaderImg {
			get {
				return (this._headerImg);
			}
			set {
				this._headerImg = value;
			}
		}

		private string _storeMap;
		public string StoreMap {
			get {
				return (this._storeMap);
			}
			set {
				this._storeMap = value;
			}
		}

		private string _storePhoto;
		public string StorePhoto {
			get {
				return (this._storePhoto);
			}
			set {
				this._storePhoto = value;
			}
		}
		#endregion

		#region Database Methods
		public void Save() {
			if (ID >0) {
				Update();
				return;
			}
			Insert();		
		}

		private void Insert() {
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["2ndWind"]);			
			SqlCommand cmd= new SqlCommand(@"
INSERT INTO Stores (Name,Manager,Blurb,Address1,Address2,City,State,Zip,EMail,Hours,Phone,Fax,HeaderImg,StoreMap,StorePhoto) 
VALUES (@name,@manager,@blurb,@address1,@address2,@city,@state,@zip,@email,@hours,@phone,@fax,@headerImg,@storeMap,@storePhoto)
Select @@IDENTITY;",conn);			
			try {
				conn.Open();				
				cmd.Parameters.Add("@name",SqlDbType.NVarChar,100).Value = this.Name;
				cmd.Parameters.Add("@Manager",SqlDbType.NVarChar,100).Value = this.Manager;
				cmd.Parameters.Add("@blurb",SqlDbType.NVarChar,255).Value = this.Blurb;
				cmd.Parameters.Add("@address1",SqlDbType.NVarChar,100).Value = this.Address1;
				cmd.Parameters.Add("@address2",SqlDbType.NVarChar,100).Value = this.Address2;
				cmd.Parameters.Add("@city",SqlDbType.NVarChar,100).Value = this.City;
				cmd.Parameters.Add("@State",SqlDbType.NVarChar,2).Value = this.State;
				cmd.Parameters.Add("@Zip", SqlDbType.NChar,5).Value = this.Zip;
				cmd.Parameters.Add("@Email",SqlDbType.NVarChar,100).Value = this.Email;
				cmd.Parameters.Add("@hours",SqlDbType.NVarChar,100).Value = this.Hours;
				cmd.Parameters.Add("@phone",SqlDbType.NVarChar,14).Value = this.Phone;
				cmd.Parameters.Add("@fax",SqlDbType.NVarChar,14).Value = this.Fax;
				cmd.Parameters.Add("@headerImg",SqlDbType.NVarChar,100).Value = this.HeaderImg;
				cmd.Parameters.Add("@storeMap",SqlDbType.NVarChar,100).Value = this.StoreMap;
				cmd.Parameters.Add("@storePhoto",SqlDbType.NVarChar,100).Value = this.StorePhoto;
				cmd.Prepare();
				ID = Convert.ToInt32(cmd.ExecuteScalar());
			} 
			finally {
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}	
		}

		private void Update() {
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["2ndWind"]);			
			SqlCommand cmd= new SqlCommand("UPDATE Stores set Name=@name,Manager=@manager,Blurb=@blurb,Address1=@address1,Address2=@address2,City=@city,State=@state,Zip=@zip,EMail=@email,Hours=@hours,Phone=@phone,Fax=@fax,HeaderImg=@headerImg,StoreMap=@storeMap,StorePhoto=@storePhoto where ID="+this.ID,conn);			
			try {
				conn.Open();
				cmd.Parameters.Add("@name",SqlDbType.NVarChar,100).Value = this.Name;
				cmd.Parameters.Add("@Manager",SqlDbType.NVarChar,100).Value = this.Manager;
				cmd.Parameters.Add("@blurb",SqlDbType.NVarChar,255).Value = this.Blurb;
				cmd.Parameters.Add("@address1",SqlDbType.NVarChar,100).Value = this.Address1;
				cmd.Parameters.Add("@address2",SqlDbType.NVarChar,100).Value = this.Address2;
				cmd.Parameters.Add("@city",SqlDbType.NVarChar,100).Value = this.City;
				cmd.Parameters.Add("@State",SqlDbType.NVarChar,2).Value = this.State;
				cmd.Parameters.Add("@Zip", SqlDbType.NChar,5).Value = this.Zip;
				cmd.Parameters.Add("@Email",SqlDbType.NVarChar,100).Value = this.Email;
				cmd.Parameters.Add("@hours",SqlDbType.NVarChar,100).Value = this.Hours;
				cmd.Parameters.Add("@phone",SqlDbType.NVarChar,14).Value = this.Phone;
				cmd.Parameters.Add("@fax",SqlDbType.NVarChar,14).Value = this.Fax;
				cmd.Parameters.Add("@headerImg",SqlDbType.NVarChar,100).Value = this.HeaderImg;
				cmd.Parameters.Add("@storeMap",SqlDbType.NVarChar,100).Value = this.StoreMap;
				cmd.Parameters.Add("@storePhoto",SqlDbType.NVarChar,100).Value = this.StorePhoto;
				cmd.Prepare();
				cmd.ExecuteNonQuery();
			} 
			finally {
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}	
		}
		#endregion



		public static ArrayList GetStores() {
			ArrayList stores = new ArrayList();

			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["2ndWind"]);			
			SqlDataAdapter adapter = new SqlDataAdapter("SELECT * from Stores order by Name",conn);
			DataSet ds = new DataSet();
			try {
				conn.Open();
				adapter.Fill(ds);
				foreach(DataRow row in ds.Tables[0].Rows) {					
					int storeID = Convert.ToInt32(row["ID"]);
					stores.Add(CreateStore(storeID, row));		
				}
				return stores;
			} catch(Exception e) {
				string errorMessage = e.Message;
				return null;
			}
			finally {
				adapter.Dispose();
				conn.Close();
				conn.Dispose();
			}	
		}

		public static Store GetStoreByID(int storeID) {
			if (storeID <=0) {
				return null;
			}
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["2ndWind"]);			
			SqlDataAdapter adapter = new SqlDataAdapter("SELECT * from Stores where ID = " + storeID,conn);
			DataSet ds = new DataSet();
			try {
				conn.Open();
				adapter.Fill(ds);
				if (ds.Tables[0].Rows.Count == 0) {
					return null;
				}
				DataRow row = ds.Tables[0].Rows[0];				
				return CreateStore(storeID, row);				
			} catch(Exception e) {
				string errorMessage = e.Message;
				return null;
			}
			finally {
				adapter.Dispose();
				conn.Close();
				conn.Dispose();
			}	
		}

		public static void Delete(int storeID) {
			if (storeID <=0) {
				return;
			}
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["2ndWind"]);			
			SqlCommand cmd = new SqlCommand("DELETE from Stores where ID = " + storeID,conn);
			
			try {
				conn.Open();
				cmd.ExecuteNonQuery();						
			} catch(Exception e) {
				string errorMessage = e.Message;				
			}
			finally {
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}	
		}

		private static Store CreateStore(int storeID, DataRow row) {
			Store store = new Store(storeID);				
			store.Name = Convert.ToString(row["Name"]);
			store.Manager = Convert.ToString(row["Manager"]);
			store.Blurb = Convert.ToString(row["Blurb"]);
			store.Address1 = Convert.ToString(row["Address1"]);
			store.Address2 = Convert.ToString(row["Address2"]);
			store.City = Convert.ToString(row["City"]);
			store.State = Convert.ToString(row["State"]);
			store.Zip = Convert.ToString(row["Zip"]);
			store.Email = Convert.ToString(row["Email"]);
			store.Hours = Convert.ToString(row["Hours"]);
			store.Phone = Convert.ToString(row["Phone"]);
			store.Fax = Convert.ToString(row["Fax"]);
			store.HeaderImg = Convert.ToString(row["HeaderImg"]);
			store.StoreMap = Convert.ToString(row["StoreMap"]);
			store.StorePhoto = Convert.ToString(row["StorePhoto"]);
			return store;
		}

	}
}
