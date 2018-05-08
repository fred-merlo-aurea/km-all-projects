using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace UAS.Web.Models.FileTools
{
    public class FileViewer
    {
        public List<dynamic> Data { get; set; }
        public Dictionary<string, System.Type> Bind { get; set; }
        public List<string> ColumnDelimiter { get; set; }
        public List<string> TrueFalseOptions { get; set; }
    }
}