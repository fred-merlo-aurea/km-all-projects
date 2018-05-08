CREATE proc [dbo].[e_Brand_Delete]
@BrandID int,
@UserID int
as
BEGIN

	set nocount on

	Update Brand set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID  where BrandID = @BrandID
End