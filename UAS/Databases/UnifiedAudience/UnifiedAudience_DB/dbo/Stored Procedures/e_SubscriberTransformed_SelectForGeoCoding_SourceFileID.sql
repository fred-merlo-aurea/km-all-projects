CREATE PROCEDURE e_SubscriberTransformed_SelectForGeoCoding_SourceFileID
@SourceFileID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = 'false'
	AND LatLonMsg not like 'Invalid from MapPoint%'
	AND SourceFileID = @SourceFileID 

END
GO