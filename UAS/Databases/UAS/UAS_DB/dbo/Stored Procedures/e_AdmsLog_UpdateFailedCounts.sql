create procedure e_AdmsLog_UpdateFailedCounts
@ProcessCode varchar(50),
@UpdatedByUserID int = 1,
@FailRecordCount int,
@FailProfileCount int,
@FailDemoCount int
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = 'Update AdmsLog Failed Counts',
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate(),
			FailedRecordCount = @FailRecordCount,
			FailedProfileCount = @FailProfileCount,
			FailedDemoCount = @FailDemoCount
		where ProcessCode = @ProcessCode
	end
go
