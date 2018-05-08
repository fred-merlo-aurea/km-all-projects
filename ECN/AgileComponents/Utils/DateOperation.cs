using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="DateOperation"/> object.
	/// </summary>
	public class DateOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DateOperation"/> class.
		/// </summary>
		public DateOperation()
		{
			
		}

		/// <summary>
		/// Gets the day position.
		/// </summary>
		/// <param name="firstDay">The first day.</param>
		/// <param name="firstDayOfWeekCalendar">The first day of week calendar.</param>
		/// <returns></returns>
		public static int GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek firstDay, System.Web.UI.WebControls.FirstDayOfWeek firstDayOfWeekCalendar)
		{
			int firstDayOfWeek = firstDayOfWeekCalendar == System.Web.UI.WebControls.FirstDayOfWeek.Default ? 0 : (int)firstDayOfWeekCalendar;
			int firstDay2 = firstDay == System.Web.UI.WebControls.FirstDayOfWeek.Default ? 0 : (int)firstDay;

			// Adjust with the first day of the week
			if (firstDay2 - firstDayOfWeek >= 0)
				return firstDay2 - firstDayOfWeek;
			else
				return 7 - firstDayOfWeek + firstDay2;
		}

		/// <summary>
		/// Formats the date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public static string FormatedDate(DateTime date, DateFormatLocale format)
		{
			string day = date.Day.ToString();
			string month = date.Month.ToString();
			string year = date.Year.ToString();
			string formatedDate = "";				
	
			switch(format)
			{
				case DateFormatLocale.en:
				{
					formatedDate = string.Format("{0}/{1}/{2}",month,day,year);
				} break;
						
				case DateFormatLocale.de:
				{
					if (Int32.Parse(day) < 10) day = "0" + day;
					if (Int32.Parse(month) < 10) month = "0" + month;
					formatedDate = string.Format("{0}.{1}.{2}",day,month,year);
				} break;

				case DateFormatLocale.en_GB:
				case DateFormatLocale.fr:
				case DateFormatLocale.it:
				case DateFormatLocale.es:
				{
					if (Int32.Parse(day) < 10) day = "0" + day;
					if (Int32.Parse(month) < 10) month = "0" + month;
					formatedDate = string.Format("{0}/{1}/{2}",day,month,year);
				} break;

				case DateFormatLocale.pt:
				{
					if (Int32.Parse(day) < 10) day = "0" + day;
					if (Int32.Parse(month) < 10) month = "0" + month;
					formatedDate = string.Format("{0}-{1}-{2}",day,month,year);
				} break;

				default : break;
			}

			return formatedDate;
		}

		/// <summary>
		/// Gets the week number.
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <returns></returns>
		public static int GetWeekNumber(DateTime dt)
		{
			// Set Year
			int yyyy=dt.Year;

			// Set Month
			int mm=dt.Month;
        
			// Set Day
			int dd=dt.Day;

			// Declare other required variables
			int DayOfYearNumber;
			int Jan1WeekDay;
			int WeekNumber=0, WeekDay;

    
			int i,j,k,l,m,n;
			int[] Mnth = new int[12] {0,31,59,90,120,151,181,212,243,273,304,334};

			int YearNumber;

        
			// Set DayofYear Number for yyyy mm dd
			DayOfYearNumber = dd + Mnth[mm-1];

			// Increase of Dayof Year Number by 1, if year is leapyear and month is february
			if ((IsLeapYear(yyyy) == true) && (mm == 2))
				DayOfYearNumber += 1;

			// Find the Jan1WeekDay for year 
			i = (yyyy - 1) % 100;
			j = (yyyy - 1) - i;
			k = i + i/4;
			Jan1WeekDay = 1 + (((((j / 100) % 4) * 5) + k) % 7);

			// Calcuate the WeekDay for the given date
			l= DayOfYearNumber + (Jan1WeekDay - 1);
			WeekDay = 1 + ((l - 1) % 7);

			// Find if the date falls in YearNumber set WeekNumber to 52 or 53
			if ((DayOfYearNumber <= (8 - Jan1WeekDay)) && (Jan1WeekDay > 4))
			{
				YearNumber = yyyy - 1;
				if ((Jan1WeekDay == 5) || ((Jan1WeekDay == 6) && (Jan1WeekDay > 4)))
					WeekNumber = 53;
				else
					WeekNumber = 52;
			}
			else
				YearNumber = yyyy;
        

			// Set WeekNumber to 1 to 53 if date falls in YearNumber
			if (YearNumber == yyyy)
			{
				if (IsLeapYear(yyyy)==true)
					m = 366;
				else
					m = 365;
				if ((m - DayOfYearNumber) < (4-WeekDay))
				{
					YearNumber = yyyy + 1;
					WeekNumber = 1;
				}
			}
        
			if (YearNumber==yyyy) 
			{
				n=DayOfYearNumber + (7 - WeekDay) + (Jan1WeekDay -1);
				WeekNumber = n / 7;
				if (Jan1WeekDay > 4)
					WeekNumber -= 1;
			}

			return (WeekNumber);
		}

		private static bool IsLeapYear(int yyyy) 
		{ 
			if ((yyyy % 4 == 0 && yyyy % 100 != 0) || (yyyy % 400 == 0))
				return true;
			else
				return false;
		}

	    internal static string FormatCustomDate(DateTime date, string format, string[] days, string[] months)
	    {
	        if (string.IsNullOrEmpty(format))
	        {
	            return date.ToShortDateString();
	        }

	        format = format.ToLower();

	        format = ReplaceYearInFormat(date, format);
	        format = ReplaceDayInFormat(date, format, days);
	        format = ReplaceMonthInFormat(date, format, months);

	        return format;
	    }

	    private static string ReplaceDayInFormat(DateTime date, string format, string[] days)
	    {
	        var start = format.IndexOf("d", 0, StringComparison.Ordinal);
	        var length = 1;
	        if (start != -1)
	        {
	            while (start + length < format.Length && format[start + length] == 'd')
	            {
	                length++;
	            }

	            format = format.Remove(start, length);

	            switch (length)
	            {
	                case 1:
	                {
	                    return format.Insert(start, date.Day.ToString());
	                }
	                case 2:
	                {
	                    return format.Insert(start, $"{date:dd}");
	                }
	                case 3:
	                {
	                    if (days[(int) date.DayOfWeek].Length >= 3)
	                    {
	                        return format.Insert(start, $"{days[(int) date.DayOfWeek].Substring(0, 3)}");
	                    }
	                    else
	                    {
	                        return format.Insert(start, $"{days[(int) date.DayOfWeek]}");
	                    }
	                }
	                case 4:
	                {
	                    return format.Insert(start, days[(int) date.DayOfWeek]);
	                }
	                default:
	                {
	                    return format.Insert(start, $"{date:dd}");
	                }
	            }
	        }

	        return format;
	    }

	    private static string ReplaceMonthInFormat(DateTime date, string format, string[] months)
	    {
	        var start = format.IndexOf("m", 0, StringComparison.Ordinal);
	        var length = 1;
	        if (start != -1)
	        {
	            while (start + length < format.Length && format[start + length] == 'm')
	            {
	                length++;
	            }

	            format = format.Remove(start, length);

	            switch (length)
	            {
	                case 1:
	                {
	                    return format.Insert(start, date.Month.ToString());
	                }
	                case 2:
	                {
	                    return format.Insert(start, $"{date:MM}");
	                }
	                case 3:
	                {
	                    if (months[date.Month - 1].Length >= 3)
	                    {
	                        return format.Insert(start, months[date.Month - 1].Substring(0, 3));
	                    }
	                    else
	                    {
	                        return format.Insert(start, months[date.Month - 1]);
	                    }
	                }
	                case 4:
	                {
	                    return format.Insert(start, months[date.Month - 1]);
	                }
	                default:
	                {
	                    return format.Insert(start, $"{date:MM}");
	                }
	            }
	        }

	        return format;
	    }

	    private static string ReplaceYearInFormat(DateTime date, string format)
	    {
	        var start = format.IndexOf("y", 0, StringComparison.Ordinal);
	        var length = 1;
	        if (start != -1)
	        {
	            while (start + length < format.Length && format[start + length] == 'y')
	            {
	                length++;
	            }

	            format = format.Remove(start, length);

	            switch (length)
	            {
	                case 1:
	                case 2:
	                {
	                    return format.Insert(start, $"{date:yy}");
	                }
	                default:
	                {
	                    return format.Insert(start, $"{date:yyyy}");
	                }
	            }
	        }

	        return format;
	    }
	}
}
