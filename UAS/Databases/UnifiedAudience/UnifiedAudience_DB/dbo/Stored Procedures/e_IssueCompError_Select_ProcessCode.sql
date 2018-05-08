CREATE PROCEDURE [dbo].[e_IssueCompError_Select_ProcessCode]
	@ProcessCode VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM IssueCompError With(NoLock) 
	WHERE ProcessCode = @ProcessCode

END