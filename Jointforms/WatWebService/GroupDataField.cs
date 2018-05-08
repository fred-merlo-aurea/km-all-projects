using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WattWebService
{
    public class GroupDataField
    {
        public GroupDataField()
        {
            GroupDataFieldsID = -1;
            GroupID = -1;
            ShortName = string.Empty;
            LongName = string.Empty;
            SurveyID = -1;
            DataFieldSetID = -1;
            IsPublic = "N";
            IsPrimaryKey = false;
        }
        public int GroupDataFieldsID { get; set; }
        public int GroupID { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public int SurveyID { get; set; }
        public int? DataFieldSetID { get; set; }
        public string IsPublic { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}