using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.DataCompare.Web.Models
{
    public class ViewComparisionViewModel
    {
        public ViewComparisionViewModel()
        {
         
            DcDownloadId = 0;
            TotalRecords = 0;
            FileName = "";
            Target = "";
            Scope = "";
            DownLoadDate = null;
            User = "";
            Query = "";
            DownloadedFileName = "";
            Type = "";
            Price = "$0.00";
        }

        public int DcDownloadId { get; set; }
        public int TargetId { get; set; }
        public int? ScopeId { get; set; }
        public int TypeId { get; set; }
        public string FileName { get; set; }
        public string Target { get; set; }
        public string Scope { get; set; }
        public string Type { get; set; }
        public DateTime? DownLoadDate { get; set; }
        public string User { get; set; }
        public string Query { get; set; }
        public int TotalRecords { get; set; }
        public string DownloadedFileName { get; set; }
        public string Price { get; set; }


    }
}