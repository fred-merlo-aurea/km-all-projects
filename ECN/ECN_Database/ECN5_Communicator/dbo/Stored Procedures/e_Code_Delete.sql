CREATE  PROC [dbo].[e_Code_Delete] 
(
	@UserID int,
    @CodeID int,
    @CustomerID int
)
AS 
BEGIN
	Update [Code] SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE CodeID = @CodeID AND CustomerID = @CustomerID
END