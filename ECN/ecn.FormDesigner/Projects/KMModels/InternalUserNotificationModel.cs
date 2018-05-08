using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels
{
    public class InternalUserNotificationModel : ConditionalNotificationModel
    {
        [Required(ErrorMessage = EmailIsEmpty)]
        [RegularExpression(EmailRex, ErrorMessage = EmailIsIncorrect)]
        public string ToEmail { get; set; }
    }
}
