using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Fakes;
using System.Windows.Fakes;
using DataCompare.Modules.Fakes;
using DQM.Modules.Fakes;
using FileMapperWizard.Modules.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Common;
using Home = DataCompare.Modules.Home;
using ShimHome = DataCompare.Modules.Fakes.ShimHome;

namespace UAS.UnitTests
{
    [TestFixture]
    public class HomeTest
    {
        private const string NewMappingTileTagName = "newMappingTile";
        private const string EditExistingTileTagName = "editExistingTile";
        private const string UploadTileTagName = "uploadTile";
        private const string ViewTileTagName = "viewTile";
        private const string FileAnalysisTagName = "fileAnalysis";

        private IDisposable _shimObject;
        private bool _isShowDialogCalled = false;
        private bool _isChildAddedToDockPanel = false;

        public void SetupForDisplayTileWindow()
        {
            _shimObject = ShimsContext.Create();

            ShimHome.ConstructorAppData = (home, appdata) => { };

            ShimFMUniversal.ConstructorBooleanBooleanClientProductStringBoolean =
                (fMUniversal, edit, isCirc, client, prod, filePath, isDataCompare) => new ShimFMUniversal();

            ShimUIElementCollection.AllInstances.AddUIElement =
                (element, number) => { return default(int); };

            _isShowDialogCalled = false;
            ShimWindow.AllInstances.ShowDialog = isShow =>
            {
                _isShowDialogCalled = true;
                return null;
            };
        }

        public void SetupForAddElementAsChildToDockPanel(TagNameEnum tagName)
        {
            _shimObject = ShimsContext.Create();

            ShimHome.ConstructorAppData = (home, appdata) => { };

            SetUpConstructorBasedOnTagName(tagName);

            ShimApplication.CurrentGet = () => { return new ShimApplication(); };

            ShimApplication.AllInstances.MainWindowGet = window => { return new ShimWindow(); };

            _isChildAddedToDockPanel = false;
            ShimUIElementCollection.AllInstances.AddUIElement =
                (element, number) =>
                {
                    _isChildAddedToDockPanel = true;
                    return default(int);
                };

            ShimHome.AllInstances.FindChild = dockPanel => { return new DockPanel(); };
        }

        private void SetUpConstructorBasedOnTagName(TagNameEnum tagName)
        {
            switch (tagName)
            {
                case TagNameEnum.FileAnalysis:
                    ShimFileAnalysis.Constructor = fileAnalysis => { };
                    break;
                case TagNameEnum.UploadTile:
                    ShimADMS_FTP_FileUpload.Constructor = fileupload => { };
                    break;
                case TagNameEnum.ViewTile:
                    ShimCompareViewer.Constructor = compareViewer => { };
                    break;
            }
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void TileOpen_TagNewMappingTile_DisplayMappingTileWindow()
        {
            //Arrange
            SetupForDisplayTileWindow();
            var homeUserControl = new Home(null);
            var button = new Button {Tag = NewMappingTileTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Tile_Open", button, null);

            //Assert
            _isShowDialogCalled.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void TileOpen_TagEditExistingTile_DisplayExistingTileWindow()
        {
            //Arrange
            SetupForDisplayTileWindow();
            var homeUserControl = new Home(null);
            var button = new Button {Tag = EditExistingTileTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Tile_Open", button, null);

            //Assert
            _isShowDialogCalled.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void TileOpen_TagUploadTile_AddsFileUploadElementAsChildToDockPanel()
        {
            //Arrange
            SetupForAddElementAsChildToDockPanel(TagNameEnum.UploadTile);
            var homeUserControl = new Home(null);
            var button = new Button {Tag = UploadTileTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Tile_Open", button, null);

            //Assert
            _isChildAddedToDockPanel.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void TileOpen_TagViewTile_AddsViewTileElementAsChildToDockPanel()
        {
            //Arrange
            SetupForAddElementAsChildToDockPanel(TagNameEnum.ViewTile);
            var homeUserControl = new Home(null);
            var button = new Button {Tag = ViewTileTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Tile_Open", button, null);

            //Assert
            _isChildAddedToDockPanel.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void TileOpen_TagFileAnalysis_AddsFileAnalysisElementAsChildToDockPanel()
        {
            //Arrange
            SetupForAddElementAsChildToDockPanel(TagNameEnum.FileAnalysis);
            var homeUserControl = new Home(null);
            var button = new Button {Tag = FileAnalysisTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Tile_Open", button, null);

            //Assert
            _isChildAddedToDockPanel.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void BorderMouseUp_TagNewMappingTile_DisplayMappingTileWindow()
        {
            //Arrange
            SetupForDisplayTileWindow();
            var homeUserControl = new Home(null);
            var border = new Border {Tag = NewMappingTileTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Border_MouseUp", border, null);

            //Assert
            _isShowDialogCalled.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void BorderMouseUp_TagEditExistingTile_DisplayExistingTileWindow()
        {
            //Arrange
            SetupForDisplayTileWindow();
            var homeUserControl = new Home(null);
            var border = new Border {Tag = EditExistingTileTagName };

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Border_MouseUp", border, null);

            //Assert
            _isShowDialogCalled.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void BorderMouseUp_TagUploadTile_AddsFileUploadElementAsChildToDockPanel()
        {
            //Arrange
            SetupForAddElementAsChildToDockPanel(TagNameEnum.UploadTile);
            var homeUserControl = new Home(null);
            var border = new Border {Tag = UploadTileTagName};

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Border_MouseUp", border, null);

            //Assert
            _isChildAddedToDockPanel.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void BorderMouseUp_TagViewTile_AddsViewTileElementAsChildToDockPanel()
        {
            //Arrange
            SetupForAddElementAsChildToDockPanel(TagNameEnum.ViewTile);
            var homeUserControl = new Home(null);
            var border = new Border {Tag = ViewTileTagName};

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Border_MouseUp", border, null);

            //Assert
            _isChildAddedToDockPanel.ShouldBeTrue();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void BorderMouseUp_TagFileAnalysis_AddsFileAnalysisElementAsChildToDockPanel()
        {
            //Arrange
            SetupForAddElementAsChildToDockPanel(TagNameEnum.FileAnalysis);
            var homeUserControl = new Home(null);
            var border = new Border {Tag = FileAnalysisTagName};

            //Act
            var privateObject = new PrivateObject(homeUserControl);
            privateObject.Invoke("Border_MouseUp", border, null);

            //Assert
            _isChildAddedToDockPanel.ShouldBeTrue();
        }
    }
}
