using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class IssuePermissions
    {
        private bool? _permission;
        public string Type { get; set; }
        public bool? Permission
        {
            get { return _permission; }
            set
            {
                _permission = value;

            }
        }
        public FrameworkUAD_Lookup.Enums.IssuePermissionTypes TypeEnum { get; set; }

        public IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes type, bool? permission)
        {
            this.Type = type.ToString().Replace('_', ' ');
            this.Permission = permission;
            this.TypeEnum = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //private FrameworkUAD_Lookup.Enums.IssuePermissionTypes issuePermissionTypes;
    }

    public enum TileType
    {
        //File_Status,
        Record_Update,
        Edit_File_Mapping,
        Import_File,
        Add_Remove,
        Issue_Splits,
        Import_Comps
    }

    public class OpenClose
    {
        public OpenClose()
        {
            Products = new List<string>();
            Products.Add("abc");
            Products.Add("UAD");
            Product = string.Empty;
            CurrentIssue = string.Empty;
            LastUpdated = DateTime.Now;
            DateCreated = DateTime.Now;
            Files = new List<UAS.Web.Models.Circulations.FileStatus>();
        }
        public List<string> Products { get; set; }
        public string Product { get; set; }
        public string CurrentIssue { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public List<UAS.Web.Models.Circulations.FileStatus> Files { get; set; }

    }
}