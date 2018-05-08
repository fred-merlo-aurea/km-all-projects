using System;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ApplicationSetting
    {
       
        public int Save(Entity.ApplicationSetting x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ApplicationSettingID = DataAccess.ApplicationSetting.Save(x);
                scope.Complete();
            }

            return x.ApplicationSettingID;
        }
    }
}
