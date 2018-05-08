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
	
	internal class IconConverter : TypeConverter
	{

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				if (((Icon)value).FileName == null)
					return value.ToString();
				else
					return ((Icon)value).FileName;
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

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is System.String)
			{
				if (System.IO.File.Exists(value.ToString()))
					return new ActiveUp.WebControls.Icon(value.ToString());
				else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(value.ToString())))
					return new ActiveUp.WebControls.Icon(System.Web.HttpContext.Current.Server.MapPath(value.ToString()));
				else 
					return new ActiveUp.WebControls.Icon();

			}
			else
				return base.ConvertFrom(context, culture, value);
	
		}	

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(typeof(ActiveUp.WebControls.Icon), attributes);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

	}
}
