using System;
using KMEntities;
using System.Collections.Generic;
using System.Linq;

namespace KMDbManagers
{
    public class ControlCategoryDbManager : DbManagerBase
    {
        public ControlCategory GetByID(int id)
        {
            return KM.ControlCategories.SingleOrDefault(x => x.ControlCategoryID == id);
        }

        public ControlCategory GetByName(int controlId, string name)
        {
            return KM.ControlCategories.SingleOrDefault(x => x.LabelHTML == name && x.Control_ID == controlId);
        }

        public void Add(ControlCategory newP)
        {
            KM.ControlCategories.Add(newP);
        }

        public void RemoveAll(int controlId)
        {
            RemoveAllExcept(controlId, new int[0]);
        }

        public void RemoveAllExcept(int controlId, IEnumerable<int> except)
        {
            IEnumerable<ControlCategory> items = KM.ControlCategories.Where
                                                    (x => x.Control_ID == controlId && !except.Contains(x.ControlCategoryID))
                                                    .ToList();
            foreach (var i in items)
            {
                KM.ControlCategories.Remove(i);
            }
        }
    }
}