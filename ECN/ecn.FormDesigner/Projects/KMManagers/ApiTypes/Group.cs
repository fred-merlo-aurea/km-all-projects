using System;

namespace KMManagers.APITypes
{
    public class Group : CustomerRelationBase
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int FolderID { get; set; }
        public string GroupDescription { get; set; }
    }
}