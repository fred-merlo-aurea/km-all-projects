using System;
using KMEntities;
using System.Collections.Generic;
using System.Linq;

namespace KMDbManagers
{
    public class NewsletterGroupDbManager : DbManagerBase
    {
        public void Add(NewsletterGroup newP)
        {
            KM.NewsletterGroups.Add(newP);
        }

        public void RemoveAll(int controlId)
        {
            RemoveAllExcept(controlId, new int[0]);
        }

        public void RemoveAllExcept(int controlId, IEnumerable<int> except)
        {
            IEnumerable<NewsletterGroup> items = KM.NewsletterGroups.Where
                                                    (x => x.Control_ID == controlId && !except.Contains(x.GroupID))
                                                    .ToList();
            foreach (var i in items)
            {
                KM.NewsletterGroups.Remove(i);
            }
        }
    }
}