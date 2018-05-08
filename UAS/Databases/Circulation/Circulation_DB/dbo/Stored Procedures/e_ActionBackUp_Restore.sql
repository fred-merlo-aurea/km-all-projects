CREATE PROCEDURE [dbo].[e_ActionBackUp_Restore]
	@ProductID int
AS
	UPDATE Subscription
	SET ActionID_Current = a.ActionID_Current, ActionID_Previous = a.ActionID_Previous
	FROM Subscription s
	INNER JOIN ActionBackUp a
	ON a.SubscriptionID = s.SubscriptionID
	WHERE s.PublicationID = @ProductID
