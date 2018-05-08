CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
		SELECT DISTINCT ROW_NUMBER() 
		OVER (ORDER BY srm.SubscriptionID) as 'RowNum', srm.* 
		FROM SubscriptionResponseMap srm  with(nolock)
			JOIN PubSubscriptions s  with(nolock) ON srm.SubscriptionID = s.SubscriptionID
		WHERE s.PubID = @ProductID)

	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
		AND RowNum < @LastRec

END