CREATE PROCEDURE e_PaidBillTo_Select_SubscriptionID
@SubscriptionID int
AS
	SELECT *
	FROM PaidBillTo With(NoLock)
	WHERE SubscriptionID = @SubscriptionID
