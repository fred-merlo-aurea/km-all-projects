create procedure e_AdmsLog_UpdateFileEnd
@ProcessCode varchar(50),
@UpdatedByUserId int = 1,
@FileEnd datetime
as
	begin
		set nocount on
		update AdmsLog
		set FileEnd = @FileEnd,
			DateUpdated = getdate(),
			UpdatedByUserID = @UpdatedByUserId
		where ProcessCode = @ProcessCode
	end
go
