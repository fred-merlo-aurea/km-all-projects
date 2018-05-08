using System;

namespace ecn.common.classes.billing
{	
	public class CreditCard
	{
		public CreditCard(string number, string cardType, DateTime expirationDate, string securityNumber) {
			_cardNumber = number;
			_cardType = cardType;
			_expirationDate = expirationDate;
			_securityNumber = securityNumber;
		}
	
		private string _cardNumber;
		public string CardNumber {
			get {
				return (this._cardNumber);
			}
			set {
				this._cardNumber = value;
			}
		}

		public string MaskedCardNumber {
			get {
				if (_cardNumber == null || _cardNumber.Length <= 4 ) {
					return string.Empty;
				}

				return new string('X', _cardNumber.Length-4) + _cardNumber.Substring(_cardNumber.Length-4, 4);
			}
		}

		private string _cardType;
		public string CardType {
			get {
				return (this._cardType);
			}
			set {
				this._cardType = value;
			}
		}

		private DateTime _expirationDate;
		public DateTime ExpirationDate {
			get {
				return (this._expirationDate);
			}
			set {
				this._expirationDate = value;
			}
		}

		private string _securityNumber;
		public string SecurityNumber {
			get {
				return (this._securityNumber);
			}
			set {
				this._securityNumber = value;
			}
		}

		private Contact _billingContact;
		public Contact BillingContact {
			get {
				return (this._billingContact);
			}
			set {
				this._billingContact = value;
			}
		}
	}
}
