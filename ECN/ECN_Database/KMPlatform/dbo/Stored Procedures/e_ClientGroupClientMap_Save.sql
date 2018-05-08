CREATE PROCEDURE [dbo].[e_ClientGroupClientMap_Save]
@ClientGroupClientMapID int,
@ClientGroupID int,
@ClientID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ClientGroupClientMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ClientGroupClientMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientGroupClientMapID = @ClientGroupClientMapID;

		SELECT @ClientGroupClientMapID;

	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ClientGroupClientMap (ClientGroupID,ClientID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@ClientGroupID,@ClientID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END