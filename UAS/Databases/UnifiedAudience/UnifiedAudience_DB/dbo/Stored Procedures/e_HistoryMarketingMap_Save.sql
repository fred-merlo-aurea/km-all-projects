CREATE PROCEDURE e_HistoryMarketingMap_Save
@HistoryMarketingMapID int,
@MarketingID int,
@PubSubscriptionID int,
@PublicationID int,
@IsActive bit,
@DateCreated datetime,
@CreatedByUserID  int
AS
BEGIN

	SET NOCOUNT ON

	IF @HistoryMarketingMapID > 0
		BEGIN
			UPDATE HistoryMarketingMap
			SET 
				MarketingID = @MarketingID,
				PubSubscriptionID = @PubSubscriptionID,
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
			INSERT INTO HistoryMarketingMap (MarketingID,PubSubscriptionID,PublicationID,IsActive,DateCreated,CreatedByUserID)
			VALUES(@MarketingID,@PubSubscriptionID,@PublicationID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END