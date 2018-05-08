using System;
using KMEntities;

namespace KMModels
{
    public class SelectableItem
    {
        public int Id;
        public string Label;

        public static bool ControlIsSelectable(Control c)
        {
            ControlType c_type = c.ControlType;
            KMEnums.ControlType type = (KMEnums.ControlType)(c_type.MainType_ID.HasValue ? c_type.MainType_ID.Value : c.Type_Seq_ID);

            return TypeIsSelectable(type);
        }

        public static bool TypeIsSelectable(KMEnums.ControlType type)
        {
            return type == KMEnums.ControlType.CheckBox || type == KMEnums.ControlType.RadioButton ||
                    type == KMEnums.ControlType.DropDown || type == KMEnums.ControlType.ListBox || type == KMEnums.ControlType.NewsLetter;
        }
    }
}