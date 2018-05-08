using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.Reporting
{
    /// <summary>
    /// Interaction logic for ReportBuilder.xaml
    /// </summary>
    public partial class ReportBuilder : UserControl, INotifyPropertyChanged
    {
        #region Services
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productW = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupW = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportsW = FrameworkServices.ServiceClient.UAD_ReportsClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueW = FrameworkServices.ServiceClient.UAD_IssueClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryW = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        #endregion
        #region Response
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>> reportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>> issueResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        FrameworkUAS.Service.Response<DataTable> dtResponse = new FrameworkUAS.Service.Response<DataTable>();
        #endregion
        #region Entities/Lists
        private ObservableCollection<FrameworkUAD.Entity.ResponseGroup> responses = new ObservableCollection<FrameworkUAD.Entity.ResponseGroup>();
        private ObservableCollection<FrameworkUAD.Entity.ResponseGroup> _availableResponses = new ObservableCollection<FrameworkUAD.Entity.ResponseGroup>();
        private ObservableCollection<FrameworkUAD.Entity.Product> _productList = new ObservableCollection<FrameworkUAD.Entity.Product>();
        private ObservableCollection<FrameworkUAD_Lookup.Entity.Code> _reportTypes = new ObservableCollection<FrameworkUAD_Lookup.Entity.Code>();
        private ObservableCollection<Field> _responses = new ObservableCollection<Field>();
        private ObservableCollection<FrameworkUAD_Lookup.Entity.Country> _countries = new ObservableCollection<FrameworkUAD_Lookup.Entity.Country>();
        #endregion
        #region Properties and Classes
        private int _pubID = 0;
        private int _reportTypeID = 0;
        private string _column = "";
        private string _row = "";
        private string _country = "";
        public ObservableCollection<FrameworkUAD.Entity.ResponseGroup> AvailableResponses { get { return _availableResponses; } }
        public ObservableCollection<Field> Responses { get { return _responses; } }
        public ObservableCollection<FrameworkUAD.Entity.Product> ProductList { get { return _productList; } }
        public ObservableCollection<FrameworkUAD_Lookup.Entity.Country> Countries { get { return _countries; } }
        public ObservableCollection<FrameworkUAD_Lookup.Entity.Code> ReportTypes { get { return _reportTypes; } }
        List<string> staticCrossTabFields = new List<string>();
        public int PubID
        {
            get { return _pubID; }
            set
            {
                _pubID = value;
                GenerateFields(_pubID);
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PubID"));
                }
            }
        }
        public int? ReportTypeID
        {
            get { return _reportTypeID; }
            set
            {
                _reportTypeID = (value ?? -1);
                this.Row = "";
                this.Column = "";
                this.Country = "";
                Generate_Report();
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ReportID"));
                }
            }
        }
        public string Column
        {
            get { return _column; }
            set
            {
                _column = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Column"));
                }
            }
        }
        public string Row
        {
            get { return _row; }
            set
            {
                _row = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Row"));
                }
            }
        }
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Country"));
                }
            }
        }
        public class Field
        {
            private string _displayName;
            private string _name;
            public string DisplayName
            {
                get { return _displayName; }
                set
                {
                    _displayName = value;
                }
            }
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                }
            }

            public Field(string display, string name)
            {
                this.DisplayName = display;
                this.Name = name;
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        public ReportBuilder(bool showCircProducts, bool showUadProducts)
        {
            InitializeComponent();

            #region Load Lists
            productResponse = productW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(productResponse.Result, productResponse.Status))
            {
                if (showCircProducts == true)
                {
                    foreach (FrameworkUAD.Entity.Product p in productResponse.Result.Where(x => x.IsCirc == true))
                    {
                        if(!ProductList.Contains(p))
                            ProductList.Add(p);
                    }
                }

                if (showUadProducts == true)
                {
                    foreach (FrameworkUAD.Entity.Product p in productResponse.Result.Where(x => x.IsUAD == true))
                    {
                        if (!ProductList.Contains(p))
                            ProductList.Add(p);
                    }
                }
                rcbPubs.Items.SortDescriptions.Add(new SortDescription("PubCode",ListSortDirection.Ascending));
            }
            rGroupResponse = responseGroupW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if(Helpers.Common.CheckResponse(rGroupResponse.Result, rGroupResponse.Status))
                responses = new ObservableCollection<FrameworkUAD.Entity.ResponseGroup>(rGroupResponse.Result);

            codeResponse = codeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Report);
            if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                foreach (FrameworkUAD_Lookup.Entity.Code c in codeResponse.Result.Where(x => 
                        x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Cross_Tab.ToString().Replace("_", " ") || 
                        x.DisplayName ==FrameworkUAD_Lookup.Enums.ReportTypes.DemoXQualification.ToString().Replace("_", " ") ||
                        x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Geo_Single_Country.ToString().Replace("_", " ") ||
                        x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Single_Response.ToString().Replace("_", " ")))
                {
                    ReportTypes.Add(c);
                }
            }

            countryResponse = countryW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if(Helpers.Common.CheckResponse(countryResponse.Result, countryResponse.Status))
            {
                foreach(FrameworkUAD_Lookup.Entity.Country c in countryResponse.Result.Where(x=> !string.IsNullOrEmpty(x.FullName)).OrderBy(x=> x.ShortName))
                {
                    this.Countries.Add(c);
                }
            }
            #endregion

            //FrameworkUAD.Entity.ProductSubscription ps = new FrameworkUAD.Entity.ProductSubscription();
            //string regionCode = Core.Utilities.WPF.GetPropertyName(() => ps.RegionCode);
            //string country = Core.Utilities.WPF.GetPropertyName(() => ps.Country);
            //string company = Core.Utilities.WPF.GetPropertyName(() => ps.Company);
            //string title = Core.Utilities.WPF.GetPropertyName(() => ps.Title);
            //string zipCode = Core.Utilities.WPF.GetPropertyName(() => ps.ZipCode);
            //string city = Core.Utilities.WPF.GetPropertyName(() => ps.City);
            //string age = Core.Utilities.WPF.GetPropertyName(() => ps.Age);
            //string income = Core.Utilities.WPF.GetPropertyName(() => ps.Income);
            //string gender = Core.Utilities.WPF.GetPropertyName(() => ps.Gender);
            //staticCrossTabFields.Add(regionCode);
            //staticCrossTabFields.Add(country);
            //staticCrossTabFields.Add(company);
            //staticCrossTabFields.Add(title);
            //staticCrossTabFields.Add(zipCode);
            //staticCrossTabFields.Add(city);
            //staticCrossTabFields.Add(age);
            //staticCrossTabFields.Add(income);
            //staticCrossTabFields.Add(gender);

            //rcbProducts.SelectedIndex = 0;
        }

        private void GenerateFields(int pubID)
        {
            int circID = codeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.FirstOrDefault(x=> x.DisplayName == FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_Only.ToString().Replace("_", " ")).CodeId;
            int circUADID = codeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.FirstOrDefault(x => x.DisplayName == FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " ")).CodeId;

            Responses.Clear();
            foreach (string s in staticCrossTabFields)
                Responses.Add(new Field(s.ToUpper(), s.ToUpper()));

            foreach (FrameworkUAD.Entity.ResponseGroup rg in responses.Where(x => x.PubID == pubID && (x.ResponseGroupTypeId == circID || x.ResponseGroupTypeId == circUADID)))
                Responses.Add(new Field(rg.DisplayName.ToUpper(), rg.ResponseGroupName.ToUpper()));  
        }
        private void Generate_Report()
        {
            txtRow.Visibility = Visibility.Collapsed;
            txtCol.Visibility = Visibility.Collapsed;
            txtCross.Visibility = Visibility.Collapsed;
            rcbCol.Visibility = Visibility.Collapsed;
            rcbRow.Visibility = Visibility.Collapsed;
            txtCountry.Visibility = Visibility.Collapsed;
            rcbCountry.Visibility = Visibility.Collapsed;

            if(_reportTypeID == ReportTypes.Where(x=> x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Cross_Tab.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault())
            {
                txtRow.Visibility = Visibility.Visible;
                txtCol.Visibility = Visibility.Visible;
                txtCross.Visibility = Visibility.Visible;
                rcbCol.Visibility = Visibility.Visible;
                rcbRow.Visibility = Visibility.Visible;
            }
            else if(_reportTypeID == ReportTypes.Where(x=> x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Geo_Single_Country.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault())
            {
                txtCountry.Visibility = Visibility.Visible;
                rcbCountry.Visibility = Visibility.Visible;
            }
            else if (_reportTypeID == ReportTypes.Where(x => x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Single_Response.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
            {
                txtRow.Visibility = Visibility.Visible;
                rcbRow.Visibility = Visibility.Visible;
            }
        }
        private void Save_Report(object sender, RoutedEventArgs e)
        {
            FrameworkUAD.Entity.Report newReport = new FrameworkUAD.Entity.Report();
            newReport.IsActive = true;
            newReport.ProductID = this.PubID;
            newReport.DateCreated = DateTime.Now;
            newReport.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newReport.Status = true;
            if (_reportTypeID == ReportTypes.Where(x => x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Cross_Tab.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
            {
                if(this.Row != "" && this.Column != "" && this.Row != this.Column)
                {
                    Field row = Responses.Where(x => x.Name == this.Row).FirstOrDefault();
                    Field col = Responses.Where(x => x.Name == this.Column).FirstOrDefault();
                    newReport.IsCrossTabReport = true;
                    newReport.Column = this.Column;
                    newReport.Row = this.Row;
                    newReport.URL = "~/main/reports/CrossTabNew.aspx";
                    if(row != null && col != null)
                        newReport.ReportName = row.DisplayName + " X " + col.DisplayName;
                    newReport.ProductID = this.PubID;
                    newReport.ReportTypeID = _reportTypeID;
                }
                else
                    Core_AMS.Utilities.WPF.Message("You must select a unique Row and a unique Column before saving.");
            }
            else if (_reportTypeID == ReportTypes.Where(x => x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Geo_Single_Country.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
            {
                if(this.Country != "")
                {
                    newReport.IsCrossTabReport = false;
                    newReport.Row = this.Country;
                    newReport.URL = "~/main/reports/geoBreakdown_c.aspx";
                    newReport.ReportName = "Geographical Breakdown - " + this.Country;
                    newReport.ProductID = this.PubID;
                    newReport.ReportTypeID = _reportTypeID;
                }
                else
                    Core_AMS.Utilities.WPF.Message("You must select a Country before saving.");
            }
            else if (_reportTypeID == ReportTypes.Where(x => x.DisplayName == FrameworkUAD_Lookup.Enums.ReportTypes.Single_Response.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
            {
                if (this.Row != "")
                {
                    Field row = Responses.Where(x => x.Name == this.Row).FirstOrDefault();
                    newReport.IsCrossTabReport = false;
                    newReport.Row = this.Row;
                    newReport.URL = "~/main/reports/ListReport.aspx";
                    if(row != null)
                        newReport.ReportName = row.DisplayName + " Report";
                    newReport.ProductID = this.PubID;
                    newReport.ReportTypeID = _reportTypeID;
                }
                else
                    Core_AMS.Utilities.WPF.Message("You must select a Row before saving.");
            }
            if (newReport.ReportTypeID > 0)
            {
                FrameworkUAS.Service.Response<int> saveResponse = new FrameworkUAS.Service.Response<int>();
                saveResponse = reportsW.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                                                   newReport);
                if (Helpers.Common.CheckResponse(saveResponse.Result, saveResponse.Status))
                {
                    Core_AMS.Utilities.WPF.Message("Save successful");
                    this.ReportTypeID = -1;
                    rcbTypes.SelectedItem = null;
                }
            }
        }
    }
}
