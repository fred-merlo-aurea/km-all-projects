using System;

namespace ecn.common.classes {
	public class DateTimeInterpreter {
		public static DateTime MaxValue = new DateTime(2318, 1, 1);
		public static DateTime MinValue = new DateTime(1980, 1, 1);

		public static string InterpretActionDate(DateTime actionDate) {
			if (actionDate == MaxValue) {
				return "N/A";
			}

			return actionDate.ToString();
		}

		public static string InterpretEndDate(DateTime endDate) {
			if (endDate == MaxValue) {
				return "No end";
			}

			return endDate.ToShortDateString();
		}
	}
}
