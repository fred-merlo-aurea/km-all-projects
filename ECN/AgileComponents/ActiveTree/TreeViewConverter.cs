using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="TreeViewConverter"/> object.
	/// </summary>
	internal class TreeViewConverter : TypeConverter
	{
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			return base.CanConvertFrom(context,sourceType);
		}

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
		{
			if ((destinationType == typeof(string)) ||
				(destinationType == typeof(InstanceDescriptor)))
				return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(
			ITypeDescriptorContext context, CultureInfo culture, Object value)
		{
			return base.ConvertFrom(context, culture, value);
		}
	}
}
