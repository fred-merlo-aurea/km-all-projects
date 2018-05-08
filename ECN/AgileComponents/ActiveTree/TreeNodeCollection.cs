using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a collection of nodes.
	/// </summary>
	public class TreeNodeCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TreeNodeCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Add a node in the collection specifying  the node.
		/// </summary>
		/// <param name="treeNode">Node to add.</param>
		public void Add(TreeNode treeNode)
		{
			List.Add(treeNode);
		}

		/// <summary>
		/// Add a node in the collection specifying the text, link, target and the unique key.
		/// </summary>
		/// <param name="text">Text displayed.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target of the link.</param>
		/// <param name="key">Unique key of the node.</param>
		public void Add(string text, string link, string target, string key)
		{
			TreeNode newNode = new TreeNode();
			newNode.Key = key;
			newNode.Target = target;
			newNode.Text = text;
			newNode.Link = link;
			List.Add(newNode);
		}

		/// <summary>
		/// Remove a node at the specified index position.
		/// </summary>
		/// <param name="index">Index of the node to delete.</param>
		public void Remove(int index)
		{
			if (index < Count && index >= 0)
				List.RemoveAt(index); 
		}

		/// <summary>
		/// Remove a node whith the specified key.
		/// </summary>
		/// <param name="key">Unique key identify the node.</param>
		public void Remove(string key)
		{
			for (int index=0;index<this.Count;index++)
			{
				if (this[index].Key == key)
					List.RemoveAt(index);
			}
		}


		/// <summary>
		/// Get a node at the specified index.
		/// </summary>
		public TreeNode this[int index]
		{
			get
			{
				return (TreeNode) List[index];
			}
		}

		/// <summary>
		/// Indicates if the collection contains a node with the specified key.
		/// </summary>
		/// <param name="key">Key to find.</param>
		/// <returns>True if the collection contains the node with the specified key, otherwise, false.</returns>
		public bool Contains(string key)
		{
			foreach (TreeNode treeNode in this)
			{
				if (treeNode.Key == key)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Allows the developer to add a collection of Nodes objects in another one.
		/// </summary>
		/// <param name="first">The first collection.</param>
		/// <param name="second">The second collection.</param>
		/// <returns>The concacened collection.</returns>
		public static TreeNodeCollection operator +(TreeNodeCollection first, TreeNodeCollection second) 
		{
			TreeNodeCollection newNodes = first;
			foreach(TreeNode node in second)
				newNodes.Add(node);

			return newNodes;
		}

		/// <summary>
		/// Render the collection.
		/// </summary>
		/// <param name="output">Write to the output.</param>
		public void Render(System.Web.UI.HtmlTextWriter output)
		{
			foreach (TreeNode treeNode in this)
			{
				treeNode.RenderControl(output);
			}
		}
	}
}
