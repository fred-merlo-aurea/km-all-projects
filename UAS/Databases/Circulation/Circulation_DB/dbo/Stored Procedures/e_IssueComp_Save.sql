CREATE PROCEDURE e_IssueComp_Save
@IssueCompId int,
@IssueId int,
@ImportedDate datetime,
@IssueCompCount int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsActive bit
AS
IF @IssueCompId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueComp
		SET IssueId = @IssueId,
			ImportedDate = @ImportedDate,
			IssueCompCount = @IssueCompCount,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			IsActive = @IsActive
		WHERE IssueCompId = @IssueCompId;

		SELECT @IssueCompId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueComp (IssueCompId,IssueId,ImportedDate,IssueCompCount,DateCreated,CreatedByUserID,IsActive)
		VALUES(@IssueCompId,@IssueId,@ImportedDate,@IssueCompCount,@DateCreated,@CreatedByUserID,@IsActive);SELECT @@IDENTITY;
	END
GO