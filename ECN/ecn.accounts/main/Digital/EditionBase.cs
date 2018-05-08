using System;
using System.IO;
using ecn.publisher.classes;
using ecn.publisher.helpers;
using ECN_Framework;
using PDF = pdftron.PDF;

namespace ecn.accounts.main.Digital
{
    public class EditionBase : WebPageHelper
    {
        private const string FileFormatJpeg = "JPEG";
        private const string FileFormatPng = "PNG";
        private const string JpegQualityEncoderName = "Quality";
        private const int JpegQualityEncoderValue = 90;

        protected Edition objEdition = new Edition();
        protected string _imagePath = string.Empty;
        protected readonly int _thumbnailResolution = 150;
        protected int[] _imageResolutions = { 450, 618, 750, 874, 1050, 1130, 1290 };

        protected void GetPageDetails(PDF.PDFDoc pdfDoc)
        {
            var editionPages = pdfDoc.PagesToEcnPages();

            foreach (Page editionPage in editionPages)
            {
                objEdition.AddPage(editionPage);
            }
        }

        protected void GenerateImages(PDF.PDFDoc pdfDoc)
        {
            PDF.Page pdfPage;
            int pageNumber;
            int imageWidth;
            int imageHeight;
            int thumbWidth;
            int thumbHeight;
            string imageExportPath;
            string thumbnailExportPath;
            string imageFileName;
            string thumbFileName;
            double pageWidth;
            double pageHeight;

            for (var pageIterator = pdfDoc.GetPageIterator(); pageIterator.HasNext(); pageIterator.Next())
            {
                pdfPage = pageIterator.Current();
                pageNumber = pageIterator.GetPageNumber();
                pageWidth = pdfPage.GetPageWidth();
                pageHeight = pdfPage.GetPageHeight();

                try
                {
                    for (var i = 0; i < _imageResolutions.Length; i++)
                    {
                        imageExportPath = string.Format("{0}{1}/", _imagePath, _imageResolutions[i]);

                        if (pageNumber == 1 && !Directory.Exists(imageExportPath))
                        {
                            Directory.CreateDirectory(imageExportPath);
                        }

                        imageWidth = CalculateImageWidth(pageWidth, pageHeight, _imageResolutions[i]);
                        imageHeight = _imageResolutions[i];
                        imageFileName = string.Format("{0}{1:d}.jpg", imageExportPath, pageNumber);
                        pdfPage.ExportAsJpeg(imageFileName, imageWidth, imageHeight, 90);
                    }

                    thumbnailExportPath = string.Format("{0}{1}/", _imagePath, _thumbnailResolution);
                    if (!Directory.Exists(thumbnailExportPath))
                    {
                        Directory.CreateDirectory(thumbnailExportPath);
                    }

                    // Create Thumbnail Image
                    thumbWidth = CalculateImageWidth(pageWidth, pageHeight, _thumbnailResolution);
                    thumbHeight = _thumbnailResolution;
                    thumbFileName = string.Format("{0}{1:d}.png", thumbnailExportPath, pageNumber);
                    pdfPage.ExportAsPng(thumbFileName, thumbWidth, thumbHeight);
                }
                catch (Exception ex)
                {
                    var exceptionMessage = string.Format("{0} -  Page No - {1}", ex.Message, pageNumber);
                    throw new InvalidOperationException(exceptionMessage, ex);
                }
            }
        }

        protected int CalculateImageWidth(double width, double height, int resolution)
        {
            if (height == 0)
            {
                throw new ArgumentException("Height parameter could not be zero", "height");
            }

            return (int)(width * resolution / height);
        }
    }
}