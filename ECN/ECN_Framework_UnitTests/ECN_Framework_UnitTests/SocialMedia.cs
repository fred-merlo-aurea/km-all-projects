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
    /// Summary description for SocialMedia
    /// </summary>
    [TestClass]
    public class SocialMedia
    {
        [TestMethod]
        public void GetByID()
        {
            ECNEntity.Communicator.SocialMedia sm = ECNBusiness.Communicator.SocialMedia.GetSocialMediaByID(1);
            Assert.IsNotNull(sm, "No SocialMedia with this ID");
        }

        [TestMethod]
        public void GetByCanPublish()
        {
            List<ECNEntity.Communicator.SocialMedia> sm = ECNBusiness.Communicator.SocialMedia.GetSocialMediaCanPublish();
            Assert.AreEqual(0,sm.Count,"Found SocialMedia that can publish");
        }

        [TestMethod]
        public void GetByCanShare()
        {
            List<ECNEntity.Communicator.SocialMedia> sm = ECNBusiness.Communicator.SocialMedia.GetSocialMediaCanShare();
            Assert.IsNotNull(sm[0], "No SocialMedia that can share");
        }
    }
}
