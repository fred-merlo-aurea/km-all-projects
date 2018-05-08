using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This is the main HTML parser class. I recommend you don't play around too much in here
	/// as it's a little fiddly.
	/// 
	/// Bascially, this class will build a tree containing HtmlNode elements.
	/// </summary>
	internal class HtmlParser
	{
	    private const string StartTagIdentifier = "<";
	    private const string StartTagIdentifierVariation1 = "</";
	    private const string EndTagIdentifier = ">";
	    private const string EndTagIdentifierVariation1 = "/>";
	    private const string EqualityIdentifier = "=";
	    private const string SlashIdentifier = "/";
	    private const string DoubleQuotesIdentifier = "\"";
	    private const string SingleQuotesIdentifier = "\'";
	    private const string HtmlCommentStartTagIdentifier = "<!--";
	    private const string HtmlCommentEndTagIdentifier = "-->";
	    private const string SgmlCommentsStartTagIdentifier = "<!";

	    private static readonly char[] WhiteSpaceChars = " \t\r\n".ToCharArray();
	    private delegate int ProcessNode(TokenizerArgument tokenizerArgument);

        private bool _removeEmptyElementText = false;

		/// <summary>
		/// This constructs a new parser. Even though this object is currently stateless,
		/// in the future, parameters coping for tollerance and SGML (etc.) will be passed.
		/// </summary>
		public HtmlParser()
		{
		}

		/// <summary>
		/// The default mechanism will extract a pure DOM tree, which will contain many text
		/// nodes containing just whitespace (carriage returns etc.) However, with normal
		/// parsing, these are useless and only serve to complicate matters. Therefore, this
		/// option exists to automatically remove those empty text nodes.
		/// </summary>
		public bool RemoveEmptyElementText
		{
			get
			{
				return _removeEmptyElementText;
			}
			set
			{
				_removeEmptyElementText = value;
			}
		}

		#region The main parser

        /// <summary>
        /// This will parse a string containing HTML and will produce a domain tree.
        /// </summary>
        /// <param name="html">The HTML to be parsed</param>
        /// <returns>A tree representing the elements</returns>
        public HtmlNodeCollection Parse(string html)
		{
            var nodes = new HtmlNodeCollection(null);
            if (html.IsNullOrWhiteSpace())
            {
                return nodes;
            }

            html = PreprocessScript(html, "script");
            html = PreprocessScript(html, "style");
            html = RemoveComments(html);
            html = RemoveSGMLComments(html);

            var tokens = GetTokens(html);

            var index = 0;
            HtmlElement element = null;
            while (index < tokens.Count)
            {
                if (StartTagIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
                {
                    index = ProcessStartTag(tokens, index, htmlElement =>
                    {
                        nodes.Add(htmlElement);
                        element = htmlElement;
                    });

                    if (index == -1)
                    {
                        break;
                    }
                }
                else if (EndTagIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
                {
                    index++;
                }
                else if (StartTagIdentifierVariation1.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
                {
                    index = ProcessCloseTag(tokens, index, nodes);
                    if (index == -1)
                    {
                        break;
                    }
                }
                else
                {
                    index = ProcessTextNode(tokens, index, element, nodes);
                }
            }

            return nodes;
		}

        private int ProcessStartTag(StringCollection tokens, int index, Action<HtmlElement> processHtmlElement)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (++index >= tokens.Count)
            {
                return -1;
            }

            var tagName = tokens[index++];
            var element = new HtmlElement(tagName);

            while (index < tokens.Count && 
                    !EndTagIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase) && 
                    !EndTagIdentifierVariation1.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
            {
                var attributeName = tokens[index++];
                var attributeValue = string.Empty;

                if (index < tokens.Count && 
                    EqualityIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
                {
                    attributeValue = HtmlElementEncoder.DecodeValue(++index < tokens.Count ? tokens[index] : null);
                    index++;
                }
                else if (index < tokens.Count)
                {
                    attributeValue = null;
                }

                var attribute = new HtmlAttribute(attributeName, attributeValue);
                element.Attributes.Add(attribute);
            }

            processHtmlElement?.Invoke(element);

            if (index < tokens.Count && 
                EndTagIdentifierVariation1.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
            {
                element.IsTerminated = true;
                index++;
            }
            else if (index < tokens.Count && 
                    EndTagIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
            {
                index++;
            }

            return index;
        }

	    private int ProcessCloseTag(StringCollection tokens, int index, HtmlNodeCollection nodes)
	    {
	        if (tokens == null)
	        {
	            throw new ArgumentNullException(nameof(tokens));
	        }

	        if (nodes == null)
	        {
	            throw new ArgumentNullException(nameof(nodes));
	        }

            if (++index >= tokens.Count)
            {
                return -1;
            }

	        var tagName = tokens[index++];
	        var openIndex = FindTagOpenNodeIndex(nodes, tagName);
	        if (openIndex != -1)
	        {
	            MoveNodesDown(ref nodes, openIndex + 1, (HtmlElement) nodes[openIndex]);
	        }

	        // Skip to the end of this tag
	        while (index < tokens.Count && !EndTagIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
	        {
	            index++;
	        }

	        if (index < tokens.Count && EndTagIdentifier.Equals(tokens[index], StringComparison.OrdinalIgnoreCase))
	        {
	            index++;
	        }

	        return index;
	    }

	    private int ProcessTextNode(StringCollection tokens, int index, HtmlElement htmlElement, HtmlNodeCollection nodes)
	    {
	        if (tokens == null)
	        {
	            throw new ArgumentNullException(nameof(tokens));
	        }

	        if (nodes == null)
	        {
	            throw new ArgumentNullException(nameof(nodes));
	        }

            var value = tokens[index];
	        if (_removeEmptyElementText)
	        {
	            value = RemoveWhitespace(value);
	        }

	        value = DecodeScript(value);
	        if (_removeEmptyElementText && (value == null || value.Length == 0))
	        {
	            return ++index;
	        }

	        if (htmlElement == null || !htmlElement.NoEscaping)
	        {
	            value = HtmlElementEncoder.DecodeValue(value);
	        }

	        nodes.Add(new HtmlText(value));

	        return ++index;
	    }

	    /// <summary>
		/// This will move all the nodes from the specified index to the new parent.
		/// </summary>
		/// <param name="nodes">The collection of nodes</param>
		/// <param name="node_index">The index of the first node (in the above collection) to move</param>
		/// <param name="new_parent">The node which will become the parent of the moved nodes</param>

		private void MoveNodesDown(ref HtmlNodeCollection nodes,int node_index,HtmlElement new_parent)
		{
			for( int i = node_index ; i < nodes.Count ; i++ )
			{
				((HtmlElement)new_parent).Nodes.Add( nodes[i] );
				nodes[i].SetParent( new_parent );
			}
			int c = nodes.Count;
			for( int i = node_index ; i < c ; i++ )
			{
				nodes.RemoveAt( node_index );
			}
			new_parent.IsExplicitlyTerminated = true;
		}

		/// <summary>
		/// This will find the corresponding opening tag for the named one. This is identified as
		/// the most recently read node with the same name, but with no child nodes.
		/// </summary>
		/// <param name="nodes">The collection of nodes</param>
		/// <param name="name">The name of the tag</param>
		/// <returns>The index of the opening tag, or -1 if it was not found</returns>
		private int FindTagOpenNodeIndex(HtmlNodeCollection nodes,string name)
		{
			for( int index = nodes.Count - 1 ; index >= 0 ; index-- )
			{
				if( nodes[index] is HtmlElement )
				{
					if( ( (HtmlElement) nodes[index] ).Name.ToLower().Equals( name.ToLower() ) && ( (HtmlElement) nodes[index] ).Nodes.Count == 0 && ( (HtmlElement) nodes[index] ).IsTerminated == false )
					{
						return index;
					}
				}
			}
			return -1;
		}

		#endregion

		#region HTML clean-up functions

		/// <summary>
		/// This will remove redundant whitespace from the string
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private string RemoveWhitespace(string input)
		{
			string output = input.Replace( "\r" , "" );
			output = output.Replace( "\n" , "" );
			output = output.Replace( "\t" , " " );
			output = output.Trim();
			return output;
		}

		/// <summary>
		/// This will remove all HTML comments from the input string. This will
		/// not remove comment markers from inside tag attribute values.
		/// </summary>
		/// <param name="input">Input HTML containing comments</param>
		/// <returns>HTML containing no comments</returns>

		private string RemoveComments(string input)
		{
		    return ProcessComments(input, HtmlCommentStartTagIdentifier, HtmlCommentEndTagIdentifier);
		}	    

        /// <summary>
		/// This will remove all HTML comments from the input string. This will
		/// not remove comment markers from inside tag attribute values.
		/// </summary>
		/// <param name="input">Input HTML containing comments</param>
		/// <returns>HTML containing no comments</returns>
		private string RemoveSGMLComments(string input)
		{
		    return ProcessComments(input, SgmlCommentsStartTagIdentifier, EndTagIdentifier);
        }

		private static string ProcessComments(
		    string input, 
		    string commentsStartTagIdentifier, 
		    string commentsEndTagIdentifier)
		{
		    if (input.IsNullOrWhiteSpace())
		    {
			    return input;
		    }

		    var commentsStartTagIdentifierLength = commentsStartTagIdentifier.Length;
		    var quotesIdentifier = string.Empty;
		    var index = 0;
		    var inTag = false;
		    var output = new StringBuilder();
		    while (index < input.Length)
		    {
			    if (index + commentsStartTagIdentifierLength < input.Length &&
			        input
                        .Substring(index, commentsStartTagIdentifierLength)
                        .Equals(commentsStartTagIdentifier, StringComparison.OrdinalIgnoreCase))
			    {
			        index += commentsStartTagIdentifierLength;
			        index = input.IndexOf(commentsEndTagIdentifier, index, StringComparison.Ordinal);
			        if (index == -1)
			        {
				        break;
			        }

			        index += commentsEndTagIdentifier.Length;
			    }
			    else if (input.Substring(index, StartTagIdentifier.Length)
                              .Equals(StartTagIdentifier, StringComparison.OrdinalIgnoreCase))
			    {
			        inTag = true;
			        output.Append(StartTagIdentifier);
			        index += StartTagIdentifier.Length;
			    }
			    else if (input.Substring(index, EndTagIdentifier.Length)
			                  .Equals(EndTagIdentifier, StringComparison.OrdinalIgnoreCase))
			    {
			        inTag = false;
			        output.Append(EndTagIdentifier);
			        index += EndTagIdentifier.Length;
			    }
			    else if (inTag && IsQuotesTag(input, index, out quotesIdentifier))
			    {
			        var startIndex = index;
			        index++;
			        index = input.IndexOf(quotesIdentifier, index, StringComparison.OrdinalIgnoreCase);
			        if (index == -1)
			        {
				        break;
			        }

			        index++;
			        output.Append(input.Substring(startIndex, index - startIndex));
			    }
			    else
			    {
			        output.Append(input.Substring(index, 1));
			        index++;
			    }
		    }

		    return output.ToString();
		}

	    private static bool IsQuotesTag(string input, int index, out string quotesIdentifier)
	    {
		    quotesIdentifier = string.Empty;

		    if (input.IsNullOrWhiteSpace())
		    {
		        return false;
		    }

		    if (input
		        .Substring(index, DoubleQuotesIdentifier.Length)
		        .Equals(DoubleQuotesIdentifier, StringComparison.OrdinalIgnoreCase))
		    {
		        quotesIdentifier = DoubleQuotesIdentifier;
			    return true;
		    }

		    if (input.Substring(index, 1).Equals(SingleQuotesIdentifier, StringComparison.OrdinalIgnoreCase))
		    {
		        quotesIdentifier = SingleQuotesIdentifier;
			    return true;
		    }

		    return false;
	    }

	    /// <summary>
        /// This will encode the scripts within the page so they get passed through the
        /// parser properly. This is due to some people using comments protect the script
        /// and others who don't. It also takes care of issues where the script itself has
        /// HTML comments in (in strings, for example).
        /// </summary>
        /// <param name="input">The HTML to examine</param>
        /// <param name="tag_name">The tag name.</param>
        /// <returns>
        /// The HTML with the scripts marked up differently
        /// </returns>
        private string PreprocessScript(string input,string tag_name)
		{
			StringBuilder output = new StringBuilder();
			int index = 0;
			int tag_name_len = tag_name.Length;
			while( index < input.Length )
			{
				bool omit_body = false;
				if( index + tag_name_len + 1 < input.Length && input.Substring( index , tag_name_len + 1 ).ToLower().Equals( "<" + tag_name ) )
				{
					// Look for the end of the tag (we pass the attributes through as normal)
					do
					{
						if( index >= input.Length )
						{
							break;
						}
						else if( input.Substring( index , 1 ).Equals( ">" ) ) 
						{
							output.Append( ">" );
							index++;
							break;
						}
						else if( index + 1 < input.Length && input.Substring( index , 2 ).Equals( "/>" ) ) 
						{
							output.Append( "/>" );
							index += 2;
							omit_body = true;
							break;
						}
						else if( input.Substring( index , 1 ).Equals( "\"" ) ) 
						{
							output.Append( "\"" );
							index++;
							while( index < input.Length && ! input.Substring( index , 1 ).Equals( "\"" ) )
							{
								output.Append( input.Substring( index , 1 ) );
								index++;
							}
							if( index < input.Length )
							{
								index++;
								output.Append( "\"" );
							}
						}
						else if( input.Substring( index , 1 ).Equals( "\'" ) ) 
						{
							output.Append( "\'" );
							index++;
							while( index < input.Length && ! input.Substring( index , 1 ).Equals( "\'" ) )
							{
								output.Append( input.Substring( index , 1 ) );
								index++;
							}
							if( index < input.Length )
							{
								index++;
								output.Append( "\'" );
							}
						}
						else
						{
							output.Append( input.Substring( index , 1 ) );
							index++;
						}
					} while( true );
					if( index >= input.Length ) break;
					// Phew! Ok now we are reading the script body

					if( ! omit_body )
					{
						StringBuilder script_body = new StringBuilder();
						while( index + tag_name_len + 3 < input.Length && ! input.Substring( index , tag_name_len + 3 ).ToLower().Equals( "</" + tag_name + ">" ) )
						{
							script_body.Append( input.Substring( index , 1 ) );
							index++;
						}
						// Done - now encode the script
						output.Append( EncodeScript( script_body.ToString() ) );
						output.Append( "</" + tag_name + ">" );
						if( index + tag_name_len + 3 < input.Length )
						{
							index += tag_name_len + 3;
						}
					}
				}
				else
				{
					output.Append( input.Substring( index , 1 ) );
					index++;
				}
			}
			return output.ToString();
		}


		private static string EncodeScript(string script)
		{
			string output = script.Replace( "<" , "[MIL-SCRIPT-LT]" );
			output = output.Replace( ">" , "[MIL-SCRIPT-GT]" );
			output = output.Replace( "\r" , "[MIL-SCRIPT-CR]" );
			output = output.Replace( "\n" , "[MIL-SCRIPT-LF]" );
			return output;
		}

		private static string DecodeScript(string script)
		{
			string output = script.Replace( "[MIL-SCRIPT-LT]" , "<" );
			output = output.Replace( "[MIL-SCRIPT-GT]" , ">" );
			output = output.Replace( "[MIL-SCRIPT-CR]" , "\r" );
			output = output.Replace( "[MIL-SCRIPT-LF]" , "\n" );
			return output;
		}

        #endregion

        #region HTML tokeniser

        /// <summary>
        /// This will tokenise the HTML input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private StringCollection GetTokens(string input)
        {
            var tokens = new StringCollection();
            if (input.IsNullOrWhiteSpace())
            {
                return tokens;
            }

            var nodeProcessorMappings = new Dictionary<ParseStatus, ProcessNode>
            {
                [ParseStatus.ReadText] = ReadTextNode,
                [ParseStatus.ReadStartTag] = ReadStartTag,
                [ParseStatus.ReadEndTag] = ReadEndTag,
                [ParseStatus.ReadAttributeName] = ReadAttributeName,
                [ParseStatus.ReadAttributeValue] = ReadAttributeValue
            };

            var index = 0;
            var status = ParseStatus.ReadText;
            while (index < input.Length)
            {
                if (nodeProcessorMappings.ContainsKey(status))
                {
                    var tokenizerArg = new TokenizerArgument(input, index, status, tokens);
                    index = nodeProcessorMappings[status].Invoke(tokenizerArg);
                    status = tokenizerArg.Status;
                    if (index == -1)
                    {
                        break;
                    }
                }
            }

            return tokens;
        }

        private static int ReadTextNode(TokenizerArgument tokenizerArg)
        {
            if (tokenizerArg == null)
            {
                throw new ArgumentNullException(nameof(tokenizerArg));
            }

            var index = tokenizerArg.Index;
            var startTagIdentifierLength = StartTagIdentifier.Length;
            var startTagIdentifierVariation1Length = StartTagIdentifierVariation1.Length;

            if (index + startTagIdentifierVariation1Length < tokenizerArg.Input.Length &&
                tokenizerArg.Input.Substring(index, startTagIdentifierVariation1Length).Equals(StartTagIdentifierVariation1))
            {
                tokenizerArg.Output.Add(StartTagIdentifierVariation1);
                tokenizerArg.Status = ParseStatus.ReadEndTag;
                index += startTagIdentifierVariation1Length;
            }
            else if (tokenizerArg.Input.Substring(index, startTagIdentifierLength).Equals(StartTagIdentifier))
            {
                tokenizerArg.Output.Add(StartTagIdentifier);
                tokenizerArg.Status = ParseStatus.ReadStartTag;
                index += startTagIdentifierLength;
            }
            else
            {
                var nextIndex = tokenizerArg.Input.IndexOf(StartTagIdentifier, index, StringComparison.OrdinalIgnoreCase);
                tokenizerArg.Output.Add(nextIndex == -1
                    ? tokenizerArg.Input.Substring(index)
                    : tokenizerArg.Input.Substring(index, nextIndex - index));
                index = nextIndex;
            }

            return index;
        }

        private static int ReadStartTag(TokenizerArgument tokenizerArg)
        {
            if (tokenizerArg == null)
            {
                throw new ArgumentNullException(nameof(tokenizerArg));
            }

            var index = tokenizerArg.Index;
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            var tagNameStartIndex = index;
            var endTagIdentifierLength = EndTagIdentifier.Length;
            var endTagIdentifierVariation1Length = EndTagIdentifierVariation1.Length;
            index = GetIndexOfNextTagIdentifier(tokenizerArg.Input, index, EndTagIdentifierVariation1);
            tokenizerArg.Output.Add(tokenizerArg.Input.Substring(tagNameStartIndex, index - tagNameStartIndex));
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            if (index + 1 < tokenizerArg.Input.Length &&
                tokenizerArg.Input.Substring(index, endTagIdentifierVariation1Length).Equals(EndTagIdentifierVariation1))
            {
                tokenizerArg.Output.Add(EndTagIdentifierVariation1);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierVariation1Length;
            }
            else if (index < tokenizerArg.Input.Length &&
                     tokenizerArg.Input.Substring(index, endTagIdentifierLength).Equals(EndTagIdentifier))
            {
                tokenizerArg.Output.Add(EndTagIdentifier);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierLength;
            }
            else
            {
                tokenizerArg.Status = ParseStatus.ReadAttributeName;
            }

            return index;
        }

        private static int ReadEndTag(TokenizerArgument tokenizerArg)
        {
            if (tokenizerArg == null)
            {
                throw new ArgumentNullException(nameof(tokenizerArg));
            }

            var index = tokenizerArg.Index;
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            var tagNameStartIndex = index;
            index = GetIndexOfNextTagIdentifier(tokenizerArg.Input, index, EndTagIdentifier);
            tokenizerArg.Output.Add(tokenizerArg.Input.Substring(tagNameStartIndex, index - tagNameStartIndex));
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            var endTagIdentifierLength = EndTagIdentifier.Length;
            if (index < tokenizerArg.Input.Length &&
                tokenizerArg.Input.Substring(index, endTagIdentifierLength).Equals(EndTagIdentifier))
            {
                tokenizerArg.Output.Add(EndTagIdentifier);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierLength;
            }

            return index;
        }

        private static int ReadAttributeName(TokenizerArgument tokenizerArg)
        {
            if (tokenizerArg == null)
            {
                throw new ArgumentNullException(nameof(tokenizerArg));
            }

            var index = tokenizerArg.Index;
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            var attributeNameStartIndex = index;
            index =
                GetIndexOfNextTagIdentifier(tokenizerArg.Input, index, EndTagIdentifierVariation1 + EqualityIdentifier);
            tokenizerArg.Output.Add(
                tokenizerArg.Input.Substring(attributeNameStartIndex, index - attributeNameStartIndex));
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            var endTagIdentifierLength = EndTagIdentifier.Length;
            var endTagIdentifierVariation1Length = EndTagIdentifierVariation1.Length;
            var equalityIdentifierLength = EqualityIdentifier.Length;
            var slashIdentifierLength = SlashIdentifier.Length;

            if (index + 1 < tokenizerArg.Input.Length &&
                tokenizerArg.Input.Substring(index, endTagIdentifierVariation1Length).Equals(EndTagIdentifierVariation1))
            {
                tokenizerArg.Output.Add(EndTagIdentifierVariation1);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierVariation1Length;
            }
            else if (index < tokenizerArg.Input.Length &&
                     tokenizerArg.Input.Substring(index, endTagIdentifierLength).Equals(EndTagIdentifier))
            {
                tokenizerArg.Output.Add(EndTagIdentifier);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierLength;
            }
            else if (index < tokenizerArg.Input.Length &&
                     tokenizerArg.Input.Substring(index, equalityIdentifierLength).Equals(EqualityIdentifier))
            {
                tokenizerArg.Output.Add(EqualityIdentifier);
                tokenizerArg.Status = ParseStatus.ReadAttributeValue;
                index += equalityIdentifierLength;
            }
            else if (index < tokenizerArg.Input.Length &&
                     tokenizerArg.Input.Substring(index, slashIdentifierLength).Equals(SlashIdentifier))
            {
                index += slashIdentifierLength;
            }

            return index;
        }

        private static int ReadAttributeValue(TokenizerArgument tokenizerArg)
        {
            if (tokenizerArg == null)
            {
                throw new ArgumentNullException(nameof(tokenizerArg));
            }

            var index = tokenizerArg.Index;
            index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);

            var valueStartIndex = index;
            var value = index < tokenizerArg.Input.Length
                ? tokenizerArg.Input.Substring(index, 1)
                : string.Empty;
            var isDoubleQuotes = value.Equals(DoubleQuotesIdentifier);
            var isSingleQuotes = value.Equals(SingleQuotesIdentifier);
            if (isDoubleQuotes || isSingleQuotes)
            {
                index++;
                index = GetIndexOfNextDelimiter(
                    tokenizerArg.Input,
                    index,
                    isDoubleQuotes ? DoubleQuotesIdentifier : SingleQuotesIdentifier);
                tokenizerArg.Output.Add(tokenizerArg.Input.Substring(valueStartIndex + 1, index - valueStartIndex - 2));
                tokenizerArg.Status = ParseStatus.ReadAttributeName;
            }
            else
            {
                index = GetIndexOfNextTagIdentifier(tokenizerArg.Input, index, EndTagIdentifierVariation1);
                tokenizerArg.Output.Add(tokenizerArg.Input.Substring(valueStartIndex, index - valueStartIndex));
                index = GetIndexOfNextNonWhitespaceChar(tokenizerArg.Input, index);
                tokenizerArg.Status = ParseStatus.ReadAttributeName;
            }

            var endTagIdentifierLength = EndTagIdentifier.Length;
            var endTagIdentifierVariation1Length = EndTagIdentifierVariation1.Length;
            if (index + 1 < tokenizerArg.Input.Length &&
                tokenizerArg.Input.Substring(index, endTagIdentifierVariation1Length).Equals(EndTagIdentifierVariation1))
            {
                tokenizerArg.Output.Add(EndTagIdentifierVariation1);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierVariation1Length;
            }
            else if (index < tokenizerArg.Input.Length &&
                     tokenizerArg.Input.Substring(index, endTagIdentifierLength).Equals(EndTagIdentifier))
            {
                tokenizerArg.Output.Add(EndTagIdentifier);
                tokenizerArg.Status = ParseStatus.ReadText;
                index += endTagIdentifierLength;
            }

            return index;
        }

        private static int GetIndexOfNextNonWhitespaceChar(string input, int startIndex)
        {
            // Whitespace strings are valid
            if (input == null)
            {
                return startIndex;
            }

            while (startIndex < input.Length && input.Substring(startIndex, 1).IndexOfAny(WhiteSpaceChars) != -1)
            {
                startIndex++;
            }

            return startIndex;
        }

        private static int GetIndexOfNextTagIdentifier(string input, int startIndex, string tagIdentifier)
        {
            // Whitespace strings are valid
            if (input == null || tagIdentifier == null)
            {
                return startIndex;
            }

            var tagIdentifierChars = new List<char>(WhiteSpaceChars);
            tagIdentifierChars.AddRange(tagIdentifier.ToCharArray());
            while (startIndex < input.Length &&
                   input.Substring(startIndex, 1).IndexOfAny(tagIdentifierChars.ToArray()) == -1)
            {
                startIndex++;
            }

            return startIndex;
        }

        private static int GetIndexOfNextDelimiter(string input, int startIndex, string delimiter)
        {
            // Whitespace strings are valid
            if (input == null || delimiter == null)
            {
                return startIndex;
            }

            while (startIndex < input.Length && !input.Substring(startIndex, 1).Equals(delimiter))
            {
                startIndex++;
            }

            if (startIndex < input.Length && input.Substring(startIndex, 1).Equals(delimiter))
            {
                startIndex++;
            }

            return startIndex;
        }

        #endregion
    }
}
