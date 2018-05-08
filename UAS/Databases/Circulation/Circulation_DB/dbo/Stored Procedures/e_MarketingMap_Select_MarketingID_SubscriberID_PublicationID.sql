
CREATE PROCEDURE e_MarketingMap_Select_MarketingID_SubscriberID_PublicationID
@MarketingID int,
@SubscriberID int,
@PublicationID int
AS
	SELECT * FROM MarketingMap With(NoLock) WHERE MarketingID = @MarketingID AND SubscriberID = @SubscriberID AND PublicationID = @PublicationID
