using System;
using KMEntities;
using System.Collections.Generic;
using System.Linq;

namespace KMDbManagers
{
    public class NewsletterGroupUDFDbManager : DbManagerBase
    {
        public void Add(NewsletterGroupUDF newP)
        {
            KM.NewsletterGroupUDFs.Add(newP);
        }

        public void RemoveAll(int groupId)
        {
            RemoveAllExcept(groupId, new int[0]);
        }

        public void RemoveAllExcept(int groupId, IEnumerable<int> except)
        {
            IEnumerable<NewsletterGroupUDF> items = KM.NewsletterGroupUDFs.Where
                                                    (x => x.NewsletterGroup.GroupID == groupId && !except.Contains(x.NewsletterGroupUDFID))
                                                    .ToList();
            foreach (var i in items)
            {
                KM.NewsletterGroupUDFs.Remove(i);
            }
        }
    }
}