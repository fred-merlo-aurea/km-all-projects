CREATE PROCEDURE e_HistoryMarketingMap_Select_SubscriberID
@SubscriberID int
AS
	SELECT *
	FROM HistoryMarketingMap With(NoLock)
	WHERE SubscriberID = @SubscriberID
