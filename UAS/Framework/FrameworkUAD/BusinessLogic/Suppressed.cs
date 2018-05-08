using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Text;
using System.IO;

namespace FrameworkUAD.BusinessLogic
{
    public class Suppressed
    {
       
        public bool SaveBulkSqlInsert(List<Entity.Suppressed> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;
            int BatchSize = 1000;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.Suppressed> bulkProcessList = new List<Entity.Suppressed>();
            foreach (Entity.Suppressed x in list)
            {
                counter++;
                processedCount++;
                done = false;
                bulkProcessList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.Suppressed.SaveBulkSqlInsert(bulkProcessList, client);
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message,x.ProcessCode));
                        }
                    }
                    counter = 0;
                    bulkProcessList = new List<Entity.Suppressed>();
                }
            }

            return done;
        }

        public int PerformSuppression(List<Entity.SubscriberFinal> list, KMPlatform.Object.ClientConnections client, int sourceFileId, string processCode, string suppFileName)
        {
            int suppCount = 0;
            int bLSuppCount = 0;
            int BatchSize = 250;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            List<Entity.SubscriberFinal> bulkUpdateList = new List<Entity.SubscriberFinal>();
            FrameworkUAS.BusinessLogic.FileLog flWorker = new FrameworkUAS.BusinessLogic.FileLog();
            foreach (Entity.SubscriberFinal x in list)
            {
                counter++;
                processedCount++;
                bulkUpdateList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, 0))
                    {
                        try
                        {
                            StringBuilder xml = new StringBuilder();
                            xml.AppendLine("<XML>");
                            foreach(Entity.SubscriberFinal sf in bulkUpdateList)
                            {
                                xml.AppendLine("<Entity>");

                                xml.AppendLine("<FName>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.FName) + "</FName>");
                                xml.AppendLine("<LName>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.LName) + "</LName>");
                                xml.AppendLine("<Company>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.Company) + "</Company>");
                                xml.AppendLine("<Address>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.Address) + "</Address>");
                                xml.AppendLine("<City>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.City) + "</City>");
                                xml.AppendLine("<State>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.State) + "</State>");
                                xml.AppendLine("<Zip>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.Zip) + "</Zip>");
                                xml.AppendLine("<Phone>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.Phone) + "</Phone>");
                                xml.AppendLine("<Fax>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.Fax) + "</Fax>");
                                xml.AppendLine("<Email>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(sf.Email) + "</Email>");

                                xml.AppendLine("</Entity>");
                            }
                            xml.AppendLine("</XML>");

                            bLSuppCount += DataAccess.Suppressed.PerformSuppression(xml.ToString(), client, sourceFileId, processCode, suppFileName);
                            
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
                    counter = 0;
                    bulkUpdateList = new List<Entity.SubscriberFinal>();
                }
            }
            suppCount = bLSuppCount;
            return suppCount;
        }
    }
}
