using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterDetails
    {
        #region Data
        public  List<Object.FilterDetails> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int FilterID)
        {
            List<Object.FilterDetails> retList =DataAccess.FilterDetails.getByFilterID(clientconnection, FilterID);
           
            return retList;
        }

        public  List<Object.FilterDetails> getByFilterGroupID(KMPlatform.Object.ClientConnections clientconnection, int FilterGroupID)
        {
            List<Object.FilterDetails> retList = DataAccess.FilterDetails.getByFilterGroupID(clientconnection, FilterGroupID);
            return retList;
        }

       
        #endregion

        #region CRUD
        public  int Save(KMPlatform.Object.ClientConnections clientconnection, Object.FilterDetails fd)
        {

            return DataAccess.FilterDetails.Save(clientconnection, fd);
        }

        public  void Delete_ByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            DataAccess.FilterDetails.Delete_ByFilterID(clientconnection,filterID);
        }
        #endregion
    }
}
