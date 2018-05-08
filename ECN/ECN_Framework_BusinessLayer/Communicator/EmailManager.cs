using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EmailManager : IEmailManager
    {
        public bool IsValidEmailAddress(string emailAddress)
        {
            return Email.IsValidEmailAddress(emailAddress);
        }
    }
}
