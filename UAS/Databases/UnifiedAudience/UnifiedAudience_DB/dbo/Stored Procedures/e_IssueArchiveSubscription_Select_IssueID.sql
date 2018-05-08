CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_IssueID]
@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT s.* 
	FROM IssueArchiveSubscription s With(NoLock)	
	WHERE s.IssueId = @IssueID

END