create proc [dbo].[e_BrandDetails_Delete]
@BrandID int 
as
BEGIN

	set nocount on

	delete from BrandDetails where BrandID = @BrandID

End