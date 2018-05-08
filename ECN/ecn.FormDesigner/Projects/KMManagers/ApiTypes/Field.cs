using System;

namespace KMManagers.APITypes
{
    public class Field
    {
        public int GroupDataFieldsID { get; set; }
        public int GroupID { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string IsPublic { get; set; }
    }
}