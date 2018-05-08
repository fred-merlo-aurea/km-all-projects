CREATE PROCEDURE e_TransformAssign_SourceFileID
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT DISTINCT ta.*
	FROM TransformAssign ta With(NoLock)
	JOIN TransformationFieldMap fm With(NoLock) ON ta.TransformationID = fm.TransformationID
	WHERE fm.SourceFileID = @SourceFileID

END