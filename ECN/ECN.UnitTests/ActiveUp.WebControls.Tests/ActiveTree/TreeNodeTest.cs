using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.Fakes;
using ActiveUp.WebControls.Fakes;
using ActiveUp.WebControls.Common.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveTree
{
	[TestFixture]
    [ExcludeFromCodeCoverage]
	public class TreeNodeTest
	{
		private const string DummyCssClass = "dummycssclass";
		private const string DummyFontName = "calibri";
        private TreeNode _treeNode;
        private PrivateObject _privateObject;
        private IDisposable _shims;

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

		public static object[] PropertyNameWithExpectedValue =
		{
			new object[] { "CssClass", DummyCssClass},
			new object[] { "ForeColor", Color.Black},
			new object[] { "Enabled", true}
		};

		[TestCaseSource("PropertyNameWithExpectedValue")]
		public void Property_SetAndGetValue_ReturnsTheSetValue(string propertyName, object expected)
		{
			// Arrange
			var treeNode = new TreeNode();
			var privateObject = new PrivateObject(treeNode);

			// Act
			privateObject.SetFieldOrProperty(propertyName, expected);

			// Assert
			privateObject.GetFieldOrProperty(propertyName).ShouldBe(expected);
		}

		[Test]
		public void Font_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange
			var treeNode = new TreeNode();

			// Act
			treeNode.Font.Name = DummyFontName;

			// Assert
			treeNode.Font.Name.ShouldBe(DummyFontName);
		}

        [Test]
        public void Render_ForNodeForSelectionTypeSingle_WriteOutput()
        {
            // Arrange
            _treeNode = new TreeNode();
            _privateObject = new PrivateObject(_treeNode);
            HtmlTextWriter htmlTextWriter = SetUp("single");

            // Act
            _privateObject.Invoke("Render", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions(
                () => htmlTextWriter.Indent.ShouldBe(0),
                () => htmlTextWriter.NewLine.ShouldBe(null)
                );
        }

        [Test]
        public void Render_ForSelectionTypeCheckBox_WriteOutput()
        {
            // Arrange
            _treeNode = new TreeNode();
            _privateObject = new PrivateObject(_treeNode);
            HtmlTextWriter htmlTextWriter = SetUp("check");

            // Act
            _privateObject.Invoke("Render", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions(
                () => htmlTextWriter.Indent.ShouldBe(0),
                () => htmlTextWriter.NewLine.ShouldBe(null)
                );
        }

        protected HtmlTextWriter SetUp(String param)
        {
            var textWriter = new Moq.Mock<TextWriter>();
            TreeView treeView = new TreeView();
            ControlCollection control = new ControlCollection(new Control());
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(textWriter.Object);
            _shims = ShimsContext.Create();
            ShimTreeNode.AllInstances.SelectedGet = (x) => true;
            ShimTreeNode.AllInstances.BaseTreeGet = (x) => treeView;
            ShimCoreWebControl.AllInstances.EnabledGet = (x) => true;
            if (param.Equals("single"))
            {
                ShimTreeView.AllInstances.SelectionTypeGet = (x) => SelectionType.Single;
                ShimTreeNode.AllInstances.IconGet = (x) => "icon";
                ShimControl.AllInstances.ControlsGet = (x) => control;
                ShimControlCollection.AllInstances.CountGet = (x) => 2;
                ShimTreeNode.AllInstances.NodesGet = (x) => control;
            }
            else
            {
                ShimTreeView.AllInstances.SelectionTypeGet = (x) => SelectionType.CheckBox;
            }
            ShimTreeView.AllInstances.LoadOnDemandGet = (x) => true;
            ShimTreeView.AllInstances.TextModeGet = (x) => false;
            ShimTreeNode.AllInstances.LinkGet = (x) => "link";
            return htmlTextWriter;
        }
	}
}