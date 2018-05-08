CREATE PROCEDURE [dbo].[e_MenuFeatureSecurityGroupMap_Save]
@MenuFeatureSecurityGroupMapID int,
@MenuFeatureID int,
@SecurityGroupID int,
@AccessID bit,
@HasAccess bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @MenuFeatureSecurityGroupMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE MenuFeatureSecurityGroupMap
		SET 
			HasAccess = @HasAccess,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE MenuFeatureSecurityGroupMapID = @MenuFeatureSecurityGroupMapID;

		SELECT @MenuFeatureSecurityGroupMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO MenuFeatureSecurityGroupMap (MenuFeatureID,SecurityGroupID,AccessID,HasAccess,DateCreated,CreatedByUserID)
		VALUES(@MenuFeatureID,@SecurityGroupID,@AccessID,@HasAccess,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO