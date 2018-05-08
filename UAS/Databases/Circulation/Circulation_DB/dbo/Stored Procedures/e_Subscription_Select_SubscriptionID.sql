CREATE PROCEDURE e_Subscription_Select_SubscriptionID
@SubscriptionID int
AS
	SELECT * FROM Subscription With(NoLock) WHERE SubscriptionID = @SubscriptionID
