using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KM.Common.Functions
{
    public partial class HtmlFunctions
    {
        private const string HtmlCommentsRegexPattern = "<!--(.|\\s)*?-->";
        private const string HtmlLinksRegexPattern = "<a[^>]*? href=\"(?<url>[^\"]+)\"[^>]*?>(?<text>.*?)</a>";
        private const string HtmlLinkUrlGroupName = "url";
        private const string HtmlLinkTextGroupName = "text";
        private const string HtmlLinkReplacementFormat = "[URL: {0}] {1}";
        private const string NewLine = "\n";
        private const string CarriageReturn = "\r";
        private const string HtmlCustomUrlTagStart = "[URL: ";
        private const string HtmlCustomUrlTagStartReplacement = "[URL: <";
        private const string HtmlCustomUrlTagEnd = "]";
        private const string HtmlCustomUrlTagEndReplacement = "> ]";
        private const string ExportCommaSingleQuoteCharset = ",'\r";
        private const string ExportCommaSingleQuoteCommaCharset = ",',";
        private const string ExportDoubleQuoteSingleQuoteCharset = "\"'\"";
        private const string ExportDoubleQuoteCharset = "\"\"";
        private const string ExportCommaCharset = ",,";
        private const string ExportCommaNewLineCharset = ",\r";

        private static Regex paragraphStartRegex = new Regex("<p>", RegexOptions.IgnoreCase);
        private static Regex paragraphEndRegex = new Regex("</p>", RegexOptions.IgnoreCase);
        private static readonly Dictionary<string, string> _htmlElementsReplacements = new Dictionary<string, string>
        {
            { "\n", string.Empty },
            { "\t", string.Empty },
            { @"( )+", " " },
            { @"<( )*head([^>])*>", "<head>" },
            { @"(<( )*(/)( )*head( )*>)", "</head>" },
            { @"(<head>).*(</head>)", string.Empty },
            { @"<( )*script([^>])*>", "<script>" },
            { @"(<( )*(/)( )*script( )*>)", "</script>" },
            { @"(<script>).*(</script>)", string.Empty },
            { @"<( )*style([^>])*>", "<style>" },
            { @"(<( )*(/)( )*style( )*>)", "</style>" },
            { @"(<style>).*(</style>)", string.Empty },
            { @"<( )*td([^>])*>", "\t" },
            { @"<( )*br( )*>", "\r" },
            { @"<( )*br( )*/>", "\r" },
            { @"<( )*li( )*>", "\r" },
            { @"<( )*div([^>])*>", "\r\r" },
            { @"<( )*tr([^>])*>", "\r\r" },
            { @"<( )*p([^>])*>", "\r\r" },
            { @"<[^>]*>", string.Empty },
            { @"&nbsp;", " " },
            { @"&#39;", "'" },
            { @"&bull;", " * " },
            { @"&lsaquo;", "<" },
            { @"&rsaquo;", ">" },
            { @"&trade;", "(tm)" },
            { @"&frasl;", "/" },
            { @"&copy;", "(c)" },
            { @"&reg;", "(r)" },
            { @"&amp;", "&" },
            { @"&(.{2,6});", string.Empty },
            { "(\r)( )+(\r)", "\r\r" },
            { "(\t)( )+(\t)", "\t\t" },
            { "(\t)( )+(\r)", "\t\r" },
            { "(\r)( )+(\t)", "\r\t" },
            { "(\r)(\t)+(\r)", "\r\r" },
            { "(\r)(\t)+", "\r\t" }
        };

        #region Utilities

        private static string EnsureOnlyAllowedHtml(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            string allowedTags = "br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em,strong,font,pre,h1,h2,h3,h4,h5,h6,address,ciate";

            var options = RegexOptions.IgnoreCase;
            var m = Regex.Matches(text, "<.*?>", options);
            for (int i = m.Count - 1; i >= 0; i--)
            {
                string tag = text.Substring(m[i].Index + 1, m[i].Length - 1).Trim().ToLower();

                if (!IsValidTag(tag, allowedTags))
                {
                    text = text.Remove(m[i].Index, m[i].Length);
                }
            }

            return text;
        }

        private static bool IsValidTag(string tag, string tags)
        {
            string[] allowedTags = tags.Split(',');
            if (tag.IndexOf("javascript") >= 0) return false;
            if (tag.IndexOf("vbscript") >= 0) return false;
            if (tag.IndexOf("onclick") >= 0) return false;

            char[] endchars = new char[] { ' ', '>', '/', '\t' };

            int pos = tag.IndexOfAny(endchars, 1);
            if (pos > 0) tag = tag.Substring(0, pos);
            if (tag[0] == '/') tag = tag.Substring(1);

            foreach (string aTag in allowedTags)
            {
                if (tag == aTag) return true;
            }

            return false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="stripTags">A value indicating whether to strip tags</param>
        /// <param name="convertPlainTextToHtml">A value indicating whether HTML is allowed</param>
        /// <param name="allowHtml">A value indicating whether HTML is allowed</param>
        /// <param name="allowBBCode">A value indicating whether BBCode is allowed</param>
        /// <param name="resolveLinks">A value indicating whether to resolve links</param>
        /// <param name="addNoFollowTag">A value indicating whether to add "noFollow" tag</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string text, bool stripTags,
            bool convertPlainTextToHtml, bool allowHtml,
            bool allowBBCode, bool resolveLinks, bool addNoFollowTag)
        {

            if (String.IsNullOrEmpty(text))
                return string.Empty;

            try
            {
                if (stripTags)
                {
                    text = HtmlFunctions.StripTags(text);
                }

                if (allowHtml)
                {
                    text = HtmlFunctions.EnsureOnlyAllowedHtml(text);
                }
                else
                {
                    text = HttpUtility.HtmlEncode(text);
                }

                if (convertPlainTextToHtml)
                {
                    text = HtmlFunctions.ConvertPlainTextToHtml(text);
                }

                if (allowBBCode)
                {
                    text = BBCodeHelper.FormatText(text, true, true, true, true, true, true);
                }

                if (resolveLinks)
                {
                    text = ResolveLinksHelper.FormatText(text);
                }

                if (addNoFollowTag)
                {
                    //add noFollow tag. not implemented
                }
            }
            catch (Exception exc)
            {
                text = string.Format("Text cannot be formatted. Error: {0}", exc.Message);
            }
            return text;
        }

        /// <summary>
        /// Strips tags
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string StripTags(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, @"(>)(\r|\n)*(<)", "><");
            text = Regex.Replace(text, "(<[^>]*>)([^<]*)", "$2");
            text = Regex.Replace(text, "(&#x?[0-9]{2,4};|&quot;|&amp;|&nbsp;|&lt;|&gt;|&euro;|&copy;|&reg;|&permil;|&Dagger;|&dagger;|&lsaquo;|&rsaquo;|&bdquo;|&rdquo;|&ldquo;|&sbquo;|&rsquo;|&lsquo;|&mdash;|&ndash;|&rlm;|&lrm;|&zwj;|&zwnj;|&thinsp;|&emsp;|&ensp;|&tilde;|&circ;|&Yuml;|&scaron;|&Scaron;)", "@");

            return text;
        }

        /// <summary>
        /// Converts plain text to HTML
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToHtml(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\r", "<br />");
            text = text.Replace("\n", "<br />");
            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");

            return text;
        }

        /// <summary>
        /// Converts HTML to plain text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertHtmlToPlainText(string text)
        {
            return ConvertHtmlToPlainText(text, false);
        }

        /// <summary>
        /// Converts HTML to plain text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="decode">A value indicating whether to decode text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertHtmlToPlainText(string text, bool decode)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            if (decode)
                text = HttpUtility.HtmlDecode(text);

            text = text.Replace("<br>", "\n");
            text = text.Replace("<br >", "\n");
            text = text.Replace("<br />", "\n");
            text = text.Replace("&nbsp;&nbsp;", "\t");
            text = text.Replace("&nbsp;&nbsp;", "  ");

            return text;
        }

        /// <summary>
        /// Converts text to paragraph
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToParagraph(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = paragraphStartRegex.Replace(text, string.Empty);
            text = paragraphEndRegex.Replace(text, "\n");
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text = text + "\n\n";
            text = text.Replace("\n\n", "\n");
            var strArray = text.Split(new char[] { '\n' });
            var builder = new StringBuilder();
            foreach (string str in strArray)
            {
                if ((str != null) && (str.Trim().Length > 0))
                {
                    builder.AppendFormat("<p>{0}</p>\n", str);
                }
            }
            return builder.ToString();
        }

        public static string StripTextFromHtml(string htmlString)
        {
            // Intentionately check only for null value, and NOT for NullOrWhiteSpace,
            // since the stripping logic must apply also on whiteshapce strings (\r\t\n etc.)
            if (htmlString == null)
            {
                return null;
            }

            htmlString = StripHtmlComments(htmlString);
            htmlString = StripHtmlLinks(htmlString);

            foreach (var replacement in _htmlElementsReplacements)
            {
                htmlString = Regex.Replace(htmlString, replacement.Key, replacement.Value, RegexOptions.IgnoreCase);
            }

            htmlString = htmlString.Replace(NewLine, CarriageReturn);
            htmlString = htmlString.Replace(HtmlCustomUrlTagStart, HtmlCustomUrlTagStartReplacement);
            htmlString = htmlString.Replace(HtmlCustomUrlTagEnd, HtmlCustomUrlTagEndReplacement);

            return htmlString;
        }

        public static string StripHtmlComments(string htmlString)
        {
            if (string.IsNullOrWhiteSpace(htmlString))
            {
                return htmlString;
            }

            return Regex.Replace(htmlString, HtmlCommentsRegexPattern, string.Empty);
        }

        public static string StripTextFromHtmlForExport(string htmlString)
        {
            // Intentionately check only for null value, and NOT for NullOrWhiteSpace,
            // since the stripping logic must apply also on whiteshapce strings (\r\t\n etc.)
            if (htmlString == null)
            {
                return null;
            }

            htmlString = StripTextFromHtml(htmlString);

            // Strip additional elements specific for the Export.
            htmlString = htmlString.Replace(ExportCommaSingleQuoteCharset, ExportCommaNewLineCharset);
            htmlString = htmlString.Replace(ExportCommaSingleQuoteCommaCharset, ExportCommaCharset);
            htmlString = htmlString.Replace(ExportDoubleQuoteSingleQuoteCharset, ExportDoubleQuoteCharset);

            return htmlString;
        }

        private static string StripHtmlLinks(string htmlString)
        {
            if (string.IsNullOrWhiteSpace(htmlString))
            {
                return htmlString;
            }

            var linkMatches = Regex.Matches(htmlString, HtmlLinksRegexPattern, RegexOptions.Singleline);
            foreach (var linkMatch in linkMatches.OfType<Match>())
            {
                var linkUrlMatch = linkMatch.Groups[HtmlLinkUrlGroupName].Value.Trim();
                var linkTextMatch = linkMatch.Groups[HtmlLinkTextGroupName].Value.Trim();

                var linkReplacement = String.Format(HtmlLinkReplacementFormat, linkUrlMatch, linkTextMatch);
                htmlString = htmlString.Replace(linkMatch.Value.Trim(), linkReplacement);
            }

            return htmlString;
        }

        #endregion
    }
}
