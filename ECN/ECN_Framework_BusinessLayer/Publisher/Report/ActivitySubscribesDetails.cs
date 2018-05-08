﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivitySubscribesDetails
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivitySubscribesDetails> GetList(int editionID, int blastID)
        {
            return ECN_Framework_DataLayer.Publisher.Report.ActivitySubscribesDetails.GetList(editionID, blastID);
        }
    }
}
