create proc [dbo].[e_UserBrand_Save](
@UserID int, 
@BrandID int
)
as
BEGIN
	
	SET NOCOUNT ON
	
    insert into UserBrand (UserID, BrandID) 
	values (@UserID, @BrandID)

End