using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class HtmlElementEncoderTest
	{
		private string inputString;
		private string decodedString;
		private IDisposable _context;
		private static readonly object[] charRefs =
			{
				new object[] { "&zwnj;", (char)8204 },
				new object[] { "&zwj;", (char)8205 }
			};

		[SetUp]
		public void Setup()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public void DecodeValue_WhenInputStringDoesNotContainAmpersandSign_resultIsSameAsInputString()
		{
			// Arrange
			inputString = "test String";

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			decodedString.ShouldBe(inputString);
		}

		[Test]
		public void DecodeValue_WhenEncodedHtmlIsGiven_resultContainsDecodedHtml()
		{
			// Arrange
			inputString = @"test&lt;h1&gt;String&lt;/h1&gt;";
			var expectedHtml = @"test<h1>String</h1>";

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			decodedString.ShouldBe(expectedHtml);
		}

		[TestCase("&#39;", "'")]
		[TestCase("&#38;", "&")]
		[TestCase("&#60;", "<")]
		public void DecodeValue_WhenInputContainsHtmlEntityNumber_resultContainsDecodeHtmlForGivenEntityNumber(string entityNumber, string expected)
		{
			// Arrange
			inputString = entityNumber;

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			decodedString.ShouldBe(expected);
		}

		[TestCase("&lt;", "<")]
		[TestCase("&gt;", ">")]
		[TestCase("&amp;", "&")]
		[TestCase("&quot;", "\"")]
		public void DecodeValue_WhenInputContainsHtmlCharacterReference_resultContainHtmlCharacterForGivenCharacterReference(string character_reference, string output)
		{
			// Arrange
			inputString = character_reference;

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			decodedString.ShouldBe(output);
		}

		[TestCase("&zwnjdsfsd;", "&zwnjdsfsd;")]
		[TestCase("&zwnjdsfsd", "&zwnjdsfsd")]
		public void DecodeValue_WhenInputContainsInvalidHtmlCharacterReference_resultSameString(string character_reference, string expected)
		{
			// Arrange
			inputString = character_reference;

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			decodedString.ShouldBe(expected);
		}

		[TestCase("&Aacute;", (char)193)]
		[TestCase("&aacute;", (char)225)]
		[TestCase("&Acirc;", (char)194)]
		[TestCase("&acirc;", (char)226)]
		[TestCase("&acute;", (char)180)]
		[TestCase("&AElig;", (char)198)]
		[TestCase("&aelig;", (char)230)]
		[TestCase("&Agrave;", (char)192)]
		[TestCase("&agrave;", (char)224)]
		[TestCase("&alefsym;", (char)8501)]
		[TestCase("&Alpha;", (char)913)]
		[TestCase("&alpha;", (char)945)]
		[TestCase("&and;", (char)8743)]
		[TestCase("&ang;", (char)8736)]
		[TestCase("&Aring;", (char)197)]
		[TestCase("&aring;", (char)229)]
		[TestCase("&asymp;", (char)8776)]
		[TestCase("&Atilde;", (char)195)]
		[TestCase("&atilde;", (char)227)]
		[TestCase("&Auml;", (char)196)]
		[TestCase("&auml;", (char)228)]
		[TestCase("&bdquo;", (char)8222)]
		[TestCase("&Beta;", (char)914)]
		[TestCase("&beta;", (char)946)]
		[TestCase("&brvbar;", (char)166)]
		[TestCase("&bull;", (char)8226)]
		[TestCase("&cap;", (char)8745)]
		[TestCase("&Ccedil;", (char)199)]
		[TestCase("&ccedil;", (char)231)]
		[TestCase("&cedil;", (char)184)]
		[TestCase("&cent;", (char)162)]
		[TestCase("&Chi;", (char)935)]
		[TestCase("&chi;", (char)967)]
		[TestCase("&circ;", (char)710)]
		[TestCase("&clubs;", (char)9827)]
		[TestCase("&cong;", (char)8773)]
		[TestCase("&copy;", (char)169)]
		[TestCase("&crarr;", (char)8629)]
		[TestCase("&cup;", (char)8746)]
		[TestCase("&curren;", (char)164)]
		[TestCase("&dagger;", (char)8224)]
		[TestCase("&Dagger;", (char)8225)]
		[TestCase("&darr;", (char)8595)]
		[TestCase("&dArr;", (char)8659)]
		[TestCase("&deg;", (char)176)]
		[TestCase("&Delta;", (char)916)]
		[TestCase("&delta;", (char)948)]
		[TestCase("&diams;", (char)9830)]
		[TestCase("&divide;", (char)247)]
		[TestCase("&Eacute;", (char)201)]
		[TestCase("&eacute;", (char)233)]
		[TestCase("&Ecirc;", (char)202)]
		[TestCase("&ecirc;", (char)234)]
		[TestCase("&Egrave;", (char)200)]
		[TestCase("&egrave;", (char)232)]
		[TestCase("&empty;", (char)8709)]
		[TestCase("&emsp;", (char)8195)]
		[TestCase("&Epsilon;", (char)917)]
		[TestCase("&epsilon;", (char)949)]
		[TestCase("&equiv;", (char)8801)]
		[TestCase("&Eta;", (char)919)]
		[TestCase("&eta;", (char)951)]
		[TestCase("&ETH;", (char)208)]
		[TestCase("&eth;", (char)240)]
		[TestCase("&Euml;", (char)203)]
		[TestCase("&euml;", (char)235)]
		[TestCase("&euro;", (char)128)]
		[TestCase("&exist;", (char)8707)]
		[TestCase("&fnof;", (char)402)]
		[TestCase("&forall;", (char)8704)]
		[TestCase("&frac12;", (char)189)]
		[TestCase("&frac14;", (char)188)]
		[TestCase("&frac34;", (char)190)]
		[TestCase("&fras1;", (char)8260)]
		[TestCase("&Gamma;", (char)915)]
		[TestCase("&gamma;", (char)947)]
		[TestCase("&ge;", (char)8805)]
		[TestCase("&harr;", (char)8596)]
		[TestCase("&hArr;", (char)8660)]
		[TestCase("&hearts;", (char)9829)]
		[TestCase("&hellip;", (char)8230)]
		[TestCase("&Iacute;", (char)205)]
		[TestCase("&iacute;", (char)237)]
		[TestCase("&Icirc;", (char)206)]
		[TestCase("&icirc;", (char)238)]
		[TestCase("&iexcl;", (char)161)]
		[TestCase("&Igrave;", (char)204)]
		[TestCase("&igrave;", (char)236)]
		[TestCase("&image;", (char)8465)]
		[TestCase("&infin;", (char)8734)]
		[TestCase("&int;", (char)8747)]
		[TestCase("&Iota;", (char)921)]
		[TestCase("&iota;", (char)953)]
		[TestCase("&iquest;", (char)191)]
		[TestCase("&isin;", (char)8712)]
		[TestCase("&Iuml;", (char)207)]
		[TestCase("&iuml;", (char)239)]
		[TestCase("&Kappa;", (char)922)]
		[TestCase("&kappa;", (char)954)]
		[TestCase("&Lambda;", (char)923)]
		[TestCase("&lambda;", (char)955)]
		[TestCase("&lang;", (char)9001)]
		[TestCase("&laquo;", (char)171)]
		[TestCase("&larr;", (char)8592)]
		[TestCase("&lArr;", (char)8656)]
		[TestCase("&lceil;", (char)8968)]
		[TestCase("&ldquo;", (char)8220)]
		[TestCase("&le;", (char)8804)]
		[TestCase("&lfloor;", (char)8970)]
		[TestCase("&lowast;", (char)8727)]
		[TestCase("&loz;", (char)9674)]
		[TestCase("&lrm;", (char)8206)]
		[TestCase("&lsaquo;", (char)8249)]
		[TestCase("&lsquo;", (char)8216)]
		[TestCase("&macr;", (char)175)]
		[TestCase("&mdash;", (char)8212)]
		[TestCase("&micro;", (char)181)]
		[TestCase("&middot;", (char)183)]
		[TestCase("&minus;", (char)8722)]
		[TestCase("&Mu;", (char)924)]
		[TestCase("&mu;", (char)956)]
		[TestCase("&nabla;", (char)8711)]
		[TestCase("&nbsp;", (char)160)]
		[TestCase("&ndash;", (char)8211)]
		[TestCase("&ne;", (char)8800)]
		[TestCase("&ni;", (char)8715)]
		[TestCase("&not;", (char)172)]
		[TestCase("&notin;", (char)8713)]
		[TestCase("&nsub;", (char)8836)]
		[TestCase("&Ntilde;", (char)209)]
		[TestCase("&ntilde;", (char)241)]
		[TestCase("&Nu;", (char)925)]
		[TestCase("&nu;", (char)957)]
		[TestCase("&Oacute;", (char)211)]
		[TestCase("&oacute;", (char)243)]
		[TestCase("&Ocirc;", (char)212)]
		[TestCase("&ocirc;", (char)244)]
		[TestCase("&OElig;", (char)338)]
		[TestCase("&oelig;", (char)339)]
		[TestCase("&Ograve;", (char)210)]
		[TestCase("&ograve;", (char)242)]
		[TestCase("&oline;", (char)8254)]
		[TestCase("&Omega;", (char)937)]
		[TestCase("&omega;", (char)969)]
		[TestCase("&Omicron;", (char)927)]
		[TestCase("&omicron;", (char)959)]
		[TestCase("&oplus;", (char)8853)]
		[TestCase("&or;", (char)8744)]
		[TestCase("&ordf;", (char)170)]
		[TestCase("&ordm;", (char)186)]
		[TestCase("&Oslash;", (char)216)]
		[TestCase("&oslash;", (char)248)]
		[TestCase("&Otilde;", (char)213)]
		[TestCase("&otilde;", (char)245)]
		[TestCase("&otimes;", (char)8855)]
		[TestCase("&Ouml;", (char)214)]
		[TestCase("&ouml;", (char)246)]
		[TestCase("&para;", (char)182)]
		[TestCase("&part;", (char)8706)]
		[TestCase("&permil;", (char)8240)]
		[TestCase("&perp;", (char)8869)]
		[TestCase("&Phi;", (char)934)]
		[TestCase("&phi;", (char)966)]
		[TestCase("&Pi;", (char)928)]
		[TestCase("&pi;", (char)960)]
		[TestCase("&piv;", (char)982)]
		[TestCase("&plusmn;", (char)177)]
		[TestCase("&pound;", (char)163)]
		[TestCase("&prime;", (char)8242)]
		[TestCase("&Prime;", (char)8243)]
		[TestCase("&prod;", (char)8719)]
		[TestCase("&prop;", (char)8733)]
		[TestCase("&Psi;", (char)936)]
		[TestCase("&psi;", (char)968)]
		[TestCase("&radic;", (char)8730)]
		[TestCase("&rang;", (char)9002)]
		[TestCase("&raquo;", (char)187)]
		[TestCase("&rarr;", (char)8594)]
		[TestCase("&rArr;", (char)8658)]
		[TestCase("&rceil;", (char)8969)]
		[TestCase("&rdquo;", (char)8221)]
		[TestCase("&real;", (char)8476)]
		[TestCase("&reg;", (char)174)]
		[TestCase("&rfloor;", (char)8971)]
		[TestCase("&Rho;", (char)929)]
		[TestCase("&rho;", (char)961)]
		[TestCase("&rlm;", (char)8207)]
		[TestCase("&rsaquo;", (char)8250)]
		[TestCase("&rsquo;", (char)8217)]
		[TestCase("&sbquo;", (char)8218)]
		[TestCase("&Scaron;", (char)352)]
		[TestCase("&scaron;", (char)353)]
		[TestCase("&sdot;", (char)8901)]
		[TestCase("&sect;", (char)167)]
		[TestCase("&shy;", (char)173)]
		[TestCase("&Sigma;", (char)931)]
		[TestCase("&sigma;", (char)963)]
		[TestCase("&sigmaf;", (char)962)]
		[TestCase("&sim;", (char)8764)]
		[TestCase("&spades;", (char)9824)]
		[TestCase("&sub;", (char)8834)]
		[TestCase("&sube;", (char)8838)]
		[TestCase("&sum;", (char)8721)]
		[TestCase("&sup;", (char)8835)]
		[TestCase("&sup1;", (char)185)]
		[TestCase("&sup2;", (char)178)]
		[TestCase("&sup3;", (char)179)]
		[TestCase("&supe;", (char)8839)]
		[TestCase("&szlig;", (char)223)]
		[TestCase("&Tau;", (char)932)]
		[TestCase("&tau;", (char)964)]
		[TestCase("&there4;", (char)8756)]
		[TestCase("&Theta;", (char)920)]
		[TestCase("&theta;", (char)952)]
		[TestCase("&thetasym;", (char)977)]
		[TestCase("&thinsp;", (char)8201)]
		[TestCase("&THORN;", (char)222)]
		[TestCase("&thorn;", (char)254)]
		[TestCase("&tilde;", (char)732)]
		[TestCase("&times;", (char)215)]
		[TestCase("&trade;", (char)8482)]
		[TestCase("&Uacute;", (char)218)]
		[TestCase("&uacute;", (char)250)]
		[TestCase("&uarr;", (char)8593)]
		[TestCase("&uArr;", (char)8657)]
		[TestCase("&Ucirc;", (char)219)]
		[TestCase("&ucirc;", (char)251)]
		[TestCase("&Ugrave;", (char)217)]
		[TestCase("&ugrave;", (char)249)]
		[TestCase("&uml;", (char)168)]
		[TestCase("&upsih;", (char)978)]
		[TestCase("&Upsilon;", (char)933)]
		[TestCase("&upsilon;", (char)965)]
		[TestCase("&Uuml;", (char)220)]
		[TestCase("&uuml;", (char)252)]
		[TestCase("&weierp;", (char)8472)]
		[TestCase("&Xi;", (char)926)]
		[TestCase("&xi;", (char)958)]
		[TestCase("&Yacute;", (char)221)]
		[TestCase("&yacute;", (char)253)]
		[TestCase("&yen;", (char)165)]
		[TestCase("&Yuml;", (char)376)]
		[TestCase("&yuml;", (char)255)]
		[TestCase("&Zeta;", (char)918)]
		[TestCase("&zeta;", (char)950)]
		[TestCase("&zwj;", (char)8205)]
		[TestCase("&zwnj;", (char)8204)]
		public void DecodeValue_WhenInputContainsHtmlCharacterReference_resultContainHtmlCharForGivenCharacterReference(string character_reference, char expected)
		{
			// Arrange
			inputString = character_reference;

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			var output = decodedString.ToCharArray()[0];
			expected.ShouldBe(output);
		}

		[TestCaseSource("charRefs")]
		public void DecodeValue_WhenInputContainsHtmlCharacterReference_resultContainHtmlCharForGivenCharacterReferences(string character_reference, char expected)
		{
			// Arrange
			inputString = character_reference;

			// Act 
			decodedString = CallMethod(inputString);

			// Assert 
			var output = decodedString.ToCharArray()[0];
			expected.ShouldBe(output);
		}

		private string CallMethod(string inputString)
		{
			var type = typeof(ActiveUp.WebControls.File).Assembly.GetType("ActiveUp.WebControls.HtmlElementEncoder");
			var method = type.GetMethod("DecodeValue",
				BindingFlags.Public | BindingFlags.Static, null,
				new Type[] { typeof(String) },
				new ParameterModifier[0]);
			var result = method.Invoke(null, new Object[] { inputString });
			return (string)result;
		}
	}
}
