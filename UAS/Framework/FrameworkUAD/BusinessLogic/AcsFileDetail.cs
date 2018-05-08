using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core_AMS.Utilities;
using KM.Common.Functions;
using ServiceStack.Text;

namespace FrameworkUAD.BusinessLogic
{
    public class AcsFileDetail
    {
        private const string ObjectSubscriber = "Subscriber";
        private const string JobAcsUpdateSubscriberAddress = "job_ACS_UpdateSubscriberAddress";

        public Entity.AcsFileDetail Parse(string detailRecord, KMPlatform.Object.ClientConnections client)
        {
            var acsFileDetail = new Entity.AcsFileDetail();
            if (detailRecord.Length == 700)
            {
                ReadHeaderInfo(detailRecord, acsFileDetail);
                ReadPersonInfo(detailRecord, acsFileDetail);
                ReadOldAddress(detailRecord, acsFileDetail);
                ReadNewAddress(detailRecord, acsFileDetail);
                ReadDeliveryParameters(detailRecord, acsFileDetail);
                ReadProductCodes(detailRecord, acsFileDetail);
                ReadFinishInformation(detailRecord, acsFileDetail);
            }

            return acsFileDetail;
        }

        private static void ReadFinishInformation(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.Filler = GetString(detailRecord, 617, 82);
            acsFileDetail.EndMarker = GetString(detailRecord, 699, 1);
        }

        private void ReadHeaderInfo(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.RecordType = GetString(detailRecord, 0, 1);
            acsFileDetail.FileVersion = GetString(detailRecord, 1, 2);
            acsFileDetail.SequenceNumber = GetInt(detailRecord, 3, 8);
            acsFileDetail.AcsMailerId = GetAlphaNumber(detailRecord, 11, 9);
            acsFileDetail.KeylineSequenceSerialNumber = GetAlphaNumber(detailRecord, 20, 16);
            acsFileDetail.MoveEffectiveDate = GetDate(detailRecord, 36, 8);
            acsFileDetail.MoveType = GetString(detailRecord, 44, 1);
            acsFileDetail.DeliverabilityCode = GetString(detailRecord, 45, 1);
            acsFileDetail.UspsSiteID = GetInt(detailRecord, 46, 3);
        }

        private void ReadDeliveryParameters(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.Hyphen = GetString(detailRecord, 352, 1);
            acsFileDetail.NewPlus4Code = GetString(detailRecord, 353, 4);
            acsFileDetail.NewDeliveryPoint = GetString(detailRecord, 357, 2);
            acsFileDetail.NewAbbreviatedCityName = GetString(detailRecord, 359, 13);
            acsFileDetail.NewAddressLabel = GetString(detailRecord, 372, 66);
            acsFileDetail.FeeNotification = GetString(detailRecord, 438, 1);
            acsFileDetail.NotificationType = GetString(detailRecord, 439, 1);
            acsFileDetail.IntelligentMailBarcode = GetString(detailRecord, 440, 31);
            acsFileDetail.IntelligentMailPackageBarcode = GetString(detailRecord, 471, 35);
            acsFileDetail.IdTag = GetString(detailRecord, 506, 16);
            acsFileDetail.HardcopyToElectronicFlag = GetString(detailRecord, 522, 1);
            acsFileDetail.TypeOfAcs = GetString(detailRecord, 523, 1);
            acsFileDetail.FulfillmentDate = GetDate(detailRecord, 524, 8);
            acsFileDetail.ProcessingType = GetString(detailRecord, 532, 1);
            acsFileDetail.CaptureType = GetString(detailRecord, 533, 1);
            acsFileDetail.MadeAvailableDate = GetDate(detailRecord, 534, 8);
            acsFileDetail.ShapeOfMail = GetString(detailRecord, 542, 1);
            acsFileDetail.MailActionCode = GetString(detailRecord, 543, 1);
            acsFileDetail.NixieFlag = GetString(detailRecord, 544, 1);
        }

        private static void ReadNewAddress(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.NewAddressType = GetString(detailRecord, 220, 1);
            acsFileDetail.NewPmb = GetString(detailRecord, 221, 8);
            acsFileDetail.NewUrb = GetString(detailRecord, 229, 28);
            acsFileDetail.NewPrimaryNumber = GetString(detailRecord, 257, 10);
            acsFileDetail.NewPreDirectional = GetString(detailRecord, 267, 2);
            acsFileDetail.NewStreetName = GetString(detailRecord, 269, 28);
            acsFileDetail.NewSuffix = GetString(detailRecord, 297, 4);
            acsFileDetail.NewPostDirectional = GetString(detailRecord, 301, 2);
            acsFileDetail.NewUnitDesignator = GetString(detailRecord, 303, 4);
            acsFileDetail.NewSecondaryNumber = GetString(detailRecord, 307, 10);
            acsFileDetail.NewCity = GetString(detailRecord, 317, 28);
            acsFileDetail.NewStateAbbreviation = GetString(detailRecord, 345, 2);
            acsFileDetail.NewZipCode = GetString(detailRecord, 347, 5);
        }

