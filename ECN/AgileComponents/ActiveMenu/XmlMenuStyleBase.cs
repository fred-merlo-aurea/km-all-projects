using System;
using System.Drawing;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	#region XmlMenuStyleBase

	/// <summary>
	/// Represents a <see cref="XmlMenuStyleBase"/> object.
	/// </summary>
	public abstract class XmlMenuStyleBase
	{
		#region Fields

		private string _backColor;
		private string _borderColor;
		private BorderStyle _borderStyle;
		private int _borderWidth;
		private string _cssClass;
		private string _foreColor;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMenuStyleBase"/> class.
		/// </summary>
		public XmlMenuStyleBase()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		/// <value>The color of the back.</value>
		[
		System.Xml.Serialization.XmlElement("backcolor",DataType="string"),
		Browsable(true)
		]
		public string BackColor
		{
			get
			{
				return _backColor;
			}

			set
			{
				_backColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		/// <value>The color of the border.</value>
		[
		System.Xml.Serialization.XmlElement("bordercolor",DataType="string"),
		Browsable(true)
		]
		public string BorderColor
		{
			get
			{
				return _borderColor;
			}

			set
			{
				_borderColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		/// <value>The border style.</value>
		[
		System.Xml.Serialization.XmlElement("borderstyle",typeof(BorderStyle)),
		Browsable(true)
		]
		public BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}

			set
			{
				_borderStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class.
		/// </summary>
		/// <value>The CSS class.</value>
		[
		System.Xml.Serialization.XmlElement("cssclass",DataType="string"),
		Browsable(true)
		]
		public string CssClass
		{
			get
			{
				return _cssClass;
			}

			set
			{
				_cssClass = value;
			}
		}

		/// <summary>
		/// Gets or sets the fore color.
		/// </summary>
		/// <value>The fore color.</value>
		[
		System.Xml.Serialization.XmlElement("forecolor",DataType="string"),
		Browsable(true)
		]
		public string ForeColor
		{
			get
			{
				return _foreColor;
			}

			set
			{
				_foreColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width.
		/// </summary>
		/// <value>The border width.</value>
		[
		System.Xml.Serialization.XmlElement("borderwidth",DataType="int"),
		Browsable(true)
		]
		public int BorderWidth
		{
			get
			{
				return _borderWidth;
			}

			set
			{
				_borderWidth = value;
			}
		}
		
		#endregion
	}

	#endregion
}
