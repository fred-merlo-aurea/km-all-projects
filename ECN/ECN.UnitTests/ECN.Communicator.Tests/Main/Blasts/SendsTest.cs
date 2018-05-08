using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="Sends"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SendsTest : BaseBlastsTest<Sends>
    {
        [Test]
        public void GetUdfData_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFdata, UDFdataValue);

            // Act, Assert
            testObject.getUDFData().ShouldBe(UDFdataValue);
        }

        [Test]
        public void GetUdfData_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFData().ShouldBeEmpty();
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void PageLoad_UserHasAccessIsPostBack_SetsMaster(bool userHasAccess, bool isPostBack)
        {
            // Arrange
            var loadSendsBlastGridCalled = false;
            var showHideDownloadCalled = false;
            ShimSends.AllInstances.loadSendsBlastGrid = (obj) => { loadSendsBlastGridCalled = true; };
            ShimSends.AllInstances.ShowHideDownload = (obj) => { showHideDownloadCalled = true; };
            ShimPage.AllInstances.IsPostBackGet = (page) => isPostBack;
            SetShimKMPlatformUserAccess(
                Services.EMAILMARKETING,
                ServiceFeatures.BlastReportSends,
                new Dictionary<Access, bool> { [Access.ViewDetails] = userHasAccess });

            // Act
            try
            {
                privateObject.Invoke(PageLoad, null, new EventArgs());
            }
            catch (Exception e)
            {
                if (e.InnerException is SecurityException securityException)
                {
                    roleAccessExceptionOccured = securityException.SecurityType == Enums.SecurityExceptionType.RoleAccess;
                }
            }

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => currentMenuCode.ShouldBe(MenuCode.REPORTS),
                () => subMenu.ShouldBe(string.Empty),
                () => heading.ShouldBe(SendsReport),
                () => helpContent.ShouldBe(string.Empty),
                () => helpTitle.ShouldBe(BlastManager),
                () =>
                {
                    if (userHasAccess)
                    {
                        if(!isPostBack)
                        {
                            loadSendsBlastGridCalled.ShouldBeTrue();
                            showHideDownloadCalled.ShouldBeTrue();
                        }

                        roleAccessExceptionOccured.ShouldBeFalse();
                    }
                    else
                    {
                        roleAccessExceptionOccured.ShouldBeTrue();
                    }
                });
        }
    }
}
