CREATE  PROC [dbo].[e_LandingPage_Exists_ByLPID] 
(
	@LPID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 LPID from LandingPage  with (nolock) where LPID = @LPID AND IsActive = 1) SELECT 1 ELSE SELECT 0
END