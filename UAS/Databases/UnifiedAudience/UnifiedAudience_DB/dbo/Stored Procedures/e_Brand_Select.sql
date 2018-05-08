create procedure e_Brand_Select
as
BEGIN

	set nocount on

	Select * from brand with (nolock) where IsDeleted = 0
	order by BrandName

END
go