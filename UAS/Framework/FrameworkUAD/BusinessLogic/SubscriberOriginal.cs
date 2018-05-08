using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using Core_AMS.Utilities;
using KMPlatform.Object;
using ServiceStack.Text;
using BusinessLogicFileLog = FrameworkUAS.BusinessLogic.FileLog;
using EntitySubscriberOriginal = FrameworkUAD.Entity.SubscriberOriginal;
using EntityFileLog = FrameworkUAS.Entity.FileLog;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberOriginal
    {
        // Makes this static readonly to be able changed by unit tests.
        private static readonly int BulkSqlInsertSize = 250000;
        private const int BulkInsertSize = 250;
        private const int BulkUpdateSize = 500;
        private const int DefaultFileId = -99;
        private const int DefaultFileStatusTypeId = -99;
        private const string DefaultProcessCode = "SubscriberOriginal";

        public List<Entity.SubscriberOriginal> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberOriginal> x = null;
            x = DataAccess.SubscriberOriginal.Select(client).ToList();
            return x;
        }
        public List<Entity.SubscriberOriginal> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberOriginal> x = null;
            x = DataAccess.SubscriberOriginal.Select(processCode, client).ToList();
            return x;
        }
        public List<Entity.SubscriberOriginal> Select(int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberOriginal> x = null;
            x = DataAccess.SubscriberOriginal.Select(sourceFileID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberOriginal> Select(string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberOriginal> x = null;
            x = DataAccess.SubscriberOriginal.Select(processCode, sourceFileID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberOriginal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberOriginal> x = null;
            x = DataAccess.SubscriberOriginal.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }
        public int Save(Entity.SubscriberOriginal x, KMPlatform.Object.ClientConnections client)
        {
            FormatData(x);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SubscriberOriginalID = DataAccess.SubscriberOriginal.Save(x, client);
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

            return x.SubscriberOriginalID;
        }

        public bool SaveBulkUpdate(List<Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            return SaveBulkInsertOrUpdate(list, client, false);
        }

        public bool SaveBulkInsert(List<Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            return SaveBulkInsertOrUpdate(list, client, true);
        }

        private bool SaveBulkInsertOrUpdate(IEnumerable<EntitySubscriberOriginal> entities, ClientConnections client, bool insert)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var list = entities.ToList();
            if (!list.Any())
            {
                return false;
            }

            list.ForEach(FormatData);

            var done = false;
            var batchSize = insert ? BulkInsertSize : BulkUpdateSize;
            var total = list.Count;
            var counter = 0;
            var processedCount = 0;
            var checkCount = 1;
            var processCode = list.First().ProcessCode;
            var stringBuilder = new StringBuilder();

            foreach (var entity in list)
            {
                var message = $"Checking Original Subscriber: {checkCount} of {total}";
                StringFunctions.WriteLineRepeater(message, ConsoleColor.Green);

                var xmlObject = GetEntityXml(entity);
                stringBuilder.AppendLine(xmlObject);

                checkCount++;
                counter++;
                processedCount++;
                done = false;

                if (processedCount == total || counter == batchSize)
                {
                    var xml = $"<XML>{stringBuilder}</XML>";
                    done = ProcessSaveBulkInsertOrUpdate(xml, client, insert, processCode);
                    stringBuilder = new StringBuilder();
                    counter = 0;
                }
            }

            return done;
        }

        private bool ProcessSaveBulkInsertOrUpdate(string xml, ClientConnections client, bool insert, string processCode)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (insert)
                    {
                        DataAccess.SubscriberOriginal.SaveBulkInsert(xml, client);
                    }
                    else
                    {
                        DataAccess.SubscriberOriginal.SaveBulkUpdate(xml, client);
                    }

                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    var errorMessage = StringFunctions.FormatException(ex);
                    var fileLog = new BusinessLogicFileLog();
                    fileLog.Save(new EntityFileLog(DefaultFileId, DefaultFileStatusTypeId, errorMessage, processCode));
                }
            }

            return false;
        }

        private static string GetEntityXml(EntitySubscriberOriginal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString(entity));
        }

        public bool SaveBulkSqlInsert(List<EntitySubscriberOriginal> list, ClientConnections client)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            list.ForEach(FormatData);

            var done = true;
            if (list.Any())
            {
                var sourceFileId = list.First().SourceFileID;
                var processCode = list.First().ProcessCode;

                if (list.Count >= BulkSqlInsertSize)
                {
                    // Batching for excel files when list count is >= batch size.
                    var total = list.Count;
                    var counter = 0;
                    var processedCount = 0;
                    var bulkProcessList = new List<EntitySubscriberOriginal>();

                    foreach (var entity in list)
                    {
                        counter++;
                        processedCount++;
                        done = false;
                        bulkProcessList.Add(entity);

                        if (processedCount == total || counter == BulkSqlInsertSize)
                        {
                            done = ProcessSaveBulkSqlInsert(bulkProcessList, client, entity.SourceFileID, entity.ProcessCode, processedCount);
                            counter = 0;
                            bulkProcessList = new List<EntitySubscriberOriginal>();
                        }
                    }
                }
                else
                {
                    // Standard process for when list count is < batch size - mainly non excel files.
                    done = ProcessSaveBulkSqlInsert(list, client, sourceFileId, processCode, null);
                }
            }

            return done;
        }

        private bool ProcessSaveBulkSqlInsert(
            IEnumerable<EntitySubscriberOriginal> entities,
            ClientConnections client,
            int sourceFileId,
            string processCode,
            int? processedCount)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            bool done;
            var list = entities.ToList();
            var fileLog = new BusinessLogicFileLog();

            using (var scope = new TransactionScope())
            {
                try
                {
                    done = DataAccess.SubscriberOriginal.SaveBulkSqlInsert(list, client);
                    if (done)
                    {
                        var message = processedCount.HasValue
                            ? $"Start Bulk Insert  SubscriberDemographicOriginal : processed count = {processedCount.Value}"
                            : "Start Bulk Insert  SubscriberDemographicOriginal";
                        fileLog.Save(new EntityFileLog(sourceFileId, DefaultFileStatusTypeId, message, processCode));

                        var sendDemos = new List<Entity.SubscriberDemographicOriginal>();
                        foreach (var entity in list)
                        {
                            if (entity.DemographicOriginalList.Count > 0)
                            {
                                sendDemos.AddRange(entity.DemographicOriginalList);
                                if (sendDemos.Count >= BulkSqlInsertSize)//send 10k at a time
                                {
                                    done = DataAccess.SubscriberDemographicOriginal.SaveBulkSqlInsert(sendDemos, client);
                                    sendDemos = new List<Entity.SubscriberDemographicOriginal>();
                                }
                            }
                        }

                        //this will get the remaining items inserted
                        if (sendDemos.Count > 0)
                        {
                            done = DataAccess.SubscriberDemographicOriginal.SaveBulkSqlInsert(sendDemos, client);
                        }

                        message = processedCount.HasValue
                            ? $"End Bulk Insert SubscriberDemographicOriginal : processed count = {processedCount.Value}"
                            : "End Bulk Insert SubscriberDemographicOriginal";
                        fileLog.Save(new EntityFileLog(sourceFileId, DefaultFileStatusTypeId, message, processCode));
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    done = false;
                    var errorMessage = StringFunctions.FormatException(ex);
                    fileLog.Save(new EntityFileLog(DefaultFileId, DefaultFileStatusTypeId, errorMessage, DefaultProcessCode));
                }
            }

            return done;
        }

        public void FormatData(Entity.SubscriberOriginal x)
        {
            try
            {
                #region truncate strings
                if (x.PubCode != null && x.PubCode.Length > 100)
                    x.PubCode = x.PubCode.Substring(0, 100);
                if (x.FName != null && x.FName.Length > 100)
                    x.FName = x.FName.Substring(0, 100);
                if (x.LName != null && x.LName.Length > 100)
                    x.LName = x.LName.Substring(0, 100);
                if (x.Title != null && x.Title.Length > 100)
                    x.Title = x.Title.Substring(0, 100);
                if (x.Company != null && x.Company.Length > 100)
                    x.Company = x.Company.Substring(0, 100);
                if (x.Address != null && x.Address.Length > 255)
                    x.Address = x.Address.Substring(0, 255);
                if (x.MailStop != null && x.MailStop.Length > 255)
                    x.MailStop = x.MailStop.Substring(0, 255);
                if (x.City != null && x.City.Length > 50)
                    x.City = x.City.Substring(0, 50);
                if (x.State != null && x.State.Length > 50)
                    x.State = x.State.Substring(0, 50);
                if (x.Zip != null && x.Zip.Length > 50)
                    x.Zip = x.Zip.Substring(0, 50);
                if (x.Plus4 != null && x.Plus4.Length > 50)
                    x.Plus4 = x.Plus4.Substring(0, 50);
                if (x.ForZip != null && x.ForZip.Length > 50)
                    x.ForZip = x.ForZip.Substring(0, 50);
                if (x.County != null && x.County.Length > 100)
                    x.County = x.County.Substring(0, 100);
                if (x.Country != null && x.Country.Length > 100)
                    x.Country = x.Country.Substring(0, 100);
                if (x.Email != null && x.Email.Length > 100)
                    x.Email = x.Email.Substring(0, 100);
                if (x.RegCode != null && x.RegCode.Length > 5)
                    x.RegCode = x.RegCode.Substring(0, 5);
                if (x.Verified != null && x.Verified.Length > 100)
                    x.Verified = x.Verified.Substring(0, 100);
                if (x.SubSrc != null && x.SubSrc.Length > 25)
                    x.SubSrc = x.SubSrc.Substring(0, 25);
                if (x.OrigsSrc != null && x.OrigsSrc.Length > 25)
                    x.OrigsSrc = x.OrigsSrc.Substring(0, 25);
                if (x.Par3C != null && x.Par3C.Length > 10)
                    x.Par3C = x.Par3C.Substring(0, 10);
                if (x.Source != null && x.Source.Length > 50)
                    x.Source = x.Source.Substring(0, 50);
                if (x.Priority != null && x.Priority.Length > 4)
                    x.Priority = x.Priority.Substring(0, 4);
                if (x.Sic != null && x.Sic.Length > 8)
                    x.Sic = x.Sic.Substring(0, 8);
                if (x.SicCode != null && x.SicCode.Length > 20)
                    x.SicCode = x.SicCode.Substring(0, 20);
                if (x.Gender != null && x.Gender.Length > 1024)
                    x.Gender = x.Gender.Substring(0, 1024);
                //if (x.IGrp_Rank != null && x.IGrp_Rank.Length > 2)
                //    x.IGrp_Rank = x.IGrp_Rank.Substring(0, 2);
                //if (x.CGrp_Rank != null && x.CGrp_Rank.Length > 2)
                //    x.CGrp_Rank = x.CGrp_Rank.Substring(0, 2);
                if (x.Address3 != null && x.Address3.Length > 255)
                    x.Address3 = x.Address3.Substring(0, 255);
                if (x.Home_Work_Address != null && x.Home_Work_Address.Length > 10)
                    x.Home_Work_Address = x.Home_Work_Address.Substring(0, 10);
                //if (x.PubIDs != null && x.PubIDs.Length > 2000)
                //    x.PubIDs = x.PubIDs.Substring(0, 2000);
                if (x.Demo7 != null && x.Demo7.Length > 1)
                    x.Demo7 = x.Demo7.Substring(0, 1);
                //if (x.LatLonMsg != null && x.LatLonMsg.Length > 500)
                //    x.LatLonMsg = x.LatLonMsg.Substring(0, 500);
                if (x.AccountNumber != null && x.AccountNumber.Length > 50)
                    x.AccountNumber = x.AccountNumber.Substring(0, 50);
                if (x.Occupation != null && x.Occupation.Length > 50)
                    x.Occupation = x.Occupation.Substring(0, 50);
                if (x.Website != null && x.Website.Length > 255)
                    x.Website = x.Website.Substring(0, 255);
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
            }
            catch (Exception ex)
            {
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "FormatData"));
            }
        }
    }
}
