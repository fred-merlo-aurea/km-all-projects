
CREATE PROCEDURE dbo.[e_Issue_Rollback]
		@imbSeq INT,
		@pubid INT,
		@issueid INT
AS
BEGIN
	
DECLARE @acsMailerId INT

SELECT @acsMailerId = AcsMailerInfoID from Pubs WHERE pubid = @pubid

DELETE IssueArchiveProductSubscriptionDetail 
WHERE IssueArchiveSubscriptionId IN (SELECT IssueArchiveSubscriptionId from IssueArchiveProductSubscription WHERE IssueID = @issueid)

DELETE IssueArchivePubSubscriptionsExtension 
WHERE IssueArchiveSubscriptionId IN (SELECT IssueArchiveSubscriptionId from IssueArchiveProductSubscription WHERE IssueID = @issueid)
			
DELETE IssueArchiveProductSubscription WHERE issueid = @issueid

DELETE issuesplit WHERE issueid = @issueid

UPDATE AcsMailerInfo SET ImbSeqCounter = @imbSeq WHERE AcsMailerInfoId = @acsMailerId


SELECT 1

END