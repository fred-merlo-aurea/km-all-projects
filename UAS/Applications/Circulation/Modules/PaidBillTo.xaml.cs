using Circulation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for PaidBillTo.xaml
    /// </summary>
    public partial class PaidBillTo : UserControl, INotifyPropertyChanged
    {
        #region Properties

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
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                myPaidBillTo.FirstName = _firstName;
                if (_firstName != origPaidBillTo.FirstName)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                }
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                myPaidBillTo.LastName = _lastName;
                if (_lastName != origPaidBillTo.LastName)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                }
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                myPaidBillTo.Phone = _phone;
                if (_phone != origPaidBillTo.Phone)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
                }
            }
        }
        public string PhoneExt
        {
            get { return _phoneExt; }
            set
            {
                _phoneExt = value;
                myPaidBillTo.PhoneExt = _phoneExt;
                if (_phoneExt != origPaidBillTo.PhoneExt)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PhoneExt"));
                }
            }
        }
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                myPaidBillTo.Company = _company;
                if (_company != origPaidBillTo.Company)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Company"));
                }
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                myPaidBillTo.Title = _title;
                if (_title != origPaidBillTo.Title)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value;
                myPaidBillTo.Mobile = _mobile;
                if (_mobile != origPaidBillTo.Mobile)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Mobile"));
                }
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                myPaidBillTo.Address1 = _address;
                if (_address != origPaidBillTo.Address1)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                }
            }
        }
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                myPaidBillTo.Address2 = _address2;
                if (_address2 != origPaidBillTo.Address2)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address2"));
                }
            }
        }
        public string Address3
        {
            get { return _address3; }
            set
            {
                _address3 = value;
                myPaidBillTo.Address3 = _address3;
                if (_address3 != origPaidBillTo.Address3)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address3"));
                }
            }
        }
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;
                myPaidBillTo.Fax = _fax;
                if (_fax != origPaidBillTo.Fax)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Fax"));
                }
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                myPaidBillTo.City = _city;
                if (_city != origPaidBillTo.City)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("City"));
                }
            }
        }
        public int RegionID
        {
            get { return _regionID; }
            set
            {
                _regionID = value;
                myPaidBillTo.RegionID = _regionID;
                if (myPaidBillTo != null && regionList != null && _regionID != origPaidBillTo.RegionID)
                {
                    if (_regionID <= 0)
                        myPaidBillTo.RegionCode = string.Empty;
                    else
                        myPaidBillTo.RegionCode = regionList.Where(r => r.RegionID == _regionID).Select(s => s.RegionCode).SingleOrDefault();
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RegionID"));
                }
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
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FullZip"));
                }
            }
        }
        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = value;
                myPaidBillTo.ZipCode = _zip;
                if (_zip != origPaidBillTo.ZipCode)
                    this.MadePaidBillToChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Zip"));
                }
            }
        }
        public string Plus4
        {
            get { return _plus4; }
            set
            {
                _plus4 = value;
                myPaidBillTo.Plus4 = _plus4;
                if (_plus4 != origPaidBillTo.Plus4)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Plus4"));
                }
            }
        }
        public string County
        {
            get { return _county; }
            set
            {
                _county = value;
                myPaidBillTo.County = _county;
                if (_county != origPaidBillTo.County)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("County"));
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                myPaidBillTo.Email = _email;
                if (_email != origPaidBillTo.Email)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                }
            }
        }
        public int AddressTypeCodeId
        {
            get { return _addressTypeCodeId; }
            set
            {
                _addressTypeCodeId = value;
                myPaidBillTo.AddressTypeId = _addressTypeCodeId;
                if (_addressTypeCodeId != origPaidBillTo.AddressTypeId)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AddressTypeCodeId"));
                }
            }
        }
        public int CountryID
        {
            get { return _countryId; }
            set
            {
                _countryId = value;
                myPaidBillTo.CountryID = _countryId;
                LoadCountryChanges(_countryId);
                if(_countryId != origPaidBillTo.CountryID)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CountryID"));
                }
            }
        }
        public string Website
        {
            get { return _website; }
            set
            {
                _website = value;
                myPaidBillTo.Website = _website;
                if (_website != origPaidBillTo.Website)
                {
                    this.MadePaidBillToChange = true;
                }
                else if (CheckChanges() == false)
                    this.MadePaidBillToChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Website"));
                }
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }
        public bool MadePaidBillToChange
        {
            get { return _madeBillToChange; }
            set
            {
                if (_madeBillToChange != value)
                {
                    _madeBillToChange = value;
                    if (InfoChanged != null)
                        InfoChanged(_madeBillToChange);
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MadePaidBillToChange"));
                }
            }
        }
        public bool CopyProfile
        {
            get { return _copyProfile; }
            set
            {
                _copyProfile = value;
                if(CopyProfileInfo != null)
                    CopyProfileInfo(_copyProfile);

                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CopyProfile"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<bool> CopyProfileInfo;
        public event Action<bool> InfoChanged;
        #endregion
        #region Entity/List/Variable
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> addressTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Region> regionList = new List<FrameworkUAD_Lookup.Entity.Region>();
        private FrameworkUAD.Entity.PaidBillTo myPaidBillTo = new FrameworkUAD.Entity.PaidBillTo();
        private FrameworkUAD.Entity.PaidBillTo origPaidBillTo = new FrameworkUAD.Entity.PaidBillTo();
        private FrameworkUAD_Lookup.Entity.Country selCountryPhonePrefix = new FrameworkUAD_Lookup.Entity.Country();
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        #endregion
        #region Workers
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker;//IAddressType
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> uasc;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> rWorker;
        #endregion

        public PaidBillTo(FrameworkUAD.Entity.PaidBillTo paidBillTo)
        {
            InitializeComponent();
            myPaidBillTo = paidBillTo;
            codeTypeList = Home.CodeTypes;
            codeList = Home.Codes;
            countryList = Home.Countries;
            regionList = Home.Regions;
            origPaidBillTo = new FrameworkUAD.Entity.PaidBillTo(myPaidBillTo);
            this.DataContext = this;

            MadePaidBillToChange = false;

            codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            uasc = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
            rWorker = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();

            int addrTypeID = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Address.ToString())).CodeTypeId;
            addressTypes = codeList.Where(x => x.CodeTypeId == addrTypeID).ToList();
            if (addressTypes.Where(x => x.CodeId == 0).Count() == 0)
                addressTypes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, DisplayName = "", IsActive = true });

            cbAddressType.ItemsSource = addressTypes.OrderBy(x=> x.CodeId);
            cbAddressType.DisplayMemberPath = "CodeName";
            cbAddressType.SelectedValuePath = "CodeId";

            if (countryList.Where(x => x.CountryID == 0).Count() == 0)
                countryList.Add(new FrameworkUAD_Lookup.Entity.Country() { CountryID = 0, ShortName = "", FullName = "", SortOrder = 0 });

            cbCountry.ItemsSource = countryList.OrderByDescending(o => o.CountryID == 0).ThenByDescending(o => o.CountryID == 1).ThenByDescending(s => s.CountryID == 2).ThenByDescending(s => s.CountryID == 429).ThenBy(x => x.ShortName);
            cbCountry.SelectedValuePath = "CountryID";
            cbCountry.DisplayMemberPath = "ShortName";

            if (regionList.Where(x => x.RegionID == 0).Count() == 0)
                regionList.Add(new FrameworkUAD_Lookup.Entity.Region() { RegionID = 0, RegionCode = "", RegionName = "", RegionGroupID = 0, CountryID = 0 });

            cbState.ItemsSource = regionList.OrderByDescending(r=> r.CountryID == 0).OrderBy(r => r.RegionName).ToList();
            cbState.SelectedValuePath = "RegionID";
            cbState.DisplayMemberPath = "RegionName";

            LoadData();
        }

        private void LoadData()
        {
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
            if (this.CountryID != null && (string.IsNullOrEmpty(tbPhoneCode.Text) || tbPhoneCode.Text == "0"))
            {
                if (this.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                    || this.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
                {
                    tbPhoneCode.Text = "1";
                }
                else
                {
                    FrameworkUAD_Lookup.Entity.Country country = countryList.Where(c => c.CountryID == myPaidBillTo.CountryID).FirstOrDefault();
                    if (country != null)
                        tbPhoneCode.Text = country.PhonePrefix.ToString();
                }
            }
            this.MadePaidBillToChange = false;
        }

        #region UI Events
        private void tbZipPlus4_TextInput(object sender, TextCompositionEventArgs e)
        {
            Common.AlphaNumericOnly(sender, e);
            tbZipPlus4.Text = Common.zipPlus4TextInput(Convert.ToInt32(cbCountry.SelectedValue), tbZipPlus4.Text);
            tbZipPlus4.CaretIndex = tbZipPlus4.Text.Length;
        }
        private void tbZipPlus4_LostFocus(object sender, RoutedEventArgs ev)
        {
            if (!Common.ZipValidation(this.CountryID, this.FullZip))
                ev.Handled = true;
        }
        public void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Common.PhoneNumberValidation(sender, e);
            Common.PhoneAutoFormat(tb);
            string num = string.Empty;
            num = tb.Text;

            if (tb.Name.Equals("tbPhone"))
                Phone = num;
            else if (tb.Name.Equals("tbMobile"))
                Mobile = num;
            else if (tb.Name.Equals("tbFax"))
                Fax = num;

            tb.CaretIndex = tb.Text.Length;
        }
        public void tbPhone_NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Common.PhoneNumberValidation(sender, e);
        }
        #endregion
        #region Helpers
        private void LoadCountryChanges(int countryID)
        {
            #region States

            if (countryID.Equals(1) || countryID.Equals(2) || countryID.Equals(3))
            {
                if (countryID.Equals(1))
                {
                    cbState.ItemsSource = regionList.Where(x => x.CountryID == 1).ToList();
                    cbState.SelectedValuePath = "RegionID";
                    cbState.DisplayMemberPath = "RegionName";
                }
                if (countryID.Equals(2))
                {
                    cbState.SelectedValue = -1;
                    cbState.ItemsSource = regionList.Where(x => x.CountryID == 2).ToList();
                    cbState.SelectedValuePath = "RegionID";
                    cbState.DisplayMemberPath = "RegionName";
                }
                if (countryID.Equals(3))
                {
                    cbState.ItemsSource = regionList.Where(x => x.CountryID == 1 || x.CountryID == 2).OrderBy(x => x.CountryID).ThenBy(x => x.RegionName).ToList();
                    cbState.SelectedValuePath = "RegionID";
                    cbState.DisplayMemberPath = "RegionName";
                }

            }
            else if (countryID.Equals(429))
            {
                cbState.SelectedValue = -1;
                cbState.ItemsSource = regionList.Where(x => x.CountryID == 0 || x.CountryID == 429).OrderBy(x => x.CountryID).ThenBy(x => x.RegionName).ToList();
                cbState.SelectedValuePath = "RegionID";
                cbState.DisplayMemberPath = "RegionName";
            }
            else
            {
                cbState.ItemsSource = regionList.Where(x => x.CountryID == 0).OrderBy(x=> x.RegionID).ToList();
                cbState.SelectedValuePath = "RegionID";
                cbState.DisplayMemberPath = "RegionName";
                //cbState.SelectedValue = 52;
            }
            #endregion
            #region Phone Prefix
            selCountryPhonePrefix = countryList.SingleOrDefault(x => x.CountryID == countryID);
            if (countryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                || countryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
            {
                tbPhoneCode.Text = "1";
            }
            else if (selCountryPhonePrefix != null)
            {
                selCountryPhonePrefix = countryList.SingleOrDefault(x => x.CountryID == countryID);
                tbPhoneCode.Text = selCountryPhonePrefix.PhonePrefix.ToString();
            }
            else
            {
                tbPhoneCode.Text = "";
            }
            #endregion
            #region ZipCode
            if (!string.IsNullOrEmpty(tbZipPlus4.Text) && (countryID.Equals(1) || countryID.Equals(2)))
            {
                Regex usZipRegEx = new Regex("(^[0-9]{5}$)|(^[0-9]{5}-[0-9]{4}$)");
                Regex caZipRegEx = new Regex("(^[A-Z]{1}[0-9]{1}[A-Z]{1}\\s[0-9]{1}[A-Z]{1}[0-9]$)");

                bool validZipCode;
                if ((usZipRegEx.IsMatch(tbZipPlus4.Text)) || (caZipRegEx.IsMatch(tbZipPlus4.Text)))
                    validZipCode = true;
                else
                    validZipCode = false;


                if (!validZipCode)
                {
                    if (countryID.Equals(1))
                    {
                        //USA                        
                        if (!tbZipPlus4.Text.Contains("-") & tbZipPlus4.Text.Length == 9)
                            tbZipPlus4.Text = tbZipPlus4.Text.Substring(0, 5) + "-" + tbZipPlus4.Text.Substring(5);

                    }
                    else if (countryID.Equals(2))
                    {
                        //CANADA                        
                        if (!tbZipPlus4.Text.Contains(" ") & tbZipPlus4.Text.Length == 6)
                            tbZipPlus4.Text = tbZipPlus4.Text.Substring(0, 3) + " " + tbZipPlus4.Text.Substring(3);

                    }
                }

                if ((usZipRegEx.IsMatch(tbZipPlus4.Text)) || (caZipRegEx.IsMatch(tbZipPlus4.Text)))
                    validZipCode = true;
                else
                    Core_AMS.Utilities.WPF.Message("Postal Code is invalid.", MessageBoxButton.OK,
                        MessageBoxImage.Information, "Invalid Postal Code");
            }
            #endregion
            #region Load Country Name
            string countryName = countryList.Where(x => x.CountryID == countryID).Select(x => x.ShortName).FirstOrDefault();
            if (countryName != "" && countryName != null)
                myPaidBillTo.Country = countryName;
            #endregion
        }
        private bool CheckChanges()
        {
            bool infoChanged = false;
            if (this.FirstName != origPaidBillTo.FirstName)
                infoChanged = true;
            if (this.LastName != origPaidBillTo.LastName)
                infoChanged = true;
            if (this.Phone != origPaidBillTo.Phone)
                infoChanged = true;
            if (this.PhoneExt != origPaidBillTo.PhoneExt)
                infoChanged = true;
            if (this.Company != origPaidBillTo.Company)
                infoChanged = true;
            if (this.Title != origPaidBillTo.Title)
                infoChanged = true;
            if (this.Mobile != origPaidBillTo.Mobile)
                infoChanged = true;
            if (this.Address != origPaidBillTo.Address1)
                infoChanged = true;
            if (this.Address2 != origPaidBillTo.Address2)
                infoChanged = true;
            if (this.Address3 != origPaidBillTo.Address3)
                infoChanged = true;
            if (this.Fax != origPaidBillTo.Fax)
                infoChanged = true;
            if (this.City != origPaidBillTo.City)
                infoChanged = true;
            if (this.RegionID != origPaidBillTo.RegionID)
                infoChanged = true;
            if (this.Zip != origPaidBillTo.ZipCode)
                infoChanged = true;
            if (this.Plus4 != origPaidBillTo.Plus4)
                infoChanged = true;
            if (this.County != origPaidBillTo.County)
                infoChanged = true;
            if (this.Email != origPaidBillTo.Email)
                infoChanged = true;
            if (this.AddressTypeCodeId != origPaidBillTo.AddressTypeId)
                infoChanged = true;
            if (this.CountryID != origPaidBillTo.CountryID)
                infoChanged = true;
            if (this.Website != origPaidBillTo.Website)
                infoChanged = true;

            return infoChanged;
        }
        #endregion

        private void txtPhoneExt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
