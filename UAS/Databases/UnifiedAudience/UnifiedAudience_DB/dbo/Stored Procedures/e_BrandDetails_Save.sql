CREATE proc [dbo].[e_BrandDetails_Save](
@BrandID int, 
@PubID int
)
as
BEGIN

	set nocount on

    insert into BrandDetails (BrandID, PubID) values (@BrandID, @PubID)
End