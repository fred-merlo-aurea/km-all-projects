using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Salesforce.Sorting;

namespace ecn.communicator.main.Salesforce.Entity
{
    public static class ECN_Sort
    {
        public static List<ECN_Framework_Entities.Accounts.Customer> Sort_Customers(List<ECN_Framework_Entities.Accounts.Customer> list, string sortBy, SortDirection sortDir)
        {
            var isAscending = sortDir == SortDirection.Ascending;
            var utility = new EntitySort();
            return utility.Sort(list, sortBy, isAscending).ToList();
        }
    }
}