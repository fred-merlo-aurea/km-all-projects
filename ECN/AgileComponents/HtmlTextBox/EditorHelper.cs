using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="EditorHelper"/> object.
	/// </summary>
	public class EditorHelper
	{
		private static bool _showDebugMessages = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="EditorHelper"/> class.
		/// </summary>
		public EditorHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds a string in the trace.
		/// </summary>
		/// <param name="traceText">The string to add.</param>
		public static void DebugTrace(string traceText)
		{
			if (System.Web.HttpContext.Current != null)
				System.Web.HttpContext.Current.Trace.Write("HtmlTextBox3", traceText);
		} 

		/// <summary>
		/// Get the specified resource from the assembly.
		/// </summary>
		/// <param name="resource">The name of the resource.</param>
		/// <returns>The string representation of the resource.</returns>
		public static string GetResource(string resource)
		{
			return GetResource(resource, null);
		}

		/// <summary>
		/// Get the specified resource from the assembly.
		/// </summary>
		/// <param name="resource">The name of the resource.</param>
		/// <param name="type">The type of the assembly.</param>
		/// <returns>The string representation of the resource.</returns>
		public static string GetResource(string resource, System.Type type)
		{

			string str = null;
			Assembly asm;
			
			if (type != null)
				asm = Assembly.GetAssembly(type);
			else
				asm = Assembly.GetExecutingAssembly();
			// We check for null just in case the variable is called at design-time.
			string[] s = asm.GetManifestResourceNames();
			if (asm != null)
			{
				// Just for clarity define multiple variables.
				Stream stm = asm.GetManifestResourceStream(resource);
				StreamReader reader = new StreamReader(stm);
				str = reader.ReadToEnd();
				reader.Close();
				stm.Close();
			}

			return str;
		}

		/// <summary>
		/// Gets or sets a value indicating whether shows the debug messages.
		/// </summary>
		/// <value><c>true</c> if shows the debug messages; otherwise, <c>false</c>.</value>
		public static bool ShowDebugMessages
		{
			get
			{
				return _showDebugMessages;
			}
			set
			{
				_showDebugMessages = value;
			}
		}

		/// <summary>
		/// Encode the specified string.
		/// </summary>
		/// <param name="stringToEncode">The string to encode.</param>
		/// <returns>the encoded string.</returns>
		public static string Encode(string stringToEncode)
		{
			//stringToEncode = HttpUtility.HtmlEncode(stringToEncode);
			if (stringToEncode != null)
				return stringToEncode.Replace("<", "&lt;").Replace(">", "&gt;");
			else
				return null;
			/*stringToEncode = stringToEncode.Replace("\"", "&quot;");*/
			
		}

		/// <summary>
		/// Decode the specified string.
		/// </summary>
		/// <param name="stringToDecode">The string to decode.</param>
		/// <returns>The decoded string.</returns>
		public static string Decode(string stringToDecode)
		{
			//stringToDecode = HttpUtility.HtmlDecode(stringToDecode);
			/*if (stringToDecode == null)
				stringToDecode = string.Empty;
			stringToDecode = stringToDecode.Replace("&quot;", "\"");*/
			if (stringToDecode != null)
				return stringToDecode.Replace("&lt;", "<").Replace("&gt;", ">");
			else
				return null;
		}

		/// <summary>
		/// Remove potential malicious code from the string.
		/// </summary>
		/// <param name="input">The string to process.</param>
		/// <returns>The cleaned string.</returns>
		public static string RemoveMaliciousCode(string input) 
		{
			string pattern = @"(?m)<script[^>]*>(\w|\W)*?</script[^>]*>";
			string newStr  = Regex.Replace(input,pattern,"");
			newStr = newStr.Replace("javascript:", "javascript :");
			return newStr;
		}

		/// <summary>
		/// Format the specified ArrayList to be used on Client-Side script.
		/// </summary>
		/// <param name="collection">The collection to format.</param>
		/// <param name="encode">Specify whether you want the data to be HTML encoded or not.</param>
		/// <returns>The formatted array.</returns>
		public static string FormatCollection(string[] collection, bool encode)
		{
			return FormatCollection(collection, "", encode);
		}

		/// <summary>
		/// Format the specified string array to be used on Client-Side script.
		/// </summary>
		/// <param name="collection">The collection to format.</param>
		/// <param name="prefix">The prefix to use for the items.</param>
		/// <param name="encode">Specify whether you want the data to be HTML encoded or not.</param>
		/// <returns>The formatted array.</returns>
		public static string FormatCollection(string[] collection, string prefix, bool encode)
		{
			string str = string.Empty;

			foreach(string item in collection)
			{
				str += "\"" + prefix + (encode ? EditorHelper.Encode(item).Replace("'", "&quot;") : item) + "\",";
			}
			str = str.Trim().Trim(',').Trim().Trim(',');
			return str;
		}

		/// <summary>
		/// Format the specified ArrayList to be used on Client-Side script.
		/// </summary>
		/// <param name="collection">The collection to format.</param>
		/// <param name="encode">Specify whether you want the data to be HTML encoded or not.</param>
		/// <returns>The formatted array.</returns>
		public static string FormatCollection(ArrayList collection, bool encode)
		{
			return FormatCollection(collection, "", encode);
		}
	
		/// <summary>
		/// Format the specified ArrayList to be used on Client-Side script.
		/// </summary>
		/// <param name="collection">The collection to format.</param>
		/// <param name="prefix">The prefix to use for the items.</param>
		/// <param name="encode">Specify whether you want the data to be HTML encoded or not.</param>
		/// <returns>The formatted array.</returns>
		public static string FormatCollection(ArrayList collection, string prefix, bool encode)
		{
			string str = string.Empty;

			foreach(string item in collection)
			{
				str += "\"" + prefix + (encode ? EditorHelper.Encode(item).Replace("'", "&quot;") : item) + "\",";
			}

			str = str.Trim().Trim(',').Trim().Trim(',');
			return str;
		}

	}
}
