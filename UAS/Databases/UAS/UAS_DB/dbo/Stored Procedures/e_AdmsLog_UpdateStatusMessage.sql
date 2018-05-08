create procedure e_AdmsLog_UpdateStatusMessage
@ProcessCode varchar(50),
@StatusMessage varchar(max),
@UpdatedByUserID int = 1
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = @StatusMessage,
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate()
		where ProcessCode = @ProcessCode
	end
go
