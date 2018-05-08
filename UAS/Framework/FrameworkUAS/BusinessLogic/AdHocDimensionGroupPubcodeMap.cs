using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class AdHocDimensionGroupPubcodeMap
    {
        public List<Entity.AdHocDimensionGroupPubcodeMap> Select(int adHocDimensionGroupId)
        {
            List<Entity.AdHocDimensionGroupPubcodeMap> x = null;
            x = DataAccess.AdHocDimensionGroupPubcodeMap.Select(adHocDimensionGroupId).ToList();
            return x;
        }
        public bool Save(Entity.AdHocDimensionGroupPubcodeMap x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.AdHocDimensionGroupPubcodeMap.Save(x);
                scope.Complete();
            }

            return done;
        }
        public bool SaveBulkSqlInsert(List<Entity.AdHocDimensionGroupPubcodeMap> list)
        {
            bool done = true;
            int BatchSize = 10000;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.AdHocDimensionGroupPubcodeMap> bulkProcessList = new List<Entity.AdHocDimensionGroupPubcodeMap>();
            foreach (Entity.AdHocDimensionGroupPubcodeMap x in list)
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
                            DataAccess.AdHocDimensionGroupPubcodeMap.SaveBulkSqlInsert(bulkProcessList);
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            FrameworkUAS.BusinessLogic.FileLog fl = new FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "AdHocDimensionGroupPubcodeMap"));
                        }
                    }
                    counter = 0;
                    bulkProcessList = new List<Entity.AdHocDimensionGroupPubcodeMap>();
                }
            }

            return done;
        }
    }
}
