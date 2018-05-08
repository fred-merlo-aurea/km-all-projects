
CREATE PROCEDURE e_MarketingMap_Save
@MarketingID int,
@SubscriberID int,
@PublicationID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF NOT EXISTS(SELECT MarketingID FROM MarketingMap With(NoLock) WHERE MarketingID = @MarketingID AND SubscriberID = @SubscriberID AND PublicationID = @PublicationID)
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO MarketingMap (MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@MarketingID,@SubscriberID,@PublicationID,@IsActive,@DateCreated,@CreatedByUserID);
	END
ELSE
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE MarketingMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		 WHERE MarketingID = @MarketingID AND SubscriberID = @SubscriberID AND PublicationID = @PublicationID;
	END

	DELETE FROM MarketingMap WHERE SubscriberID = @SubscriberID AND IsActive = 0
