
CREATE PROCEDURE [dbo].[e_Par3c_Save]
@Par3CID int,
@DisplayName varchar(250),
@DisplayOrder int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @Par3CID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Par3c
		SET 
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE Par3CID = @Par3CID;
		
		SELECT @Par3CID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Par3c (DisplayName,DisplayOrder,IsActive,DateCreated,CreatedByUserID)
		VALUES(@DisplayName,@DisplayOrder,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END

