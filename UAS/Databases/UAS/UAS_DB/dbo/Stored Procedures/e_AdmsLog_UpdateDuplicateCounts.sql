create procedure e_AdmsLog_UpdateDuplicateCounts
@ProcessCode varchar(50),
@UpdatedByUserID int = 1,
@DupRecordCount int,
@DupProfileCount int,
@DupDemoCount int
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = 'Update AdmsLog Duplicate Counts',
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate(),
			DuplicateRecordCount = @DupRecordCount,
			DuplicateProfileCount = @DupProfileCount,
			DuplicateDemoCount = @DupDemoCount
		where ProcessCode = @ProcessCode
	end
go
