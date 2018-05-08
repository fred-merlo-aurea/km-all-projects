CREATE PROCEDURE [dbo].[e_MenuSecurityGroupMap_Save]
@MenuSecurityGroupMapID int,
@SecurityGroupID int,
@MenuID int,
@HasAccess bit,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @MenuSecurityGroupMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE MenuSecurityGroupMap
		SET 
			HasAccess = @HasAccess,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE MenuSecurityGroupMapID = @MenuSecurityGroupMapID;
		
		SELECT @MenuSecurityGroupMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO MenuSecurityGroupMap (SecurityGroupID,MenuID,HasAccess,IsActive,DateCreated,CreatedByUserID)
		VALUES(@SecurityGroupID,@MenuID,@HasAccess,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
