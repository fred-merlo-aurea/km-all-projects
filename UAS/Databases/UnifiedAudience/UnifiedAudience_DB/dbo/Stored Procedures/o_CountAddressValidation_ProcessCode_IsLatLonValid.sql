CREATE PROCEDURE o_CountAddressValidation_ProcessCode_IsLatLonValid
@ProcessCode varchar(50),
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE ProcessCode = @ProcessCode
	AND IsLatLonValid = @IsLatLonValid

END
GO