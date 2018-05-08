using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Core_AMS.Utilities;
using FrameworkSubGen.Entity;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic;
using FrameworkUAS.Entity;
using KM.Common.Functions;
using KMPlatform.Entity;
using Enums = FrameworkUAD_Lookup.Enums;
using Payment = FrameworkSubGen.BusinessLogic.Payment;
using Service = KMPlatform.BusinessLogic.Service;
using StringFunctions = KM.Common.StringFunctions;

namespace AMS_Operations
{
    public delegate void LogImportErrorFuncDelegate(
        string method,
        string errorMsg,
        StringDictionary myRow = null,
        FileInfo file = null,
        ImportSubscriber iSub = null,
        SubscriberTransformed st = null);

    public class ImportSubscriberConverter
    {
        private const char CommaDelimiter = ',';

        private readonly Action<string> _consoleMessage;
        private readonly LogImportErrorFuncDelegate _logImportError;
        private readonly List<Publication> _publications;

        public ImportSubscriberConverter(Action<string> consoleMessageFunc, LogImportErrorFuncDelegate logImportErrorFunc, List<Publication> publications)
        {
            _consoleMessage = consoleMessageFunc;
            _logImportError = logImportErrorFunc;
            _publications = publications;
        }

        public List<SubscriberTransformed> Convert_ImportSubscriber_to_SubscriberTrans(IReadOnlyCollection<ImportSubscriber> isList, Account account)
        {
            Client client;
            List<Product> clientProducts;
            List<CodeSheet> codeSheets;
            List<ResponseGroup> responseGroups;
            List<ProductSubscriptionsExtensionMapper> productSubscriptionsExtensionMappers;
            List<AdHocDimensionGroup> ahdGroups;
            List<SubscriberTransformed> subscriberTransformeds;
            var counter = 1;
            var processCode = ConfigureConvertImportSubscriberSubscriberTransProcessing(account, out client, out clientProducts, out codeSheets, out responseGroups, out productSubscriptionsExtensionMappers, out ahdGroups, out subscriberTransformeds);

            var sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
            var sourceFile = sfWorker.Select(client.ClientID, "Paid_CDC_Import") ?? CreateUasWsAddSubscriberFile(client, sfWorker);

            var rowNumber = 1;
            foreach (var importSubscriber in isList)
            {
                ConvertSubscriberImportedToTransformed(
                    isList,
                    counter,
                    importSubscriber,
                    rowNumber,
                    processCode,
                    sourceFile,
                    client,
                    clientProducts,
                    codeSheets,
                    responseGroups,
                    productSubscriptionsExtensionMappers,
                    ahdGroups,
                    subscriberTransformeds);
                counter++;
            }
            return subscriberTransformeds;
        }

        private void ConvertSubscriberImportedToTransformed(
            IReadOnlyCollection<ImportSubscriber> isList,
            int counter,
            ImportSubscriber importSubscriber,
            int rowNumber,
            string processCode,
            SourceFile sf,
            Client client,
            List<Product> clientProducts,
            List<CodeSheet> csList,
            List<ResponseGroup> rgList,
            List<ProductSubscriptionsExtensionMapper> pseList,
            List<AdHocDimensionGroup> ahdGroups,
            List<SubscriberTransformed> stList)
        {
            _consoleMessage($"SubGen to UAD Conversion item {counter} of {isList.Count}");
            var subscriberTransformed = new SubscriberTransformed();
            UpdatePayment(importSubscriber, subscriberTransformed);

            try
            {
                CopyFieldsImportedToTransformed(importSubscriber, rowNumber, processCode, sf, client, clientProducts, subscriberTransformed);
                FillTransformedCategoryId(importSubscriber, subscriberTransformed);
                FillTransformedTransactionId(importSubscriber, subscriberTransformed);
                FillTransformedQSourceId(importSubscriber, subscriberTransformed);
                FillTransformedDimensions(importSubscriber, clientProducts, csList, rgList, pseList, ahdGroups, subscriberTransformed);
                PrepareDemoData(importSubscriber, clientProducts, rgList, subscriberTransformed);

                if (subscriberTransformed.QDate.HasValue &&
                    subscriberTransformed.CategoryID > 0 &&
                    subscriberTransformed.QSourceID > 0 &&
                    subscriberTransformed.TransactionID > 0 &&
                    subscriberTransformed.SubGenSubscriberID > 0 &&
                    !string.IsNullOrWhiteSpace(subscriberTransformed.PubCode))
                {
                    stList.Add(subscriberTransformed);
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("** Excluded Record **");
                    sb.AppendLine("Record did not meet minimum requirements");
                    sb.AppendLine($"QDate must have value - does QDate have value - {subscriberTransformed.QDate.HasValue}");
                    sb.AppendLine($"CategoryID > 0 - CategoryID value - {subscriberTransformed.CategoryID}");
                    sb.AppendLine($"QSourceID > 0 - QSourceID value - {subscriberTransformed.QSourceID}");
                    sb.AppendLine($"TransactionID > 0 - TransactionID value - {subscriberTransformed.TransactionID}");
                    sb.AppendLine($"SubGenSubscriberID > 0 - SubGenSubscriberID value - {subscriberTransformed.SubGenSubscriberID}");
                    sb.AppendLine($"PubCode not blank - PubCode value - {subscriberTransformed.PubCode}");
                    sb.AppendLine("*********************");
                    _logImportError(
                        "Convert_ImportSubscriber_to_SubscriberTrans",
                        sb.ToString(),
                        null,
                        null,
                        importSubscriber,
                        subscriberTransformed);
                }
            }
            catch (Exception ex)
            {
                _logImportError(
                    "Convert_ImportSubscriber_to_SubscriberTrans",
                    StringFunctions.FormatException(ex),
                    null,
                    null,
                    importSubscriber);
            }
        }

