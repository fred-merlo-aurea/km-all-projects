using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Communicator;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using CampaignItemBlast = ECN_Framework_Entities.Communicator.CampaignItemBlast;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }
    }
}
