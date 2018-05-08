using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface INewUserProxy
    {
        IList<NewUser> Get(int month, int year, bool isTestBlast);
    }
}
