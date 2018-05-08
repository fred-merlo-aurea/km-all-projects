using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Web;

namespace ActiveUp.WebControls
{
    #region class ToolMenuComponentEditor

	/// <summary>
	/// Represents a <see cref="MenuComponentEditor"/> object.
	/// </summary>
    internal class MenuComponentEditor : WindowsFormsComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
        {
            ActiveUp.WebControls.Menu toolMenu = component as ActiveUp.WebControls.Menu;
            if (toolMenu == null)
                throw new ArgumentException("Component must be a ActiveUp.WebControls.Menu object.", "component");

            IServiceProvider site = toolMenu.Site;
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
                            changeService.OnComponentChanging(toolMenu, null);
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
                    MenuPropertyBuilderForm form = new MenuPropertyBuilderForm(toolMenu);
                    if (form.ShowDialog(owner) == DialogResult.OK)
                    {
                        changed = true;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Unable to create the ToolMenu : " + ex.ToString());
                }

                finally
                {
                    if (changed && changeService != null)
                    {
                        changeService.OnComponentChanged(toolMenu, null, null, null);
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
