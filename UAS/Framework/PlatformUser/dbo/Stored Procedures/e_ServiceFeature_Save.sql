CREATE PROCEDURE [dbo].[e_ServiceFeature_Save]
@ServiceFeatureID int,
@ServiceID int,
@SFName varchar(100),
@Description varchar(500),
@SFCode varchar(5),
@DisplayOrder int,
@IsEnabled bit,
@IsAdditionalCost bit,
@DefaultRate decimal(14,2),
@DefaultDurationInMonths int,
@KMAdminOnly bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ServiceFeatureID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ServiceFeature
		SET SFName = @SFName,
			Description = @Description,
			SFCode = @SFCode,
			DisplayOrder = @DisplayOrder,
			IsEnabled = @IsEnabled,
			IsAdditionalCost = @IsAdditionalCost,
			DefaultRate = @DefaultRate,
			DefaultDurationInMonths = @DefaultDurationInMonths,
			KMAdminOnly = @KMAdminOnly,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ServiceFeatureID = @ServiceFeatureID

		SELECT @ServiceFeatureID
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ServiceFeature (ServiceID,SFName,Description,SFCode,DisplayOrder,IsEnabled,IsAdditionalCost,DefaultRate,DefaultDurationInMonths,KMAdminOnly,DateCreated,CreatedByUserID)
		VALUES(@ServiceID,@SFName,@Description,@SFCode,@DisplayOrder,@IsEnabled,@IsAdditionalCost,@DefaultRate,@DefaultDurationInMonths,@KMAdminOnly,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
