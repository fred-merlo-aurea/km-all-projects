using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Web;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="BarcodeComponentEditor"/> object.
    /// </summary>
    internal class BarcodeComponentEditor : WindowsFormsComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
        {
            ActiveUp.WebControls.Barcode barcode = component as ActiveUp.WebControls.Barcode;
            if (barcode == null)
                throw new ArgumentException("Component must be a Barcodes.Barcode object.", "component");

            IServiceProvider site = barcode.Site;
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
                            changeService.OnComponentChanging(barcode, null);
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
                    ActiveUp.WebControls.BarcodePropertyBuilderForm form = new ActiveUp.WebControls.BarcodePropertyBuilderForm(barcode);
                    if (form.ShowDialog(owner) == DialogResult.OK)
                    {
                        changed = true;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("ERROR : " + ex.ToString());
                }

                finally
                {
                    if (changed && changeService != null)
                    {
                        changeService.OnComponentChanged(barcode, null, null, null);
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
}
