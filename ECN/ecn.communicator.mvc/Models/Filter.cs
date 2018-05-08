using ECN_Framework_BusinessLayer.Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class Filter : ECN_Framework_Entities.Communicator.Filter
    {
        public Filter() : base()
        {
            FilterID = -1;
        }

        public Filter(ECN_Framework_Entities.Communicator.Filter f)
        {
            FilterID = -1;
            CustomerID = null;
            GroupID = null;
            FilterName = string.Empty;
            WhereClause = string.Empty;
            DynamicWhere = string.Empty;
            GroupCompareType = string.Empty;
            CreatedDate = null;
            UpdatedDate = null;
            CreatedUserID = null;
            UpdatedUserID = null;
            IsDeleted = null;
            FilterGroupList = null;
            Archived = false;
        }
    }
}