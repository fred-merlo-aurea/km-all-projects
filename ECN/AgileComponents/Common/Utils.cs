using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Text.RegularExpressions;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region class Utils

	/// <summary>
	/// Contains severals tools.
	/// </summary>
	internal class Utils
	{
		private static int index = 0;

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Utils()
		{
		}

		#endregion

		#region Functions

		public static HtmlTextWriter tagWriter;

		public static HtmlTextWriter GetCorrectTagWriter( HtmlTextWriter writer ) 
		{
			if ( tagWriter != null ) return tagWriter;

			if ( writer is System.Web.UI.Html32TextWriter ) 
			{
				tagWriter =  new HtmlTextWriter( writer.InnerWriter );
			} 
			else 
			{
				tagWriter = writer;
			}
			return tagWriter;
		}

		/// <summary>
		/// Converts a Color object to this html string representation.
		/// </summary>
		/// <param name="color">Color object to convert.</param>
		/// <returns>Html string representation.</returns>
		public static string Color2Hex(System.Drawing.Color color)
		{
			if (color.IsEmpty)
				return "#FFFFFF";
			else
				return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		/// <summary>
		/// Gets the resolved data source.
		/// </summary>
		/// <param name="dataSource">Data source.</param>
		/// <param name="dataMember">Data member.</param>
		/// <returns>Resolved data source enumerator.</returns>
		public static IEnumerable GetResolvedDataSource(object dataSource, string dataMember)
		{
			if (dataSource == null)
			{
				return null;
			}
			IListSource iListSource = dataSource as IListSource;
			if (iListSource != null)
			{
				IList iList = iListSource.GetList();
				if (!iListSource.ContainsListCollection)
				{
					return iList;
				}
				if (iList != null && iList is ITypedList)
				{
					PropertyDescriptorCollection propertyDescriptorCollection = ((ITypedList)iList).GetItemProperties(new PropertyDescriptor[0]);
					if (propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0)
					{
						throw new HttpException("List source without DataMembers");
					}
					PropertyDescriptor propertyDescriptor = null;
					if (dataMember == null || dataMember.Length == 0)
					{
						propertyDescriptor = propertyDescriptorCollection[0];
					}
					else
					{
						propertyDescriptor = propertyDescriptorCollection.Find(dataMember, true);
					}
					if (propertyDescriptor != null)
					{
						object local1 = iList[0];
						object local2 = propertyDescriptor.GetValue(local1);
						if (local2 != null && local2 is IEnumerable)
						{
							return (IEnumerable)local2;
						}
					}
					throw new HttpException("List source missing DataMember");
				}
			}
			if (dataSource is IEnumerable)
			{
				return (IEnumerable)dataSource;
			}
			else
			{
				return null;
			}
		}

		internal static string GetTextDecoration(bool underline, bool overline, bool strikeout)
		{
			string line = string.Empty;

			if (underline)
			{
				line = "underline";
			}
			if (overline)
			{
				line = String.Concat(line, " overline");
			}
			if (strikeout)
			{
				line = String.Concat(line, " line-through");
			}

			return line;
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
		/// Get the specified resource from the assembly.
		/// </summary>
		/// <param name="resource">The name of the resource.</param>
		/// <returns>The string representation of the resource.</returns>
		public static string GetResource(string resource)
		{
			return GetResource(resource, null);
		}

		/// <summary>
		/// Create a string represents a style.
		/// </summary>
		/// <param name="backColor">Background color used with the style.</param>
		/// <param name="backImage">Background image used with the style.</param>
		/// <param name="foreColor">Foreground color used with the style.</param>
		/// <param name="imagePath">Path where the images are located.</param>
		/// <returns></returns>
		public static string CreateStyleVariable(System.Drawing.Color backColor, string backImage, System.Drawing.Color foreColor, string imagePath)
		{
			return CreateStyleVariable(backColor,backImage,foreColor,imagePath,string.Empty);
		}

		/// <summary>
		/// Create a string represents a style.
		/// </summary>
		/// <param name="backColor">Background color used with the style.</param>
		/// <param name="backImage">Background image used with the style.</param>
		/// <param name="foreColor">Foreground color used with the style.</param>
		/// <param name="imagePath">Path where the images are located.</param>
		/// <param name="cssClass">CSS class</param>
		/// <returns></returns>
		public static string CreateStyleVariable(System.Drawing.Color backColor, string backImage, System.Drawing.Color foreColor, string imagePath, string cssClass)
		{
			string ret = "";

			ret += "background-color:";
			ret += Utils.Color2Hex(backColor) + ";";
			if (backImage.Trim() != string.Empty) 
			{ 
				ret += "background-image:url("; 
				ret += ConvertToImageDir(imagePath,backImage) + ");"; 
			}
			ret += "fore-color:";
			ret += Utils.Color2Hex(foreColor) + ";";
			if (cssClass.Trim() != string.Empty) 
			{
				ret += "class:";
				ret += cssClass;
				ret += ";";
			}
			
			return ret;
		}

		/// <summary>
		/// Convert a image directory and the images file in full path.
		/// </summary>
		/// <param name="imageDir">Directory where the images are located.</param>
		/// <param name="image">Images filename.</param>
		/// <returns>Full path containing the path and the image filename.</returns>
        /// 
        public static string ConvertToImageDir(string imageDir, string image)
		{
			string result = string.Empty;
			if (imageDir != string.Empty && image.IndexOf(":") == -1 && image.IndexOf("http") == -1 && image != string.Empty)
			{
				if (imageDir[imageDir.Length-1] != '/')
					result = imageDir + @"/";
				else
					result = imageDir;
			}

			result += image;

			return result;
		
		}

        public static string ConvertToImageDir(string imageDir, string image, string ressourceName, Page page, Type type)
        {
#if (!FX1_1)
            if (/*imageDir == string.Empty &&*/ image == string.Empty && page != null)
                return page.ClientScript.GetWebResourceUrl(type, "ActiveUp.WebControls._resources.Images." + ressourceName);
            else
                return ConvertToImageDir(imageDir,image);
#else
            return ConvertToImageDir(imageDir,image);
#endif
        }

        public static string GetCorrectImageDir(string imageDir)
        {
            string result = string.Empty;
			if (imageDir != string.Empty)
			{
				if (imageDir[imageDir.Length-1] != '/')
					result = imageDir + @"/";
				else
					result = imageDir;
			}

			return result;
        }

		/// <summary>
		/// Get an embedded resource using this name.
		/// </summary>
		/// <param name="resName">Name of the resource.</param>
		/// <returns>An object contenaing the resource.</returns>
		public static object GetEmbeddedResource(string resName) 
		{
			/*resName = resName.Replace("/", ".").Replace("\\", ".");
			if (resName.IndexOf(Utils.ClassType.Namespace) != 0) 
			{
				resName = Utils.ClassType.Namespace + "." + resName;
			}*/
			Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName);
			if (s != null) 
			{
				string ext = resName.Substring(resName.LastIndexOf(".") + 1);
				switch (ext.ToLower()) 
				{
					case "txt":
					case "htm":
					case "html":
						StreamReader sr = new StreamReader(s);
						return sr.ReadToEnd();
					case "xml":
					case "config":
						StreamReader sr2 = new StreamReader(s);
						XmlDocument xDoc = new XmlDocument();
						xDoc.LoadXml(sr2.ReadToEnd());
						return xDoc;
					case "ico":
						return new System.Drawing.Icon(s);
					case "bmp":
					case "gif":
					case "jpg":
					case "jpeg":
					case "exif":
					case "wmf":
					case "emf":
					case "png":
					case "tif":
					case "tiff": 
						return new System.Drawing.Bitmap(s);
					default:
						return s;
				}
			}
			return null;
		}

		internal static void AddStyleFontAttribute(HtmlTextWriter writer, FontInfo fontInfo)
		{
			string[] fontNames = fontInfo.Names;	
			if ((int)fontNames.Length > 0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, FormatStringArray(fontNames, ','));
			}

			FontUnit fontUnit = fontInfo.Size;
			if (!fontUnit.IsEmpty)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, fontUnit.ToString(CultureInfo.InvariantCulture));
			}
			bool bold = fontInfo.Bold;
			if (bold)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
			}
			bool italic = fontInfo.Italic;
			if (italic)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontStyle, "italic");
			}
			bool underline = fontInfo.Underline;
			bool overline = fontInfo.Overline;
			bool strikeout = fontInfo.Strikeout;
			string textDecoration = string.Empty;

			if (underline)
			{
				textDecoration = "underline";
			}

			if (overline)
			{
				textDecoration = String.Concat(textDecoration, " overline");
			}

			if (strikeout)
			{
				textDecoration = String.Concat(textDecoration, " line-through");
			}

			if (textDecoration.Length > 0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.TextDecoration, textDecoration);
			}
		}

		internal static string FormatStringArray(string[] array, char delimiter)
		{
			int i = (int)array.Length;
			string str1 = String.Empty;
			if (i == 1)
			{
				return array[0];
			}
			if (i != 0)
			{
				StringBuilder stringBuilder = new StringBuilder(i * array[0].Length);
				for (int j = 0; j < i; j++)
				{
					string str2 = array[j].ToString();
					if (j > 0)
					{
						stringBuilder.Append(delimiter);
					}
					stringBuilder.Append(str2);
				}
				str1 = stringBuilder.ToString();
			}
			return str1;
		}

		internal static string ConvertStringArrayToRegisterArray(string[] sArray)
		{
			string result = string.Empty;	
			char separator = ',';
			foreach (string s in sArray)
			{
				result += string.Format("'{0}'",s);
				result += separator;
			}

			result = result.TrimEnd(separator);

			return result;
		} 

		internal static Color HexStringToColor(string hexColor)
		{
			string hc = ExtractHexDigits(hexColor);
			if (hc.Length != 6)
			{
				// you can choose whether to throw an exception
				//throw new ArgumentException("hexColor is not exactly 6 digits.");
				return Color.Empty;
			}
			string r = hc.Substring(0, 2);
			string g = hc.Substring(2, 2);
			string b = hc.Substring(4, 2);
			Color color = Color.Empty;
			try
			{
				int ri 
					= Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
				int gi 
					= Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
				int bi 
					= Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
				color = Color.FromArgb(ri, gi, bi);
			}
			catch
			{
				// you can choose whether to throw an exception
				//throw new ArgumentException("Conversion failed.");
				return Color.Empty;
			}
			return color;
		}

		internal static string ExtractHexDigits(string input)
		{
			// remove any characters that are not digits (like #)
			Regex isHexDigit 
				= new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
			string newnum = "";
			foreach (char c in input)
			{
				if (isHexDigit.IsMatch(c.ToString()))
					newnum += c.ToString();
			}
			return newnum;
		}

		internal static string GetUniqueID()
		{
			return DateTime.Now.Ticks.ToString() + string.Format("{0:000}",index++);
		}

		internal static string GetUniqueID(string prefix)
		{
			return prefix + DateTime.Now.Ticks.ToString() + string.Format("{0:000}",index++);
		}

		#endregion

	}

	#endregion
}
