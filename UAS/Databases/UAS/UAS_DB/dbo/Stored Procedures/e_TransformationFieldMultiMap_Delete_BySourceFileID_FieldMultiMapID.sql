CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_Delete_BySourceFileID_FieldMultiMapID]
	@SourceFileID int,
	@FieldMultiMapID int
AS
BEGIN

	set nocount on

	DELETE TransformationFieldMultiMap WHERE SourceFileID = @SourceFileID and FieldMultiMapID = @FieldMultiMapID

END