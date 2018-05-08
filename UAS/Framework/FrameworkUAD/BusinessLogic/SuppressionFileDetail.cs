using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SuppressionFileDetail
    {
        public void deleteBySourceFileId(Entity.SuppressionFile x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.SuppressionFileDetail.deleteBySourceFileId(x, client);
                scope.Complete();
            }
        }
        public void SaveBulkInsert(List<Entity.SuppressionFileDetail> list, KMPlatform.Object.ClientConnections client, int suppFileID)
        {
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.SuppressionFileDetail x in list)
            {
                try
                {
                    Entity.SuppressionFileDetail CleanedX = new Entity.SuppressionFileDetail(x);
                    string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.SuppressionFileDetail>(CleanedX));
                    sbXML.AppendLine(xmlObject);
                }
                catch(Exception ex)
                {
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                    if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                        app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + "XML Serialize error", app, string.Empty);
                }

                counter++;
                processedCount++;
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.SuppressionFileDetail.SaveBulkInsert("<XML>" + sbXML.ToString() + "</XML>", suppFileID,client);
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                            KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + "Suppression Bulk Insert Error", app, string.Empty);
                        }
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
        }
    }
}
