CREATE PROCEDURE e_QualificationSource_Save
@QSourceID int,
@QSourceTypeID int,
@QSourceName varchar(100),
@QSourceCode nchar(10),
@DisplayName varchar(250),
@DisplayOrder int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @QSourceID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE QualificationSource
		SET 
			QSourceTypeID = @QSourceTypeID,
			QSourceName = @QSourceName,
			QSourceCode = @QSourceCode,
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE QSourceID = @QSourceID;
		
		SELECT @QSourceID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO QualificationSource (QSourceTypeID,QSourceName,QSourceCode,DisplayName,DisplayOrder,IsActive,DateCreated,CreatedByUserID)
		VALUES(@QSourceTypeID,@QSourceName,@QSourceCode,@DisplayName,@DisplayOrder,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
