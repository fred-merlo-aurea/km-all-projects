using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMManagers.APITypes
{
    
    public class Item
    {
        public string name;
        public string id;
        public bool hasChildren;
        public KMEnums.APITypes itemType;
        public string CustomerName;
        public static IEnumerable<Item> ConvertCustomer(IEnumerable<Customer> list)
        {
            foreach (var elm in list) 
            {
                yield return new Item { name = elm.CustomerName, id = elm.CustomerID, itemType = KMEnums.APITypes.Customer, hasChildren = true, CustomerName = null };
            }
        }
        public static IEnumerable<Item> ConvertFolder(IEnumerable<Folder> list)
        {            
            foreach (var elm in list)
            {
                yield return new Item { name = elm.FolderName, id = elm.FolderID.ToString(), itemType = KMEnums.APITypes.Customer, hasChildren = true, CustomerName = elm.CustomerName };
            }
        }
    }
}
