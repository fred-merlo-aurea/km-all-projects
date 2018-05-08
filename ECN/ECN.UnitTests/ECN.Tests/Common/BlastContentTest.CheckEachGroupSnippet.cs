using System;
using System.Data.SqlClient;
using Shouldly;
using NUnit.Framework;
using ecn.common.classes.Fakes;

namespace ECN.Tests.Common
{
    public partial class BlastContentTest
    {
        private const string TestedMethodName_CheckEachGroupSnippet = "CheckEachGroupSnippet";
        private const string GroupIDs = "11";
        private const string LayoutID = "1";
        private const string Subject = "Just a dummy subject";

        [Test]
        public void CheckEachGroupSnippet_ReturnsTrue()
        {
            //Arrange
            var errorMessage = String.Empty;
            var methodArguments = new object[]
            {
                GroupIDs,
                LayoutID,
                Subject,
                errorMessage
            };

            //Act
            var returnResult = (bool)_testedClass.InvokeStatic(TestedMethodName_CheckEachGroupSnippet, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
            errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void CheckEachGroupSnippet_MatchingBadSnippetRegexForOddOcurrences_ReturnsFalse()
        {
            //Arrange
            var errorMessage = String.Empty;
            _dataTableContent.Rows.Add(DataTableOtherShortNameColumnName, DataTableContentRegexMathcingValue);
            var methodArguments = new object[]
            {
                GroupIDs,
                LayoutID,
                Subject,
                errorMessage
            };

            //Act
            var returnResult = (bool)_testedClass.InvokeStatic(TestedMethodName_CheckEachGroupSnippet, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckEachGroupSnippet_MatchingBadSnippetRegexForEvenOcurrences_ReturnsFalse()
        {
            //Arrange
            var errorMessage = String.Empty;
            _dataTableContent.Rows.Add(DataTableOtherShortNameColumnName, DataTableContentRegexMathcingValueMoreThanOnce);
            var methodArguments = new object[]
            {
                GroupIDs,
                LayoutID,
                Subject,
                errorMessage
            };

            //Act
            var returnResult = (bool)_testedClass.InvokeStatic(TestedMethodName_CheckEachGroupSnippet, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckEachGroupSnippet_MatchingBadSnippetRegexForTwoOcurrences_ReturnsFalse()
        {
            //Arrange
            var errorMessage = String.Empty;
            _dataTableContent.Rows.Add(DataTableOtherShortNameColumnName, DataTableContentRegexMathcingValueTwice);
            var methodArguments = new object[]
            {
                GroupIDs,
                LayoutID,
                Subject,
                errorMessage
            };

            //Act
            var returnResult = (bool)_testedClass.InvokeStatic(TestedMethodName_CheckEachGroupSnippet, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckEachGroupSnippet_ThrowsSqlException_ExceptionCaught_ReturnsFalse()
        {
            //Arrange
            var errorMessage = String.Empty;

            ShimDataFunctions.GetDataTableStringString =
               (sql, connString) =>
               {
                   throw Instantiate<SqlException>();
               };

            var methodArguments = new object[]
            {
                GroupIDs,
                LayoutID,
                Subject,
                errorMessage
            };

            //Act
            var returnResult = (bool)_testedClass.InvokeStatic(TestedMethodName_CheckEachGroupSnippet, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        public static T Instantiate<T>() where T : class
        {
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T)) as T;
        }
    }
}
