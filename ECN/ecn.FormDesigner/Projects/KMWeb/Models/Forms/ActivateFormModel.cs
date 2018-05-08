using KMEnums;
using KMModels.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.Models.Forms
{
    public class ActivateFormModel
    {
        public int Id { get; set; }

        public FormActive State { get; set; }

        [RequiredIf("State", FormActive.UseActivationDates, "From Activation Date Required")]
        public DateTime? From { get; set; }

        [RequiredIf("State", FormActive.UseActivationDates, "To Activation Date Required")]
        public DateTime? To { get; set; }
    }
}