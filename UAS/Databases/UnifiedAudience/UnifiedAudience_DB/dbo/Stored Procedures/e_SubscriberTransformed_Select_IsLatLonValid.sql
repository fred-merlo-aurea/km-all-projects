CREATE PROCEDURE e_SubscriberTransformed_Select_IsLatLonValid
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = @IsLatLonValid

END
GO