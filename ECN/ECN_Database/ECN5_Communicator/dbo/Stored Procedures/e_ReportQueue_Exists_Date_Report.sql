CREATE PROCEDURE [dbo].[e_ReportQueue_Exists_Date_Report]
	@ReportScheduleID int,
	@ReportID int,
	@SendTime datetime
AS
	if exists(SELECT top 1 ReportQueueID from ReportQueue rq with(nolock) where rq.ReportID = @ReportID and rq.ReportScheduleID = @ReportScheduleID and rq.SendTime = @SendTime and rq.Status in ('pending', 'sent'))
		SELECT 1
	ELSE
		SELECT 0
