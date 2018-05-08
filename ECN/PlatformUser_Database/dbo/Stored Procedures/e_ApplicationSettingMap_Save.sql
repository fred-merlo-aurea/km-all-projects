CREATE PROCEDURE [dbo].[e_ApplicationSettingMap_Save]
@ApplicationSettingMapID int,
@ApplicationID int,
@ApplicationSettingID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ApplicationSettingMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ApplicationSettingMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ApplicationSettingMapID = @ApplicationSettingMapID;

	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ApplicationSettingMap (ApplicationID,ApplicationSettingID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@ApplicationID,@ApplicationSettingID,@IsActive,@DateCreated,@CreatedByUserID);
	END
