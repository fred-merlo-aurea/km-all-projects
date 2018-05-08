#if !FX1_1
#define NOT_FX1_1
#endif

using System.Diagnostics;
using System.ComponentModel;

namespace ActiveUp.WebControls.Common
{
	internal static class Fx1ConditionalHelper<T>
	{
		internal static T Value { get; private set; }

		[Conditional("NOT_FX1_1")]
		private static void SetNotFx1ConditionalValue(T notFx1Value, T fx1Value)
		{
			Value = notFx1Value;
		}

		[Conditional("FX1_1")]
		private static void SetFx1ConditionalValue(T notFx1Value, T fx1Value)
		{
			Value = fx1Value;
		}

		internal static T GetFx1ConditionalValue(T notFx1Value, T fx1Value)
		{
			SetNotFx1ConditionalValue(notFx1Value, fx1Value);
			SetFx1ConditionalValue(notFx1Value, fx1Value);
			return Value;
		}
	}

	internal class Fx1ConditionalDefaultValueAttribute : DefaultValueAttribute
	{
		public Fx1ConditionalDefaultValueAttribute(object notFx1Value, object fx1Value) 
			: base(notFx1Value)
		{
			SetFx1Value(fx1Value);
		}

		[Conditional("FX1_1")]
		private void SetFx1Value(object fx1Value)
		{
			base.SetValue(fx1Value);
		}
	}
}
