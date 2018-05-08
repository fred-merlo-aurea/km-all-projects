/*
* Helma License Notice
*
* The contents of this file are subject to the Helma License
* Version 2.0 (the "License"). You may not use this file except in
* compliance with the License. A copy of the License is available at
* http://adele.helma.org/download/helma/license.txt
*
* Copyright 1998-2003 Helma Software. All Rights Reserved.
*
* $RCSfile: HtmlEncoder.java,v $
* $Author: hannes $
* $Revision: 1.24 $
* $Date: 2003/06/26 14:10:03 $
*
* Converted to C# by Pierre M. from Active Up SPRL.
*
*/
namespace ActiveUp.WebControls
{
	using System;
	using System.Collections;
	using System.Text;
	/// <summary> This is a utility class to encode special characters and do formatting
	/// for HTML output.
	/// </summary>
	public sealed class HtmlEncoder
	{
		
		// transformation table for characters 128 to 255. These actually fall into two
		// groups, put together for efficiency: "Windows" chacacters 128-159 such as
		// "smart quotes", which are encoded to valid Unicode entities, and
		// valid ISO-8859 caracters 160-255, which are encoded to the symbolic HTML
		// entity. Everything >= 256 is encoded to a numeric entity.
		internal static readonly System.String[] transform = new System.String[]{"&euro;", "", "&#8218;", "&#402;", "&#8222;", "&#8230;", "&#8224;", "&#8225;", "&#710;", "&#8240;", "&#352;", "&#8249;", "&#338;", "", "&#381;", "", "", "&#8216;", "&#8217;", "&#8220;", "&#8221;", "&#8226;", "&#8211;", "&#8212;", "&#732;", "&#8482;", "&#353;", "&#8250;", "&#339;", "", "&#382;", "&#376;", "&nbsp;", "&iexcl;", "&cent;", "&pound;", "&curren;", "&yen;", "&brvbar;", "&sect;", "&uml;", "&copy;", "&ordf;", "&laquo;", "&not;", "&shy;", "&reg;", "&macr;", "&deg;", "&plusmn;", "&sup2;", "&sup3;", "&acute;", "&micro;", "&para;", "&middot;", "&cedil;", "&sup1;", "&ordm;", "&raquo;", "&frac14;", "&frac12;", "&frac34;", "&iquest;", "&Agrave;", "&Aacute;", "&Acirc;", "&Atilde;", "&Auml;", "&Aring;", "&AElig;", "&Ccedil;", "&Egrave;", "&Eacute;", "&Ecirc;", "&Euml;", "&Igrave;", "&Iacute;", "&Icirc;", "&Iuml;", "&ETH;", "&Ntilde;", "&Ograve;", "&Oacute;", "&Ocirc;", "&Otilde;", "&Ouml;", "&times;", "&Oslash;", "&Ugrave;", "&Uacute;", "&Ucirc;", "&Uuml;", "&Yacute;", "&THORN;", "&szlig;", "&agrave;", "&aacute;", "&acirc;", "&atilde;", "&auml;", "&aring;", "&aelig;", "&ccedil;", "&egrave;", "&eacute;", "&ecirc;", "&euml;", "&igrave;", "&iacute;", "&icirc;", "&iuml;", "&eth;", "&ntilde;", "&ograve;", "&oacute;", "&ocirc;", "&otilde;", "&ouml;", "&divide;", "&oslash;", "&ugrave;", "&uacute;", "&ucirc;", "&uuml;", "&yacute;", "&thorn;", "&yuml;"};
		
		internal static readonly ArrayList allTags = new ArrayList();
		
		
		// HTML block tags need to suppress automatic newline to <br>
		// conversion around them to look good. However, they differ 
		// in how many newlines around them should ignored. These sets
		// help to treat each tag right in newline conversion.
		internal static readonly ArrayList swallowAll = new ArrayList();
		internal static readonly ArrayList swallowTwo = new ArrayList();
		internal static readonly ArrayList swallowOne = new ArrayList();
		
		
		// set of tags that are always empty
		internal static readonly ArrayList emptyTags = new ArrayList();
		
