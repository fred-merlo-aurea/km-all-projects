//using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class UserLog
    {
        public  KMPlatform.Entity.UserLog CreateLog(int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes userLogType,
                                                          int userID, string objectName, string originalObjectJson, string newObjectJson, int userLogTypeID, int clientID = 0, string groupTransCode = "")
        {
            KMPlatform.Entity.UserLog ul = new KMPlatform.Entity.UserLog();
            ul.ApplicationID = applicationID;
            ul.UserLogTypeID = userLogTypeID;
            ul.UserID = userID;
            ul.Object = objectName;
            ul.FromObjectValues = originalObjectJson;
            ul.ToObjectValues = newObjectJson;
            ul.DateCreated = DateTime.Now;
            ul.ClientID = clientID;
            ul.GroupTransactionCode = groupTransCode;
            KMPlatform.BusinessLogic.UserLog uls = new UserLog();
            ul.UserLogID = uls.Save(ul);

            return ul;
        }
        public KMPlatform.Entity.UserLog LogIn(string userName, string password, int userLogTypeID, int userID = -1)
        {
            KMPlatform.Entity.UserLog ul = new KMPlatform.Entity.UserLog();
            ul.ApplicationID = userID;
            ul.UserLogTypeID = userLogTypeID;
            ul.UserID = -1;
            ul.Object = "User";
            ul.FromObjectValues = "UserName:" + userName;
            ul.ToObjectValues = "Password:" + password;
            ul.DateCreated = DateTime.Now;
            ul.ClientID = 0;
            ul.GroupTransactionCode = "";
            KMPlatform.BusinessLogic.UserLog uls = new UserLog();
            ul.UserLogID = uls.Save(ul);

            return ul;
        }
        public KMPlatform.Entity.UserLog LogOut(KMPlatform.Entity.User user)
        {
            KMPlatform.Entity.UserLog ul = new KMPlatform.Entity.UserLog();
            ul.ApplicationID = -1;
            //KMPlatform.BusinessLogic.UserLogType ult = new UserLogType();
            //ul.UserLogTypeID = ult.Select(KMPlatform.BusinessLogic.Enums.UserLogType.LogOut).UserLogTypeID;
            //Code cworker = new Code();
            //ul.UserLogTypeID = cworker.SelectCodeName(Enums.CodeTypes.User_Log, Enums.UserLogTypes.Log_Out.ToString()).CodeId;
            ul.UserID = user.UserID;
            ul.Object = "User";
            ul.FromObjectValues = string.Empty;
            ul.ToObjectValues = string.Empty;
            ul.DateCreated = DateTime.Now;
            ul.ClientID = 0;
            ul.GroupTransactionCode = "";
            KMPlatform.BusinessLogic.UserLog uls = new UserLog();
            ul.UserLogID = uls.Save(ul);

            return ul;
        }
        public  List<Entity.UserLog> Select()
        {
            List<Entity.UserLog> x = null;
            x = DataAccess.UserLog.Select().ToList();

            return x;
        }
        public  Entity.UserLog Select(int userLogID)
        {
            Entity.UserLog x = null;
            x = DataAccess.UserLog.Select(userLogID);

            return x;
        }
    
        public  int Save(Entity.UserLog x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.UserLogID = DataAccess.UserLog.Save(x);
                scope.Complete();
            }

            return x.UserLogID;
        }

        public List<KMPlatform.Entity.UserLog> SaveBulkInsert(List<Entity.UserLog> list, KMPlatform.Entity.Client client)
        {
            //throw new NotImplementedException(); // TODO

            List<KMPlatform.Entity.UserLog> done = new List<KMPlatform.Entity.UserLog>();
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.UserLog x in list)
            {
                string xml = "";
                System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer(typeof(Entity.UserLog));
                using (System.IO.StringWriter sww = new System.IO.StringWriter())
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xmlSer.Serialize(writer, x);
                    xml = sww.ToString();
                    xml = Core_AMS.Utilities.XmlFunctions.CleanSerializedXML(xml);
                }

                sbXML.AppendLine(xml);
                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        try
                        {
                            done = DataAccess.UserLog.SaveBulkInsert("<XML>" + sbXML.ToString() + "</XML>", client);
                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                        }
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
    }
}
