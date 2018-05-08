using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class CssClassDbManager : DbManagerBase
    {
        public void Add(string style)
        {
            CssClass css = new CssClass();
            css.Name = style.Trim();
            KM.CssClasses.Add(css);
        }

        public IEnumerable<CssClass> GetAll()
        {
            return KM.CssClasses.ToList();
        }
    }
}