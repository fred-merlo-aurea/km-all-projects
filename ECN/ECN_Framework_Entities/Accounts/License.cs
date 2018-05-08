using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects.Accounts;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class License
    {
        public License() 
        {
            LicenseOption = Enums.LicenseOption.notavailable;
            Allowed = 0;
            Available = 0;
            Used = 0;
        }

        public ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption LicenseOption { get; set; }

        [DataMember]
        public int Allowed { get; set; }
        [DataMember]
        public int Available { get; set; }
        [DataMember]
        public int Used { get; set; }
        

    }
}