        private static void ReadOldAddress(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.OldAddressType = GetString(detailRecord, 96, 1);
            acsFileDetail.OldUrb = GetString(detailRecord, 97, 28);
            acsFileDetail.OldPrimaryNumber = GetString(detailRecord, 125, 10);
            acsFileDetail.OldPreDirectional = GetString(detailRecord, 135, 2);
            acsFileDetail.OldStreetName = GetString(detailRecord, 137, 28);
            acsFileDetail.OldSuffix = GetString(detailRecord, 165, 4);
            acsFileDetail.OldPostDirectional = GetString(detailRecord, 169, 2);
            acsFileDetail.OldUnitDesignator = GetString(detailRecord, 171, 4);
            acsFileDetail.OldSecondaryNumber = GetString(detailRecord, 175, 10);
            acsFileDetail.OldCity = GetString(detailRecord, 185, 28);
            acsFileDetail.OldStateAbbreviation = GetString(detailRecord, 213, 2);
            acsFileDetail.OldZipCode = GetString(detailRecord, 215, 5);
        }

        private static void ReadPersonInfo(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.LastName = GetString(detailRecord, 49, 20);
            acsFileDetail.FirstName = GetString(detailRecord, 69, 15);
            acsFileDetail.Prefix = GetString(detailRecord, 84, 6);
            acsFileDetail.Suffix = GetString(detailRecord, 90, 6);
        }

        private void ReadProductCodes(string detailRecord, Entity.AcsFileDetail acsFileDetail)
        {
            acsFileDetail.ProductCode1 = GetInt(detailRecord, 545, 6);
            acsFileDetail.ProductCodeFee1 = GetDecimal(detailRecord, 551, 6);

            acsFileDetail.ProductCode2 = GetInt(detailRecord, 557, 6);
            acsFileDetail.ProductCodeFee2 = GetDecimal(detailRecord, 563, 6);

            acsFileDetail.ProductCode3 = GetInt(detailRecord, 569, 6);
            acsFileDetail.ProductCodeFee3 = GetDecimal(detailRecord, 575, 6);

            acsFileDetail.ProductCode4 = GetInt(detailRecord, 581, 6);
            acsFileDetail.ProductCodeFee4 = GetDecimal(detailRecord, 587, 6);

            acsFileDetail.ProductCode5 = GetInt(detailRecord, 593, 6);
            acsFileDetail.ProductCodeFee5 = GetDecimal(detailRecord, 599, 6);

            acsFileDetail.ProductCode6 = GetInt(detailRecord, 605, 6);
            acsFileDetail.ProductCodeFee6 = GetDecimal(detailRecord, 611, 6);
        }

        private decimal GetDecimal(string detailRecord, int startPos, int offset)
        {
            decimal pcf1 = 0;
            decimal.TryParse(detailRecord.Substring(startPos, offset).Trim(), out pcf1);
            return pcf1 > 0 ? pcf1 / 100 : 0;
        }

        private DateTime GetDate(string detailRecord, int startPos, int offset)
        {
             return DateTimeFunctions.ParseDate(DateFormat.YYYYMMDD, detailRecord.Substring(startPos, offset).Trim());
        }

        private string GetAlphaNumber(string detailRecord, int startPos, int offset)
        {
            return StringFunctions.RemoveNonAlphaNumeric(detailRecord.Substring(startPos, offset).Trim());
        }

        private static int GetInt(string detailRecord, int startPos, int length)
        {
            var sq = 0;
            int.TryParse(detailRecord.Substring(startPos, length).Trim(), out sq);
            return sq;
        }

        private static string GetString(string detailRecord, int startPos, int length)
        {
            return detailRecord.Substring(startPos, length).Trim();
        }

        public List<Entity.AcsFileDetail> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.AcsFileDetail> x = null;
            x = DataAccess.AcsFileDetail.Select(processCode,client).ToList();
            return x;
        }
        public int Save(Entity.AcsFileDetail detail, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                detail.AcsFileDetailId = DataAccess.AcsFileDetail.Save(detail, client);
                scope.Complete();
            }

