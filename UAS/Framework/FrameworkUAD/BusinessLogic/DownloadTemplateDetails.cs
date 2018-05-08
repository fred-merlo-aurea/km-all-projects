using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class DownloadTemplateDetails
    {
        #region Data
        public static List<Entity.DownloadTemplateDetails> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.DownloadTemplateDetails> x = null;
            x = DataAccess.DownloadTemplateDetails.GetAll(clientconnection);
            return x;
        }

       
        public static List<Entity.DownloadTemplateDetails> GetByDownloadTemplateID(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            List<Entity.DownloadTemplateDetails> x = null;
            x = DataAccess.DownloadTemplateDetails.GetByDownloadTemplateID(clientconnection, DownloadTemplateID);
            return x;
        }

        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.DownloadTemplateDetails t)
        {
            return DataAccess.DownloadTemplateDetails.Save(clientconnection, t);
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
             DataAccess.DownloadTemplateDetails.Delete(clientconnection, DownloadTemplateID);
        }
        #endregion
    }
}
