CREATE PROCEDURE [dbo].[e_FpsMap_Delete_SourceFileByID]
	@SourceFileID int
AS
BEGIN

	set nocount on

	DELETE FpsMap 
	WHERE SourceFileID = @SourceFileID

END