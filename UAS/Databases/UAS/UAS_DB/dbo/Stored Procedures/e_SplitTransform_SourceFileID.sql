CREATE PROCEDURE [dbo].[e_SplitTransform_SourceFileID]
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT DISTINCT ts.*
	FROM TransformSplitTrans ts With(NoLock)
	JOIN TransformationFieldMap fm With(NoLock) ON ts.TransformationID = fm.TransformationID
	WHERE fm.SourceFileID = @SourceFileID

END
GO