using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class UserDataMask
    {
        #region Data

        public static List<Entity.UserDataMask> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.UserDataMask> x = null;
            x = DataAccess.UserDataMask.GetAll(clientconnection);
            return x;
        }
       
        public static List<Entity.UserDataMask> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            List<Entity.UserDataMask> udm = DataAccess.UserDataMask.GetByUserID(clientconnection, userID);
            return udm;
        }

       #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.UserDataMask udm)
        {
            return DataAccess.UserDataMask.Save(clientconnection, udm);
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            DataAccess.UserDataMask.Delete(clientconnection, userID);
        }
        #endregion
    }
}
