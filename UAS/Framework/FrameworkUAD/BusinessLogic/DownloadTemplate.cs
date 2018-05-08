using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class DownloadTemplate
    {
        public static List<Entity.DownloadTemplate> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.DownloadTemplate> x = null;
            x = DataAccess.DownloadTemplate.GetAll(clientconnection);
            return x;
        }

       
        public static Entity.DownloadTemplate GetByID(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            Entity.DownloadTemplate x = null;
            x = DataAccess.DownloadTemplate.GetByID(clientconnection, DownloadTemplateID);
            return x;
        }

        public static List<Entity.DownloadTemplate> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int BrandID)
        {
            List<Entity.DownloadTemplate> x = null;
            x = DataAccess.DownloadTemplate.GetByBrandID(clientconnection, BrandID);
            return x;
        }

        public static List<Entity.DownloadTemplate> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.DownloadTemplate> x = null;
            x = DataAccess.DownloadTemplate.GetNotInBrand(clientconnection);
            return x;
        }
        public static List<Entity.DownloadTemplate> GetByPubIDBrandID(KMPlatform.Object.ClientConnections clientconnection, int PubID, int BrandID)
        {
            List<Entity.DownloadTemplate> x = null;
            x = DataAccess.DownloadTemplate.GetByPubIDBrandID(clientconnection, PubID,BrandID);
            return x;
        }

        public static bool ExistsByDownloadTemplateName(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID, string DownloadTemplateName)
        {
            return DataAccess.DownloadTemplate.ExistsByDownloadTemplateName(clientconnection, DownloadTemplateID, DownloadTemplateName);
        }

      

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.DownloadTemplate t)
        {
            return DataAccess.DownloadTemplate.Save(clientconnection,t);
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID, int UserID)
        {
             DataAccess.DownloadTemplate.Delete(clientconnection, DownloadTemplateID, UserID);
        }
        #endregion
    }
}
