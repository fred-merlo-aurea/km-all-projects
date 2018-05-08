CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Delete_SourceFileID]
@SourceFileID int
AS
BEGIN

	set nocount on

	DELETE FROM TransformationFieldMap
	WHERE SourceFileID = @SourceFileID
	Select @SourceFileID;

END