		internal const sbyte TagName = 0;
		internal const sbyte TagSpace = 1;
		internal const sbyte TagAttName = 2;
		internal const sbyte TagAttVal = 3;
		private const char UnicodeNull = '\u0000';
		private const char LesserThan = '<';
		private const char Percentage = '%';
		private const char Minus = '-';
		private const char BackwardSlash = '/';
		private const char Not = '!';
		private const string Code = "code";
		private const string Pre = "pre";
		private const string BreakTagNewLine = "<br />\n";
		private const string HtmlLesserThan = "&lt;";
		private const char Ampersand = '&';
		private const char Hash = '#';
		private const char Semicolon = ';';
		private const string AmpersandString = "&";
		private const string HtmlAmpersand = "&amp;";
		private const char DoubleSlash = '\\';
		private const char DoubleQuotes = '"';
		private const char SingleQuote = '\'';
		private const char NewlineChar = '\n';
		private const char GreaterThan = '>';
		private const string HtmlGreaterThan = "&gt;";
		private const string SemiColonString = ";";
		private const string PrefixCloseTag = "</";
		private const string SuffixTagEnd = ">";
		private const string AmpersandHash = "&#";
		private const char EqualsChar = '=';
		private const char SpaceChar = ' ';
		private const int SwallowAllLineBreaks = 1000;

		/// <summary>  Do "smart" encodging on a string. This means that valid HTML entities and tags,
		/// Helma macros and HTML comments are passed through unescaped.
		/// </summary>
		public static System.String Encode(System.String str)
		{
			if (str == null)
			{
				return null;
			}
			
			int l = str.Length;
			
			if (l == 0)
			{
				return "";
			}
			
			// try to make stringbuffer large enough from the start
			System.Text.StringBuilder ret = new System.Text.StringBuilder((int) System.Math.Round((double) (l * 1.4f)));
			
			Encode(str, ret, null);
			
			return ret.ToString();
		}
		
		/// <summary>  Do "smart" encodging on a string. This means that valid HTML entities and tags,
		/// Helma macros and HTML comments are passed through unescaped.
		/// </summary>
		public static void Encode(System.String str, System.Text.StringBuilder ret)
		{
			Encode(str, ret, null);
		}
		
		/// <summary>  Do "smart" encodging on a string. This means that valid HTML entities and tags,
		/// Helma macros and HTML comments are passed through unescaped.
		/// </summary>
		public static void Encode(string str, StringBuilder ret, ArrayList allowedTags)
		{
			if(string.IsNullOrEmpty(str))
			{
				return;
			}

			var strLength = str.Length;
			var openTags = new Stack();
			EncodeParams.Init();

			for (var charIndex = 0; charIndex < strLength; charIndex++)
			{
				var character = str[charIndex];

				if (character == LesserThan)
				{
					if (ProcessLesserThan(str, ret, allowedTags,openTags, ref charIndex)) { continue; }
				}

				if ((EncodeParams.Linebreaks > 0 || EncodeParams.SwallowLinebreaks > 0) && !char.IsWhiteSpace(character))
				{
					if (!EncodeParams.InsidePreTag && EncodeParams.Linebreaks > EncodeParams.SwallowLinebreaks)
					{
						EncodeParams.Linebreaks -= EncodeParams.SwallowLinebreaks;

						for (var lineBreakIndex = 0; lineBreakIndex < EncodeParams.Linebreaks; lineBreakIndex++) { ret.Append(BreakTagNewLine); }
					}

					if (!EncodeParams.InsideTag) { EncodeParams.SwallowLinebreaks = 0; }

					EncodeParams.Linebreaks = 0;
				}

				ProcessTags(str, ret, character, charIndex, openTags);
			}

			// if tags were opened but not closed, close them.
			for (var tagIndex = 0; tagIndex < openTags.Count; tagIndex++)
			{
				var tag = openTags.Pop();
				if (!emptyTags.Contains(tag)) { ret.AppendFormat("{0}tag{1}", PrefixCloseTag, SuffixTagEnd); }
			}

			// add remaining newlines we may have collected
			if (EncodeParams.Linebreaks > 0 && EncodeParams.Linebreaks > EncodeParams.SwallowLinebreaks)
			{
				EncodeParams.Linebreaks -= EncodeParams.SwallowLinebreaks;

				for (var lineBreakIndex = 0; lineBreakIndex < EncodeParams.Linebreaks; lineBreakIndex++) { ret.Append(BreakTagNewLine); }
			}
		}

