CREATE PROCEDURE [dbo].[e_ClientServiceMap_Save]
@ClientServiceMapID int,
@ClientID int,
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

IF @ClientServiceMapID > 0
	BEGIN
		SET @DateUpdated = GETDATE();			
		UPDATE ClientServiceMap
		SET IsEnabled       = @IsEnabled,
			Rate = @Rate,
			RateDurationInMonths = @RateDurationInMonths,
			RateStartDate = @RateStartDate,
			RateExpireDate = @RateExpireDate,
			DateUpdated     = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientServiceMapID = @ClientServiceMapID

		SELECT @ClientServiceMapID;
	END
ELSE
	BEGIN
		SET @DateCreated = GETDATE();
		INSERT INTO ClientServiceMap (ClientID,ServiceID,IsEnabled,Rate,RateDurationInMonths,RateStartDate,RateExpireDate,DateCreated,CreatedByUserID)
		VALUES(@ClientID,@ServiceID,@IsEnabled,@Rate,@RateDurationInMonths,@RateStartDate,@RateExpireDate,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END