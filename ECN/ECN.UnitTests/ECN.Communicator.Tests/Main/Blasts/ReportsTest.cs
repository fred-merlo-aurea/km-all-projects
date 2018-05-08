using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using ecn.communicator.blastsmanager;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Activity;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit tests for <see cref="reports.loadReportsData"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ReportsTest : BasePageTests
    {
        private IDisposable _context;
        private reports _page;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _page = new reports();
            InitializePage(_page);

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SetupFakes()
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => _hasAccess;

            ShimPage.AllInstances.MasterGet = _ => new ecn.communicator.MasterPages.Communicator();
            ShimECNSession.CurrentSession = () =>
            {
                ECNSession session = typeof(ECNSession).CreateInstance();
                session.CurrentUser = new User
                {
                    CustomerID = CustomerId
                };

                return session;
            };

            ShimSuppressedCodes.GetAll = () => new List<SuppressedCodes>
            {
                new SuppressedCodes
                {
                    SuppressedCodeID = SuppressedCodeID,
                    SupressedCode = SupressedCode
                }
            };
        }
    }
}
