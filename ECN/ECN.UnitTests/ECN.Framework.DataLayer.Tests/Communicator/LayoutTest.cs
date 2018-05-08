using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;
using CommLayout = ECN_Framework_Entities.Communicator.Layout;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LayoutTest : Fakes
    {
        private const int FolderId = 10;
        private const int LayoutId = 20;
        private const int CustomerId = 30;
        private const int UserId = 40;
        private const int ContentId = 50;
        private const int TemplateId = 60;
        private const string LayoutName = "Layout Name";
        private readonly DateTime? UpdatedDateFrom = DateTime.Now;
        private readonly DateTime? UpdatedDateTo = DateTime.Now;
        private bool? _archived;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Layout_IsValidated_ShouldFillAllParameters_ReturnsBoolean(int execResult)
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
            var result = Layout.IsValidated(LayoutId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutIsValidatedById),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutId).ShouldBe(LayoutId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Layout_Exists_ShouldFillAllParameters_ReturnsBoolean(int execResult)
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
            var result = Layout.Exists(LayoutId, LayoutName, FolderId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutExistsByName),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutName).ShouldBe(LayoutName),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBe(FolderId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutId).ShouldBe(LayoutId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Layout_ContentUsedInLayout_ShouldFillParameter_ReturnsBoolean(int execResult)
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
            var result = Layout.ContentUsedInLayout(ContentId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutExistsContentId),
                () => GetParameterValue(Consts.ParamContentId).ShouldBe(ContentId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Layout_TemplateUsedInLayout_ShouldFillParameter_ReturnsBoolean(int execResult)
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
            var result = Layout.TemplateUsedInLayout(TemplateId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutExistsTemplateId),
                () => GetParameterValue(Consts.ParamTemplateId).ShouldBe(TemplateId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        public void Layout_GetByLayoutSearch_ShouldFillAllParameters_ReturnsListLayoutItems()
        {
            // Arrange
            var commandText = string.Empty;
            ShimLayout.GetListSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new List<CommLayout>();
            };

            _archived = true;

            // Act
            var result = Layout.GetByLayoutSearch(
                LayoutName, 
                FolderId, 
                CustomerId, 
                UserId, 
                UpdatedDateFrom, 
                UpdatedDateTo, 
                _archived);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutSelectSearch),
                () => GetParameterValue(Consts.ParamLayoutName).ShouldBe(LayoutName),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBe(FolderId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedDateFrom).ShouldBe(UpdatedDateFrom.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedDateTo).ShouldBe(UpdatedDateTo.ToString()),
                () => GetParameterValue(Consts.ParamArchived).ShouldBe(_archived.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBeOfType<List<CommLayout>>());
        }

        [Test]
        public void Layout_GetByLayoutSearch_ShouldFillOnlyRequiredParameters_ReturnsListLayoutItems()
        {
            // Arrange
            var commandText = string.Empty;
            ShimLayout.GetListSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new List<CommLayout>();
            };

            _archived = null;

            // Act
            var result = Layout.GetByLayoutSearch(
                LayoutName, 
                null, 
                CustomerId, 
                null, 
                null, 
                null, 
                _archived);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutSelectSearch),
                () => GetParameterValue(Consts.ParamLayoutName).ShouldBe(LayoutName),
                () => GetParameterValue(Consts.ParamFolderId).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUserId).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUpdatedDateFrom).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamUpdatedDateTo).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamArchived).ShouldBeNull(),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBeOfType<List<CommLayout>>());
        }
    }
}
