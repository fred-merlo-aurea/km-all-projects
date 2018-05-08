create procedure e_PaidOrderDetail_Select_SubGenBundleId
@SubGenBundleId int
as
BEGIN

	set nocount on

	select *
	from PaidOrderDetail with(nolock)
	where SubGenBundleId = @SubGenBundleId

END
go