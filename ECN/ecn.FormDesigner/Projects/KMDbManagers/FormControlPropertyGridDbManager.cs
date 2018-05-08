using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class FormControlPropertyGridDbManager : DbManagerBase
    {
        public FormControlPropertyGrid GetByID(int id)
        {
            return KM.FormControlPropertyGrids.SingleOrDefault(x => x.FormControlPropertyGrid_Seq_ID == id);
        }

        public void Add(FormControlPropertyGrid newPG)
        {
            KM.FormControlPropertyGrids.Add(newPG);
        }

        public void RemoveAll(int controlId, int propertyId)
        {
            RemoveAllExcept(controlId, propertyId, new int[0]);
        }

        public void RemoveAllExcept(int controlId, int propertyId, IEnumerable<int> except)
        {
            IEnumerable<FormControlPropertyGrid> items = KM.FormControlPropertyGrids.Where
                                                    (x => x.Control_ID == controlId && x.ControlProperty_ID == propertyId && !except.Contains(x.FormControlPropertyGrid_Seq_ID))
                                                    .ToList();
            foreach(var i in items)
            {
                KM.FormControlPropertyGrids.Remove(i);
            }
        }
    }
}