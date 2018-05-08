CREATE PROCEDURE e_DataMapping_Save
@DataMappingID int,
@FieldMapping int,
@IncomingValue varchar(100),
@MAFValue varchar(100),
@Ignore bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @DataMappingID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE DataMapping
			SET FieldMapping = @FieldMapping,
				IncomingValue = @IncomingValue,
				MAFValue = @MAFValue,
				Ignore = @Ignore,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE DataMappingID = @DataMappingID;
		
			SELECT @DataMappingID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO DataMapping (FieldMapping,IncomingValue,MAFValue,Ignore,DateCreated,CreatedByUserID)
			VALUES(@FieldMapping,@IncomingValue,@MAFValue,@Ignore,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END