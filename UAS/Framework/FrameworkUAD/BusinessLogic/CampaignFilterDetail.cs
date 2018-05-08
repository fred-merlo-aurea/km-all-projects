using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class CampaignFilterDetail
    {
        #region Data
        public  List<Entity.CampaignFilterDetail> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            var retList = FrameworkUAD.DataAccess.CampaignFilterDetail.Get(clientconnection);
            return retList;
        }
        #endregion

        #region CRUD
        public  void saveCampaignDetails(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID, string xmlSubscriber)
        {
            FrameworkUAD.DataAccess.CampaignFilterDetail.saveCampaignDetails(clientconnection, CampaignFilterID, xmlSubscriber);

        }
        #endregion
    }
}
