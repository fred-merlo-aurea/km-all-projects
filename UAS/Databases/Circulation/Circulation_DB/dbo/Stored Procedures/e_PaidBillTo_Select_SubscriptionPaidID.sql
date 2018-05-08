CREATE PROCEDURE e_PaidBillTo_Select_SubscriptionPaidID
@SubscriptionPaidID int
AS
	SELECT *
	FROM PaidBillTo With(NoLock)
	WHERE SubscriptionPaidID = @SubscriptionPaidID
