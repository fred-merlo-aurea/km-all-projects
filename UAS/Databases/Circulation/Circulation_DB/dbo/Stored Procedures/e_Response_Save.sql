CREATE PROCEDURE e_Response_Save
@ResponseID int,
@ResponseTypeID int,
@PublicationID int,
@ResponseName varchar(250),
@ResponseCode nchar(10),
@DisplayName varchar(250),
@DisplayOrder int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsOther BIT
AS

IF @ResponseID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Response
		SET 
			ResponseTypeID = @ResponseTypeID,
			PublicationID = @PublicationID,
			ResponseName = @ResponseName,
			ResponseCode = @ResponseCode,
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			IsOther = @IsOther
		WHERE ResponseID = @ResponseID;
		
		SELECT @ResponseID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Response (ResponseTypeID,PublicationID,ResponseName,ResponseCode,DisplayName,DisplayOrder,IsActive,DateCreated,CreatedByUserID,IsOther)
		VALUES(@ResponseTypeID,@PublicationID,@ResponseName,@ResponseCode,@DisplayName,@DisplayOrder,@IsActive,@DateCreated,@CreatedByUserID,@IsOther);SELECT @@IDENTITY;
	END
