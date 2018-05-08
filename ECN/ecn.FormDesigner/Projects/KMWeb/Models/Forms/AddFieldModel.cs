using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.Models.Forms
{
    public class AddFieldModel
    {
        public int CustomerId { get; set; }

        public int GroupId { get; set; }

        public string FieldName { get; set; }
    }
}