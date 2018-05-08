using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMModels.Controls.Standard.Uncommon
{
    public class Gender : DropDown
    {
        public Gender()
        {
            Categories = new List<ControlCategory>();
            Items = new List<ListItem>();
        }

        public Gender(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Gender";
                LabelHTML = "Gender";
                Items = new List<ListItem> 
                            { 
                                new ListItem
                                    {
                                        Default = true,
                                        Value = "Male",
                                        Text = "Male",
                                        CategoryName = ""
                                    },
                                new ListItem
                                    {
                                        Value="Female",
                                        Text = "Female",
                                        CategoryName = ""
                                    }
                            };
                Categories = new List<ControlCategory>();
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Gender;
            }
        }

        public bool IsStandard
        {
            get
            {
                return true;
            }
        }

        

        //public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        //{
        //    base.Fill(control, properties);
        //    SetPopulationType(control, properties);
            
        //    //DataType = TextboxDataTypes.Text;
        //}

    }


}
