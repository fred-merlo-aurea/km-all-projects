create procedure e_PaidOrder_Select_SubGenSubscriberId
@SubGenSubscriberId int
as
BEGIN

	set nocount on

	select *
	from PaidOrder with(nolock)
	where SubGenSubscriberId = @SubGenSubscriberId

END
go