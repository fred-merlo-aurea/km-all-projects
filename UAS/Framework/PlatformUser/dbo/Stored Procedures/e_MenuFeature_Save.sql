CREATE PROCEDURE [dbo].[e_MenuFeature_Save]
@MenuFeatureID int,
@FeatureName varchar(50),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @MenuFeatureID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE MenuFeature
		SET 
			FeatureName = @FeatureName,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE MenuFeatureID = @MenuFeatureID;
		
		SELECT @MenuFeatureID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO MenuFeature (FeatureName,IsActive,DateCreated,CreatedByUserID)
		VALUES(@FeatureName,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
