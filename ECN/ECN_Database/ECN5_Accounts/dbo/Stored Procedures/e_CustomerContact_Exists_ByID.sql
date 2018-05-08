create  PROC [dbo].[e_CustomerContact_Exists_ByID] 
(
	@ContactID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 ContactID from CustomerContact  with (nolock) where ContactID = @ContactID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
