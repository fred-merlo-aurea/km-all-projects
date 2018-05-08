using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.IO;

namespace ActiveUp.WebControls.Design
{
	#region class TreeViewLabelDesigner

	internal class TreeViewControlDesigner : ControlDesigner
	{
		private DesignerVerbCollection designerVerbs;

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				if (designerVerbs == null) 
				{
					designerVerbs = new DesignerVerbCollection();
					designerVerbs.Add(new DesignerVerb("Property Builder...", new EventHandler(this.OnPropertyBuilder)));
				}

				return designerVerbs;
			}
		}

		private void OnPropertyBuilder(object sender, EventArgs e) 
		{
			TreeViewComponentEditor compEditor = new TreeViewComponentEditor();
			compEditor.EditComponent(Component);
		}

		public override void Initialize(IComponent component) 
		{
			if (!(component is ActiveUp.WebControls.TreeView)) 
			{
				throw new ArgumentException("Component must be a ActiveUp.WebControls.TreeView control.", "component");
			}
			base.Initialize(component);
		}

		/// <summary>
		/// Gets the design time HTML code.
		/// </summary>	
		/// <returns>A string containing the HTML to render.</returns>
		public override string GetDesignTimeHtml()
		{
			TreeView treeView = (TreeView)base.Component;

			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

			treeView.RenderControl(writer);

            return stringWriter.ToString();
		}
	}

	#endregion
}
