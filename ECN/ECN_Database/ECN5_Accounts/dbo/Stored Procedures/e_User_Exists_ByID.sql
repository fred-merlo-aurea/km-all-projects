CREATE  PROC [dbo].[e_User_Exists_ByID] 
(
	@UserID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	if exists (select top 1 UserID from [Users] where CustomerID = @CustomerID and UserID = @UserID AND IsDeleted = 0) select 1 else select 0
END
