CREATE  PROCEDURE [dbo].[e_FilterSchedule_Select_BySchedule]   
(
@dt date,
@time time
)
AS
BEGIN

	SET NOCOUNT ON

	Declare @dayofweek int,
			@lastdayofmonth int

	set @lastdayofmonth = DAY(DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@dt)+1,0)))
	set @dayofweek = datepart(dw, @dt)

	--select @lastdayofmonth, @dayofweek

	select rt.Type as RecurrenceType, fs.*, f.Name as FilterName
	from FilterSchedule fs with(nolock) 
		left outer join RecurrenceType rt with(nolock) on fs.RecurrenceTypeID = rt.RecurrenceTypeID 
		left outer join Filters f on f.FilterID = fs.filterID
	where fs.IsDeleted = 0  and 
		f.IsDeleted = 0 and
		StartTime = @time and
		(
			(IsRecurring = 0 and StartDate = @dt) --onetime
			or
			(
				IsRecurring = 1 and rt.Type = 'Daily' and (StartDate is null or StartDate <= @dt) and (EndDate is null or EndDate >= @dt ) -- Daily
			)
			or
			(
				IsRecurring = 1 and rt.Type = 'weekly' and (StartDate is null or StartDate <= @dt) and (EndDate is null or EndDate >= @dt ) -- weekly
				and 
				( 
					(@dayofweek=1 and RunSunday=1) or 
					(@dayofweek=2 and RunMonday=1) or 
					(@dayofweek=3 and RunTuesday=1) or 
					(@dayofweek=4 and RunWednesday=1) or 
					(@dayofweek=5 and RunThursday=1) or 
					(@dayofweek=6 and RunFriday=1) or 
					(@dayofweek=7 and RunSaturday=1)
				)
			)
			or
			(
				IsRecurring = 1 and rt.Type = 'Monthly' and (StartDate is null or StartDate <= @dt) and (EndDate is null or EndDate >= @dt ) -- Monthly
				and 
				( 
					(DAY(@dt) = MonthScheduleDay) or
					(DAY(@dt)= @lastdayofmonth and MonthLastDay=1)
				)
			)
		)
End