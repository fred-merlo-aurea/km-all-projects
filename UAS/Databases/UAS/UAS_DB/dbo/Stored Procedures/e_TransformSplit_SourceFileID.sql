CREATE PROCEDURE e_TransformSplit_SourceFileID
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT DISTINCT ts.*
	FROM TransformSplit ts With(NoLock)
	JOIN TransformationFieldMap fm With(NoLock) ON ts.TransformationID = fm.TransformationID
	WHERE fm.SourceFileID = @SourceFileID
	
END