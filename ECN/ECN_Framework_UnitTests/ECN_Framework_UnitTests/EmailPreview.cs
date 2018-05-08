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
    /// Summary description for EmailPreview
    /// </summary>
    [TestClass]
    public class EmailPreview
    {
        [TestMethod]
        public void GetByBlastID()
        {
            List<ECNEntity.Communicator.EmailPreview> ep = ECNBusiness.Communicator.EmailPreview.GetByBlastID(448209);
            Assert.IsNotNull(ep[0], "No EmailPreview with this BlastID");
        }

        [TestMethod]
        public void GetByCustomerID()
        {
            List<ECNEntity.Communicator.EmailPreview> ep = ECNBusiness.Communicator.EmailPreview.GetByCustomerID(1);
            Assert.IsNotNull(ep[0], "No EmailPreview for this Customer");
        }

        [TestMethod]
        public void GetByEmailTestID()
        {
            ECNEntity.Communicator.EmailPreview ep = ECNBusiness.Communicator.EmailPreview.GetByEmailTestID(7063592);
            Assert.IsNotNull(ep, "No EmailPreview with this EmailTestID");
        }
    }
}
