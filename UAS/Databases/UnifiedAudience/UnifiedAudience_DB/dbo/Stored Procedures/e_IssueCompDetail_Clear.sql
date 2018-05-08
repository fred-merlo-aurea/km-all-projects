CREATE PROCEDURE [dbo].[e_IssueCompDetail_Clear]
@IssueCompID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE FROM IssueCompDetail
	WHERE IssueCompID = @IssueCompID

END