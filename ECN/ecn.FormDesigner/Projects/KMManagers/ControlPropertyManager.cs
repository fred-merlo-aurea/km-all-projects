using System;
using System.Collections.Generic;
using KMEntities;

namespace KMManagers
{
    public class ControlPropertyManager : ManagerBase
    {
        internal IEnumerable<ControlProperty> GetAll()
        {
            return DB.ControlPropertyDbManager.GetAll();
        }

        internal ControlProperty GetValuePropertyByControl(Control c)
        {
            return DB.ControlPropertyDbManager.GetValuePropertyByControl(c);
        }

        internal ControlProperty GetRequiredPropertyByControl(Control c)
        {
            return DB.ControlPropertyDbManager.GetRequiredPropertyByControl(c);
        }

        internal ControlProperty GetPropertyByNameAndControl(string name, Control c)
        {
            return DB.ControlPropertyDbManager.GetPropertyByNameAndControl(name, c);
        }
    }
}