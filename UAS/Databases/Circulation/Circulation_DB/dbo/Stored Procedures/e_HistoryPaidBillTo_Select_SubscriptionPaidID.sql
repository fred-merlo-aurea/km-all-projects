CREATE PROCEDURE e_HistoryPaidBillTo_Select_SubscriptionPaidID
@SubscriptionPaidID int
AS
	SELECT *
	FROM HistoryPaidBillTo With(NoLock)
	WHERE SubscriptionPaidID = @SubscriptionPaidID
