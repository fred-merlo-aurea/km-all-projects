CREATE PROCEDURE [dbo].[e_ActionBackUp_Bulk_Insert]
	@ProductID int
AS
	DELETE FROM ActionBackUp
	WHERE ProductID = @ProductID

	INSERT INTO ActionBackUp
	SELECT PublicationID, SubscriptionID, ActionID_Current, ActionID_Previous
	FROM Subscription s
	WHERE s.PublicationID = @ProductID