		private static void ProcessTags(string str, StringBuilder ret, char character, int charIndex, Stack openTags)
		{
			var strLength = str.Length;

			switch (character)
			{
				case LesserThan:
					if (EncodeParams.InsideTag) { ret.Append(LesserThan); }
					else { ret.Append(HtmlLesserThan); }

					break;
				case Ampersand:
					if (charIndex < strLength - 3 && !EncodeParams.InsideCodeTag)
					{
						var isHash = str[charIndex + 1] == Hash;
						var tempCharIndex = charIndex + (isHash ? 2 : 1);

						while (tempCharIndex < strLength && (isHash ? char.IsDigit(str[tempCharIndex]) : char.IsLetterOrDigit(str[tempCharIndex]))) { tempCharIndex++; }

						if (tempCharIndex < strLength && str[tempCharIndex] == Semicolon)
						{
							ret.Append(AmpersandString);
							break;
						}
					}

					ret.Append(HtmlAmpersand);
					break;
				case DoubleSlash:
					ret.Append(character);
					if (EncodeParams.InsideTag && !EncodeParams.InsideComment) { EncodeParams.Escape = !EncodeParams.Escape; }

					break;
				case DoubleQuotes:
				case SingleQuote:
					ProcessQuote(ret, character);
					break;
				case NewlineChar:
					ret.Append(NewlineChar);
					if (!EncodeParams.InsideTag) { EncodeParams.Linebreaks++; }

					break;
				case GreaterThan:
					ProcessGreaterThan(str, ret, charIndex, openTags);
					break;
				default:
					ProcessTagsDefault(ret, character);
					break;
			}
		}

		private static void ProcessTagsDefault(StringBuilder ret, char character)
		{
			if (EncodeParams.InsideHtmlTag && !EncodeParams.InsideCloseTag)
			{
				switch (EncodeParams.HtmlTagMode)
				{
					case TagName:
						if (!char.IsLetterOrDigit(character)) { EncodeParams.HtmlTagMode = TagSpace; }

						break;
					case TagSpace:
						if (char.IsLetterOrDigit(character)) { EncodeParams.HtmlTagMode = TagAttName; }

						break;
					case TagAttName:
						if (character == EqualsChar) { EncodeParams.HtmlTagMode = TagAttVal; }
						else if (character == SpaceChar) { EncodeParams.HtmlTagMode = TagSpace; }

						break;
					case TagAttVal:
						if (char.IsWhiteSpace(character) && EncodeParams.HtmlQuoteChar == UnicodeNull) { EncodeParams.HtmlTagMode = TagSpace; }

						break;
				}
			}

			if (character < 128) { ret.Append(character); }
			else if (character >= 128 && character < 256) { ret.Append(transform[character - 128]); }
			else
			{
				ret.Append(AmpersandHash);
				ret.Append((int)character);
				ret.Append(SemiColonString);
			}

			EncodeParams.Escape = false;
		}

		private static void ProcessGreaterThan(string str, StringBuilder ret, int charIndex, Stack openTags)
		{
			if (EncodeParams.InsideComment)
			{
				ret.Append(GreaterThan);
				EncodeParams.InsideComment = !(str[charIndex - 2] == Minus && str[charIndex - 1] == Minus);
			}
			else if (EncodeParams.InsideMacroTag)
			{
				ret.Append(GreaterThan);
				EncodeParams.InsideMacroTag = !(str[charIndex - 1] == Percentage && EncodeParams.MacroQuoteChar == UnicodeNull);
			}
			else if (EncodeParams.InsideHtmlTag)
			{
				ret.Append(GreaterThan);
				EncodeParams.InsideHtmlTag = EncodeParams.HtmlQuoteChar != UnicodeNull;
				if (str[charIndex - 1] == BackwardSlash && EncodeParams.HtmlTagMode != TagAttVal && EncodeParams.HtmlTagMode != TagAttName) { openTags.Pop(); }
			}
			else { ret.Append(HtmlGreaterThan); }

			EncodeParams.InsideTag = EncodeParams.InsideComment || EncodeParams.InsideMacroTag || EncodeParams.InsideHtmlTag;
		}

