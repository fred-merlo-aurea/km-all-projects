using System;
using System.Collections.Specialized;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.HtmlControls.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using System.Xml.Linq;
using KMPlatform.Object;
using KMPS.MD.Administration;
using KMPS.MD.Administration.Fakes;
using KMPS.MD.Objects.Fakes;
using KMPS.MDAdmin.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Administration
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CodesheetImportTest : BasePageTests
    {
        private const string Product = "PRODUCT IMPORT";
        private const string CodeSheet = "CODESHEET IMPORT AND MAPPING";
        private const string DummyValue = "DUMMY";
        private const string DropDownImport = "drpImport";
        private const string MethodButtonUpload = "btnUpload_Click";
        private const string ErrorMessage = "lblErrorMessage";
        private const string LabelMessage = "lblMessage";
        private const string Message = "divMessage";
        private const string Error = "divError";
        private const string FileName = "text.txt";
        private const string Document = "document";
        private const string Key = "key";
        private const string Value = "value";

        private CodesheetImport _testEntity;
        private PrivateObject _privateObject;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new CodesheetImport();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
        }

        [Test]
        public void ButtonUploadClick_ForInvalidFile_DisplayError()
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            ShimHtmlInputFile.AllInstances.PostedFileGet = (x) => new ShimHttpPostedFile();
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => string.Empty;

            // Act
            _privateObject.Invoke(MethodButtonUpload, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty(),
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(Product)]
        [TestCase(CodeSheet)]
        [TestCase(DummyValue)]
        public void ButtonUploadClick_ForFalseValidateUpdate_DisplayError(string param)
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            SetUpValidate(param);

            // Act
            _privateObject.Invoke(MethodButtonUpload, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty(),
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(Product, true)]
        public void ButtonUploadClick_ForValidateUpdate_ShouldUploadFile(string param, bool value)
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            SetUpButton(param, value);

            // Act
            _privateObject.Invoke(MethodButtonUpload, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(LabelMessage).Text.ShouldNotBeEmpty(),
                () => GetField<HtmlGenericControl>(Message).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(CodeSheet, true)]
        [TestCase(DummyValue, true)]
        [TestCase(CodeSheet, false)]
        [TestCase(DummyValue, false)]
        public void ButtonUploadClick_ForCodeSheet_ShouldDisplayError(string param, bool value)
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            SetUpButton(param, value);

            // Act
            _privateObject.Invoke(MethodButtonUpload, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty(),
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(Product)]
        [TestCase(DummyValue)]
        [TestCase(CodeSheet)]
        public void ButtonUploadClick_ForDifferentSelectedValues_ShouldThrowException(string param)
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            SetUpException(param);

            // Act
            _privateObject.Invoke(MethodButtonUpload, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty(),
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue());
        }

        private void SetUpValidate(string param)
        {
            ShimHtmlInputFile.AllInstances.PostedFileGet = (x) => new ShimHttpPostedFile();
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => FileName;
            var stream = new Moq.Mock<Stream>();
            stream.SetupGet(x => x.CanRead).Returns(true);
            ShimHttpPostedFile.AllInstances.InputStreamGet = (x) => stream.Object;
            if (param.Equals(Product))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => Product;
                GetField<DropDownList>(DropDownImport).SelectedValue = Product;
            }
            else if (param.Equals(CodeSheet))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => CodeSheet;
                GetField<DropDownList>(DropDownImport).SelectedValue = CodeSheet;
            }
        }

        private void SetUpButton(string param, bool value)
        {
            ShimHtmlInputFile.AllInstances.PostedFileGet = (x) => new ShimHttpPostedFile();
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => FileName;
            var stream = new Moq.Mock<Stream>();
            ShimHttpPostedFile.AllInstances.InputStreamGet = (x) => stream.Object;
            ShimCodesheetImport.AllInstances.ValidateUploadTxtHeadersStream = (x, y) => true;
            ShimCodesheetImport.AllInstances.MasterGet = (x) => new ShimSite();
            ShimSite.AllInstances.clientconnectionsGet = (x) => new ClientConnections();
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) => new ShimSqlConnection();
            ShimDataFunctions.executeScalarSqlCommandSqlConnection = (x, y) => new Object();
            var nameValueCollection = new NameValueCollection { { Key, Value } };
            if (param.Equals(Product))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => Product;
                GetField<DropDownList>(DropDownImport).SelectedValue = Product;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 = (x, y, z, c) => CreateTuple();
            }
            else if (param.Equals(CodeSheet) && value)
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => CodeSheet;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 = (x, y, z, c) => CreateTuple();
                GetField<DropDownList>(DropDownImport).SelectedValue = CodeSheet;
                Objects.Fakes.ShimCodeSheet.ImportClientConnectionsXDocumentInt32 =
                    (x, y, z) => nameValueCollection;
            }
            else if (param.Equals(CodeSheet) && !value)
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => CodeSheet;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 =
                    (x, y, z, c) => new Tuple<XDocument, string>(new XDocument(), Document);
                GetField<DropDownList>(DropDownImport).SelectedValue = CodeSheet;
                Objects.Fakes.ShimCodeSheet.ImportClientConnectionsXDocumentInt32 =
                    (x, y, z) => nameValueCollection;
            }
            else if (param.Equals(DummyValue) && value)
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => DummyValue;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 = (x, y, z, c) => CreateTuple();
                GetField<DropDownList>(DropDownImport).SelectedValue = DummyValue;
                ShimMasterGroup.ImportClientConnectionsXDocumentInt32 =
                    (x, y, z) => nameValueCollection;
            }
            else if (param.Equals(DummyValue) && !value)
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => DummyValue;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 =
                    (x, y, z, c) => new Tuple<XDocument, string>(new XDocument(), Document);
                GetField<DropDownList>(DropDownImport).SelectedValue = DummyValue;
                ShimMasterGroup.ImportClientConnectionsXDocumentInt32 =
                    (x, y, z) => nameValueCollection;
            }
        }

        private void SetUpException(string param)
        {
            ShimHtmlInputFile.AllInstances.PostedFileGet = (x) => new ShimHttpPostedFile();
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => FileName;
            var stream = new Moq.Mock<Stream>();
            stream.SetupGet(x => x.CanRead).Returns(true);
            ShimHttpPostedFile.AllInstances.InputStreamGet = (x) => stream.Object;
            ShimCodesheetImport.AllInstances.ValidateUploadTxtHeadersStream = (x, y) => true;
            if (param.Equals(Product))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => Product;
                GetField<DropDownList>(DropDownImport).SelectedValue = Product;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 = (x, y, z, c) => CreateTuple();
            }
            else if (param.Equals(CodeSheet))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => CodeSheet;
                GetField<DropDownList>(DropDownImport).SelectedValue = CodeSheet;
            }
            else if (param.Equals(DummyValue))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => DummyValue;
                ShimCodesheetImport.SerializeTxtFileStreamStringStringInt32 = (x, y, z, c) => CreateTuple();
                GetField<DropDownList>(DropDownImport).SelectedValue = DummyValue;
            }
        }

        public Tuple<XDocument, string> CreateTuple()
        {
            var xDocument = new XDocument();
            return new Tuple<XDocument, string>(xDocument, string.Empty);
        }
    }
}