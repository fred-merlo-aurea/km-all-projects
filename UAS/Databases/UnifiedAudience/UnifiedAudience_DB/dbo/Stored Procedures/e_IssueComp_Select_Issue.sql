CREATE PROCEDURE [dbo].[e_IssueComp_Select_Issue]
	@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM IssueComp With(NoLock)
	WHERE IssueId = @IssueID

END