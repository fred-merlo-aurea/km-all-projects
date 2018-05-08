using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FrameworkSubGen.Entity;
using FrameworkUAD.Entity;
using Enums = FrameworkSubGen.Entity.Enums;

namespace AMS_Operations
{
    internal class SubGenSynchronization
    {
        private const string KMToSubGenSyncActiveKey = "KMtoSubGenActive";
        private const string Whitespace = " ";

        public void HourlySyncKMToSubGen(Account account, Enums.Client sgClient)
        {
            bool kmtoSubGenActive;
            bool.TryParse(ConfigurationManager.AppSettings[KMToSubGenSyncActiveKey], out kmtoSubGenActive);
            if (!kmtoSubGenActive)
            {
                return;
            }

            //CustomField - ValueOption (Codesheet)
            var client = new KMPlatform.BusinessLogic.Client().Select(account.KMClientId);
            var customFieldWorker = new FrameworkSubGen.BusinessLogic.CustomField();
            var cfList = customFieldWorker.Select(account.account_id);
            var newCfList = new List<CustomField>();
            var pList = new FrameworkUAD.BusinessLogic.Product().Select(client.ClientConnections, true)
                .Where(x => x.HasPaidRecords)
                .ToList();
            foreach (var product in pList)
            {
                foreach (var responseGroup in product.ResponseGroups)
                {
                    if (!cfList.Exists(cf => cf.name.Equals(GetResponseGroupKey(product, responseGroup), StringComparison.CurrentCultureIgnoreCase)))
                    {
                        CreateCodeSheetCustomFields(account, sgClient, product, responseGroup, newCfList, customFieldWorker);
                    }
                    else
                    {
                        UpdateCodeSheetCustomFields(account, sgClient, cfList, product, responseGroup);
                    }
                }
            }
        }

        public void HourlySyncSubGenToKM(Enums.Client sgClient, Account account)
        {
            SyncBundles(sgClient, account);
            SyncCustomFields(sgClient, account);
            SyncPublications(sgClient, account);
            SyncUsers(sgClient, account);
        }

        private static void UpdateCodeSheetCustomFields(Account account,
            Enums.Client sgClient,
            List<CustomField> cfList,
            Product product,
            ResponseGroup responseGroup)
        {
            var customFieldToUpdate = cfList.FirstOrDefault(cf => cf.name.Equals(GetResponseGroupKey(product, responseGroup), StringComparison.CurrentCultureIgnoreCase));
            if (customFieldToUpdate == null)
            {
                return;
            }

            var counter = 1;
            foreach (var codeSheet in product.CodeSheets.Where(x => x.ResponseGroupID == responseGroup.ResponseGroupID))
            {
                var valueOption = new ValueOption();
                if (customFieldToUpdate.value_options.Exists(x => x.field_id == customFieldToUpdate.field_id && x.KMCodeSheetID == codeSheet.CodeSheetID))
                {
                    valueOption = customFieldToUpdate.value_options.First(x => x.field_id == customFieldToUpdate.field_id && x.KMCodeSheetID == codeSheet.CodeSheetID);
                    if (!valueOption.display_as.Equals(codeSheet.ResponseDesc, StringComparison.CurrentCultureIgnoreCase) ||
                        !valueOption.value.Equals(codeSheet.ResponseValue, StringComparison.CurrentCultureIgnoreCase))
                    {
                        valueOption.display_as = codeSheet.ResponseDesc;
                        valueOption.order = counter;
                        valueOption.value = codeSheet.ResponseValue;
                        valueOption.KMCodeSheetID = codeSheet.CodeSheetID;
                        valueOption.KMProductCode = product.PubCode;
                        valueOption.KMProductId = product.PubID;
                        new FrameworkSubGen.BusinessLogic.API.CustomField().UpdateFieldOption(sgClient, valueOption);
                    }
                }
                else
                {
                    if (!customFieldToUpdate.value_options.Exists(x =>
                        x.field_id == customFieldToUpdate.field_id &&
                        x.display_as.Replace(Whitespace, string.Empty).Equals(codeSheet.ResponseDesc.Replace(Whitespace, string.Empty))))
                    {
                        //new
                        valueOption.account_id = account.account_id;
                        valueOption.active = true;
                        valueOption.display_as = codeSheet.ResponseDesc;
                        valueOption.order = counter;
                        valueOption.value = codeSheet.ResponseValue;
                        valueOption.field_id = customFieldToUpdate.field_id;
                        valueOption.KMCodeSheetID = codeSheet.CodeSheetID;
                        valueOption.KMProductCode = product.PubCode;
                        valueOption.KMProductId = product.PubID;
                        valueOption.option_id = new FrameworkSubGen.BusinessLogic.API.CustomField().CreateFieldOption(sgClient, valueOption.field_id, valueOption);
                        var voList = new List<ValueOption> { valueOption };
                        new FrameworkSubGen.BusinessLogic.ValueOption().SaveBulkXml(voList);
                    }
                }

                counter++;
            }
        }

        private static string GetResponseGroupKey(Product product, ResponseGroup responseGroup)
        {
            return $"{product.PubCode} - {responseGroup.ResponseGroupName}";
        }

