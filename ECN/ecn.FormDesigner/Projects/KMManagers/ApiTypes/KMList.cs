using System;

namespace KMManagers.APITypes
{
    public class KMList<T>
    {
        public T ApiObject { get; set; }
        public string Location { get; set; }
    }
}