		private static void ProcessQuote(StringBuilder ret, char character)
		{
			ret.Append(character);

			if (!EncodeParams.InsideComment)
			{
				// check if the quote is escaped
				if (EncodeParams.InsideMacroTag)
				{
					if (EncodeParams.Escape) { EncodeParams.Escape = false; }
					else if (EncodeParams.MacroQuoteChar == character) { EncodeParams.MacroQuoteChar = UnicodeNull; }
					else if (EncodeParams.MacroQuoteChar == UnicodeNull) { EncodeParams.MacroQuoteChar = character; }
				}
				else if (EncodeParams.InsideHtmlTag)
				{
					if (EncodeParams.Escape) { EncodeParams.Escape = false; }
					else if (EncodeParams.HtmlQuoteChar == character)
					{
						EncodeParams.HtmlQuoteChar = UnicodeNull;
						EncodeParams.HtmlTagMode = TagSpace;
					}
					else if (EncodeParams.HtmlQuoteChar == UnicodeNull) { EncodeParams.HtmlQuoteChar = character; }
				}
			}
		}

		private static bool ProcessLesserThan(string str, StringBuilder ret, ArrayList allowedTags, Stack openTags, ref int charIndex)
		{
			var strLength = str.Length;

			if (charIndex < strLength - 2)
			{
				if (!EncodeParams.InsideMacroTag && Percentage == str[charIndex + 1])
				{
					// this is the beginning of a Helma macro tag
					if (!EncodeParams.InsideCodeTag)
					{
						EncodeParams.InsideMacroTag = EncodeParams.InsideTag = true;
						EncodeParams.MacroQuoteChar = UnicodeNull;
					}
				}
				else if (Not == str[charIndex + 1] && Minus == str[charIndex + 2])
				{
					// the beginning of an HTML comment?
					if (!EncodeParams.InsideCodeTag) { EncodeParams.InsideComment = EncodeParams.InsideTag = charIndex < strLength - 3 && Minus == str[charIndex + 3]; }
				}
				else if (!EncodeParams.InsideTag)
				{
					// check if this is a HTML tag.
					EncodeParams.InsideCloseTag = BackwardSlash == str[charIndex + 1];
					var tagStart = EncodeParams.InsideCloseTag ? charIndex + 2 : charIndex + 1;
					var tempTagStart = tagStart;

					while (tempTagStart < strLength && char.IsLetterOrDigit(str[tempTagStart])) { tempTagStart++; }

					if (tempTagStart > tagStart && tempTagStart < strLength)
					{
						var tagName = str.Substring(tagStart, tempTagStart - tagStart).ToLower();

						if (Code.Equals(tagName) && EncodeParams.InsideCloseTag && EncodeParams.InsideCodeTag) { EncodeParams.InsideCodeTag = false; }

						if ((allowedTags == null || allowedTags.Contains(tagName)) && allTags.Contains(tagName) && !EncodeParams.InsideCodeTag)
						{
							EncodeParams.InsideHtmlTag = EncodeParams.InsideTag = true;
							EncodeParams.HtmlQuoteChar = UnicodeNull;
							EncodeParams.HtmlTagMode = TagName;
							EncodeParams.Linebreaks = Math.Max(EncodeParams.Linebreaks - EncodeParams.SwallowLinebreaks, 0);

							if (swallowAll.Contains(tagName)) { EncodeParams.SwallowLinebreaks = SwallowAllLineBreaks; }
							else if (swallowTwo.Contains(tagName)) { EncodeParams.SwallowLinebreaks = 2; }
							else if (swallowOne.Contains(tagName)) { EncodeParams.SwallowLinebreaks = 1; }
							else { EncodeParams.SwallowLinebreaks = 0; }

							if (EncodeParams.InsideCloseTag)
							{
								var tempIndex = -1;
								var tempCont = 0;
								var tempEnumerator = openTags.GetEnumerator();
								while (tempEnumerator.MoveNext())
								{
									tempCont++;
									if (tempEnumerator.Current != null && tempEnumerator.Current.Equals(tagName))
									{
										tempIndex = tempCont;
										break;
									}
								}

								var openTagsCount = tempIndex;

								if (openTagsCount == -1)
								{
									charIndex = tempTagStart;
									EncodeParams.InsideHtmlTag = EncodeParams.InsideTag = false;

									return true;
								}

								for (var openTagsIndex = 1; openTagsIndex < openTagsCount; openTagsIndex++)
								{
									var tag = openTags.Pop();
									if (!emptyTags.Contains(tag)) { ret.Append(PrefixCloseTag + tag + SuffixTagEnd); }
								}

								openTags.Pop();
							}
							else
							{
								StackPush(openTags, tagName);
								EncodeParams.SwallowLinebreaks = Math.Max(EncodeParams.SwallowLinebreaks - 1, 0);
							}

							if (Code.Equals(tagName) && !EncodeParams.InsideCloseTag) { EncodeParams.InsideCodeTag = true; }

							if (Pre.Equals(tagName)) { EncodeParams.InsidePreTag = !EncodeParams.InsideCloseTag; }
						}
					}
				}
			}

			return false;
		}

