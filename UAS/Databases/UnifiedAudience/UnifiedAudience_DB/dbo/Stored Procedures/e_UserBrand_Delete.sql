create proc [dbo].[e_UserBrand_Delete](
@UserID int
)
as
BEGIN
	
	SET NOCOUNT ON
	
	delete 
	from UserBrand 
	where UserID = @UserID

End