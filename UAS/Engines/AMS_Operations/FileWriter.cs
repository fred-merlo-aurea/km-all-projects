using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using FrameworkSubGen.Entity;
using FrameworkUAD.Entity;
using KM.Common;
using KM.Common.Functions;
using BusinessImportSubscriber = FrameworkSubGen.BusinessLogic.ImportSubscriber;

namespace AMS_Operations
{
    public class FileWriter
    {
        private const string PubCodeFieldName = "PubCode";
        private const string FNameFieldName = "FName";
        private const string LNameFieldName = "LName";
        private const string TitleFieldName = "Title";
        private const string CompanyFieldName = "Company";
        private const string AddressFieldName = "Address";
        private const string MailStopFieldName = "MailStop";
        private const string CityFieldName = "City";
        private const string StateFieldName = "State";
        private const string ZipFieldName = "Zip";
        private const string Plus4FieldName = "Plus4";
        private const string ForZipFieldName = "ForZip";
        private const string CountyFieldName = "County";
        private const string CountryFieldName = "Country";
        private const string PhoneFieldName = "Phone";
        private const string FaxFieldName = "Fax";
        private const string EmailFieldName = "Email";
        private const string CategoryIdFieldName = "CategoryID";
        private const string TransactionIdFieldName = "TransactionID";
        private const string QDateFieldName = "QDate";
        private const string QSourceIdFieldName = "QSourceID";
        private const string RegCodeFieldName = "RegCode";
        private const string VerifiedFieldName = "Verified";
        private const string SubSrcFieldName = "SubSrc";
        private const string OrigsSrcFieldName = "OrigsSrc";
        private const string Par3CFieldName = "Par3C";
        private const string SourceFieldName = "Source";
        private const string PriorityFieldName = "Priority";
        private const string SicFieldName = "Sic";
        private const string SicCodeFieldName = "SicCode";
        private const string GenderFieldName = "Gender";
        private const string Address3FieldName = "Address3";
        private const string HomeWorkAddressFieldName = "Home_Work_Address";
        private const string Demo7FieldName = "Demo7";
        private const string MobileFieldName = "Mobile";
        private const string LatitudeFieldName = "Latitude";
        private const string LongitudeFieldName = "Longitude";
        private const string AccountNumberFieldName = "AccountNumber";
        private const string CopiesFieldName = "Copies";
        private const string GraceIssuesFieldName = "GraceIssues";
        private const string IsPaidFieldName = "IsPaid";
        private const string OccupationFieldName = "Occupation";
        private const string SubscriptionStatusIdFieldName = "SubscriptionStatusID";
        private const string SubsrcIdFieldName = "SubsrcID";
        private const string WebsiteFieldName = "Website";
        private const string Demo31FieldName = "Demo31";
        private const string Demo32FieldName = "Demo32";
        private const string Demo33FieldName = "Demo33";
        private const string Demo34FieldName = "Demo34";
        private const string Demo35FieldName = "Demo35";
        private const string Demo36FieldName = "Demo36";
        private const string TextPermissionFieldName = "TextPermission";
        private const string SubGenSubscriberIdFieldName = "SubGenSubscriberID";
        private const string SubGenSubscriptionIdFieldName = "SubGenSubscriptionID";
        private const string SubGenPublicationIdFieldName = "SubGenPublicationID";
        private const string SubGenMailingAddressIdFieldName = "SubGenMailingAddressId";
        private const string SubGenBillingAddressIdFieldName = "SubGenBillingAddressId";
        private const string IssuesLeftFieldName = "IssuesLeft";
        private const string UnearnedReveueFieldName = "UnearnedReveue";
        private const string DemoValue = "xxNVxx";
        private const string SubGenFolderKeyName = "SubGenSubscriberFileImportFolder";
        private readonly Action<string> _consoleMessage;
        private readonly LogImportErrorFuncDelegate _logImportError;

        public FileWriter(Action<string> consoleMessageFunc, LogImportErrorFuncDelegate logImportErrorFunc)
        {
            _consoleMessage = consoleMessageFunc;
            _logImportError = logImportErrorFunc;
        }

