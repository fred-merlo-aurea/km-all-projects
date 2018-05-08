using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class CampaignFilter
    {
        #region Data
        public  List<Entity.CampaignFilter> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.CampaignFilter> retList = new List<Entity.CampaignFilter>();
            retList = FrameworkUAD.DataAccess.CampaignFilter.Get(clientconnection);
            return retList;
        }

        public  Entity.CampaignFilter GetByID(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID)
        {
            Entity.CampaignFilter cf = FrameworkUAD.DataAccess.CampaignFilter.GetByID(clientconnection, CampaignFilterID);
            return cf;
        }

        public  List<Entity.CampaignFilter> GetByCampaignID(KMPlatform.Object.ClientConnections clientconnection, int CampaignID)
        {
            List<Entity.CampaignFilter> retList = new List<Entity.CampaignFilter>();
            retList = FrameworkUAD.DataAccess.CampaignFilter.GetByCampaignID(clientconnection, CampaignID);
            return retList;
        }

        public  int CampaignFilterExists(KMPlatform.Object.ClientConnections clientconnection, string filtername, int CampaignID)
        {

            return FrameworkUAD.DataAccess.CampaignFilter.CampaignFilterExists(clientconnection, filtername, CampaignID);
        }
        #endregion

        #region CRUD
        public  int Insert(KMPlatform.Object.ClientConnections clientconnection, string filtername, int UserID, int CampaignID, string PromoCode)
        {

            return FrameworkUAD.DataAccess.CampaignFilter.Insert(clientconnection, filtername, UserID,  CampaignID,  PromoCode);
        }

        public  void Delete(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID)
        {
             FrameworkUAD.DataAccess.CampaignFilter.Delete(clientconnection, CampaignFilterID);
        }
        #endregion
    }
}
