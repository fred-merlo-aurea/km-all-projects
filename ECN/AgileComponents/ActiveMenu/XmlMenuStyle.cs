using System;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	#region XmlMenuStyle

	/// <summary>
	/// Represents a <see cref="XmlMenuStyle"/> object.
	/// </summary>
	[
	System.Xml.Serialization.XmlTypeAttribute(),
	Serializable
	]
	public class XmlMenuStyle : XmlMenuStyleBase
	{
		#region Fields

		int _cellPadding;
		int _cellSpacing;

		#endregion
	
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMenuStyle"/> class.
		/// </summary>
		public XmlMenuStyle()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets or sets the cell padding.
		/// </summary>
		/// <value>The cell padding.</value>
		[
		System.Xml.Serialization.XmlElement("cellpadding",DataType="int"),
		Browsable(true)
		]
		public int CellPadding
		{
			get
			{
				return _cellPadding;
			}

			set
			{
				_cellPadding = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell spacing.
		/// </summary>
		/// <value>The cell spacing.</value>
		[
		System.Xml.Serialization.XmlElement("cellspacing",DataType="int"),
		Browsable(true)
		]
		public int CellSpacing
		{
			get
			{
				return _cellSpacing;
			}

			set
			{
				_cellSpacing = value;
			}
		}

		#endregion

	}

	#endregion
}
