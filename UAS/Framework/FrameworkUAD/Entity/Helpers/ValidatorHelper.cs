using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrameworkUAS.Entity;
using KM.Common;

namespace FrameworkUAD.Entity.Helpers
{
    public class ValidatorHelper
    {
        private const string DemographicsOtherToken = "_RESPONSEOTHER";
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(2079, 1, 1);

        public static void ExtendWithDemographicsItem<T>(
            IEnumerable<AdHocDimensionGroup> ahdGroups,
            T subscriber,
            int demographicUpdateCodeId,
            string fieldName,
            int pubCodeId,
            DateTime? dateCreated,
            bool isDemoDate,
            string value) where T : ISubscriber
        {
            var subscriberOriginal = subscriber as IOriginalSubscriber;
            var subscriberTransformed = subscriber as ITransformedSubscriber;

            var sdo = subscriberOriginal != null ?
                new SubscriberDemographicOriginal() :
                (ISubscriberDemographic)new SubscriberDemographicTransformed();

            sdo.CreatedByUserID = subscriber.CreatedByUserID;
            sdo.MAFField = fieldName;
            sdo.NotExists = false;
            sdo.PubID = pubCodeId;
            sdo.SORecordIdentifier = subscriber.SORecordIdentifier;
            sdo.Value = value;
            sdo.DemographicUpdateCodeId = demographicUpdateCodeId;

            if (ahdGroups.Any(x => x.CreatedDimension.Equals(sdo.MAFField, StringComparison.CurrentCultureIgnoreCase)))
            {
                sdo.IsAdhoc = true;
            }

            sdo.ResponseOther = string.Empty;

            if (subscriberOriginal != null)
            {
                var originalSdo = (SubscriberDemographicOriginal)sdo;
                originalSdo.DateCreated = dateCreated.GetValueOrDefault(MinDate);
                subscriberOriginal.DemographicOriginalList.Add(originalSdo);
            }

            if (subscriberTransformed != null)
            {
                var transformedSdo = (SubscriberDemographicTransformed)sdo;
                transformedSdo.DateCreated = dateCreated;
                transformedSdo.IsDemoDate = isDemoDate;

                transformedSdo.STRecordIdentifier = subscriberTransformed.STRecordIdentifier;
                subscriberTransformed.DemographicTransformedList.Add(transformedSdo);
            }
        }

        public static void ExtendWithDemographicsItem<T>(
            IEnumerable<AdHocDimensionGroup> ahdGroups,
            T subscriber,
            int demographicUpdateCodeId,
            string fieldName,
            int pubCodeId,
            string value) where T: ISubscriber
        {
            ExtendWithDemographicsItem(
                ahdGroups,
                subscriber,
                demographicUpdateCodeId,
                fieldName,
                pubCodeId,
                DateTime.Now,
                false,
                value);
        }

