using System;

namespace KMManagers.APITypes
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string AccessKey { get; set; }
        public bool hasChildren { get { return true; } }
    }
}