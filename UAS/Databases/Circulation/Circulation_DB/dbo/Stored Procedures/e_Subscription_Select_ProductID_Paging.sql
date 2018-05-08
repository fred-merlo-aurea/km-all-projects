CREATE PROCEDURE [dbo].[e_Subscription_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY SubscriptionID) as 'RowNum', * FROM
	Subscription WHERE PublicationID = @ProductID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
	
GO