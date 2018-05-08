CREATE PROCEDURE o_CountAddressValidation_IsLatLonValid
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = @IsLatLonValid

END
GO