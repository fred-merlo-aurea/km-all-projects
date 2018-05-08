using System;

namespace FrameworkUAS.BusinessLogic
{
    public class FpsMap : LogicHelper
    {
        public int Save(Entity.FpsMap fpsMap)
        {
            try
            {
                return DataAccess.FpsMap.Save(fpsMap);
            }
            catch (Exception ex)
            {
                string json = string.Empty;
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                json = jf.ToJson<Entity.FpsMap>(fpsMap);
                LogError(ex, this.GetType().Name.ToString() + ".Save", "FpsMap: " + json);
                return 0;
            }
        }
        public bool Delete(int sourceFileId)
        {
            try
            {
                return DataAccess.FpsMap.Delete(sourceFileId);
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".Delete", "sourceFileId = " + sourceFileId.ToString());
                return false;
            }
        }
        public void DeleteAll()
        {
            try
            {
                DataAccess.FpsMap.DeleteAll();
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".DeleteAll");
            }
        }
    }
}
