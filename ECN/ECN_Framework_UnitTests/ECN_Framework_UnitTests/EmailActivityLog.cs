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
    /// Summary description for EmailActivityLog
    /// </summary>
    [TestClass]
    public class EmailActivityLog
    {
        [TestMethod]
        public void GetByEAID()
        {
            //As of 1/18/2013 there are no records in the EmailActivityLog table
            ECNEntity.Communicator.EmailActivityLog eal = ECNBusiness.Communicator.EmailActivityLog.GetByBlastEAID(-1);
        }
    }
}
