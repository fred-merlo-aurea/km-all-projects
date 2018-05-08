CREATE PROCEDURE e_IssueSplit_SaveNew
	@IssueSplitId int,
	@IssueId int,
	@IssueSplitCode varchar(250),
	@IssueSplitName varchar(250),
	@IssueSplitCount int,
	@FilterId int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,
	@IsActive bit,
	@KeyCode varchar(250),
	@IssueSplitRecords int,
	@IssueSplitDescription varchar(250),
    @TVP_IssueSplitArchivePubSubscriptionMap IdListType READONLY 
AS
BEGIN
	SET NOCOUNT ON
	IF @IssueSplitId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE IssueSplit
			SET IssueId = @IssueId,
				IssueSplitCode = @IssueSplitCode,
				IssueSplitName = @IssueSplitName,
				IssueSplitCount = @IssueSplitCount,
				FilterId = @FilterId,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				IsActive = @IsActive,
				KeyCode = @KeyCode,
				IssueSplitRecords = @IssueSplitRecords,
				IssueSplitDescription = @IssueSplitDescription
			WHERE IssueSplitId = @IssueSplitId;
			
			DELETE FROM IssueSplitArchivePubSubscriptionMap WHERE IssueSplitId = @IssueSplitId

			INSERT INTO IssueSplitArchivePubSubscriptionMap(IssueSplitPubSubscriptionId,IssueSplitId) 
            SELECT Id,@IssueSplitId
		    FROM  @TVP_IssueSplitArchivePubSubscriptionMap
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			
			INSERT INTO IssueSplit (IssueId,IssueSplitCode,IssueSplitName,IssueSplitCount,FilterId,DateCreated,CreatedByUserID, IsActive, KeyCode, IssueSplitRecords, IssueSplitDescription)
			VALUES(@IssueId,@IssueSplitCode,@IssueSplitName,@IssueSplitCount,@FilterId,@DateCreated,@CreatedByUserID, @IsActive, @KeyCode, @IssueSplitRecords, @IssueSplitDescription);
			
			SET @IssueSplitId = @@IDENTITY ;
			
			INSERT INTO IssueSplitArchivePubSubscriptionMap(IssueSplitPubSubscriptionId,IssueSplitId) 
            SELECT Id,@IssueSplitId
		    FROM  @TVP_IssueSplitArchivePubSubscriptionMap
		END	
	SELECT @IssueSplitId;
END  
