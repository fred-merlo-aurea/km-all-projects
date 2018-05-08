using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KMWeb.Models.Forms
{
    public class AddGroupModel
    {
        public int CustomerId { get; set; }

        public int? FolderId { get; set; }

        [Required(ErrorMessage = "Group Name is required")]
        public string GroupName { get; set; }
    }
}