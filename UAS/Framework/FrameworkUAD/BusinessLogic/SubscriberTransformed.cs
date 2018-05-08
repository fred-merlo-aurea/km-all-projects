using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using KM.Common;
using KMPlatform.Object;
using ServiceStack.Text;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberTransformed
    {
        private const int SqlBatchSize = 250000;

        #region selects
        public List<Entity.SubscriberTransformed> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.Select(client).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.Select(processCode, client).ToList();
            return x;
        }

        public Entity.SubscriberTransformed SelectTopOne(string processCode, KMPlatform.Object.ClientConnections client)
        {
            Entity.SubscriberTransformed x = null;
            x = DataAccess.SubscriberTransformed.SelectTopOne(processCode, client);
            return x;
        }
        public List<Entity.SubscriberTransformed> Select(KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.Select(client, sourceFileID).ToList();
            return x;
        }
        public Object.DimensionErrorCount SelectDimensionCount(string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubscriberTransformed.SelectDimensionCount(processCode, client);
        }        
        public List<FrameworkUAD.Object.ImportRowNumber> SelectImportRowNumbers(KMPlatform.Object.ClientConnections client, string ProcessCode)
        {
            List<FrameworkUAD.Object.ImportRowNumber> x = null;
            x = DataAccess.SubscriberTransformed.SelectImportRowNumbers(client, ProcessCode).ToList();
            return x;
        }
        #endregion

        #region Address validation and geocoding selects
        public List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, bool isLatLonValid)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectByAddressValidation(client, sourceFileID, isLatLonValid).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, int sourceFileID, bool isLatLonValid)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectByAddressValidation(client, processCode, sourceFileID, isLatLonValid).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectByAddressValidation(client, processCode, isLatLonValid).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectByAddressValidation(client, isLatLonValid).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }
        public List<Entity.SubscriberTransformed> SelectForGeoCoding(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectForGeoCoding(client).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> SelectForGeoCoding(KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.SelectForGeoCoding(client, sourceFileID).ToList();
            return x;
        }
        public List<Entity.SubscriberTransformed> AddressValidation_Paging(int currentPage, int pageSize, string processCode, KMPlatform.Object.ClientConnections client, bool isLatLonValid = false, int sourceFileID = 0)
        {
            List<Entity.SubscriberTransformed> x = null;
            x = DataAccess.SubscriberTransformed.AddressValidation_Paging(currentPage, pageSize, processCode, client, isLatLonValid, sourceFileID).ToList();
            return x;
        }
        #endregion

        #region Geocode counts
        public int CountAddressValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, bool isLatLonValid)
        {
            int count = 0;
            count = DataAccess.SubscriberTransformed.CountAddressValidation(client, sourceFileID, isLatLonValid);
            return count;
        }
        public int CountAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid)
        {
            int count = 0;
           count = DataAccess.SubscriberTransformed.CountAddressValidation(client, processCode, isLatLonValid);
            return count;
        }
        public int CountAddressValidation(KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            int count = 0;
            count = DataAccess.SubscriberTransformed.CountAddressValidation(client, isLatLonValid);
            return count;
        }
        /// <summary>
        /// return INVALID count
        /// </summary>
        /// <param name="client"></param>
        /// <returns>int</returns>
        public int CountForGeoCoding(KMPlatform.Object.ClientConnections client)
        {
            int count = 0;
            count = DataAccess.SubscriberTransformed.CountForGeoCoding(client);
            return count;
        }
        /// <summary>
        /// reutrn INVALID count
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sourceFileID"></param>
        /// <returns>int </returns>
        public int CountForGeoCoding(KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            int count = 0;
            count = DataAccess.SubscriberTransformed.CountForGeoCoding(client, sourceFileID);
            return count;
        }
        #endregion

        #region distinct PubCode list
        public List<string> GetDistinctPubCodes(KMPlatform.Object.ClientConnections client, string processCode)
        {
            List<string> pubs = new List<string>();
            pubs = DataAccess.SubscriberTransformed.GetDistinctPubCodes(client, processCode);
            return pubs;
        }
        #endregion

        #region Save  / Update methods
        public int Save(Entity.SubscriberTransformed x, KMPlatform.Object.ClientConnections client)
        {
            FormatData(x);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SubscriberTransformedID = DataAccess.SubscriberTransformed.Save(x, client);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, x.ProcessCode));
                }
            }

            return x.SubscriberTransformedID;
        }
        public bool SaveBulkInsert(List<Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            // DisableIndexes(client);
            foreach (Entity.SubscriberTransformed x in list)
                FormatData(x);

            bool done = false;
            int BatchSize = 250;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            string processCode = list.FirstOrDefault().ProcessCode;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.SubscriberTransformed x in list)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.SubscriberTransformed>(x));
                sbXML.AppendLine(xmlObject);

                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.SubscriberTransformed.SaveBulkInsert("<XML>" + sbXML.ToString() + "</XML>", client);
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, processCode));
                        }
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            //EnableIndexes(client);
            return done;
        }
        //public void DisableTableIndexes(KMPlatform.Object.ClientConnections client, int sourceFileId, string processCode)
        //{
        //    try
        //    {
        //        DataAccess.SubscriberTransformed.DisableIndexes(client);
        //        DataAccess.SubscriberDemographicTransformed.DisableIndexes(client);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, message, processCode));
        //    }
        //}
        //public void EnableTableIndexes(KMPlatform.Object.ClientConnections client, int sourceFileId, string processCode)
        //{
        //    try
        //    {
        //        DataAccess.SubscriberTransformed.EnableIndexes(client);
        //        DataAccess.SubscriberDemographicTransformed.EnableIndexes(client);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, message, processCode));
        //    }
        //}
        public bool SaveBulkSqlInsert(List<Entity.SubscriberTransformed> list, ClientConnections client, bool isDataCompare)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(client, nameof(client));

            foreach (var item in list)
            {
                FormatData(item);
            }

            if (!list.Any())
            {
                return true;
            }

            var done = true;

            var sourceFileId = list.First().SourceFileID;
            var processCode = list.First().ProcessCode;

            var sourceList = list;
            var processedCount = 0;

            while (sourceList.Any())
            {
                var batchList = sourceList.Take(SqlBatchSize).ToList();
                sourceList = sourceList.Skip(SqlBatchSize).ToList();

                processedCount += batchList.Count;

                done = SqlDoBulkSave(batchList, client, processedCount, isDataCompare, sourceFileId, processCode);
            }

            return done;
        }

        private bool SqlDoBulkSave(List<Entity.SubscriberTransformed> batchList, ClientConnections client, int processedCount, bool isDataCompare, int sourceFileId, string processCode)
        {
            try
            {
                var done = DataAccess.SubscriberTransformed.SaveBulkSqlInsert(batchList, client);
                if (!done)
                {
                    return false;
                }

                var fileLog = new FrameworkUAS.BusinessLogic.FileLog();
                fileLog.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, $"Start Bulk Insert SubscriberDemographicTransformed : processed count = {processedCount}", processCode));

                foreach (var item in batchList)
                {
                    var sourceList = item.DemographicTransformedList.ToList();

                    while (sourceList.Any())
                    {
                        var demographicBatchList = sourceList.Take(SqlBatchSize).ToList();
                        sourceList = sourceList.Skip(SqlBatchSize).ToList();

                        DataAccess.SubscriberDemographicTransformed.SaveBulkSqlInsert(
                            demographicBatchList,
                            client,
                            isDataCompare,
                            sourceFileId,
                            processCode);
                    }
                }

                fileLog.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, $"End Bulk Insert SubscriberDemographicTransformed : processed count = {processedCount}", processCode));

                return true;
            }
            catch (Exception ex)
            {
                var message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                var fileLog = new FrameworkUAS.BusinessLogic.FileLog();
                fileLog.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, message, processCode));
                return false;
            }
        }

        public bool AddressUpdateBulkSql(List<Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            //DisableIndexes(client);
            foreach (Entity.SubscriberTransformed x in list)
                FormatData(x);
            bool done = true;
            int BatchSize = 2500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            string processCode = list.FirstOrDefault().ProcessCode;

            List<Entity.SubscriberTransformed> bulkUpdateList = new List<Entity.SubscriberTransformed>();
            foreach (Entity.SubscriberTransformed x in list)
            {
                counter++;
                processedCount++;
                done = false;
                bulkUpdateList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.SubscriberTransformed.AddressUpdateBulkSql(bulkUpdateList, client);

                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, processCode));
                        }
                    }
                    counter = 0;
                    bulkUpdateList = new List<Entity.SubscriberTransformed>();
                }
            }

            //EnableIndexes(client);

            return done;
        }

        public void FormatData(Entity.SubscriberTransformed x)
        {
            try
            {
                //x = PopulateNull(x);

                //if (x.STRecordIdentifier == Guid.Empty)
                //    x.STRecordIdentifier = Guid.NewGuid();
                if (!(string.IsNullOrEmpty(x.Phone)))
                    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
                if (!(string.IsNullOrEmpty(x.Mobile)))
                    x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
                if (!(string.IsNullOrEmpty(x.Fax)))
                    x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);
                //if (x.Email != null && x.Email.Length <= 4)
                //    x.Email = string.Empty;

                #region truncate strings
                if (!(string.IsNullOrEmpty(x.PubCode)) && x.PubCode.Length > 100)
                    x.PubCode = x.PubCode.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.FName)) && x.FName.Length > 100)
                    x.FName = x.FName.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.LName)) && x.LName.Length > 100)
                    x.LName = x.LName.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.Title)) && x.Title.Length > 100)
                    x.Title = x.Title.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.Company)) && x.Company.Length > 100)
                    x.Company = x.Company.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.Address)) && x.Address.Length > 255)
                    x.Address = x.Address.Substring(0, 255);
                if (!(string.IsNullOrEmpty(x.MailStop)) && x.MailStop.Length > 255)
                    x.MailStop = x.MailStop.Substring(0, 255);
                if (!(string.IsNullOrEmpty(x.City)) && x.City.Length > 50)
                    x.City = x.City.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.State)) && x.State.Length > 50)
                    x.State = x.State.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.Zip)) && x.Zip.Length > 50)
                    x.Zip = x.Zip.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.Plus4)) && x.Plus4.Length > 50)
                    x.Plus4 = x.Plus4.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.ForZip)) && x.ForZip.Length > 50)
                    x.ForZip = x.ForZip.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.County)) && x.County.Length > 100)
                    x.County = x.County.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.Country)) && x.Country.Length > 100)
                    x.Country = x.Country.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.Email)) && x.Email.Length > 100)
                    x.Email = x.Email.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.RegCode)) && x.RegCode.Length > 5)
                    x.RegCode = x.RegCode.Substring(0, 5);
                if (!(string.IsNullOrEmpty(x.Verified)) && x.Verified.Length > 100)
                    x.Verified = x.Verified.Substring(0, 100);
                if (!(string.IsNullOrEmpty(x.SubSrc)) && x.SubSrc.Length > 25)
                    x.SubSrc = x.SubSrc.Substring(0, 25);
                if (!(string.IsNullOrEmpty(x.OrigsSrc)) && x.OrigsSrc.Length > 25)
                    x.OrigsSrc = x.OrigsSrc.Substring(0, 25);
                if (!(string.IsNullOrEmpty(x.Par3C)) && x.Par3C.Length > 10)
                    x.Par3C = x.Par3C.Substring(0, 10);
                if (!(string.IsNullOrEmpty(x.Source)) && x.Source.Length > 50)
                    x.Source = x.Source.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.Priority)) && x.Priority.Length > 4)
                    x.Priority = x.Priority.Substring(0, 4);
                if (!(string.IsNullOrEmpty(x.Sic)) && x.Sic.Length > 8)
                    x.Sic = x.Sic.Substring(0, 8);
                if (!(string.IsNullOrEmpty(x.SicCode)) && x.SicCode.Length > 20)
                    x.SicCode = x.SicCode.Substring(0, 20);
                if (!(string.IsNullOrEmpty(x.Gender)) && x.Gender.Length > 1024)
                    x.Gender = x.Gender.Substring(0, 1024);
                //if (!(string.IsNullOrEmpty(x.IGrp_Rank)) && x.IGrp_Rank.Length > 2)
                //    x.IGrp_Rank = x.IGrp_Rank.Substring(0, 2);
                //if (!(string.IsNullOrEmpty(x.CGrp_Rank)) && x.CGrp_Rank.Length > 2)
                //    x.CGrp_Rank = x.CGrp_Rank.Substring(0, 2);
                if (!(string.IsNullOrEmpty(x.Address3)) && x.Address3.Length > 255)
                    x.Address3 = x.Address3.Substring(0, 255);
                if (!(string.IsNullOrEmpty(x.Home_Work_Address)) && x.Home_Work_Address.Length > 10)
                    x.Home_Work_Address = x.Home_Work_Address.Substring(0, 10);
                //if (!(string.IsNullOrEmpty(x.PubIDs)) && x.PubIDs.Length > 2000)
                //    x.PubIDs = x.PubIDs.Substring(0, 2000);
                if (!(string.IsNullOrEmpty(x.Demo7)) && x.Demo7.Length > 1)
                    x.Demo7 = x.Demo7.Substring(0, 1);
                if (!(string.IsNullOrEmpty(x.LatLonMsg)) && x.LatLonMsg.Length > 500)
                    x.LatLonMsg = x.LatLonMsg.Substring(0, 500);
                if (!(string.IsNullOrEmpty(x.AccountNumber)) && x.AccountNumber.Length > 50)
                    x.AccountNumber = x.AccountNumber.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.Occupation)) && x.Occupation.Length > 50)
                    x.Occupation = x.Occupation.Substring(0, 50);
                if (!(string.IsNullOrEmpty(x.Website)) && x.Website.Length > 255)
                    x.Website = x.Website.Substring(0, 255);
                if (!string.IsNullOrEmpty(x.SubGenRenewalCode) && x.SubGenRenewalCode.Length > 50)
                    x.SubGenRenewalCode = x.SubGenRenewalCode.Substring(0, 50);
                #endregion

                if (x.QDate == DateTime.Parse("0001-01-01T00:00:00") || x.QDate == DateTime.MinValue || x.QDate <= DateTime.Parse("1/1/1900"))
                    x.QDate = DateTime.Now;
                //if (x.StatusUpdatedDate == DateTime.Parse("0001-01-01T00:00:00") || x.StatusUpdatedDate == DateTime.MinValue || x.StatusUpdatedDate <= DateTime.Parse("1/1/1900"))
                //    x.StatusUpdatedDate = DateTime.Now;

                // This logic requested by Sunil - q.k 04072015
                //if (!string.IsNullOrEmpty(x.Address))
                //    x.IsMailable = true;
                //else
                //    x.IsMailable = false;

                Core_AMS.Utilities.AddressValidator av = new Core_AMS.Utilities.AddressValidator();
                x.Address = av.CleanAddress(x.Address);
            }
            catch (Exception ex)
            {
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "FormatData"));
            }
        }
        #endregion

        #region Jobs / Operations
        public bool StandardRollUpToMaster(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            bool done = false;

            bool mailPermissionOverRide = false;
            bool faxPermissionOverRide = false;
            bool phonePermissionOverRide = false;
            bool otherProductsPermissionOverRide = false;
            bool thirdPartyPermissionOverRide = false;
            bool emailRenewPermissionOverRide = false;
            bool textPermissionOverRide = false;

            FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            List<FrameworkUAS.Entity.FieldMapping> list = fmWorker.Select(sourceFileID);

            if (list.Exists(x => x.MAFField.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase)))
                mailPermissionOverRide = true;
            if (list.Exists(x => x.MAFField.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase)))
                faxPermissionOverRide = true;
            if (list.Exists(x => x.MAFField.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase)))
                phonePermissionOverRide = true;
            if (list.Exists(x => x.MAFField.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase)))
                otherProductsPermissionOverRide = true;
            if (list.Exists(x => x.MAFField.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase)))
                thirdPartyPermissionOverRide = true;
            if (list.Exists(x => x.MAFField.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase)))
                emailRenewPermissionOverRide = true;
            if (list.Exists(x => x.MAFField.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase)))
                textPermissionOverRide = true;

            done = DataAccess.SubscriberTransformed.StandardRollUpToMaster(client, sourceFileID, processCode, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);

            return done;
        }
        public bool StandardRollUpToMaster(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode,
            bool mailPermissionOverRide = false,bool faxPermissionOverRide = false,bool phonePermissionOverRide = false,
            bool otherProductsPermissionOverRide = false,bool thirdPartyPermissionOverRide = false,bool emailRenewPermissionOverRide = false,
            bool textPermissionOverRide = false,bool updateEmail = true, bool updatePhone = true, bool updateFax = true, bool updateMobile = true)
        {
            bool done = false;
            done = DataAccess.SubscriberTransformed.StandardRollUpToMaster(client, sourceFileID, processCode, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide, updateEmail,updatePhone,updateFax,updateMobile);

            return done;
        }
        public bool AddressValidateExisting(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            bool done = false;
            done = DataAccess.SubscriberTransformed.AddressValidExisting(client, sourceFileID, processCode);
            return done;
        }
        public bool DataMatching(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            bool done = false;
            done = DataAccess.SubscriberTransformed.DataMatching(client, sourceFileID, processCode);
            return done;
        }
        public bool SequenceDataMatching(KMPlatform.Object.ClientConnections client, string processCode)
        {
            bool done = false;
            done = DataAccess.SubscriberTransformed.SequenceDataMatching(client, processCode);
            return done;
        }
        public bool DataMatching_multiple(KMPlatform.Object.ClientConnections client, int sourceFileId, string processCode, string matchFields)
        {
            bool done = false;
            done = DataAccess.SubscriberTransformed.DataMatching_multiple(client, sourceFileId, processCode, matchFields);
            return done;
        }
        public bool DataMatching_single(KMPlatform.Object.ClientConnections client, string processCode, string matchField)
        {
            bool done = false;
            done = DataAccess.SubscriberTransformed.DataMatching_single(client, processCode, matchField);
            return done;
        }
        //public bool DisableIndexes(KMPlatform.Object.ClientConnections client)
        //{
        //    return DataAccess.SubscriberTransformed.DisableIndexes(client);
        //}
        //public bool EnableIndexes(KMPlatform.Object.ClientConnections client)
        //{
        //    return DataAccess.SubscriberTransformed.EnableIndexes(client);
        //}
        public bool RevertXmlFormattingAfterBulkInsert(string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubscriberTransformed.RevertXmlFormattingAfterBulkInsert(processCode, client);
        }
        #endregion
    }
}
