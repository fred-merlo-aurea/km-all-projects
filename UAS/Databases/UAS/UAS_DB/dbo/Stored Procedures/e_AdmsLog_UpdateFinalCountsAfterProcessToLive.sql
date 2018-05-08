CREATE PROCEDURE e_AdmsLog_UpdateFinalCountsAfterProcessToLive
	@ProcessCode varchar(50),
	@UpdatedByUserID int = 1,
	@FinalRecordCount int = 0,
	@FinalProfileCount int = 0,
	@FinalDemoCount int = 0,
	@MatchedRecordCount int = 0,
	@UadConsensusCount int = 0,
	@IgnoredRecordCount int = 0,
	@IgnoredProfileCount int = 0,
	@IgnoredDemoCount int = 0
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = 'Update AdmsLog Final Counts',
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate(),
			FinalRecordCount = @FinalRecordCount,
			FinalProfileCount = @FinalProfileCount,
			FinalDemoCount = @FinalDemoCount,
			MatchedRecordCount = @MatchedRecordCount,
			UadConsensusCount = @UadConsensusCount,
			IgnoredRecordCount = @IgnoredRecordCount,
			IgnoredProfileCount = @IgnoredProfileCount,
			IgnoredDemoCount = @IgnoredDemoCount
		where ProcessCode = @ProcessCode
	end
go
