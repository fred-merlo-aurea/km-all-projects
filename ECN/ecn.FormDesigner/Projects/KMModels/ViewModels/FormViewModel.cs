using System;
using System.Linq;
using KMEnums;
using KMEntities;

namespace KMModels.ViewModels
{
    public class FormViewModel : ModelBase
    {     
        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Name { get; set; }

        [GetFromField("FormType")]
        public String Type { get; set; }

        public String Status { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? LastUpdated { get; set; }

        public DateTime? LastPublished { get; set; }

        public FormActive Active { get; set; }

        [GetFromField("ActivationDateFrom")]
        public DateTime? ActivationFrom { get; set; }

        [GetFromField("ActivationDateTo")]
        public DateTime? ActivationTo { get; set; }

        public OptInType OptInType { get; set; }

        public FormViewModel Child { get; set; }

        [GetFromField("TokenUID")]
        public Guid TokenUID { get; set; }        

        public bool HasChild
        {
            get
            {
                return Child != null;
            }
        }

        public int? ChildId 
        { 
            get { return Child != null ? (int?)Child.Id : null; } 
        }

        public string TotalRecordCounts { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);

            var form = (Form)entity;

            var child = form.Form1.SingleOrDefault();
            if (child != null) 
            {
                Child = child.ConvertToModel<FormViewModel>();
            }
        }
    }
}