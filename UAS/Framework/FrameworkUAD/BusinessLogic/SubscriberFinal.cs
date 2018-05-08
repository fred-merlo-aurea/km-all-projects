using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using FrameworkUAD.Entity;
using KM.Common;
using ServiceStack.Text;
using ClientConnections = KMPlatform.Object.ClientConnections;
using DataFunctions = FrameworkUAD.DataAccess.DataFunctions;
using FileLogBusiness = FrameworkUAS.BusinessLogic.FileLog;
using FileLogEntity = FrameworkUAS.Entity.FileLog;
using StringFunctions = KM.Common.StringFunctions;
using SubscriberFinalAccess = FrameworkUAD.DataAccess.SubscriberFinal;
using SubscriberFinalEntity = FrameworkUAD.Entity.SubscriberFinal;
using XmlFunctions = Core_AMS.Utilities.XmlFunctions;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberFinal
    {
        private const int DefaultId = -99;
        private const int DefaultBatchSize = 500;
        private const int MaxEmailLength = 100;
        private const string EmailAddressKey = "EmailAddress";
        private const string SubscribeTypeCodeKey = "SubscribeTypeCode";
        private const string EcnOtherProductsSuppression = "ECN_OtherProductsSuppression";
        private const string EcnThirdPartySuppresion = "ECN_ThirdPartySuppresion";
        private const string InsertType = "Insert";
        private const string UpdateType = "Update";
        private const string UKey = "U";

        private const string OpenEmailNode = "<Email>";
        private const string CloseEmailNode = "</Email>";

        private const string StartEmailsNode = "<Emails>";
        private const string EndEmailsNode = "</Emails>";

        public List<Entity.SubscriberFinal> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.Select(client).ToList();
            return x;
        }
        public List<Entity.SubscriberFinal> Select(string processCode, KMPlatform.Object.ClientConnections client, bool includeDemoList = false)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.Select(processCode, client).ToList();
            if (includeDemoList)
                SelectDemographicFinalList(ref x, processCode, client);
            return x;
        }
        public List<Entity.SubscriberFinal> SelectByAddressValidation(KMPlatform.Object.ClientConnections client,string processCode, int sourceFileID, bool isLatLonValid, bool includeDemoList = false)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.SelectByAddressValidation(client,processCode, sourceFileID, isLatLonValid).ToList();
            if (includeDemoList)
                SelectDemographicFinalList(ref x, processCode, client);
            return x;
        }
        public List<Entity.SubscriberFinal> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.SelectByAddressValidation(client, isLatLonValid).ToList();
            return x;
        }
        public List<Entity.SubscriberFinal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client, bool includeDemoList = false)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            if (includeDemoList)
                SelectDemographicFinalList(ref x, processCode, client);
            return x;
        }
        public List<Entity.SubscriberFinal> SelectForFieldUpdate(string processCode, KMPlatform.Object.ClientConnections client, bool includeDemoList = false)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.SelectForFieldUpdate(processCode, client).ToList();
            if (includeDemoList)
                SelectDemographicFinalList(ref x, processCode, client);
            return x;
        }
        public List<Entity.SubscriberFinal> SelectForIgnoredReport(string processCode, bool ignored, KMPlatform.Object.ClientConnections client, bool includeDemoList = false)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.SelectForIgnoredReport(processCode, ignored, client).ToList();
            if (includeDemoList)
                SelectDemographicFinalList(ref x, processCode, client);
            return x;
        }
        public List<Entity.SubscriberFinal> SelectForProcessedReport(string processCode, bool ignored, KMPlatform.Object.ClientConnections client, bool includeDemoList = false)
        {
            List<Entity.SubscriberFinal> x = null;
            x = DataAccess.SubscriberFinal.SelectForProcessedReport(processCode, ignored, client).ToList();
            if (includeDemoList)
                SelectDemographicFinalList(ref x, processCode, client);
            return x;
        }
        /// <summary>
        /// will return a List<Object.RecordIdentifier> with SORecordIdentifier, STRecordIdentifier, SFRecordIdentifier
        /// </summary>
        /// <param name="processCode"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<Object.RecordIdentifier> SelectRecordIdentifiers(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.RecordIdentifier> x = null;
            x = DataAccess.SubscriberFinal.SelectRecordIdentifiers(processCode, client).ToList();
            return x;
        }
        public Object.AdmsResultCount SelectResultCount(string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubscriberFinal.SelectResultCount(processCode, client);
        }
        public Object.AdmsResultCount SelectResultCountAfterProcessToLive(string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubscriberFinal.SelectResultCountAfterProcessToLive(processCode, client);
        }
        private void SelectDemographicFinalList(ref List<Entity.SubscriberFinal> sfList, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SubscriberDemographicFinal sdfWrk = new BusinessLogic.SubscriberDemographicFinal();
            List<Entity.SubscriberDemographicFinal> sdfAll = sdfWrk.Select(processCode, client);
            foreach (var s in sfList)
                s.DemographicFinalList = new HashSet<Entity.SubscriberDemographicFinal>(sdfAll.Where(x => x.SFRecordIdentifier == s.SFRecordIdentifier).ToList());
        }

        public bool SaveBulkUpdate(IList<SubscriberFinalEntity> list, ClientConnections client)
        {
            return SaveOrUpdateBulkInsert(list, client, UpdateType);
        }

        public bool SaveBulkInsert(IList<SubscriberFinalEntity> list, ClientConnections client)
        {
            return SaveOrUpdateBulkInsert(list, client, InsertType);
        }

        private bool SaveOrUpdateBulkInsert(ICollection<SubscriberFinalEntity> subscriberList, ClientConnections clientConnection, string operationType)
        {
            Guard.NotNull(subscriberList, nameof(subscriberList));
            Guard.NotNull(clientConnection, nameof(clientConnection));
            Guard.NotNullOrWhitespace(operationType, nameof(operationType));

            var done = false;
            var total = subscriberList.Count;
            var counter = 0;
            var processedCount = 0;
            var sbXml = new StringBuilder();

            foreach (var item in subscriberList)
            {
                FormatData(item);
            }

            foreach (var item in subscriberList)
            {
                var xmlObject = DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<SubscriberFinalEntity>(item));
                sbXml.AppendLine(xmlObject);

                done = false;
                counter++;
                processedCount++;

                if (processedCount == total || counter == DefaultBatchSize)
                {
                    done = SaveOrUpdateData(clientConnection, sbXml, item, operationType);
                    sbXml = new StringBuilder();
                    counter = 0;
                }
            }

            return done;
        }

        private static bool SaveOrUpdateData(
            ClientConnections clientConnection, 
            StringBuilder sbXml, 
            SubscriberBase subscriberBase, 
            string operationType)
        {
            var done = false;
            var xml = $"<XML>{sbXml}</XML>";

            using (var scope = new TransactionScope())
            {
                try
                {
                    if (operationType.Equals(InsertType))
                    {
                        done = SubscriberFinalAccess.SaveBulkInsert(xml, clientConnection);
                    }

                    if (operationType.Equals(UpdateType))
                    {
                        done = SubscriberFinalAccess.SaveBulkUpdate(xml, clientConnection);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    done = false;
                    var message = StringFunctions.FormatException(ex);
                    var fileLog = new FileLogBusiness();
                    fileLog.Save(new FileLogEntity(DefaultId, DefaultId, message, subscriberBase.ProcessCode));
                }
            }

            return done;
        }

        public int Save(Entity.SubscriberFinal x, KMPlatform.Object.ClientConnections client)
        {
            FormatData(x);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SubscriberFinalID = DataAccess.SubscriberFinal.Save(x, client);
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

            return x.SubscriberFinalID;
        }

        public void FormatData(Entity.SubscriberFinal x)
        {
            try
            {
                //x = PopulateNull(x);

                //if (x.SFRecordIdentifier == Guid.Empty)
                //    x.SFRecordIdentifier = Guid.NewGuid();
                if (x.Phone != null)
                    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
                if (x.Mobile != null)
                    x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
                if (x.Fax != null)
                    x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);
                //if (x.Email != null && x.Email.Length <= 4)
                //    x.Email = string.Empty;

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
                if (x.SubSrc != null && x.SubSrc.Length > 8)
                    x.SubSrc = x.SubSrc.Substring(0, 8);
                if (x.OrigsSrc != null && x.OrigsSrc.Length > 8)
                    x.OrigsSrc = x.OrigsSrc.Substring(0, 8);
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
                if (x.IGrp_Rank != null && x.IGrp_Rank.Length > 2)
                    x.IGrp_Rank = x.IGrp_Rank.Substring(0, 2);
                if (x.CGrp_Rank != null && x.CGrp_Rank.Length > 2)
                    x.CGrp_Rank = x.CGrp_Rank.Substring(0, 2);
                if (x.Address3 != null && x.Address3.Length > 255)
                    x.Address3 = x.Address3.Substring(0, 255);
                if (x.Home_Work_Address != null && x.Home_Work_Address.Length > 10)
                    x.Home_Work_Address = x.Home_Work_Address.Substring(0, 10);
                //if (x.PubIDs != null && x.PubIDs.Length > 2000)
                //    x.PubIDs = x.PubIDs.Substring(0, 2000);
                if (x.Demo7 != null && x.Demo7.Length > 1)
                    x.Demo7 = x.Demo7.Substring(0, 1);
                if (x.LatLonMsg != null && x.LatLonMsg.Length > 500)
                    x.LatLonMsg = x.LatLonMsg.Substring(0, 500);
                if (x.AccountNumber != null && x.AccountNumber.Length > 50)
                    x.AccountNumber = x.AccountNumber.Substring(0, 50);
                if (x.Occupation != null && x.Occupation.Length > 50)
                    x.Occupation = x.Occupation.Substring(0, 50);
                if (x.Website != null && x.Website.Length > 255)
                    x.Website = x.Website.Substring(0, 255);
                if(!string.IsNullOrEmpty(x.SubGenRenewalCode) && x.SubGenRenewalCode.Length > 50)
                    x.SubGenRenewalCode = x.SubGenRenewalCode.Substring(0, 50);
                #endregion

                if (x.QDate == DateTime.Parse("0001-01-01T00:00:00") || x.QDate == DateTime.MinValue || x.QDate <= DateTime.Parse("1/1/1900"))
                    x.QDate = DateTime.Now;
                //if (x.StatusUpdatedDate == DateTime.Parse("0001-01-01T00:00:00") || x.StatusUpdatedDate == DateTime.MinValue || x.StatusUpdatedDate <= DateTime.Parse("1/1/1900"))
                //    x.StatusUpdatedDate = DateTime.Now;

                //// This logic requested by Sunil - q.k 04072015
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

        public void ECN_ThirdPartySuppresion(ClientConnections client, string processCode, List<int> groupIds)
        {
            CreateAndSaveXml(client, processCode, groupIds, EcnThirdPartySuppresion);
        }

        public void ECN_OtherProductsSuppresion(ClientConnections client, string processCode, List<int> groupIds)
        {
            CreateAndSaveXml(client, processCode, groupIds, EcnOtherProductsSuppression);
        }

        private void CreateAndSaveXml(ClientConnections client, string processCode, IEnumerable<int> groupIds, string type)
        {
            Guard.NotNull(client, nameof(client));
            Guard.NotNullOrWhitespace(processCode, nameof(processCode));
            Guard.NotNull(groupIds, nameof(groupIds));

            foreach (var groupId in groupIds)
            {
                var emailList = SubscriberFinalAccess.GetEmailListFromEcn(groupId);
                if (emailList?.Rows.Count == 0)
                {
                    continue;
                }

                var xml = new StringBuilder();
                xml.AppendLine("<XML>");

                foreach (DataRow row in emailList.Rows)
                {
                    xml.AppendLine(StartEmailsNode);
                    TryAppendEmailNode(row, xml, processCode, type);
                    xml.AppendLine(EndEmailsNode);
                }
                xml.AppendLine("</XML>");

                SaveXml(client, xml, processCode, type);
            }
        }

        private static void TryAppendEmailNode(DataRow row, StringBuilder xml, string processCode, string type)
        {
            try
            {
                if (type.Equals(EcnOtherProductsSuppression))
                {
                    if (row[SubscribeTypeCodeKey].ToString().Equals(UKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        AppendEmailNode(xml, row);
                    }
                }
                else
                {
                    AppendEmailNode(xml, row);
                }
            }
            catch (Exception ex)
            {
                var message = StringFunctions.FormatException(ex);
                var fileLog = new FileLogBusiness();
                fileLog.Save(new FileLogEntity(DefaultId, DefaultId, message, processCode));
            }
        }

        private static void AppendEmailNode(StringBuilder xml, DataRow row)
        {
            var email = row[EmailAddressKey].ToString().Trim();
            if (email.Length > MaxEmailLength)
            {
                email = email.Substring(0, MaxEmailLength);
            }

            xml.AppendLine(OpenEmailNode);
            xml.AppendLine(XmlFunctions.CleanAllXml(email));
            xml.AppendLine(CloseEmailNode);
        }

        private static void SaveXml(ClientConnections client, StringBuilder xml, string processCode, string type)
        {
            if (type.Equals(EcnOtherProductsSuppression))
            {
                SubscriberFinalAccess.ECN_OtherProductsSuppression(xml, processCode, client);
            }
            else
            {
                SubscriberFinalAccess.ECN_ThirdPartySuppresion(xml, processCode, client);
            }
        }

        #region Jobs
        public bool SaveDQMClean(KMPlatform.Object.ClientConnections client, string processCode, string fileType = "")
        {
            return DataAccess.SubscriberFinal.SaveDQMClean(client, processCode, fileType);
        }
        public bool NullifyKMPSGroupEmails(KMPlatform.Object.ClientConnections client, string processCode)
        {
            return DataAccess.SubscriberFinal.NullifyKMPSGroupEmails(client, processCode);
        }
        public bool SetMissingMaster(KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.SubscriberFinal.SetMissingMaster(client);
                scope.Complete();
                done = true;
            }

            return done;
        }
        public bool SetOneMaster(KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.SubscriberFinal.SetOneMaster(client);
                scope.Complete();
                done = true;
            }

            return done;
        }
        public bool AddressSearch(string address, string mailstop, string city, string state, string zip, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;

            DataAccess.SubscriberFinal.AddressSearch(address, mailstop, city, state, zip, client);
            done = true;

            return done;
        }
        #endregion

        #region DataCompare
        public void SetPubCode(KMPlatform.Object.ClientConnections client, string processCode, int productId, int brandId)
        {

        }
        #endregion
    }
}
