CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_IssueID]
@IssueID int
AS
	SELECT s.* FROM IssueArchiveSubscription s With(NoLock)	
	JOIN IssueArchiveSubscriber ias ON ias.IssueArchiveSubscriberId = s.IssueArchiveSubscriberId
	WHERE ias.IssueId = @IssueID