using System;
using System.ComponentModel.DataAnnotations;

namespace ecn.MarketingAutomation.Models
{
    public class DiagramPostModel
    {
        protected const string Required = "Required";
        private const string GroupRequired = "Please, select Group";
        //Bug 37074 validvalidation messages when input for fields is invalid
        private const string NameRequired = "Automation Name is required. Please enter an Automation Name.";
        private const string TooLong = "Automation Name is limited to 100 characters";// Bug 36987 - Automation Name field size limit and its Validation message
        private const string IncorrectName = "Automation Name is incorrect";
        private const int FormNameMaxLen = 100;// Bug 36987 Automation Name field size limit should be 100 char and allow any special characters

        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        [GetFromField("GroupID")]
        //[Required(ErrorMessage = GroupRequired)]
        public int? GroupId { get; set; }

        [GetFromField("CustomerID")]
        public int? CustomerId { get; set; }

        [GetFromField("Name")]
        [Required(ErrorMessage = NameRequired)]
        [MaxLength(FormNameMaxLen, ErrorMessage = TooLong)]
        public string Name { get; set; }

        [GetFromField("FormType")]
        public DiagramType Type { get; set; }

        [GetFromField("Active")]
        public DiagramType Active { get; set; }

        [GetFromField("StartFrom")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Automation Start Date is required.")]
        public DateTime? StartFrom { get; set; }

        [GetFromField("StartTo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Automation End Date is required.")]
        public DateTime? StartTo { get; set; }

        [GetFromField("Goal")]
        [Required(ErrorMessage = "Automation Goal is required.")]
        public string Goal { get; set; }
        
        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus State { get; set; }

        public string Diagram { get; set; }

        public bool IsCreate { get; set; }

        public DiagramFromTemplate FromTemplate { get; set; }

        public int? TemplateId { get; set; }

        public bool IsCopy { get; set; }
    }
}