create procedure e_Market_Select
as
BEGIN

	SET NOCOUNT ON

	Select m.*
	from Markets m 
	left join brand b on m.brandID = b.brandID 
	where isnull(b.IsDeleted,0) = 0 
	order by Marketname asc

END
go