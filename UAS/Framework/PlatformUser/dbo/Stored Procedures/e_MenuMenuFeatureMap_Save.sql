CREATE PROCEDURE [dbo].[e_MenuMenuFeatureMap_Save]
@MenuMenuFeatureMapID int,
@MenuID int,
@MenuFeatureID int,
@HasAccess bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @MenuMenuFeatureMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE MenuMenuFeatureMap
		SET 
			HasAccess = @HasAccess,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE MenuMenuFeatureMapID = @MenuMenuFeatureMapID;

		SELECT @MenuMenuFeatureMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO MenuMenuFeatureMap (MenuID,MenuFeatureID,HasAccess,DateCreated,CreatedByUserID)
		VALUES(@MenuID,@MenuFeatureID,@HasAccess,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
