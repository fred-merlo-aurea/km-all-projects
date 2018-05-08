CREATE PROCEDURE [dbo].[e_DataCompareRun_Save]
@DcRunId int,
@ClientId int,
@SourceFileId int,
@FileRecordCount int = 0,
@MatchedRecordCount int = 0,
@UadConsensusCount int = 0,
@ProcessCode varchar(50) = '',
@DateCreated datetime,
@IsBillable bit = 'true',
@Notes varchar(max) = null
as
	begin
		set nocount on

		if @DcRunId > 0
			begin
			
				UPDATE DataCompareRun
				SET FileRecordCount = @FileRecordCount,
					MatchedRecordCount = @MatchedRecordCount,
					UadConsensusCount = @UadConsensusCount,
					IsBillable = @IsBillable,
					Notes = @Notes		  
				WHERE DcRunId = @DcRunId;
		
				SELECT @DcRunId;
			END
		ELSE
			BEGIN
				IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
				INSERT INTO DataCompareRun (ClientId,SourceFileId,FileRecordCount,MatchedRecordCount,UadConsensusCount,ProcessCode,DateCreated,IsBillable,Notes)
				VALUES(@ClientId,@SourceFileId,@FileRecordCount,@MatchedRecordCount,@UadConsensusCount,@ProcessCode,@DateCreated,@IsBillable,@Notes);SELECT @@IDENTITY;
			END
	end
go