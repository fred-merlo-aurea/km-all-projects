CREATE PROCEDURE [dbo].[e_FieldMultiMap_Save]
	@FieldMultiMapID int,
	@FieldMappingID int,
	@FieldMappingTypeID int,
	@MAFField varchar(50),
	@DataType varchar(50),
	@PreviewData varchar(1000),
	@ColumnOrder int,
	@DateCreated DateTime,
	@DateUpdated DateTime,
	@CreatedByUserID int, 
	@UpdatedByUserID int 
AS
BEGIN

	set nocount on

	IF @FieldMultiMapID > 0
		BEGIN
			IF @DateUpdated IS NULL
					BEGIN
						SET @DateUpdated = GETDATE();
					END
                  
		UPDATE FieldMultiMap
		SET 
			FieldMappingID = @FieldMappingID,
			FieldMappingTypeID = @FieldMappingTypeID,
			MAFField = @MAFField,
			DataType = @DataType,
			PreviewData = @PreviewData,
			ColumnOrder = @ColumnOrder,	
			DateUpdated = @DateUpdated,		 
			UpdatedByUserID = @UpdatedByUserID
		WHERE FieldMultiMapID = @FieldMultiMapID;

		SELECT @FieldMultiMapID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
		INSERT INTO FieldMultiMap (FieldMappingID,FieldMappingTypeID,MAFField,DataType,PreviewData,ColumnOrder,DateCreated,CreatedByUserID)
			VALUES(@FieldMappingID,@FieldMappingTypeID,@MAFField,@DataType,@PreviewData,@ColumnOrder,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;   
		END	 
		
END	  