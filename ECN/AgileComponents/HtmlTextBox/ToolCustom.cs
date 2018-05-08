using System;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolCustom"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolCustom : ToolButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCustom"/> class.
		/// </summary>
		public ToolCustom() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCustom"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolCustom(string id) : base(id)
		{
			_Init(id);
		}
 
		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolCustom" + Editor.indexTools++;
			else
				this.ID = id;
			this.ToolTip = "Custom";
		}

	}
}
