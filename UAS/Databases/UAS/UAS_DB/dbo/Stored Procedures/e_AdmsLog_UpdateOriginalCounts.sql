create procedure e_AdmsLog_UpdateOriginalCounts
@ProcessCode varchar(50),
@UpdatedByUserID int = 1,
@OrigRecordCount int,
@OrigProfileCount int,
@OrigDemoCount int
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = 'Update AdmsLog Original Counts',
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate(),
			OriginalRecordCount = @OrigRecordCount,
			OriginalProfileCount = @OrigProfileCount,
			OriginalDemoCount = @OrigDemoCount
		where ProcessCode = @ProcessCode
	end
go
