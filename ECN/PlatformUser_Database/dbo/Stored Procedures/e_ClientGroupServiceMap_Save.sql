CREATE PROCEDURE [dbo].[e_ClientGroupServiceMap_Save]
@ClientGroupServiceMapID int,
@ClientGroupID int,
@ServiceID int,
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

IF @ClientGroupServiceMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ClientGroupServiceMap
		SET IsEnabled = @IsEnabled,
			Rate = @Rate,
			RateDurationInMonths = @RateDurationInMonths,
			RateStartDate = @RateStartDate,
			RateExpireDate = @RateExpireDate,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientGroupServiceMapID = @ClientGroupServiceMapID

		SELECT @ClientGroupServiceMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ClientGroupServiceMap (ClientGroupID,ServiceID,IsEnabled,Rate,RateDurationInMonths,RateStartDate,RateExpireDate,DateCreated,CreatedByUserID)
		VALUES(@ClientGroupID,@ServiceID,@IsEnabled,@Rate,@RateDurationInMonths,@RateStartDate,@RateExpireDate,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO


