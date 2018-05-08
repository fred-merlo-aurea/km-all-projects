using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMEntities;
using KMEnums;

namespace KMModels
{
    public class ConditionModel : ModelBase
    {
        public ConditionType Type { get; set; }

        [GetFromField("Control_ID")]
        public int ControlId { get; set; }

        [GetFromField("Operation_ID")]
        public ComparisonType ComparisonType { get; set; }

        public string Value { get; set; }

        public bool IsSelectableItem { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);
            Condition c = (Condition)entity;
            IsSelectableItem = SelectableItem.ControlIsSelectable(c.Control);
        }
    }
}
