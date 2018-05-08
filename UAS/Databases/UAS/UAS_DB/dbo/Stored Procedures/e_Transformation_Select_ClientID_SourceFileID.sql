CREATE PROCEDURE e_Transformation_Select_ClientID_SourceFileID 
@ClientID int,
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT t.* 
	FROM Transformation t With(NoLock)
	JOIN TransformationFieldMap tfm With(NoLock) ON t.TransformationID = tfm.TransformationID
	WHERE t.ClientID = @ClientID AND tfm.SourceFileID = @SourceFileID and t.IsActive = 'true' and tfm.IsActive = 'true'

END