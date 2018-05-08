using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// HTML 4 Entity coding routines
	/// </summary>
	internal abstract class HtmlElementEncoder
	{
		private const char Ampersand = '&';
		private const char Semicolon = ';';
		private const char Hash = '#';
		private const char LesserThan = '<';
		private const char GreaterThan = '>';
		private const char Quotes = '\"';

		private static readonly Dictionary<int, string> DictionaryEncodings = new Dictionary<int, string>
		{
			[LesserThan] = "&lt;",
			[GreaterThan] = "&gt;",
			[Quotes] = "&quot;",
			[Ampersand] = "&amp;",
			[193] = "&Aacute;",
			[225] = "&aacute;",
			[194] = "&Acirc;",
			[226] = "&acirc;",
			[180] = "&acute;",
			[198] = "&AElig;",
			[230] = "&aelig;",
			[192] = "&Agrave;",
			[224] = "&agrave;",
			[8501] = "&alefsym;",
			[913] = "&Alpha;",
			[945] = "&alpha;",
			[8743] = "&and;",
			[8736] = "&ang;",
			[197] = "&Aring;",
			[229] = "&aring;",
			[8776] = "&asymp;",
			[195] = "&Atilde;",
			[227] = "&atilde;",
			[196] = "&Auml;",
			[228] = "&auml;",
			[8222] = "&bdquo;",
			[914] = "&Beta;",
			[946] = "&beta;",
			[166] = "&brvbar;",
			[8226] = "&bull;",
			[8745] = "&cap;",
			[199] = "&Ccedil;",
			[231] = "&ccedil;",
			[184] = "&cedil;",
			[162] = "&cent;",
			[935] = "&Chi;",
			[967] = "&chi;",
			[710] = "&circ;",
			[9827] = "&clubs;",
			[8773] = "&cong;",
			[169] = "&copy;",
			[8629] = "&crarr;",
			[8746] = "&cup;",
			[164] = "&curren;",
			[8224] = "&dagger;",
			[8225] = "&Dagger;",
			[8595] = "&darr;",
			[8659] = "&dArr;",
			[176] = "&deg;",
			[916] = "&Delta;",
			[948] = "&delta;",
			[9830] = "&diams;",
			[247] = "&divide;",
			[201] = "&Eacute;",
			[233] = "&eacute;",
			[202] = "&Ecirc;",
			[234] = "&ecirc;",
			[200] = "&Egrave;",
			[232] = "&egrave;",
			[8709] = "&empty;",
			[8195] = "&emsp;",
			[917] = "&Epsilon;",
			[949] = "&epsilon;",
			[8801] = "&equiv;",
			[919] = "&Eta;",
			[951] = "&eta;",
			[208] = "&ETH;",
			[240] = "&eth;",
			[203] = "&Euml;",
			[235] = "&euml;",
			[128] = "&euro;",
			[8707] = "&exist;",
			[402] = "&fnof;",
			[8704] = "&forall;",
			[189] = "&frac12;",
			[188] = "&frac14;",
			[190] = "&frac34;",
			[8260] = "&fras1;",
			[915] = "&Gamma;",
			[947] = "&gamma;",
			[8805] = "&ge;",
			[8596] = "&harr;",
			[8660] = "&hArr;",
			[9829] = "&hearts;",
			[8230] = "&hellip;",
			[205] = "&Iacute;",
			[237] = "&iacute;",
			[206] = "&Icirc;",
			[238] = "&icirc;",
			[161] = "&iexcl;",
			[204] = "&Igrave;",
			[236] = "&igrave;",
			[8465] = "&image;",
			[8734] = "&infin;",
			[8747] = "&int;",
			[921] = "&Iota;",
			[953] = "&iota;",
			[191] = "&iquest;",
			[8712] = "&isin;",
			[207] = "&Iuml;",
			[239] = "&iuml;",
			[922] = "&Kappa;",
			[954] = "&kappa;",
			[923] = "&Lambda;",
			[955] = "&lambda;",
			[9001] = "&lang;",
			[171] = "&laquo;",
			[8592] = "&larr;",
			[8656] = "&lArr;",
			[8968] = "&lceil;",
			[8220] = "&ldquo;",
			[8804] = "&le;",
			[8970] = "&lfloor;",
			[8727] = "&lowast;",
			[9674] = "&loz;",
			[8206] = "&lrm;",
			[8249] = "&lsaquo;",
			[8216] = "&lsquo;",
			[175] = "&macr;",
			[8212] = "&mdash;",
			[181] = "&micro;",
			[183] = "&middot;",
			[8722] = "&minus;",
			[924] = "&Mu;",
			[956] = "&mu;",
			[8711] = "&nabla;",
			[160] = "&nbsp;",
			[8211] = "&ndash;",
			[8800] = "&ne;",
			[8715] = "&ni;",
			[172] = "&not;",
			[8713] = "&notin;",
			[8836] = "&nsub;",
			[209] = "&Ntilde;",
			[241] = "&ntilde;",
			[925] = "&Nu;",
			[957] = "&nu;",
			[211] = "&Oacute;",
			[243] = "&oacute;",
			[212] = "&Ocirc;",
			[244] = "&ocirc;",
			[338] = "&OElig;",
			[339] = "&oelig;",
			[210] = "&Ograve;",
			[242] = "&ograve;",
			[8254] = "&oline;",
			[937] = "&Omega;",
			[969] = "&omega;",
			[927] = "&Omicron;",
			[959] = "&omicron;",
			[8853] = "&oplus;",
			[8744] = "&or;",
			[170] = "&ordf;",
			[186] = "&ordm;",
			[216] = "&Oslash;",
			[248] = "&oslash;",
			[213] = "&Otilde;",
			[245] = "&otilde;",
			[8855] = "&otimes;",
			[214] = "&Ouml;",
			[246] = "&ouml;",
			[182] = "&para;",
			[8706] = "&part;",
			[8240] = "&permil;",
			[8869] = "&perp;",
			[934] = "&Phi;",
			[966] = "&phi;",
			[928] = "&Pi;",
			[960] = "&pi;",
			[982] = "&piv;",
			[177] = "&plusmn;",
			[163] = "&pound;",
			[8242] = "&prime;",
			[8243] = "&Prime;",
			[8719] = "&prod;",
			[8733] = "&prop;",
			[936] = "&Psi;",
			[968] = "&psi;",
			[8730] = "&radic;",
			[9002] = "&rang;",
			[187] = "&raquo;",
			[8594] = "&rarr;",
			[8658] = "&rArr;",
			[8969] = "&rceil;",
			[8221] = "&rdquo;",
			[8476] = "&real;",
			[174] = "&reg;",
			[8971] = "&rfloor;",
			[929] = "&Rho;",
			[961] = "&rho;",
			[8207] = "&rlm;",
			[8250] = "&rsaquo;",
			[8217] = "&rsquo;",
			[8218] = "&sbquo;",
			[352] = "&Scaron;",
			[353] = "&scaron;",
			[8901] = "&sdot;",
			[167] = "&sect;",
			[173] = "&shy;",
			[931] = "&Sigma;",
			[963] = "&sigma;",
			[962] = "&sigmaf;",
			[8764] = "&sim;",
			[9824] = "&spades;",
			[8834] = "&sub;",
			[8838] = "&sube;",
			[8721] = "&sum;",
			[8835] = "&sup;",
			[185] = "&sup1;",
			[178] = "&sup2;",
			[179] = "&sup3;",
			[8839] = "&supe;",
			[223] = "&szlig;",
			[932] = "&Tau;",
			[964] = "&tau;",
			[8756] = "&there4;",
			[920] = "&Theta;",
			[952] = "&theta;",
			[977] = "&thetasym;",
			[8201] = "&thinsp;",
			[222] = "&THORN;",
			[254] = "&thorn;",
			[732] = "&tilde;",
			[215] = "&times;",
			[8482] = "&trade;",
			[218] = "&Uacute;",
			[250] = "&uacute;",
			[8593] = "&uarr;",
			[8657] = "&uArr;",
			[219] = "&Ucirc;",
			[251] = "&ucirc;",
			[217] = "&Ugrave;",
			[249] = "&ugrave;",
			[168] = "&uml;",
			[978] = "&upsih;",
			[933] = "&Upsilon;",
			[965] = "&upsilon;",
			[220] = "&Uuml;",
			[252] = "&uuml;",
			[8472] = "&weierp;",
			[926] = "&Xi;",
			[958] = "&xi;",
			[221] = "&Yacute;",
			[253] = "&yacute;",
			[165] = "&yen;",
			[376] = "&Yuml;",
			[255] = "&yuml;",
			[918] = "&Zeta;",
			[950] = "&zeta;",
			[8205] = "&zwj;",
			[8204] = "&zwnj;"
		};

		public static string EncodeValue(string value)
		{
			var output = new StringBuilder();
			using (var reader = new StringReader(value))
			{
				var charToEncode = reader.Read();
				while (charToEncode != -1)
				{
					string dictValue;
					if (DictionaryEncodings.TryGetValue(charToEncode, out dictValue))
					{
						output.Append(dictValue);
					}
					else
					{
						if (charToEncode <= 127)
						{
							output.Append((char)charToEncode);
						}
						else
						{
							output.Append($"&#{charToEncode};");
						}
					}

					charToEncode = reader.Read();
				}
			}

			return output.ToString();
		}

		public static string DecodeValue(string value)
		{
			var output = new StringBuilder();
			using (var reader = new StringReader(value))
			{
				StringBuilder token;
				var encodedValue = reader.Read();
				while (encodedValue != -1)
				{
					token = new StringBuilder();
					while (encodedValue != Ampersand && encodedValue != -1)
					{
						token.Append((char)encodedValue);
						encodedValue = reader.Read();
					}

					output.Append(token);
					if (encodedValue == Ampersand)
					{
						token = new StringBuilder();
						while (encodedValue != Semicolon && encodedValue != -1)
						{
							token.Append((char)encodedValue);
							encodedValue = reader.Read();
						}

						if (encodedValue == Semicolon)
						{
							encodedValue = reader.Read();
							token.Append(Semicolon);
							if (token[1] == Hash)
							{
								var nextChar = int.Parse(token.ToString().Substring(2, token.Length - 3));
								output.Append((char)nextChar);
							}
							else
							{
								var tokenString = token.ToString();
								int key;
								if (TryGetKeyByValue(tokenString, out key))
								{
									output.Append((char)key);
								}
								else
								{
									output.Append(token);
								}
							}
						}
						else
						{
							output.Append(token);
						}
					}
				}
			}

			return output.ToString();
		}

		private static bool TryGetKeyByValue(string value, out int key)
		{
			foreach (var pair in DictionaryEncodings)
			{
				if (pair.Value == value)
				{
					key = pair.Key;
					return true;
				}
			}

			key = -1;
			return false;
		}
	}
}
