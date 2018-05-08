CREATE PROCEDURE [dbo].[e_ApplicationLog_Select_DateRange]
@StartDate date,
@EndDate date
AS
	select *
	from ApplicationLog with(nolock)
	where LogAddedDate between @StartDate and @EndDate
	order by ApplicationId,LogAddedDate, LogAddedTime