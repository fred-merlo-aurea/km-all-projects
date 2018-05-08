CREATE PROCEDURE [dbo].[e_Subscriber_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY s.SubscriberID) as 'RowNum', s.* FROM
	Subscriber s JOIN Subscription sp ON s.SubscriberID = sp.SubscriberID WHERE sp.PublicationID = @ProductID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec