CREATE PROCEDURE [dbo].[e_ReportQueue_Select_ReportScheduleID]
	@ReportScheduleID int
AS
	SELECT * 
	FROM ReportQueue rq with(nolock)
	where rq.ReportScheduleID = @ReportScheduleID
RETURN 0
