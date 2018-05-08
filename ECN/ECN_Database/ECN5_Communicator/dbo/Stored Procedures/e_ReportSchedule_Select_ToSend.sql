CREATE PROCEDURE [dbo].[e_ReportSchedule_Select_ToSend] 
	@timeToSend Datetime
AS
BEGIN

	SET NOCOUNT ON;

	
    SELECT * 
    FROM 
		ReportSchedule rs with(nolock)
    WHERE 
		CONVERT(date,rs.StartDate) <= Convert(date,@timeToSend) and 
		Convert(date,case len(rs.EndDate) when 0 then GetDate() when 10 then rs.EndDate end) >= Convert(date,@timeToSend) and 
		Convert(time,rs.StartTime) = Convert(time,@timeToSend) and 
		rs.IsDeleted = 0 and
		(
			UPPER(rs.ScheduleType) = 'RECURRING' and
			(
				UPPER(rs.RecurrenceType) = 'DAILY' or
				UPPER(rs.RecurrenceType) = 'WEEKLY' and
				(
					(rs.RunSunday = 1 and datename(dw,getdate()) = 'SUNDAY') or
					(rs.RunMonday = 1 and datename(dw,getdate()) = 'MONDAY') or
					(rs.RunTuesday = 1 and datename(dw,getdate()) = 'TUESDAY') or
					(rs.RunWednesday = 1 and datename(dw,getdate()) = 'WEDNESDAY') or
					(rs.RunThursday = 1 and datename(dw,getdate()) = 'THURSDAY') or
					(rs.RunFriday = 1 and datename(dw,getdate()) = 'FRIDAY') or
					(rs.RunSaturday = 1 and datename(dw,getdate()) = 'SATURDAY')					
				) or
				UPPER(rs.RecurrenceType) = 'MONTHLY' and
				(
					DATEPART(day, GETDATE()) = ISNULL(rs.MonthScheduleDay, 0) or
					(rs.MonthLastDay = 1 and DATEPART(day, GETDATE()) = DATEPART(day, DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0))))
				)
			) or
			UPPER(rs.ScheduleType) = 'ONE-TIME'
			
		)
	ORDER BY
		ISNULL(rs.BlastID, 0) asc
END