using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for AdHocFilters.xaml
    /// </summary>
    public partial class AdHocFilters : UserControl, INotifyPropertyChanged
    {
        //DEPRECATED -- FilterControls now controls this.
        #region Properties
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsExpanded"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subscriberData = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> pubSubscriptionData = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public List<AdHocFilterField> filterFields = new List<AdHocFilterField>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();

        public class AdHocFilterField : INotifyPropertyChanged
        {
            private string _Value, _ToValue, _FromValue, _SelectedCondition;
            public string FilterObject { get; set; }
            public List<string> Conditions { get; set; }
            public string SelectedCondition
            {
                get { return _SelectedCondition; }
                set
                {
                    if(_SelectedCondition != null && _SelectedCondition != "")
                    {
                        this.Value = "";
                    }
                    this.ToValue = "";
                    this.FromValue = "";
                    _SelectedCondition = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedCondition"));
                    }
                }
            }
            public string Type { get; set; }
            public string Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                    }
                }
            }
            public string ToValue
            {
                get { return _ToValue; }
                set
                {
                    _ToValue = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ToValue"));
                    }
                }
            }
            public string FromValue
            {
                get { return _FromValue; }
                set
                {
                    _FromValue = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FromValue"));
                    }
                }
            }

            public AdHocFilterField(string filterObj, List<string> conditions, string type)
            {
                this.FilterObject = filterObj;
                this.Conditions = conditions;
                this.Type = type;
                this.Value = "";
                this.ToValue = "";
                this.FromValue = "";
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public AdHocFilters(int pubID)
        {
            InitializeComponent();

            codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
            if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                filterTypes = codeResponse.Result;
            }

            List<string> exclude = new List<string>()
            {
                "SubscriberID", "ExternalKeyID", "Occupation", "AddressTypeID", "RegionCode", "RegionID", "CarrierRoute", "CountryID", "Latitude", 
                "Longitude", "IsAddressValidated", "AddressValidationDate", "AddressValidationSource", "AddressValidationMessage", "Birthdate", "Age",
                "Income", "Gender", "DateCreated", "DateUpdated", "CreatedByUserID", "UpdatedByUserID", "tmpSubscriptionID", "IsLocked", "LockedByUserID",
                "LockDate", "LockDateRelease", "AddressTypeCodeId", "AddressLastUpdatedDate", "AddressUpdatedSourceTypeCodeId", "IGrp_No", "SFRecordIdentifier",
                "FullName", "FullAddress", "PublicationToolTip", "ProspectList", "SubscriptionList", "SubscriptionSearchResults", "MarketingMapList", "IsComp",
                "ResponseMapList", "FullZip", "SSequenceID", "PhoneCode", "AccountNumber", "SSubscriptionID", "SPublisherName" ,"SPublicationCode" ,"SIsSubscribed",
                "SSubscriptionStatusID","SubscriptionID", "PublisherID", "SubscriberID", "PublicationID", "ActionID_Current", "ActionID_Previous", "SubscriptionStatusID",
                "IsPaid", "QSourceID", "QSourceDate", "DeliverabilityID", "IsSubscribed", "OriginalSubscriberSourceCode",
                "DateCreated", "DateUpdated", "CreatedByUserID", "UpdatedByUserID", "Par3cID", "SubsrcTypeID", "IsNewSubscription", "AddRemoveID",
                "IMBSeq", "IsActive","SubscriptionPaidID", "SubscriptionID", "PriceCodeID", "CPRate", "BalanceDue", "CCHolderName", "DeliverID", "GraceIssues", "WriteOffAmount", "OtherType", "DateCreated",
                "DateUpdated", "CreatedByUserID", "UpdatedByUserID", "AccountNumber", "ClientName", "PubCategoryID", "PubCode", "PubID", "PubName", "PubQSourceID",
                "PubTransactionID", "PubTypeDisplayName", "Status", "StatusUpdatedDate", "StatusUpdatedReason", "SubSrcID", "SubGenSubscriberID", "SubGenSubscriptionID", "SubGenPublicationID",
                "SubGenMailingAddressId", "SubGenBillingAddressId", "IssuesLeft", "UnearnedReveue", "AdHocFields"
            };

            List<string> adHocs = new List<string>();
            FrameworkUAS.Service.Response<List<string>> adHocResponse = new FrameworkUAS.Service.Response<List<string>>();
            adHocResponse = pubSubscriptionData.Proxy.Get_AdHocs(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

            if (Helpers.Common.CheckResponse(adHocResponse.Result, adHocResponse.Status))
                adHocs = adHocResponse.Result;

            FrameworkUAD.Entity.ProductSubscription ps = new FrameworkUAD.Entity.ProductSubscription();
            List<System.Reflection.PropertyInfo> uadProperties = ps.GetType().GetProperties().ToList();

            List<string> standardCon = new List<string>(){"CONTAINS", "EQUAL", "START WITH", "END WITH", "DOES NOT CONTAIN", "NO RESPONSE", "ANY RESPONSE"};
            List<string> rangeCon = new List<string>() { "RANGE", "EQUAL", "GREATER THAN", "LESSER THAN" };
            List<string> dateCon = new List<string>() { "RANGE", "Year", "Month" };            

            foreach(System.Reflection.PropertyInfo i in uadProperties)
            {
                if (!exclude.Contains(i.Name.Trim()))
                {
                    if (i.PropertyType == typeof(System.Int32) || i.PropertyType == typeof(System.Decimal))
                        filterFields.Add(new AdHocFilterField(i.Name.Trim(), rangeCon, "Range"));
                    else if (i.PropertyType == typeof(System.String))
                        filterFields.Add(new AdHocFilterField(i.Name.Trim(), standardCon, "Standard"));
                    else if (i.PropertyType == typeof(System.DateTime))
                        filterFields.Add(new AdHocFilterField(i.Name.Trim(), dateCon, "DateRange"));
                }
            }

            foreach(string s in adHocs)
            {
                filterFields.Add(new AdHocFilterField(s, standardCon, "AdHoc"));
            }

            filterFields.Sort((x, y) => x.FilterObject.CompareTo(y.FilterObject));
            filterFields = filterFields.Distinct().ToList();
            icAdHocFields.ItemsSource = filterFields;
        }

        public List<Helpers.FilterOperations.FilterDetailContainer> GetSelection()
        {
            List<Helpers.FilterOperations.FilterDetailContainer> myDetailContainers = new List<Helpers.FilterOperations.FilterDetailContainer>();
            foreach(AdHocFilterField aff in icAdHocFields.Items)
            {
                Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                fdc.FilterDetail.SearchCondition = aff.SelectedCondition;
                fdc.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.AdHoc.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
                if (aff.Type == "Range" && aff.SelectedCondition != null)
                {
                    if (aff.FromValue != null || aff.ToValue != null)
                    {
                        if (aff.FromValue == null)
                            aff.FromValue = "0";

                        fdc.FilterDetail.AdHocFieldValue = "";
                        fdc.FilterDetail.AdHocFromField = aff.FromValue;
                        fdc.FilterDetail.AdHocToField = aff.ToValue;
                        fdc.FilterDetail.FilterField = aff.FilterObject;
                        fdc.FilterDetail.FilterObjectType = aff.Type;
                        myDetailContainers.Add(fdc);
                    }

                }
                else if ((aff.Type == "Standard" || aff.Type == "AdHoc") && aff.SelectedCondition != null)
                {
                    if (aff.Value != null && aff.Value != "")
                    {
                        fdc.FilterDetail.AdHocFieldValue = aff.Value;
                        fdc.FilterDetail.AdHocFromField = "";
                        fdc.FilterDetail.AdHocToField = "";
                        fdc.FilterDetail.FilterField = aff.FilterObject;
                        fdc.FilterDetail.FilterObjectType = aff.Type;
                        myDetailContainers.Add(fdc);
                    }
                    else if(aff.SelectedCondition == "NO RESPONSE" || aff.SelectedCondition == "ANY RESPONSE")
                    {
                        fdc.FilterDetail.AdHocFieldValue = "";
                        fdc.FilterDetail.AdHocFromField = "";
                        fdc.FilterDetail.AdHocToField = "";
                        fdc.FilterDetail.FilterField = aff.FilterObject;
                        fdc.FilterDetail.FilterObjectType = aff.Type;
                        myDetailContainers.Add(fdc);
                    }
                }
                else if (aff.Type == "DateRange" && aff.SelectedCondition != null)
                {
                    if (aff.FromValue != null || aff.ToValue != null)
                    {
                        if (aff.FromValue == null && aff.SelectedCondition == "RANGE")
                            aff.FromValue = DateTime.MinValue.ToString();
                        else if (aff.FromValue == null && aff.SelectedCondition == "Year")
                            aff.FromValue = DateTime.MinValue.Year.ToString();
                        else if (aff.FromValue == null && aff.SelectedCondition == "Month")
                            aff.FromValue = DateTime.MinValue.Month.ToString();

                        fdc.FilterDetail.AdHocFieldValue = "";
                        fdc.FilterDetail.AdHocFromField = aff.FromValue;
                        fdc.FilterDetail.AdHocToField = aff.ToValue;
                        fdc.FilterDetail.FilterField = aff.FilterObject;
                        fdc.FilterDetail.FilterObjectType = aff.Type;
                        myDetailContainers.Add(fdc);
                    }
                }
            }
            return myDetailContainers;
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            if (_isExpanded)
                this.IsExpanded = false;
            else
                this.IsExpanded = true;
        }
    }
}