		/// <summary>*
		/// </summary>
		public static System.String EncodeFormValue(System.String str)
		{
			if (str == null)
			{
				return null;
			}
			
			int l = str.Length;
			
			if (l == 0)
			{
				return "";
			}
			
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			System.Text.StringBuilder ret = new System.Text.StringBuilder((int) System.Math.Round((double) (l * 1.2f)));
			
			EncodeAll(str, ret, false);
			
			return ret.ToString();
		}
		
		/// <summary>*
		/// </summary>
		public static void EncodeFormValue(System.String str, System.Text.StringBuilder ret)
		{
			EncodeAll(str, ret, false);
		}
		
		/// <summary>*
		/// </summary>
		public static System.String EncodeAll(System.String str)
		{
			if (str == null)
			{
				return null;
			}
			
			int l = str.Length;
			
			if (l == 0)
			{
				return "";
			}
			
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			System.Text.StringBuilder ret = new System.Text.StringBuilder((int) System.Math.Round((double) (l * 1.2f)));
			
			EncodeAll(str, ret, true);
			
			return ret.ToString();
		}
		
		/// <summary>*
		/// </summary>
		public static void EncodeAll(System.String str, System.Text.StringBuilder ret)
		{
			EncodeAll(str, ret, true);
		}
		
		/// <summary>*
		/// </summary>
		public static void EncodeAll(System.String str, System.Text.StringBuilder ret, bool encodeNewline)
		{
			if (str == null)
			{
				return ;
			}
			
			int l = str.Length;
			
			for (int i = 0; i < l; i++)
			{
				char c = str[i];
				
				switch (c)
				{
					
					case '<': 
						ret.Append("&lt;");
						
						break;
					
					
					case '>': 
						ret.Append("&gt;");
						
						break;
					
					
					case '&': 
						ret.Append("&amp;");
						
						break;
					
					
					case '"': 
						ret.Append("&quot;");
						
						break;
					
					
					case '\n': 
						ret.Append('\n');
						
						if (encodeNewline)
						{
							ret.Append("<br />");
						}
						
						break;
					
					
					default: 
						if (c < 128)
						{
							ret.Append(c);
						}
						else if ((c >= 128) && (c < 256))
						{
							ret.Append(transform[c - 128]);
						}
						else
						{
							ret.Append("&#");
							ret.Append((int) c);
							ret.Append(";");
						}
						break;
					
				}
			}
		}
		
		/// <summary>*
		/// *
		/// </summary>
		/// <param name="str">...
		/// *
		/// </param>
		/// <returns> ...
		/// 
		/// </returns>
		public static System.String EncodeXml(System.String str)
		{
			if (str == null)
			{
				return null;
			}
			
			int l = str.Length;
			
			if (l == 0)
			{
				return "";
			}
			
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			System.Text.StringBuilder ret = new System.Text.StringBuilder((int) System.Math.Round((double) (l * 1.2f)));
			
			EncodeXml(str, ret);
			
			return ret.ToString();
		}
		
