CREATE PROCEDURE [dbo].[e_ReportQueue_Save]
	@ReportQueueID int = null,
	@ReportScheduleID int,
	@ReportID int,
	@SendTime datetime,
	@Status varchar(50)
AS
if(@ReportQueueID is null)
	BEGIN
		INSERT INTO ReportQueue(ReportID, ReportScheduleID, SendTime, Status)
		VALUES(@ReportID, @ReportScheduleID, @SendTime, @Status)
		Select @@IDENTITY;
	END
ELSE
	BEGIN
		if(@Status = 'Sent')
		BEGIN
			UPDATE ReportQueue
			SET Status = @Status, FinishTime = GETDATE()
			WHERE ReportQueueId = @ReportQueueID
			SELECT @ReportQueueID
		END
		ELSE
		BEGIN
			UPDATE ReportQueue
			SET Status = @Status, SendTime = @SendTime
			WHERE ReportQueueId = @ReportQueueID
			SELECT @ReportQueueID
		END
	END