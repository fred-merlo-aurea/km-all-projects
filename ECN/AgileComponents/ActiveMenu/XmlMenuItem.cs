using System;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	#region XmlMenuItem

	/// <summary>
	/// Represents a <see cref="XmlMenuItem"/> object.
	/// </summary>
	[
	System.Xml.Serialization.XmlTypeAttribute("xmltoolmenuitem")
	]
	public class XmlMenuItem
	{
		#region Fields

		private string _id;
		private string _text;
		private string _image;
		private string _imageOver;
		private HorizontalAlign _align;
		private int _width;
		private int _height;
		private string _navigateURL;
		private string _target;
		private string _onClickClient;
		private XmlSubMenu _subMenu = new XmlSubMenu();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMenuItem"/> class.
		/// </summary>
		public XmlMenuItem()
		{
			
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		[
		System.Xml.Serialization.XmlElement("id",DataType="string"),
		Browsable(true)
		]
		public string Id
		{
			get {return _id;}
			set {_id = value;}
		}

			
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		[
			System.Xml.Serialization.XmlElement("text",DataType="string"),
			Browsable(true)
		]
		public string Text
		{
			get {return _text;}
			set {_text = value;}
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		[
		System.Xml.Serialization.XmlElement("image",DataType="string"),
		Browsable(true)
		]
		public string Image
		{
			get {return _image;}
			set {_image = value;}
		}

		/// <summary>
		/// Gets or sets the image over.
		/// </summary>
		/// <value>The image over.</value>
		[
		System.Xml.Serialization.XmlElement("imageover",DataType="string"),
		Browsable(true)
		]
		public string ImageOver
		{
			get {return _imageOver;}
			set {_imageOver = value;}
		}

		/// <summary>
		/// Gets or sets the align.
		/// </summary>
		/// <value>The align.</value>
		[
		System.Xml.Serialization.XmlElement("align",typeof(HorizontalAlign)),
		Browsable(true)
		]
		public HorizontalAlign Align
		{
			get {return _align;}
			set {_align = value;}
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		[
		System.Xml.Serialization.XmlElement("width",DataType="int"),
		Browsable(true)
		]
		public int Width
		{
			get {return _width;}
			set {_width = value;}
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		[
		System.Xml.Serialization.XmlElement("height",DataType="int"),
		Browsable(true)
		]
		public int Height
		{
			get {return _height;}
			set {_height = value;}
		}

		/// <summary>
		/// Gets or sets the navigate URL.
		/// </summary>
		/// <value>The navigate URL.</value>
		[
		System.Xml.Serialization.XmlElement("navigateurl",DataType="string"),
		Browsable(true)
		]
		public string NavigateURL
		{
			get {return _navigateURL;}
			set {_navigateURL = value;}
		}

		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		[
		System.Xml.Serialization.XmlElement("target",DataType="string"),
		Browsable(true)
		]
		public string Target
		{
			get {return _target;}
			set {_target = value;}
		}

		/// <summary>
		/// Gets or sets the on click client.
		/// </summary>
		/// <value>The on click client.</value>
		[
		System.Xml.Serialization.XmlElement("onclickclient",DataType="string"),
		Browsable(true)
		]
		public string OnClickClient
		{
			get {return _onClickClient;}
			set {_onClickClient = value;}
		}

		/// <summary>
		/// Gets or sets the sub menu.
		/// </summary>
		/// <value>The sub menu.</value>
		[
		System.Xml.Serialization.XmlElement("submenu",typeof(XmlSubMenu)),
		Browsable(true)
		]
		public XmlSubMenu SubMenu
		{
			get {return _subMenu;}
			set {_subMenu = value;}
		}

        #endregion

	}

	#endregion
}
