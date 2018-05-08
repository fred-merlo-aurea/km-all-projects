CREATE  PROC [dbo].[e_User_Exists_ByUserName] 
(
	@UserID int,
	@CustomerID int,
	@UserName varchar(100)
)
AS 
BEGIN
	if exists (select top 1 UserID from [Users] where CustomerID = @CustomerID and UserID <> @UserID and username = @username AND IsDeleted = 0) select 1 else select 0
END
