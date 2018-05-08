using System.Text;
using ecn.publisher.classes;
using pdftron.SDF;
using PDF = pdftron.PDF;

namespace ecn.publisher.helpers
{
    public static class PDFNetExtensions
    {
        private const string EcnGoToLinkType = "GoTo";
        private const string EcnURILinkType = "URI";
        private const string UriHttpScheme = "http";
        private const string UriHttpsScheme = "https";
        private const string UriMailtoScheme = "mailto";
        private const string FileFormatJpeg = "JPEG";
        private const string FileFormatPng = "PNG";
        private const string JpegQualityEncoderName = "Quality";
        private const int ImageDefaultDpi = 72;

        public static Pages PagesToEcnPages(this PDF.PDFDoc pdfDoc)
        {
            int pageNumber;
            Page ecnPage;
            PDF.Page pdfPage;

            var ecnPages = new Pages();

            for (var pageIterator = pdfDoc.GetPageIterator(); pageIterator.HasNext(); pageIterator.Next())
            {
                pdfPage = pageIterator.Current();
                pageNumber = pageIterator.GetPageNumber();

                ecnPage = ToEcnPage(pdfPage, pageNumber);
                ecnPages.Add(ecnPage);
            }

            return ecnPages;
        }

        public static Page ToEcnPage(this PDF.Page pdfpage, int pageNumber)
        {
            PDF.Annot annot;
            Link ecnLink;

            var ecnPage = new Page();
            var annotCount = pdfpage.GetNumAnnots();

            for (int i = 0; i < annotCount; ++i)
            {
                annot = pdfpage.GetAnnot(i);
                ecnLink = ToEcnLink(annot);

                if (ecnLink != null)
                {
                    var bbox = annot.GetRect();
                    var cbox = pdfpage.GetCropBox();

                    var linkX1 = (int)(bbox.x1 - cbox.x1);
                    var linkY1 = (int)(pdfpage.GetPageHeight() - bbox.y2 + cbox.y1);
                    var linkX2 = (int)(bbox.x2 - cbox.x1);
                    var linkY2 = (int)(pdfpage.GetPageHeight() - bbox.y1 + cbox.y1);

                    ecnLink.x1 = linkX1;
                    ecnLink.x2 = linkX2;
                    ecnLink.y1 = linkY1;
                    ecnLink.y2 = linkY2;

                    ecnPage.AddLink(ecnLink);
                }
            }

            ecnPage.PageNo = pageNumber;
            ecnPage.DisplayNo = pageNumber.ToString();
            ecnPage.Width = (int)pdfpage.GetPageWidth();
            ecnPage.Height = (int)pdfpage.GetPageHeight();
            ecnPage.TextContent = GetTextContent(pdfpage);

            return ecnPage;
        }

        public static Link ToEcnLink(this PDF.Annot annot)
        {
            var ecnLink = default(Link);
            var ecnLinkURL = string.Empty;

            var annotType = annot.GetType();

            if (annotType == PDF.Annot.Type.e_Link)
            {
                var action = annot.GetLinkAction();
                var actionType = action.GetType();

                switch (actionType)
                {
                    case PDF.Action.Type.e_GoTo:
                        ecnLinkURL = action.GetDest().GetPage().GetIndex().ToString();
                        ecnLink = new Link(EcnGoToLinkType, ecnLinkURL);
                        break;

                    case PDF.Action.Type.e_URI:
                        try
                        {
                            ecnLinkURL = action.GetSDFObj().Get(EcnURILinkType).Value().GetAsPDFText();
                        }
                        catch
                        {
                            ecnLinkURL = string.Empty;
                        }

                        if (!(ecnLinkURL.StartsWith(string.Format("{0}://", UriHttpScheme), System.StringComparison.OrdinalIgnoreCase) ||
                            ecnLinkURL.StartsWith(string.Format("{0}:", UriMailtoScheme), System.StringComparison.OrdinalIgnoreCase) ||
                            ecnLinkURL.StartsWith(string.Format("{0}://", UriHttpsScheme), System.StringComparison.OrdinalIgnoreCase)))
                        {
                            ecnLinkURL = string.Format("{0}://{1}", UriHttpScheme, ecnLinkURL);
                        }

                        ecnLink = new Link(EcnURILinkType, ecnLinkURL);
                        break;
                    default:
                        ecnLink = null;
                        break;
                }
            }

            return ecnLink;
        }

        public static string GetTextContent(this PDF.Page pdfPage)
        {
            var element = default(PDF.Element);
            var elementReader = new PDF.ElementReader();
            var elementType = PDF.Element.Type.e_null;
            var textBuilder = new StringBuilder();

            elementReader.Begin(pdfPage);

            while ((element = elementReader.Next()) != null) // Read page contents
            {
                elementType = element.GetType();

                if (elementType == PDF.Element.Type.e_text) // Process text strings...
                {
                    textBuilder.Append(element.GetTextString());
                }
            }

            elementReader.End();
            return textBuilder.ToString();
        }

        public static string ToTableOfContents(this PDF.Bookmark item)
        {
            var contentBuilder = new StringBuilder();

            if (item != null)
            {
                for (; item.IsValid(); item = item.GetNext())
                {
                    var action = item.GetAction();
                    if (action.IsValid() && action.GetType() == PDF.Action.Type.e_GoTo)
                    {
                        var destination = action.GetDest();
                        contentBuilder.AppendFormat("<bookmark pageno='{0}' title='{1}'>", CleanString(destination.GetPage().GetIndex().ToString()), CleanString(item.GetTitle()));

                        if (item.HasChildren())  // Recursively get children sub-trees
                        {
                            contentBuilder.Append(item.GetFirstChild().ToTableOfContents());
                        }

                        contentBuilder.Append("</bookmark>");
                    }
                }
            }
            return contentBuilder.ToString();
        }

        public static void ExportAsJpeg(this PDF.Page pdfPage, string fileName, int width, int height, int quality)
        {
            // Use optional encoder parameter to specify JPEG quality.
            var hintSet = new ObjSet();
            var jpegQualityEncoder = hintSet.CreateDict();
            jpegQualityEncoder.PutNumber(JpegQualityEncoderName, quality);

            ExportAsImage(pdfPage, FileFormatJpeg, fileName, width, height, jpegQualityEncoder);
        }

        public static void ExportAsPng(this PDF.Page pdfPage, string fileName, int width, int height)
        {
            ExportAsImage(pdfPage, FileFormatPng, fileName, width, height, null);
        }

        private static void ExportAsImage(this PDF.Page pdfPage, string format, string fileName, int width, int height, Obj encoderHints)
        {
            using (var draw = new PDF.PDFDraw())
            {
                draw.SetDPI(ImageDefaultDpi); // Set the output resolution is to 72 DPI.
                draw.SetImageSmoothing(true);
                draw.SetDrawAnnotations(true);
                draw.SetImageSize(width, height);

                if (encoderHints == null)
                {
                    draw.Export(pdfPage, fileName, format);
                }
                else
                {
                    draw.Export(pdfPage, fileName, format, encoderHints);
                }
            }
        }

        private static string CleanString(string dirtyString)
        {
            var cleanSignature = dirtyString;
            cleanSignature = cleanSignature.Replace("\"", string.Empty);
            cleanSignature = cleanSignature.Replace("'", string.Empty);
            cleanSignature = cleanSignature.Replace(",", string.Empty);
            cleanSignature = cleanSignature.Replace("<", string.Empty);
            cleanSignature = cleanSignature.Replace(">", string.Empty);
            cleanSignature = cleanSignature.Replace("&", "&amp;");

            return cleanSignature;
        }
    }
}