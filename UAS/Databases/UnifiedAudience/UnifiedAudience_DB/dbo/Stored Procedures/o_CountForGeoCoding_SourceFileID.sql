CREATE PROCEDURE o_CountForGeoCoding_SourceFileID
@SourceFileID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID AND IsLatLonValid = 'false'

END
GO