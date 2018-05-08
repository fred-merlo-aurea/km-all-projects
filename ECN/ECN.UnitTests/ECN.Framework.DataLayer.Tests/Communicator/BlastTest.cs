using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Fakes;
using KM.Common;
using NUnit.Framework;
using Shouldly;
using CommCampaignItemBlastFilter = ECN_Framework_Entities.Communicator.CampaignItemBlastFilter;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BlastTest : Fakes
    {
        private const int BlastId = 10;
        private const int CustomerId = 20;
        private const string CommaSeparator = ",";
        private const string CloseTag = "\">";
        private const string OpenTag = "</";
        private const string FilterIdsTag = "<FilterIDs>";
        private const string FilterIdsCloseTag = "</FilterIDs>";
        private const string SsIdTag = "<ssID id=\"";
        private const string RefBlastIdOpenTag = "<refBlastIDs>";
        private const string RefBlastIdCloseTag = "</refBlastIDs></ssID>";
        private const string SuppFiltersCloseTag = "</SuppFilters>";
        private const string SuppresionGroupTag = "<SuppressionGroup id=\"";
        private const string SuppressionGroupCloseTag = "</SuppressionGroup>";

        private List<CommCampaignItemBlastFilter> _blastFilters;
        private StringBuilder _suppFilters;

        [SetUp]
        public void Setup()
        {
            SetupFakes();

            _suppFilters = new StringBuilder();
            _blastFilters = new List<CommCampaignItemBlastFilter>
            {
                new CommCampaignItemBlastFilter
                {
                    SuppressionGroupID = 10, 
                    FilterID = 100, 
                    SmartSegmentID = 1000,
                    RefBlastIDs = $"{BlastId}{CustomerId}"
                },
                new CommCampaignItemBlastFilter { SuppressionGroupID = 20 },
                new CommCampaignItemBlastFilter { SuppressionGroupID = 30 }
            };
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Blast_Exists_ShouldFillAllParameters_ReturnsBoolean(int execResult)
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return execResult;
            };

            // Act
            var result = Blast.Exists(BlastId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureBlastExistsByBlastId),
                () => GetParameterValue(Consts.ParamBlastId).ShouldBe(BlastId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Blast_ActiveOrSent_ShouldFillAllParameters_ReturnsBoolean(int execResult)
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return execResult;
            };

            // Act
            var result = Blast.ActiveOrSent(BlastId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureBlastActiveOrSent),
                () => GetParameterValue(Consts.ParamBlastId).ShouldBe(BlastId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        public void Blast_FillSuppresionFilters_WhenBlastFiltersIsNull_ThrowsException()
        {
            // Arrange
            _blastFilters = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                Blast.FillSuppresionFilters(_blastFilters, _suppFilters);
            });
        }

        [Test]
        public void Blast_FillSuppresionFilters_WhenSuppFiltersIsNull_ThrowsException()
        {
            // Arrange
            _suppFilters = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                Blast.FillSuppresionFilters(_blastFilters, _suppFilters);
            });
        }

        [Test]
        public void Blast_FillSuppresionFilters_ReturnsStringBuilder()
        {
            // Arrange
            var sbSuppFilters = CreateSuppFilterStringBuilder(_blastFilters, _suppFilters);

            // Act
            var result = Blast.FillSuppresionFilters(_blastFilters, _suppFilters);

            // Assert
            result.ToString().ShouldBe(sbSuppFilters.ToString());
        }

        private static StringBuilder CreateSuppFilterStringBuilder(
            IList<CommCampaignItemBlastFilter> blastFilters,
            StringBuilder suppFilters)
        {
            Guard.NotNull(blastFilters, nameof(blastFilters));
            Guard.NotNull(suppFilters, nameof(suppFilters));

            var groups = blastFilters.Where(x => x.SuppressionGroupID != null)
                .Select(e => e.SuppressionGroupID ?? 0)
                .Distinct()
                .ToArray();

            foreach (var currentGroupId in groups)
            {
                var suppFilter = blastFilters.Where(x => x.SuppressionGroupID == currentGroupId).ToList();
                suppFilters.Append($"{SuppresionGroupTag}{currentGroupId}{CloseTag}");
                suppFilters.Append(FilterIdsTag);

                foreach (var filter in suppFilter.Where(x => x.FilterID != null))
                {
                    suppFilters.Append(filter.FilterID + CommaSeparator);
                }

                suppFilters.Append(FilterIdsCloseTag);
                foreach (var filter in suppFilter.Where(x => x.SmartSegmentID != null))
                {
                    suppFilters.Append($"{SsIdTag}{filter.SmartSegmentID}{CloseTag}");
                    suppFilters.Append($"{RefBlastIdOpenTag}{filter.RefBlastIDs}{RefBlastIdCloseTag}");
                }
                suppFilters.Append(SuppressionGroupCloseTag);
            }

            suppFilters.Append(SuppFiltersCloseTag);
            suppFilters = suppFilters.Replace(CommaSeparator + OpenTag, OpenTag);
            return suppFilters;
        }
    }
}
