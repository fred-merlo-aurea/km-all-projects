CREATE PROCEDURE [dbo].[e_AdmsLog_UpdateDimensionCounts]
	@ProcessCode varchar(50),
	@UpdatedByUserID int = 1,	
	@DimensionCount int,
	@DimensionSubscriberCount int
as
	begin
		set nocount on
		
		update AdmsLog
		set StatusMessage = 'Update AdmsLog Final Counts',
            UpdatedByUserID = @UpdatedByUserID,
            DateUpdated = GetDate(),
			DimensionRecordCount = @DimensionCount,
			DimensionProfileCount = @DimensionSubscriberCount			
		where ProcessCode = @ProcessCode
	end
go
