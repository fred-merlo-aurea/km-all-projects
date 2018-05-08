using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Core_AMS.Utilities;
using FrameworkUAD.Entity.Helpers;
using FrameworkUAS.Entity;
using KMPlatform.Entity;

namespace FrameworkUAD.BusinessLogic.Transformations
{
    public abstract class FileValidatorBase
    {
        private const string ErrorMessageAddressIsNotPresent = "Address is not present ";
        private const string ErrorMessagePhoneIsNotPresent = "Phone is not present ";
        private const string ErrorMessageCompanyIsNotPresent = "Company is not present ";
        private const string ErrorMessageNameOrTitleIsNotPresent = "Name or Title is not present for the profile";

        protected SourceFile sourceFile;
        protected string processCode;
        protected ServiceFeature serviceFeature;
        protected List<string> ErrorMessages;
        protected int standarTypeID;
        protected int demoTypeID;
        protected int ignoredTypeID;
        protected int demoRespOtherTypeID;
        protected int kmTransformTypeId;
        protected Object.ImportFile dataIV;

        protected Entity.SubscriberTransformed CreateTransformedSubscriber(
            List<TransformSplit> allTransformSplit, 
            StringDictionary myRow,
            int pubCodeId,
            int originalRowNumber, 
            int transformedRowNumber, 
            Dictionary<int, Guid> subOriginalDictionary, 
            List<AdHocDimensionGroup> ahdGroups,
            List<TransformationFieldMap> splitTranFieldMappings, 
            List<Transformation> splitTrans)
        {
            var mapping = sourceFile.FieldMappings
                .Where(x => !x.MAFField.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            var soRecordIdentifier = Guid.Empty;
            if (subOriginalDictionary.ContainsKey(originalRowNumber))
            {
                soRecordIdentifier = subOriginalDictionary[originalRowNumber];
            }

            var subscriber =
                new Entity.SubscriberTransformed(sourceFile.SourceFileID, soRecordIdentifier, processCode)
                {
                    ImportRowNumber = transformedRowNumber,
                    OriginalImportRow = originalRowNumber
                };

            // Standard/Demographic
            foreach (var map in mapping)
            {
                try
                {
                    var value = myRow[map.IncomingField] ?? string.Empty;
                    if (map.FieldMappingTypeID == standarTypeID)
                    {
                        ValidatorHelper.SetProfileFieldValueByFieldMapping(map, subscriber, value);
                    }
                    else if (map.FieldMappingTypeID == demoTypeID && !string.IsNullOrWhiteSpace(value))
                    {
                        if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) ==
                            KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split &&
                            splitTranFieldMappings.Select(x => x.FieldMappingID).Contains(map.FieldMappingID))
                        {
                            foreach (var data in ValidatorHelper.SplitDataStringByMapping(
                                                            allTransformSplit,
                                                            splitTranFieldMappings,
                                                            splitTrans,
                                                            map,
                                                            value)
                                .Distinct().Where(s => !string.IsNullOrWhiteSpace(s)))
                            {
                                ValidatorHelper.ExtendWithDemographicsItem(
                                    ahdGroups,
                                    subscriber,
                                    map.DemographicUpdateCodeId,
                                    map.MAFField,
                                    pubCodeId,
                                    data.Trim());
                            }
                        }
                        else
                        {
                            ValidatorHelper.ExtendWithDemographicsItem(
                                ahdGroups,
                                subscriber,
                                map.DemographicUpdateCodeId,
                                map.MAFField,
                                pubCodeId,
                                value);
                        }
                    }
                    else if (map.FieldMappingTypeID == demoRespOtherTypeID && 
                             !string.IsNullOrWhiteSpace(value) &&
                             KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) ==
                             KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split)
                         {
                             if (splitTranFieldMappings.Select(x => x.FieldMappingID).Contains(map.FieldMappingID))
                             {
                                 var splitData = ValidatorHelper.SplitDataStringByMapping(
                                     allTransformSplit, 
                                     splitTranFieldMappings,
                                     splitTrans, 
                                     map, 
                                     value);
                                 foreach (var s in splitData.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct())
                                 {
                                     ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriber,
                                         s);
                                 }
                             }
                             else
                             {
                                 ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriber,
                                     value);
                             }
                         }

                    if (map.FieldMultiMappings.Count > 0)
                    {
                        foreach (var mmap in map.FieldMultiMappings)
                        {
                            if (mmap.FieldMappingTypeID == standarTypeID)
                            {
                                ValidatorHelper.SetProfileFieldValueByFieldMapping(mmap, subscriber, value);
                            }
                            else if ((mmap.FieldMappingTypeID == demoTypeID) && !string.IsNullOrEmpty(value))
                            {
                                ValidatorHelper.ExtendWithDemographicsItem(
                                    ahdGroups,
                                    subscriber,
                                    map.DemographicUpdateCodeId,
                                    mmap.MAFField,
                                    pubCodeId,
                                    value);
                            }
                            else if (map.FieldMappingTypeID == demoRespOtherTypeID && !string.IsNullOrWhiteSpace(value))
                            {
                                ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriber, value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = StringFunctions.FormatException(ex);
                    ValidationError(msg);
                    ErrorMessages.Add(msg);
                }
            }
            return subscriber;
        }

