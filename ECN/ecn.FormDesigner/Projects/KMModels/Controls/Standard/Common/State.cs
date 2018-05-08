using System;
using System.Collections.Generic;
using KMEnums;
using KMModels.Wrappers;
using System.Data.SqlClient;

namespace KMModels.Controls.Standard.Common
{
    public class State : DropDown
    {
        public State()
        {
            Categories = new List<ControlCategory>();
            Items = new List<ListItem>();
        }

        public State(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "State";
                LabelHTML = "States";
                Items = new List<ListItem> 
                            { 
                                new ListItem
                                    {
                                        Value = "States",
                                        Text = "States",
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
                return ControlType.State;
            }
        }

        //public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        //{
        //    base.Fill(control, properties);
        //    SetPopulationType(control, properties);
        //    DataType = TextboxDataTypes.Text;
        //}
    }
}