CREATE PROCEDURE e_HistoryPaid_Select_SubscriptionID
@PubSubscriptionID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM HistoryPaid With(NoLock)
	WHERE PubSubscriptionID = @PubSubscriptionID

END