using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class SubscriberDemographic
    {
        public bool Save(List<Entity.SubscriberDemographic> list)
        {
            foreach (Entity.SubscriberDemographic x in list)
                FormatData(x);
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    DataAccess.SubscriberDemographic.Save(list);
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
        public void FormatData(Entity.SubscriberDemographic x)
        {
            try
            {
                #region truncate strings
                if (x.text_value != null && x.text_value.Length > 255)
                    x.text_value = x.text_value.Substring(0, 255);
                #endregion
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
    }
}
