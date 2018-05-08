using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class BillTo
    {
        #region Private
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _phoneExt;
        private string _company;
        private string _title;
        private string _mobile;
        private string _address;
        private string _address2;
        private string _address3;
        private string _fax;
        private string _city;
        private int _regionID;
        private string _fullZip;
        private string _zip;
        private string _plus4;
        private string _county;
        private string _email;
        private int _addressTypeCodeId;
        private int _countryId;
        private string _website;
        private string _country;
        private string _region;
        private bool _madeBillToChange;
        private bool _copyProfile;
        private bool _enabled;
        #endregion

        #region public

        public bool MadePaidBillToChange { get; set; }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
               
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                
            }
        }
        public string PhoneExt
        {
            get;
            set;
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
               
            }
        }
        public string PhonePrefix
        {
            get { return _phoneExt; }
            set
            {
                _phoneExt = value;
               
            }
        }
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
               
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                
            }
        }
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value;
               
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
               
            }
        }
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
               
            }
        }
        public string Address3
        {
            get { return _address3; }
            set
            {
                _address3 = value;
               
            }
        }
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;
               
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                
            }
        }
        public int RegionID
        {
            get { return _regionID; }
            set
            {
                _regionID = value;
               
            }
        }
        public string FullZip
        {
            get { return _fullZip; }
            set
            {
                _fullZip = (value ?? "");
                if (string.IsNullOrEmpty(_fullZip.Trim()))
                {
                    this.Zip = "";
                    this.Plus4 = "";
                }
                else if (_fullZip.Contains("-"))
                {
                    string[] fullZip;
                    fullZip = _fullZip.Split('-');
                    this.Zip = fullZip[0];
                    this.Plus4 = fullZip[1];
                }
                else if (_fullZip.Length == 5)
                {
                    this.Zip = _fullZip.Trim();
                    this.Plus4 = string.Empty;
                }
                else if (_fullZip.Length == 7)
                {
                    // Canada zip
                    this.Zip = _fullZip.Trim().Substring(0, 3);
                    this.Plus4 = _fullZip.Trim().Substring(4, 3);
                }
                else if (_fullZip.Length == 9)
                {
                    this.Zip = _fullZip.Trim().Substring(0, 5);
                    this.Plus4 = _fullZip.Trim().Substring(5, 4);
                }
                else
                {
                    this.Zip = _fullZip;
                    this.Plus4 = "";
                }
               
            }
        }
        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = value;
               
            }
        }
        public string Plus4
        {
            get { return _plus4; }
            set
            {
                _plus4 = value;
               
            }
        }
        public string County
        {
            get { return _county; }
            set
            {
                _county = value;
               
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
            }
               
        }
        public int AddressTypeCodeId
        {
            get { return _addressTypeCodeId; }
            set
            {
                _addressTypeCodeId = value;
               
            }
        }
        public int CountryID
        {
            get { return _countryId; }
            set
            {
                _countryId = value;
                
            }
        }
        public string Website
        {
            get { return _website; }
            set
            {
                _website = value;
            }
              
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;

            }
        }
        public bool CopyProfile
        {
            get { return _copyProfile; }
            set
            {
                _copyProfile = value;
               
            }
        }
        public List<SelectListItem> AddressTypeSelectList { get; set; }
        public List<SelectListItem> CountrySelectList { get; set; }
        public List<SelectListItem> RegionSelectList { get; set; }
        #endregion
        public BillTo()
        {

        }
        public BillTo(FrameworkUAD.Entity.PaidBillTo myPaidBillTo, EntityLists entlst)
        {
            this.AddressTypeSelectList = entlst.AddressTypesSelectList;
            this.CountrySelectList = entlst.CountrySelectList;
            this.RegionSelectList = entlst.StatesSelectList;
            this.FirstName = myPaidBillTo.FirstName;
            this.LastName = myPaidBillTo.LastName;
            this.Phone = myPaidBillTo.Phone;
            this.PhoneExt = myPaidBillTo.PhoneExt;
            this.Company = myPaidBillTo.Company;
            this.Title = myPaidBillTo.Title;
            this.Mobile = myPaidBillTo.Mobile;
            this.Address = myPaidBillTo.Address1;
            this.Address2 = myPaidBillTo.Address2;
            this.Address3 = myPaidBillTo.Address3;
            this.Fax = myPaidBillTo.Fax;
            this.City = myPaidBillTo.City;
            this.RegionID = myPaidBillTo.RegionID;
            this.Website = myPaidBillTo.Website;
            this.CountryID = myPaidBillTo.CountryID;
            this.Email = myPaidBillTo.Email;
            this.County = myPaidBillTo.County;

            if (myPaidBillTo.CountryID == 1 && !string.IsNullOrEmpty(myPaidBillTo.Plus4))
                this.FullZip = myPaidBillTo.ZipCode + "-" + myPaidBillTo.Plus4;
            else if (myPaidBillTo.CountryID == 2 && !string.IsNullOrEmpty(myPaidBillTo.Plus4))
                this.FullZip = myPaidBillTo.ZipCode + " " + myPaidBillTo.Plus4;
            else
                this.FullZip = myPaidBillTo.ZipCode;

            this.AddressTypeCodeId = myPaidBillTo.AddressTypeId;

            // Default phone prefix to 1 for U.S. and Canada
            if (this.CountryID != null)
            {
                if (this.CountryID == entlst.countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                    || this.CountryID == entlst.countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
                {
                    PhonePrefix = "1";
                }
                else
                {
                    FrameworkUAD_Lookup.Entity.Country country = entlst.countryList.Where(c => c.CountryID == myPaidBillTo.CountryID).FirstOrDefault();
                    if (country != null)
                        PhonePrefix = country.PhonePrefix.ToString();
                }
            }

        }
    }
}