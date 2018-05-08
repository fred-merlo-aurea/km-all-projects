using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ecn.activity.Areas.User.Models
{
    public class UserAcceptModel
    {
        public UserAcceptModel()
        {
            UserID = -1;
            UserName = string.Empty;
            Password = string.Empty;
            ReTypePassword = string.Empty;
            NewUser = true;
            setID = null;
            SecurityGroupsToAccept = new List<KMPlatform.Entity.SecurityGroupOptIn>();
            CreatedByUserID = -1;
            ValidForAccepting = true;
            PlatformLoginURL = string.Empty;
            ErrorMessage = string.Empty;
            IsExistingUser = false;
            IsSysAdmin = false;
            UserIsLocked = false;
        }

        #region properties
        public int UserID { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
        public string ReTypePassword { get; set; }

        public bool NewUser { get; set; }

        public Guid? setID { get; set; }

        public List<KMPlatform.Entity.SecurityGroupOptIn> SecurityGroupsToAccept { get; set; }

        public int CreatedByUserID { get; set; }

        public bool ValidForAccepting { get; set; }

        public string PlatformLoginURL { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsExistingUser { get; set; }

        public bool IsSysAdmin { get; set; }

        public bool UserIsLocked { get; set; }
        #endregion
    }
}