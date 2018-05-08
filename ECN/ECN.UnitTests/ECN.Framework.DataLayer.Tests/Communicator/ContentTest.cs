using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;
using CommContent = ECN_Framework_Entities.Communicator.Content;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ContentTest : Fakes
    {
        private const int FolderId = 10;
        private const int ValidatedOnly = 20;
        private const int CustomerId = 30;
        private const int UserId = 40;
        private const int BaseChannelId = 50;
        private const int CurrentPage = 60;
        private const int PageSize = 70;
        private const string ArchiveFilter = "Archive Filter";
        private const string ContentTitle = "Sample Title";
        private const string SortDirection = "ASC";
        private const string SortColumn = "Sort Column";

        private readonly DateTime? UpdatedDateFrom = DateTime.Now;
        private readonly DateTime? UpdatedDateTo = DateTime.Now;
        private readonly SqlCommand SqlCommand = null;
        private List<CommContent> _contentList;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void Content_GetByContentTitle_ShouldFillOnlyRequiredParameters_ReturnsDataSet()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.GetDataSetSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new DataSet();
            };

            // Act
            var result = Content.GetByContentTitle(
                ContentTitle,
                null,
                null,
                CustomerId,
                null,
                null,
                null,
                BaseChannelId,
                CurrentPage,
                PageSize,
                SortDirection,
                SortColumn,
                ArchiveFilter);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureContentSelectTitle),
                () => GetParameterValue(Consts.ParamContentTitle).ShouldBe(ContentTitle),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUserId).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUpdatedDateFrom).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUpdatedDateTo).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamBaseChannelId).ShouldBe(BaseChannelId.ToString()),
                () => GetParameterValue(Consts.ParamCurrentPage).ShouldBe(CurrentPage.ToString()),
                () => GetParameterValue(Consts.ParamPageSize).ShouldBe(PageSize.ToString()),
                () => GetParameterValue(Consts.ParamSortDirection).ShouldBe(SortDirection),
                () => GetParameterValue(Consts.ParamSortColumn).ShouldBe(SortColumn),
                () => GetParameterValue(Consts.ParamValidatedOnly).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamArchiveFilter).ShouldBe(ArchiveFilter),
                () => result.ShouldBeOfType<DataSet>());
        }

        [Test]
        public void Content_GetByContentTitle_ShouldFillAllParameters_ReturnsDataSet()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.GetDataSetSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new DataSet();
            };

            // Act
            var result = Content.GetByContentTitle(
                ContentTitle,
                FolderId,
                ValidatedOnly,
                CustomerId,
                UserId,
                UpdatedDateFrom,
                UpdatedDateTo,
                BaseChannelId,
                CurrentPage,
                PageSize,
                SortDirection,
                SortColumn,
                ArchiveFilter);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureContentSelectTitle),
                () => GetParameterValue(Consts.ParamContentTitle).ShouldBe(ContentTitle),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBe(FolderId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedDateFrom).ShouldBe(UpdatedDateFrom.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedDateTo).ShouldBe(UpdatedDateTo.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamBaseChannelId).ShouldBe(BaseChannelId.ToString()),
                () => GetParameterValue(Consts.ParamCurrentPage).ShouldBe(CurrentPage.ToString()),
                () => GetParameterValue(Consts.ParamPageSize).ShouldBe(PageSize.ToString()),
                () => GetParameterValue(Consts.ParamSortDirection).ShouldBe(SortDirection),
                () => GetParameterValue(Consts.ParamSortColumn).ShouldBe(SortColumn),
                () => GetParameterValue(Consts.ParamValidatedOnly).ShouldBe(ValidatedOnly.ToString()),
                () => GetParameterValue(Consts.ParamArchiveFilter).ShouldBe(ArchiveFilter),
                () => result.ShouldBeOfType<DataSet>());
        }

        [Test]
        public void Content_AddContentListItems_WhenContentListIsNull_ThrowsException()
        {
            // Arrange
            _contentList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                Content.AddContentListItems(_contentList, SqlCommand);
            });
        }

        [Test]
        public void Content_AddContentListItems_WhenSqlCommandIsNull_ThrowsException()
        {
            // Arrange
            _contentList = new List<CommContent>();

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                Content.AddContentListItems(_contentList, SqlCommand);
            });
        }

        [Test]
        public void Content_GetListByContentTitle_ShouldFillOnlyRequiredParameters_ReturnsContentList()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteReaderSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return null;
            };

            ShimForSqlConnection();
            ShimForCloseSqlConnection();

            // Act
            var result = Content.GetListByContentTitle(
                ContentTitle,
                null,
                CustomerId,
                null,
                null,
                null,
                BaseChannelId,
                CurrentPage,
                PageSize,
                SortDirection,
                SortColumn,
                ArchiveFilter);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureContentSelectTitle),
                () => GetParameterValue(Consts.ParamContentTitle).ShouldBe(ContentTitle),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUserId).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUpdatedDateFrom).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUpdatedDateTo).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamBaseChannelId).ShouldBe(BaseChannelId.ToString()),
                () => GetParameterValue(Consts.ParamCurrentPage).ShouldBe(CurrentPage.ToString()),
                () => GetParameterValue(Consts.ParamPageSize).ShouldBe(PageSize.ToString()),
                () => GetParameterValue(Consts.ParamSortDirection).ShouldBe(SortDirection),
                () => GetParameterValue(Consts.ParamSortColumn).ShouldBe(SortColumn),
                () => GetParameterValue(Consts.ParamArchiveFilter).ShouldBe(ArchiveFilter),
                () => result.ShouldBeOfType<List<CommContent>>());
        }

        [Test]
        public void Content_GetListByContentTitle_ShouldFillAllParameters_ReturnsContentList()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteReaderSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return null;
            };

            ShimForSqlConnection();
            ShimForCloseSqlConnection();

            // Act
            var result = Content.GetListByContentTitle(
                ContentTitle,
                FolderId,
                CustomerId,
                UserId,
                UpdatedDateFrom,
                UpdatedDateTo,
                BaseChannelId,
                CurrentPage,
                PageSize,
                SortDirection,
                SortColumn,
                ArchiveFilter);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureContentSelectTitle),
                () => GetParameterValue(Consts.ParamContentTitle).ShouldBe(ContentTitle),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBe(FolderId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedDateFrom).ShouldBe(UpdatedDateFrom.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedDateTo).ShouldBe(UpdatedDateTo.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamBaseChannelId).ShouldBe(BaseChannelId.ToString()),
                () => GetParameterValue(Consts.ParamCurrentPage).ShouldBe(CurrentPage.ToString()),
                () => GetParameterValue(Consts.ParamPageSize).ShouldBe(PageSize.ToString()),
                () => GetParameterValue(Consts.ParamSortDirection).ShouldBe(SortDirection),
                () => GetParameterValue(Consts.ParamSortColumn).ShouldBe(SortColumn),
                () => GetParameterValue(Consts.ParamArchiveFilter).ShouldBe(ArchiveFilter),
                () => result.ShouldBeOfType<List<CommContent>>());
        }
    }
}
