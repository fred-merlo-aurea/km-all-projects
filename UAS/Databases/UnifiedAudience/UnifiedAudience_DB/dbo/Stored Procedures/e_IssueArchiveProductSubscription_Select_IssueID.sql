CREATE PROCEDURE [dbo].[e_IssueArchiveProductSubscription_Select_IssueID]
@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT s.* 
	FROM IssueArchiveSubscription s With(NoLock)	
		JOIN IssueArchiveProductSubscription ias with(nolock) ON ias.IssueArchiveSubscriptionId = s.IssueArchiveSubscriptionId
	WHERE s.IssueId = @IssueID

END