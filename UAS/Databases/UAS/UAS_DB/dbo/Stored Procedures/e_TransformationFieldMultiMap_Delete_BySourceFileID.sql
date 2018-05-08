CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_Delete_BySourceFileID]
	@SourceFileID int
AS
BEGIN

	set nocount on

	DELETE TransformationFieldMultiMap WHERE SourceFileID = @SourceFileID

END