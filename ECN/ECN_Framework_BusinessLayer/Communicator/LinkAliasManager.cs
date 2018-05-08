using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LinkAliasManager : ILinkAliasManager
    {
        public bool Exists(int customerId)
        {
            return LinkAlias.Exists(customerId);
        }

        public bool Exists(int contentId, int customerId)
        {
            return LinkAlias.Exists(contentId, customerId);
        }

        public bool Exists(int layoutId, string link)
        {
            return LinkAlias.Exists(layoutId, link);
        }
    }
}
