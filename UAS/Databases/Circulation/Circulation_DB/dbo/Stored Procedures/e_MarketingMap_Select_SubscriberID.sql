CREATE PROCEDURE e_MarketingMap_Select_SubscriberID
@SubscriberID int
AS
	SELECT * FROM MarketingMap With(NoLock) WHERE SubscriberID = @SubscriberID