        private void PrepareDemoData(
            ImportSubscriber importSubscriber,
            List<Product> clientProducts,
            List<ResponseGroup> rgList,
            SubscriberTransformed subscriberTransformed)
        {
            var rgCheck = rgList.Where(x =>
                    x.PubCode.Equals(subscriberTransformed.PubCode, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            var requiredDemos =
                rgCheck.Where(x => x.IsRequired == true).Select(y => y.ResponseGroupName.ToUpper()).ToList();
            foreach (var maf in requiredDemos)
            {
                if (subscriberTransformed.DemographicTransformedList.Any(x =>
                    x.MAFField.Equals(maf, StringComparison.CurrentCultureIgnoreCase)))
                {
                    continue;
                }

                try
                {
                    var pubId = 0;
                    if (subscriberTransformed.DemographicTransformedList.Count > 0)
                    {
                        pubId = subscriberTransformed.DemographicTransformedList.First().PubID;
                    }

                    if (pubId == 0)
                    {
                        pubId = clientProducts.First(x => x.PubCode == subscriberTransformed.PubCode).PubID;
                    }

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
                        SORecordIdentifier = subscriberTransformed.SORecordIdentifier,
                        STRecordIdentifier = subscriberTransformed.STRecordIdentifier,
                        Value = "xxNVxx"
                    };
                    subscriberTransformed.DemographicTransformedList.Add(sdt);
                }
                catch (Exception ex)
                {
                    _logImportError(
                        "Convert_ImportSubscriber_to_SubscriberTrans",
                        StringFunctions.FormatException(ex),
                        null,
                        null,
                        importSubscriber);
                }
            }
        }

        private void FillTransformedDimensions(
            ImportSubscriber importSubscriber,
            List<Product> clientProducts,
            List<CodeSheet> csList,
            List<ResponseGroup> rgList,
            List<ProductSubscriptionsExtensionMapper> pseList,
            List<AdHocDimensionGroup> ahdGroups,
            SubscriberTransformed subscriberTransformed)
        {
            subscriberTransformed.MailPermission = null;
            subscriberTransformed.FaxPermission = null;
            subscriberTransformed.PhonePermission = null;
            subscriberTransformed.OtherProductsPermission = null;
            subscriberTransformed.ThirdPartyPermission = null;
            subscriberTransformed.EmailRenewPermission = null;

            foreach (var idd in importSubscriber.Dimensions.Dimensions)
            {
                ParseNullableBoolDimension(idd, "demo31", result => subscriberTransformed.MailPermission = result);
                ParseNullableBoolDimension(idd, "demo32", result => subscriberTransformed.FaxPermission = result);
                ParseNullableBoolDimension(idd, "demo33", result => subscriberTransformed.PhonePermission = result);
                ParseNullableBoolDimension(idd, "demo34", result => subscriberTransformed.OtherProductsPermission = result);
                ParseNullableBoolDimension(idd, "demo35", result => subscriberTransformed.ThirdPartyPermission = result);
                ParseNullableBoolDimension(idd, "demo36", result => subscriberTransformed.EmailRenewPermission = result);
                if (idd.DimensionField.Equals("fax", StringComparison.CurrentCultureIgnoreCase))
                {
                    subscriberTransformed.Fax = idd.DimensionValue;
                }

                if (idd.DimensionField.Contains("-"))
                {
                    ParseMultivalueDimension(importSubscriber, clientProducts, csList, rgList, pseList, ahdGroups, subscriberTransformed, idd);
                }
            }
        }

        private void ParseMultivalueDimension(
            ImportSubscriber importSubscriber,
            IEnumerable<Product> clientProducts,
            IEnumerable<CodeSheet> csList,
            IEnumerable<ResponseGroup> rgList,
            List<ProductSubscriptionsExtensionMapper> pseList,
            List<AdHocDimensionGroup> ahdGroups,
            ITransformedSubscriber subscriberTransformed,
            ImportDimensionDetail idd)
        {
            //need to support Multi value answers - need example coming out of SubGen
            //if SubGen field is xxxx - Other then need to take the first part as the Demographic and the answer goes to the ResponseOther field
            var prod = clientProducts.Single(x => x.PubCode.Trim() == subscriberTransformed.PubCode.Trim());

            bool isOther;
            var field = GetFieldName(pseList, idd, prod, out isOther);

            var csProd = csList.Where(x =>
                    x.PubID == prod.PubID &&
                    x.ResponseGroup.Trim().Equals(field, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            var rgProd = rgList.SingleOrDefault(x =>
                x.PubID == prod.PubID &&
                x.ResponseGroupName.Trim().Equals(field, StringComparison.CurrentCultureIgnoreCase));
            if (rgProd == null || rgProd.PubID == 0)
            {
                rgProd = new ResponseGroup();
            }

            var values = new string[1]; //idd.DimensionValue;
            if (rgProd.IsMultipleValue == true)
            {
                values = idd.DimensionValue.Split(CommaDelimiter); //if value has legit commas it gets split
            }
            else
            {
                values[0] = idd.DimensionValue;
            }

            foreach (var value in values)
            {
                var dimValue = string.Empty;
                dimValue = BuildDimensionValue(importSubscriber, idd, isOther, dimValue, value, csProd, rgProd);
                AddSubscriberDemographicsTransformed(pseList, ahdGroups, subscriberTransformed, field, prod, isOther, dimValue);
            }
        }

        private static void AddSubscriberDemographicsTransformed(
            List<ProductSubscriptionsExtensionMapper> pseList,
            List<AdHocDimensionGroup> ahdGroups,
            ITransformedSubscriber subscriberTransformed,
            string field,
            Product prod,
            bool isOther,
            string dimValue)
        {
            var sdt = new SubscriberDemographicTransformed
            {
                CreatedByUserID = subscriberTransformed.CreatedByUserID,
                DateCreated = DateTime.Now,
                MAFField = field,
                NotExists = false,
                PubID = prod.PubID,
                STRecordIdentifier = subscriberTransformed.STRecordIdentifier,
                ResponseOther = isOther ? dimValue : string.Empty,
                Value = isOther ? string.Empty : dimValue,
                IsAdhoc = ahdGroups.Exists(x => x.CreatedDimension.Equals(field, StringComparison.CurrentCultureIgnoreCase))
            };

            if (pseList.Exists(x =>
                x.PubID == prod.PubID &&
                x.CustomField.Trim().Equals(field, StringComparison.CurrentCultureIgnoreCase)))
            {
                sdt.IsAdhoc = true;
                sdt.ResponseOther = string.Empty;
                sdt.Value = dimValue;
            }

            subscriberTransformed.DemographicTransformedList.Add(sdt);
        }

        private string BuildDimensionValue(
            ImportSubscriber importSubscriber,
            ImportDimensionDetail idd,
            bool isOther,
            string dimValue,
            string value,
            List<CodeSheet> csProd,
            ResponseGroup rgProd)
        {
            if (isOther == false)
            {
                dimValue = StringFunctions.RemoveNonAlphaNumeric(value);
                if (csProd.Exists(x =>
                    x.ResponseDesc.Equals(dimValue, StringComparison.CurrentCultureIgnoreCase)))
                {
                    dimValue = csProd
                        .FirstOrDefault(x => x.ResponseDesc.Equals(dimValue, StringComparison.CurrentCultureIgnoreCase))
                        ?.ResponseValue.Trim();
                }
                else if (csProd.Exists(x => x.ResponseValue.Equals(dimValue, StringComparison.CurrentCultureIgnoreCase)))
                {
                    dimValue = csProd
                        .FirstOrDefault(x => x.ResponseValue.Equals(dimValue, StringComparison.CurrentCultureIgnoreCase))
                        ?.ResponseValue.Trim();
                }
                else if (rgProd.IsMultipleValue == true)
                {
                    dimValue = Core_AMS.Utilities.StringFunctions.Allow_Numbers_Letters_Spaces_Dashes(idd.DimensionValue);
                    if (csProd.Exists(x => x.ResponseDesc.Equals(dimValue, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        dimValue = csProd
                            .FirstOrDefault(x => x.ResponseDesc.Equals(dimValue, StringComparison.CurrentCultureIgnoreCase))
                            ?.ResponseValue.Trim();
                    }
                    else
                    {
                        _logImportError(
                            "Convert_ImportSubscriber_to_SubscriberTrans",
                            $"SubGen DimensionValue does not match values in UAD.CodeSheet - Field: {idd.DimensionField} Value: {idd.DimensionValue}",
                            null,
                            null,
                            importSubscriber);
                    }
                }
                else
                {
                    _logImportError(
                        "Convert_ImportSubscriber_to_SubscriberTrans",
                        $"SubGen DimensionValue does not match values in UAD.CodeSheet - Field: {idd.DimensionField} Value: {idd.DimensionValue}",
                        null,
                        null,
                        importSubscriber);
                }
            }
            else
            {
                dimValue = Core_AMS.Utilities.StringFunctions.Allow_Numbers_Letters_Spaces_Dashes(value);
            }

            return dimValue;
        }

        private static string GetFieldName(List<ProductSubscriptionsExtensionMapper> pseList, ImportDimensionDetail idd, Product prod, out bool isOther)
        {
            var dimField = idd.DimensionField.Split('-');
            string field;
            isOther = false;
            if (idd.DimensionField.IndexOf("OTHER",StringComparison.OrdinalIgnoreCase)>=0)
            {
                field = dimField[1].Trim();
                isOther = true;
            }
            else
            {
                field = dimField[1].Trim();
            }

            if (pseList.Exists(x =>
                x.PubID == prod.PubID &&
                x.CustomField.Trim().Equals(field, StringComparison.CurrentCultureIgnoreCase)))
            {
                isOther = true;
            }

            return field;
        }

        private static void ParseNullableBoolDimension(ImportDimensionDetail idd, string fieldName, Action<bool?> resultAction)
        {
            if (idd.DimensionField.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase))
            {
                if (idd.DimensionValue.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    resultAction(true);
                }
                else if (idd.DimensionValue.Equals("No", StringComparison.CurrentCultureIgnoreCase))
                {
                    resultAction(false);
                }
                else
                {
                    resultAction(null);
                }
            }
        }

        private static void FillTransformedQSourceId(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            //iSub.AuditRequestTypeName;//Qualification Source
            //iSub.AuditRequestTypeCode;//Qualification Source
            //SubGen Codes (NRI,OTH,CDI,CDW,PDI,PDT,PDW,blank) P=Personal, C=Company
            //KM Codes
            //--Internet or email                       NRI = KM S CodeValue CodeId = 1884
            //--Other sources                           OTH = KM N CodeValue CodeId = 1890
            //--Written                                 CDW = KM C CodeValue CodeId = 1877
            //--Internet or email                       CDI = KM R CodeValue CodeId = 1879
            //--Internet or email                       PDI = KM Q CodeValue CodeId = 1876
            //--Telecommunication                       PDT = KM B CodeValue CodeId = 1875
            //--Written                                 PDW = KM A CodeValue CodeId = 1874
            //--blank                                   blank = KM N CodeValue CodeId = 1890
            //--Other Communication - Non-Request       blank  = KM N CodeValue CodeId = 1890
            var qsWorker = new Code();
            var codeMap = new Dictionary<string, string>
            {
                {"NRI", "S"},
                {"OTH", "N"},
                {"CDW", "C"},
                {"CDI", "R"},
                {"PDI", "Q"},
                {"PDT", "B"},
                {"PDW", "A"}
            };

            var codeVal = importSubscriber?.AuditRequestTypeCode ?? "";
            subscriberTransformed.QSourceID =
                qsWorker.SelectCodeValue(
                    Enums.CodeType.Qualification_Source,
                    codeMap.ContainsKey(codeVal) ? codeMap[codeVal] : "N").CodeId;
        }

        private static void FillTransformedTransactionId(
            ImportSubscriber importSubscriber,
            SubscriberTransformed subscriberTransformed)
        {
            //if TransactionID has not been set lets set based on business rule
            //need to decide TransactionID based on a set of Busniess Rules - see AMS: Transaction Types sheet
            //look at file field "Subscription Expire Date"
            //if date > current date then it is an active so set to 40
            //if date <= current date then it is an InActive so set to 60
            if (importSubscriber.TransactionID == 0)
            {
                importSubscriber.TransactionID = importSubscriber.SubscriptionExpireDate.Date > DateTime.Now.Date ? 40 : 60;
            }

            if (importSubscriber.TransactionID > 0)
            {
                subscriberTransformed.TransactionID = importSubscriber.TransactionID;
            }
            else
            {
                //UAD_Lookup..TransactionCodeType = 3   Paid Active
                //per Meghan setting this to TranCode 40 Renewal Payment (Active Paid)
                var tcWorker = new TransactionCode();
                var tc = tcWorker.SelectTransactionCodeValue(40);
                subscriberTransformed.TransactionID = tc.TransactionCodeID;
            }
        }

        private static void FillTransformedCategoryId(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            if (importSubscriber == null)
            {
                throw new ArgumentNullException(nameof(importSubscriber));
            }

            if (importSubscriber.AuditCategoryCode == null)
            {
                throw new ArgumentException(nameof(importSubscriber));
            }

            var categoryMap = new Dictionary<string, int>
            {
                {"NQP", 30},
                {"NQO", 32},
                {"PI", 20},
                {"PB", 21},
                {"PM", 27},
                {"", 30}
            };
            var categoryKey = importSubscriber.AuditCategoryCode ?? "";
            if (!categoryMap.ContainsKey(categoryKey))
            {
                categoryKey = string.Empty;
            }

            subscriberTransformed.CategoryID = categoryMap[categoryKey];

            //PBI - 33215
            //If SubGen record is Non-Qualified Paid:
            //      and Audit Category = Non-Qualified Paid
            //      and copies >1
            //      and Site License Master = blank…OR “0”…OR “No”
            //      and Site License Seat = blank…OR “0”…OR “No”
            //          process to AMS Category Code = 31 (Bulk, NQ paid)
            if (importSubscriber.AuditCategoryCode.Equals("NQP") &&
                importSubscriber.Copies > 1 &&
                importSubscriber.IsSiteLicenseMaster == false &&
                importSubscriber.IsSiteLicenseSeat == false)
            {
                subscriberTransformed.CategoryID = 31;
            }
        }

        private void CopyFieldsImportedToTransformed(
            ImportSubscriber importSubscriber,
            int rowNumber,
            string processCode,
            SourceFile sf,
            Client client,
            List<Product> clientProducts,
            SubscriberTransformed subscriberTransformed)
        {
            subscriberTransformed.ImportRowNumber = rowNumber;
            subscriberTransformed.IsPaid = true;
            subscriberTransformed.ProcessCode = processCode;
            subscriberTransformed.SourceFileID = sf.SourceFileID;

            subscriberTransformed.SubGenSubscriberID = importSubscriber.SystemSubscriberID;
            subscriberTransformed.AccountNumber = importSubscriber.SystemSubscriberID.ToString();
            subscriberTransformed.SubGenSubscriptionID = importSubscriber.SubscriptionID;
            subscriberTransformed.SubGenMailingAddressId = importSubscriber.SubscriptionGeniusMailingAddressID;
            subscriberTransformed.SubGenBillingAddressId = importSubscriber.SystemBillingAddressID;
            subscriberTransformed.SubGenPublicationID = importSubscriber.PublicationID;
            TransformFullName(importSubscriber, subscriberTransformed);

            TransfrormCompanyName(importSubscriber, subscriberTransformed);

            subscriberTransformed.Title = importSubscriber.MailingAddressTitle != null
                ? XmlFunctions.RemoveFormatXMLSpecialCharacters(importSubscriber.MailingAddressTitle)
                : string.Empty;
            subscriberTransformed.Email = importSubscriber.SubscriberEmail ?? string.Empty;
            subscriberTransformed.Phone = importSubscriber.SubscriberPhone ?? string.Empty;
            subscriberTransformed.Source = importSubscriber.SubscriberSource ?? string.Empty;
            subscriberTransformed.Address = importSubscriber.MailingAddressLine1 != null
                ? XmlFunctions.RemoveFormatXMLSpecialCharacters(importSubscriber.MailingAddressLine1)
                : string.Empty;
            subscriberTransformed.MailStop = importSubscriber.MailingAddressLine2 != null
                ? XmlFunctions.RemoveFormatXMLSpecialCharacters(importSubscriber.MailingAddressLine2)
                : string.Empty;
            subscriberTransformed.City = importSubscriber.MailingAddressCity ?? string.Empty;
            TransformMailingAddress(importSubscriber, subscriberTransformed);
            subscriberTransformed.Country = importSubscriber.MailingAddressCountry ?? string.Empty;
            TransformDigitalPrint(importSubscriber, subscriberTransformed);
            TransformQDate(importSubscriber, subscriberTransformed);
            TransformPubCode(importSubscriber, client, clientProducts, subscriberTransformed);
            subscriberTransformed.Copies = importSubscriber.Copies;
            subscriberTransformed.IssuesLeft = importSubscriber.IssuesLeft;
            subscriberTransformed.UnearnedReveue = Convert.ToDecimal(importSubscriber.UnearnedRevenue);
            subscriberTransformed.SubGenRenewalCode = importSubscriber.RenewalCode_CustomerID ?? string.Empty;
            subscriberTransformed.SubGenIsLead = importSubscriber.IsLead;
            subscriberTransformed.SubGenSubscriptionRenewDate = importSubscriber.SubscriptionRenewDate;
            subscriberTransformed.SubGenSubscriptionExpireDate = importSubscriber.SubscriptionExpireDate;
            subscriberTransformed.SubGenSubscriptionLastQualifiedDate = importSubscriber.SubscriptionLastQualifiedDate;
        }

        private void TransformPubCode(
            ImportSubscriber importSubscriber,
            Client client,
            List<Product> clientProducts,
            SubscriberTransformed subscriberTransformed)
        {
            var pubCode = _publications.SingleOrDefault(x => x.publication_id == importSubscriber.PublicationID)?.KMPubCode;
            if (_publications.Exists(x => x.publication_id == importSubscriber.PublicationID))
            {
                subscriberTransformed.PubCode = !string.IsNullOrWhiteSpace(pubCode) 
                        ? pubCode
                        : Find_KM_PubCode(client, importSubscriber.PublicationName, clientProducts);
            }
            else
            {
                subscriberTransformed.PubCode = Find_KM_PubCode(client, importSubscriber.PublicationName, clientProducts);
            }
        }

        private static void TransformQDate(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            //rule changed per Gail 3/31/2016 to this set KM QDate equal to SubGen SubscriptionLastQualifiedDate, if that is blank then set to SubscriptionCreatedDate
            //st.QDate = iSub.SubscriptionCreatedDate != null ? iSub.SubscriptionCreatedDate : DateTime.Now; old rule through 3/30/2016 per Joe B and Meghan
            if (!string.IsNullOrWhiteSpace(importSubscriber?.SubscriptionLastQualifiedDate.ToShortDateString()) &&
                importSubscriber.SubscriptionLastQualifiedDate > DateTimeFunctions.GetMinDate())
            {
                subscriberTransformed.QDate = importSubscriber.SubscriptionLastQualifiedDate;
            }
            else
            {
                subscriberTransformed.QDate = importSubscriber?.SubscriptionCreatedDate ?? DateTime.Now;
            }
        }

        private static void TransformDigitalPrint(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            if (importSubscriber.SubscriptionType.Equals("Both"))
            {
                subscriberTransformed.Demo7 = "C";
            }
            else if (importSubscriber.SubscriptionType.Equals("Digital"))
            {
                subscriberTransformed.Demo7 = "B";
            }
            else if (importSubscriber.SubscriptionType.Equals("Print"))
            {
                subscriberTransformed.Demo7 = "A";
            }
        }

        private static void TransformMailingAddress(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            if (importSubscriber.MailingAddressCountry != null)
            {
                if (importSubscriber.MailingAddressCountry.Equals("United States"))
                {
                    subscriberTransformed.State = importSubscriber.MailingAddressState ?? string.Empty;
                    if (importSubscriber.MailingAddressZip.Length > 5)
                    {
                        if (importSubscriber.MailingAddressZip.Contains("-"))
                        {
                            var zips = importSubscriber.MailingAddressZip.Split('-');
                            subscriberTransformed.Zip = zips[0];
                            subscriberTransformed.Plus4 = zips[1];
                        }
                        else
                        {
                            subscriberTransformed.Zip = importSubscriber.MailingAddressZip.Take(5).ToString();
                            subscriberTransformed.Plus4 = importSubscriber.MailingAddressZip.Substring(5);
                        }
                    }
                    else if (importSubscriber.MailingAddressZip.Length == 4)
                    {
                        subscriberTransformed.Zip = "0" + importSubscriber.MailingAddressZip;
                        subscriberTransformed.Plus4 = string.Empty;
                    }
                    else
                    {
                        subscriberTransformed.Zip = importSubscriber.MailingAddressZip ?? string.Empty;
                        subscriberTransformed.Plus4 = string.Empty;
                    }
                }
                else if (
                    importSubscriber.MailingAddressCountry.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) ||
                    importSubscriber.MailingAddressCountry.Equals("Mexico", StringComparison.CurrentCultureIgnoreCase))
                {
                    subscriberTransformed.State = importSubscriber.MailingAddressState;
                    subscriberTransformed.Zip = importSubscriber.MailingAddressZip ?? string.Empty;
                    subscriberTransformed.Plus4 = string.Empty;
                }
                else
                {
                    subscriberTransformed.State = "FO";
                    subscriberTransformed.Zip = importSubscriber.MailingAddressZip ?? string.Empty;
                    subscriberTransformed.Plus4 = string.Empty;
                }
            }
            else
            {
                subscriberTransformed.State = importSubscriber.MailingAddressState ?? string.Empty;
                subscriberTransformed.Zip = importSubscriber.MailingAddressZip ?? string.Empty;
                subscriberTransformed.Plus4 = string.Empty;
            }
        }

        private static void TransfrormCompanyName(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            if (!string.IsNullOrWhiteSpace(importSubscriber.MailingAddressCompany))
            {
                subscriberTransformed.Company = importSubscriber.MailingAddressCompany != null
                    ? XmlFunctions.RemoveFormatXMLSpecialCharacters(importSubscriber.MailingAddressCompany)
                    : string.Empty;
            }
            else
            {
                subscriberTransformed.Company = importSubscriber.BillingAddressCompany != null
                    ? XmlFunctions.RemoveFormatXMLSpecialCharacters(importSubscriber.BillingAddressCompany)
                    : string.Empty;
            }
        }

        private static void TransformFullName(ImportSubscriber importSubscriber, SubscriberTransformed subscriberTransformed)
        {
            if (!string.IsNullOrWhiteSpace(importSubscriber.SubscriberAccountFirstName) &&
                !string.IsNullOrWhiteSpace(importSubscriber.SubscriberAccountLastName))
            {
                subscriberTransformed.FName = importSubscriber.SubscriberAccountFirstName ?? string.Empty;
                subscriberTransformed.LName = importSubscriber.SubscriberAccountLastName ?? string.Empty;
            }
            else if (!string.IsNullOrWhiteSpace(importSubscriber.MailingAddressFirstName) &&
                     !string.IsNullOrWhiteSpace(importSubscriber.MailingAddressLastName))
            {
                subscriberTransformed.FName = importSubscriber.MailingAddressFirstName ?? string.Empty;
                subscriberTransformed.LName = importSubscriber.MailingAddressLastName ?? string.Empty;
            }
            else if (!string.IsNullOrWhiteSpace(importSubscriber.MailingAddressTitle) &&
                     !string.IsNullOrWhiteSpace(importSubscriber.MailingAddressCompany))
            {
                subscriberTransformed.FName = string.Empty;
                subscriberTransformed.LName = string.Empty;
            }
            else if (!string.IsNullOrWhiteSpace(importSubscriber.BillingAddressFirstName) &&
                     !string.IsNullOrWhiteSpace(importSubscriber.BillingAddressLastName))
            {
                subscriberTransformed.FName = importSubscriber.BillingAddressFirstName ?? string.Empty;
                subscriberTransformed.LName = importSubscriber.BillingAddressLastName ?? string.Empty;
            }
        }

        private void UpdatePayment(ImportSubscriber iSub, SubscriberTransformed st)
        {
            try
            {
                var pwrk = new Payment();
                var pay = pwrk.Select(iSub.SystemSubscriberID, iSub.DateCreated);
                if (pay != null && pay.order_id > 0)
                {
                    pwrk.Update_STRecordIdentifier(pay.order_id, st.STRecordIdentifier);
                }
            }
            catch (Exception ex)
            {
                _logImportError("Convert_ImportSubscriber_to_SubscriberTrans",
                    StringFunctions.FormatException(ex),
                    null,
                    null,
                    iSub);
            }
        }

        private static SourceFile CreateUasWsAddSubscriberFile(Client client, FrameworkUAS.BusinessLogic.SourceFile sfWorker)
        {
            var codeWorker = new Code();
            var codes = codeWorker.Select(Enums.CodeType.File_Recurrence);
            var dbFiletypes = codeWorker.Select(Enums.CodeType.Database_File);
            var service = new Service().Select(KMPlatform.Enums.Services.FULFILLMENT, true);

            //create the UAD_WS_AddSubscriber file for the client
            var sourceFile = new SourceFile { FileRecurrenceTypeId = 0 };
            if (codes.Exists(x => x.CodeName.Equals(Enums.FileRecurrenceTypes.Recurring.ToString())))
            {
                sourceFile.FileRecurrenceTypeId = codes.Single(x => x.CodeName.Equals(Enums.FileRecurrenceTypes.Recurring.ToString())).CodeId;
            }

            sourceFile.DatabaseFileTypeId = dbFiletypes.Single(x => x.CodeName.Equals("Paid Transaction")).CodeId;
            sourceFile.FileName = "Paid_CDC_Import";
            sourceFile.Extension = ".csv";
            sourceFile.Delimiter = "comma";
            sourceFile.IsTextQualifier = true;
            sourceFile.ClientID = client.ClientID;
            sourceFile.IsDQMReady = true;
            sourceFile.ServiceID = service.ServiceID;
            sourceFile.ServiceFeatureID = 19;
            if (service.ServiceFeatures.Exists(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.FulfillmentFeatures.File_Import.ToString().Replace("_", " "))))
            {
                sourceFile.ServiceFeatureID = service.ServiceFeatures.Single(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.FulfillmentFeatures.File_Import.ToString()
                    .Replace("_", " ")))
                    .ServiceFeatureID;
            }

            sourceFile.CreatedByUserID = 1;
            sourceFile.QDateFormat = "MMDDYYYY";
            sourceFile.BatchSize = 2500;
            sourceFile.SourceFileID = sfWorker.Save(sourceFile);
            return sourceFile;
        }

        private string ConfigureConvertImportSubscriberSubscriberTransProcessing(
            Account account,
            out Client client,
            out List<Product> clientProducts,
            out List<CodeSheet> csList,
            out List<ResponseGroup> rgList,
            out List<ProductSubscriptionsExtensionMapper> pseList,
            out List<AdHocDimensionGroup> ahdGroups,
            out List<SubscriberTransformed> stList)
        {
            var processCode = StringFunctions.GenerateProcessCode();
            _consoleMessage("Start - Get UAD data - client, Products, CodeSheet, AdHocDimensionGroup");
            var cWorker = new KMPlatform.BusinessLogic.Client();
            client = cWorker.Select(account.KMClientId);
            var pWorker = new FrameworkUAD.BusinessLogic.Product();
            clientProducts = pWorker.Select(client.ClientConnections);
            var csWorker = new FrameworkUAD.BusinessLogic.CodeSheet();
            csList = csWorker.Select(client.ClientConnections);
            var rgWorker = new FrameworkUAD.BusinessLogic.ResponseGroup();
            rgList = rgWorker.Select(client.ClientConnections);
            var pseWrk = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
            pseList = pseWrk.SelectAll(client.ClientConnections);

            foreach (var cs in csList)
            {
                cs.ResponseDesc = StringFunctions.RemoveNonAlphaNumeric(cs.ResponseDesc);
                cs.ResponseValue = StringFunctions.RemoveNonAlphaNumeric(cs.ResponseValue);
            }

            var ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            ahdGroups = ahdgWorker.Select(client.ClientID)
                .Where(x => x.IsActive)
                .OrderBy(y => y.OrderOfOperation)
                .ToList();
            _consoleMessage("Done - Get UAD data - client, Products, CodeSheet, AdHocDimensionGroup");
            stList = new List<SubscriberTransformed>();
            return processCode;
        }

        string Find_KM_PubCode(Client client, string subGenPubName, List<Product> clientProducts = null)
        {
            var pubCode = string.Empty;
            if (clientProducts == null)
            {
                var pWorker = new FrameworkUAD.BusinessLogic.Product();
                clientProducts = pWorker.Select(client.ClientConnections);
            }
            foreach (var product in clientProducts)
            {
                if (product.PubName.ToLower().Contains(subGenPubName.ToLower()))
                {
                    var prod = clientProducts.Single(x => x.PubName.Replace($"{x.PubCode}- ", "")
                        .Trim()
                        .Equals(subGenPubName.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (prod == null || prod.PubID <= 0)
                    {
                        continue;
                    }

                    pubCode = prod.PubCode;
                    break;
                }
            }
            return pubCode;
        }
    }
}
