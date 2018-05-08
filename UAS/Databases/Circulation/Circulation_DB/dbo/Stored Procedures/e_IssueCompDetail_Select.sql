CREATE PROCEDURE [dbo].[e_IssueCompDetail_Select]
	@IssueCompID int
AS
	SELECT * FROM IssueCompDetail icd With(NoLock)
	JOIN IssueComp ic With(NoLock) ON ic.IssueCompId = icd.IssueCompID
	WHERE ic.IssueCompId = @IssueCompID
