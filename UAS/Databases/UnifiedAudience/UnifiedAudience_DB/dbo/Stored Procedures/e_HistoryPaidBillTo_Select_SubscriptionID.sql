CREATE PROCEDURE e_HistoryPaidBillTo_Select_SubscriptionID
@PubSubscriptionID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM HistoryPaidBillTo With(NoLock)
	WHERE PubSubscriptionID = @PubSubscriptionID

END