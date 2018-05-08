CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_Count]
@IssueID int
AS
	Select COUNT(*) FROM IssueArchiveSubscription sp
	JOIN IssueArchiveSubscriber s ON s.IssueArchiveSubscriberId = sp.IssueArchiveSubscriberId
	WHERE s.IssueId = @IssueID