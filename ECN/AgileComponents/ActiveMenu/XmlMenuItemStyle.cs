using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
	#region XmlMenuItemStyle

	/// <summary>
	/// Represents a <see cref="XmlMenuItemStyle"/> object.
	/// </summary>
	[
	System.Xml.Serialization.XmlTypeAttribute,
	Serializable
	]
	public class XmlMenuItemStyle : XmlMenuStyleBase
	{
		#region Fields

		private bool _allowRollOver = false;
		private string _backColorOver;
		private string _borderColorOver;
		private int _borderWidth;
		private BorderStyle _borderStyleOver;
		private int _borderWidthOver;
		private string _cssClassOver;
		private int _margin;
		private string _foreColorOver;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMenuItemStyle"/> class.
		/// </summary>
		public XmlMenuItemStyle()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether rollover is allowed.
		/// </summary>
		/// <value><c>true</c> if rollover is allowed; otherwise, <c>false</c>.</value>
		[
		System.Xml.Serialization.XmlElement("allowrollover",DataType="boolean"),
		Browsable(true),
		DefaultValue(false)
		]
		public bool AllowRollOver
		{
			get
			{
				return _allowRollOver;
			}

			set
			{
				_allowRollOver = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color over.
		/// </summary>
		/// <value>The background color over.</value>
		[
		System.Xml.Serialization.XmlElement("backcolorover",DataType = "string"),
		Browsable(true)
		]
		public string BackColorOver
		{
			get
			{
				return _backColorOver;
			}

			set
			{
				_backColorOver = value;
			}
		}

		/// <summary>
		/// Gets or sets the border color over.
		/// </summary>
		/// <value>The border color over.</value>
		[
		System.Xml.Serialization.XmlElement("bordercolorover",DataType="string"),
		Browsable(true)
		]
		public string BorderColorOver
		{
			get
			{
				return _borderColorOver;
			}

			set
			{
				_borderColorOver = value;
			}
		}


		/// <summary>
		/// Gets or sets the border style over.
		/// </summary>
		/// <value>The border style over.</value>
		[
		System.Xml.Serialization.XmlElement("borderstyleover",typeof(BorderStyle)),
		Browsable(true)
		]
		public BorderStyle BorderStyleOver
		{
			get
			{
				return _borderStyleOver;
			}

			set
			{
				_borderStyleOver = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width over.
		/// </summary>
		/// <value>The border width over.</value>
		[
		System.Xml.Serialization.XmlElement("borderwidthover",DataType="int"),
		Browsable(true)
		]
		public int BorderWidthOver
		{
			get
			{
				return _borderWidthOver;
			}

			set
			{
				_borderWidthOver = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class over.
		/// </summary>
		/// <value>The CSS class over.</value>
		[
		System.Xml.Serialization.XmlElement("cssclassover",DataType="string"),
		Browsable(true)
		]
		public string CssClassOver
		{
			get
			{
				return _cssClassOver;
			}

			set
			{
				_cssClassOver = value;
			}
		}

		/// <summary>
		/// Gets or sets the margin.
		/// </summary>
		/// <value>The margin.</value>
		[
		System.Xml.Serialization.XmlElement("margin",DataType="int"),
		Browsable(true)
		]
		public int Margin
		{
			get
			{
				return _margin;
			}

			set
			{
				_margin = value;
			}
		}

		/// <summary>
		/// Gets or sets the fore color over.
		/// </summary>
		/// <value>The fore color over.</value>
		[
		System.Xml.Serialization.XmlElement("forecolorover",DataType="string"),
		Browsable(true)
		]
		public string ForeColorOver
		{
			get
			{
				return _foreColorOver;
			}

			set
			{
				_foreColorOver = value;
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
		public new int BorderWidth
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
