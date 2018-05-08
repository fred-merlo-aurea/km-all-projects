using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_BusinessLayer.Activity.Report;
using ActivityEntities = ECN_Framework_Entities.Activity.Report;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Activity.Report
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportViewerExportTest
    {
        private const string SampleFirstName = "SampleFirstName";
        private const string SampleLastName = "SampleLastName";
        private const string SampleFullName = "SampleFullName";

        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetTabDelimited_WhenNoSubReport_GeneratesTabDemilitedStringOfProperties()
        {
            // Arrange
            var blastList = new List<ActivityEntities.BlastDelivery>
            {
                new ActivityEntities.BlastDelivery { BlastID  = 1 }
            };
            List<string> fakeList = null;

            // Act
            var resultString = ReportViewerExport.GetTabDelimited(blastList, fakeList);

            // Act
            resultString.ShouldSatisfyAllConditions(
                () => resultString.ShouldNotBeNullOrWhiteSpace(),
                () => resultString.ShouldContain(nameof(ActivityEntities.BlastDelivery.BlastID)),
                () => resultString.ShouldContain("1"));
        }

        [Test]
        public void GetTabDelimited_WhenSubReportArgumentIsNotNull_GeneratesTabDemilitedStringOfProperties()
        {
            // Arrange
            var blastList = new List<ActivityEntities.BlastDelivery>
            {
                new ActivityEntities.BlastDelivery { BlastID  = 1 }
            };
            var linkDetailsList = new List<ActivityEntities.LinkDetails>
            {
                new ActivityEntities.LinkDetails
                {
                    FirstName = SampleFirstName,
                    LastName = SampleLastName,
                    FullName = SampleFullName
                }
            };

            // Act
            var resultString = ReportViewerExport.GetTabDelimited(blastList, linkDetailsList);

            // Assert
            resultString.ShouldSatisfyAllConditions(
                () => resultString.ShouldNotBeNullOrWhiteSpace(),
                () => resultString.ShouldContain(nameof(ActivityEntities.BlastDelivery.BlastID)),
                () => resultString.ShouldContain("1"),
                () => resultString.ShouldContain(nameof(ActivityEntities.LinkDetails.FirstName)),
                () => resultString.ShouldContain(nameof(ActivityEntities.LinkDetails.LastName)),
                () => resultString.ShouldContain(nameof(ActivityEntities.LinkDetails.FullName)),
                () => resultString.ShouldContain(SampleFirstName),
                () => resultString.ShouldContain(SampleLastName),
                () => resultString.ShouldContain(SampleFullName));
        }
    }
}
