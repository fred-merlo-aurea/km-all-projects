CREATE PROCEDURE [dbo].[e_ReportQueue_Delete_ReportScheduleID]
	@ReportScheduleID int
AS
	UPDATE ReportQueue
	SET Status = 'Deleted'
	where ReportScheduleID = @ReportScheduleID and Status not in('Sent','Failed')