create procedure e_AdmsLog_UpdateTransformedCounts
@ProcessCode varchar(50),
@UpdatedByUserID int = 1,
@TransRecordCount int,
@TransProfileCount int,
@TransDemoCount int
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = 'Update AdmsLog Transformed Counts',
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate(),
			TransformedRecordCount = @TransRecordCount,
			TransformedProfileCount = @TransProfileCount,
			TransformedDemoCount = @TransDemoCount
		where ProcessCode = @ProcessCode
	end
go
