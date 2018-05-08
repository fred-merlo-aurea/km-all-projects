CREATE PROCEDURE [e_Subscription_Select_SubscriptionID]
@SubscriptionID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT *
	FROM Subscriptions With(NoLock)
	WHERE SubscriptionID = @SubscriptionID

END