        protected Entity.SubscriberOriginal CreateSubscriber(StringDictionary myRow, int pubCodeId, List<AdHocDimensionGroup> ahdGroups)
        {
            var rowNumber = 0;
            int.TryParse(myRow["OriginalImportRow"], out rowNumber);

            var subscriberOriginal = new Entity.SubscriberOriginal(sourceFile.SourceFileID, rowNumber, processCode);

            // Standard/Demographic
            foreach (var map in sourceFile.FieldMappings
                                        .Where(x => !x.MAFField.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase)))
            {
                try
                {
                    var value = myRow[map.IncomingField] ?? string.Empty;
                    if (map.FieldMappingTypeID == standarTypeID)
                    {
                        ValidatorHelper.SetProfileFieldValueByFieldMapping(map, subscriberOriginal, value);
                    }
                    else if ((map.FieldMappingTypeID == demoTypeID) &&  !string.IsNullOrWhiteSpace(value))
                    {
                        ValidatorHelper.ExtendWithDemographicsItem(
                            ahdGroups,
                            subscriberOriginal,
                            map.DemographicUpdateCodeId,
                            map.MAFField,
                            pubCodeId,
                            value);
                    }
                    else if ((map.FieldMappingTypeID == demoRespOtherTypeID) && !string.IsNullOrWhiteSpace(value))
                    {
                        ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriberOriginal, value);
                    }

                    if (map.FieldMultiMappings.Count > 0)
                    {
                        foreach (var mmap in map.FieldMultiMappings)
                        {
                            if (mmap.FieldMappingTypeID == standarTypeID)
                            {
                                ValidatorHelper.SetProfileFieldValueByFieldMapping(mmap, subscriberOriginal, value);
                            }
                            else if ((mmap.FieldMappingTypeID == demoTypeID) && !string.IsNullOrWhiteSpace(value))
                            {
                                ValidatorHelper.ExtendWithDemographicsItem(
                                    ahdGroups,
                                    subscriberOriginal,
                                    map.DemographicUpdateCodeId,
                                    mmap.MAFField,
                                    pubCodeId,
                                    value);
                            }
                            else if ((mmap.FieldMappingTypeID == demoRespOtherTypeID) && !string.IsNullOrWhiteSpace(value))
                            {
                                ValidatorHelper.SetDemographicsResponseFields(mmap.MAFField, subscriberOriginal, value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = StringFunctions.FormatException(ex);
                    ValidationError(msg);
                    ErrorMessages.Add(msg);
                }
            }

            return subscriberOriginal;
        }

        protected IEnumerable<Entity.SubscriberTransformed> HasQualifiedProfile(IEnumerable<Entity.SubscriberTransformed> listToCheck)
        {
            var qualified = new List<Entity.SubscriberTransformed>();
            foreach (var subscriberTransformed in listToCheck)
            {
                var isQualified = true;
                var hasName = HasQualifiedName(subscriberTransformed);
                var hasAddress = HasQualifiedAddress(subscriberTransformed);
                var hasPhone = HasQualifiedPhone(subscriberTransformed);
                var hasCompany = HasQualifiedCompany(subscriberTransformed);
                var hasTitle = HasQualifiedTitle(subscriberTransformed);
                var row = dataIV.DataTransformed[subscriberTransformed.ImportRowNumber];
                var rowIndex = subscriberTransformed.ImportRowNumber;
                var stringBuilder = new StringBuilder();

                if (hasName)
                {
                    if (!(hasAddress || hasPhone || hasCompany))
                    {
                        isQualified = false;
                        stringBuilder.Append(ErrorMessageAddressIsNotPresent);
                        stringBuilder.Append(ErrorMessagePhoneIsNotPresent);
                        stringBuilder.Append(ErrorMessageCompanyIsNotPresent);
                    }
                }
                else if (hasTitle && hasCompany)
                {
                    if (!hasAddress)
                    {
                        isQualified = false;
                        stringBuilder.Append($"{ErrorMessageAddressIsNotPresent}");
                    }
                }
                else
                {
                    isQualified = false;
                    stringBuilder.Append($"{ErrorMessageNameOrTitleIsNotPresent}");
                }

                if (isQualified)
                {
                    qualified.Add(subscriberTransformed);
                }
                else
                {
                    var errorMessage = $"{stringBuilder}@ Row {rowIndex}";

                    ErrorMessages.Add(errorMessage);
                    ValidationError(errorMessage, rowIndex, row);
                    ErrorMessages.Add($"Row missing Subscriber Profile and Email @ Row {rowIndex}, you must have one or the other");
                }
            }

            return qualified;
        }

        protected bool HasQualifiedName(Entity.SubscriberTransformed subscriberTransformed)
        {
            return !string.IsNullOrWhiteSpace(subscriberTransformed.FName) 
                   || !string.IsNullOrWhiteSpace(subscriberTransformed.LName);
        }

        protected bool HasQualifiedAddress(Entity.SubscriberTransformed subscriberTransformed)
        {
            if (string.IsNullOrWhiteSpace(subscriberTransformed.Address))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(subscriberTransformed.Zip) 
                && string.IsNullOrWhiteSpace(subscriberTransformed.City))
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(subscriberTransformed.State) 
                   || !string.IsNullOrWhiteSpace(subscriberTransformed.Country);
        }

        protected bool HasQualifiedPhone(Entity.SubscriberTransformed subscriberTransformed)
        {
            return !(string.IsNullOrWhiteSpace(subscriberTransformed.Phone) 
                     && string.IsNullOrWhiteSpace(subscriberTransformed.Mobile) 
                     && string.IsNullOrWhiteSpace(subscriberTransformed.Fax));
        }

        protected bool HasQualifiedCompany(Entity.SubscriberTransformed subscriberTransformed)
        {
            return !string.IsNullOrWhiteSpace(subscriberTransformed.Company);
        }

        protected bool HasQualifiedTitle(Entity.SubscriberTransformed subscriberTransformed)
        {
            return !string.IsNullOrWhiteSpace(subscriberTransformed.Title);
        }

        protected abstract bool ValidationError(string errorMsg, int rowNumber = 0, StringDictionary rowData = null);        
    }
}
