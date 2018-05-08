CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Select_SourceFileID]
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformationFieldMap With(NoLock)
	Where IsActive = 'true' and SourceFileID = @SourceFileID

END