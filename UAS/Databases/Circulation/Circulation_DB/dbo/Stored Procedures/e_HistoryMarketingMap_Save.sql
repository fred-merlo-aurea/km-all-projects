CREATE PROCEDURE e_HistoryMarketingMap_Save
@HistoryMarketingMapID int,
@MarketingID int,
@SubscriberID int,
@PublicationID int,
@IsActive bit,
@DateCreated datetime,
@CreatedByUserID  int
AS

IF @HistoryMarketingMapID > 0
	BEGIN
		UPDATE HistoryMarketingMap
		SET 
			MarketingID = @MarketingID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			IsActive = @IsActive,
			DateCreated = @DateCreated,
			CreatedByUserID = @CreatedByUserID
		WHERE HistoryMarketingMapID = @HistoryMarketingMapID 
		
		SELECT @HistoryMarketingMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO HistoryMarketingMap (MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@MarketingID,@SubscriberID,@PublicationID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END

