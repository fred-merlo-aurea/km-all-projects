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
    /// Summary description for BlastSetupInfo
    /// </summary>
    [TestClass]
    public class BlastSetupInfo
    {
        [TestMethod]
        public void GetByBlastID()
        {
            //Cant get this one to work
            ECNEntity.Communicator.BlastSetupInfo bsi = ECNBusiness.Communicator.BlastSetupInfo.GetNextScheduledBlastSetupInfo(1568664);
            Assert.IsNotNull(bsi, "No BlastSetupInfo for this BlastID");
        }
    }
}
