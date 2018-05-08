using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class UADFilter
    {
        public  List<Object.UADFilter> GetFilterByUserIDType(KMPlatform.Object.ClientConnections clientconnection, int userID, FrameworkUAD.BusinessLogic.Enums.ViewType ViewType, int pubID, int brandID, bool IsAdmin, bool IsViewMode)
        {
            var retList = DataAccess.UADFilter.GetFilterByUserIDType(clientconnection, userID, ViewType, pubID, brandID, IsAdmin, IsViewMode);
            return retList;
        }

        public  Object.UADFilter GetByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            var x = DataAccess.UADFilter.GetByFilterID(clientconnection, filterID);
            return x;
        }

        public  List<Object.UADFilter> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Object.UADFilter> retList = null;
            retList = DataAccess.UADFilter.GetAll(clientconnection);
            return retList;
        }

        public  List<Object.UADFilter> GetNotInBrand_TypeAddedinName(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Object.UADFilter> retList = DataAccess.UADFilter.GetNotInBrand_TypeAddedinName(clientconnection);
            return retList;
        }

        public  List<Object.UADFilter> GetByBrandID_TypeAddedinName(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<Object.UADFilter> retList = DataAccess.UADFilter.GetByBrandID_TypeAddedinName(clientconnection, brandID);
            return retList;
        }

        public  List<Object.UADFilter> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return DataAccess.UADFilter.GetByBrandID(clientconnection, brandID);
        }

        public  List<Object.UADFilter> GetInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataAccess.UADFilter.GetInBrand(clientconnection);
        }

        public  List<Object.UADFilter> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataAccess.UADFilter.GetNotInBrand(clientconnection);
        }

        public  List<Object.UADFilter> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
           var retList = DataAccess.UADFilter.GetByUserID( clientconnection, userID);
           
            return retList;
        }

        public  List<Object.UADFilter> GetInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            return DataAccess.UADFilter.GetInBrandByUserID(clientconnection, userID);
        }

        public  List<Object.UADFilter> GetByBrandIDUserID(KMPlatform.Object.ClientConnections clientconnection, int brandID, int userID)
        {
            return DataAccess.UADFilter.GetByBrandIDUserID(clientconnection, brandID, userID);
        }

        public  List<Object.UADFilter> GetNotInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            return DataAccess.UADFilter.GetNotInBrandByUserID( clientconnection,  userID);
        }


        public  Object.UADFilter GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            return DataAccess.UADFilter.GetByID(clientconnection, filterID);
        }

        private static FrameworkUAD.BusinessLogic.Enums.ViewType GetViewType(string ViewType)
        {
            return (FrameworkUAD.BusinessLogic.Enums.ViewType) Enum.Parse(typeof(FrameworkUAD.BusinessLogic.Enums.ViewType), ViewType, true);
        }

        public  bool ExistsByFilterName(KMPlatform.Object.ClientConnections clientconnection, int filterID, string filterName)
        {
            return DataAccess.UADFilter.ExistsByFilterName(clientconnection, filterID, filterName);
        }
        public  bool ExistsByFilterCategoryID(KMPlatform.Object.ClientConnections clientconnection, int filterCategoryID)
        {
            return DataAccess.UADFilter.ExistsByFilterCategoryID(clientconnection, filterCategoryID);
        }

        public  bool ExistsByQuestionCategoryID(KMPlatform.Object.ClientConnections clientconnection, int questionCategoryID)
        {
            return DataAccess.UADFilter.ExistsByQuestionCategoryID(clientconnection, questionCategoryID);
        }

        public  bool ExistsQuestionName(KMPlatform.Object.ClientConnections clientconnection, int filterID, string questionName)
        {
            return DataAccess.UADFilter.ExistsQuestionName(clientconnection, filterID, questionName);
        }


        
        public  int insert(KMPlatform.Object.ClientConnections clientconnection, Object.UADFilter mdf)
        {
            return DataAccess.UADFilter.insert(clientconnection, mdf);
        }

        public  void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterID, int userID)
        {
            DataAccess.UADFilter.Delete(clientconnection, filterID, userID);
        }

    }
}
