using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.MasterPages.Fakes;
using ecn.communicator.blastsmanager;
using ECN_Framework_Common.Objects;
using ecn.communicator.blastsmanager.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="resends"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ResendsTest : BaseBlastsTest<resends>
    {
        private const string ExpHelpContent = "<p><b>Resend Soft Bounces</b><br />Lists all email address who Unsubscribed for this email. <br /><br />Displays the<br />-&nbsp;&nbsp;<i>Time</i> unsubscribed<br />-&nbsp;&nbsp;<i>email address</i> unsubscribed <br />-&nbsp;&nbsp;<i>change</i>";

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void PageLoad_UserHasAccessIsPostBack_SetsMaster(bool userHasAccess, bool isPostBack)
        {
            // Arrange
            var loadResendsGridCalled = false;
            var showHideDownloadCalled = false;
            Shimresends.AllInstances.loadResendsGrid = (obj) => { loadResendsGridCalled = true; };
            Shimresends.AllInstances.ShowHideDownload = (obj) => { showHideDownloadCalled = true; };
            ShimPage.AllInstances.IsPostBackGet = (page) => isPostBack;
            SetShimKMPlatformUserAccess(
                Services.EMAILMARKETING,
                ServiceFeatures.BlastReportResends,
                new Dictionary<Access, bool> { [Access.View] = userHasAccess });

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
                () => heading.ShouldBe(ResendReport),
                () => helpContent.ShouldBe(ExpHelpContent),
                () => helpTitle.ShouldBe(BlastManager),
                () =>
                {
                    if (userHasAccess)
                    {
                        if (!isPostBack)
                        {
                            loadResendsGridCalled.ShouldBeTrue();
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