        public DataTable CreateSaveFileDataTable(IEnumerable<SubscriberTransformed> stList)
        {
            var dtValid = new DataTable();

            dtValid.Columns.Add("PubCode");
            dtValid.Columns.Add("FName");
            dtValid.Columns.Add("LName");
            dtValid.Columns.Add("Title");
            dtValid.Columns.Add("Company");
            dtValid.Columns.Add("Address");
            dtValid.Columns.Add("MailStop");
            dtValid.Columns.Add("City");
            dtValid.Columns.Add("State");
            dtValid.Columns.Add("Zip");
            dtValid.Columns.Add("Plus4");
            dtValid.Columns.Add("ForZip");
            dtValid.Columns.Add("County");
            dtValid.Columns.Add("Country");
            dtValid.Columns.Add("Phone");
            dtValid.Columns.Add("Fax");
            dtValid.Columns.Add("Email");
            dtValid.Columns.Add("CategoryID");
            dtValid.Columns.Add("TransactionID");
            dtValid.Columns.Add("QDate");
            dtValid.Columns.Add("QSourceID");
            dtValid.Columns.Add("RegCode");
            dtValid.Columns.Add("Verified");
            dtValid.Columns.Add("SubSrc");
            dtValid.Columns.Add("OrigsSrc");
            dtValid.Columns.Add("Par3C");
            dtValid.Columns.Add("Source");
            dtValid.Columns.Add("Priority");
            dtValid.Columns.Add("Sic");
            dtValid.Columns.Add("SicCode");
            dtValid.Columns.Add("Gender");
            dtValid.Columns.Add("Address3");
            dtValid.Columns.Add("Home_Work_Address");
            dtValid.Columns.Add("Demo7");
            dtValid.Columns.Add("Mobile");
            dtValid.Columns.Add("Latitude");
            dtValid.Columns.Add("Longitude");
            dtValid.Columns.Add("AccountNumber");
            dtValid.Columns.Add("Copies");
            dtValid.Columns.Add("GraceIssues");
            dtValid.Columns.Add("IsPaid");
            dtValid.Columns.Add("Occupation");
            dtValid.Columns.Add("SubscriptionStatusID");
            dtValid.Columns.Add("SubsrcID");
            dtValid.Columns.Add("Website");
            dtValid.Columns.Add("Demo31"); //MailPermission
            dtValid.Columns.Add("Demo32"); //FaxPermission
            dtValid.Columns.Add("Demo33"); //PhonePermission
            dtValid.Columns.Add("Demo34"); //OtherProductsPermission
            dtValid.Columns.Add("Demo35"); //ThirdPartyPermission
            dtValid.Columns.Add("Demo36"); //EmailRenewPermission
            dtValid.Columns.Add("TextPermission");
            dtValid.Columns.Add("SubGenSubscriberID");
            dtValid.Columns.Add("SubGenSubscriptionID");
            dtValid.Columns.Add("SubGenPublicationID");
            dtValid.Columns.Add("SubGenMailingAddressId");
            dtValid.Columns.Add("SubGenBillingAddressId");
            dtValid.Columns.Add("IssuesLeft");
            dtValid.Columns.Add("UnearnedReveue");

            var demoColumns = stList.SelectMany(l => l.DemographicTransformedList).Select(sdt => sdt.MAFField).Distinct();
            foreach (var col in demoColumns)
            {
                dtValid.Columns.Add(col);
            }

            return dtValid;
        }

