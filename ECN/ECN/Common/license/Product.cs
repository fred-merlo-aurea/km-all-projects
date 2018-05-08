using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ecn.common.classes.license
{
	public class Product
	{
		public Product(int id, string name)
		{
			ID = id;
			Name = name;
			_features = new ArrayList();
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

		private string _name;
		public string Name {
			get {
				return (this._name);
			}
			set {
				this._name = value;
			}
		}

		private ArrayList _features;
		public ArrayList Features {
			get {
				return (this._features);
			}
			set {
				this._features = value;
			}
		}

		#region Static Database Methods
		public static ArrayList GetAllProducts() {
			ArrayList products = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable("SELECT * FROM Product");
			foreach(DataRow row in dt.Rows) {
				products.Add(GetProduct(row));
			}
			return products;
		}

		public static Product GetProductByID(int productID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * FROM Product WHERE ProductID = " + productID.ToString());
			if (dt.Rows.Count == 0) {
				return null;
			}

			return GetProduct(dt.Rows[0]);
		}

		private static Product GetProduct(DataRow row) {
			return new Product(Convert.ToInt32(row["ProductID"]), Convert.ToString(row["ProductName"]));
		}
		#endregion
	}
}
