using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class AdHocDimensionGroup
    {
        public List<Entity.AdHocDimensionGroup> Select(bool includeCustomProperties = false)
        {
            List<Entity.AdHocDimensionGroup> x = null;
            x = DataAccess.AdHocDimensionGroup.Select().ToList();
            if (includeCustomProperties && x != null)
            {
                foreach (var adg in x)
                {
                    if (adg != null)
                    {
                        AdHocDimension adWorker = new AdHocDimension();
                        AdHocDimensionGroupPubcodeMap mapWorker = new AdHocDimensionGroupPubcodeMap();

                        adg.AdHocDimensions = adWorker.Select(adg.AdHocDimensionGroupId);
                        adg.DimensionGroupPubcodeMappings = mapWorker.Select(adg.AdHocDimensionGroupId);
                    }
                }
            }
            else if (x != null)
            {
                foreach (Entity.AdHocDimensionGroup adg in x)
                {
                    if (adg != null)
                    {
                        adg.AdHocDimensions = new List<Entity.AdHocDimension>();
                        adg.DimensionGroupPubcodeMappings = new List<Entity.AdHocDimensionGroupPubcodeMap>();
                    }
                }
            }
            return x;
        }
        public List<Entity.AdHocDimensionGroup> Select(int clientId, bool includeCustomProperties = false)
        {
            List<Entity.AdHocDimensionGroup> x = null;
            x = DataAccess.AdHocDimensionGroup.Select(clientId).ToList();
            if (includeCustomProperties && x != null)
            {
                foreach (var adg in x)
                {
                    if (adg != null)
                    {
                        AdHocDimension adWorker = new AdHocDimension();
                        AdHocDimensionGroupPubcodeMap mapWorker = new AdHocDimensionGroupPubcodeMap();

                        adg.AdHocDimensions = adWorker.Select(adg.AdHocDimensionGroupId);
                        adg.DimensionGroupPubcodeMappings = mapWorker.Select(adg.AdHocDimensionGroupId);
                    }
                }
            }
            else if (x != null)
            {
                foreach (Entity.AdHocDimensionGroup adg in x)
                {
                    if (adg != null)
                    {
                        adg.AdHocDimensions = new List<Entity.AdHocDimension>();
                        adg.DimensionGroupPubcodeMappings = new List<Entity.AdHocDimensionGroupPubcodeMap>();
                    }
                }
            }
            return x;
        }
        public List<Entity.AdHocDimensionGroup> Select(int clientId, string adHocDimensionGroupName, bool includeCustomProperties = false)
        {
            List<Entity.AdHocDimensionGroup> x = null;
            x = DataAccess.AdHocDimensionGroup.Select(clientId, adHocDimensionGroupName).ToList();
            if (includeCustomProperties && x != null)
            {
                foreach (var adg in x)
                {
                    if (adg != null)
                    {
                        AdHocDimension adWorker = new AdHocDimension();
                        AdHocDimensionGroupPubcodeMap mapWorker = new AdHocDimensionGroupPubcodeMap();

                        adg.AdHocDimensions = adWorker.Select(adg.AdHocDimensionGroupId);
                        adg.DimensionGroupPubcodeMappings = mapWorker.Select(adg.AdHocDimensionGroupId);
                    }
                }
            }
            else if (x != null)
            {
                foreach (Entity.AdHocDimensionGroup adg in x)
                {
                    if (adg != null)
                    {
                        adg.AdHocDimensions = new List<Entity.AdHocDimension>();
                        adg.DimensionGroupPubcodeMappings = new List<Entity.AdHocDimensionGroupPubcodeMap>();
                    }
                }
            }
            return x;
        }

        public Entity.AdHocDimensionGroup Select(int clientId, int sourceFileId, string dimensionGroupName, bool includeCustomProperties = false)
        {
            var dimensionGroup = DataAccess.AdHocDimensionGroup.Select(clientId, sourceFileId, dimensionGroupName);

            return GetDimensionGroup(includeCustomProperties, dimensionGroup);
        }
      
        public List<Entity.AdHocDimensionGroup> Select(int clientId, int sourceFileId, bool includeCustomProperties = false)
        {
            List<Entity.AdHocDimensionGroup> x = null;
            x = DataAccess.AdHocDimensionGroup.Select(clientId, sourceFileId).ToList();
            if (includeCustomProperties && x != null)
            {
                foreach (var adg in x)
                {
                    if (adg != null)
                    {
                        AdHocDimension adWorker = new AdHocDimension();
                        AdHocDimensionGroupPubcodeMap mapWorker = new AdHocDimensionGroupPubcodeMap();

                        adg.AdHocDimensions = adWorker.Select(adg.AdHocDimensionGroupId);
                        adg.DimensionGroupPubcodeMappings = mapWorker.Select(adg.AdHocDimensionGroupId);
                    }
                }
            }
            else if (x != null)
            {
                foreach (Entity.AdHocDimensionGroup adg in x)
                {
                    if (adg != null)
                    {
                        adg.AdHocDimensions = new List<Entity.AdHocDimension>();
                        adg.DimensionGroupPubcodeMappings = new List<Entity.AdHocDimensionGroupPubcodeMap>();
                    }
                }
            }
            return x;
        }

        public Entity.AdHocDimensionGroup SelectByAdHocDimensionGroupId(int dimensionGroupId, bool includeCustomProperties = false)
        {
            var dimensionGroup = DataAccess.AdHocDimensionGroup.SelectByAdHocDimensionGroupId(dimensionGroupId);

            return GetDimensionGroup(includeCustomProperties, dimensionGroup);
        }

        private Entity.AdHocDimensionGroup GetCustomProperties(FrameworkUAS.Entity.AdHocDimensionGroup adg)
        {
            AdHocDimension adWorker = new AdHocDimension();
            AdHocDimensionGroupPubcodeMap mapWorker = new AdHocDimensionGroupPubcodeMap();

            adg.AdHocDimensions = adWorker.Select(adg.AdHocDimensionGroupId);
            adg.DimensionGroupPubcodeMappings = mapWorker.Select(adg.AdHocDimensionGroupId);

            return adg;
        }
        public bool Save(Entity.AdHocDimensionGroup x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.AdHocDimensionGroup.Save(x);
                scope.Complete();
            }

            return done;
        }
        public bool SaveBulkSqlInsert(List<Entity.AdHocDimensionGroup> list)
        {
            bool done = true;
            int BatchSize = 10000;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.AdHocDimensionGroup> bulkProcessList = new List<Entity.AdHocDimensionGroup>();
            foreach (Entity.AdHocDimensionGroup x in list)
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
                            DataAccess.AdHocDimensionGroup.SaveBulkSqlInsert(bulkProcessList);
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            FrameworkUAS.BusinessLogic.FileLog fl = new FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "AdHocDimensionGroup"));
                        }
                    }
                    counter = 0;
                    bulkProcessList = new List<Entity.AdHocDimensionGroup>();
                }
            }

            return done;
        }

        private Entity.AdHocDimensionGroup GetDimensionGroup(bool includeCustomProperties, Entity.AdHocDimensionGroup dimensionGroup)
        {
            if (dimensionGroup == null)
            {
                return null;
            }

            if (includeCustomProperties)
            {
                dimensionGroup = this.GetCustomProperties(dimensionGroup);
            }
            else
            {
                dimensionGroup.AdHocDimensions = new List<Entity.AdHocDimension>();
                dimensionGroup.DimensionGroupPubcodeMappings = new List<Entity.AdHocDimensionGroupPubcodeMap>();
            }

            return dimensionGroup;
        }
    }
}
