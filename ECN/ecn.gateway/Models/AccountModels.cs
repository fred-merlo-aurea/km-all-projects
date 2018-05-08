using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using ECN_Framework_Entities.Communicator;

namespace ecn.gateway.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }


    public class UserProfile
    {
        [Key]

        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class LoginModel
    {
        
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z|0-9]{2,63})(\]?)$",ErrorMessage="Invalid Email Address")]
        [Required]
        [Display(Name = "Email Address")]
        public string EMail { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        public string GatewayVSProfile { get; set; }

        public Gateway Gateway { get; set; }
    
    }

    public class ConfirmationModel
    {
        [Display(Name = "Your username and password have been confirmed.")]
        public string ConfirmationMessage { get; set; }
        public Gateway Gateway { get; set; }
        public bool HasError { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class SendPasswordModel
    {
        [Required]
        [Display(Name = "Forgot password or password not working? Type in your email address and click submit. Your password will be emailed to you.")]
        public string ForgotPasswordMessage { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z|0-9]{2,63})(\]?)$", ErrorMessage="Invalid Email Address")]
        [Required]
        [Display(Name = "Email Address")]
        public string EMail { get; set; }

        [Required]
        public string UserPassword { get; set; }

        public string SuccessMessage { get; set; }
    }
}
