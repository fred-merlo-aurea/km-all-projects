CREATE PROCEDURE [dbo].[e_ApplicationSetting_Save]
@ApplicationSettingID int,
@AppSettingDescription varchar(250),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @ApplicationSettingID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ApplicationSetting
		SET IsActive = @IsActive,
			AppSettingDescription = @AppSettingDescription,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ApplicationSettingID = @ApplicationSettingID;
		
		SELECT @ApplicationSettingID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ApplicationSetting (AppSettingDescription,IsActive,DateCreated,CreatedByUserID)
		VALUES(@AppSettingDescription,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END