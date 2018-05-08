CREATE PROCEDURE [dbo].[e_Subscriber_Select_ProductID_Count]
@ProductID int
AS
	Select COUNT(*) FROM Subscriber s
	JOIN Subscription sp ON s.SubscriberID = sp.SubscriberID
	WHERE sp.PublicationID = @ProductID
	