using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for Mapper2.xaml
    /// </summary>
    public partial class ColumnMapper : UserControl
    {
        #region Variables
        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        private int NumberOfUADControlsAdded;
        private Dictionary<StackPanel, StackPanel> abc = new Dictionary<StackPanel, StackPanel>();
        public KMPlatform.Entity.Client myClient;
        List<FileMappingColumn> myMappings;
        public string MapperControlType;
        public string FieldMapType;
        public bool IsUserDefinedColumn;
        public string MapperRowType;                
        public string mySourceColumn;
        public string myPreviewData;
        public string myUadColumn;
        public bool isFreeze;
        public int mySourceFileID;
        public int myFieldMappingID;
        public int myFieldMultiMapID;
        public List<FrameworkUAS.Entity.FieldMultiMap> multiMappings = new List<FrameworkUAS.Entity.FieldMultiMap>();
        public List<FrameworkUAS.Entity.TransformationFieldMap> allTranFieldMap = new List<FrameworkUAS.Entity.TransformationFieldMap>();
        public List<FrameworkUAS.Entity.TransformationFieldMultiMap> allTranFieldMultiMap = new List<FrameworkUAS.Entity.TransformationFieldMultiMap>();
        public List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();
        public string currentType;

        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMultiMap> tfmmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMultiMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMultiMap> fmmData = FrameworkServices.ServiceClient.UAS_FieldMultiMapClient();

        public List<string> adhocBitFields = new List<string>();
        public List<string> isRequiredFields = new List<string>();
        #endregion

        public ColumnMapper(string type, FrameworkUAS.Object.AppData appData, KMPlatform.Entity.Client client, List<FileMappingColumn> mappingColumns, string controlType, string sourceColumn, 
            string previewData, List<FrameworkUAD_Lookup.Entity.Code> demoUpdateOptions, List<string> adhocBitColumns, List<string> isRequiredColumns, FrameworkUAD_Lookup.Entity.Code demoUpdateOption = null, string uadColumn = "", string mappingType = "", string mapperRowType = "Normal", bool freeze = false)
        {
            InitializeComponent();
            DemoUpdateOptions = demoUpdateOptions;
            adhocBitFields = adhocBitColumns;
            isRequiredFields = isRequiredColumns;
            currentType = type;
            myAppData = appData;
            NumberOfUADControlsAdded = 1;
            myClient = client;
            myMappings = mappingColumns;
            MapperControlType = controlType;
            mySourceColumn = sourceColumn;
            myPreviewData = previewData;
            myUadColumn = uadColumn;
            if (mappingType == "")
                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
            else
                FieldMapType = mappingType;

            MapperRowType = mapperRowType;
            TypeOfRow = mapperRowType;
            isFreeze = freeze;            
            
            if (MapperControlType == "User" || MapperControlType == "EditUser")
                IsUserDefinedColumn = true;
            else
                IsUserDefinedColumn = false;
            
            
            DropDownMapper.Items.Add("Ignore");
            DropDownMapper.Items.Add("kmTransform");
            foreach (FrameworkUAD.Object.FileMappingColumn md in myMappings)
                DropDownMapper.Items.Add(md.ColumnName);

            FileMappingColumn foundColumn = null;

            if (MapperControlType == "User")
            {                
                TextBoxColumnName.Text = "";
                TextBoxColumnName.Visibility = Visibility.Hidden;
                TextBoxPreviewData.Text = "";
                TextBoxPreviewData.Visibility = Visibility.Hidden;

                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                DropDownMapper.SelectedItem = "Ignore";                
            }
            else if (MapperControlType == "New")
            {
                foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(mySourceColumn, StringComparison.CurrentCultureIgnoreCase));
                if (foundColumn == null)
                {
                    #region Extra Matching
                    switch (mySourceColumn.ToLower())
                    {
                        case "account number":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acctnum":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acct num":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acctnbr":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acct nbr":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "firstname":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;                            
                        case "first name":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "1st name":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "first":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fname":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "lastname":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "last name":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "last":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "lname":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zipcode":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zip code":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zip":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postal":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postal code":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postalcode":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break; 
                        case "emailaddress":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                            break; 
                        case "email address":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address1":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address 1":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mailing address":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "street":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "streetaddress":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "street address":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mailstop":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mail stop":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address2":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address 2":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phone number":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phonenumber":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "voice":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "cell":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Mobile", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fax number":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Fax", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "state":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("RegionCode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreign country name":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreigncountryname":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreigncountry":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreign country":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verified":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Verify", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "categoryid":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "category id":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "cat":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "category code":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "categorycode":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transactionid":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transaction id":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "xact":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transaction code":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transactioncode":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "sequence":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("SequenceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qsource":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "q source":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qsourceid":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "par3c":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Par3cID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qdate":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "q date":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verification date":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verificationdate":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo 7":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "media":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subscriptiontype":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subscription type":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "original subsrc":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("OrigsSrc", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "originalsubsrc":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("OrigsSrc", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subsrc":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("SUBSCRIBERSOURCECODE", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo31":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mail permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo32":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fax permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo33":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phone permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo34":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "other products permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo35":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "third party permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "3rd party permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo36":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "email renew permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "text permission":
                            foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        default:
                            foundColumn = null;
                            break;
                    }
                    #endregion
                }

                if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                {
                    DropDownMapper.SelectedItem = foundColumn.ColumnName;
                    if (foundColumn.IsDemographicDate == true)
                    {
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString();
                    }
                    else if (foundColumn.IsDemographic == false)
                    {
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                    }
                    else
                    {
                        if (foundColumn.IsDemographicOther == false)
                            FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString();
                        else
                            FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString();
                    }                    
                }
                else
                {
                    DropDownMapper.SelectedItem = "Ignore";                    
                    FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                }
            }
            else
            {
                if (MapperControlType == "EditUser")
                {
                    if (currentType != "MM")
                    {
                        TextBoxColumnName.Text = "";
                        TextBoxColumnName.Visibility = Visibility.Hidden;
                    }
                    TextBoxPreviewData.Text = "";
                    TextBoxPreviewData.Visibility = Visibility.Hidden;
                }

                if (freeze)
                {
                    DropDownMapper.IsEnabled = false;
                }

                if (myUadColumn != "")
                {
                    foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(myUadColumn, StringComparison.CurrentCultureIgnoreCase));

                    if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                    {
                        DropDownMapper.SelectedItem = foundColumn.ColumnName;
                        if (foundColumn.IsDemographicDate == true)
                        {
                            FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString();
                        }
                        else if (foundColumn.IsDemographic == false)
                        {
                            FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                        }
                        else
                        {
                            if (foundColumn.IsDemographicOther == false)
                                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString();
                            else
                                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString();
                        }                        
                    }
                    else if (myUadColumn.Equals("kmTransform", StringComparison.CurrentCultureIgnoreCase))
                    {
                        DropDownMapper.SelectedItem = "kmTransform";
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString();
                    }
                    else
                    {
                        DropDownMapper.SelectedItem = "Ignore";                        
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                    }
                }
                else
                {
                    foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(mySourceColumn, StringComparison.CurrentCultureIgnoreCase));

                    if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                    {
                        DropDownMapper.SelectedItem = foundColumn.ColumnName;
                        if (foundColumn.IsDemographicDate == true)
                        {
                            FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString();
                        }
                        else if (foundColumn.IsDemographic == false)
                        {
                            FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                        }
                        else
                        {
                            if (foundColumn.IsDemographicOther == false)
                                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString();
                            else
                                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString();
                        }                        
                    }
                    else
                    {
                        DropDownMapper.SelectedItem = "Ignore";                        
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                    }
                }
            }

            SetDemoUpdate(foundColumn, demoUpdateOption);

            TextBoxColumnName.Text = mySourceColumn;
            TextBoxPreviewData.Text = myPreviewData;
            if (myPreviewData == "")
                DataExpanderGrid.Visibility = Visibility.Hidden;
        }        
        private void SetDemoUpdate(FileMappingColumn fmc, FrameworkUAD_Lookup.Entity.Code demoUpdateOption = null)
        {

            if (fmc != null && !String.IsNullOrEmpty(fmc.ColumnName))
            {
                if (fmc.IsDemographic == false)
                {
                    ddDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
                    LabelDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;                    
                }
                else
                {
                    ddDemoUpdate.Visibility = System.Windows.Visibility.Visible;
                    LabelDemoUpdate.Visibility = System.Windows.Visibility.Visible;
                    if (adhocBitFields.Count(x => x.Equals(fmc.ColumnName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        List<FrameworkUAD_Lookup.Entity.Code> removeAppend = new List<FrameworkUAD_Lookup.Entity.Code>();
                        removeAppend.Add(DemoUpdateOptions.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Append.ToString(), StringComparison.CurrentCultureIgnoreCase)));
                        ddDemoUpdate.ItemsSource = DemoUpdateOptions.Except(removeAppend);
                        ddDemoUpdate.DisplayMemberPath = "CodeName";
                        ddDemoUpdate.SelectedValuePath = "CodeId";
                    }
                    else if (isRequiredFields.Count(x => x.Equals(fmc.ColumnName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        List<FrameworkUAD_Lookup.Entity.Code> removeOverwrite = new List<FrameworkUAD_Lookup.Entity.Code>();
                        removeOverwrite.Add(DemoUpdateOptions.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite.ToString(), StringComparison.CurrentCultureIgnoreCase)));
                        ddDemoUpdate.ItemsSource = DemoUpdateOptions.Except(removeOverwrite);
                        ddDemoUpdate.DisplayMemberPath = "CodeName";
                        ddDemoUpdate.SelectedValuePath = "CodeId";
                    }
                    else
                    {
                        ddDemoUpdate.ItemsSource = DemoUpdateOptions;
                        ddDemoUpdate.DisplayMemberPath = "CodeName";
                        ddDemoUpdate.SelectedValuePath = "CodeId";
                    }

                    //Edit should pass for the demoUpdateOption otherwise new should be replace. We can also edit here for different file types to display different.
                    if (demoUpdateOption != null)
                        ddDemoUpdate.SelectedItem = demoUpdateOption;
                    else
                    {
                        FrameworkUAD_Lookup.Entity.Code replace = DemoUpdateOptions.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString().Replace("_", " ")));
                        ddDemoUpdate.SelectedItem = replace;
                    }
                }        
            }
            else
            {
                ddDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
                LabelDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        public bool EnableMapper
        {
            set
            {
                if (value == true)
                {
                    spControl.Visibility = Visibility.Visible;
                }
                else
                {
                    spControl.Visibility = Visibility.Collapsed;
                }
            }
        }
        public bool HasTransformation
        {
            set
            {
                if (value == true)
                {
                    //Blue #FF4B87BC
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#FF4B87BC");                    
                    TransformationIcon.Background = brush;                    
                }
                else
                {
                    //Orange #FFF47E1F
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#FFF47E1F");
                    TransformationIcon.Background = brush;
                }
            }
        }
        public List<FrameworkUAS.Entity.FieldMultiMap> AddMultiMapColumn
        {
            set
            {
                multiMappings = value;
            }
        }
        public int AddSourceFileID
        {
            set
            {
                mySourceFileID = value;                
            }
        }
        public int AddFieldMappingID
        {
            set
            {
                myFieldMappingID = value;                
            }
        }                
        public List<FrameworkUAS.Entity.TransformationFieldMultiMap> AddTranFieldMultiMap
        {
            set
            { allTranFieldMultiMap = value; }
        }
        public void OpenPreviewData()
        {
            DataExpanderGrid.Visibility = Visibility.Visible;
            DataExpander.Visibility = Visibility.Visible;
        }
        public void ClosePreviewData()
        {
            DataExpanderGrid.Visibility = Visibility.Collapsed;
            DataExpander.Visibility = Visibility.Collapsed;
        }
        public void ShowLabelRow()
        {
            LabelRow.Visibility = Visibility.Visible;
        }
        public void CloseLabelRow()
        {
            LabelRow.Visibility = Visibility.Collapsed;
        }
        public void CollapseSource()
        {
            TextBoxColumnName.Visibility = Visibility.Collapsed;
        }
        public void Shrink(int shrinkSize)
        {
            TextBoxColumnName.Width = TextBoxColumnName.Width - shrinkSize;
            DropDownMapper.Width = DropDownMapper.Width - shrinkSize;
        }
        public string MapperControlTypeData
        {
            get { return MapperControlType; }
            set { MapperControlType = value; }
        }
        public int DemographicUpdateCodeId
        {
            get
            {
                if (ddDemoUpdate.SelectedValue != null)
                {
                    int codeId = 0;
                    int.TryParse(ddDemoUpdate.SelectedValue.ToString(), out codeId);
                    return codeId;
                }
                else
                {
                    FrameworkUAD_Lookup.Entity.Code replace = DemoUpdateOptions.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString().Replace("_", " ")));
                    return replace.CodeId;
                }
            }
        }
        public string ComboBoxMappingText
        {
            get
            {
                if (DropDownMapper.SelectedItem != null)
                    return DropDownMapper.SelectedItem.ToString();
                else
                    return "";
            }
            set { DropDownMapper.Text = value; }
        }        
        public string TextboxColumnText
        {
            get { return TextBoxColumnName.Text; }
            set { TextBoxColumnName.Text = value; }
        }
        public string TextboxPreviewData
        {
            get { return TextBoxPreviewData.Text; }
            set { TextBoxPreviewData.Text = value; }
        }
        public string TypeOfRow
        {
            get
            {
                return MapperRowType;

            }
            set
            {
                MapperRowType = value;
                if (MapperRowType.Equals(ColumnMapperRowType.Remove.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    spControl.Background = Brushes.Red;
                else if (MapperRowType.Equals(ColumnMapperRowType.New.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    spControl.Background = Brushes.Green;
                else if (MapperRowType.Equals(ColumnMapperRowType.Normal.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    spControl.Background = Brushes.Transparent;

            }
        }
        public string ButtonTag
        {
            get
            {
                if (ButtonDelete.Tag == null)
                    return "";
                else
                    return ButtonDelete.Tag.ToString();

            }
            set { ButtonDelete.Tag = value; }
        }

        #region ButtonClicks                        
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentType != "MM")
            {
                MessageBoxResult res = MessageBox.Show("Do you want to delete this field data from your mapping?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.No) return;

                int fieldMap = 0;
                Telerik.Windows.Controls.RadButton thisButton = (Telerik.Windows.Controls.RadButton)sender;
                if (thisButton.Tag != null)
                {
                    Int32.TryParse(thisButton.Tag.ToString(), out fieldMap);
                    if (fieldMap > 0)
                    {
                        int t = tfmData.Proxy.DeleteFieldMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fieldMap).Result;
                        int i = fmData.Proxy.DeleteMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fieldMap).Result;
                        Core_AMS.Utilities.WPF.MessageDeleteComplete();
                    }
                }

                StackPanel sp = (StackPanel)this.Parent;
                sp.Children.Remove(this);
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("Are you sure this should be deleted?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.No) return;

                Telerik.Windows.Controls.RadButton thisButton = (Telerik.Windows.Controls.RadButton)sender;                
                int btnTag = 0;
                if (thisButton.Tag != null)
                    int.TryParse(thisButton.Tag.ToString(), out btnTag);

                if (btnTag > 0)
                {
                    int t = tfmmData.Proxy.DeleteByFieldMultiMapID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, btnTag).Result;
                    int i = fmmData.Proxy.DeleteByFieldMultiMapID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, btnTag).Result;
                    if (this.myFieldMappingID > 0)
                    {
                        List<FrameworkUAS.Entity.FieldMultiMap> thisFieldMultiMap = fmmData.Proxy.SelectFieldMappingID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, this.myFieldMappingID).Result;
                        if (thisFieldMultiMap == null || !(thisFieldMultiMap.Count > 0))
                        {
                            FrameworkUAS.Entity.FieldMapping thisFieldMap = fmData.Proxy.SelectForFieldMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, this.myFieldMappingID).Result;
                            if (thisFieldMap != null)
                            {
                                thisFieldMap.HasMultiMapping = false;
                                thisFieldMap.DateUpdated = DateTime.Now;
                                thisFieldMap.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                int f = fmData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisFieldMap).Result;
                            }
                        }
                    }
                    Core_AMS.Utilities.WPF.MessageDeleteComplete();
                }

                StackPanel sp = (StackPanel)this.Parent;
                sp.Children.Remove(this);
            }
        }
        #endregion               

        private void DropDownMapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileMappingColumn foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(DropDownMapper.SelectedValue.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
            {
                DropDownMapper.SelectedItem = foundColumn.ColumnName;
                if (foundColumn.IsDemographicDate == true)
                {
                    FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString();
                }
                else if (foundColumn.IsDemographic == false)
                {
                    FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                }
                else
                {
                    if (foundColumn.IsDemographicOther == false)
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString();
                    else
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString();
                }                
            }
            else if (DropDownMapper.SelectedValue.ToString().Equals("kmTransform"))
            {
                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString();
            }
            else
            {
                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();                
            }

            SetDemoUpdate(foundColumn);
        }

        private void ddDemoUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ddDemoUpdate.SelectedItem.GetType() == typeof(FrameworkUAD_Lookup.Entity.Code))
            {
                FrameworkUAD_Lookup.Entity.Code code = new FrameworkUAD_Lookup.Entity.Code();
                code = (FrameworkUAD_Lookup.Entity.Code) ddDemoUpdate.SelectedItem;

                ddDemoUpdate.ToolTip = code.CodeDescription;
            }
        }
    }

    public enum ColumnMapperControlType
    {
        User,
        EditUser,
        Edit,
        New       
    }

    public enum ColumnMapperRowType
    {
        New,
        Remove,
        Normal
    }
}
