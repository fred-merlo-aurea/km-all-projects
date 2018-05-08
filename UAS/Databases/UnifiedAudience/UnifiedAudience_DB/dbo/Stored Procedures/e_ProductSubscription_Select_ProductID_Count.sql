CREATE PROCEDURE [dbo].[e_ProductSubscription_Select_ProductID_Count]
@ProductID int
AS
BEGIN

	SET NOCOUNT ON

	Select COUNT(*) 
	FROM PubSubscriptions  WITH (NOLOCK)
	WHERE PubID = @ProductID

END	
GO