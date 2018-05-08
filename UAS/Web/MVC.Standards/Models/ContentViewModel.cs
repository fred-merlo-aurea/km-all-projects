using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Standards.Models
{
    public class ContentViewModel
    {
        public string TotalRecordCounts { get; set; }
        public string ContentTitle { get; set; }
        public string ContentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ContentURL { get; set; }
        public ContentViewModel()
        {
            TotalRecordCounts = "";
            ContentTitle = "";
            ContentType = "";
            CreatedDate = null;
            ContentURL = "";
        }
    }
}