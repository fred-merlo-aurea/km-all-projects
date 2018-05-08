create procedure e_Bundle_Select_Name_BundleId
@bundleId int
as
BEGIN

	set nocount on

	select *
	from Bundle with(nolock)
	where bundle_id = @bundleId

END
go