		/// <summary>*
		/// *
		/// </summary>
		/// <param name="str">...
		/// </param>
		/// <param name="ret">...
		/// 
		/// </param>
		public static void EncodeXml(System.String str, System.Text.StringBuilder ret)
		{
			if (str == null)
			{
				return ;
			}
			
			int l = str.Length;
			
			for (int i = 0; i < l; i++)
			{
				char c = str[i];
				
				switch (c)
				{
					
					case '<': 
						ret.Append("&lt;");
						
						break;
					
					
					case '>': 
						ret.Append("&gt;");
						
						break;
					
					
					case '&': 
						ret.Append("&amp;");
						
						break;
					
					
					case '"': 
						ret.Append("&quot;");
						
						break;
					
					
					case '\'': 
						ret.Append("&apos;");
						
						break;
					
					
					default: 
						
						if (c < 0x20)
						{
							// sort out invalid XML characters below 0x20 - all but 0x9, 0xA and 0xD.
							// The trick is an adaption of java.lang.Character.isSpace().
							if (((((1L << 0x9) | (1L << 0xA) | (1L << 0xD)) >> (int) c) & 1L) != 0)
							{
								ret.Append(c);
							}
						}
						else
						{
							ret.Append(c);
						}
						break;
					
				}
			}
		}
		
		
		/// <summary>
		/// Prints the character range.
		/// </summary>
		/// <param name="from">From position.</param>
		/// <param name="to">To position.</param>
		/// <returns></returns>
		public static System.String PrintCharRange(int from, int to)
		{
			System.Text.StringBuilder response = new System.Text.StringBuilder();
			
			for (int i = from; i < to; i++)
			{
				response.Append(i);
				response.Append("      ");
				response.Append((char) i);
				response.Append("      ");
				
				if (i < 128)
				{
					response.Append((char) i);
				}
				else if ((i >= 128) && (i < 256))
				{
					response.Append(transform[i - 128]);
				}
				else
				{
					response.Append("&#");
					response.Append(i);
					response.Append(";");
				}
				
				response.Append("\r\n");
			}
			
			return response.ToString();
		}
		
