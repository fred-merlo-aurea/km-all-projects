CREATE PROCEDURE [dbo].[e_IssueCompError_Select]
AS
	SELECT * FROM IssueCompError With(NoLock)
