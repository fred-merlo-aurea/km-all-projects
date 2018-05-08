using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class NewUserProxy : INewUserProxy
    { 
        public IList<NewUser> Get(int month, int year, bool isTestBlast)
        {
            return NewUser.Get(month, year, isTestBlast);
        }
    }
}
