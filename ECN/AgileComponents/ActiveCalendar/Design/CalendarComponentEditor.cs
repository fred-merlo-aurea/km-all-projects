// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Web;

namespace ActiveUp.WebControls.Design
{
	#region class CalendarComponentEditor

	/// <summary>
	/// Represents a <see cref="CalendarComponentEditor"/> object.
	/// </summary>
	internal class CalendarComponentEditor : WindowsFormsComponentEditor
	{
		public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
		{
			ActiveUp.WebControls.Calendar calendar = component as ActiveUp.WebControls.Calendar;
			if (calendar == null)
				throw new ArgumentException("Component must be a ActiveUp.WebControls.ActiveCalendar object.","component");

			IServiceProvider site = calendar.Site;
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
							changeService.OnComponentChanging(calendar, null);
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
					CalendarPropertyBuilderForm form = new CalendarPropertyBuilderForm(calendar,changeService);
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
						changeService.OnComponentChanged(calendar, null, null, null);
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
