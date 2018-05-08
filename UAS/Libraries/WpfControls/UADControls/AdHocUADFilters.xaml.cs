using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for AdHocUADFilters.xaml
    /// </summary>
    public partial class AdHocUADFilters : UserControl
    {
        //private FrameworkServices.ServiceClient<Circulation_WS.Interface.ISubscriber> subscriberData = FrameworkServices.ServiceClient.Circ_SubscriberClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IAdhoc> adHocData = FrameworkServices.ServiceClient.UAD_AdhocClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>> adHocResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>>();
        public List<AdHocFilterField> filterFields = new List<AdHocFilterField>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD.Entity.Adhoc> adHocs = new List<FrameworkUAD.Entity.Adhoc>();

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
                    if (_SelectedCondition != null && _SelectedCondition != "")
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
            public string GroupType { get; set; }
            public string IDTag { get; set; }

            public AdHocFilterField(string filterObj, List<string> conditions, string type)
            {
                this.FilterObject = filterObj;
                this.Conditions = conditions;
                this.Type = type;
                this.Value = "";
                this.ToValue = "";
                this.FromValue = "";
                this.IDTag = "";
                this.GroupType = "";
            }

            public AdHocFilterField()
            {
                
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public AdHocUADFilters()
        {
            InitializeComponent();

            codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                filterTypes = codeResponse.Result;
            }

            adHocResponse = adHocData.Proxy.SelectCategoryID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, 0);
            if(Helpers.Common.CheckResponse(adHocResponse.Result, adHocResponse.Status))
            {
                adHocs = adHocResponse.Result;
            }

            List<string> standardCon = new List<string>() { "CONTAINS", "EQUAL", "START WITH", "END WITH", "DOES NOT CONTAIN" };
            List<string> rangeCon = new List<string>() { "RANGE", "EQUAL", "GREATER THAN", "LESSER THAN" };
            List<string> dateCon = new List<string>() { "RANGE", "Year", "Month" };

            foreach (FrameworkUAD.Entity.Adhoc adhoc in adHocs)
            {
                AdHocFilterField result = new AdHocFilterField();
                if (adhoc.ColumnType == "int")
                    result = new AdHocFilterField(adhoc.ColumnName, rangeCon, "Range"); //filterFields.Add(new AdHocFilterField(adhoc.ColumnName, rangeCon, "Range"));
                else if (adhoc.ColumnType == "varchar")
                    result = new AdHocFilterField(adhoc.ColumnName, standardCon, "Standard"); //filterFields.Add(new AdHocFilterField(adhoc.ColumnName, standardCon, "Standard"));
                else if (adhoc.ColumnType == "datetime" || adhoc.ColumnType == "smalldatetime")
                    result = new AdHocFilterField(adhoc.ColumnName, dateCon, "DateRange"); //filterFields.Add(new AdHocFilterField(adhoc.ColumnName, dateCon, "DateRange"));

                string[] vals = adhoc.ColumnValue.Split('|');
                if (vals[0] == "m")
                {
                    result.GroupType = "MasterGroup";
                    //result.FromValue = vals[1];
                    result.IDTag = vals[1];
                }
                else if (vals[0] == "e")
                    result.GroupType = "SubscriptionsExtension";
                else if (vals[0] == "i")
                    result.GroupType = "Integer";
                else if (vals[0] == "f")
                    result.GroupType = "Float";

                filterFields.Add(result);
            }

            filterFields.Sort((x, y) => x.FilterObject.CompareTo(y.FilterObject));
            filterFields = filterFields.Distinct().ToList();
            icAdHocFields.ItemsSource = filterFields;
        }

        public List<Helpers.FilterOperations.FilterDetailContainer> GetSelection()
        {
            List<Helpers.FilterOperations.FilterDetailContainer> myDetailContainers = new List<Helpers.FilterOperations.FilterDetailContainer>();
            foreach (AdHocFilterField aff in icAdHocFields.Items)
            {
                Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                fdc.FilterDetail.SearchCondition = aff.SelectedCondition;
                fdc.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.AdHoc.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
                if (aff.Type == "Range" && aff.SelectedCondition != null)
                {
                    if ((aff.FromValue != null || aff.ToValue != null) && (aff.FromValue != "" && aff.ToValue != ""))
                    {
                        if (aff.FromValue == null)
                            aff.FromValue = "0";

                        fdc.FilterDetail.AdHocFieldValue = "";
                        fdc.FilterDetail.AdHocFromField = aff.FromValue;
                        fdc.FilterDetail.AdHocToField = aff.ToValue;
                        fdc.FilterDetail.FilterField = aff.FilterObject;
                        fdc.FilterDetail.FilterObjectType = aff.Type;
                        fdc.FilterDetail.GroupType = aff.GroupType;
                        fdc.FilterDetail.IDTag = aff.IDTag;
                        myDetailContainers.Add(fdc);
                    }

                }
                else if (aff.Type == "Standard" && aff.SelectedCondition != null)
                {
                    if (aff.Value != null && aff.Value != "")
                    {
                        fdc.FilterDetail.AdHocFieldValue = aff.Value;
                        fdc.FilterDetail.AdHocFromField = "";
                        fdc.FilterDetail.AdHocToField = "";
                        fdc.FilterDetail.FilterField = aff.FilterObject;
                        fdc.FilterDetail.FilterObjectType = aff.Type;
                        fdc.FilterDetail.GroupType = aff.GroupType;
                        fdc.FilterDetail.IDTag = aff.IDTag;
                        myDetailContainers.Add(fdc);
                    }
                }
                else if (aff.Type == "DateRange" && aff.SelectedCondition != null)
                {
                    if ((aff.FromValue != null || aff.ToValue != null) && (aff.FromValue != "" && aff.ToValue != ""))
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
                        fdc.FilterDetail.GroupType = aff.GroupType;
                        fdc.FilterDetail.IDTag = aff.IDTag;
                        myDetailContainers.Add(fdc);
                    }
                }
            }
            return myDetailContainers;
        }
    }
}
