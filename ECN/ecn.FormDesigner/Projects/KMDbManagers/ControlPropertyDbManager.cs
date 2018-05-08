using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class ControlPropertyDbManager : DbManagerBase
    {
        private const string LabelPropertyName = "Values";
        private const string RequiredPropertyName = "Required";

        public IEnumerable<ControlProperty> GetAll()
        {
            return KM.ControlProperties.ToList();
        }

        public ControlProperty GetValuePropertyByType(int type)
        {
            return GetPropertyByNameAndType(LabelPropertyName, type);
        }

        public ControlProperty GetValuePropertyByControl(Control c)
        {
            return GetPropertyByNameAndControl(LabelPropertyName, c);
        }

        public ControlProperty GetRequiredPropertyByControl(Control c)
        {
            return GetPropertyByNameAndControl(RequiredPropertyName, c);
        }
        
        public ControlProperty GetPropertyByNameAndType(string name, int type)
        {
            return KM.ControlProperties.SingleOrDefault(x => x.Type_ID == type && x.PropertyName == name);
        }

        public ControlProperty GetPropertyByNameAndControl(string name, Control c)
        {
            ControlType type = c.ControlType;
            if (type == null)
            {
                type = KM.ControlTypes.Single(x => x.ControlType_Seq_ID == c.Type_Seq_ID);
            }

            ControlProperty res = GetPropertyByNameAndType(name, type.MainType_ID.HasValue ? type.MainType_ID.Value : type.ControlType_Seq_ID);
            if (res == null && type.MainType_ID.HasValue)
            {
                res = GetPropertyByNameAndType(name, c.Type_Seq_ID);
            }

            return res;
        }
    }
}