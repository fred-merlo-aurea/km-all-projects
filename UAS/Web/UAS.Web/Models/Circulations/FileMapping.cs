using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class FileMapping
    {
        public FileMapping()
        {
            Clients = new List<string>();
        }
        public List<string> Clients { get; set; }
        public string Client { get; set; }
        public List<string> Files { get; set; }
        public string File { get; set; }

    }
}