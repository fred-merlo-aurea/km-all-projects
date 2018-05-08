using System;

namespace FrameworkUAS.BusinessLogic
{
    public class FpsArchive : LogicHelper
    {
        public int Save(Entity.FpsArchive fpsArchive)
        {
            try
            {
                return DataAccess.FpsArchive.Save(fpsArchive);
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".Save");
                return 0;
            }
        }

        public void DeleteAll()
        {
            try
            {
                DataAccess.FpsArchive.DeleteAll();
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString() + ".DeleteAll");
            }
        }
    }
}
