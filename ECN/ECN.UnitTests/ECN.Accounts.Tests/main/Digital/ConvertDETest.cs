using System;
using System.IO.Fakes;
using System.Linq;
using ecn.accounts.main.Digital;
using NUnit.Framework;
using pdftron.PDF;
using pdftron.PDF.Fakes;
using Shouldly;
using ActionDelegate = System.Action;
using PDFAction = pdftron.PDF.Action;

namespace ECN.Accounts.Tests.main.Digital
{
    [TestFixture]
    public class ConvertDETest : EditionTestsBase<ConvertDE>
    {
        [Test]
        public void GetPageDetails_IfPdfDocHasNoPages_NoPagesAddedToObjectEdition()
        {
            // Arrange
            var pdfDoc = new PDFDoc();

            try
            {
                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            _objectEdition.PageCollection.Count.ShouldBe(0);
        }

        [Test]
        public void GetPageDetails_IfPdfDocHasThreePages_ThreePagesAddedIntoObjectEdition()
        {
            // Arrange
            var pdfDoc = new PDFDoc();

            try
            {
                var pdfPage01 = pdfDoc.PageCreate();
                var pdfPage02 = pdfDoc.PageCreate();
                var pdfPage03 = pdfDoc.PageCreate();

                pdfDoc.PagePushBack(pdfPage01);
                pdfDoc.PagePushBack(pdfPage02);
                pdfDoc.PagePushBack(pdfPage03);

                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            _objectEdition.PageCollection.Count.ShouldBe(3);
        }

        [Test]
        public void GetPageDetails_IfPdfPageHasGoToLink_ObjectEditionPageHasGoToLink()
        {
            // Arrange
            const int page01Index = 0;
            var pdfDoc = new PDFDoc();

            try
            {
                var pdfPage01 = pdfDoc.PageCreate();
                var pdfPage02 = pdfDoc.PageCreate();
                pdfDoc.PagePushBack(pdfPage01);
                pdfDoc.PagePushBack(pdfPage02);

                // create annot of pdfPage01
                // goto type of annot links to pdfPage02
                var destination = Destination.CreateFit(pdfPage02);
                var gotoAction = PDFAction.CreateGoto(destination);
                var linkAnnot = Annot.CreateLink(pdfDoc, new Rect(X1, Y1, X2, Y2), gotoAction);
                pdfPage01.AnnotInsert(0, linkAnnot);

                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            var page01LinkCollection = _objectEdition.PageCollection[page01Index].LinkCollection;
            var page01Links = ConvertToLinkArray(page01LinkCollection);

            page01Links.ShouldSatisfyAllConditions(
                () => page01Links.Length.ShouldBe(1),
                () => page01Links.ShouldContain(link => link.LinkType == GoToLinkType));
        }

        [Test]
        public void GetPageDetails_IfPdfPageHasUriLink_ObjectEditionPageUriLink()
        {
            // Arrange
            const int page02Index = 1;
            var pdfDoc = new PDFDoc();

            try
            {
                var pdfPage01 = pdfDoc.PageCreate();
                var pdfPage02 = pdfDoc.PageCreate();

                pdfDoc.PagePushBack(pdfPage01);
                pdfDoc.PagePushBack(pdfPage02);

                // create annot of pdfPage02
                // uri type of annot to an uri with mailto scheme
                var uriAcion = PDFAction.CreateURI(pdfDoc, EmailUriWithMailtoScheme);
                var linkAnnot = Annot.CreateLink(pdfDoc, new Rect(X1, Y1, X2, Y2), uriAcion);
                pdfPage02.AnnotInsert(0, linkAnnot);

                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            var page02LinkCollection = _objectEdition.PageCollection[page02Index].LinkCollection;
            var page02Links = ConvertToLinkArray(page02LinkCollection);

            page02Links.ShouldSatisfyAllConditions(
                () => page02Links.Length.ShouldBe(1),
                () => page02Links.ShouldContain(link => link.LinkType == UriLinkType),
                () => page02Links.First().LinkURL.ShouldStartWith(MailtoScheme));
        }

        [Test]
        public void GetPageDetails_IfPdfPageHasUriLinkWithoutScheme_ObjectEditionHasPageUriLinkAddedUriScheme()
        {
            // Arrange
            const int page02Index = 1;
            var pdfDoc = new PDFDoc();

            try
            {
                var pdfPage01 = pdfDoc.PageCreate();
                var pdfPage02 = pdfDoc.PageCreate();

                pdfDoc.PagePushBack(pdfPage01);
                pdfDoc.PagePushBack(pdfPage02);

                // create annot of pdfPage02
                // uri type of annot to an uri with mailto scheme
                var uriAcion = PDFAction.CreateURI(pdfDoc, WebUriWithoutScheme);
                var linkAnnot = Annot.CreateLink(pdfDoc, new Rect(X1, Y1, X2, Y2), uriAcion);
                pdfPage02.AnnotInsert(0, linkAnnot);

                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            var page02LinkCollection = _objectEdition.PageCollection[page02Index].LinkCollection;
            var page02Links = ConvertToLinkArray(page02LinkCollection);

            page02Links.ShouldSatisfyAllConditions(
                () => page02Links.Length.ShouldBe(1),
                () => page02Links.ShouldContain(link => link.LinkType == UriLinkType),
                () => page02Links.First().LinkURL.ShouldStartWith(HttpScheme));
        }

        [Test]
        public void GetPageDetails_IfPdfPageHasNoAnnots_ObjectEditionPageHasNoLinks()
        {
            // Arrange
            const int page01Index = 0;
            var pdfDoc = new PDFDoc();

            try
            {
                var pdfPage01 = pdfDoc.PageCreate();
                pdfDoc.PagePushBack(pdfPage01);

                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            var page01LinkCollection = _objectEdition.PageCollection[page01Index].LinkCollection;
            page01LinkCollection.Count.ShouldBe(0);
        }

        [Test]
        public void GetPageDetails_IfPdfDocHasPage_SetsObjectEditionPageProperties()
        {
            // Arrange
            const int page01Index = 0;
            const int pdfPage01Number = 1;
            var pdfDoc = new PDFDoc();

            int pdfPage01Width, pdfPage01Height;
            try
            {
                var pdfPage01 = pdfDoc.PageCreate();
                pdfPage01Width = (int)pdfPage01.GetPageWidth();
                pdfPage01Height = (int)pdfPage01.GetPageHeight();
                pdfDoc.PagePushBack(pdfPage01);
                AddTextDataToPdfPage(pdfDoc, pdfPage01, PdfPageText);

                // Act
                _testEntityPrivate.Invoke(GetPageDetailsMethodName, pdfDoc);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            // Assert
            var page01 = _objectEdition.PageCollection[page01Index];
            page01.PageNo.ShouldBe(pdfPage01Number);
            page01.DisplayNo.ShouldBe(pdfPage01Number.ToString());
            page01.TextContent.ShouldBe(PdfPageText);
            page01.Width.ShouldBe(pdfPage01Width);
            page01.Height.ShouldBe(pdfPage01Height);
        }

        [Test]
        public void GenerateImages_IfImageDirectoriesDoesNotExist_CreatesDirectoriesByCountOfResolutionsArray()
        {
            // Arrange
            const int pdfDocPageCount = 2;
            var expectedCreateDirectoryCallCount = _imageResolutions.Length;
            var thumbnailPath = string.Format("{0}{1}/", DummyImagePath, _thumbnailResolution);

            var createDirectoryCallCount = 0;
            ShimDirectory.ExistsString = (directoryPath) => { return false; };
            ShimDirectory.CreateDirectoryString = (directoryPath) =>
            {
                if (directoryPath != thumbnailPath)
                {
                    createDirectoryCallCount++;
                }
                return null;
            };

            ShimPDFDraw.AllInstances.ExportPageStringStringObj = (_, __, ___, ____, _____) => { };

            using (var pdfDoc = new PDFDoc())
            {
                for (var i = 0; i < pdfDocPageCount; i++)
                {
                    pdfDoc.PagePushBack(pdfDoc.PageCreate());
                }

                // Act
                _testEntityPrivate.Invoke(GenerateImagesMethodName, pdfDoc);
            }

            // Assert
            createDirectoryCallCount.ShouldBe(expectedCreateDirectoryCallCount);
        }

        [Test]
        public void GenerateImages_IfThumbnailDirectoriesDoesNotExist_CreatesDirectoriesByCountOfPages()
        {
            // Arrange
            const int pdfDocPageCount = 2;
            var thumbnailPath = string.Format("{0}{1}/", DummyImagePath, _thumbnailResolution);

            var createDirectoryCallCount = 0;
            ShimDirectory.ExistsString = (directoryPath) => { return false; };
            ShimDirectory.CreateDirectoryString = (directoryPath) =>
            {
                if (directoryPath == thumbnailPath)
                {
                    createDirectoryCallCount++;
                }
                return null;
            };

            ShimPDFDraw.AllInstances.ExportPageStringStringObj = (_, __, ___, ____, _____) => { };

            using (var pdfDoc = new PDFDoc())
            {
                for (var i = 0; i < pdfDocPageCount; i++)
                {
                    pdfDoc.PagePushBack(pdfDoc.PageCreate());
                }

                // Act
                _testEntityPrivate.Invoke(GenerateImagesMethodName, pdfDoc);
            }

            // Assert
            createDirectoryCallCount.ShouldBe(pdfDocPageCount);
        }

        [Test]
        public void GenerateImages_WhenCalled_GeneratesPageImages()
        {
            // Arrange
            const int pdfDocPageCount = 2;
            var expectedExportCount = _imageResolutions.Length * pdfDocPageCount;
            var thumbnailPath = string.Format("{0}{1}/", DummyImagePath, _thumbnailResolution);

            ShimDirectory.ExistsString = (directoryPath) => { return true; };
            ShimDirectory.CreateDirectoryString = (directoryPath) => { return null; };

            var exportCallCount = 0;
            ShimPDFDraw.AllInstances.ExportPageStringStringObj = (pdfDraw, pdfPage, fileName, fileFormat, encoderHints) =>
            {
                if (!fileName.StartsWith(thumbnailPath, StringComparison.OrdinalIgnoreCase) &&
                fileFormat.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    exportCallCount++;
                }
            };

            using (var pdfDoc = new PDFDoc())
            {
                for (var i = 0; i < pdfDocPageCount; i++)
                {
                    pdfDoc.PagePushBack(pdfDoc.PageCreate());
                }

                // Act
                _testEntityPrivate.Invoke(GenerateImagesMethodName, pdfDoc);
            }

            // Assert
            exportCallCount.ShouldBe(expectedExportCount);
        }

        [Test]
        public void GenerateImages_WhenCalled_GeneratesPageThumbnails()
        {
            // Arrange
            const int pdfDocPageCount = 2;
            var thumbnailPath = string.Format("{0}{1}/", DummyImagePath, _thumbnailResolution);

            ShimDirectory.ExistsString = (directoryPath) => { return true; };
            ShimDirectory.CreateDirectoryString = (directoryPath) => { return null; };

            var exportCallCount = 0;
            ShimPDFDraw.AllInstances.ExportPageStringStringObj = (pdfDraw, pdfPage, fileName, fileFormat, encoderHints) =>
            {
                if (fileName.StartsWith(thumbnailPath, StringComparison.OrdinalIgnoreCase) &&
                fileFormat.Equals("png", StringComparison.OrdinalIgnoreCase))
                {
                    exportCallCount++;
                }
            };

            using (var pdfDoc = new PDFDoc())
            {
                for (var i = 0; i < pdfDocPageCount; i++)
                {
                    pdfDoc.PagePushBack(pdfDoc.PageCreate());
                }

                // Act
                _testEntityPrivate.Invoke(GenerateImagesMethodName, pdfDoc);
            }

            // Assert
            exportCallCount.ShouldBe(pdfDocPageCount);
        }

        [Test]
        public void GenerateImages_IfAnErrorOccured_ThrowsException()
        {
            // Arrange
            ShimDirectory.ExistsString = (directoryPath) => { return true; };
            ShimDirectory.CreateDirectoryString = (directoryPath) => { return null; };

            ShimPDFDraw.AllInstances.ExportPageStringStringObj = (_, __, ___, ____, _____) =>
            {
                throw new Exception();
            };

            var generateImagesAction = default(ActionDelegate);
            using (var pdfDoc = new PDFDoc())
            {
                pdfDoc.PagePushBack(pdfDoc.PageCreate());

                // Act
                generateImagesAction = new ActionDelegate(() => { _testEntityPrivate.Invoke(GenerateImagesMethodName, pdfDoc); });
            }

            // Assert
            generateImagesAction.ShouldThrow<Exception>();
        }

        [Test]
        public void CalculateImageWidth_WhenCalled_ReturnsCalculatedImageWidth()
        {
            // Arrange
            const double width = 400d;
            const double height = 100d;
            const int resolution = 25;
            const int expectedValue = 100;

            // Act
            var returnedValue = _testEntityPrivate.Invoke(CalculateImageWidthMethodName, width, height, resolution) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(expectedValue));
        }
    }
}