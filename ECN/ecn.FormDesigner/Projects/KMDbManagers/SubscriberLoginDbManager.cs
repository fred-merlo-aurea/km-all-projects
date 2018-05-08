using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class SubscriberLoginDbManager : DbManagerBase
    {
        public SubscriberLogin GetByFormID(int formId)
        {
            return KM.SubscriberLogins.SingleOrDefault(x => x.FormID == formId);
        }

        public void Add(SubscriberLogin sl)
        {
            KM.SubscriberLogins.Add(sl);
        }

        public void Remove(int formId)
        {
            SubscriberLogin toRemove = KM.SubscriberLogins.SingleOrDefault(x => x.FormID == formId);
            if (toRemove != null)
            {
                KM.SubscriberLogins.Remove(toRemove);
            }
        }
    }
}
