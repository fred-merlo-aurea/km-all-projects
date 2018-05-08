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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Converter used to convert string array to other types. 
	/// </summary>
	public class StringArrayConverter : TypeConverter
	{

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
		/// <param name="culture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current culture is assumed. </param>
		/// <param name="value">The <see cref="Object"/> to convert. </param>
		/// <param name="destinationType">The Type to convert the value parameter to. </param>
		/// <returns>An Object that represents the converted value.	</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				string result = "";
				for (int i = 0 ; i < ((string[])value).Length ; i++)
				{
					if (i != 0) result += ",";
					result += ((string[])value)[i];
				}
				return result;
			}
			if (destinationType != typeof(byte[]))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return new byte[0]; 
			}

			return new byte[0];
		}

		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
		{

			if (sourceType == typeof(System.String))
			{
				return true;
			}

			else
			{
				return base.CanConvertFrom(context, sourceType);
			}
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">The CultureInfo to use as the current culture.</param>
		/// <param name="value">The Object to convert. </param>
		/// <returns>An Object that represents the converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is System.String)
			{
				return ((string)value).Split(',');
			}
			else
				return base.ConvertFrom(context, culture, value);

		}	

		/// <summary>
		/// Returns whether this object supports properties, using the specified context.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <returns>true if GetProperties should be called to find the properties of this object; otherwise, false.</returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		
	}
}
