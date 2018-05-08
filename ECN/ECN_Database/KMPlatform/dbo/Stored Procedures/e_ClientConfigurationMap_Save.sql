CREATE PROCEDURE [dbo].[e_ClientConfigurationMap_Save]
@ClientConfigurationMapId int,
@ClientID int,
@CodeTypeId int,
@CodeId int,
@ClientValue varchar(500),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as

	IF @ClientConfigurationMapId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
				
			UPDATE ClientConfigurationMap
			SET ClientID = @ClientID,
				CodeTypeId = @CodeTypeId,
				CodeId = @CodeId,
				ClientValue = @ClientValue,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE ClientConfigurationMapId = @ClientConfigurationMapId;
			
			SELECT @ClientConfigurationMapId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO ClientConfigurationMap (ClientID,CodeTypeId,CodeId,ClientValue,IsActive,DateCreated,CreatedByUserID)
			VALUES(@ClientID,@CodeTypeId,@CodeId,@ClientValue,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END