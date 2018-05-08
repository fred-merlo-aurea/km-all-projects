using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	internal class SizeIconConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
			if (sourceType == typeof(System.String))
				return true;

			return base.CanConvertFrom(context,sourceType);
		}

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
		{
			if (destinationType == typeof(System.String) || destinationType == typeof(InstanceDescriptor))
				return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
				return new SizeIcon();

			if (value is System.String)
			{
                string s = (string)value;

				if (s.Length == 0)
					return new SizeIcon();

				string[] parts = s.Split(culture.TextInfo.ListSeparator[0]);

				if (parts.Length != 2)
				{
					throw new ArgumentException("Invalid SizeIcon","value");
				}

				TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(Int32));
				return new SizeIcon((int)intConverter.ConvertFromString(context,culture,parts[0]),
									(int)intConverter.ConvertFromString(context,culture,parts[1]));

			}

			return base.ConvertFrom(context,culture,value);
		}

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
		{
			if (value != null)
			{
				if (!(value is SizeIcon))
					throw new ArgumentException("Invalid SizeIcon","value");
			}

			if (destinationType == typeof(System.String))
			{
				if (value == null)
					return String.Empty;

				SizeIcon sizeIcon = (SizeIcon)value;

				TypeConverter intConverter = 
					TypeDescriptor.GetConverter(typeof(Int32));
				return String.Join(culture.TextInfo.ListSeparator,new String[] {
																				   intConverter.ConvertToString(context,culture, sizeIcon.Width),
																				   intConverter.ConvertToString(context,culture, sizeIcon.Heigth)
																			   }
					);
			}

			else if (destinationType == typeof(InstanceDescriptor))
			{
				if (value == null)
					return null;

				MemberInfo mi = null;
				object[] args = null;

				SizeIcon sizeIcon = (SizeIcon)value;
				
				Type intType = typeof(int);
				mi = typeof(SizeIcon).GetConstructor(new Type[] {intType,intType});
				args = new object[] {sizeIcon.Width, sizeIcon.Heigth};

				if (mi != null)
					return new InstanceDescriptor(mi,args);
				else return null;
			}

			return base.ConvertTo(context,culture,value,destinationType);
		}
	}
}
