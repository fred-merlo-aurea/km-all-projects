using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Web;

namespace ActiveUp.WebControls.Design
{
	#region class EditorComponentEditor

	internal class EditorComponentEditor : WindowsFormsComponentEditor
	{
		public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
		{
			ActiveUp.WebControls.Editor editor = component as ActiveUp.WebControls.Editor;
			if (editor == null)
				throw new ArgumentException("Component must be a ActiveUp.WebControls.Editor object.","component");

			IServiceProvider site = editor.Site;
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
							changeService.OnComponentChanging(editor, null);
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
					EditorPropertyBuilderForm form = new EditorPropertyBuilderForm(editor);
					if (form.ShowDialog(owner) == DialogResult.OK) 
					{
						changed = true;
					}
				}
				
				catch(Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}

				finally 
				{
					if (changed && changeService != null) 
					{
						changeService.OnComponentChanged(editor, null, null, null);
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
