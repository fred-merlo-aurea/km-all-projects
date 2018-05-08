CREATE PROCEDURE e_Subscription_Select_SubscriberID
@SubscriberID int
AS
	SELECT * FROM Subscription With(NoLock) WHERE SubscriberID = @SubscriberID
