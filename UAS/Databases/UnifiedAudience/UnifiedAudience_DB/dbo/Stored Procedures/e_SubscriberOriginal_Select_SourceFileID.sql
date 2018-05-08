CREATE PROCEDURE e_SubscriberOriginal_Select_SourceFileID
@SourceFileID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberOriginal With(NoLock)
	WHERE SourceFileID = @SourceFileID 
	ORDER BY ImportRowNumber

END