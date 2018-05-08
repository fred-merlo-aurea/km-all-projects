CREATE proc [dbo].[e_Filters_Delete](
@FilterID int,
@UserID int
)
as
BEGIN

	SET NOCOUNT ON

	update Filters 
	set IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate=GETDATE() 
	where FilterID = @FilterID

End