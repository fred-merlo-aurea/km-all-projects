using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class File
    {
        public File()
        {
            Name = string.Empty;
        }
        public File(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}