        private static void CreateCodeSheetCustomFields(Account account,
            Enums.Client sgClient,
            Product product,
            ResponseGroup responseGroup,
            List<CustomField> newCfList,
            FrameworkSubGen.BusinessLogic.CustomField customFieldWorker)
        {
            var newCustomField = new CustomField
            {
                account_id = account.account_id,
                allow_other = product.CodeSheets.Exists(x =>
                    x.ResponseGroupID == responseGroup.ResponseGroupID && x.IsOther == true),
                display_as = GetResponseGroupKey(product, responseGroup),
                name = GetResponseGroupKey(product, responseGroup),
                value_options = new List<ValueOption>(),
                KMProductCode = product.PubCode,
                KMProductId = product.PubID,
                KMResponseGroupID = responseGroup.ResponseGroupID
            };

            CreateValueOptions(account, product, responseGroup, newCustomField);

            if (responseGroup.IsMultipleValue == true)
            {
                newCustomField.type = Enums.HtmlFieldType.checkbox;
            }
            else if (responseGroup.IsMultipleValue == false)
            {
                newCustomField.type = Enums.HtmlFieldType.radio;
            }

            var acfw = new FrameworkSubGen.BusinessLogic.API.CustomField();
            acfw.Create(sgClient, ref newCustomField);
            newCfList.Add(newCustomField);

            //if allow other = true need to create extra other demo
            if (newCustomField.allow_other)
            {
                var otherCf = new CustomField
                {
                    account_id = account.account_id,
                    allow_other = product.CodeSheets.Exists(x =>
                        x.ResponseGroupID == responseGroup.ResponseGroupID && x.IsOther == true),
                    display_as = $"{GetResponseGroupKey(product, responseGroup)} - OTHER",
                    name = $"{GetResponseGroupKey(product, responseGroup)} - OTHER",
                    value_options = new List<ValueOption>(),
                    type = Enums.HtmlFieldType.text,
                    KMProductCode = product.PubCode,
                    KMProductId = product.PubID,
                    KMResponseGroupID = responseGroup.ResponseGroupID
                };

                acfw.Create(sgClient, ref otherCf);
                newCfList.Add(otherCf);
            }

            customFieldWorker.SaveBulkXml(newCfList, account.account_id);
        }

        private static void CreateValueOptions(Account account,
            Product product,
            ResponseGroup responseGroup,
            CustomField newCustomField)
        {
            var counter = 1;
            foreach (var valueOption in product.CodeSheets.Where(x => x.ResponseGroupID == responseGroup.ResponseGroupID)
                .Select(cs => new ValueOption
                {
                    account_id = account.account_id,
                    active = true,
                    display_as = cs.ResponseDesc,
                    order = counter,
                    value = cs.ResponseValue,
                    KMCodeSheetID = cs.CodeSheetID,
                    KMProductCode = product.PubCode,
                    KMProductId = product.PubID
                }))
            {
                newCustomField.value_options.Add(valueOption);
                counter++;
            }
        }


        private void SyncUsers(Enums.Client sgClient, Account account)
        {
            try
            {
                var uWorker = new FrameworkSubGen.BusinessLogic.API.User();
                var uList = uWorker.GetUsers(sgClient, false);
                var uw = new FrameworkSubGen.BusinessLogic.User();
                uw.SaveBulkXml(uList, account.account_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "User");
            }
        }

        private void SyncPublications(Enums.Client sgClient, Account account)
        {
            try
            {
                var pubWorker = new FrameworkSubGen.BusinessLogic.API.Publication();
                var pubList = pubWorker.GetPublications(sgClient);
                var pwk = new FrameworkSubGen.BusinessLogic.Publication();
                pwk.SaveBulkXml(pubList, account.account_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Publication");
            }
        }

        private void SyncCustomFields(Enums.Client sgClient, Account account)
        {
            try
            {
                //CustomField - ValueOption
                var cfWorker = new FrameworkSubGen.BusinessLogic.API.CustomField();
                var cfListU = cfWorker.GetCustomFields(sgClient);
                var cfwU = new FrameworkSubGen.BusinessLogic.CustomField();
                cfwU.SaveBulkXml(cfListU, account.account_id);

                //not going to sync to UAD - business rule is to create in AMS then create in SubGen
                //since rule is from KM to SubGen we can assume that when going from SubGen to our SubGenData database that the CF/VO will exist
                //in client UAD ResponseGroup/CodeSheet

                var cfList = cfwU.Select(account.account_id);
                var cWrk = new KMPlatform.BusinessLogic.Client();
                var c = cWrk.Select(account.KMClientId);
                var oWrk = new FrameworkUAD.BusinessLogic.Objects();
                var dList = oWrk.GetDimensions(c.ClientConnections);
                foreach (var cf in cfList)
                {
                    if (!dList.Exists(x =>
                        x.SubGenResponseGroupName.Equals(cf.name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        continue;
                    }

                    var d = dList.First(x =>
                        x.SubGenResponseGroupName.Equals(cf.name, StringComparison.CurrentCultureIgnoreCase));
                    cf.KMProductCode = d.ProductCode;
                    cf.KMProductId = d.ProductId;
                    cf.KMResponseGroupID = d.ResponseGroupID;

                    foreach (var vo in cf.value_options)
                    {
                        foreach (var dim in dList.Where(x =>
                            x.ResponseGroupID == d.ResponseGroupID &&
                            x.Responsevalue.Equals(vo.value, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            vo.KMCodeSheetID = dim.CodeSheetID;
                            vo.KMProductCode = dim.ProductCode;
                            vo.KMProductId = dim.ProductId;
                            break;
                        }
                    }
                }

                cfwU.SaveBulkXml(cfList, account.account_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "CustomField");
            }
        }

        private void SyncBundles(Enums.Client sgClient, Account account)
        {
            try
            {
                var bunList = new FrameworkSubGen.BusinessLogic.API.Bundle().GetBundles(sgClient, true, false);
                new FrameworkSubGen.BusinessLogic.Bundle().SaveBulkXml(bunList, account.account_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Bundle");
            }
        }

        public void LogError(Exception ex, string method)
        {
            LogHelper.LogError(ex, method, string.Empty);
        }

    }
}
