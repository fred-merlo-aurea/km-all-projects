CREATE PROCEDURE e_ResponseType_Save
@ResponeTypeID int,
@PublicationID int,
@ResponseTypeName varchar(100),
@DisplayName varchar(100),
@DisplayOrder int,
@IsMultipleValue bit,
@IsRequired bit,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ResponeTypeID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ResponseType
		SET 
			PublicationID = @PublicationID,
			ResponseTypeName = @ResponseTypeName,
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			IsMultipleValue = @IsMultipleValue,
			IsRequired = @IsRequired,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ResponseTypeID = @ResponeTypeID;
		
		SELECT @ResponeTypeID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ResponseType (PublicationID,ResponseTypeName,DisplayName,DisplayOrder,IsMultipleValue,IsRequired,IsActive,DateCreated,CreatedByUserID)
		VALUES(@PublicationID,@ResponseTypeName,@DisplayName,@DisplayOrder,@IsMultipleValue,@IsRequired,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
