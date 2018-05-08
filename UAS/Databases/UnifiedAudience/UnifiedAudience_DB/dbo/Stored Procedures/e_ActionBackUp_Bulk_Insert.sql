CREATE PROCEDURE [dbo].[e_ActionBackUp_Bulk_Insert]
	@ProductID int
AS
BEGIN

	set nocount on

	DELETE FROM ActionBackUp
	WHERE ProductID = @ProductID

	INSERT INTO ActionBackUp
	SELECT PubID, PubSubscriptionID, PubCategoryID, PubTransactionID
	FROM PubSubscriptions s
	WHERE s.PubID = @ProductID and PubTransactionID is not null

END