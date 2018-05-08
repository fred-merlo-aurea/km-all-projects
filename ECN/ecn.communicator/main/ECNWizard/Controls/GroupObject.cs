using System;
using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.ECNWizard.Controls
{
    [Serializable]
    public class GroupObject
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<CampaignItemBlastFilter> filters { get; set; }

    }
}