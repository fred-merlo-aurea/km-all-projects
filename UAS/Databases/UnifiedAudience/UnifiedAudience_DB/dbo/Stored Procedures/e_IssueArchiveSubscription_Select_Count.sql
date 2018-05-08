CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_Count]
@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	Select COUNT(*) 
	FROM IssueArchiveSubscription s with(nolock)
	WHERE s.IssueId = @IssueID

END