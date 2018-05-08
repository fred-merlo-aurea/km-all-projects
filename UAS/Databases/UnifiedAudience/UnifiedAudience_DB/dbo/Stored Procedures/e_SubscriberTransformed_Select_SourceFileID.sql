CREATE PROCEDURE [e_SubscriberTransformed_Select_SourceFileID]
@SourceFileID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID 

END