using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.ActivityImport.Entity
{
    [DataContract]
    [Serializable]
    public class TopicFile
    {
        public TopicFile() { }

        public string Pubcode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ActivityDateTime { get; set; }
        public string TopicCode { get; set; }
        public string TopicCodeText { get; set; }
    }
}
