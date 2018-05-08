create procedure e_AdmsLog_UpdateFinalCounts
@ProcessCode varchar(50),
@UpdatedByUserID int = 1,
@FinalRecordCount int,
@FinalProfileCount int,
@FinalDemoCount int,
@MatchedRecordCount int,
@UadConsensusCount int,
@ArchiveCount int
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
			DimensionProfileCount += @ArchiveCount
		where ProcessCode = @ProcessCode
	end
go
