using System;
using System.Collections;
using System.Text;
using ecn.common.classes.license;
using System.Collections.Generic;

namespace ecn.common.classes.billing
{
	
	/// Base class for QuoteItemOption and QuoteItem
	
	public abstract class QuoteItemBase
	{		
		public QuoteItemBase(string code, string name, string description, long quantity, double rate, LicenseTypeEnum licenseType, PriceTypeEnum priceType)
		{
			Quantity = quantity;
			Code = code;
			Name = name;
			Description = description;
			LicenseType = licenseType;
			Rate = rate;
			PriceType = priceType;
			_products = new ArrayList();
			_productFeatures = new ArrayList();
			_services = new ServiceCollection();
		}

		#region Properties
		private int _id = -1;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private string _code;
		public string Code {
			get {
				return (this._code);
			}
			set {
				this._code = value;
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

		private long _quantity;
		public long Quantity {
			get {
				return (this._quantity);
			}
			set {
				this._quantity = value;
			}
		}
		
		public Double ItemPrice {
			get {
				return Rate*Quantity;
			}			
		}
		
		private double _rate;
		public double Rate {
			get {
				return (this._rate);
			}
			set {
				this._rate = value;
			}
		}

		private LicenseTypeEnum _licenseType;
		public LicenseTypeEnum LicenseType {
			get {
				return (this._licenseType);
			}
			set {
				this._licenseType = value;
			}
		}

		private PriceTypeEnum _priceType;
		public PriceTypeEnum PriceType {
			get {
				return (this._priceType);
			}
			set {
				this._priceType = value;
			}
		}

		private bool _isCustomerCredit;
		public bool IsCustomerCredit {
			get {
				return (this._isCustomerCredit);
			}
			set {
				this._isCustomerCredit = value;
			}
		}

		private ArrayList _products;
		public ArrayList Products {
			get {
				return ArrayList.ReadOnly(this._products);
			}			
		}

        private List<KMPlatform.Entity.Service> _platformServices;
        public List<KMPlatform.Entity.Service> PlatformServices
        {
            get { return this.PlatformServices; }
        }

		private ArrayList _productFeatures;
		public ArrayList ProductFeatures {
			get {
				return (this._productFeatures);
			}
			set {
				this._productFeatures = value;
			}
		}

		private ServiceCollection _services;
		public ServiceCollection Services {
			get {
				return (this._services);
			}
		}

		#endregion

		public void RemoveProducts() {
			_products.Clear();
		}
		public void AddProduct(Product product) {
			if (product == null) {
				throw new ArgumentException("Null product can't be added.");
			}
			foreach(Product p in _products) {
				if (p.Name.ToLower() == product.Name.ToLower()) {
					throw new ArgumentException("Dupliated product can't be added.");
				}
			}
			_products.Add(product);
		}

        public void AddPlatformService(KMPlatform.Entity.Service service)
        {
            if (service == null)
                throw new ArgumentException("Null service can't be added.");
            foreach(KMPlatform.Entity.Service s in _platformServices)
            {
                if(s.ServiceName.ToLower() == service.ServiceName.ToLower())
                {
                    throw new ArgumentException("Duplicated platform service can't be added.");
                }
            }
            _platformServices.Add(service);
        }

		public void AddProducts(ArrayList products) {
			foreach(Product product in products) {
				AddProduct(product);
			}
		}

		public void RemoveProductFeatures() {
			_productFeatures.Clear();
		}
		public void AddProductFeature(ProductFeature productFeature) {
			if (productFeature == null) {
				throw new ArgumentException("Null product feature can't be added.");
			}

			foreach(ProductFeature pf in _productFeatures) {
				if (pf.Name.ToLower() == productFeature.Name.ToLower()) {
					throw new ArgumentException("Dupliated product feature can't be added.");
				}
			}
			_productFeatures.Add(productFeature);	
		}

		public void AddProductFeatures(ArrayList features) {
			foreach(ProductFeature f in features) {
				AddProductFeature(f);
			}
		}

		public void RemoveServices() {
			_services.Clear();
		}

		public void AddService(Service service) {
			if (service == null) {
				throw new ArgumentException("Null service can't be added.");
			}
			foreach(Service s in _services) {
				if (s.ServiceType == service.ServiceType) {
					throw new ArgumentException("Duplicated service can't be added.");
				}
			}
			_services.Add(service);
		}
		

		public string ProductNames {
			get { 
				if (_products == null || _products.Count ==0) {
					return string.Empty;
				}

				StringBuilder names = new StringBuilder();
				foreach(Product p in Products) {
					if (names.Length > 0) {
						names.Append("/");				
					}
					names.Append(p.Name);					
				}
				return names.ToString();
			}
		}

		public string ProductFeatureNames {
			get { 
				if (_productFeatures == null || _productFeatures.Count == 0) {
					return string.Empty;
				}

				StringBuilder names = new StringBuilder();
				foreach(ProductFeature f in ProductFeatures) {
					if (names.Length > 0) {
						names.Append("/");
					}
					names.Append(string.Format("{0}({1})",f.Product.Name, f.Name));
				}
				return names.ToString();
			}
		}

		protected string ProductIDs {
			get {
				StringBuilder ids = new StringBuilder();
				foreach(Product product in _products) {
					if (ids.Length == 0) {
						ids.Append(product.ID);
						continue;
					}
					ids.Append(string.Format(",{0}", product.ID));
				}
				return ids.ToString();
			}
		}

		protected string ProductFeatureIDs {
			get {
				StringBuilder ids = new StringBuilder();
				foreach(ProductFeature productFeature in _productFeatures) {
					if (ids.Length == 0) {
						ids.Append(productFeature.ID);
						continue;
					}
					ids.Append(string.Format(",{0}", productFeature.ID));
				}
				return ids.ToString();
			}
		}	
	
		protected bool needToSaveProductIDs {
			get {return ProductIDs.Trim().Length > 0;}
		}
		protected bool needToSaveProductFeatureIDs {
			get { return ProductFeatureIDs.Trim().Length > 0;}
		}

		#region Static Methods 
		protected static void LoadProducts(QuoteItemBase item, string productIDs) {
			if (productIDs.Trim().Length == 0) {
				return;
			}

			ArrayList products = Product.GetAllProducts();
			string[] ids = productIDs.Split(',');
			foreach(string id_str in ids) {
				int id = Convert.ToInt32(id_str);
				foreach(Product product in products) {
					if (product.ID == id) {
						item.AddProduct(product);
						break;
					}
				}
			}
		}

        //protected static void LoadPlatformServices(QuoteItemBase item, string serviceIDs)
        //{
        //    if(serviceIDs.Trim().Length == 0)
        //    {
        //        return;
        //    }

        //    List<KMPlatform.Entity.Service> services = new KMPlatform.BusinessLogic.Service().Select(false);
        //    string[] ids = serviceIDs.Split(',');
        //    foreach(string id_str in ids)
        //    {
        //        int id = Convert.ToInt32(id_str);
        //        foreach(KMPlatform.Entity.Service s in services)
        //        {
        //            if(s.ServiceID == id)
        //                item.
        //        }
        //    }
        //}


		protected static void LoadProductFeatures(QuoteItemBase item, string productFeatureIDs) {
			if  (productFeatureIDs.Trim().Length == 0) {
				return;
			}

			ArrayList productFeatures = ProductFeature.GetAllFeatures();
			string[] ids = productFeatureIDs.Split(',');
			foreach(string id_str in ids) {
				int id = Convert.ToInt32(id_str);
				foreach(ProductFeature feature in productFeatures) {
					if (feature.ID == id) {
						item.AddProductFeature(feature);
						break;
					}
				}
			}
		}

		protected static void LoadServices(QuoteItemBase item, string servicesString) {
			ServiceCollection services = Service.DeserializeServices(servicesString);
			foreach(Service service in services) {
				item.AddService(service);
			}
		}
		#endregion
	}
}
