using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.UI;
using ActiveUp.WebControls.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveUpload
{

	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class UploadAppletTest
	{
		private const int MinusOne = -1;
		private const int HeightDefaultValue = 200;
		private const int MaxFileSizeDefaultValue = 0;
		private const int MaxUploadSizeDefaultValue = 0;
		private const string TestValueString = "TestValue1";
		private const string DefaultDirectoryDefaultValue = "c:\\";
    private UploadApplet _uploadApplet;
    private PrivateObject _privateObject;
    private IDisposable _shims;
    private const int TestValue = 20;
    
    [TearDown]
    public void TearDown()
    {
      _shims?.Dispose();
    }

    [TestCase("fd")]
    [TestCase("f")]
    [TestCase("d")]
    public void Render_ForFilesAndDirectry_WriteOutput(String fileType)
    {
      // Arrange
      _uploadApplet = new UploadApplet();
      _privateObject = new PrivateObject(_uploadApplet);
      var htmlTextWriter = SetUp(fileType);

      // Act
      _privateObject.Invoke("Render", htmlTextWriter);

      // Assert
      htmlTextWriter.ShouldSatisfyAllConditions(
        () => htmlTextWriter.Indent.ShouldBe(0),
        () => htmlTextWriter.NewLine.ShouldBe(null));
    }
    
		[Test]
		public void ViewStateIntegerProperties_DefaultValue_ExpectDefaultValues()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.ShouldSatisfyAllConditions(
					() => testObject.ThumbnailColumnMaxWidth.ShouldBe(MinusOne),
					() => testObject.ImagePreviewHeight.ShouldBe(MinusOne),
					() => testObject.Height.ShouldBe(HeightDefaultValue),
					() => testObject.MaxFileSize.ShouldBe(MaxFileSizeDefaultValue),
					() => testObject.MaxUploadSize.ShouldBe(MaxUploadSizeDefaultValue));
			}
		}

		[Test]
		public void ViewStateIntegerProperties_SetValue_ExpectSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet
			{
				// Act
				ThumbnailColumnMaxWidth = TestValue,
				ImagePreviewHeight = TestValue,
				Height = TestValue,
				MaxFileSize = TestValue,
				MaxUploadSize = TestValue
			})
			{
				// Assert
				testObject.ShouldSatisfyAllConditions(
					() => testObject.ThumbnailColumnMaxWidth.ShouldBe(TestValue),
					() => testObject.ImagePreviewHeight.ShouldBe(TestValue),
					() => testObject.Height.ShouldBe(TestValue),
					() => testObject.MaxFileSize.ShouldBe(TestValue),
					() => testObject.MaxUploadSize.ShouldBe(TestValue));
			}
		}

		[Test]
		public void LabelButtonRemove_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.LabelButtonRemove.ShouldBe(string.Empty);
			}
		}

		[Test]
		public void LabelButtonRemove_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.LabelButtonRemove = TestValueString;

				// Assert
				testObject.LabelButtonRemove.ShouldBe(TestValueString);
			}
		}

		[Test]
		public void LabelButtonUpload_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.LabelButtonUpload.ShouldBe(string.Empty);
			}
		}

		[Test]
		public void LabelButtonUpload_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.LabelButtonUpload = TestValueString;

				// Assert
				testObject.LabelButtonUpload.ShouldBe(TestValueString);
			}
		}

		[Test]
		public void TableHeaderFilename_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.TableHeaderFilename.ShouldBe(string.Empty);
			}
		}

		[Test]
		public void TableHeaderFilename_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.TableHeaderFilename = TestValueString;

				// Assert
				testObject.TableHeaderFilename.ShouldBe(TestValueString);
			}
		}

		[Test]
		public void TableHeaderSize_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.TableHeaderSize.ShouldBe(string.Empty);
			}
		}

		[Test]
		public void TableHeaderSize_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.TableHeaderSize = TestValueString;

				// Assert
				testObject.TableHeaderSize.ShouldBe(TestValueString);
			}
		}

		[Test]
		public void DefaultDirectory_DefaultValue_ReturnsCDrive()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.DefaultDirectory.ShouldBe(DefaultDirectoryDefaultValue);
			}
		}

		[Test]
		public void DefaultDirectory_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.DefaultDirectory = TestValueString;

				// Assert
				testObject.DefaultDirectory.ShouldBe(TestValueString);
			}
		}

		public void RedirectTarget_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.RedirectTarget.ShouldBeEmpty();
			}
		}

		[Test]
		public void RedirectTarget_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.RedirectTarget = TestValueString;

				// Assert
				testObject.RedirectTarget.ShouldBe(TestValueString);
			}
		}

		public void ProxyPort_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act, Assert
				testObject.ProxyPort.ShouldBeEmpty();
			}
		}

		[Test]
		public void ProxyPort_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new UploadApplet())
			{
				// Act
				testObject.ProxyPort = TestValueString;

				// Assert
				testObject.ProxyPort.ShouldBe(TestValueString);
			}
		}
    
    private HtmlTextWriter SetUp(String param)
    {
      var textWriter = new Moq.Mock<TextWriter>();
      var htmlTextWriter = new HtmlTextWriter(textWriter.Object);
      _shims = ShimsContext.Create();
      var label = new LabelValueCollection();
      var item = new LabelValueCollectionItem("label", "value");
      label.Add(item);
      ShimUploadApplet.AllInstances.FileFiltersGet = (fd) => label;
      if (param.Equals("fd"))
      {
        ShimUploadApplet.AllInstances.UploadSelectionModeGet = (fd) => UploadSelectionMode.FilesAndDirectories;
      }
      else if (param.Equals("f"))
      {
        ShimUploadApplet.AllInstances.UploadSelectionModeGet = (fd) => UploadSelectionMode.FilesOnly;
      }
      else
      {
        ShimUploadApplet.AllInstances.UploadSelectionModeGet = (fd) => UploadSelectionMode.DirectoriesOnly;
      }
      return htmlTextWriter;
    }
	}
}
