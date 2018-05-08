CREATE PROCEDURE [dbo].[e_ActionBackUp_Restore]
	@ProductID int
AS
BEGIN

	set nocount on

	UPDATE PubSubscriptions
	SET PubCategoryID = a.PubCategoryID, PubTransactionID = a.PubTransactionID
	FROM PubSubscriptions s
	INNER JOIN ActionBackUp a
	ON a.PubSubscriptionID = s.SubscriptionID
	WHERE s.PubID = @ProductID

END