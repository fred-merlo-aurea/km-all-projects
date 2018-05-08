CREATE PROCEDURE [dbo].[e_IssueSplit_Select_IssueID]
	@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM IssueSplit i With(NoLock)
	WHERE i.IssueId = @IssueID

END