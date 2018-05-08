using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterSchedule
    {
        public  bool ExistsByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            return DataAccess.FilterSchedule.ExistsByFilterID(clientconnection,filterID);
        }

        public  bool ExistsByFilterSegmentationID(KMPlatform.Object.ClientConnections clientconnection, int filterSegmentationID)
        {
            return DataAccess.FilterSchedule.ExistsByFilterSegmentationID(clientconnection, filterSegmentationID);
        }

        public  bool ExistsByFileName(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID, string fileName)
        {
            return DataAccess.FilterSchedule.ExistsByFileName(clientconnection, filterScheduleID, fileName);
        }

        public  List<Entity.FilterSchedule> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int BrandID, bool IsFilterSegmentation)
        {
            var retList = DataAccess.FilterSchedule.GetByBrandID(clientconnection, BrandID, IsFilterSegmentation);
            return retList;
        }

        public  List<Entity.FilterSchedule> GetByBrandIDUserID(KMPlatform.Object.ClientConnections clientconnection, int BrandID, int UserID, bool IsFilterSegmentation)
        {
            var retList = DataAccess.FilterSchedule.GetByBrandIDUserID(clientconnection, BrandID, UserID, IsFilterSegmentation);
            return retList;
        }

        private  List<int> GetFilterGroupID(string FilterGroupIDs)
        {
            return FilterGroupIDs.Split(',').Select(n => int.Parse(n)).ToList();
        }

        public  Entity.FilterSchedule GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterscheduleID)
        {
            var retList = DataAccess.FilterSchedule.GetByID(clientconnection,filterscheduleID);

            return retList;
        }

        public  List<Entity.FilterSchedule> GetScheduleByDateTime(KMPlatform.Object.ClientConnections clientconnection, string dt, string time)
        {
            var retList = DataAccess.FilterSchedule.GetScheduleByDateTime( clientconnection,  dt,  time);
            
            return retList;
        }

        public  int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.FilterSchedule filterSchedule)
        {
            return DataAccess.FilterSchedule.Save(clientconnection, filterSchedule);
        }

        public  void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID, int userID)
        {
            DataAccess.FilterSchedule.Delete(clientconnection, filterScheduleID, userID);
        }
    }
}
