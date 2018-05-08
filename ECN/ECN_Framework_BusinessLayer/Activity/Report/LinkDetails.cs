﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class LinkDetails
    {
        public static List<ECN_Framework_Entities.Activity.Report.LinkDetails> Get(int customerID, string linkownerID, string linktypeID, DateTime fromdate, DateTime todate, string campaignID)
        {
            return ECN_Framework_DataLayer.Activity.Report.LinkDetails.Get(customerID, linkownerID, linktypeID, fromdate, todate, campaignID);
        }
    }
}
