using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterGroup
    {
        public  List<Entity.FilterGroup> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int FilterID)
        {
            var retList = DataAccess.FilterGroup.getByFilterID(clientconnection, FilterID);
           
            return retList;
        }
        public  int Save(KMPlatform.Object.ClientConnections clientconnection, int filterID, int sortOrder)
        {
            return DataAccess.FilterGroup.Save(clientconnection, filterID, sortOrder);
        }
        public  void Delete_ByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            DataAccess.FilterGroup.Delete_ByFilterID(clientconnection,  filterID);
        }
    }
}
