CREATE PROCEDURE e_Subscriptions_UpdateLatLon
@SubscriptionID int,
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsLatLonValid bit,
@LatLonMsg nvarchar(500)
AS
BEGIN
	
	SET NOCOUNT ON
	
	UPDATE Subscriptions
		SET Latitude =@Latitude, Longitude = @Longitude, IsLatLonValid = @IsLatLonValid, LatLonMsg = @LatLonMsg
		WHERE SubscriptionID = @SubscriptionID

END