using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Site.Models
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel()
        {
            UserName = "";
            Exists = true;
            EmailSent = false;
        }
        
        [Required(ErrorMessage="UserName is required")]
        public string UserName { get; set; }

        public bool Exists { get; set; }

        public bool EmailSent { get; set; }
    }
}