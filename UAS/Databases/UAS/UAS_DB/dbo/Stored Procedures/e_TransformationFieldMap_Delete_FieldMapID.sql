CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Delete_FieldMapID]
@FieldMappingID int
AS
BEGIN

	set nocount on

	DELETE FROM TransformationFieldMap
	WHERE FieldMappingID = @FieldMappingID
	Select @FieldMappingID;

END