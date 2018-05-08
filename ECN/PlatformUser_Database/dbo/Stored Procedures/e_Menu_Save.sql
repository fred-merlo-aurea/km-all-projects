CREATE PROCEDURE [dbo].[e_Menu_Save]
@MenuID int,
@ApplicationID int,
@IsServiceFeature bit = 'false',
@ServiceFeatureID int = 0,
@MenuName varchar(50),
@Description varchar(500),
@IsParent bit,
@ParentMenuID int,
@URL varchar(250),
@IsActive bit,
@MenuOrder int,
@HasFeatures bit,
@ImagePath varchar(250),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @MenuID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Menu
		SET ApplicationID = @ApplicationID,
			IsServiceFeature = @IsServiceFeature,
			ServiceFeatureID = @ServiceFeatureID,
			MenuName = @MenuName,
			Description = @Description,
			IsParent = @IsParent,
			ParentMenuID = @ParentMenuID,
			URL = @URL,
			IsActive = @IsActive,
			MenuOrder = @MenuOrder,
			HasFeatures = @HasFeatures,
			ImagePath = @ImagePath,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE MenuID = @MenuID;
		
		SELECT @MenuID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Menu (ApplicationID,IsServiceFeature,ServiceFeatureID,MenuName,Description,IsParent,ParentMenuID,URL,IsActive,MenuOrder,HasFeatures,ImagePath,DateCreated,CreatedByUserID)
		VALUES(@ApplicationID,@IsServiceFeature,@ServiceFeatureID,@MenuName,@Description,@IsParent,@ParentMenuID,@URL,@IsActive,@MenuOrder,@HasFeatures,@ImagePath,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END