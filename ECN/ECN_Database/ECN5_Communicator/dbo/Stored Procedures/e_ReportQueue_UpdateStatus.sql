CREATE PROCEDURE [dbo].[e_ReportQueue_UpdateStatus]
	@ReportQueueID int,
	@Status varchar(20),
	@FailureReason varchar(500)
AS
	if(@Status = 'Sent')
	BEGIN
		UPDATE ReportQueue
		SET Status = @Status, FinishTime = GETDATE(),FailureReason = @FailureReason
		WHERE ReportQueueID = @ReportQueueID
	END
	ELSE IF(@Status = 'Active')
	BEGIN
		UPDATE ReportQueue
		SET Status = @Status
		WHERE ReportQueueID = @ReportQueueID
	END
	ELSE
	BEGIN
		UPDATE ReportQueue
		SET Status = @Status, FinishTime = GETDATE(),FailureReason = @FailureReason
		WHERE ReportQueueID = @ReportQueueID
	END
