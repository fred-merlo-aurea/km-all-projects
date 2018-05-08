CREATE PROCEDURE e_PaidBillTo_Select_SubscriptionPaidID
@SubscriptionPaidID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM PaidBillTo With(NoLock)
	WHERE SubscriptionPaidID = @SubscriptionPaidID

END