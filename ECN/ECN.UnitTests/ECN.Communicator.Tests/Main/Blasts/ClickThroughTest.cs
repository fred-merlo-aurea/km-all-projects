using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using Shouldly;
using NUnit.Framework;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="ClickThrough"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClickThroughTest : BaseBlastsTest<ClickThrough>
    {
        private const string DownloadPanel = "DownloadPanel";

        [Test]
        public void GetUdfName_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFName, UDFNameValue);

            // Act, Assert
            testObject.getUDFName().ShouldBe(UDFNameValue);
        }

        [Test]
        public void GetUdfName_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFName().ShouldBeEmpty();
        }

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

        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(true, true, false)]
        [TestCase(false, true, false)]
        public void PageLoad_UserHasAccessIsPostBack_SetsMaster(bool userHasViewAccess, bool userHasDownloadAccess, bool isPostBack)
        {
            // Arrange
            var loadDataCalled = false;
            privateObject.SetField(DownloadPanel, new Panel());
            ShimClickThrough.AllInstances.LoadData = (obj) => loadDataCalled = true;
            ShimPage.AllInstances.IsPostBackGet = (page) => isPostBack;
            SetShimBusinessLogicUserAccess(
                Services.EMAILMARKETING,
                ServiceFeatures.BlastReportClickThroughRatio,
                new Dictionary<Access, bool> {
                    [Access.ViewDetails] = userHasViewAccess,
                    [Access.DownloadDetails] = userHasDownloadAccess});

            // Act
            try
            {
                privateObject.Invoke(PageLoad, null, new EventArgs());
            }
            catch (Exception e)
            {
                roleAccessExceptionOccured = e.InnerException is SecurityException securityException;
            }

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => currentMenuCode.ShouldBe(MenuCode.REPORTS),
                () => subMenu.ShouldBe(string.Empty),
                () => heading.ShouldBe(ClickThroughRatioReport),
                () => helpContent.ShouldBe(HelpContentClickThrough),
                () => helpTitle.ShouldBe(BlastManager),
                () =>
                {
                    if(!isPostBack)
                    {
                        if(userHasViewAccess)
                        {
                            var downloadPanelVisible = (privateObject.GetField(DownloadPanel) as Panel).Visible;
                            if (userHasDownloadAccess)
                            {
                                downloadPanelVisible.ShouldBeTrue();
                            }
                            else
                            {
                                downloadPanelVisible.ShouldBeFalse();
                            }
                            loadDataCalled.ShouldBeTrue();
                        }
                        else
                        {
                            roleAccessExceptionOccured.ShouldBeTrue();
                        }
                    }
                });
        }
    }
}
