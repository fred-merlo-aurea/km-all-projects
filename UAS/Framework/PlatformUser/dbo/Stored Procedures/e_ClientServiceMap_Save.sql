CREATE PROCEDURE [dbo].[e_ClientServiceMap_Save]
@ClientServiceMapID int,
@ClientID int,
@ServiceID int,
@IsEnabled bit,
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
			DateUpdated     = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientServiceMapID = @ClientServiceMapID

		SELECT @ClientServiceMapID;
	END
ELSE
	BEGIN
		SET @DateCreated = GETDATE();
		INSERT INTO ClientServiceMap (ClientID,ServiceID,IsEnabled,DateCreated,CreatedByUserID)
		VALUES(@ClientID,@ServiceID,@IsEnabled,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
