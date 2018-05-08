CREATE PROCEDURE e_IssueSplit_Save
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
@KeyCode varchar(250)
AS
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
			KeyCode = @KeyCode
		WHERE IssueSplitId = @IssueSplitId;

		SELECT @IssueSplitId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT intO IssueSplit (IssueId,IssueSplitCode,IssueSplitName,IssueSplitCount,FilterId,DateCreated,CreatedByUserID, IsActive, KeyCode)
		VALUES(@IssueId,@IssueSplitCode,@IssueSplitName,@IssueSplitCount,@FilterId,@DateCreated,@CreatedByUserID, @IsActive, @KeyCode);SELECT @@IDENTITY;
	END
GO
