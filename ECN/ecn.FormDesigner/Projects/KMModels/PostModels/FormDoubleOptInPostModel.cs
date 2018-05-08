using KMEntities;
using KMEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.PostModels
{
    public class FormDoubleOptInPostModel : PostModelBase
    {
        public int Id { get; set; }

        public DOINotificationModel Notification { get; set; }

        [Required(ErrorMessage = "Landing Page cannot be empty")]
        public string Page
        { 
            get
            {
                return Notification.LandingPage;
            }
            set
            {
                Notification.LandingPage = value;
            }
        }
    }
}