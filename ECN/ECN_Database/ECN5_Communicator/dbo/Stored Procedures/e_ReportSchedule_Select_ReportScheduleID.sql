CREATE  PROC dbo.e_ReportSchedule_Select_ReportScheduleID
(
	@ReportScheduleID int
) 
AS 
BEGIN
	select * from  ReportSchedule with (NOLOCK)
	where ReportScheduleID= @ReportScheduleID
	and IsDeleted=0
END