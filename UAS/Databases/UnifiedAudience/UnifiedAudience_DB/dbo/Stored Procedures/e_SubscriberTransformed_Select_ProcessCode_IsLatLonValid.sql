CREATE PROCEDURE e_SubscriberTransformed_Select_ProcessCode_IsLatLonValid
@ProcessCode varchar(50),
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE ProcessCode = @ProcessCode
	AND IsLatLonValid = @IsLatLonValid

END