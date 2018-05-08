using System;

namespace KMModels
{
    public class ControlFieldModel : ModelBase
    {
        [GetFromField("Control_ID")]
        public int ControlId { get; set; }

        [GetFromField("FieldLabel")]
        public string ControlLabel { get; set; }

        [GetFromField("FieldID")]
        public int? FieldId { get; set; }

        public string FieldName { get; set; }
    }
}