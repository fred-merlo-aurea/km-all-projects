using KMEntities;
using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KMModels
{
    public class NotificationModel : ModelBase
    {
        protected const string EmailRex = "^(([a-zA-Z0-9_\\-\\.+]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-+]+\\.)+))([a-zA-Z+]{2,4}|[0-9]{1,3})(\\]?)(\\s*,\\s*|\\s*$))*$"; //"^[A-z\\-_0-9\\.]+@[A-z\\-_0-9\\.]+\\.[A-z_0-9]{2,}$";
        protected const string EmailIsEmpty = "Email cannot be empty";
        protected const string EmailIsIncorrect = "Please enter valid email address(es)";

        [GetFromField("Notification_Seq_ID")]
        public int Id { get; set; }

        public EmailType Type { get; set; }

        public bool IsConfirmation
        {
            get;set;
        }

        //[RegularExpression(EmailRex, ErrorMessage = EmailIsIncorrect)]
        //public string FromEmail { get; set; }

        [Required(ErrorMessage = "From Name cannot be empty")]
        public string FromName { get; set; }

        //[Required(ErrorMessage = EmailIsEmpty)]
        //[RegularExpression(EmailRex, ErrorMessage = EmailIsIncorrect)]
        //public string ReplyEmail { get; set; }

        [GetFromField("Subject")]
        [Required(ErrorMessage = "Subject cannot be empty")]
        public string SubjectLine { get; set; }

        [Required(ErrorMessage = "Message cannot be empty")]
        public string Message { get; set; }
    }
}
