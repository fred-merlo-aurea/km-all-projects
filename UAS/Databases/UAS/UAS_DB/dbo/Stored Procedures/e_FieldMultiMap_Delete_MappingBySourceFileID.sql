CREATE PROCEDURE [dbo].[e_FieldMultiMap_Delete_MappingBySourceFileID]
	@SourceFileID int
AS
BEGIN

	set nocount on

	DELETE FieldMultiMap
	WHERE FieldMappingID in 
	(
		SELECT FieldMappingID FROM FieldMapping WHERE SourceFileID = @SourceFileID
	)

END