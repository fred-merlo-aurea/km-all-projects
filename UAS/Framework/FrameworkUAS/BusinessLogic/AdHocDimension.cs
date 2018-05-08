using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class AdHocDimension
    {
        public List<Entity.AdHocDimension> Select(int adHocDimensionGroupId)
        {
            List<Entity.AdHocDimension> x = null;
            x = DataAccess.AdHocDimension.Select(adHocDimensionGroupId).ToList();

            return x;
        }
        public bool Delete(int SourceFileID)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                 DataAccess.AdHocDimension.Delete(SourceFileID);
                scope.Complete();
                complete = true;
            }

            return complete;
        }
       
        public bool SaveBulkSqlInsert(List<Entity.AdHocDimension> list)
        {
            bool done = true;
            int BatchSize = 10000;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.AdHocDimension> bulkProcessList = new List<Entity.AdHocDimension>();
            foreach (Entity.AdHocDimension x in list)
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
                            DataAccess.AdHocDimension.SaveBulkSqlInsert(bulkProcessList);
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            FrameworkUAS.BusinessLogic.FileLog fl = new FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message,"AdHocDimension"));
                        }
                    }
                    counter = 0;
                    bulkProcessList = new List<Entity.AdHocDimension>();
                }
            }

            return done;
        }
    }
}
