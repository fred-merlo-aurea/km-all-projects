using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EmailPreview
{
    [Serializable]
    [DataContract]
    public class EmailTest
    {
        public EmailTest() { Error = false; ErrorMessage = "";Id = 0; }
        #region Properties
        [DataMember]
        public List<TestingApplication> TestingApplications { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string InboxGuid { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Html { get; set; }
        [DataMember]
        public string ZipFile { get; set; }
        [DataMember]
        public string TestType { get; set; }
        [DataMember]
        public bool? Sandbox { get; set; }
        [DataMember]
        public string UserGuid { get; set; }
        [DataMember]
        public bool? Error { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        #endregion
    }
}
