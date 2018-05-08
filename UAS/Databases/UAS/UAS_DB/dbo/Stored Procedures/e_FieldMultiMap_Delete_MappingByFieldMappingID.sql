CREATE PROCEDURE [dbo].[e_FieldMultiMap_Delete_MappingByFieldMappingID]
	@FieldMappingID int
AS
BEGIN

	set nocount on

	DELETE FieldMultiMap WHERE FieldMappingID = @FieldMappingID

END