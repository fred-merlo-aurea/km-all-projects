CREATE PROCEDURE e_HistoryPaidBillTo_Select_SubscriptionPaidID
@SubscriptionPaidID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM HistoryPaidBillTo With(NoLock)
	WHERE SubscriptionPaidID = @SubscriptionPaidID

END