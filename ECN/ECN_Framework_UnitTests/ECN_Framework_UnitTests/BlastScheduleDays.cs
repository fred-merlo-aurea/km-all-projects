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
    /// Summary description for BlastScheduleDays
    /// </summary>
    [TestClass]
    public class BlastScheduleDays
    {
        [TestMethod]
        public void GetByBlastScheduleDaysID()
        {
            ECNEntity.Communicator.BlastScheduleDays bsd = ECNBusiness.Communicator.BlastScheduleDays.GetByBlastScheduleDaysID(314, false);
            Assert.IsNotNull(bsd, "No BlastScheduleDays with this ID");
        }
        [TestMethod]
        public void GetByBlastScheduleID()
        {
            List<ECNEntity.Communicator.BlastScheduleDays> bsd = ECNBusiness.Communicator.BlastScheduleDays.GetByBlastScheduleID(128);
            Assert.IsNotNull(bsd[0], "No BlastScheduleDays with this BlastScheduleID");
        }

        
    }
}
