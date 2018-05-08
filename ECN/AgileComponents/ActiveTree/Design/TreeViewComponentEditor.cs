using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Web;

namespace ActiveUp.WebControls.Design
{
	#region class TreeViewComponentEditor

	/// <summary>
	/// Represents a <see cref="TreeViewComponentEditor"/> object.
	/// </summary>
	internal class TreeViewComponentEditor : WindowsFormsComponentEditor
	{
		public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
		{
			ActiveUp.WebControls.TreeView treeView = component as ActiveUp.WebControls.TreeView;
			if (treeView == null)
				throw new ArgumentException("Component must be a ActiveUp.ActiveTreeView.TreeView object.","component");

			IServiceProvider site = treeView.Site;
			IComponentChangeService changeService = null;

			DesignerTransaction transaction = null;
			bool changed = false;

			try 
			{
				if (site != null) 
				{
					IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
					transaction = designerHost.CreateTransaction("Property Builder");

					changeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
					if (changeService != null) 
					{
						try 
						{
							changeService.OnComponentChanging(treeView, null);
						}
						catch (CheckoutException ex) 
						{
							if (ex == CheckoutException.Canceled)
								return false;
							throw ex;
						}
					}
				}

				try 
				{
					PropertyBuilderForm form = new PropertyBuilderForm(treeView);
					if (form.ShowDialog(owner) == DialogResult.OK) 
					{
						changed = true;
					}
				}
				
				catch(Exception ex)
				{
					MessageBox.Show("ERROR : " + ex.ToString());
				}

				finally 
				{
					if (changed && changeService != null) 
					{
						changeService.OnComponentChanged(treeView, null, null, null);
					}
				}
			}
			finally 
			{
				if (transaction != null) 
				{
					if (changed) 
					{
						transaction.Commit();
					}
					else 
					{
						transaction.Cancel();
					}
				}
			}

			return changed;
		}
	}

	#endregion
}
