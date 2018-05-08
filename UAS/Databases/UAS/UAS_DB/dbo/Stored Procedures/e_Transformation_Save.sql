CREATE PROCEDURE [e_Transformation_Save]
@TransformationId int,
@TransformationTypeId int,
@TransformationName varchar(100),
@TransformationDescription varchar(750),
@MapsPubCode bit = 'false',
@LastStepDataMap bit = 'false',
@ClientId int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsTemplate bit = 'true'
AS
BEGIN

	set nocount on

	IF @TransformationId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
		
			UPDATE Transformation
			SET TransformationTypeID = @TransformationTypeId,
				  TransformationName = @TransformationName,
				  TransformationDescription = @TransformationDescription, 
				  MapsPubCode = @MapsPubCode,
				  LastStepDataMap = @LastStepDataMap,                                  
				  ClientID = @ClientId,
				  IsActive = @IsActive,
				  DateUpdated = @DateUpdated,
				  UpdatedByUserID = @UpdatedByUserID,
				  IsTemplate = @IsTemplate
			WHERE TransformationID = @TransformationId;

			SELECT @TransformationId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Transformation (TransformationTypeID, TransformationName, TransformationDescription, MapsPubCode,LastStepDataMap, ClientID, IsActive, DateCreated, CreatedByUserID, IsTemplate)
			VALUES(@TransformationTypeId, @TransformationName, @TransformationDescription, @MapsPubCode,@LastStepDataMap, @ClientId, @IsActive, @DateCreated, @CreatedByUserID, @IsTemplate);SELECT @@IDENTITY;
		END

END