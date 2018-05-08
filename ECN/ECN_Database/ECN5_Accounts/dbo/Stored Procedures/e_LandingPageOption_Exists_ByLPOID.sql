CREATE  PROC [dbo].[e_LandingPageOption_Exists_ByLPOID] 
(
	@LPOID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 LPOID from LandingPageOption  with (nolock) where LPOID = @LPOID AND IsActive = 1) SELECT 1 ELSE SELECT 0
END