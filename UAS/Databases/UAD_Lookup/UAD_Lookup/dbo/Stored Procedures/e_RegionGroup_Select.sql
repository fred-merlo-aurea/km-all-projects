create procedure e_RegionGroup_Select
as
BEGIN

	set nocount on

	select *
	from RegionGroup with(nolock)
	order by Sortorder 

END
go