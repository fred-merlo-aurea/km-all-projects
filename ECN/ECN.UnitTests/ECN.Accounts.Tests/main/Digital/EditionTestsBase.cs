using System;
using System.Configuration;
using ecn.publisher.classes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using pdftron;
using pdftron.PDF;
using ASPNet = System.Web.UI;

namespace ECN.Accounts.Tests.main.Digital
{
    public abstract class EditionTestsBase<T> where T : ASPNet.Page, new()
    {
        protected internal const string PdfTronLicenseAppSettingsKey = "PDFTron_LicenseKey";
        protected internal const string GetPageDetailsMethodName = "GetPageDetails";
        protected internal const string GenerateImagesMethodName = "GenerateImages";
        protected internal const string CalculateImageWidthMethodName = "CalculateImageWidth";
        protected internal const string ObjectEditionFieldName = "objEdition";
        protected internal const string GoToLinkType = "GoTo";
        protected internal const string UriLinkType = "URI";
        protected internal const string MailtoScheme = "mailto";
        protected internal const string HttpScheme = "http";
        protected internal const string EmailUriWithMailtoScheme = "mailto:mailbox@example.com";
        protected internal const string WebUriWithoutScheme = "www.example.com";
        protected internal const string PdfPageText = "lorem ipsum dolor sit amet";
        protected internal const int X1 = 0;
        protected internal const int X2 = 100;
        protected internal const int Y1 = 0;
        protected internal const int Y2 = 100;
        protected internal const string DummyImagePath = "DummyPath/";
        protected internal const string ImagePathFieldName = "_imagePath";
        protected internal const string ThumbnailSizeFieldName = "_thumbnailResolution";
        protected internal const string ResolutionsArrayFieldName = "_imageResolutions";

        protected T _testEntity;
        protected PrivateObject _testEntityPrivate;
        protected Edition _objectEdition;
        protected int[] _imageResolutions;
        protected int _thumbnailResolution;
        protected IDisposable _shimsContext;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            PDFNet.Initialize(ConfigurationManager.AppSettings[PdfTronLicenseAppSettingsKey].ToString());
        }

        [SetUp]
        public virtual void SetUp()
        {
            _shimsContext = ShimsContext.Create();

            _testEntity = new T();
            _testEntityPrivate = new PrivateObject(_testEntity);
            _testEntityPrivate.SetField(ImagePathFieldName, DummyImagePath);

            _objectEdition = (Edition)_testEntityPrivate.GetField(ObjectEditionFieldName);
            var imageResolutions = _testEntityPrivate.GetField(ResolutionsArrayFieldName) as int[];
            var thumbnailResolution = _testEntityPrivate.GetField(ThumbnailSizeFieldName) as int?;

            _imageResolutions = imageResolutions ?? new int[] { 100, 200, 300 };
            _thumbnailResolution = thumbnailResolution ?? 150;
        }

        [TearDown]
        public virtual void TearDown()
        {
            _shimsContext.Dispose();
        }

        protected void AddTextDataToPdfPage(PDFDoc pdfDoc, pdftron.PDF.Page pdfPage, string text)
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

        protected Link[] ConvertToLinkArray(Links linkCollection)
        {
            var linkArray = new Link[linkCollection.Count];

            for (int i = 0; i < linkCollection.Count; i++)
            {
                linkArray[i] = linkCollection[i];
            }

            return linkArray;
        }
    }
}