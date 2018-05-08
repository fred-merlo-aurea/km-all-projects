using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.BusinessLogic
{
    public class FpsStandardRule : LogicHelper
    {
        public List<Entity.FpsStandardRule> Select()
        {
            try
            {
                return DataAccess.FpsStandardRule.Select();
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".Select");
                return new List<Entity.FpsStandardRule>();
            }
        }
        public int Save(Entity.FpsStandardRule fpsStandardRule)
        {
            try
            {
                return DataAccess.FpsStandardRule.Save(fpsStandardRule);
            }
            catch (Exception ex)
            {
                string json = string.Empty;
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                json = jf.ToJson<Entity.FpsStandardRule>(fpsStandardRule);
                LogError(ex, this.GetType().Name.ToString() + ".Save", "FpsStandarRule: " + json);
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
                DataAccess.FpsStandardRule.DeleteAll();
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".DeleteAll");
            }
        }
    }
}
