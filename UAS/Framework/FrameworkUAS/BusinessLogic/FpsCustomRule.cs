using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.BusinessLogic
{
    public class FpsCustomRule : LogicHelper
    {
        public List<Entity.FpsCustomRule> SelectClientId(int clientId)
        {
            try
            {
                return DataAccess.FpsCustomRule.SelectClientId(clientId);
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".SelectClientId");
                return new List<Entity.FpsCustomRule>();
            }
        }
        public List<Entity.FpsCustomRule> SelectSourceFileId(int sourceFileId)
        {
            try
            {
                return DataAccess.FpsCustomRule.SelectSourceFileId(sourceFileId);
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".SelectSourceFileId");
                return new List<Entity.FpsCustomRule>();
            }
        }
        public int Save(Entity.FpsCustomRule fpsCustomRule)
        {
            try
            {
                return DataAccess.FpsCustomRule.Save(fpsCustomRule);
            }
            catch (Exception ex)
            {
                string json = string.Empty;
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                json = jf.ToJson<Entity.FpsCustomRule>(fpsCustomRule);
                LogError(ex, this.GetType().Name.ToString() + ".Save", "FpsCustomRule: " + json);
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
                DataAccess.FpsCustomRule.DeleteAll();
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".DeleteAll");
            }
        }
    }
}
