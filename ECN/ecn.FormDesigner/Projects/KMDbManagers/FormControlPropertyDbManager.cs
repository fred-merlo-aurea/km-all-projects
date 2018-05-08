using System;
using KMEntities;

namespace KMDbManagers
{
    public class FormControlPropertyDbManager : DbManagerBase
    {
        public void Add(FormControlProperty newP)
        {
            KM.FormControlProperties.Add(newP);
        }
    }
}