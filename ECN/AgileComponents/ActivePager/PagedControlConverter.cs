// ActivePager
// Copyright (c) 2002 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This class is used at design time to select only valid data sources to page.
	/// </summary>
	/// <remarks>You should not use this class in your project.</remarks>
	internal class PagedControlConverter : StringConverter
	{

		/// <summary>
		/// Gets the valid controls.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns>A collection of valid controls.</returns>
		private object[] GetControls(IContainer container)
		{
			ComponentCollection componentCollection = container.Components;
			ArrayList arrayList = new ArrayList();
			IEnumerator iEnumerator = componentCollection.GetEnumerator();
			try
			{
				while (iEnumerator.MoveNext())
				{
					IComponent iComponent = (IComponent)iEnumerator.Current;
					if (iComponent is Control)
					{
						Control control = (Control)iComponent;
						if (control.ID != null && control.ID.Length != 0)
						{
							/*ValidationPropertyAttribute validationPropertyAttribute = (ValidationPropertyAttribute)TypeDescriptor.GetAttributes(control)[typeof(ValidationPropertyAttribute)];
							if (validationPropertyAttribute != null && validationPropertyAttribute.Name != null)
							{
								arrayList.Add(String.Copy(control.ID));
							}*/
							if (control.ToString() == "System.Web.UI.WebControls.DataGrid")
								arrayList.Add(String.Copy(control.ID));
						}
					}
				}
			}
			finally
			{
				/*IDisposable iDisposable = (IDisposable)iEnumerator;
				if (iDisposable != null)
				{
					iDisposable.Dispose();
				}*/
			}
			arrayList.Sort();
			return arrayList.ToArray();
		}

		/// <summary>
		/// Returns a standard value collection.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>A standard value collection.</returns>
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context == null || context.Container == null)
			{
				return null;
			}
			object[] locals = GetControls(context.Container);
			if (locals != null)
			{
				return new TypeConverter.StandardValuesCollection(locals);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Get standard values.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>A boolean value.</returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>
		/// Get standard values.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>A boolean value.</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}

}
