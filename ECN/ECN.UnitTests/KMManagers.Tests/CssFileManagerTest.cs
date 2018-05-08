using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using System.Web.Configuration.Fakes;
using KMDbManagers;
using KMDbManagers.Fakes;
using KMEntities;
using KMEnums;
using KMManagers.Tests.Helpers;
using KMModels.PostModels;
using KMModels.PostModels.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMManagers.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class CssFileManagerTest
    {
        private const string Username = "dummy_username";
        private const string DummyString = "DummyString";
        private const string TempFile = "temp_DummyString";
        private const string TestUrl = "http://km.com";
        private const string DefaultCssPath = "DefaultCSSPath";
        private const string CssDir = "CssDir";
        private const string MergedCssString = ".abc{color:black;bg:bacl;}";
        private const string CssString = ".abc{color:red;bg:bacl;}";
        private const string SplitMinCssString = ".abc{color:black;}";
        private string _resultCss;
        private FileStream _fileStream;
        private CssFileManager _cssFileManager;
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            KMManagerTestsHelper.CreateWebAppSettingsShim();
            KMManagerTestsHelper.CreateMXValidateShim();
            _fileStream = File.Create(Path.GetFullPath(TempFile));
        }

        [TearDown]
        public void TestCleanUp()
        {
            _fileStream.Close();
            File.Delete(Path.GetFullPath(TempFile));
            _shimObject.Dispose();
        }

        [Test]
        public void CompareAndMergeFull_Success_CssIsMerged()
        {
            // Arranage
            SetupForMethodCompareAndMergeFull();
            _resultCss = string.Empty;

            // Act
            _cssFileManager.CompareAndMergeFull(DummyString);

            // Assert
            _cssFileManager.ShouldSatisfyAllConditions(
                () => _resultCss.ShouldNotBeNullOrWhiteSpace(),
                () => _resultCss.ShouldBe(MergedCssString));
        }

        [Test]
        public void CompareAndMergeFull_WhenSingleCssClass_ReturnsNewCss()
        {
            // Arranage
            SetupForMethodCompareAndMergeFull();
            var counter = 0;
            ShimStreamReader.AllInstances.ReadLine = (x) =>
            {
                counter++;
                return counter < GetCssStringListWithSingleClass().Count ? GetCssStringListWithSingleClass()[counter] : null;
            };
            _resultCss = string.Empty;

            // Act
            _cssFileManager.CompareAndMergeFull(DummyString);

            // Assert
            _cssFileManager.ShouldSatisfyAllConditions(
                () => _resultCss.ShouldNotBeNullOrWhiteSpace(),
                () => _resultCss.ShouldBe(CssString));
        }

        [Test]
        public void CompareAndSplitMin_Success_ReturnsMinSplitString()
        {
            // Arranage
            SetupForMethodCompareAndMergeFull();
            var counter = 0;
            ShimStreamReader.AllInstances.ReadLine = (x) =>
            {
                counter++;
                return counter < GetCssStringList().Count ? GetCssStringList()[counter] : null;
            };
            _resultCss = string.Empty;

            // Act
            _cssFileManager.CompareAndSplitMin(DummyString);

            // Assert
            _cssFileManager.ShouldSatisfyAllConditions(
                () => _resultCss.ShouldNotBeNullOrWhiteSpace(),
                () => _resultCss.ShouldBe(SplitMinCssString));
        }

        [Test]
        public void CompareAndSplitMinReleaseScript_Success_ReturnsMinSplitString()
        {
            // Arranage
            SetupForMethodCompareAndMergeFull();
            var counter = 0;
            ShimStreamReader.AllInstances.ReadLine = (x) =>
            {
                counter++;
                return counter < GetCssStringList().Count ? GetCssStringList()[counter] : null;
            };
            _resultCss = string.Empty;

            // Act
            _cssFileManager.CompareAndSplitMinReleaseScript(DummyString, DummyString);

            // Assert
            _cssFileManager.ShouldSatisfyAllConditions(
                () => _resultCss.ShouldNotBeNullOrWhiteSpace(),
                () => _resultCss.ShouldBe(SplitMinCssString));
        }

        [TestCase(StylingType.Upload)]
        [TestCase(StylingType.Custom)]
        [TestCase(StylingType.External)]
        public void Update_Success_CssIsUpdated(StylingType styling)
        {
            // Arranage
            SetupForMethodCompareAndMergeFull();
            Fakes.ShimCssFileManager.AllInstances.CompareAndMergeFullString = (x, y) => { };
            ShimFile.ReadAllTextString = (x) => DummyString;
            ShimFormStylesPostModel.AllInstances.RewriteCssString = (x, y) => DummyString;
            var cssUpdated = false;
            ShimDbManagerBase.AllInstances.SaveChanges = (x) => 
            {
                cssUpdated = true;
            };
            var model = new FormStylesPostModel
            {
                ExternalUrl = TestUrl,
                StylingType = styling,
                File = new KMModels.CssFile
                {
                    Name = DummyString
                }
            };

            // Act

            _cssFileManager.Update(new User(), 1, model);

            // Assert
            cssUpdated.ShouldBeTrue();
        }

        private void SetupForMethodCompareAndMergeFull()
        {
            ShimCssFileDbManager.StaticConstructor = () => { };
            ShimCssFileDbManager.AllInstances.AddOnlyCssFile = (x, y) => { };
            ShimCssFileDbManager.AllInstances.AddCssFile = (x, y) => { };
            ShimDbResolver.Constructor = (db) =>
            {
                db.CssClassDbManager = new CssClassDbManager();
                db.FormDbManager = new FormDbManager();
                db.CssFileDbManager = new CssFileDbManager();
            };
            ShimCssClassDbManager.AllInstances.GetAll = (x) => new List<CssClass>();
            _cssFileManager = new CssFileManager();
            ShimFile.ExistsString = (x) => false;
            ShimFile.CopyStringString = (x, y) => { };
            ShimFile.OpenStringFileModeFileAccess = (x, y, z) => new ShimFileStream
            {
                CanReadGet = () => true,
                CanWriteGet = () => true
            };
            ShimStreamReader.ConstructorStream = (reader, stream) => new ShimStreamReader();
            ShimWebConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                [CssDir] = string.Empty,
                [DefaultCssPath] = DummyString
            };
            var counter = 0;
            ShimStreamReader.AllInstances.ReadLine = (x) =>
            {
                counter++;
                return counter < GetCssStringList().Count ? GetCssStringList()[counter] : null;
            };
            _resultCss = string.Empty;
            ShimTextWriter.AllInstances.WriteLineString = (item, line) =>
            {
                _resultCss += line;
            };
            ShimFormDbManager.AllInstances.GetByIDInt32Int32 = (x, y, z) => new Form
            {
                CssFile_Seq_ID = 1,
                CssFile = new CssFile
                {
                    Name = DummyString
                }
            };
            ShimCssFileDbManager.AllInstances.DeleteByIDInt32 = (x, y) => { };
        }

        private List<string> GetCssStringList()
        {
            return new List<string>
            {
                "",
                ".abc",
                "{",
                "color:red;",
                "bg:bacl;",
                "}",
                ".abc",
                "{",
                "color:black;",
                "cd:bacl;",
                "}",
            };
        }

        private List<string> GetCssStringListWithSingleClass()
        {
            return new List<string>
            {
                "",
                ".abc",
                "{",
                "color:red;",
                "bg:bacl;",
                "}",
            };
        }
    }
}