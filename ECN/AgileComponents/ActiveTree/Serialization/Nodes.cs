using System;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using System.IO;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// A collection of nodes.
	/// </summary>
	[System.Xml.Serialization.XmlRootAttribute("activetreeview", IsNullable=false)]
	public class Nodes
	{
		#region Variables

		/// <summary>
		/// List of nodes.
		/// </summary>
		private ArrayList _nodes = new ArrayList();	

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Nodes()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the list of nodes
		/// </summary>
		[System.Xml.Serialization.XmlArray("nodes")]
		[System.Xml.Serialization.XmlArrayItem("node",typeof(Node))]
		public ArrayList NodesList
		{
			get
			{
				return _nodes;
			}

			set
			{
				_nodes = value;
			}
		}

		#endregion
	}
}
