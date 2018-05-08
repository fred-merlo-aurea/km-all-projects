using System;

namespace ecn.common.classes
{
	public class Contact
	{		
		public Contact() : this("","","","","","","","","","","") {}


		public Contact(string salutation, string contactName, string title, string phone, string fax, string email,
			string address, string city, string state, string country, string zip) {
			Salutation = salutation;
			ContactName = contactName;
			ContactTitle = title;
			Phone = phone;
			Fax = fax;
			Email = email;
			StreetAddress = address;
			City = city;
			State = state;
			Country = country;
			Zip = zip;

			ProcessFullName(contactName);
		}

		public Contact(string salutation, string firstName,string lastName, string title, string phone, string fax, string email,
			string address, string city, string state, string country, string zip) {
			Salutation = salutation;
			FirstName = firstName;
			LastName = lastName;
			ContactName = string.Format("{0} {1}", firstName, lastName);
			ContactTitle = title;
			Phone = phone;
			Fax = fax;
			Email = email;
			StreetAddress = address;
			City = city;
			State = state;
			Country = country;
			Zip = zip;		
		}

		#region Properties
		private string _salutation;
		public string Salutation {
			get {
				return (this._salutation);
			}
			set {
				this._salutation = value;
			}
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



		private string _contactName;
		public string ContactName {
			get {
				return (this._contactName);
			}
			set {
				this._contactName = value;
			}
		}


		private string _contactTitle;
		public string ContactTitle {
			get {
				return (this._contactTitle);
			}
			set {
				this._contactTitle = value;
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


		private string _email;
		public string Email {
			get {
				return (this._email);
			}
			set {
				this._email = value;
			}
		}

		private string _streetAddress;
		public string StreetAddress {
			get {
				return (this._streetAddress);
			}
			set {
				this._streetAddress = value;
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


		private string _country;
		public string Country {
			get {
				return (this._country);
			}
			set {
				this._country = value;
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
		#endregion

		private bool _isTheSameAsBillingContact;
		public bool IsTheSameAsBillingContact {
			get {
				return (this._isTheSameAsBillingContact);
			}
			set {
				this._isTheSameAsBillingContact = value;
			}
		}

		private bool _isTheSameAsTechContact;
		public bool IsTheSameAsTechContact {
			get {
				return (this._isTheSameAsTechContact);
			}
			set {
				this._isTheSameAsTechContact = value;
			}
		}

		private void ProcessFullName(string fullName) {
			string[] names = fullName.Split(new char[] {' '});
			if (names.Length == 1) {
				FirstName = names[0];				
				return;
			}

			if (names.Length == 2) {
				FirstName = names[0];
				LastName = names[1];
				return;
			}			
		}
		public const string EmailForBillingServiceNotification = "bservice@knowledgemarketing.com";
	}
}
