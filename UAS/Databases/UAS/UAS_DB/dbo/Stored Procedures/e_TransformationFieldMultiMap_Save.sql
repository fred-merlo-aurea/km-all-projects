CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_Save]
	@TransformationFieldMultiMapID int,
	@TransformationID int,
	@SourceFileID int,
	@FieldMappingID int,
	@FieldMultiMapID int,
	@IsActive bit,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @DateCreated IS NULL
		BEGIN
			SET @DateCreated = GETDATE();
		END
	INSERT INTO TransformationFieldMultiMap (TransformationID,SourceFileID,FieldMappingID,FieldMultiMapID,IsActive,DateCreated,CreatedByUserID)
	VALUES(@TransformationID,@SourceFileID,@FieldMappingID,@FieldMultiMapID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
END