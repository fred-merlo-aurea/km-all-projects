CREATE PROCEDURE [dbo].[e_IssueArchiveProductSubscription_Select_Count]
@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	Select COUNT(*) 
	FROM IssueArchiveSubscription s with(nolock)
		JOIN IssueArchiveProductSubscription sp with(nolock) ON s.IssueArchiveSubscriptionId = sp.IssueArchiveSubscriptionId
	WHERE s.IssueId = @IssueID

END