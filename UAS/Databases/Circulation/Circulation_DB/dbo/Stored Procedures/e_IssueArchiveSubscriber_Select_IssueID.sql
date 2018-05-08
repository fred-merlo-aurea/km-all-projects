CREATE PROCEDURE [dbo].[e_IssueArchiveSubscriber_Select_IssueID]
@IssueID int
AS
	SELECT s.* FROM IssueArchiveSubscriber s With(NoLock)	
	WHERE s.IssueId = @IssueID