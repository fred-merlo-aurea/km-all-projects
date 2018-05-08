using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Site.Models
{
    public class ResetPasswordViewModel
    {
        public const bool NoRoles = false;
        public const bool IsLocked = false;
        public const bool IsDisabled = false;
        public const bool InvalidUP = false;
        public const bool InvalidTemp = false;
        public const bool PasswordsMustMatch = false;

        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string TempPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int UserID { get; set; }

        [DefaultValue(NoRoles)]
        public bool NoActiveRoles { get; set; }

        [DefaultValue(IsLocked)]
        public bool UserIsLocked { get; set; }

        [DefaultValue(IsDisabled)]
        public bool UserIsDisabled { get; set; }

        [DefaultValue(InvalidUP)]
        public bool InvalidUsername_Password { get; set; }

        [DefaultValue(InvalidTemp)]
        public bool InvalidTemp_Password { get; set; }

        [DefaultValue(PasswordsMustMatch)]
        public bool PasswordCompare { get; set; }
    }
}