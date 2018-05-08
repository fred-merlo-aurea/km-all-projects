create  PROC [dbo].[e_CustomerTemplate_Exists_ByID] 
(
	@CTID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 CTID from CustomerTemplate  with (nolock) where CTID = @CTID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
