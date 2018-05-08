CREATE PROCEDURE e_HistoryMarketingMap_Select_PubSubscriptionID
@PubSubscriptionID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM HistoryMarketingMap With(NoLock)
	WHERE PubSubscriptionID = @PubSubscriptionID

END