using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMEntities;

namespace KMModels
{
    public class ControlModel : ModelBase, IItemed
    {
        [GetFromField("Control_ID")]
        public int Id { get; set; }
        [GetFromField("Form_Seq_ID")]
        public int FormId { get; set; }
        public string FieldLabel { get; set; }
              
        public KMEnums.ControlType Type { get; set; }
        public KMEnums.DataType DataType { get; set; }

        public ControlTypeModel Control_Type { get; set; }
        public int? SelectableItemId { get; set; }
        public string SelectableLabel { get; set; }
        public int FieldID { get; set; }
        public string KMPaidQueryString { get; set; }
        public SelectableItem[] SelectableItems { get; set; }

        
       public IItemed GetItem(SelectableItem item)
        {
            ControlModel res = (ControlModel)MemberwiseClone();
            res.SelectableItemId = item.Id;
            res.SelectableLabel = item.Label;

            return res;
        }

        public override void FillData(object entity)
        {
            base.FillData(entity);
            Control control = (Control)entity;
            ControlType c_type = control.ControlType;
            Type = (KMEnums.ControlType)(c_type.MainType_ID.HasValue ? c_type.MainType_ID.Value : control.Type_Seq_ID);
            Control_Type = c_type.ConvertToModel<ControlTypeModel>();
            DataType = KMEnums.DataType.Text;
            FieldID = control.FieldID.HasValue ? (int)control.FieldID : 0 ;
            if (SelectableItem.TypeIsSelectable(Type))
            {
                if (Type == KMEnums.ControlType.NewsLetter)
                {
                    DataType = KMEnums.DataType.Selection;
                    var values = control.NewsletterGroups;
                    SelectableItems = new SelectableItem[values.Count];
                    int i = 0;
                    foreach (var v in values)
                    {
                        ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(v.GroupID);
                        SelectableItems[i] = new SelectableItem();
                        SelectableItems[i].Id = v.GroupID;
                        SelectableItems[i].Label = g.GroupName;
                        i++;
                    }
                }
                else
                {
                    DataType = KMEnums.DataType.Selection;
                    var values = control.FormControlPropertyGrids;
                    SelectableItems = new SelectableItem[values.Count];
                    int i = 0;
                    foreach (var v in values)
                    {
                        SelectableItems[i] = new SelectableItem();
                        SelectableItems[i].Id = v.FormControlPropertyGrid_Seq_ID;
                        SelectableItems[i].Label = string.IsNullOrEmpty(v.DataText) ? v.DataValue : v.DataText;
                        i++;
                    }
                }
            }
            //else
            //{
            //    if (Type == KMEnums.ControlType.NewsLetter)
            //    {
            //        DataType = KMEnums.DataType.Newsletter;
            //    }
            //}
        }

        public void SetDataType(string result)
        {
            var textboxDataType = (KMEnums.TextboxDataTypes)int.Parse(result);
            switch (textboxDataType)
            {
                case KMEnums.TextboxDataTypes.Number:
                    DataType = KMEnums.DataType.Number;
                    break;
                case KMEnums.TextboxDataTypes.Decimal:
                    DataType = KMEnums.DataType.Decimal;
                    break;
                case KMEnums.TextboxDataTypes.Date:
                    DataType = KMEnums.DataType.Date;
                    break;
            }
        }
    }
}