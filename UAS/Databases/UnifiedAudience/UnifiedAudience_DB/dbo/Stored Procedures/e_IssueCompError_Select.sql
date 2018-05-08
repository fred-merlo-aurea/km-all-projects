CREATE PROCEDURE [dbo].[e_IssueCompError_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM IssueCompError With(NoLock)

END