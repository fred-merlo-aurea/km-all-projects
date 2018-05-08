CREATE PROCEDURE o_CountForGeoCoding
AS
BEGIN

	SET NOCOUNT ON

	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = 'false'

END
GO