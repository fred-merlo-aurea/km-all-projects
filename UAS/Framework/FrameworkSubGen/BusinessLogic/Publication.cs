using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class Publication
    {
        public List<Entity.Publication> Select()
        {
            List<Entity.Publication> x = null;
            x = DataAccess.Publication.Select().ToList();

            return x;
        }
        public Entity.Publication Select(string publicationName, int accountId)
        {
            Entity.Publication x = null;
            x = DataAccess.Publication.Select(publicationName, accountId);

            return x;
        }
        public Entity.Publication SelectKmPubId(int kmPubId, int kmClientId)
        {
            Entity.Publication x = null;
            x = DataAccess.Publication.SelectKmPubId(kmPubId, kmClientId);

            return x;
        }
        public Entity.Publication SelectKmPubCode(string kmPubCode, int kmClientId)
        {
            Entity.Publication x = null;
            x = DataAccess.Publication.SelectKmPubCode(kmPubCode, kmClientId);

            return x;
        }
        public bool SaveBulkXml(List<Entity.Publication> list, int accountId)
        {
            foreach (Entity.Publication x in list)
            {
                FormatData(x);
                x.account_id = accountId;
            }

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.Publication x in list)
            {
                string msg = "Checking Publication: " + checkCount.ToString() + " of " + total.ToString();
                Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                checkCount++;

                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.Publication>(x));
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
                            DataAccess.Publication.SaveBulkXml("<XML>" + sbXML.ToString() + "</XML>");
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                        }
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
        public void FormatData(Entity.Publication x)
        {
            try
            {
                #region truncate strings
                if (x.name != null && x.name.Length > 50)
                    x.name = x.name.Substring(0, 50);
                if (x.KMPubCode != null && x.KMPubCode.Length > 50)
                    x.KMPubCode = x.KMPubCode.Substring(0, 50);
                #endregion
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
    }
}