		static HtmlEncoder()
		{
			{
				allTags.Add("a");
				allTags.Add("abbr");
				allTags.Add("acronym");
				allTags.Add("address");
				allTags.Add("applet");
				allTags.Add("area");
				allTags.Add("b");
				allTags.Add("base");
				allTags.Add("basefont");
				allTags.Add("bdo");
				allTags.Add("bgsound");
				allTags.Add("big");
				allTags.Add("blink");
				allTags.Add("blockquote");
				allTags.Add("bq");
				allTags.Add("body");
				allTags.Add("br");
				allTags.Add("button");
				allTags.Add("caption");
				allTags.Add("center");
				allTags.Add("cite");
				allTags.Add("code");
				allTags.Add("col");
				allTags.Add("colgroup");
				allTags.Add("del");
				allTags.Add("dfn");
				allTags.Add("dir");
				allTags.Add("div");
				allTags.Add("dl");
				allTags.Add("dt");
				allTags.Add("dd");
				allTags.Add("em");
				allTags.Add("embed");
				allTags.Add("fieldset");
				allTags.Add("font");
				allTags.Add("form");
				allTags.Add("frame");
				allTags.Add("frameset");
				allTags.Add("h1");
				allTags.Add("h2");
				allTags.Add("h3");
				allTags.Add("h4");
				allTags.Add("h5");
				allTags.Add("h6");
				allTags.Add("head");
				allTags.Add("html");
				allTags.Add("hr");
				allTags.Add("i");
				allTags.Add("iframe");
				allTags.Add("img");
				allTags.Add("input");
				allTags.Add("ins");
				allTags.Add("isindex");
				allTags.Add("kbd");
				allTags.Add("label");
				allTags.Add("legend");
				allTags.Add("li");
				allTags.Add("link");
				allTags.Add("listing");
				allTags.Add("map");
				allTags.Add("marquee");
				allTags.Add("menu");
				allTags.Add("meta");
				allTags.Add("nobr");
				allTags.Add("noframes");
				allTags.Add("noscript");
				allTags.Add("object");
				allTags.Add("ol");
				allTags.Add("option");
				allTags.Add("optgroup");
				allTags.Add("p");
				allTags.Add("param");
				allTags.Add("plaintext");
				allTags.Add("pre");
				allTags.Add("q");
				allTags.Add("s");
				allTags.Add("samp");
				allTags.Add("script");
				allTags.Add("select");
				allTags.Add("small");
				allTags.Add("span");
				allTags.Add("strike");
				allTags.Add("strong");
				allTags.Add("style");
				allTags.Add("sub");
				allTags.Add("sup");
				allTags.Add("table");
				allTags.Add("tbody");
				allTags.Add("td");
				allTags.Add("textarea");
				allTags.Add("tfoot");
				allTags.Add("th");
				allTags.Add("thead");
				allTags.Add("title");
				allTags.Add("tr");
				allTags.Add("tt");
				allTags.Add("u");
				allTags.Add("ul");
				allTags.Add("var");
				allTags.Add("wbr");
				allTags.Add("xmp");
			}
			{
				// actual block level elements
				swallowOne.Add("address");
				swallowTwo.Add("blockquote");
				swallowTwo.Add("center");
				swallowOne.Add("dir");
				swallowOne.Add("div");
				swallowTwo.Add("dl");
				swallowTwo.Add("fieldset");
				swallowTwo.Add("form");
				swallowTwo.Add("h1");
				swallowTwo.Add("h2");
				swallowTwo.Add("h3");
				swallowTwo.Add("h4");
				swallowTwo.Add("h5");
				swallowTwo.Add("h6");
				swallowTwo.Add("hr");
				swallowTwo.Add("isindex");
				swallowAll.Add("menu");
				swallowAll.Add("noframes");
				swallowAll.Add("noscript");
				swallowTwo.Add("ol");
				swallowTwo.Add("p");
				swallowTwo.Add("pre");
				swallowOne.Add("table");
				swallowTwo.Add("ul");
				
				// to be treated as block level elements
				swallowTwo.Add("br");
				swallowTwo.Add("dd");
				swallowTwo.Add("dt");
				swallowTwo.Add("frameset");
				swallowTwo.Add("li");
				swallowAll.Add("tbody");
				swallowTwo.Add("td");
				swallowAll.Add("tfoot");
				swallowOne.Add("th");
				swallowAll.Add("thead");
				swallowAll.Add("tr");
			}
			{
				emptyTags.Add("area");
				emptyTags.Add("base");
				emptyTags.Add("basefont");
				emptyTags.Add("br");
				emptyTags.Add("col");
				emptyTags.Add("frame");
				emptyTags.Add("hr");
				emptyTags.Add("img");
				emptyTags.Add("input");
				emptyTags.Add("isindex");
				emptyTags.Add("link");
				emptyTags.Add("meta");
				emptyTags.Add("param");
			}
		}

		/// <summary>
		/// Push in the stack
		/// </summary>
		/// <param name="stack">The stack.</param>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public static System.Object StackPush(System.Collections.Stack stack, System.Object element)
		{
			stack.Push(element);
			return element;
		}

		private struct EncodeParams
		{
			public static bool InsideTag;
			public static bool InsideHtmlTag;
			public static bool InsideCloseTag;
			public static sbyte HtmlTagMode;
			public static bool InsideCodeTag;
			public static bool InsidePreTag;
			public static bool InsideMacroTag;
			public static bool InsideComment;
			public static char HtmlQuoteChar;
			public static char MacroQuoteChar;
			public static int SwallowLinebreaks;
			public static int Linebreaks;
			public static bool Escape;

			public static void Init()
			{
				InsideTag = false;
				InsideHtmlTag = false;
				InsideCloseTag = false;
				HtmlTagMode = TagName;
				InsideCodeTag = false;
				InsidePreTag = false;
				InsideMacroTag = false;
				InsideComment = false;
				HtmlQuoteChar = UnicodeNull;
				MacroQuoteChar = UnicodeNull;
				SwallowLinebreaks = 0;
				Linebreaks = 0;
				Escape = false;
			}
		}
	}
	// end of class
}