CREATE PROCEDURE [dbo].[e_Service_Save]
@ServiceID int,
@ServiceName varchar(100),
@Description varchar(500),
@ServiceCode varchar(5),
@DisplayOrder int,
@IsEnabled bit,
@IsAdditionalCost bit,
@HasFeatures bit,
@DefaultRate decimal(14,2),
@DefaultDurationInMonths int,
@DefaultApplicationID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ServiceID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Service
		SET ServiceName = @ServiceName,
			Description = @Description,
			ServiceCode = @ServiceCode,
			DisplayOrder = @DisplayOrder,
			IsEnabled = @IsEnabled,
			IsAdditionalCost = @IsAdditionalCost,
			HasFeatures = @HasFeatures,
			DefaultRate = @DefaultRate,
			DefaultDurationInMonths = @DefaultDurationInMonths,
			DefaultApplicationID = @DefaultApplicationID,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ServiceID = @ServiceID

		SELECT @ServiceID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Service (ServiceName,Description,ServiceCode,DisplayOrder,IsEnabled,IsAdditionalCost,HasFeatures,DefaultRate,DefaultDurationInMonths,DefaultApplicationID,DateCreated,CreatedByUserID)
		VALUES(@ServiceName,@Description,@ServiceCode,@DisplayOrder,@IsEnabled,@IsAdditionalCost,@HasFeatures,@DefaultRate,@DefaultDurationInMonths,@DefaultApplicationID,@DateCreated,@CreatedByUserID);Select @@IDENTITY;
	END
