CREATE PROCEDURE e_HistoryPaid_Select_SubscriptionID
@SubscriptionID int
AS
	SELECT *
	FROM HistoryPaid With(NoLock)
	WHERE SubscriptionID = @SubscriptionID