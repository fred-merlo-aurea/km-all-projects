CREATE PROCEDURE [dbo].[e_ApplicationLog_Select_ApplicationId_DateRange]
@ApplicationId int,
@StartDate date,
@EndDate date
AS
	select *
	from ApplicationLog with(nolock)
	where ApplicationId = @ApplicationId
	and LogAddedDate between @StartDate and @EndDate
	order by LogAddedDate, LogAddedTime
go