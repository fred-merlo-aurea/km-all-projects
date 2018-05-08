CREATE PROCEDURE e_SubscriptionResponseMap_Select_SubscriberID
@SubscriberID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT rm.* 
	FROM SubscriptionResponseMap rm With(NoLock) 
		JOIN Subscription s With(NoLock) ON s.SubscriptionID = rm.SubscriptionID
	WHERE s.SubscriberID = @SubscriberID

END