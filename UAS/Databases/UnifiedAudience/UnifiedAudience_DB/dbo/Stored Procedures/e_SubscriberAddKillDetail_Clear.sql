CREATE PROCEDURE e_SubscriberAddKillDetail_Clear
	@ProductID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE s
	FROM SubscriberAddKillDetail s
		JOIN PubSubscriptions p ON p.PubsubscriptionID = s.PubsubscriptionID
	WHERE p.PubID = @ProductID and s.AddKillID = 0


END