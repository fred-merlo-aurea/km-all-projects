create procedure e_FileProcessingStat_Select_DateRange
@StartDate date,
@EndDate date
as
BEGIN

	set nocount on

	select *
	from FileProcessingStat with(nolock)
	where ProcessDate between @StartDate and @EndDate

END
go