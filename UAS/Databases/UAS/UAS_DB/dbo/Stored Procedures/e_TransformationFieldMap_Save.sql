CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Save]
@TransformationFieldMapID int,
@TransformationID int,
@SourceFileID int,
@FieldMappingID int,
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
	INSERT INTO TransformationFieldMap (TransformationID, SourceFileID, FieldMappingID, IsActive, DateCreated, CreatedByUserID)
	VALUES(@TransformationID, @SourceFileID, @FieldMappingID, @IsActive, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;

END