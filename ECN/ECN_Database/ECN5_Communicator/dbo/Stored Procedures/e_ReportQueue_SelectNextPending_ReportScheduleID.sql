CREATE PROCEDURE [dbo].[e_ReportQueue_SelectNextPending_ReportScheduleID]
	@ReportScheduleID int
AS
	Select top 1 * 
	from ReportQueue rq with(nolock)
	where rq.ReportScheduleID = @ReportScheduleID and rq.status = 'pending'

