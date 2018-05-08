CREATE PROCEDURE [dbo].[e_ClientGroupSecurityGroupMap_Save]
@ClientGroupSecurityGroupMapID int,
@ClientGroupID int,
@SecurityGroupID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ClientGroupSecurityGroupMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ClientGroupSecurityGroupMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientGroupSecurityGroupMapID = @ClientGroupSecurityGroupMapID;

		SELECT @ClientGroupSecurityGroupMapID;

	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ClientGroupSecurityGroupMap (ClientGroupID,SecurityGroupID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@ClientGroupID,@SecurityGroupID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END