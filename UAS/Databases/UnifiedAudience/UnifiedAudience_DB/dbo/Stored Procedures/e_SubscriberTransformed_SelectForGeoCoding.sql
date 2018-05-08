CREATE PROCEDURE e_SubscriberTransformed_SelectForGeoCoding
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = 'false' AND LatLonMsg not like 'Invalid from MapPoint%'

END
GO