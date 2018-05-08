CREATE PROCEDURE e_TransformJoin_SourceFileID
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT DISTINCT tj.*
	FROM TransformJoin tj With(NoLock)
	JOIN TransformationFieldMap fm With(NoLock) ON tj.TransformationID = fm.TransformationID
	WHERE fm.SourceFileID = @SourceFileID

END