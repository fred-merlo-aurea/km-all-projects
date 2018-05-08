using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECNBusiness = ECN_Framework_BusinessLayer;
using ECNEntity = ECN_Framework_Entities;
using ECNCommon = ECN_Framework_Common;
using System.Configuration;
using System.Data;

namespace ECN_Framework_UnitTests
{
    /// <summary>
    /// Summary description for SmartSegment
    /// </summary>
    [TestClass]
    public class SmartSegment
    {
        [TestMethod]
        public void GetBySmartSegmentID()
        {

            ECNEntity.Communicator.SmartSegment ss = ECNBusiness.Communicator.SmartSegment.GetSmartSegmentByID(1);
            Assert.IsNotNull(ss, "No SmartSegment");
        }

        [TestMethod]
        public void GetByOldID()
        {
            int? ID = null;
            ID = ECNBusiness.Communicator.SmartSegment.GetNewIDFromOldID(2147483647);
            Assert.IsNotNull(ID, "No SmartSegment");
        }
    }
}
