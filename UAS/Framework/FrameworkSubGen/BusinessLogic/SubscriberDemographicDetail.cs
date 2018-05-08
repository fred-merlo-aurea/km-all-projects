using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class SubscriberDemographicDetail
    {
        public bool Save(List<Entity.SubscriberDemographicDetail> list)
        {
            foreach (Entity.SubscriberDemographicDetail x in list)
                FormatData(x);
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    DataAccess.SubscriberDemographicDetail.Save(list);
                    scope.Complete();
                    done = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    done = false;
                    API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
            }
            return done;
        }
        public void FormatData(Entity.SubscriberDemographicDetail x)
        {
            try
            {
                #region truncate strings
                if (x.value != null && x.value.Length > 255)
                    x.value = x.value.Substring(0, 255);
                #endregion
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
    }
}
