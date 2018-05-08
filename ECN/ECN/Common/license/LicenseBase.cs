using System;
using ecn.common.classes;
using ecn.common.classes.billing;

namespace ecn.common.classes.license
{
	public abstract class LicenseBase
	{
		public LicenseBase(QuoteItem quoteItem)
		{
			_quoteItem = quoteItem;
		}

		private int _id = -1;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private QuoteItem _quoteItem;
		public QuoteItem QuoteItem {
			get {
				return (this._quoteItem);
			}
			set {
				this._quoteItem = value;
			}
		}

		private BillItem _billItem;
		public BillItem BillItem {
			get {
				return (this._billItem);
			}
			set {
				this._billItem = value;
			}
		}


		protected void SetUsageLicense(UsageLicense ul, string licenseTypeCode,  bool isActive) {
			string sql = string.Format(@"
DECLARE @licenseID as int
SET @licenseID = (select CLID from CustomerLicense where QuoteItemID = {1} AND ExpirationDate = '{5}' and LicenseTypeCode = '{2}')
if (@licenseID is null) 
BEGIN
	INSERT INTO CustomerLicense (CustomerID, QuoteItemID, LicenseTypeCode, Quantity, Used, ExpirationDate, AddDate, IsActive) VALUES
({0}, {1}, '{2}', {3}, {4}, '{5}', '{6}', {7});
END
ELSE
BEGIN
   UPDATE CustomerLicense SET Quantity = {3}, Used = {4}, IsActive = {7} WHERE CLID = @licenseID;
END", QuoteItem.Parent.Customer.ID, QuoteItem.ID, licenseTypeCode, ul.Quantity, ul.UsedCount, ul.EndDate.ToShortDateString(), ul.StartDate.ToShortDateString(), isActive?1:0) ;
			DataFunctions.Execute(sql);
		}

		#region Abstract Methos
		public abstract void Enable();
		public abstract void Disable();
		#endregion
	}
}
