CREATE PROCEDURE e_HistoryPaidBillTo_Select_SubscriptionID
@SubscriptionID int
AS
	SELECT *
	FROM HistoryPaidBillTo With(NoLock)
	WHERE SubscriptionID = @SubscriptionID
