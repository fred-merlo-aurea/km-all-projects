using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace ActiveUp.WebControls
{
	#region class ToolCollectionEditor

	/// <summary>
	/// Provides a user interface that can edit most types of the <see cref="ToolCollection"/> at design time.
	/// </summary>
	[Serializable]
	public class ToolCollectionEditor : CollectionEditor 
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCollectionEditor"/> class using the specified collection type.
		/// </summary>
		/// <param name="type">The type of the collection for this editor to edit.</param>
		public ToolCollectionEditor(Type type) : base(type) 
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the data types that this <see cref="ToolCollectionEditor"/> can contain.
		/// </summary>
		/// <returns>An array of data types that this collection can contain.</returns>
		protected override System.Type[] CreateNewItemTypes() 
		{
            System.Type[] types = new System.Type[] {
										  typeof(ToolButton),
										  typeof(ToolButtonMenu),
										  typeof(ToolCheckBox),
										  typeof(ToolDropDownList),
										  typeof(ToolRadioButton),
										  typeof(ToolSpacer),
										  typeof(ToolTextBox),
										  typeof(ToolColorPicker)
									  };
			return types;           
		}

		#endregion
	}

	#endregion
}