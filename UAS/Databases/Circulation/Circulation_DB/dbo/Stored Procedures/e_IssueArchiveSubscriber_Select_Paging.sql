CREATE PROCEDURE [dbo].[e_IssueArchiveSubscriber_Select_Paging]
@CurrentPage int,
@PageSize int,
@IssueID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY s.IssueArchiveSubscriberID) as 'RowNum', s.* FROM
	IssueArchiveSubscriber s WHERE s.IssueId = @IssueID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec