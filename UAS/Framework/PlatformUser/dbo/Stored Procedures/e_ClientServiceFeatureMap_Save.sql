CREATE PROCEDURE [dbo].[e_ClientServiceFeatureMap_Save]
@ClientServiceFeatureMapID int,
@ClientID int,
@ServiceFeatureID int,
@IsEnabled bit,
@Rate decimal(14,2),
@RateDurationInMonths int,
@RateStartDate date,
@RateExpireDate date,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF	@ClientServiceFeatureMapID > 0 
	BEGIN
		SET @DateUpdated = GETDATE();
		UPDATE ClientServiceFeatureMap
		SET IsEnabled = @IsEnabled,
			Rate = @Rate,
			RateDurationInMonths = @RateDurationInMonths,
			RateStartDate = @RateStartDate,
			RateExpireDate = @RateExpireDate,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientServiceFeatureMapID = @ClientServiceFeatureMapID;
		SELECT @ClientServiceFeatureMapID;
	END
ELSE
	BEGIN
		SET @DateCreated = GETDATE();
		INSERT INTO ClientServiceFeatureMap (ClientID,ServiceFeatureID,IsEnabled,Rate,RateDurationInMonths,RateStartDate,RateExpireDate,DateCreated,CreatedByUserID)
		VALUES(@ClientID,@ServiceFeatureID,@IsEnabled,@Rate,@RateDurationInMonths,@RateStartDate,@RateExpireDate,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
