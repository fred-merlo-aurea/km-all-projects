using System;
using System.Configuration;
using ecn.publisher.helpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using pdftron;
using pdftron.PDF;
using pdftron.PDF.Fakes;
using Shouldly;
using PDFAction = pdftron.PDF.Action;

namespace ECN.Tests.Publisher.Helpers
{
    [TestFixture]
    public class PDFNetExtensionsTest
    {
        protected const string PdfTronLicenseAppSettingsKey = "PDFTron_LicenseKey";
        protected const string PdfPageText = "lorem ipsum dolor sit amet";

        private IDisposable _shimsContext;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            PDFNet.Initialize(ConfigurationManager.AppSettings[PdfTronLicenseAppSettingsKey].ToString());
        }

        [SetUp]
        public virtual void SetUp()
        {
            _shimsContext = ShimsContext.Create();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void GetTextContent_ReturnsTextContentOfPdfPage()
        {
            // Arrange
            var pdfDoc = new PDFDoc();
            var pdfPage01 = pdfDoc.PageCreate();
            AddTextDataToPdfPage(pdfDoc, pdfPage01, PdfPageText);

            // Act
            var returnedData = PDFNetExtensions.GetTextContent(pdfPage01);

            // Assert
            returnedData.ShouldSatisfyAllConditions(
                () => returnedData.ShouldBeOfType<string>(),
                () => returnedData.ShouldBe(PdfPageText));
        }

        [Test]
        public void ToTableOfContents_IfBookmarkIsNull_ReturnsEmptyString()
        {
            // Arrange
            var bookmark = default(Bookmark);

            // Act
            var returnedValue = PDFNetExtensions.ToTableOfContents(bookmark);

            // Assert
            returnedValue.ShouldBeEmpty();
        }

        [Test]
        public void ToTableOfContents_IfBookmarkIsNotValid_ReturnsEmptyString()
        {
            // Arrange
            ShimBookmark shimBookmark = new ShimBookmark();
            shimBookmark.IsValid = () => { return false; };

            var bookmark = shimBookmark.Instance;

            // Act
            var returnedValue = PDFNetExtensions.ToTableOfContents(bookmark);

            // Arrange
            returnedValue.ShouldBeEmpty();
        }

        [Test]
        public void ToTableOfContents_IfActionOfBookmarkIsNotValid_ReturnsEmptyString()
        {
            // Arrange
            ShimAction shimAction = new ShimAction();
            shimAction.IsValid = () => { return false; };

            ShimBookmark shimBookmark = new ShimBookmark();
            shimBookmark.IsValid = () => { return false; };
            shimBookmark.GetNext = () => { return shimBookmark.Instance; };
            shimBookmark.GetAction = () => { return shimAction.Instance; };

            var bookmark = shimBookmark.Instance;

            // Act
            var returnedValue = PDFNetExtensions.ToTableOfContents(bookmark);

            // Arrange
            returnedValue.ShouldBeEmpty();
        }

        [Test]
        public void ToTableOfContents_IfActionTypeIsNotGoTo_ReturnsEmptyString()
        {
            // Arrange
            ShimAction shimAction = new ShimAction();
            shimAction.IsValid = () => { return true; };
            shimAction.GetType = () => { return PDFAction.Type.e_Unknown; };

            ShimBookmark shimBookmark = new ShimBookmark();
            shimBookmark.IsValid = () => { return false; };
            shimBookmark.GetNext = () => { return shimBookmark.Instance; };
            shimBookmark.GetAction = () => { return shimAction.Instance; };

            var bookmark = shimBookmark.Instance;

            // Act
            var returnedValue = PDFNetExtensions.ToTableOfContents(bookmark);

            // Arrange
            returnedValue.ShouldBeEmpty();
        }

        [Test]
        public void ToTableOfContents_IfActionIsValidAndIsGoToType_ReturnsTableOfContents()
        {
            // Arrange
            var returnedValue = string.Empty;
            var expectedBookmarkValue = string.Empty;
            var bookmarkTemplate = "<bookmark pageno='{0}' title='{1}'></bookmark>";

            using (var pdfDoc = new PDFDoc())
            {
                var pdfPage01 = pdfDoc.PageCreate();
                pdfDoc.PagePushBack(pdfPage01);
                var page01Number = pdfPage01.GetIndex();

                var bookmark01Title = "Bookmark to Page01";
                var bookmark01 = Bookmark.Create(pdfDoc, bookmark01Title);
                var destinationToPage01 = Destination.CreateFit(pdfPage01);
                var gotoActionToPage01 = PDFAction.CreateGoto(destinationToPage01);
                bookmark01.SetAction(gotoActionToPage01);

                expectedBookmarkValue = string.Format(bookmarkTemplate, page01Number, bookmark01Title);

                // Act
                returnedValue = PDFNetExtensions.ToTableOfContents(bookmark01);
            }

            // Arrange
            returnedValue.ShouldBe(expectedBookmarkValue);
        }

        [Test]
        public void ToTableOfContents_IfActionIsValidAndIsGoToTypeAndHasChildren_ReturnsTableOfContents()
        {
            // Arrange
            var returnedValue = string.Empty;
            var expectedBookmarkValue = string.Empty;
            var bookmarkTemplate = "<bookmark pageno='{0}' title='{1}'>{2}</bookmark>";

            using (var pdfDoc = new PDFDoc())
            {
                var pdfPage01 = pdfDoc.PageCreate();
                pdfDoc.PagePushBack(pdfPage01);
                var page01Number = pdfPage01.GetIndex();

                var pdfPage02 = pdfDoc.PageCreate();
                pdfDoc.PagePushBack(pdfPage02);
                var page02Number = pdfPage02.GetIndex();

                var bookmark01Title = "Bookmark to Page01";
                var bookmark01 = Bookmark.Create(pdfDoc, bookmark01Title);
                var destinationToPage01 = Destination.CreateFit(pdfPage01);
                var gotoActionToPage01 = PDFAction.CreateGoto(destinationToPage01);
                bookmark01.SetAction(gotoActionToPage01);

                var bookmark02Title = "Bookmark to Page02";
                var bookmark02 = Bookmark.Create(pdfDoc, bookmark02Title);
                var destinationToPage02 = Destination.CreateFit(pdfPage02);
                var gotoActionToPage02 = PDFAction.CreateGoto(destinationToPage02);
                bookmark02.SetAction(gotoActionToPage02);
                bookmark01.AddChild(bookmark02);

                var childBookmarkValue = string.Format(bookmarkTemplate, page02Number, bookmark02Title, string.Empty);
                expectedBookmarkValue = string.Format(bookmarkTemplate, page01Number, bookmark01Title, childBookmarkValue);

                // Act
                returnedValue = PDFNetExtensions.ToTableOfContents(bookmark01);
            }

            // Arrange
            returnedValue.ShouldBe(expectedBookmarkValue);
        }

        private void AddTextDataToPdfPage(PDFDoc pdfDoc, Page pdfPage, string text)
        {
            var elementWriter = new ElementWriter();

            try
            {
                elementWriter.Begin(pdfPage);

                var elementBuilder = new ElementBuilder();
                var textFont = Font.Create(pdfDoc, Font.StandardType1Font.e_times_roman);
                var textFontSize = 10.0;
                var textElement = elementBuilder.CreateTextBegin(textFont, textFontSize);
                elementWriter.WriteElement(textElement);
                elementBuilder.CreateTextRun(text);
                elementWriter.WriteElement(textElement);
                var textEnd = elementBuilder.CreateTextEnd();
                elementWriter.WriteElement(textEnd);

                elementWriter.Flush();
                elementWriter.End();
            }
            finally
            {
                elementWriter.Dispose();
            }
        }
    }
}