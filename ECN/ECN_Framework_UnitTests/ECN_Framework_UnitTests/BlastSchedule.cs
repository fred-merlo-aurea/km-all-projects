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
    /// Summary description for BlastSchedule
    /// </summary>
    [TestClass]
    public class BlastSchedule
    {
        [TestMethod]
        public void GetByBlastID()
        {
            ECNEntity.Communicator.BlastSchedule bs = ECNBusiness.Communicator.BlastSchedule.GetByBlastID(448415, true);
            Assert.IsNotNull(bs, "No Blast with that ID");
        }

        [TestMethod]
        public void GetByBlastScheduleID()
        {
            ECNEntity.Communicator.BlastSchedule bs = ECNBusiness.Communicator.BlastSchedule.GetByBlastScheduleID(128, true);
            Assert.IsNotNull(bs, "No BlastSchedule with that ID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Communicator.BlastSchedule schedule = ECNBusiness.Communicator.BlastSchedule.GetByBlastID(448415, false);

            int? test = null;
            test = ECNBusiness.Communicator.BlastSchedule.Update(schedule, 50547);
            Assert.IsNotNull(test, "BlastSchedule did not save");
        }
    }
}