        public int CreateSaveFileDataTableRow(
            int rowCounter,
            IList<SubscriberTransformed> stList,
            SubscriberTransformed st,
            IList<ResponseGroup> rgList,
            DataTable dtValid,
            DataTable dtBad)
        {
            _consoleMessage($"Write to file row {rowCounter} of {stList.Count}");
            try
            {
                var pubId = st.DemographicTransformedList.First().PubID;
                var rgCheck = rgList.Where(x => x.PubCode.Equals(st.PubCode, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
                var requiredDemos =
                    rgCheck.Where(x => x.IsRequired == true).Select(y => y.ResponseGroupName.ToUpper()).ToList();
                foreach (var maf in requiredDemos)
                {
                    if (!st.DemographicTransformedList.Any(x =>
                        x.MAFField.Equals(maf, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        //add the demo with value of xxNVxx
                        var sdt = new SubscriberDemographicTransformed
                        {
                            CreatedByUserID = 1,
                            DateCreated = DateTime.Now,
                            IsAdhoc = false,
                            MAFField = maf,
                            NotExists = false,
                            ResponseOther = string.Empty,
                            PubID = pubId,
                            SORecordIdentifier = st.SORecordIdentifier,
                            STRecordIdentifier = st.STRecordIdentifier,
                            Value = DemoValue,
                        };
                        st.DemographicTransformedList.Add(sdt);
                    }
                }

                if (st.QDate.HasValue &&
                    st.CategoryID > 0 &&
                    st.QSourceID > 0 &&
                    st.TransactionID > 0 &&
                    st.SubGenSubscriberID > 0 &&
                    !string.IsNullOrWhiteSpace(st.PubCode))
                {
                    AddSubscriberRow(dtValid, st);
                }
                else
                {
                    AddSubscriberRow(dtBad, st);
                }
            }
            catch (Exception ex)
            {
                var msg = StringFunctions.FormatException(ex);
                _logImportError("CreateFile", msg, null, null, null, st);
            }

            rowCounter++;
            return rowCounter;
        }

        public void WriteSaveFileDataTable(
            string fileName,
            List<ImportSubscriber> isList,
            DataTable dtValid,
            DataTable dtBad,
            BusinessImportSubscriber isWorker)
        {
            var ff = new Core_AMS.Utilities.FileFunctions();
            _consoleMessage("Create Valid file");
            var dir = $"{ConfigurationManager.AppSettings[SubGenFolderKeyName]}FinalFiles\\";
            ff.CreateCSVFromDataTable(dtValid, $"{dir}{fileName}_Valid.csv");
            if (dtBad.Rows.Count > 1)
            {
                _consoleMessage("Create Bad file");
                ff.CreateCSVFromDataTable(dtBad, $"{dir}{fileName}_Bad.csv");
            }

            _consoleMessage("Start UpdateMergedToUAD");
            isWorker.UpdateMergedToUAD(isList);
            _consoleMessage("Done UpdateMergedToUAD");
        }

        private void AddSubscriberRow(DataTable table, SubscriberTransformed subscriber)
        {
            if (Should.AnyBeNull(table, subscriber))
            {
                return;
            }

            var dataRow = table.NewRow();

            SetDataRowField(dataRow, PubCodeFieldName, subscriber.PubCode);
            SetDataRowField(dataRow, FNameFieldName, subscriber.FName);
            SetDataRowField(dataRow, LNameFieldName, subscriber.LName);
            SetDataRowField(dataRow, TitleFieldName, subscriber.Title);
            SetDataRowField(dataRow, CompanyFieldName, subscriber.Company);
            SetDataRowField(dataRow, AddressFieldName, subscriber.Address);
            SetDataRowField(dataRow, MailStopFieldName, subscriber.MailStop);
            SetDataRowField(dataRow, CityFieldName, subscriber.City);
            SetDataRowField(dataRow, StateFieldName, subscriber.State);
            SetDataRowField(dataRow, ZipFieldName, subscriber.Zip);
            SetDataRowField(dataRow, Plus4FieldName, subscriber.Plus4);
            SetDataRowField(dataRow, ForZipFieldName, subscriber.ForZip);
            SetDataRowField(dataRow, CountyFieldName, subscriber.County);
            SetDataRowField(dataRow, CountryFieldName, subscriber.Country);
            SetDataRowField(dataRow, PhoneFieldName, subscriber.Phone);
            SetDataRowField(dataRow, FaxFieldName, subscriber.Fax);
            SetDataRowField(dataRow, EmailFieldName, subscriber.Email);
            SetDataRowField(dataRow, CategoryIdFieldName, subscriber.CategoryID);
            SetDataRowField(dataRow, TransactionIdFieldName, subscriber.TransactionID);
            SetDataRowField(dataRow, QDateFieldName, subscriber.QDate?.ToShortDateString());
            SetDataRowField(dataRow, QSourceIdFieldName, subscriber.QSourceID);
            SetDataRowField(dataRow, RegCodeFieldName, subscriber.RegCode);
            SetDataRowField(dataRow, VerifiedFieldName, subscriber.Verified);
            SetDataRowField(dataRow, SubSrcFieldName, subscriber.SubSrc);
            SetDataRowField(dataRow, OrigsSrcFieldName, subscriber.OrigsSrc);
            SetDataRowField(dataRow, Par3CFieldName, subscriber.Par3C);
            SetDataRowField(dataRow, SourceFieldName, subscriber.Source);
            SetDataRowField(dataRow, PriorityFieldName, subscriber.Priority);
            SetDataRowField(dataRow, SicFieldName, subscriber.Sic);
            SetDataRowField(dataRow, SicCodeFieldName, subscriber.SicCode);
            SetDataRowField(dataRow, GenderFieldName, subscriber.Gender);
            SetDataRowField(dataRow, Address3FieldName, subscriber.Address3);
            SetDataRowField(dataRow, HomeWorkAddressFieldName, subscriber.Home_Work_Address);
            SetDataRowField(dataRow, Demo7FieldName, subscriber.Demo7);
            SetDataRowField(dataRow, MobileFieldName, subscriber.Mobile);
            SetDataRowField(dataRow, LatitudeFieldName, subscriber.Latitude);
            SetDataRowField(dataRow, LongitudeFieldName, subscriber.Longitude);
            SetDataRowField(dataRow, AccountNumberFieldName, subscriber.AccountNumber);
            SetDataRowField(dataRow, CopiesFieldName, subscriber.Copies);
            SetDataRowField(dataRow, GraceIssuesFieldName, subscriber.GraceIssues);
            SetDataRowField(dataRow, IsPaidFieldName, subscriber.IsPaid);
            SetDataRowField(dataRow, OccupationFieldName, subscriber.Occupation);
            SetDataRowField(dataRow, SubscriptionStatusIdFieldName, subscriber.SubscriptionStatusID);
            SetDataRowField(dataRow, SubsrcIdFieldName, subscriber.SubsrcID);
            SetDataRowField(dataRow, WebsiteFieldName, subscriber.Website);
            SetDataRowField(dataRow, Demo31FieldName, subscriber.MailPermission);
            SetDataRowField(dataRow, Demo32FieldName, subscriber.FaxPermission);
            SetDataRowField(dataRow, Demo33FieldName, subscriber.PhonePermission);
            SetDataRowField(dataRow, Demo34FieldName, subscriber.OtherProductsPermission);
            SetDataRowField(dataRow, Demo35FieldName, subscriber.ThirdPartyPermission);
            SetDataRowField(dataRow, Demo36FieldName, subscriber.EmailRenewPermission);
            SetDataRowField(dataRow, TextPermissionFieldName, subscriber.TextPermission);
            SetDataRowField(dataRow, SubGenSubscriberIdFieldName, subscriber.SubGenSubscriberID);
            SetDataRowField(dataRow, SubGenSubscriptionIdFieldName, subscriber.SubGenSubscriptionID);
            SetDataRowField(dataRow, SubGenPublicationIdFieldName, subscriber.SubGenPublicationID);
            SetDataRowField(dataRow, SubGenMailingAddressIdFieldName, subscriber.SubGenMailingAddressId);
            SetDataRowField(dataRow, SubGenBillingAddressIdFieldName, subscriber.SubGenBillingAddressId);
            SetDataRowField(dataRow, IssuesLeftFieldName, subscriber.IssuesLeft);
            SetDataRowField(dataRow, UnearnedReveueFieldName, subscriber.UnearnedReveue);

            foreach (var subscriberDemographic in subscriber.DemographicTransformedList)
            {
                SetDataRowField(dataRow, subscriberDemographic.MAFField, subscriberDemographic.Value);
            }

            table.Rows.Add(dataRow);
        }

        private void SetDataRowField<T>(DataRow row, string columnName, T fieldValue)
        {
            if (row == null || string.IsNullOrWhiteSpace(columnName))
            {
                return;
            }

            row[columnName] = fieldValue;
        }
    }
}
