using System;
using System.ComponentModel.DataAnnotations;
using KMEnums;
using KMModels.Attributes;

namespace KMModels.PostModels
{
    public abstract class FormPostModelBase : PostModelBase
    {
        protected const string Required = "Required";

        private const string GroupRequired = "Please, select Group";
        private const string NameRequired = "Form Name Required";

        private const string TooLong = "Form Name is too long";
        private const string IncorrectName = "Form Name is incorrect";
        //private const string NameRex = "^[A-z_0-9\\s]*$";
        private const int FormNameMaxLen = 100;

        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        [GetFromField("GroupID")]
        [Required(ErrorMessage = GroupRequired)]
        public int? GroupId { get; set; }

        [GetFromField("CustomerID")]
        public int? CustomerId { get; set; }

        [GetFromField("Name")]
        [Required(ErrorMessage = NameRequired)]
        [MaxLength(FormNameMaxLen, ErrorMessage = TooLong)]
        //[RegularExpression(NameRex, ErrorMessage = IncorrectName)]
        public string Name { get; set; }

        [GetFromField("Active")]
        public FormActive Active { get; set; }

        [GetFromField("ActivationDateFrom")]
        [RequiredIf("Active", FormActive.UseActivationDates, "From Activation Date Required")]
        public DateTime? ActivationFrom { get; set; }

        [GetFromField("ActivationDateTo")]
        [RequiredIf("Active", FormActive.UseActivationDates, "To Activation Date Required")]
        public DateTime? ActivationTo { get; set; }

        [GetFromField("TokenUID")]
        public Guid Token { get; set; }
    }
}