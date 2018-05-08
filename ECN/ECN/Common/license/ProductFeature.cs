using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ecn.common.classes.license
{	
	public class ProductFeature
	{
		public ProductFeature(int id) : this(null, id, "","") {}
		public ProductFeature(Product parent, int id, string name, string description)
		{
			Product = parent;
			ID = id;
			Name = name;
			Description = description;
		}

		private Product _product;
		public Product Product {
			get {
				return (this._product);
			}
			set {
				this._product = value;
			}
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

		private string _description;
		public string Description {
			get {
				return (this._description);
			}
			set {
				this._description = value;
			}
		}

		#region Static Database Methods
		public static ArrayList GetAllFeatures() {
			ArrayList features = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable("select pd.*, p.ProductName from ProductDetail pd join Product p on pd.ProductID = p.ProductID");			
			foreach(DataRow row in dt.Rows) {
				features.Add(GetFeature(row, new Product(Convert.ToInt32(row["ProductID"]), Convert.ToString(row["ProductName"]))));				
			}		
			return features;
		}

		public static ArrayList GetFeaturesByProductID(int productID) {
			ArrayList features = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable("SELECT * from ProductDetail where ProductID = " + productID.ToString());
			Product parent = Product.GetProductByID(productID);
			foreach(DataRow row in dt.Rows) {
				features.Add(GetFeature(row, parent));
			}
			return features;
		}

		private static ProductFeature GetFeature(DataRow row, Product parent) {
			return new ProductFeature(parent,Convert.ToInt32(row["ProductDetailID"]),Convert.ToString(row["ProductDetailName"]), Convert.ToString(row["ProductDetailDesc"]));
		}
		#endregion
	}
}
