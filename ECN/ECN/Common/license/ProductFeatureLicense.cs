using System;
using ecn.common.classes;
using ecn.common.classes.billing;
using ecn.common.classes.license;

namespace ecn.common.classes.license
{
	public class ProductFeatureLicense : LicenseBase
	{
		public ProductFeatureLicense(QuoteItem quoteItem, ProductFeature productFeature) : base(quoteItem) {
			_productFeature = productFeature;
		}

		private ProductFeature _productFeature;
		public ProductFeature ProductFeature {
			get {
				return (this._productFeature);
			}			
		}

		#region Overriden Methods
		public override void Enable() {	
			SetActive(true);
		}

		public override void Disable() {
			SetActive(false);
		}

		private void SetActive(bool isActive) {
			string sql = string.Format(@"
DECLARE @customerProductID int
SET @customerProductID = (select CustomerProductID from CustomerProduct where CustomerID = {0} and ProductDetailID = {1})
IF (@customerProductID is null)
   BEGIN
	INSERT INTO CustomerProduct (CustomerID, ProductDetailID, Active, UpdatedDate) VALUES
			({0}, {1}, '{2}', '{3}');
   END
ELSE
   BEGIN
	UPDATE CustomerProduct SET Active = '{2}', UpdatedDate = '{3}' where CustomerProductID = @CustomerProductID
   END", QuoteItem.Parent.Customer.ID, ProductFeature.ID, isActive?"y":"n", DateTime.Now);
			DataFunctions.Execute(sql);
		}
		#endregion
	}
}
