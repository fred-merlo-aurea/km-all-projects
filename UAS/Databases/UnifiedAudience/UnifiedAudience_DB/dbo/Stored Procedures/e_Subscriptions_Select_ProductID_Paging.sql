CREATE PROCEDURE [dbo].[e_Subscriptions_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as 
		(SELECT DISTINCT ROW_NUMBER() 
		OVER (ORDER BY s.SubscriptionID) as 'RowNum', s.* 
		FROM Subscriptions s with(nolock) 
			JOIN PubSubscriptions sp with(nolock) ON s.SubscriptionID = sp.SubscriptionID 
		WHERE sp.PubID = @ProductID)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
		AND RowNum < @LastRec

END