
CREATE PROCEDURE dbo.[e_Issue_ValidateArchive]
@issueid INT,
@pubid INT
AS
BEGIN

SET NOCOUNT ON

DECLARE 
@issueArchiveSubCount INT,
@issueArchiveDetailCount INT,
@issueArchiveExtCount INT,
@pubSubCount INT,
@pubDetailCount INT,
@pubExtCount INT,
@return INT = 1


SELECT @issueArchiveSubCount = COUNT(*) 
FROM IssueArchiveProductSubscription WITH(NOLOCK)
WHERE IssueID = @issueId

SELECT @issueArchiveDetailCount = COUNT(*) 
FROM IssueArchiveProductSubscription iaps WITH(NOLOCK)
	JOIN IssueArchiveProductSubscriptionDetail iapsd WITH(NOLOCK) ON iaps.IssueArchiveSubscriptionId = iapsd.IssueArchiveSubscriptionId
WHERE IssueID = @issueId

SELECT @issueArchiveExtCount = COUNT(*) 
FROM IssueArchiveProductSubscription iaps WITH(NOLOCK)
	JOIN IssueArchivePubSubscriptionsExtension iapse WITH(NOLOCK) ON iaps.IssueArchiveSubscriptionId = iapse.IssueArchiveSubscriptionId
WHERE IssueID = @issueId

SELECT @pubSubCount = COUNT(*) 
FROM PubSubscriptions WITH(NOLOCK)
WHERE pubid = @pubid

SELECT @pubDetailCount = COUNT(*)
FROM PubSubscriptions ps WITH(NOLOCK) 
	JOIN PubSubscriptionDetail psd WITH(NOLOCK) ON ps.PubSubscriptionID = psd.PubSubscriptionID
WHERE ps.PubID = @pubid

SELECT @pubExtCount = COUNT(*)
FROM PubSubscriptions ps WITH(NOLOCK) 
	JOIN PubSubscriptionsExtension pse WITH(NOLOCK) ON ps.PubSubscriptionID = pse.PubSubscriptionID
WHERE ps.PubID = @pubid

IF (@issueArchiveSubCount != @pubSubCount or @issueArchiveDetailCount != @pubDetailCount or @issueArchiveExtCount != @pubExtCount)
BEGIN
	SET @return = 0
END

SELECT @return

END