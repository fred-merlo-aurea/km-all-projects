CREATE PROCEDURE [e_FieldMapping_Save]
@FieldMappingID int,
@FieldMappingTypeID int,
@IsNonFileColumn bit,
@SourceFileID int,
@IncomingField varchar(50),
@MAFField varchar(50),
@PubNumber int,
@DataType varchar(50),
@PreviewData varchar(1000),
@ColumnOrder int,
@HasMultiMapping bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@DemographicUpdateCodeId int
AS
BEGIN

	set nocount on

	IF @FieldMappingID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
                  
		UPDATE FieldMapping
		SET 
				SourceFileID = @SourceFileID,
				FieldMappingTypeID = @FieldMappingTypeID,
				IsNonFileColumn = @IsNonFileColumn,
				IncomingField = @IncomingField,
				MAFField = @MAFField,
				PubNumber = @PubNumber,
				DataType = @DataType,
				PreviewData = @PreviewData,
				ColumnOrder = @ColumnOrder,
				HasMultiMapping = @HasMultiMapping,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DemographicUpdateCodeId = @DemographicUpdateCodeId
		WHERE FieldMappingID = @FieldMappingID;

		SELECT @FieldMappingID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
		INSERT INTO FieldMapping (FieldMappingTypeID, IsNonFileColumn, SourceFileID, IncomingField, MAFField, PubNumber, DataType, PreviewData, ColumnOrder, HasMultiMapping, DateCreated, CreatedByUserID, DemographicUpdateCodeId)
			VALUES(@FieldMappingTypeID, @IsNonFileColumn, @SourceFileID, @IncomingField, @MAFField, @PubNumber, @DataType, @PreviewData, @ColumnOrder, @HasMultiMapping, @DateCreated, @CreatedByUserID, @DemographicUpdateCodeId);SELECT @@IDENTITY;   
		END

END