        public static void SetProfileFieldValueByFieldMapping(IFieldMapping mmap, ISubscriber subscriberOriginal, string value)
        {
            var field = mmap.MAFField.ToLower();
            switch (field)
            {
                case "pubcode":
                    subscriberOriginal.PubCode = value;
                    break;
                case "sequenceid":
                    subscriberOriginal.Sequence = ParseInt(value);
                    break;
                case "firstname":
                    subscriberOriginal.FName = value;
                    break;
                case "lastname":
                    subscriberOriginal.LName = value;
                    break;
                case "title":
                    subscriberOriginal.Title = value;
                    break;
                case "company":
                    subscriberOriginal.Company = value;
                    break;
                case "address1":
                    subscriberOriginal.Address = value;
                    break;
                case "address2":
                    subscriberOriginal.MailStop = value;
                    break;
                case "city":
                    subscriberOriginal.City = value;
                    break;
                case "regioncode":
                    subscriberOriginal.State = value;
                    break;
                case "zipcode":
                    subscriberOriginal.Zip = value;
                    break;
                case "plus4":
                    subscriberOriginal.Plus4 = value;
                    break;
                case "forzip":
                    subscriberOriginal.ForZip = value;
                    break;
                case "county":
                    subscriberOriginal.County = value;
                    break;
                case "country":
                    subscriberOriginal.Country = value;
                    break;
                case "countryid":
                    subscriberOriginal.CountryID = ParseInt(value);
                    break;
                case "phone":
                    subscriberOriginal.Phone = value;
                    break;
                case "fax":
                    subscriberOriginal.Fax = value;
                    break;
                case "email":
                    subscriberOriginal.Email = value;
                    break;
                case "pubcategoryid":
                    subscriberOriginal.CategoryID = ParseInt(value);
                    break;
                case "pubtransactionid":
                    subscriberOriginal.TransactionID = ParseInt(value);
                    break;
                case "pubtransactiondate":
                    subscriberOriginal.TransactionDate = ParseNullableDate(value);
                    break;
                case "qualificationdate":
                    subscriberOriginal.QDate = ParseNullableDate(value);
                    break;
                case "pubqsourceid":
                    subscriberOriginal.QSourceID = ParseInt(value);
                    break;
                case "regcode":
                    subscriberOriginal.RegCode = value;
                    break;
                case "verify":
                    subscriberOriginal.Verified = value;
                    break;
                case "subscribersourcecode":
                    subscriberOriginal.SubSrc = value;
                    break;
                case "origssrc":
                    subscriberOriginal.OrigsSrc = value;
                    break;
                case "par3cid":
                    subscriberOriginal.Par3C = value;
                    break;
                case "mailpermission":
                    subscriberOriginal.MailPermission = ParseNullableBool(value);
                    break;
                case "faxpermission":
                    subscriberOriginal.FaxPermission = ParseNullableBool(value);
                    break;
                case "phonepermission":
                    subscriberOriginal.PhonePermission = ParseNullableBool(value);
                    break;
                case "otherproductspermission":
                    subscriberOriginal.OtherProductsPermission = ParseNullableBool(value);
                    break;
                case "thirdpartypermission":
                    subscriberOriginal.ThirdPartyPermission = ParseNullableBool(value);
                    break;
                case "emailrenewpermission":
                    subscriberOriginal.EmailRenewPermission = ParseNullableBool(value);
                    break;
                case "textpermission":
                    subscriberOriginal.TextPermission = ParseNullableBool(value);
                    break;
                case "source":
                    subscriberOriginal.Source = value;
                    break;
                case "priority":
                    subscriberOriginal.Priority = value;
                    break;
                case "sic":
                    subscriberOriginal.Sic = value;
                    break;
                case "siccode":
                    subscriberOriginal.SicCode = value;
                    break;
                case "gender":
                    subscriberOriginal.Gender = value;
                    break;
                case "address3":
                    subscriberOriginal.Address3 = value;
                    break;
                case "home_work_address":
                    subscriberOriginal.Home_Work_Address = value;
                    break;
                case "demo7":
                    subscriberOriginal.Demo7 = value;
                    break;
                case "mobile":
                    subscriberOriginal.Mobile = value;
                    break;
                case "latitude":
                    subscriberOriginal.Latitude = ParseDecimal(value);
                    break;
                case "longitude":
                    subscriberOriginal.Longitude = ParseDecimal(value);
                    break;
                case "islatlonvalid":
                    if (subscriberOriginal is ITransformedSubscriber)
                    {
                        ((ITransformedSubscriber)subscriberOriginal).IsLatLonValid = ParseBool(value);
                    }
                    break;
                case "latlonmsg":
                    if (subscriberOriginal is ITransformedSubscriber)
                    {
                        ((ITransformedSubscriber)subscriberOriginal).LatLonMsg = value;
                    }
                    break;
                case "score":
                    if (subscriberOriginal is IOriginalSubscriber)
                    {
                        ((IOriginalSubscriber)subscriberOriginal).Score = ParseInt(value);
                    }
                    break;
                case "emailstatusid":
                    if (subscriberOriginal is IOriginalSubscriber)
                    {
                        ((IOriginalSubscriber)subscriberOriginal).EmailStatusID = ParseInt(value);
                    }

                    if (subscriberOriginal is ITransformedSubscriber)
                    {
                        ((ITransformedSubscriber)subscriberOriginal).EmailStatusID = ParseInt(value);
                    }
                    break;
                case "importrownumber":
                    subscriberOriginal.ImportRowNumber = ParseInt(value);
                    break;
                case "isactive":
                    subscriberOriginal.IsActive = ParseBool(value);
                    break;
                case "processcode":
                    subscriberOriginal.ProcessCode = value;
                    break;
                case "externalkeyid":
                    subscriberOriginal.ExternalKeyId = ParseInt(value);
                    break;
                case "accountnumber":
                    subscriberOriginal.AccountNumber = value;
                    break;
                case "emailid":
                    subscriberOriginal.EmailID = ParseInt(value);
                    break;
                case "copies":
                    subscriberOriginal.Copies = ParseInt(value);
                    break;
                case "graceissues":
                    subscriberOriginal.GraceIssues = ParseInt(value);
                    break;
                case "iscomp":
                    subscriberOriginal.IsComp = ParseBool(value);
                    break;
                case "ispaid":
                    subscriberOriginal.IsPaid = ParseBool(value);
                    break;
                case "issubscribed":
                    subscriberOriginal.IsSubscribed = ParseBool(value);
                    break;
                case "occupation":
                    subscriberOriginal.Occupation = value;
                    break;
                case "subscriptionstatusid":
                    subscriberOriginal.SubscriptionStatusID = ParseInt(value);
                    break;
                case "subsrcid":
                    subscriberOriginal.SubsrcID = ParseInt(value);
                    break;
                case "website":
                    subscriberOriginal.Website = value;
                    break;
            }
        }

