using System;
using System.Linq;


namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class FormViewModel : ModelBase
    {     
        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Name { get; set; }

        public String FormType { get; set; }

        public String Status { get; set; }

    
        public FormActive Active { get; set; }

        [GetFromField("ActivationDateFrom")]
        public DateTime? ActivationFrom { get; set; }

        [GetFromField("ActivationDateTo")]
        public DateTime? ActivationTo { get; set; }

       
        [GetFromField("TokenUID")]
        public Guid TokenUID { get; set; }        

      
        public string TotalRecordCounts { get; set; }

       
        public enum FormActive
        {
            Active = 0,
            Inactive = 1,
            UseActivationDates = 2
        }
      
    }
}