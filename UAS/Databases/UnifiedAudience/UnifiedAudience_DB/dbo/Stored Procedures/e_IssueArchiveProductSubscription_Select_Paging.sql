CREATE PROCEDURE [dbo].[e_IssueArchiveProductSubscription_Select_Paging]
@CurrentPage int,
@PageSize int,
@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY s.IssueArchiveSubscriptionID) as 'RowNum', s.* 
	FROM IssueArchiveSubscription s with(nolock)
		JOIN IssueArchiveProductSubscription ias with(nolock) ON ias.IssueArchiveSubscriptionId = s.IssueArchiveSubscriptionId
	WHERE s.IssueId = @IssueID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec

END