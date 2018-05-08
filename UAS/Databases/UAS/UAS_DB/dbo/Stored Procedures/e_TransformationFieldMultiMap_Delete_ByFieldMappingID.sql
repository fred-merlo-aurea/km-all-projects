CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_Delete_ByFieldMappingID]
	@FieldMappingID int
AS
BEGIN

	set nocount on

	DELETE TransformationFieldMultiMap WHERE FieldMappingID = @FieldMappingID

END