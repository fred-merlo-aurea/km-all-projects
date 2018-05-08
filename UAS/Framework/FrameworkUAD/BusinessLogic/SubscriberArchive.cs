using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using ServiceStack.Text;
using CommonStringFunctions = KM.Common.StringFunctions;
using FileLogEntity = FrameworkUAS.Entity.FileLog;
using FileLog = FrameworkUAS.BusinessLogic.FileLog;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberArchive
    {
        private const int DefaultFileLogId = -99;

        public List<Entity.SubscriberArchive> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberArchive> x = null;
            x = DataAccess.SubscriberArchive.Select(client).ToList();
            return x;
        }
        public List<Entity.SubscriberArchive> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberArchive> x = null;
            x = DataAccess.SubscriberArchive.Select(processCode, client).ToList();
            return x;
        }
        public List<Entity.SubscriberArchive> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberArchive> x = null;
            x = DataAccess.SubscriberArchive.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }

        public int Save(Entity.SubscriberArchive x, KMPlatform.Object.ClientConnections client)
        {
            FormatData(x);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SubscriberArchiveID = DataAccess.SubscriberArchive.Save(x, client);
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

            return x.SubscriberArchiveID;
        }

        public bool SaveBulkInsert(List<Entity.SubscriberArchive> list, KMPlatform.Object.ClientConnections client)
        {
            foreach (Entity.SubscriberArchive x in list)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.SubscriberArchive x in list)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.SubscriberArchive>(x));
                sbXML.AppendLine(xmlObject);

                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    done = SaveSubscriberArchive(client, x, sbXML);
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }

        public bool SaveSubscriberArchive(
            KMPlatform.Object.ClientConnections clientConnections,
            Entity.SubscriberArchive subscriberArchive,
            StringBuilder sbXml)
        {
            if (clientConnections == null)
            {
                throw new ArgumentNullException(nameof(clientConnections));
            }
            if (subscriberArchive == null)
            {
                throw new ArgumentNullException(nameof(subscriberArchive));
            }
            if (sbXml == null)
            {
                throw new ArgumentNullException(nameof(sbXml));
            }

            bool done;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var xmlLine = $"<XML>{sbXml}</XML>";
                    done = DataAccess.SubscriberArchive.SaveBulkInsert(xmlLine, clientConnections);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    done = false;
                    var message = CommonStringFunctions.FormatException(ex);
                    var fileLog = new FileLog();
                    fileLog.Save(new FileLogEntity(DefaultFileLogId, DefaultFileLogId, message, subscriberArchive.ProcessCode));
                }
            }
            return done;
        }

        public bool SaveBulkInsert(List<Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberArchive> converted = ConvertToArchive(list);
            foreach (Entity.SubscriberArchive x in converted)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = converted.Count;
            int counter = 0;
            int processedCount = 0;
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.SubscriberArchive x in converted)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.SubscriberArchive>(x));
                sbXML.AppendLine(xmlObject);

                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    done = SaveSubscriberArchive(client, x, sbXML);
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
        public bool SaveBulkInsert(List<Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberArchive> converted = ConvertToArchive(list);
            foreach (Entity.SubscriberArchive x in converted)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = converted.Count;
            int counter = 0;
            int processedCount = 0;
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.SubscriberArchive x in converted)
            {
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.SubscriberArchive>(x));
                sbXML.AppendLine(xmlObject);

                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    done = SaveSubscriberArchive(client, x, sbXML);
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
        private List<Entity.SubscriberArchive> ConvertToArchive(List<Entity.SubscriberOriginal> list)
        {
            List<Entity.SubscriberArchive> converted = new List<Entity.SubscriberArchive>();
            foreach (Entity.SubscriberOriginal x in list)
            {
                Entity.SubscriberArchive a = new Entity.SubscriberArchive();
                a.AccountNumber = x.AccountNumber;
                a.Address = x.Address;
                a.Address3 = x.Address3;
                a.CategoryID = x.CategoryID;
                //a.CGrp_Cnt = x.CGrp_Cnt;
                //a.CGrp_No = x.CGrp_No;
                //a.CGrp_Rank = x.CGrp_Rank;
                a.City = x.City;
                a.Company = x.Company;
                a.Copies = x.Copies;
                a.Country = x.Country;
                a.CountryID = x.CountryID;
                a.County = x.County;
                a.CreatedByUserID = x.CreatedByUserID;
                a.DateCreated = x.DateCreated;
                a.DateUpdated = x.DateUpdated;
                a.MailPermission = x.MailPermission;
                a.FaxPermission = x.FaxPermission;
                a.PhonePermission = x.PhonePermission;
                a.OtherProductsPermission = x.OtherProductsPermission;
                a.ThirdPartyPermission = x.ThirdPartyPermission;
                a.EmailRenewPermission = x.EmailRenewPermission;
                a.TextPermission = x.TextPermission;
                a.Demo7 = x.Demo7;
                a.Email = x.Email;
                a.EmailID = x.EmailID;
                //a.EmailExists = x.EmailExists;
                a.EmailStatusID = x.EmailStatusID;
                a.ExternalKeyId = x.ExternalKeyId;
                a.Fax = x.Fax;
                //a.FaxExists = x.FaxExists;
                a.FName = x.FName;
                a.ForZip = x.ForZip;
                a.Gender = x.Gender;
                a.GraceIssues = x.GraceIssues;
                a.Home_Work_Address = x.Home_Work_Address;
                //a.IGrp_Cnt = x.IGrp_Cnt;
                //a.IGrp_No = x.IGrp_No;
                //a.IGrp_Rank = x.IGrp_Rank;
                a.ImportRowNumber = x.ImportRowNumber;
                a.IsActive = x.IsActive;
                a.IsComp = x.IsComp;
                a.IsPaid = x.IsPaid;
                a.IsSubscribed = x.IsSubscribed;
                //a.IsMailable = x.IsMailable;
                a.Latitude = x.Latitude;
                a.LName = x.LName;
                a.Longitude = x.Longitude;
                a.MailStop = x.MailStop;
                a.Mobile = x.Mobile;
                a.Occupation = x.Occupation;
                a.OrigsSrc = x.OrigsSrc;
                a.Par3C = x.Par3C;
                a.Phone = x.Phone;
                //a.PhoneExists = x.PhoneExists;
                a.Plus4 = x.Plus4;
                a.Priority = x.Priority;
                a.ProcessCode = x.ProcessCode;
                a.PubCode = x.PubCode;
                a.QDate = x.QDate;
                a.QSourceID = x.QSourceID;
                a.RegCode = x.RegCode;
                a.Sequence = x.Sequence;
                a.Sic = x.Sic;
                a.SicCode = x.SicCode;
                a.Source = x.Source;
                a.SourceFileID = x.SourceFileID;
                a.State = x.State;
                a.SubscriptionStatusID = x.SubscriptionStatusID;
                a.SubSrc = x.SubSrc;
                a.SubsrcID = x.SubsrcID;
                a.Title = x.Title;
                a.TransactionDate = x.TransactionDate;
                a.TransactionID = x.TransactionID;
                a.UpdatedByUserID = x.UpdatedByUserID;
                a.Verified = x.Verified;
                a.Website = x.Website;
                a.Zip = x.Zip;

                converted.Add(a);
            }
            return converted;
        }
        private List<Entity.SubscriberArchive> ConvertToArchive(List<Entity.SubscriberTransformed> list)
        {
            List<Entity.SubscriberArchive> converted = new List<Entity.SubscriberArchive>();
            foreach (Entity.SubscriberTransformed x in list)
            {
                Entity.SubscriberArchive a = new Entity.SubscriberArchive();
                a.AccountNumber = x.AccountNumber;
                a.Address = x.Address;
                a.Address3 = x.Address3;
                a.CategoryID = x.CategoryID;
                //a.CGrp_Cnt = x.CGrp_Cnt;
                //a.CGrp_No = x.CGrp_No;
                //a.CGrp_Rank = x.CGrp_Rank;
                a.City = x.City;
                a.Company = x.Company;
                a.Copies = x.Copies;
                a.Country = x.Country;
                a.CountryID = x.CountryID;
                a.County = x.County;
                a.CreatedByUserID = x.CreatedByUserID;
                a.DateCreated = x.DateCreated;
                a.DateUpdated = x.DateUpdated;
                a.MailPermission = x.MailPermission;
                a.FaxPermission = x.FaxPermission;
                a.PhonePermission = x.PhonePermission;
                a.OtherProductsPermission = x.OtherProductsPermission;
                a.ThirdPartyPermission = x.ThirdPartyPermission;
                a.EmailRenewPermission = x.EmailRenewPermission;
                a.TextPermission = x.TextPermission;
                a.Demo7 = x.Demo7;
                a.Email = x.Email;
                //a.EmailExists = x.EmailExists;
                a.EmailID = x.EmailID;
                a.EmailStatusID = x.EmailStatusID;
                a.ExternalKeyId = x.ExternalKeyId;
                a.Fax = x.Fax;
                //a.FaxExists = x.FaxExists;
                a.FName = x.FName;
                a.ForZip = x.ForZip;
                a.Gender = x.Gender;
                a.GraceIssues = x.GraceIssues;
                a.Home_Work_Address = x.Home_Work_Address;
                //a.IGrp_Cnt = x.IGrp_Cnt;
                //a.IGrp_No = x.IGrp_No;
                //a.IGrp_Rank = x.IGrp_Rank;
                a.ImportRowNumber = x.ImportRowNumber;
                a.IsActive = x.IsActive;
                a.IsComp = x.IsComp;
                a.IsPaid = x.IsPaid;
                a.IsSubscribed = x.IsSubscribed;
                //a.IsMailable = x.IsMailable;
                a.Latitude = x.Latitude;
                a.LName = x.LName;
                a.Longitude = x.Longitude;
                a.MailStop = x.MailStop;
                a.Mobile = x.Mobile;
                a.Occupation = x.Occupation;
                a.OrigsSrc = x.OrigsSrc;
                a.Par3C = x.Par3C;
                a.Phone = x.Phone;
                //a.PhoneExists = x.PhoneExists;
                a.Plus4 = x.Plus4;
                a.Priority = x.Priority;
                a.ProcessCode = x.ProcessCode;
                a.PubCode = x.PubCode;
                a.QDate = x.QDate;
                a.QSourceID = x.QSourceID;
                a.RegCode = x.RegCode;
                a.Sequence = x.Sequence;
                a.Sic = x.Sic;
                a.SicCode = x.SicCode;
                a.Source = x.Source;
                a.SourceFileID = x.SourceFileID;
                a.State = x.State;
                //a.StatList = x.StatList;
                a.SubscriptionStatusID = x.SubscriptionStatusID;
                a.SubSrc = x.SubSrc;
                a.SubsrcID = x.SubsrcID;
                a.Title = x.Title;
                a.TransactionDate = x.TransactionDate;
                a.TransactionID = x.TransactionID;
                a.UpdatedByUserID = x.UpdatedByUserID;
                a.Verified = x.Verified;
                a.Zip = x.Zip;
                a.Website = x.Website;
                converted.Add(a);
            }
            return converted;
        }

        public void FormatData(Entity.SubscriberArchive x)
        {
            try
            {
                //x = PopulateNull(x);

                //if (x.SARecordIdentifier == Guid.Empty)
                //    x.SARecordIdentifier = Guid.NewGuid();
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
    }
}
