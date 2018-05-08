using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FrameworkUAD.Object;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using FilterMVC = FrameworkUAD.BusinessLogic.FilterMVC;

namespace FrameworkUAD.Tests.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FilterMVCTests
    {
        private const int UserId = 42;
        private const int ProductId = 24;
        private const int BrandId = 31;
        private const int IssuedId = 12345;
        private const string EmptyCombinationQueryResult =
            "<xml><Queries></Queries><Results><Result linenumber=\"1\"  selectedfilterno=\"\" selectedfilteroperation=\"\" suppressedfilterno=\"\" suppressedfilteroperation=\"\"  filterdescription=\"\"></Result></Results></xml>";

        private const string EmptyCombinationQueryResultWithText =
            "<xml><Queries></Queries><Results><Result linenumber=\"1\"  selectedfilterno=\"1,2,3,4,5\" selectedfilteroperation=\"MySelectFilterOperation\" suppressedfilterno=\"6,7,8,9\" suppressedfilteroperation=\"MySuppressFilterOperation\"  filterdescription=\"\"></Result></Results></xml>";

        private const string WithFilterCollectionFilterParameterSetTestResult =
            "<xml><Queries><Query filterno=\"1\" ><![CDATA[select  distinct 1, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"2\" ><![CDATA[select  distinct 2, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"3\" ><![CDATA[select  distinct 3, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"4\" ><![CDATA[select  distinct 4, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"5\" ><![CDATA[select  distinct 5, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"6\" ><![CDATA[select  distinct 6, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"7\" ><![CDATA[select  distinct 7, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"8\" ><![CDATA[select  distinct 8, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query><Query filterno=\"9\" ><![CDATA[select  distinct 9, ps.SubscriptionID  from pubsubscriptions ps  with (nolock) MyAddFilters;]]></Query></Queries><Results><Result linenumber=\"1\"  selectedfilterno=\"1,2,3,4,5\" selectedfilteroperation=\"MySelectFilterOperation\" suppressedfilterno=\"6,7,8,9\" suppressedfilteroperation=\"MySuppressFilterOperation\"  filterdescription=\"\"></Result></Results></xml>";
      
        private const string WithFilterCollectionFilterArchivedParameterSetTestResult =
            "<xml><Queries><Query filterno=\"1\" ><![CDATA[select  distinct 1, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"2\" ><![CDATA[select  distinct 2, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"3\" ><![CDATA[select  distinct 3, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"4\" ><![CDATA[select  distinct 4, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"5\" ><![CDATA[select  distinct 5, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"6\" ><![CDATA[select  distinct 6, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"7\" ><![CDATA[select  distinct 7, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"8\" ><![CDATA[select  distinct 8, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query><Query filterno=\"9\" ><![CDATA[select  distinct 9, ps.SubscriptionID  from IssueArchiveProductSubscription ps  with (nolock) MyAddFilters where  IssueID = 12345;]]></Query></Queries><Results><Result linenumber=\"1\"  selectedfilterno=\"1,2,3,4,5\" selectedfilteroperation=\"MySelectFilterOperation\" suppressedfilterno=\"6,7,8,9\" suppressedfilteroperation=\"MySuppressFilterOperation\"  filterdescription=\"\"></Result></Results></xml>";

        private const string SelectFilterOperation = "MySelectFilterOperation";
        private const string SuppressFilterOperation = "MySuppressFilterOperation";
        private const string SelectedFilterNo = "1,2,3,4,5";
        private const string SuppressedFilterNo = "6,7,8,9";
        private const string AddFilters = "MyAddFilters";

        [Test]
        public void generateCombinationQuery_EmptyFilterCollection_EmptyQueryReturned()
        {
            // Arrange
            var filterCollection = new FilterCollection(new ClientConnections(), UserId);

            // Act
            var result = FilterMVC.generateCombinationQuery(
                filterCollection,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                ProductId,
                BrandId,
                new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(typeof(StringBuilder)),
                () => result.ToString().ShouldBe(EmptyCombinationQueryResult));
        }
        
        [Test]
        public void generateCombinationQueryForIssueArchived_EmptyFilterCollection_EmptyQueryReturned()
        {
            // Arrange
            var filterCollection = new FilterCollection(new ClientConnections(), UserId);

            // Act
            var result = FilterMVC.generateCombinationQueryForIssueArchived(
                filterCollection,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                ProductId,
                BrandId,
                IssuedId,
                new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(typeof(StringBuilder)),
                () => result.ToString().ShouldBe(EmptyCombinationQueryResult));
        } 
        
        [Test]
        public void generateCombinationQuery_EmptyFilterCollectionFilterParameterSet_EmptyQueryWithFilterSetReturned()
        {
            // Arrange
            var filterCollection = new FilterCollection(new ClientConnections(), UserId);

            // Act
            var result = FilterMVC.generateCombinationQuery(
                filterCollection,
                SelectFilterOperation,
                SuppressFilterOperation,
                SelectedFilterNo,
                SuppressedFilterNo,
                AddFilters,
                ProductId,
                BrandId,
                new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(typeof(StringBuilder)),
                () => result.ToString().ShouldBe(EmptyCombinationQueryResultWithText));
        }  
        
        [Test]
        public void generateCombinationQueryForIssueArchived_EmptyFilterCollectionFilterParameterSet_EmptyQueryWithFilterSetReturned()
        {
            // Arrange
            var filterCollection = new FilterCollection(new ClientConnections(), UserId);

            // Act
            var result = FilterMVC.generateCombinationQueryForIssueArchived(
                filterCollection,
                SelectFilterOperation,
                SuppressFilterOperation,
                SelectedFilterNo,
                SuppressedFilterNo,
                AddFilters,
                ProductId,
                BrandId,
                IssuedId,
                new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(typeof(StringBuilder)),
                () => result.ToString().ShouldBe(EmptyCombinationQueryResultWithText));
        }  

        [Test]
        public void generateCombinationQuery_WithFilterCollectionFilterParameterSet_QueryWithFilterSetReturned()
        {
            // Arrange
            var filterCollection = CreateFilterCollection();

            // Act
            var result = FilterMVC.generateCombinationQuery(
                filterCollection,
                SelectFilterOperation,
                SuppressFilterOperation,
                SelectedFilterNo,
                SuppressedFilterNo,
                AddFilters,
                ProductId,
                BrandId,
                new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(typeof(StringBuilder)),
                () => result.ToString().ShouldBe(WithFilterCollectionFilterParameterSetTestResult));
        } 
        
        [Test]
        public void generateCombinationQueryArchived_WithFilterCollectionFilterParameterSet_QueryWithFilterSetReturned()
        {
            // Arrange
            var filterCollection = CreateFilterCollection();

            // Act
            var result = FilterMVC.generateCombinationQueryForIssueArchived(
                filterCollection,
                SelectFilterOperation,
                SuppressFilterOperation,
                SelectedFilterNo,
                SuppressedFilterNo,
                AddFilters,
                ProductId,
                BrandId,
                IssuedId,
                new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(typeof(StringBuilder)),
                () => result.ToString().ShouldBe(WithFilterCollectionFilterArchivedParameterSetTestResult));
        }

        private static FilterCollection CreateFilterCollection()
        {
            var filterCollection = new FilterCollection(new ClientConnections(), UserId);
            foreach (var index in Enumerable.Range(1, 10))
            {
                var filterMvc = new Object.FilterMVC
                {
                    BrandID = BrandId,
                    Executed = false,
                    FilterName = $"FilterName_{index}",
                    FilterNo = index,
                    FilterID = index
                };

                filterCollection.Add(filterMvc, false);
            }
           
            return filterCollection;
        }
    }
}