using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.editor.ckeditor.controls;
using ECN_Framework_Common.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Editor.Tests.ckeditor.controls
{
    /// <summary>
    /// Unit Test class for <see cref="groupexplorer"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GroupExplorerTest
    {
        private const string ErrorMsg = "Test Error Message";
        private const string PlaceHolderError = "phError";
        private const string LabelErrorMessage = "lblErrorMessage";
        private const string SetEcnError = "setECNError";
        private const string SelectedGroupIdName = "selectedGroupID";
        private groupexplorer _testObject;
        private PrivateObject _privateObject;

        [SetUp]
        public void InitTest()
        {
            _testObject = new groupexplorer();
            _privateObject = new PrivateObject(_testObject);
        }

        [TearDown]
        public void CleanUp()
        {
            _testObject.Dispose();
            _privateObject = null;
        }

        [Test]
        public void SelectedGroupId_SetValue_ReturnsSetValue()
        {
            // Arrange, Act
            var defaultValue = (int) _privateObject.GetFieldOrProperty(SelectedGroupIdName);
            _privateObject.SetFieldOrProperty(SelectedGroupIdName, 10);

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => defaultValue.ShouldBe(0),
                () => ((int) _privateObject.GetFieldOrProperty(SelectedGroupIdName)).ShouldBe(10));
        }

        [Test]
        public void SetEcnError_SetEcnExcpetion_SetsLblErrorMessageText()
        {
            // Arrange
            var ecnError = new ECNError(Enums.Entity.APILogging, Enums.Method.Validate, ErrorMsg);
            var ecnExcpetion = new ECNException(new List<ECNError>
            {
                ecnError
            });
            using(var placeHolder = new PlaceHolder {Visible = false})
            {
                using(var label = new Label())
                {
                    _privateObject.SetFieldOrProperty(PlaceHolderError, placeHolder);
                    _privateObject.SetFieldOrProperty(LabelErrorMessage, label);

                    // Act
                    _privateObject.Invoke(SetEcnError, ecnExcpetion);

                    // Assert
                    _testObject.ShouldSatisfyAllConditions(
                        () => ((PlaceHolder) _privateObject.GetFieldOrProperty(PlaceHolderError)).Visible.ShouldBeTrue(),
                        () => ((Label) _privateObject.GetFieldOrProperty(LabelErrorMessage)).Text.ShouldBe(
                            $"<br/>{ecnError.Entity}: {ecnError.ErrorMessage}"));
                }
            }
        }
    }
}