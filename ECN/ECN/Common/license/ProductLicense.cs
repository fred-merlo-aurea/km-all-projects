using System;
using ecn.common.classes;
using ecn.common.classes.billing;
using ecn.common.classes.license;

namespace ecn.common.classes.license
{	
	public class ProductLicense : LicenseBase
	{
		public ProductLicense(QuoteItem quoteItem) : base(quoteItem) {
			foreach(Product product in QuoteItem.Products) {
				SetProductAccess(product, true);
			}
		}	
		
		private void SetProductAccess(Product product, bool canAccess) {
			switch(product.Name) {
				case "ecn.communicator":
					_isCommunicatorOn = canAccess;
					break;
				case "ecn.creator":
					_isCreatorOn = canAccess;
					break;
				case "ecn.collector":
					_isCollectorOn = canAccess;
					break;
				case "ecn.accounts":
					_isAccountsOn = canAccess;
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format("Can't handle product '{0}'.", product.Name));
			}
		}

		private bool _isCommunicatorOn;
		public bool IsCommunicatorOn {
			get {
				return (this._isCommunicatorOn);
			}			
		}

		private bool _isCreatorOn;
		public bool IsCreatorOn {
			get {
				return (this._isCreatorOn);
			}			
		}

		private bool _isCollectorOn;
		public bool IsCollectorOn {
			get {
				return (this._isCollectorOn);
			}			
		}

		private bool _isAccountsOn;
		public bool IsAccountsOn {
			get {
				return (this._isAccountsOn);
			}
		}

		#region Overriden Methods	
		public override void Enable() {
			int baseChannelID = QuoteItem.Parent.Customer.BaseChannelID;
			string sql = string.Format(@"UPDATE Customer SET CommunicatorLevel = {1}, CommunicatorChannelID = {2},
			CreatorLevel = {3}, CreatorChannelID = {4}, CollectorLevel = {5}, CollectorChannelID = {6}, AccountsLevel = {7} where CustomerID = {0};",
				QuoteItem.Parent.Customer.ID, 
				IsCommunicatorOn?3:0, IsCommunicatorOn?BaseChannel.GetChannelIDFromChannelCode(baseChannelID, "communicator"):0,
				IsCreatorOn?1:0, IsCreatorOn?BaseChannel.GetChannelIDFromChannelCode(baseChannelID, "creator"):0,
				IsCollectorOn?1:0, IsCollectorOn?BaseChannel.GetChannelIDFromChannelCode(baseChannelID,"collector"):0,
				0);
			DataFunctions.Execute(sql);
		}

		public override void Disable() {
			int baseChannelID = QuoteItem.Parent.Customer.BaseChannelID;
			string sql = string.Format(@"UPDATE Customer SET CommunicatorLevel = {1}, CommunicatorChannelID = {2},
			CreatorLevel = {3}, CreatorChannelID = {4}, CollectorLevel = {5}, CollectorChannelID = {6}, AccountsLevel = {7} where CustomerID = {0};",
				QuoteItem.Parent.Customer.ID, 
				0, 0,
				0, 0,
				0, 0,
				0);
			DataFunctions.Execute(sql);
		}		
		#endregion

	}
}
