CREATE PROCEDURE [dbo].[e_FieldMapping_Delete_MappingBySourceFile]
@SourceFileID int
AS
BEGIN

	set nocount on

	DELETE FROM FieldMapping
	WHERE SourceFileID = @SourceFileID;
	Select 1

END