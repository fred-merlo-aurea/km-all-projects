using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.Service.Models
{

    public class Content
    {

        public int ErrorCode { get; set; }

        public int ID { get; set; }

        public string Response { get; set; }
    }


    public class ContentTest1
    {
        public string ecnAccessAccessKey { get; set; }

        public string Title { get; set; }

        public string ContentHTML { get; set; }

        public string ContentText { get; set; }

        public int FolderID { get; set; }
    }
}