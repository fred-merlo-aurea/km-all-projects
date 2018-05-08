CREATE PROCEDURE [dbo].[e_IssueCompDetail_Clear]
@IssueCompID int
AS
DELETE FROM IssueCompDetail
WHERE IssueCompID = @IssueCompID