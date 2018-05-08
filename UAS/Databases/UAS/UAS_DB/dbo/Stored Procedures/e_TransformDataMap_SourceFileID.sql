CREATE PROCEDURE e_TransformDataMap_SourceFileID
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT DISTINCT tdm.*
	FROM TransformDataMap tdm With(NoLock)
	JOIN TransformationFieldMap fm With(NoLock) ON tdm.TransformationID = fm.TransformationID
	WHERE fm.SourceFileID = @SourceFileID

END