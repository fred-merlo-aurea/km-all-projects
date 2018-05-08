using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAD.DataCompare.Web.Models
{

    public class CustomDropDown
    {
        
        public string Item { get; set; }
        public string SelectedItem { get; set; }


        public CustomDropDown(string _item, string _selectedItem = "Ignore")
        {
            this.Item = _item;
            this.SelectedItem = _selectedItem;
           
        }
    }

    public class ColumnMapper
    {

        #region Variables
        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        private int NumberOfUADControlsAdded;
        public KMPlatform.Entity.Client myClient;
        public List<FrameworkUAD.Object.FileMappingColumn> myMappings;
        public string MapperControlType;
        public string FieldMapType;
        public bool IsUserDefinedColumn;
        public string MapperRowType;
        public string TypeOfRow;
        public string mySourceColumn;
        public bool mySourceColumnVisibility;
        public string myPreviewData;
        public bool myPreviewDataVisibility;
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
        public List<string> adhocBitFields = new List<string>();
        public List<string> isRequiredFields = new List<string>();

        public List<CustomDropDown> _dropDownMapper;

        public List<SelectListItem> fileColumnMapping;
       

        public bool isAlreadyAvailable;
        public bool isRescan;

        #endregion

        public ColumnMapper()
        {

        }

        public ColumnMapper( string type, KMPlatform.Entity.Client client, List<FrameworkUAD.Object.FileMappingColumn> mappingColumns, string controlType, string sourceColumn,
           string previewData, List<FrameworkUAD_Lookup.Entity.Code> demoUpdateOptions, List<string> adhocBitColumns, List<string> isRequiredColumns,bool AlreadyAvailable = false,bool isRescan=false, FrameworkUAD_Lookup.Entity.Code demoUpdateOption = null, string uadColumn = "", string mappingType = "", string mapperRowType = "Normal", bool freeze = false)
        {
            this.isRescan = isRescan;
            isAlreadyAvailable = AlreadyAvailable;
             DemoUpdateOptions = demoUpdateOptions;
            adhocBitFields = adhocBitColumns;
            isRequiredFields = isRequiredColumns;
            currentType = type;
            //myAppData = appData;
            NumberOfUADControlsAdded = 1;
            myClient = client;
            myMappings = mappingColumns;
            MapperControlType = controlType;
            mySourceColumn = sourceColumn;
            mySourceColumnVisibility = true;
            myPreviewData = previewData;
            myPreviewDataVisibility = true;
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


            
            _dropDownMapper = new List<CustomDropDown>();
            _dropDownMapper.Add(new CustomDropDown("Ignore", "Ignore"));
            _dropDownMapper.Add(new CustomDropDown("kmTransform", "Ignore"));



            fileColumnMapping = new List<SelectListItem>();
            SelectListItem _ignore = new SelectListItem();
            _ignore.Text="Ignore";
            _ignore.Value = "Ignore";
           
            SelectListItem _kmTransform = new SelectListItem();
            _kmTransform.Text = "kmTransform";
            _kmTransform.Value = "kmTransform";
            fileColumnMapping.Add(_ignore);
            fileColumnMapping.Add(_kmTransform);


            foreach (FrameworkUAD.Object.FileMappingColumn md in myMappings)
            {
                _dropDownMapper.Add(new CustomDropDown(md.ColumnName, "Ignore"));
                fileColumnMapping.Add(new SelectListItem() { Text= md.ColumnName , Value = md.ColumnName });
            }

            FrameworkUAD.Object.FileMappingColumn foundColumn=null;

            if (MapperControlType == "New")
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
                    _dropDownMapper.Add(new CustomDropDown(foundColumn.ColumnName, foundColumn.ColumnName));

                    //Added To Store Matched column value -start
                    myUadColumn = foundColumn.ColumnName;
                    //End
                    if (foundColumn.IsDemographic == false)
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
                    _dropDownMapper.Add(new CustomDropDown("Ignore", "Ignore"));
                    //Added To Store Matched column value -start
                    myUadColumn = "Ignore";
                    //End
                    FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                }
            }

            else
            {
                if (MapperControlType == "EditUser")
                {
                    if (currentType != "MM")
                    {
                        mySourceColumn = "";
                        mySourceColumnVisibility = false;
                    }
                    myPreviewData = "";
                    myPreviewDataVisibility = false;
                }

               

                if (myUadColumn != "")
                {
                    foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(myUadColumn, StringComparison.CurrentCultureIgnoreCase));

                    if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                    {
                        //Added To Store Matched column value -start
                        myUadColumn = foundColumn.ColumnName;
                        //End
                        _dropDownMapper.Add(new CustomDropDown(foundColumn.ColumnName, foundColumn.ColumnName));
                        if (foundColumn.IsDemographic == false)
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
                        //Added To Store Matched column value -start
                        myUadColumn = "Ignore";
                        //End
                        _dropDownMapper.Add(new CustomDropDown("Ignore", "Ignore"));
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                    }
                }
                else
                {
                    foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(mySourceColumn, StringComparison.CurrentCultureIgnoreCase));

                    if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                    {
                        //Added To Store Matched column value -start
                        myUadColumn = foundColumn.ColumnName;
                        //End
                        _dropDownMapper.Add(new CustomDropDown(foundColumn.ColumnName, foundColumn.ColumnName));
                        if (foundColumn.IsDemographic == false)
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
                        //Added To Store Matched column value -start
                        myUadColumn = "Ignore";
                        //End
                        _dropDownMapper.Add(new CustomDropDown("Ignore", "Ignore"));
                        FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
                    }
                }
            }

         

            SetDemoUpdate(foundColumn, demoUpdateOption);

         }

        private void SetDemoUpdate(FrameworkUAD.Object.FileMappingColumn fmc, FrameworkUAD_Lookup.Entity.Code demoUpdateOption = null)
        {

            if (fmc != null && !String.IsNullOrEmpty(fmc.ColumnName))
            {
                if (fmc.IsDemographic == false)
                {
                    //ddDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
                    //LabelDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    //ddDemoUpdate.Visibility = System.Windows.Visibility.Visible;
                    //LabelDemoUpdate.Visibility = System.Windows.Visibility.Visible;
                    if (adhocBitFields.Count(x => x.Equals(fmc.ColumnName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        List<FrameworkUAD_Lookup.Entity.Code> removeAppend = new List<FrameworkUAD_Lookup.Entity.Code>();
                        //removeAppend.Add(DemoUpdateOptions.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Append.ToString(), StringComparison.CurrentCultureIgnoreCase)));
                        //ddDemoUpdate.ItemsSource = DemoUpdateOptions.Except(removeAppend);
                        //ddDemoUpdate.DisplayMemberPath = "CodeName";
                        //ddDemoUpdate.SelectedValuePath = "CodeId";
                    }
                    else if (isRequiredFields.Count(x => x.Equals(fmc.ColumnName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        List<FrameworkUAD_Lookup.Entity.Code> removeOverwrite = new List<FrameworkUAD_Lookup.Entity.Code>();
                        removeOverwrite.Add(DemoUpdateOptions.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite.ToString(), StringComparison.CurrentCultureIgnoreCase)));
                        //ddDemoUpdate.ItemsSource = DemoUpdateOptions.Except(removeOverwrite);
                        //ddDemoUpdate.DisplayMemberPath = "CodeName";
                        //ddDemoUpdate.SelectedValuePath = "CodeId";
                    }
                    else
                    {
                        //ddDemoUpdate.ItemsSource = DemoUpdateOptions;
                        //ddDemoUpdate.DisplayMemberPath = "CodeName";
                        //ddDemoUpdate.SelectedValuePath = "CodeId";
                    }

                    //Edit should pass for the demoUpdateOption otherwise new should be replace. We can also edit here for different file types to display different.
                    if (demoUpdateOption != null)
                    {
                        //ddDemoUpdate.SelectedItem = demoUpdateOption;
                    }
                    else
                    {
                        FrameworkUAD_Lookup.Entity.Code replace = DemoUpdateOptions.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString().Replace("_", " ")));
                        //ddDemoUpdate.SelectedItem = replace;
                    }
                }
            }
            else
            {
                //ddDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
                //LabelDemoUpdate.Visibility = System.Windows.Visibility.Collapsed;
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


       
    }

   
}