        public static void SetDemographicsResponseFields(string fieldName, IOriginalSubscriber subscriberOriginal, string value)
        {
            var demoName = fieldName.ToUpper().Replace(DemographicsOtherToken, string.Empty);
            foreach (var sdo in subscriberOriginal.DemographicOriginalList.Where(x => fieldName.Equals(demoName, StringComparison.CurrentCultureIgnoreCase)))
            {
                sdo.ResponseOther = value;
            }
        }

        public static void SetDemographicsResponseFields(string fieldName, ITransformedSubscriber subscriberOriginal, string value)
        {
            var demoName = fieldName.ToUpper().Replace(DemographicsOtherToken, string.Empty);
            foreach (var sdo in subscriberOriginal.DemographicTransformedList.Where(x => fieldName.Equals(demoName, StringComparison.CurrentCultureIgnoreCase)))
            {
                sdo.ResponseOther = value;
            }
        }

        public static IEnumerable<string> SplitDataStringByMapping(
            IEnumerable<TransformSplit> allTransformSplit,
            IEnumerable<TransformationFieldMap> splitTranFieldMappings,
            IEnumerable<Transformation> splitTrans,
            IFieldMapping map,
            string value)
        {
            var tid = splitTranFieldMappings.First(y => y.FieldMappingID == map.FieldMappingID)
                .TransformationID;
            var transformation = splitTrans.First(x => x.TransformationID == tid);
            var tsx = allTransformSplit.First(x => x.TransformationID == transformation.TransformationID);
            var delimiter = Enums.GetDelimiterSymbol(tsx.Delimiter);
            return delimiter.HasValue 
                ? value.Trim().Split(delimiter.Value).ToList() 
                : Enumerable.Empty<string>(); 
        }        

        private static bool ParseBool(string value)
        {
            var result = false;
            if (value != null)
            {
                bool.TryParse(value, out result);
            }

            return result;
        }

        private static decimal ParseDecimal(string value)
        {
            decimal result = 0;
            if (value != null)
            {
                decimal.TryParse(value, out result);
            }

            return result;
        }

        private static bool? ParseNullableBool(string value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value) &&
                (value.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase) ||
                 value.Equals(bool.FalseString, StringComparison.CurrentCultureIgnoreCase)))
            {
                bool result;
                bool.TryParse(value, out result);
                return result;
            }

            return null;
        }

        private static int ParseInt(string value)
        {
            int result = 0;
            if (value != null)
            {
                int.TryParse(value, out result);
            }

            return result;
        }

        private static DateTime ParseNullableDate(string value)
        {
            var result = MinDate;

            if (value != null)
            {
                if(!DateTime.TryParseExact(value, "yyyyMMdd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    DateTime.TryParse(value, out result);
                }
            }

            if (result == DateTime.MinValue ||
                result > MaxDate ||
                result < MinDate)
            {
                result = MinDate;
            }

            return result;
        }
    }
}