            return detail.AcsFileDetailId;
        }
        public bool Update(List<Entity.AcsFileDetail> details, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                System.Text.StringBuilder xml = new System.Text.StringBuilder();
                foreach (Entity.AcsFileDetail x in details)
                {
                    string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.AcsFileDetail>(x), "FrameworkUAD.Entity");
                    xml.AppendLine(xmlObject);
                }
                complete = DataAccess.AcsFileDetail.Update("<XML>" + xml.ToString() + "</XML>",client);
                scope.Complete();
            }

            return complete;
        }
        public bool Insert(List<Entity.AcsFileDetail> details, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.AcsFileDetail.Insert(details, client);
                scope.Complete();
            }

            return complete;
        }
        public bool UpdateSubscriberAddress(List<Entity.AcsFileDetail> details, int publicationID, KMPlatform.Object.ClientConnections client, int userLogTypeID, int SourceFileID)
        {
            bool complete = false;

            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            foreach (Entity.AcsFileDetail x in details)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.AcsFileDetail>(x), "FrameworkUAD.Entity");
                xml.AppendLine(xmlObject);
            }

            KMPlatform.BusinessLogic.Application aWorker = new KMPlatform.BusinessLogic.Application();
            int appId = aWorker.Select().Single(x => x.ApplicationName.Equals("UAD", StringComparison.CurrentCultureIgnoreCase)).ApplicationID;

            int userId = 0;
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            userId = uWorker.SearchEmail("AcsImport@TeamKM.com").UserID;

            var userLogId = SaveUserLog(userLogTypeID, appId, userId);
            complete = DataAccess.AcsFileDetail.UpdateSubscriberAddress("<XML>" + xml.ToString() + "</XML>", publicationID, client, userLogId, userId, SourceFileID);

            return complete;
        }

        public int SaveUserLog(int userLogTypeId, int appId, int userId)
        {
            var userLog = new KMPlatform.Entity.UserLog
            {
                ApplicationID = appId,
                DateCreated = DateTime.Now,
                FromObjectValues = JobAcsUpdateSubscriberAddress,
                Object = ObjectSubscriber,
                ToObjectValues = string.Empty,
                UserID = userId,
                UserLogTypeID = userLogTypeId
            };

            var ulWorker = new KMPlatform.BusinessLogic.UserLog();
            return ulWorker.Save(userLog);
        }

        public bool KillSubscriber(List<Entity.AcsFileDetail> details, int publicationID, KMPlatform.Object.ClientConnections client, int userLogTypeID, int SourceFileID)
        {
            bool complete = false;

            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            foreach (Entity.AcsFileDetail x in details)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.AcsFileDetail>(x), "FrameworkUAD.Entity");
                xml.AppendLine(xmlObject);
            }

            KMPlatform.BusinessLogic.Application aWorker = new KMPlatform.BusinessLogic.Application();
            int appId = aWorker.Select().Single(x => x.ApplicationName.Equals("Circulation", StringComparison.CurrentCultureIgnoreCase)).ApplicationID;

            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            int userId = uWorker.SearchEmail("AcsImport@TeamKM.com").UserID;

            var userLogId = SaveUserLog(userLogTypeID, appId, userId);
            complete = DataAccess.AcsFileDetail.KillSubscriber("<XML>" + xml.ToString() + "</XML>", publicationID, client, appId, userId, userLogId, SourceFileID);

            return complete;
        }

        public bool UpdateUADSubscriberAddress(List<Entity.AcsFileDetail> details, KMPlatform.Object.ClientConnections client, int userLogTypeID)
        {
            bool complete = false;

            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            foreach (Entity.AcsFileDetail x in details)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.AcsFileDetail>(x), "FrameworkUAD.Entity");
                xml.AppendLine(xmlObject);
            }

            KMPlatform.BusinessLogic.Application aWorker = new KMPlatform.BusinessLogic.Application();
            int appId = aWorker.Select().Single(x => x.ApplicationName.Equals("UAD", StringComparison.CurrentCultureIgnoreCase)).ApplicationID;

            //--= (select UserID from UAS..[User] where EmailAddress = 'AcsImport@TeamKM.com')
            int userId = 0;
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            userId = uWorker.SearchEmail("AcsImport@TeamKM.com").UserID;

            var userLogId = SaveUserLog(userLogTypeID, appId, userId);
            complete = DataAccess.AcsFileDetail.UpdateUADSubscriberAddress("<XML>" + xml.ToString() + "</XML>", userId, client);

            return complete;
        }
        public bool KillUADSubscriber(List<Entity.AcsFileDetail> details, int clientID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                System.Text.StringBuilder xml = new System.Text.StringBuilder();
                foreach (Entity.AcsFileDetail x in details)
                {
                    string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.AcsFileDetail>(x), "FrameworkUAD.Entity");
                    xml.AppendLine(xmlObject);
                }
                DataAccess.AcsFileDetail.KillUADSubscriber("<XML>" + xml.ToString() + "</XML>", clientID, client);
                scope.Complete();
                complete = true;
            }

            return complete;
        }
    }
}
