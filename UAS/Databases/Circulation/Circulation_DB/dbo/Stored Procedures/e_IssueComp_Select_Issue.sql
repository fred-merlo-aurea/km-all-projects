CREATE PROCEDURE [dbo].[e_IssueComp_Select_Issue]
	@IssueID int
AS
	SELECT * FROM IssueComp With(NoLock)
	WHERE IssueId = @IssueID
