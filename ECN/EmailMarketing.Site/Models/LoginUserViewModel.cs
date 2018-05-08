using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Site.Models
{
    public class LoginUserViewModel
    {
        public const bool PersistByDefault = true;
        public const bool NoRoles = false;
        public const bool IsLocked = false;
        public const bool IsDisabled = false;
        public const bool InvalidUP = false;
        public const bool NoActiveClients = false;
        [Required(ErrorMessage="Please enter your User name")]
        [DataType(DataType.Text)]
        public string User { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DefaultValue(PersistByDefault)]
        public bool Persist { get; set; }

        [DefaultValue(NoRoles)]
        public bool NoActiveRoles { get; set; }

        [DefaultValue(IsLocked)]
        public bool UserIsLocked { get; set; }

        [DefaultValue(IsDisabled)]
        public bool UserIsDisabled { get; set; }

        [DefaultValue(InvalidUP)]
        public bool InvalidUsername_Password { get; set; }

        [DefaultValue(NoActiveClients)]
        public bool NoActive_Clients { get; set; }
    }
}