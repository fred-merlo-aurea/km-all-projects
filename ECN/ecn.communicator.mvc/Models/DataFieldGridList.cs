using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class DataFieldGridList : ECN_Framework_Entities.Communicator.GroupDataFields
    {
        public string CodeSnippet { get; set; }
        public string GroupingName { get; set; }
        public string Transactional { get; set; }

        // PostModel for AddUDF
        public bool UseDefaultValue { get; set; }
        public string DefaultType { get; set; }
        public string DefaultValue { get; set; }
        public string SystemValue { get; set; }
    }
}