CREATE PROCEDURE [dbo].[e_ClientGroupServiceFeatureMap_Save]
@ClientGroupServiceFeatureMapID int,
@ClientGroupID int,
@ServiceID int,
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

IF	@ClientGroupServiceFeatureMapID > 0 
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ClientGroupServiceFeatureMap
		SET IsEnabled = @IsEnabled,
			Rate = @Rate,
			RateDurationInMonths = @RateDurationInMonths,
			RateStartDate = @RateStartDate,
			RateExpireDate = @RateExpireDate,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientGroupServiceFeatureMapID = @ClientGroupServiceFeatureMapID

		SELECT @ClientGroupServiceFeatureMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ClientGroupServiceFeatureMap (ClientGroupID,ServiceID,ServiceFeatureID,IsEnabled,Rate,RateDurationInMonths,RateStartDate,RateExpireDate,DateCreated,CreatedByUserID)
		VALUES(@ClientGroupID,@ServiceID,@ServiceFeatureID,@IsEnabled,@Rate,@RateDurationInMonths,@RateStartDate,@RateExpireDate,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
