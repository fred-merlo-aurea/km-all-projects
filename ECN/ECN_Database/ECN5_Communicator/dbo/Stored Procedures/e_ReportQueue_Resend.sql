CREATE PROCEDURE [dbo].[e_ReportQueue_Resend]
	@ReportQueueID int
AS
	UPDATE ReportQueue
	set Status = 'Pending'
	where ReportQueueID = @ReportQueueID