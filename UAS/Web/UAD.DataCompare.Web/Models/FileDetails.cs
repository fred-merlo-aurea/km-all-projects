using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FrameworkUAD.Object;
using KMPlatform.Entity;
using System.Web.Mvc;

namespace UAD.DataCompare.Web.Models
{
    public class FileDetails
    {
        
        [Display(Name = "Select a File to Map:")]
        public HttpPostedFileBase DataFile { get; set; }

        [Required(ErrorMessage = "File name cannot be blank.")]
        [Display(Name = "Save Filename As:")]
        [MaxLength(50, ErrorMessage = "File name cannot exceed 50 characters.")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Notification Email cannot be blank.")]
        [Display(Name = "Notification Email Address:")]
        public string NotificationEmail { get; set; }

        [Required]
        [Display(Name = "File Delimiter:")]
        public string Delimiter { get; set; }

        [Required]
        [Display(Name = "Does this contain double quotation mark:")]
        public string HasQuotation { get; set; }

        [Display(Name = "Is Import Billable?")]
        public string IsImportBillable { get; set; }

        [Required(ErrorMessage = "Notes cannot be blank.")]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Date Created:")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated:")]
        public DateTime? DateUpdated { get; set; }

        public int SourceFileID { get; set; }
        
        public string FilePath { get; set; } //Hidden
        public string Extention { get; set; } //Hidden
        public bool isKMStaff { get; set; } //Hidden
        public List<YesNoDropDown> TrueFalse { get; set; }
        public List<DelimitersDropDown> Delimiters { get; set; }

        public List<ColumnMap> ColumnMapping { get; set; }

        public FileDetails()
        {
            FileName = "";
            NotificationEmail = "";
            Delimiter = "";
            HasQuotation = "No";
            IsImportBillable = "No";
            isKMStaff = false;
            SourceFileID = 0;
            Notes = "";
            DateCreated = DateTime.Now;
            TrueFalse = new List<YesNoDropDown>();
            TrueFalse.Add(new YesNoDropDown() { Text = "Yes" });
            TrueFalse.Add(new YesNoDropDown() { Text = "No" });
            Delimiters = new List<DelimitersDropDown>();
            Delimiters.Add(new DelimitersDropDown() { Text = "comma" });
            Delimiters.Add(new DelimitersDropDown() { Text = "tab" });
            Delimiters.Add(new DelimitersDropDown() { Text = "semicolon" });
            Delimiters.Add(new DelimitersDropDown() { Text = "colon" });
            Delimiters.Add(new DelimitersDropDown() { Text = "tild" });
            ColumnMapping = new List<ColumnMap>();

        }
    }
    public class DelimitersDropDown
    {
        public string Text { get; set; }
    }
    public class YesNoDropDown
    {
        public string Text { get; set; }
    }
    public class ColumnMap
    {
        private Client _client;
        private List<FileMappingColumn> myMappings;
        private string MapperControlType;
        public int FieldMapID { get; set; }
        public int FieldMapTypeID { get; set; }
        public int SourceFileID { get; set; }
        public string PreviewDataColumn { get; set; }
        public string SourceColumn { get; set; }
        public string MappedColumn { get; set; }
        public List<SelectListItem> ProfileColumnList { get; set; }
        public ColumnMap()
        {

        }
        public ColumnMap(Client _client, List<FileMappingColumn> uadColumns, string controlType, string sourcecolumn, string dataPreview, string mappedcolumn ="")
        {
            this._client = _client;
            this.myMappings = uadColumns.Where(x => !x.IsDemographic && !x.IsDemographicOther).ToList();
            this.SourceColumn =Core_AMS.Utilities.StringFunctions.CleanDangerousFormString(sourcecolumn);
            this.PreviewDataColumn = Core_AMS.Utilities.StringFunctions.CleanDangerousFormString(dataPreview);
            this.MapperControlType = controlType;
            FieldMapID = 0;
            FieldMapTypeID = 0;
            ProfileColumnList = new List<SelectListItem>();
            ProfileColumnList.Add(new SelectListItem() { Text = "Ignore", Value = "Ignore" });
            ProfileColumnList.Add(new SelectListItem() { Text = "Delete", Value = "Delete" });
            foreach (FrameworkUAD.Object.FileMappingColumn md in myMappings)
            {
                ProfileColumnList.Add(new SelectListItem() { Text = md.ColumnName, Value = md.ColumnName });
            }

            FrameworkUAD.Object.FileMappingColumn foundColumn = new FileMappingColumn();

            if (MapperControlType == "New")
            {
               
                foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(sourcecolumn, StringComparison.CurrentCultureIgnoreCase));
                if (foundColumn == null)
                {
                    #region Extra Matching
                    switch (sourcecolumn.ToLower())
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
                    ProfileColumnList.Add(new SelectListItem() { Text = foundColumn.ColumnName, Value = foundColumn.ColumnName, Selected = true });
                    MappedColumn = Core_AMS.Utilities.StringFunctions.CleanDangerousFormString(foundColumn.ColumnName);
                }
                else
                {

                    ProfileColumnList.Add(new SelectListItem() { Text = "Ignore", Value = "Ignore", Selected = true });
                    MappedColumn = "Ignore";
                }
            }
           

        }
    }
}