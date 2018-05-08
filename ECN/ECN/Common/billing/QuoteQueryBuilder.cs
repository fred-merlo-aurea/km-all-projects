using System;

namespace ecn.common.classes.billing
{
	public class QuoteQueryBuilder
	{
		public static string GetWhereClause(QuoteStatusEnum status, CustomerTypeEnum ctype, DateTime start, DateTime end) {
			return GetStatusClause(status) + 
				GetCustomerTypeClause(ctype)+
				GetCreateDateClause(start, true)+
				GetCreateDateClause(end, false);
		}

		private static string GetStatusClause(QuoteStatusEnum status) 
		{
			if ((int) status == 3)
				return string.Format(" q.Status<>{0}", (int) status);
			else
				return string.Format(" q.Status={0}", (int) status);
		}

		private static string GetCustomerTypeClause(CustomerTypeEnum ctype) {
			if (ctype == CustomerTypeEnum.New) {
				return " AND q.CustomerID=-1";
			}

			if (ctype == CustomerTypeEnum.Existing) {
				return " AND q.CustomerID<>-1";
			}

			return string.Empty;
		}

		private static string GetCreateDateClause(DateTime date, bool isStartDate) {
            if (isStartDate && date == DateTimeInterpreter.MinValue)
            {
				return string.Empty;
			}

            if (!isStartDate && date == DateTimeInterpreter.MaxValue)
            {
				return string.Empty;
			}
			return string.Format(" AND q.CreatedDate{0}'{1}'", isStartDate?">":"<",  date.ToShortDateString()); 
		}
	}
}
