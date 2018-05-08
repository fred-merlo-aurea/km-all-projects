CREATE PROCEDURE [dbo].[e_IssueArchiveSubscriber_Select_Count]
@IssueID int
AS
	Select COUNT(*) FROM IssueArchiveSubscriber s
	WHERE s.IssueId = @IssueID
