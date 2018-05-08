CREATE PROCEDURE e_SubscriberFinal_Select_IsLatLonValid
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberFinal With(NoLock)
	WHERE IsLatLonValid = @IsLatLonValid

END
GO