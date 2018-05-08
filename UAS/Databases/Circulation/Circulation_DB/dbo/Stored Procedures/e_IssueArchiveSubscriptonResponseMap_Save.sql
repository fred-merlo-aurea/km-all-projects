CREATE PROCEDURE e_IssueArchiveSubscriptonResponseMap_Save
@IssueArchiveSubscriptionId int,
@SubscriptionID  int,
@ResponseID int,
@IsActive bit,
@ResponseOther varchar(300),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueArchiveSubscriptionId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueArchiveSubscriptonResponseMap
		SET SubscriptionID = @SubscriptionID,
			ResponseID = @ResponseID,
			IsActive = @IsActive,
			ResponseOther = @ResponseOther,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueArchiveSubscriptionId = @IssueArchiveSubscriptionId;

		SELECT @IssueArchiveSubscriptionId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueArchiveSubscriptonResponseMap (SubscriptionID,ResponseID,IsActive,ResponseOther,DateCreated,CreatedByUserID)
		VALUES(@SubscriptionID,@ResponseID,@IsActive,@ResponseOther,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
