CREATE PROCEDURE [dbo].[e_Subscription_Select_ProductID_Count]
@ProductID int
AS
	Select COUNT(*) FROM Subscription
	WHERE PublicationID = @ProductID
	
GO