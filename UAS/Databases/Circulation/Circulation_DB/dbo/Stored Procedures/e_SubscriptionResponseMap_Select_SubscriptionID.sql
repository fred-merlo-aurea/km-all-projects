CREATE PROCEDURE e_SubscriptionResponseMap_Select_SubscriptionID
@SubscriptionID int
AS
	SELECT * FROM SubscriptionResponseMap With(NoLock) WHERE SubscriptionID = @SubscriptionID
